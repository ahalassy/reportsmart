/*
 * 
 * Sx Xavier
 * 2009-01-19@nb-adam
 * 
 * Required sources:
 * 	common\common-classes.cs
 * 	common\Strings.cs
 * 	common\list-classes.cs
 * 
 */
 
 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Windows.Forms;

using Halassy.Classes;

namespace Halassy {
		public static class CommonTools {
				public const string MY_NAME = "Halassy Studio";

				public static System.Array ResizeArray(System.Array aArray, int aSize) {
   						int lSize = aArray.Length;
   						System.Type lType = aArray.GetType().GetElementType();
   						System.Array Result = System.Array.CreateInstance(lType, aSize);
   						int lPLength = System.Math.Min(aSize, lSize);

   						if (lPLength > 0)
      							System.Array.Copy (aArray, Result, lPLength);

   						return Result;
					}
					
			}
			
		public delegate void XmlPropertyChanged(CXmlSettingsNode aSender);
		
		public class CXmlSettingsNode {
				public const string XMLa_ID = "ID";
				public const string XMLv_YES = "yes";
				public const string XMLv_NO = "no";
				
				public static string ValidateNewID(string aID, XmlNode aParentNode) {
						int lNumber = 0;
						string lResult = aID;
						bool lIDisOk = true;
					
						if (aParentNode != null && aParentNode.ChildNodes.Count > 0) {
								lIDisOk = false;
								while (!lIDisOk)
										foreach (XmlNode iNode in aParentNode.ChildNodes) {
												lIDisOk = lNumber > 0 ?
															XmlTools.GetAttrib(iNode, XMLa_ID).ToUpper()+lNumber.ToString() != aID.ToUpper()+lNumber.ToString()
															:
															XmlTools.GetAttrib(iNode, XMLa_ID).ToUpper() != aID.ToUpper();
															
												if (!lIDisOk) {
														lNumber++; 
														break;
													}
											}
							}
							
						return lNumber > 0 ? aID + lNumber.ToString() : aID;
					}					
					
				public event XmlPropertyChanged PropertyChanged;
					
				private XmlNode _ParentNode, _DataNode;
				private bool _Released = false;
				
				public XmlNode ParentNode { get { return _ParentNode; }}
				public XmlNode DataNode { get { return _DataNode; }}
				public XmlDocument Document { get { return _ParentNode.OwnerDocument; }}
				
				public string this[string iAttribute] {
						get { return XmlTools.GetAttrib(_DataNode, iAttribute); }
						set {
								XmlTools.SetAttrib(_DataNode, iAttribute, value);
								if (PropertyChanged != null)
										PropertyChanged(this);
							}
					}
					
				public virtual string NodeID {
						get { return this[XMLa_ID]; }
						set { this[XMLa_ID] = value; }
					}
					
				public bool Serialized {
						get { return XmlTools.IsAttr(DataNode, XMLa_ID); }
					}
				
				public bool Released {
						get { return _Released; }
					}
				
				// "yes" value means true, otherwise value is false.
				public bool GetAsBool(string iAttribute) { return XmlTools.GetAttrib(_DataNode, iAttribute).ToUpper() == XMLv_YES.ToUpper(); }
				public void SetAsBool(string iAttribute, bool aValue) {
						XmlTools.SetAttrib(_DataNode, iAttribute, aValue ? XMLv_YES : XMLv_NO);
						if (PropertyChanged != null)
								PropertyChanged(this);
					}
					
				// If value cannot be converted to number, returns 0.
				public int GetAsInt(string iAttribute) {
						int lResult = 0;
						bool lSuccess = int.TryParse( XmlTools.GetAttrib(_DataNode, iAttribute), out lResult);
						return lSuccess ? lResult : 0;						
					}
				public void SetAsInt(string iAttribute, int aValue) {
						XmlTools.SetAttrib(_DataNode, iAttribute, aValue.ToString());
						if (PropertyChanged != null)
								PropertyChanged(this);
					}

				protected virtual void initialize() {
						_ParentNode = DataNode.ParentNode;
					}
				
				// Create as absolutely new node:
				protected virtual void createAsNewNode(string aName, string aID, XmlNode aParentNode) {
						_DataNode = XmlTools.CreateXmlNode(aParentNode.OwnerDocument, aName, aParentNode);
						
						if (aID != "")
								NodeID = aID;
					}
				
