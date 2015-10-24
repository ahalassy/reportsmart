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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using ReportSmart;
using ReportSmart.Engine.Config;
using ReportSmart.Documents.Collections;

namespace ReportSmart.Engine {
		public class RsCollectionManager {
		
				public static RsReportCollection CreateReportCollection(RsCollectionConfig aConfig) {
						RsReportCollection lResult = new RsReportCollection();
						lResult.LoadFromXML(aConfig.Path);
						return lResult;
					}
				
				public RsProfileManager ProfileManager { get; protected set; }
				
				public List<RsCollectionConfig> CollectionList { get { return ProfileManager.Profile.Collections; }}
				
				public void AddCollection(RsReportCollection aCollection) {
						RsCollectionConfig lConfig = new RsCollectionConfig();
						lConfig.Path = aCollection.FileName;
						lConfig.Name = aCollection.CollectionName;
						lConfig.Type = RsCollectionType.Custom;
						
						ProfileManager.Profile.Collections.Add(lConfig);
						ProfileManager.SaveProfile();
					}
					
				public void RemoveCollection(RsReportCollection aCollection) {
						foreach (RsCollectionConfig iConfig in ProfileManager.Profile.Collections) {
								if (aCollection.FileName.ToUpper().Equals(iConfig.Path.ToUpper())) {
										ProfileManager.Profile.Collections.Remove(iConfig);
										break;
									}
							}
					}
					
				public RsReportCollection GetCollection(string aCollectionName) {
						foreach (RsCollectionConfig iConfig in ProfileManager.Profile.Collections) {
								if (iConfig.Name.ToUpper().Equals(aCollectionName.ToUpper())) {
										RsReportCollection lResult = new RsReportCollection();
										lResult.LoadFromXML(iConfig.Path);
										return lResult;
									}
							}
						
						return null;
					}
					
				public RsReportCollection CreateCollection(string aCollectionName, string aFile) {
						// TODO Create a method to handle more than one collection with the same name
						
						RsReportCollection lResult;
				
						RsReportCollection lCollection = GetCollection(aCollectionName);
						if (lCollection != null)
								throw new Exception("Collection named \"" + aCollectionName + "\" already linked.");
								
							else {
								lResult = new RsReportCollection();
								lResult.CollectionName = aCollectionName;
								lResult.SaveToXML(aFile);
								AddCollection(lResult);
							}
							
						return lResult;
					}
		
				public RsCollectionManager(RsProfileManager aProfileManager) {
						ProfileManager = aProfileManager;
					}
			}
	}
