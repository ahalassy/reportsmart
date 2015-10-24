/*
 * 2009.09.08. (Adam ReportSmart)
 * 
 */

using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using ReportSmart;
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;

namespace ReportSmart.Documents.Collections {
		
	public class RsReportCollection {
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
					#if DEBUG
					System.Diagnostics.Debugger.Log(0, "FileOperation", "Loading collection: " + aFile + "\n");
					#endif
			
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
							throw new Exception("Invalid report collection file!");
						} else {
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
				
			public RsReportCollection() {
					_RootFolder = createReportRootFolder("root");
					_Killed = false;
					_LastFile = "";
					_Modified = true;
				}
				
			
		}
		
}
