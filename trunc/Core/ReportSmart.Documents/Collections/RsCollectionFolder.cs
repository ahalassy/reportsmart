/*
 *
 * Licensing:			GPL
 * Original project:	RsReportProvider.csproj
 *
 * Copyright: Adam Halassy (2010.12.09.)
 * 
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;

using ReportSmart;
using ReportSmart.Documents;

namespace ReportSmart.Documents.Collections {
		/// <summary>
		/// Description of RsCollectionFolder.
		/// </summary>
		public class RsCollectionFolder: RsCollectionItem {	
		
				public CReportFolder ReportFolder { get { return (CReportFolder)ReportItem; }}
		
				public List<RsCollectionItem> GetSubItems() {
						List<RsCollectionItem> lResult = new List<RsCollectionItem>();
						
						if (ReportFolder.hasChildren) {
								for (int i = 0; i < ReportFolder.ItemCount; i++)  {
										CReportItem iItem = ReportFolder[i];
										lResult.Add(new RsCollectionItem(iItem));
									}
							}
							
						return lResult;
					}
		
				public List<RsCollectionFolder> GetSubFolders() {
						List<RsCollectionFolder> lResult = new List<RsCollectionFolder>();
						
						if (ReportFolder.hasChildren) {
								for (int i = 0; i < ReportFolder.ItemCount; i++)  {
										CReportItem iItem = ReportFolder[i];
								
										if (iItem is CReportFolder) {
												RsCollectionFolder lFolder = new RsCollectionFolder((CReportFolder)iItem);
												lResult.Add(lFolder);
											}
									}
							}
							
						return lResult;
					}
		
				public List<RsCollectionReport> GetReports() {
						List<RsCollectionReport> lResult = new List<RsCollectionReport>();
						
						if (ReportFolder.hasChildren) {
								for (int i = 0; i < ReportFolder.ItemCount; i++)  {
										CReportItem iItem = ReportFolder[i];
								
										if (iItem is CReportFile) {
												RsCollectionReport lReport = new RsCollectionReport((CReportFile)iItem);
												lResult.Add(lReport);
												
											}
									}
							}
							
						return lResult;
					}
		
				public string GetNextName(string aDefaultName) {
						List<RsCollectionItem> lItems = GetSubItems();
						bool lFound;
						string lNewName;
						int lIndexer = 0;
						
						do {
								lFound = false;
								lNewName = aDefaultName + ((lIndexer == 0) ? "" : " " + lIndexer.ToString());
								
								foreach(RsCollectionItem iItem in lItems)
										if (iItem.ItemName.ToUpper().Equals(lNewName.ToUpper())) {
												lFound = true;
												lIndexer++;
												break;
											}
										
					
							} while (lFound);
							
						return lNewName;
					}
		
				public RsCollectionFolder AddSubFolder(string aName) {				
						CReportFolder lFolder = new CReportFolder(aName);
						lFolder.Parent = this.ReportFolder;
						ReportFolder.Collection.QuickSave();
						
						return new RsCollectionFolder(lFolder);
					}
		
				public RsCollectionFolder(CReportFolder aFolder): base(aFolder) {}
			}
	}
