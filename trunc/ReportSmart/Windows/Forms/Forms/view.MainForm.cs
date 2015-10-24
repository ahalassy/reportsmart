#region Source information

//*****************************************************************************
//
//    view.MainForm.cs
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

using ReportSmart;
using ReportSmart.Controls;
using ReportSmart.GUI;
using ReportSmart.Localization;
using ReportSmart.Special.WinApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using ReportSmart.Application;
using ReportSmart.Windows.Forms;

namespace ReportSmart {
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	internal partial class CReportSmartForm : CLocalizedForm {
		private int _FontSize = 10;
		
		private CRSReportViewerHost _CurrentView = null;
		
		public void UpdateRptView() {
				if (_CurrentView != null) {
						_iZoomWidth.Pushed = _CurrentView.FittingWidth;
						_iZoomWnd.Pushed = _CurrentView.FittingWindow;
					}
			}
			
		public override string GetInstanceName() { return "CReportSmartForm"; }		
		public override void ApplyLocale(CLocalization aLocale) {
				XmlNode lData = aLocale.GetFormData("MainForm");
				
				_pHome.Title = XmlTools.getXmlNodeByName("pHome", lData).InnerText;
				_pEdit.Title = XmlTools.getXmlNodeByName("pEdit", lData).InnerText;
				_pExport.Title = XmlTools.getXmlNodeByName("pExport", lData).InnerText;
				_pView.Title = XmlTools.getXmlNodeByName("pView", lData).InnerText;
				
				_iOpenRpt.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "open", lData).InnerText;
				_iSettings.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "settings", lData).InnerText;
				_iHelp.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "about", lData).InnerText;
				_iImportCollection.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "import", lData).InnerText;
				_iExportCollection.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "export", lData).InnerText;
				_iAddRpt.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "addRpt", lData).InnerText;
				_iEditRpt.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "editRpt", lData).InnerText;
				_iNewFolder.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "addCatalog", lData).InnerText;
				_iRemove.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "remove", lData).InnerText;
				_iAddColl.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "addColl", lData).InnerText;
				_iPrint.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "print", lData).InnerText;
				_iMail.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "mail", lData).InnerText;
				_iPDF.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "expPdf", lData).InnerText;
				_iWord.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "expWord", lData).InnerText;
				_iXLS.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "expExcel", lData).InnerText;
				_iHTML.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "expHtml", lData).InnerText;
				_iXML.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "expXml", lData).InnerText;
				_iRefresh.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "refresh", lData).InnerText;
				_iOffline.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "offline", lData).InnerText;
				_iGroups.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "groups", lData).InnerText;
				_iZoomIn.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "zoomIn", lData).InnerText;
				_iZoomOut.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "zoomOut", lData).InnerText;
				_iZoomWidth.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "fitWidth", lData).InnerText;
				_iZoomWnd.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "fitPage", lData).InnerText;
				_iFirst.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "pFirst", lData).InnerText;
				_iPrev.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "pPrev", lData).InnerText;
				_iNext.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "pNext", lData).InnerText;
				_iLast.Hint = XmlTools.getXmlNodeByAttrVal("hint", "name", "pLast", lData).InnerText;
				
				_pgCollections.PageTitle = RsViewEngine.Locale.GetTagText(RsLocalization.TAG_MANCOLLECTIONS);
				_pgSettings.PageTitle = RsViewEngine.Locale.GetTagText(RsLocalization.TAG_SETTINGS);
			
				_aExcelDataOnly.Text = aLocale.GetTagText("fileXlsDataOnly");
				_aExcelFormatted.Text = aLocale.GetTagText("fileXlsFormatted");
			}
	
		public void InitializeForm() {
				InitializeComponent();
			
				_ctlPageSelector.Host = _ctlPagesHost;
				
				_pgCollections = new CPageSelectorPage(CollectionManagement);
				_pgCollections.CloseAble = false;
				_pgCollections.PageTitle = RsViewEngine.Locale.GetTagText(RsLocalization.TAG_MANCOLLECTIONS);
				_ctlPageSelector.AddPage(_pgCollections);
				
				_pgSettings = new CPageSelectorPage(_ctlSettingsPanel);
				_pgSettings.CloseAble = true;
				_pgSettings.PageTitle = RsViewEngine.Locale.GetTagText(RsLocalization.TAG_SETTINGS);
				
				initToolStrip(); 
				
				_ctlPageSelector.SelectedPage = 0;

				Shown += EH_Shown;
				this.Closed += new EventHandler(EH_Close);
				_pgCollections.PageSelected += new PageEventNotify(EH_CollEditSelected);
				_pgSettings.PageSelected += new PageEventNotify(EH_SettingsSelected);
				
				RsViewEngine.Locale.AddLocalizedControl(this);
				RsViewEngine.Locale.ApplyLocalization();
				
			}

		public void OpenReport(string aTitle, string aRptFile) {
				CRSReportViewerHost _newRptView = new CRSReportViewerHost();
				_newRptView.ReportFile = aRptFile;
				_newRptView.Title = aTitle;
				_ctlPageSelector.AddPage(_newRptView.SelectorPage);
				_newRptView.SelectorPage.PageSelected += new PageEventNotify(EH_RptViewSelected);
				_ctlPageSelector.VisiblePage = _newRptView.SelectorPage;
				_newRptView.UIInfoGet += ehRptUIReceieved;
				_newRptView.OpenReport();
			}
	
