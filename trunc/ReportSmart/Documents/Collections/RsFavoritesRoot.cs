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
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;
using ReportSmart.Controls;

namespace ReportSmart.Documents.Collections {
		internal class CReportFavoritesRoot: CReportRootFolder {
				protected override void setItemName(string aName) {
						base.setItemName(aName);
						GUINode.Text = RsViewEngine.Locale.GetTagText(RsLocalization.TAG_FAVORITES);
					}
		
				public CReportFavoritesRoot(string aName, RsReportCollection aCollection): base(aName, aCollection) {
						GUINode.ImageIndex = 2;
						GUINode.SelectedImageIndex = 2;
						GUINode.Text = RsViewEngine.Locale.GetTagText(RsLocalization.TAG_FAVORITES);
					}
			}
	}
