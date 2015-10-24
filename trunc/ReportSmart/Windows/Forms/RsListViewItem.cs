/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.18.)
 * 
 * 
 */	
using System;
using System.Drawing;
using System.Windows.Forms;

using ReportSmart.Localization;

using ReportSmart;
using ReportSmart.Application;
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;
using ReportSmart.Windows;
using ReportSmart.Windows.Forms;


namespace ReportSmart.Windows.Forms {
		public partial class RsCollectionManagement: Panel, ILocalizedControl {
				
				protected class RsListViewItem: ListViewItem {
						public RsCollectionItem CollectionItem { get; protected set; }
						
						public CReportItem ReportItem {
								get { return CollectionItem.ReportItem; }
							}
						
						public bool IsReportFile() {
								return ReportItem is CReportFile;
							}
							
						public bool IsReportFolder() {
								return ReportItem is CReportFolder;
							}
						
						public RsListViewItem(RsCollectionItem aItem): base(aItem.ItemName) {
								CollectionItem = aItem;
							}
					}
			}
}
