#region Source information

//*****************************************************************************
//
//    RsCollectionControl.cs
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
