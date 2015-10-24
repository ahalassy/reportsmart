/*
 *
 * Licensing:			GPL
 * Original project:	RsReportProvider.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.09.)
 * 
 * 
 */
using System;

namespace ReportSmart.Documents.Collections {
		/// <summary>
		/// Description of RsCollectionItem.
		/// </summary>
		public class RsCollectionItem {
		
				public CReportItem ReportItem { get; protected set; }
				
				public override string ToString() {
						return ReportItem.ItemName;
					}
		
				public string ItemName {
						get { return ReportItem.ItemName; }
						set {
								ReportItem.ItemName = value;
								ReportItem.Collection.QuickSave();
							}
					}
					
				public RsCollectionItem(CReportItem aReportItem) {
						ReportItem = aReportItem;
					}
			}
	}
