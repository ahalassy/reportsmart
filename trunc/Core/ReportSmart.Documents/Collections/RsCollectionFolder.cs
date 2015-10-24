#region Source information

//*****************************************************************************
//
//    RsCollectionFolder.cs
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