				// Create from an existing, and known node:
				protected virtual void createAsExistingNode(XmlNode aNode) {
						_DataNode = aNode;
					}
				
				// Lookup and create node:
				protected virtual void createAsExistingNode(string aName, XmlNode aParentNode) {
						_DataNode = XmlTools.getXmlNodeByName(aName, aParentNode);
						
						if (_DataNode == null)
								createAsNewNode(aName, "", aParentNode);
					}

				protected virtual void createAsExistingSerializedNode(string aName, string aID, XmlNode aParentNode) {
						_DataNode = XmlTools.getXmlNodeByAttrVal(aName, XMLa_ID, aID, aParentNode);
						if (_DataNode == null)
								createAsNewNode(aName, aID, aParentNode);
					}
				
				public virtual void Release() {
						_Released = true;
						ParentNode.RemoveChild(DataNode);
					}
				
				// Create new node:
				public CXmlSettingsNode(string aName, XmlNode aParentNode, bool aIsNew) {
						if (aIsNew)
								createAsNewNode(aName, "", aParentNode);
							else
								createAsExistingNode(aName, aParentNode);
						initialize();
					}
				public CXmlSettingsNode(string aName, string aID, XmlNode aParentNode, bool aIsNew) {
						if (aIsNew)
								createAsNewNode(aName, aID, aParentNode);
							else
								createAsExistingSerializedNode(aName, aID, aParentNode);
						initialize();
					}
					
				// Create new known node:
				public CXmlSettingsNode(XmlNode aNode) {
						createAsExistingNode(aNode);
						initialize();
					}
					
			}
		
		public abstract class CXmlSettingsContainer: CXmlSettingsNode {

				protected override void initialize() {
						base.initialize();
						if (!CheckIntegrity())
								buildDefaultStructure();
					}
					
				protected abstract void buildDefaultStructure();
				
				public abstract bool CheckIntegrity();
				
				public CXmlSettingsContainer(string aName, XmlNode aParentNode, bool aIsNew): base(aName, aParentNode, aIsNew) {}
				public CXmlSettingsContainer(string aName, string aID, XmlNode aParentNode, bool aIsNew): base(aName, aID, aParentNode, aIsNew) {}
				public CXmlSettingsContainer(XmlNode aNode): base(aNode) {}
			}

		public class CXmlSettingsList {
				private ArrayList _List;
				
				public CXmlSettingsNode this[int aIndex] {
						get { return ((CXmlSettingsNode)_List[aIndex]); }
						set {_List[aIndex] = value; }
					}
				
				public int Count { get { return _List.Count; }}
				
				
				public void Add(CXmlSettingsNode aNode) {
						if (_List.IndexOf(aNode) < 0)
								_List.Add(aNode);
					}
				
				public void Remove(CXmlSettingsNode aNode) {
						if (_List.IndexOf(aNode) >= 0)
								_List.Remove(aNode);
					}
				
				public CXmlSettingsList() {
						_List = new ArrayList();
					}
			}

		public static partial class XmlTools {
				public static bool IsNode(XmlNode aNode, string aNodeName) {
						aNodeName = aNodeName.ToUpper();
						foreach (XmlNode iNode in aNode.ChildNodes) {
								if (iNode.Name.ToUpper() == aNodeName)
										return true;
							}
							
						return false;
					}
		
				public static bool IsAttr(XmlNode aNode, string aAttr) {
						foreach (XmlAttribute lAttr in aNode.Attributes)
										if (lAttr.Name.ToUpper() == aAttr.ToUpper())
												return true;
						return false;
					}
		
				public static void SetAttrib(XmlNode aNode, string aAttr, string aValue) {
						XmlAttribute lAttr = IsAttr(aNode, aAttr) ? aNode.Attributes[aAttr] : CreateAttr(aNode, aAttr);
						lAttr.Value = aValue;
					}
					
				public static string GetAttrib(XmlNode aNode, string aAttr) {
						return IsAttr(aNode, aAttr) ? aNode.Attributes[aAttr].Value : "";
					}
					
				public static XmlNode GetNode(XmlNode aNode, string aName) {
						return getXmlNodeByName(aName, aNode);
					}
					
				public static XmlNode getXmlNodeByName(string aName, XmlNode aNode) {
						return aNode == null ? null : getXmlNodeByName(0, aName, aNode);
					}
				
