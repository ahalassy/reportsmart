/*
 *
 * Licensing:			GPL
 * Original project:	RsCore.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.08.)
 * 
 * 
 */
using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ReportSmart.Documents.Collections {
		public class CReportItem: IComparable {
				private string _Name;
				protected string _Type;
				private TreeNode _Node;
				private CReportItem _Parent;
				private RsReportCollection _Collection;
				private bool _Killed;
				
				public CReportFolder Parent {
						get { return (CReportFolder)_Parent; }
						set {
								if (_Parent != null)
										((CReportFolder)_Parent).Remove(this);
																			
								setParentDirectly(value);
								
								if (_Parent != null)
										((CReportFolder)_Parent).Add(this);
							}
					}
				
				public bool Killed { get { return _Killed; }}
				
				public virtual RsReportCollection Collection { get { return _Collection; }}
				
				protected virtual void edit() {
						if (_Collection != null)
								_Collection.setModified(true);
					}
				
				protected virtual void recreateNode() {
						_Node = new TreeNode();
						_Node.Text = _Name;
						if (_Parent.GUINode != null)
								_Parent.GUINode.Nodes.Add(_Node);
	
						_Node.ImageIndex = 0;
						_Node.SelectedImageIndex = 0;
					}
				
				protected virtual void setParentDirectly(CReportItem aParent) {
						_Parent = aParent;
						if (aParent != null)
								SetCollection(aParent.Collection);
							else
								SetCollection(null);
					}
					
				protected RsReportCollection GetCollection() { 
						return _Collection;
					}
					
				protected virtual void SetCollection(RsReportCollection aCollection) {
						RsReportCollection lLastCollection = _Collection;
						_Collection = aCollection;
						edit();
						if (lLastCollection != null)
								lLastCollection.setModified(true);
					}
					
				protected virtual void setItemName(string aName) {
						_Name = aName;
						_Node.Text = _Name;
					}		
				
				public void Orphanize(CReportFolder aNewParent) {
				        setParentDirectly(aNewParent);
				    }
				
				public TreeNode GUINode {
						get { return _Node; }
					}
	
				public string ItemName {
						get { return _Name; }
						set { 
								setItemName(value);
								if (_Collection != null)
										_Collection.setModified(true);
							}
					}
					
				public virtual void Release() {
				        if (_Killed) return;
							else _Killed = true;
							
						if (_Parent != null)
								((CReportFolder)_Parent).Remove(this);
								
						_Node.Remove();
						_Node = null;
											
					}
					
				public int CompareTo(object aObject) {
						if (aObject is CReportItem)
								return _Name.CompareTo(((CReportItem)aObject).ItemName);
							else
								return 1;
					}
					
				public void SetCollection() {
						if (_Parent != null)
								SetCollection(_Parent.Collection);
					}
					
				public virtual XmlNode BuildXML(XmlNode aParent, XmlDocument aDoc) {
				        if (_Killed) return null;
				    
						XmlNode lResult = aDoc.CreateNode("element", _Type, "");
						XmlAttribute lName = aDoc.CreateAttribute("name");
						lName.Value = this.ItemName;
						lResult.Attributes.Append(lName);
						aParent.AppendChild(lResult);
						return lResult;
					}
					
				public CReportItem(string aName) {
						_Name = aName;
						_Node = new TreeNode(aName);
						_Node.Tag = this;
						_Type = "item";
						_Parent = null;
						_Killed = false;
					}
			}
}
