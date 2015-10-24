/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.09.)
 * 
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using ReportSmart;
using ReportSmart.Forms;
using ReportSmart.Localization;
using ReportSmart.Network;
using ReportSmart.Network.Email;
using ReportSmart.Security;
using ReportSmart.Special;
using ReportSmart.Special.WinApi;

using ReportSmart.Application;
using ReportSmart.Engine;
using ReportSmart.Engine.Config;
using ReportSmart.Controls; 
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;

namespace ReportSmart.Windows.Forms {
		internal static class RsCollectionControl {
				public static SaveFileDialog SaveCollectionDialog { get; private set; }
				public static OpenFileDialog OpenCollectionDialog { get; private set; }
				public static CdlgCreateCollection CreateCollectionDialog { get; private set; }
	
				private static void EhLocalizationChanged(CLocalization aLocale) {
						XmlNode lDialogLocale = aLocale.GetDialogData("generalDialogs");
				
						SaveCollectionDialog.Filter = FileDialogFilters.BuildCollectionFilter(lDialogLocale);
						OpenCollectionDialog.Filter = FileDialogFilters.BuildCollectionFilter(lDialogLocale);
					}
				
				public static RsReportCollection CreateCollection() {
						// TODO Implement CreateCollection
						
						throw new NotImplementedException();
					}
					
				public static RsReportCollection ImportCollection() {
						if (OpenCollectionDialog.ShowDialog() == DialogResult.OK) {
								RsReportCollection lResult = new RsReportCollection();
								lResult.LoadFromXML(OpenCollectionDialog.FileName);
						
								RsViewEngine.CollectionManager.AddCollection(lResult);
								return lResult;
								
							} else
								return null;
				
					}
					
				public static void ExcludeCollection() {
						// TODO Implement ExcludeCollection
						
						throw new NotImplementedException();
					}

				public static void ExportCollection(RsReportCollection aCollection) {
						if (SaveCollectionDialog.ShowDialog() == DialogResult.OK)
								aCollection.ExportToXML(SaveCollectionDialog.FileName);
					}
				
				public static void Initialize() {
						SaveCollectionDialog = new SaveFileDialog();
						OpenCollectionDialog = new OpenFileDialog();
						CreateCollectionDialog = new CdlgCreateCollection();
						
						RsViewEngine.Locale.AddLocalizedControl(CreateCollectionDialog);
						
						RsViewEngine.Locale.LocalizationChanged += new LocalizationChangeEventHandler(EhLocalizationChanged);
					}
			
				#region EventHandlers:
				#endregion
			}
	}