//		protected override CreateParams CreateParams {
//				get {					
//						if (!DwmApi.DwmAvailable()) {
//								CreateParams lCrParams = base.CreateParams;
//								lCrParams.ClassStyle |= 0x00020000; // adding "DropShadow" property to the form
//								return lCrParams; 
//							} else {
//								return base.CreateParams;
//							}
//					}
//			}

		protected void EH_SettingsSelected(CPageSelectorPage aPage) {
				_ctlSettingsPanel.UpdatePanel();
				_ctlToolStrip.PageSet = _setSettings;
				_ctlToolStrip.SelectedPage = _pHome;
				_CurrentView = null;
			}
			
		protected void EH_CollEditSelected(CPageSelectorPage aPage) {
				_ctlToolStrip.PageSet = _setCollection;
				_ctlToolStrip.SelectedPage = _pEdit;
				_CurrentView = null;
			}
			
		protected void EH_RptViewSelected(CPageSelectorPage aPage) {
				_CurrentView = (CRSReportViewerHost)(aPage.Page);
				_ctlToolStrip.PageSet = _setReport;
				_ctlToolStrip.SelectedPage = _pView;
				
				_iGroups.Enabled = false;
				_iGroups.Pushed = false;
				_iOffline.Enabled = false;
				_iOffline.Pushed = false;
				
				_gPages.Enabled = false;
				_gZoom.Enabled = false;
				_gRefresh.Enabled = false;
				_gPrint.Enabled = false;
				_gToFile.Enabled = false;

				_CurrentView.RequestUIState();
				
				//_iGroups.Pushed = ((CRSReportViewerHost)(aPage.Page)).GetGroupPanelState();
				//_iZoomWidth.Pushed = ((CRSReportViewerHost)(aPage.Page)).FittingWidth;
				//_iZoomWnd.Pushed = ((CRSReportViewerHost)(aPage.Page)).FittingWindow;
			}
		
		protected void EH_Shown(object aSender, EventArgs aEArgs) {
				RsViewEngine.Splash.Hide();
				RsViewEngine.Splash.Dispose();
			}
			
		protected void EH_Close(object aSender, EventArgs aEArgs) {
				RsViewEngine.ShutdownApplication();
			}
	
		protected void ehRptUIReceieved(CRSReportViewerHost aRptHost) {
				if (aRptHost == _CurrentView) {
						_gPages.Enabled = true;
						_gZoom.Enabled = true;
						_gRefresh.Enabled = true;
						_gPrint.Enabled = true;
						_gToFile.Enabled = true;

						_iGroups.Enabled = true;
						_iGroups.Pushed = aRptHost.GroupsOn;
						_iOffline.Enabled = true;
						_iOffline.Pushed = aRptHost.Offline;						
					}
			}
	}
}
