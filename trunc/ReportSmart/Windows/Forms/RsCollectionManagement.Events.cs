#region Source information

//*****************************************************************************
//
//    RsCollectionManagement.Events.cs
//    Created by Adam (2009-09-17, 8:57)
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
