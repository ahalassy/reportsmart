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

using ReportSmart;

namespace ReportSmart.Engine.Collections {
		public class CReportFile: CReportItem {
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
	}
