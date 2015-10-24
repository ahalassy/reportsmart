/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam Halassy (2010.12.18.)
 * 
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Halassy.Localization;

using ReportSmart;
using ReportSmart.Application;
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;
using ReportSmart.Windows;
using ReportSmart.Windows.Forms;

namespace ReportSmart.Windows.Forms {
		public partial class RsCollectionManagement: Panel, ILocalizedControl {
		
				protected void AssignEventHandlers() {
						CollectionBrowser.CollectionNodeSelected += new NodeSelectedEvent(ehSelectNode);
						
						ItemList.DoubleClick += new EventHandler(ehOpenNode);
					}
		
				protected void ehOpenNode(object aSender, EventArgs aE) {
						foreach (RsListViewItem iItem in ItemList.SelectedItems) {				
								if (iItem.IsReportFile()) {
										CReportFile lFile = iItem.ReportItem as CReportFile;
								
										RsViewEngine.MainForm.OpenReport(lFile.ItemName, lFile.ReportFile);
									}
							}
					}
		
				protected void ehSelectNode(RsCollectionTree aSender, RsCollectionTreeNode aNode) {
						if (ItemList.Items != null)
								ItemList.Items.Clear();
					
						if (aNode != null && aNode.CollectionItem is RsCollectionFolder) {
								RsCollectionFolder lFolder = aNode.CollectionItem as RsCollectionFolder;
						
								List<RsCollectionItem> lItems = lFolder.GetSubItems();
								foreach (RsCollectionItem iItem in lItems) {
										ItemList.Items.Add(new RsListViewItem(iItem));
									}
							}
					}
		
			}
	}