				public static XmlNode getXmlNodeByName(int aFrom, string aName, XmlNode aNode) {
						if (aNode == null)
								return null;
								
						aName = aName.ToUpper();
						for (int i = aFrom; i < aNode.ChildNodes.Count; i++) {
								if (aNode.ChildNodes[i].Name.ToUpper() == aName)
										return aNode.ChildNodes[i];
							}
						return null;
					}
					
				/*
				public static XmlNode getXmlNodeByAttrVal(string aName, string aValue, XmlNode aNode) {
						return aNode == null ? null : getXmlNodeByAttrVal(aName, aValue, aNode);
					}
				*/
					
				public static XmlAttribute AddNewAttr(XmlDocument aDoc, XmlNode aNode, string aName, string aValue) {
						XmlAttribute lResult = aDoc.CreateAttribute(aName);
						lResult.Value = aValue;
						aNode.Attributes.Append(lResult);
						return lResult;
					}
				
				public static XmlDocument CreateEmptyXML() {
						XmlDocument lResult = new XmlDocument();
						lResult.AppendChild(lResult.CreateXmlDeclaration("1.0", "utf-8", ""));
						return lResult;
					}
				
				public static XmlNode CreateXmlNode(XmlDocument aDoc, string aName, XmlNode aParent) {
						XmlNode lResult = aDoc.CreateNode("element", aName, "");
						if (aParent == null)
								aDoc.AppendChild(lResult);
							else
								aParent.AppendChild(lResult);
								
						return lResult;
					}
					
				public static XmlAttribute CreateAttr(XmlNode aNode, string aName) {
						XmlAttribute lResult = aNode.OwnerDocument.CreateAttribute(aName);
						aNode.Attributes.Append(lResult);
						return lResult;
					}

				public static XmlNode getXmlNodeByAttrVal(string aNodeName, string aAttrName, string aValue, XmlNode aNode) {
						if (aNode == null)
								return null;
				
						aNodeName = aNodeName.ToUpper();
						aAttrName = aAttrName.ToUpper();
						aValue = aValue.ToUpper();
						foreach (XmlNode iNode in aNode.ChildNodes) {
								if (iNode.Name.ToUpper() == aNodeName)
										foreach (XmlAttribute iAttr in iNode.Attributes) {
												if (iAttr.Name.ToUpper() == aAttrName) {
														if (iAttr.Value.ToUpper() == aValue)
																return iNode;
													}
											}
							}
					
						return null;
					}				
				
				
				public static XmlNode getXmlNodeByAttrVal(string aName, string aValue, XmlNode aNode) {
						if (aNode == null)
								return null;
				
						aName = aName.ToUpper();
						aValue = aValue.ToUpper();
						foreach (XmlNode iNode in aNode.ChildNodes) {
								if (iNode.Attributes != null) foreach (XmlAttribute iAttr in iNode.Attributes) {
										if (iAttr.Name.ToUpper() == aName) {
												if (iAttr.Value.ToUpper() == aValue)
														return iNode;
											}
									}
							}
					
						return null;
					}
				
			}
		
		public static class FileFormats {
				public static int[] GetExcelHeader() {
						return new int[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
					}
		
				public static bool IsExcel(string aFileName) {				
						if (File.Exists(aFileName)) {
								FileStream lFile = new FileStream(aFileName, FileMode.Open, FileAccess.Read);
								int[] lExcelHeader = GetExcelHeader();
								bool lResult = true;
								int lData = 0;
								if (lFile.Length < 8) {
										lFile.Close();
										return false;
									}
								
								for (int i = 0; i < 8; i++) {
										lData = lFile.ReadByte();
										lResult &= lExcelHeader[i] == lData;
									}
								lFile.Close();
								return lResult;
							} else {
								return false;
							}
					}
			}
		
		public static class FileSystemTools {
				public static bool IsRootNode(string aPath) {
						if (aPath.IndexOf(":") == 1 && aPath.Length <= 3)
								return true;
							else
								return false;
					}
		
				public static string toPath(string aPath) {
						// Converts aPath string to "C:\ ... \folder\" form.
						if (aPath[aPath.Length-1] != '\\')
								return aPath += '\\';
							else
								return aPath;
					}
					
