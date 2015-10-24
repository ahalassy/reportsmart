/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.19.)
 * 
 * 
 */
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
				protected class RsCollectionMgmtContext: ContextMenu, ILocalizedControl {
						public CLocalization Locale { get; protected set; }
						
						public RsCollectionManagement Owner { get; protected set; }
						
						public MenuItem ItemOpen { get; protected set; }
						public MenuItem ItemRename { get; protected set; }
						public MenuItem ItemRemove { get; protected set; }
						
						public void AssignLocale(CLocalization aLocale) {
								Locale = aLocale;
							}
							
						public void ApplyLocale(CLocalization aLocale) {
								ItemOpen.Text = aLocale.GetTagText("open");
								ItemRename.Text = aLocale.GetTagText("rename");
								ItemRemove.Text = aLocale.GetTagText("remove");
							}
							
						public string GetInstanceName() {
								return "RsCollectionManagementContext";
							}
							
						public void ReleaseLocale() {
								Locale = null;
							}
							
						public RsCollectionMgmtContext(RsCollectionManagement aOwner) {
								Owner = aOwner;
								
								ItemOpen = this.MenuItems.Add("_open");
								ItemRename = this.MenuItems.Add("_rename");
								this.MenuItems.Add("-");
								ItemRemove = this.MenuItems.Add("_remove");
								
								ItemOpen.Click += new EventHandler(ehItemOpen);
							}
							
						public void ehItemOpen(object aSender, EventArgs aE) {
								foreach (RsListViewItem iItem in Owner.ItemList.SelectedItems) {
										if (iItem.IsReportFile()) {
												RsViewEngine.MainForm.OpenReport(
															iItem.CollectionItem.ItemName,
															((CReportFile)(iItem.ReportItem)).ReportFile
														);
												System.Windows.Forms.Application.DoEvents();
												System.Threading.Thread.Sleep(500);
											}
									}
							}
					}
			}
	}
