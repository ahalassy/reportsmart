#region Source information

//*****************************************************************************
//
//    RsProfileManager.cs
//    Created by Adam (2015-10-23, 9:21)
//
// ---------------------------------------------------------------------------
//
//    Report Smart View
//    Copyright (C) 2009-2015, Adam Halassy
//
// ---------------------------------------------------------------------------
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
//*****************************************************************************

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

using ReportSmart;
using ReportSmart.Engine;
using ReportSmart.Engine.Config;
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;

namespace ReportSmart.Engine {
		public class RsProfileManager {
				
				#region Static methods:
				public static int GetProfileVersion(string aProfile) {
						if (!File.Exists(aProfile))
								return -1;
				
						XmlDocument lDoc = new XmlDocument();
						lDoc.Load(aProfile);
						
						XmlNode lNode = XmlTools.GetNode(lDoc, "ReportSmartSettings");
						try {
								return lNode == null ? -1 : int.Parse(XmlTools.GetAttrib(lNode, "configVersion"));
							} catch {
								return 0;
							}
						
						
					}
					
				public static bool IsProfileExists(string aProfile) {
						return File.Exists(aProfile);
					}
					
				public static void UpdateProfile(string aProfile) {
						List<RsCollectionConfig> lNewList = new List<RsCollectionConfig>();
						XmlDocument lDoc = new XmlDocument();
						lDoc.Load(aProfile);
						
						XmlNode lRoot = XmlTools.GetNode(lDoc, "ReportSmartSettings");
						if (lRoot == null) return;
												
						XmlNode lNode = XmlTools.GetNode(lRoot, "collections");
						if (lNode != null && lNode.ChildNodes != null) {
								foreach (XmlNode iNode in lNode.ChildNodes) {
										RsCollectionConfig iConfig = new RsCollectionConfig();
										iConfig.Path = XmlTools.GetAttrib(iNode, "path");
										iConfig.Type = iNode.Name == "favorites" ? RsCollectionType.Favorites : RsCollectionType.Custom;
										
                                        lNewList.Add(iConfig);
									}
							}
							
						lNode.RemoveAll();
						lRoot.RemoveChild(lNode);
						
						lNode = XmlTools.CreateXmlNode(lDoc, "Collections", lRoot);
						
						foreach (RsCollectionConfig iConfig in lNewList) {
								RsReportCollection iCollection = new RsReportCollection();
								iCollection.LoadFromXML(iConfig.Path);
								XmlNode iNode = XmlTools.CreateXmlNode(lDoc, "collection", lNode);
								XmlTools.SetAttrib(iNode, "path", iConfig.Path);
								XmlTools.SetAttrib(iNode, "type", iConfig.Type.ToString());
								XmlTools.SetAttrib(iNode, "name", iCollection.CollectionName);
								
							}
							
						XmlTools.SetAttrib(lRoot, "configVersion", "1");
						lDoc.Save(aProfile);
					}
				#endregion
		
				public string ProfileXmlFile { get; protected set; }
		
				public RsProfileConfig Profile { get; protected set; }
				
				public virtual void SaveProfile() {
						SaveProfile(ProfileXmlFile);
					}
				
				public virtual void SaveProfile(string aProfile) {
						ProfileXmlFile = aProfile;
						XmlTools.Serialize(Profile, aProfile);
					}
				
				public virtual void OpenProfile() {
						OpenProfile(ProfileXmlFile);
					}
					
				public virtual void OpenProfile(string aProfile) {
						ProfileXmlFile = aProfile;
						Profile = ((RsProfileConfig)(XmlTools.DeserializeFromFile(aProfile, typeof(RsProfileConfig))));
					}
					
				public void SaveMainFormPosition(Form aMainForm) {
						Profile.Settings.UserInterface.WindowConfig.Left = aMainForm.Location.X;
						Profile.Settings.UserInterface.WindowConfig.Top = aMainForm.Location.Y;
						Profile.Settings.UserInterface.WindowConfig.Width = aMainForm.Size.Width;
						Profile.Settings.UserInterface.WindowConfig.Height = aMainForm.Size.Height;
					}
		
				public RsProfileManager() {
						Profile = new RsProfileConfig();
					}
			}
	}
