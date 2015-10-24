/*
 * 2009.09.08. (Adam Halassy)
 * 
 */

using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ReportSmart.Collection {

	internal class CReportItem: IComparable {
			private string _Name;
			protected string _Type;
			private TreeNode _Node;
			private CReportItem _Parent;
			private CReportCollection _Collection;
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
			
			public virtual CReportCollection Collection { get { return _Collection; }}
			
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
				
			protected CReportCollection GetCollection() { 
					return _Collection;
				}
				
			protected virtual void SetCollection(CReportCollection aCollection) {
					CReportCollection lLastCollection = _Collection;
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

	internal class CReportFile: CReportItem {
			private string _File;
			
			public string ReportFile {
					get { return _File; }
					set {
							_File = value;
							edit();
						}
				}
				
			public override XmlNode BuildXML(XmlNode aParent, XmlDocument aDoc) {
					XmlNode lResult = base.BuildXML(aParent, aDoc);
					if (lResult == null) return null;
					XmlAttribute lPath = aDoc.CreateAttribute("path");
					lPath.Value = _File;
					lResult.Attributes.Append(lPath);
					return lResult;
				}
				
			public CReportFile(string aName, string aFile): base(aName) {
					_File = aFile;
					_Type = "file";
					GUINode.SelectedImageIndex = 1;
					GUINode.ImageIndex = 1;
				}
		}
		
	internal class CReportFolder: CReportItem {
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
				
			protected override void SetCollection(CReportCollection aCollection) {
					base.SetCollection(aCollection);
					foreach (object iItem in _SubItems)
							((CReportItem)iItem).SetCollection();
				}
				
			public void GUIRefresh(TreeNode aParent) {
					if (Killed) return;
			
					if (aParent != null)
							if (aParent.Nodes.IndexOf(this.GUINode) < 0) {
							        if (this.GUINode == null)
							                ReportSmart.Controls.CRSMessageBox.ShowBox("Meggyilkolt újjáélesztése?");
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
							CReportItem lNewItem = CReportCollection.CreateItemFromXMLNode(aSource.ChildNodes[i]);
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
		
	internal class CReportRootFolder: CReportFolder {
			public new CReportCollection Collection {
					get { return GetCollection(); }
					set { SetCollection(value); }
				}
	
			public CReportRootFolder(string aName, CReportCollection aCollection): base(aName) {
					GUINode.ImageIndex = 3;
					GUINode.SelectedImageIndex = 3;
			
					SetCollection(aCollection);
				}
		}
		
	internal class CReportCollection {
			public const string XMLNODE_ITEM = "item";
			public const string XMLNODE_FOLDER = "folder";
			public const string XMLNODE_REPORT = "file";
			// public const string XMLNODE_LINK = "link"; // Later version"s will support link collections
			
			public static CReportItem CreateItemFromXMLNode(XmlNode aXmlNode) {
					CReportItem lResult = null;
					
					if (aXmlNode.Name == XMLNODE_FOLDER) {
							lResult = new CReportFolder(aXmlNode.Attributes["name"].Value);
						} else
					if (aXmlNode.Name == XMLNODE_REPORT) {
							lResult = new CReportFile(aXmlNode.Attributes["name"].Value, aXmlNode.Attributes["path"].Value);
						} else
					lResult = new CReportItem(aXmlNode.Attributes["name"].Value);
					
					return lResult;
				}
		
			private TreeView _ctlGUITreeView;
			private CReportRootFolder _RootFolder;
			private string _LastFile;
			private bool _Killed;
			public bool _Modified;
			
			public bool Modified {
					get { return _Modified; }
				}
			
			public string CollectionName {
					get { return RootFolder.ItemName; }
					set { 
							RootFolder.ItemName = value;
							_Modified = true;
						}
				}
			
			public bool Killed { get { return _Killed; }}
		
			public string FileName {
					get { return _LastFile; }
					set { _LastFile = value; }			
				}
				
			public CReportRootFolder RootFolder {
					get { return getRootFolder(); }
					//set { 
					//		_Root = value;
					//		ApplyToGUI();
					//	}
				}
				
			public TreeView GUITreeView {
					get { return _ctlGUITreeView; }
					set {
							_ctlGUITreeView = value;
							if (_ctlGUITreeView != null)
									if (_ctlGUITreeView.Nodes.IndexOf(RootFolder.GUINode) < 0)
											_ctlGUITreeView.Nodes.Add(RootFolder.GUINode);
							RootFolder.GUIRefresh(null);
						}
				}
			
			protected internal void setModified(bool aSet) {
					_Modified = aSet;
				}
			
			protected virtual CReportRootFolder getRootFolder() {
					return _RootFolder;
				}
				
			protected CReportRootFolder setRootFolder(CReportRootFolder aFolder) {
					CReportRootFolder lResult = _RootFolder;
					_RootFolder = aFolder;
					_RootFolder.Collection = this;
					ApplyToGUI();
					return lResult;
				}
			
			protected virtual CReportRootFolder createReportRootFolder(string aCollectionName) {
					return new CReportRootFolder(aCollectionName, this);
				}
			
			public void ClearCollection() {
					string lCollectionName = _RootFolder.ItemName;
					_RootFolder.Release();
					_RootFolder = createReportRootFolder(lCollectionName);
					_ctlGUITreeView.Nodes.Add(_RootFolder.GUINode);
				}
				
			public void ApplyToGUI() {
					if (_ctlGUITreeView != null) {
							RootFolder.GUIRefresh(null);
						}
				}
			
			public bool QuickSave() {
					if (_LastFile == "")
							return false;
						else
							SaveToXML(_LastFile);
					return true;
				}
			
			public void SaveToXML(string aFile) {
					ExportToXML(aFile);
					_LastFile = aFile;
					setModified(false);
				}
				
			public void ExportToXML(string aFile) {
					XmlDocument lXMLDoc = new XmlDocument();
					
					lXMLDoc.AppendChild(lXMLDoc.CreateXmlDeclaration("1.0", "utf-8", ""));
					lXMLDoc.AppendChild(lXMLDoc.CreateDocumentType("ReportCollection", null, null, null));
					XmlNode lRootNode = lXMLDoc.CreateNode("element", "report-collection", "");
					XmlAttribute lDocVer = lXMLDoc.CreateAttribute("version");
					lDocVer.Value = "rcf0";
					lRootNode.Attributes.Append(lDocVer);
					
					lXMLDoc.AppendChild(lRootNode);
					RootFolder.BuildXML(lRootNode, lXMLDoc);
					
					lXMLDoc.Save(aFile);

				}
				
			public void LoadFromXML(string aFile) {
					int i = 0;
					XmlNode lRoot = null;
					XmlDocument lXMLDoc = new XmlDocument();
					lXMLDoc.Load(aFile);
				
					XmlNodeList lRootSet = lXMLDoc.ChildNodes;
					for (int j = 0; j < lRootSet.Count; j++)
							if (lRootSet[j].Name == "report-collection") {
									lRoot = lRootSet[j];
									break;
								}
					
					if (lRoot == null) {
							ReportSmart.Controls.CRSMessageBox.ShowBox("Hibás fájl!", "Hiba");
						} else {
							while (lRoot.ChildNodes.Count > i && lRoot.ChildNodes[i].Name != "folder") {
									ReportSmart.Controls.CRSMessageBox.ShowBox(lRoot.ChildNodes[i].Name, "DEBUG");
									i++;
								}
							if (i < lRoot.ChildNodes.Count) {
									RootFolder.BuildFromXML(lRoot.ChildNodes[i]);
								}
							ApplyToGUI();
						}
					_LastFile = aFile;
					setModified(false);
				}

			public void Release() {
					ClearCollection();
					_RootFolder.Release();
					_Killed = true;
				}
				
			public CReportCollection() {
					_RootFolder = createReportRootFolder("root");
					_Killed = false;
					_LastFile = "";
					_Modified = true;
				}
				
			
		}
		
}
