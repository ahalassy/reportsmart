#region Source information

//*****************************************************************************
//
//    RsLocalization.cs
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

using ReportSmart.Engine;
using ReportSmart.Engine.Config;
using ReportSmart.Controls;
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;


namespace ReportSmart.Application {
		/// <summary>
		/// Description of RsLocalization.
		/// </summary>
		internal class RsLocalization: CLocalization {
				public const string TAG_FAVORITES = "favorites";
				public const string TAG_MANCOLLECTIONS = "manageCollections";
				public const string TAG_SETTINGS = "settings";
		
				public const string DLG_ADDREPORT = "AddReport";
				
				public const string FRM_COLLECTIONEDITOR = "CollectionEditor";
		
				public static RsLocalization Current = null;
				
				public static void loadRSLocalization(string aFile) {
						Current = new RsLocalization();
						Current.LoadLocalization(aFile);
					}
					
				public string GetAddReportTitle(string aCollectionName) {
						XmlNode lData = GetDialogData(DLG_ADDREPORT);
						if (lData != null) {
								XmlNode lNode = XmlTools.getXmlNodeByAttrVal(XMLa_NAME, "title1", lData);
								string lPart1 = lNode == null ? "-" : lNode.InnerText;
								lNode = XmlTools.getXmlNodeByAttrVal(XMLa_NAME, "title2", lData);
								string lPart2 = lNode == null ? "-" : lNode.InnerText;
								return lPart1 + " \"" + aCollectionName + "\" " + lPart2;
							} else {
								#if DEBUG
								ReportSmart.Controls.CRSMessageBox.ShowBox("Hiba a dialógus keresése során!", "DEBUG");
								return "--";
								#else
								throw new Exception("Dialog \"" + DLG_ADDREPORT + "\" not found in localization file.");
								#endif
							}
					}
			}
	}