				public static int forcePath(string aPath) {
						// Creates the full path
						int Result = 0;
						int lPos = -1;
						int lStrPos;
						#if DEBUG
						System.Diagnostics.Debugger.Log(0, "RsView", "Building path: (" + aPath + "):\n");
						#endif
						
						do {
								lPos = aPath.IndexOf('\\', lPos + 1);
								lStrPos = lPos == -1 ? aPath.Length : lPos;
								string iNode = aPath.Substring(0, lStrPos);
						
								//lPos = StringTools.IndexOf(lPos + 1, "\\", aPath);
								
								#if DEBUG
								System.Diagnostics.Debugger.Log(0, "RsView", (Directory.Exists(iNode) ? "::" : "!!") + iNode);
								
								#endif
								
								if (!IsRootNode(iNode) && !Directory.Exists(iNode))
										Directory.CreateDirectory(iNode);
							} while (lPos != -1);
							
						#if DEBUG
						System.Diagnostics.Debugger.Log(1, "RsView", "-- Done. ----\n");
						#endif
						return Result;
						
						
					}
					
				public static string toDirName(string aPath) {
						if (aPath[aPath.Length-1] == '\\') 
								aPath = aPath.Substring(0, aPath.Length-1);
								
						return aPath;							
					}
					
				public static string pathOf(string aPath) {
						int i;
						for (i = -1; aPath.IndexOf('\\', i+1) > -1; i = aPath.IndexOf('\\', i+1)) ;
						return aPath.Substring(i+1);
					}
					
				public static string generateTempName(string aPath) {
						//string lResult;
						List<string> lFiles = new List<string>();
						aPath = FileSystemTools.toPath(aPath);
						foreach (string lFileName in Directory.GetFiles(aPath)) {
								lFiles.Add(lFileName);
							}
							
                        for (int i = 0; i < lFiles.Count; i++) {
                                if (lFiles.Contains(lFiles[i])) {
                                        Console.WriteLine("Error!\nNot relocated: " + lFiles[i]);
                                        break;
                                    }
                            }
							
						//for (ulong i = Random(0xFFFFFFFF), lFiles.inde)
							
						return "temp.tmp";
					}
			}

		public static class StringTools {
					
				public static string toHex(long aNum) {
						return toHex(aNum, 0);
					}
				
				public static string toHex(long aNum, int aLength) {
						const string HEX_ALPHABET = "0123456789ABCDEF";
						string lResult = "";
						long lMod;
						
						do {
								lMod = aNum % 16;
								aNum /= 16;
								lResult = HEX_ALPHABET[(int)lMod] + lResult;
							} while (aNum > 1);
							
						for (int i = lResult.Length; i < aLength; i++)
								lResult = "0" + lResult;
							
						return "0x" + lResult;
					}

			}

		public static class FileTools {
				public static string getPath(string aFullName) {
					/* Returns with the path of the file */
						//while String.
						
					return aFullName;
					}

			}
	}
	
namespace Halassy.Controls {

		public struct ControlProperties {
				// Sizes:
				public const int ButtonWidth = 128;
				public const int ButtonHeight = 32;
				public const int TextCtlWidth = 256;
				public const int TextCtlHeight = 24;
				
				public static Size ButtonSize() { return new Size(ButtonWidth, ButtonHeight); }
				public static Size TextCtlSize() { return new Size(TextCtlWidth, TextCtlHeight); }
				public static Size HeaderCtlSize() { return new Size(ButtonWidth/2, TextCtlHeight); }
				
				public static Size GroupBoxSize(int lColCount, int lRowCount, int lSumColWidth, int lSumRowHeight) {
						return new Size(
								lSumColWidth + (lColCount + 1) * ControlSpacing,
								lSumRowHeight + (lColCount) * ControlSpacing + HeaderSpacing + BottomSpacing
							);
					}
				
				// Spacing:
				public const int ControlSpacing = 8;
				public const int HeaderSpacing = 3*ControlSpacing;
				public const int BottomSpacing = ControlSpacing;
				
				// Coloring:
				public const byte HiglightingRate = 64;
				
				public static Color ColorItemInBack() {
						return Color.FromArgb(63, 83, 107);
					}
			
			}
	}

namespace Halassy.Graph {

		public class Coloring {
				public static Color MergeColors(Color clSource, Color clTarget, double aScale) {
							return Color.FromArgb(
									(byte)(Math.Round(clSource.A * aScale + clTarget.A * (1 - aScale))),
									(byte)(Math.Round(clSource.R * aScale + clTarget.R * (1 - aScale))),
									(byte)(Math.Round(clSource.G * aScale + clTarget.G * (1 - aScale))),
									(byte)(Math.Round(clSource.B * aScale + clTarget.B * (1 - aScale)))
								);
					}
			}
	}