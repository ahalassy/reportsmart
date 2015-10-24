/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam Halassy (2010.12.09.)
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

using Halassy;
using Halassy.Forms;
using Halassy.Localization;
using Halassy.Network;
using Halassy.Network.Email;
using Halassy.Security;
using Halassy.Special;
using Halassy.Special.WinApi;

using ReportSmart;
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
