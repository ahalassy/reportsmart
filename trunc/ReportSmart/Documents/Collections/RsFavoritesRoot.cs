#region Source information

//*****************************************************************************
//
//    RsFavoritesRoot.cs
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
