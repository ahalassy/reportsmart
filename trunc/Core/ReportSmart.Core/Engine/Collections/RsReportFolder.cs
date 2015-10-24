/*
 *
 * Licensing:			GPL
 * Original project:	RsCore.csproj
 *
 * Copyright: Adam Halassy (2010.12.08.)
 * 
 * 
 */
using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ReportSmart.Engine.Collections {
		public class CReportFolder: CReportItem {
				private ArrayList _SubItems;
				private bool _Expanded;
				
				public bool GUIExpanded {
						get { return _Expanded; }
						set { 
								_Expanded = value;
								if ( GUINode != null)
										if (_Expanded)
												GUINode.Expand();
											else
												GUINode.Collapse();
							}
					}				
		
				public CReportItem this[int iIndex] {
						get {
								if (iIndex < _SubItems.Count && iIndex > -1)
										return (CReportItem)_SubItems[iIndex];
									else
										return null;
							}
					}
					
				protected override void SetCollection(RsReportCollection aCollection) {
						base.SetCollection(aCollection);
						foreach (object iItem in _SubItems)
								((CReportItem)iItem).SetCollection();
					}
					
				public void GUIRefresh(TreeNode aParent) {
						if (Killed) return;
				
						if (aParent != null)
								if (aParent.Nodes.IndexOf(this.GUINode) < 0) {
										aParent.Nodes.Add(this.GUINode);
									}
						Sort();
						
						for (int i = 0; i < _SubItems.Count; i++) {
								if (this[i] is CReportFolder)
										((CReportFolder)this[i]).GUIRefresh(this.GUINode);
							}
							
						for (int i = 0; i < _SubItems.Count; i++) {
								if (this[i] is CReportFile && GUINode.Nodes.IndexOf(this[i].GUINode) < 0)
										GUINode.Nodes.Add(this[i].GUINode);
							}
						
						if (GUIExpanded)
								GUINode.Expand();
							else
								GUINode.Collapse();
					}
					
				public int ItemCount {
						get { return _SubItems.Count; }
					}
					
				public bool hasChildren {
						get { return ItemCount > 0; }
					}
							
				public override XmlNode BuildXML(XmlNode aParent, XmlDocument aDoc) {
						XmlNode lResult = base.BuildXML(aParent, aDoc);
						if (lResult == null) return null;
						
						for (int i = 0; i < ItemCount; i++) {
								if (this[i] is CReportFolder) {
										this[i].BuildXML(lResult, aDoc);
									}
							}
					
						for (int i = 0; i < ItemCount; i++) {
								if (this[i] is CReportFile) {
										this[i].BuildXML(lResult, aDoc);
									}
							}
						return lResult;
					}
					
				public void BuildFromXML(XmlNode aSource) {			
						this.ItemName = aSource.Attributes["name"].Value;
						for (int i = 0; i < aSource.ChildNodes.Count; i++) {
								CReportItem lNewItem = RsReportCollection.CreateItemFromXMLNode(aSource.ChildNodes[i]);
								lNewItem.Parent = this;
								if (lNewItem is CReportFolder)
										((CReportFolder)lNewItem).BuildFromXML(aSource.ChildNodes[i]);
							}
					}
				
				public CReportFolder(string aName): base(aName) {
						_SubItems = new ArrayList();
						_Expanded = false;
						_Type = "folder";
						
						GUINode.SelectedImageIndex = 0;
						GUINode.ImageIndex = 0;
					}
									
				public void Sort() { _SubItems.Sort(); }
					
				public CReportItem Add(CReportItem aItem) {
						_Expanded = true;
						if (_SubItems.IndexOf(aItem) < 0) {
								_SubItems.Add(aItem);
								setParentDirectly(this);
							}
						return aItem;
					}
					
				public CReportItem Remove(CReportItem aItem) {
				        int lItemId = _SubItems.IndexOf(aItem);
				
						if (lItemId > -1) {
								_SubItems.RemoveAt(lItemId);
								
							}
							
						aItem.Orphanize(null);
						return aItem;				
					}
				
				public CReportItem Remove(int aIndex) {
						CReportItem lItem = this[aIndex];
						this.Remove(lItem);
						return lItem;
					}
					
				public void RemoveAll() {
						foreach (object lItem in _SubItems) {
								if (lItem is CReportFolder)
									((CReportFolder)lItem).RemoveAll();
							}
						_SubItems.Clear();
					}
					
				public override void Release() {
						for (int i = ItemCount - 1; i > -1; i--) {
								if (this[i] is CReportFolder)
										((CReportFolder)this[i]).Release();
							}
						_SubItems.Clear();
						base.Release();
					}
			}
	}
