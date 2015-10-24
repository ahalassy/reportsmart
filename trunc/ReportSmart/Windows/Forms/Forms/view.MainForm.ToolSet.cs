/*
 *
 * Licensing:			GPL
 * Original project:	bin.csproj
 *
 * Copyright: Adam Halassy (2009.10.08.)
 * 
 * 
 */

using System.Drawing;
using System.Windows.Forms;

using Halassy;
using Halassy.Controls;
using Halassy.Forms;

using ReportSmart;
using ReportSmart.Application;
using ReportSmart.Controls;
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;
using ReportSmart.Windows.Forms;
 
namespace ReportSmart {
 
		partial class CReportSmartForm {		
				protected void EH_SettingsClick(StripToolItem aSender) {
						_ctlPageSelector.VisiblePage = _pgSettings;
					}
		
				protected void EH_OpenAction(StripToolItem aSender) {
						if (RsViewEngine.dlgOpenReport.ShowDialog() == DialogResult.OK) {
								OpenReport(
											FileSystem.NameOf(RsViewEngine.dlgOpenReport.FileName),
											RsViewEngine.dlgOpenReport.FileName
										);
							}
					}
					
				protected void EH_ExportAction(StripToolItem aSender) {
						//RsCollectionControl.ExportCollection(CollectionManagement.CurrentCollection);
					}
		
				protected void EH_ImportAction(StripToolItem aSender) {
						if (RsViewEngine.dlgOpenCollection.ShowDialog() == DialogResult.OK)
								RsViewEngine.LoadCollection(RsViewEngine.dlgOpenCollection.FileName);
					}
					
				protected void EH_AddCollectionAction(StripToolItem aSender) {
						CollectionManagement.DoAddCollection();
					}
					
				protected void EH_AddRptAction(StripToolItem aSender) {
						CollectionManagement.DoAddReport();
					}
					
				protected void EH_AddFolderAction(StripToolItem aSender) {
						CollectionManagement.DoAddFolder();
					}
		
				protected void EH_EditAction(StripToolItem aSender) {
						CollectionManagement.DoModify();
					}
					
				protected void EH_RemoveAction(StripToolItem aSender) {
						CollectionManagement.DoRemove();
					}
					
				protected void EH_PrintAction(StripToolItem aSender) {
						if (_CurrentView != null)
								_CurrentView.PrintReport();
					}
					
				protected void EH_ShowAbout(StripToolItem aSender) {
						CRSAbout lAbout = new CRSAbout();
						
						lAbout.ShowDialog();
					}
					
				protected void EH_RefreshReport(StripToolItem aSender) {
						if (_CurrentView != null)
								_CurrentView.RefreshReport();
					}
					
				protected void EH_ShowGroups(StripToolItem aSender) {
						if (_CurrentView != null)
								_CurrentView.ShowGroups(aSender.Pushed);
					}
					
				protected void EH_Paging(StripToolItem aSender) {
						if (_CurrentView != null) {
								if (aSender == _iFirst) {_CurrentView.ShowPage(TShowPage.spFirst); return; }
								if (aSender == _iPrev) {_CurrentView.ShowPage(TShowPage.spPrev); return; }
								if (aSender == _iNext) {_CurrentView.ShowPage(TShowPage.spNext); return; }
								if (aSender == _iLast) {_CurrentView.ShowPage(TShowPage.spLast); return; }
							}
					}
					
				protected void EH_Mail(StripToolItem aSender)
						{ if (_CurrentView != null) _CurrentView.SendEmail(); }
						
				protected void EH_ZoomIn(StripToolItem aSender)
						{ if (_CurrentView != null)	_CurrentView.RptZoomIn(); UpdateRptView(); }
						
				protected void EH_ZoomOut(StripToolItem aSender)
						{ if (_CurrentView != null) _CurrentView.RptZoomOut(); UpdateRptView(); }
						
				protected void EH_ZoomFitwidth(StripToolItem aSender)
						{ if (_CurrentView != null) _CurrentView.RptZoomFitWidth(); UpdateRptView(); }
						
				protected void EH_ZoomFitwindow(StripToolItem aSender)
						{ if (_CurrentView != null) _CurrentView.RptZoomFitWindow(); UpdateRptView(); }
						
				protected void EH_ExportPDFAction(StripToolItem aSender)
						{ if (_CurrentView != null) _CurrentView.ExportToPDF(); }
						
				protected void EH_ExportWordAction(StripToolItem aSender)
						{ if (_CurrentView != null) _CurrentView.ExportToWord(); }
						
				protected void EH_ExportExcelAction(StripToolItem aSender)
						{ if (_CurrentView != null) _CurrentView.ExportToExcel(); }
						
				protected void EH_ExportXMLAction(StripToolItem aSender)
						{ if (_CurrentView != null) _CurrentView.ExportToXML(); }
			
				protected void EH_ExportHTMLAction(StripToolItem aSender)
						{ if (_CurrentView != null) _CurrentView.ExportToHTML(); }
				
				protected void EH_ExportExcelDataOnlyAction(StripToolItem aSender)
						{ if (_CurrentView != null) _CurrentView.ExportToExcelDataOnly(); }
				
				protected void EH_Offline(StripToolItem aSender) {
						if (_CurrentView != null) {
								_CurrentView.SwitchToOffline();
								_iOffline.Enabled = false;
								_iOffline.Pushed = true;
							}
					}
		
				private CPagedToolStrip _ctlToolStrip;
				
				public StripToolPageset _setCollection, _setSettings, _setReport;
				public StripToolPage _pHome, _pEdit, _pExport, _pView;
				private StripToolGroup
					/* Collections	Home: */	_gFile, _gSpec,
					/* Collections	Edit: */	_gCollAdd, _gCollEdit, _gCollRemove,
					/* Collections  Export:*/	_gPrint, _gToFile,
					/* Collections  View: */	_gRefresh, _gPanels, _gZoom, _gPages;
					
				private StripToolItem
					/* Collections	Home: */	_iOpenRpt, _iSettings, _iHelp, _iImportCollection, _iExportCollection,
					/* Collections	Edit: */	_iAddRpt, _iEditRpt, _iNewFolder, _iRemove, _iAddColl,
					/* Collections  Export:*/	_iPrint, _iMail, _iPDF, _iWord, _iXLS, _iHTML, _iXML,
					/* Collections  View: */	_iRefresh, _iOffline, _iGroups, _iZoomIn, _iZoomOut, _iZoomWidth, _iZoomWnd, _iFirst, _iPrev, _iNext, _iLast;
					
				private StripToolItem.StripToolItemAction
					/* Excel actions: */		_aExcelFormatted, _aExcelDataOnly;
				
				protected virtual void initToolStrip() {
						this.SuspendLayout();
				
						_ctlToolStrip = new CPagedToolStrip();
						_ctlToolStrip.IconSize = new Size(32, 32);
						_ctlToolStrip.Font = new System.Drawing.Font("Arial", _FontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
						_ctlToolStrip.Name = "ToolStrip";
						_ctlToolStrip.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
						_ctlToolStrip.Location = new Point(0, _ctlPageSelector.Bottom);
						//_ctlToolStrip.Location = new Point(0, 0);
						_ctlToolStrip.Size = new Size(_ctlPageSelector.Width, _ctlPagesHost.Top - _ctlPageSelector.Bottom);
						_ctlToolStrip.StripBackground = (Image)(RsViewEngine.SpecialResources.GetObject("ToolBG"));
						_ctlToolStrip.Dock = DockStyle.Top;
												
						
						// Create objects:
						_setCollection = new StripToolPageset(_ctlToolStrip);
						_setCollection.Add(_pHome = new StripToolPage(_ctlToolStrip, "home"));						
						_setCollection.Add(_pEdit = new StripToolPage(_ctlToolStrip, "edit"));
						_setSettings = new StripToolPageset(_ctlToolStrip);
						_setSettings.Add(_pHome);
						_setReport = new StripToolPageset(_ctlToolStrip);
						_setReport.Add(_pView = new StripToolPage(_ctlToolStrip, "view"));
						_setReport.Add(_pExport = new StripToolPage(_ctlToolStrip, "export"));
						_gFile = new StripToolGroup(_ctlToolStrip);
						_gSpec = new StripToolGroup(_ctlToolStrip);
						_gCollAdd = new StripToolGroup(_ctlToolStrip);
						_gCollEdit = new StripToolGroup(_ctlToolStrip);
						_gCollRemove = new StripToolGroup(_ctlToolStrip);
						_gPrint = new StripToolGroup(_ctlToolStrip);
						_gToFile = new StripToolGroup(_ctlToolStrip);
						_gRefresh = new StripToolGroup(_ctlToolStrip);
						_gPanels = new StripToolGroup(_ctlToolStrip);
						_gZoom = new StripToolGroup(_ctlToolStrip);
						_gPages = new StripToolGroup(_ctlToolStrip);

						_gFile.GroupColor = ControlProperties.ColorItemInBack();
						_gSpec.GroupColor = ControlProperties.ColorItemInBack();
						_gCollAdd.GroupColor = ControlProperties.ColorItemInBack();
						_gCollEdit.GroupColor = ControlProperties.ColorItemInBack();
						_gCollRemove.GroupColor = ControlProperties.ColorItemInBack();
						_gPrint.GroupColor = ControlProperties.ColorItemInBack();
						_gToFile.GroupColor = ControlProperties.ColorItemInBack();
						_gRefresh.GroupColor = ControlProperties.ColorItemInBack();
						_gPanels.GroupColor = ControlProperties.ColorItemInBack();
						_gZoom.GroupColor = ControlProperties.ColorItemInBack();
						_gPages.GroupColor = ControlProperties.ColorItemInBack();
						
						// Create icons:
						_iOpenRpt = _gFile.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_open_32x32"))));
						_iImportCollection = _gFile.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_importCollection_32x32"))));
						_iExportCollection = _gFile.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_exportCollection_32x32"))));
						_iSettings = _gSpec.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_settings_32x32"))));
						_iHelp = _gSpec.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_help_32x32"))));
						_iAddColl = _gCollAdd.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_addColl_32x32"))));
						_iNewFolder = _gCollAdd.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_newFolder_32x32"))));
						_iAddRpt = _gCollAdd.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_add_32x32"))));
						_iEditRpt = _gCollEdit.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_editItem_32x32"))));
						_iRemove = _gCollRemove.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_remove_32x32"))));
						_iPrint = _gPrint.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_print_32x32"))));
						_iMail = _gPrint.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_mail_32x32"))));
						_iPDF = _gToFile.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_pdf_32x32"))));
						_iWord = _gToFile.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_word_32x32"))));
						_iXLS = _gToFile.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_excel_32x32"))));
						_iHTML = _gToFile.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_html_32x32"))));
						_iXML = _gToFile.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_xml_32x32"))));
						_iRefresh = _gRefresh.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_refresh_32x32"))));
						_iOffline = _gRefresh.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_offline_32x32"))));
						_iGroups = _gPanels.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_groups_32x32"))));
						_iZoomIn = _gZoom.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_zoomIn_32x32"))));
						_iZoomWidth = _gZoom.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_zoomFitWidth_32x32"))));
						_iZoomWnd = _gZoom.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_zoomFitWindow_32x32"))));
						_iZoomOut = _gZoom.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_zoomOut_32x32"))));
						_iFirst = _gPages.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_first_32x32"))));
						_iPrev = _gPages.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_previous_32x32"))));
						_iNext = _gPages.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_next_32x32"))));
						_iLast = _gPages.Add(new StripToolItem(_ctlToolStrip, (Image)(RsViewEngine.Resources.GetObject("icon_last_32x32"))));
												
						// Build structure:
						_pHome.Add(_gFile);
						_pHome.Add(_gSpec);
						
						_pEdit.Add(_gFile);
						_pEdit.Add(_gCollAdd);
						_pEdit.Add(_gCollEdit);
						_pEdit.Add(_gCollRemove);
						
						_pExport.Add(_gFile);
						_pExport.Add(_gRefresh);
						_pExport.Add(_gPrint);
						_pExport.Add(_gToFile);
						
						_pView.Add(_gFile);
						_pView.Add(_gRefresh);
						_pView.Add(_gPages);
						_pView.Add(_gZoom);
						_pView.Add(_gPanels);

						
						// Add event handlers:
						_iOpenRpt.Action += new ToolItemAction(EH_OpenAction);
						_iSettings.Action += new ToolItemAction(EH_SettingsClick);
						_iExportCollection.Action += new ToolItemAction(EH_ExportAction);
						_iImportCollection.Action += new ToolItemAction(EH_ImportAction);
						_iEditRpt.Action += new ToolItemAction(EH_EditAction);
						_iAddRpt.Action += new ToolItemAction(EH_AddRptAction);
						_iRemove.Action += new ToolItemAction(EH_RemoveAction);
						_iNewFolder.Action += new ToolItemAction(EH_AddFolderAction);
						_iAddColl.Action += new ToolItemAction(EH_AddCollectionAction);
						_iPrint.Action += new ToolItemAction(EH_PrintAction);
						_iPDF.Action += new ToolItemAction(EH_ExportPDFAction);
						_iMail.Action += new ToolItemAction(EH_Mail);
						_iWord.Action += new ToolItemAction(EH_ExportWordAction);
						_iXLS.Action += new ToolItemAction(EH_ExportExcelAction);
						_iXML.Action += new ToolItemAction(EH_ExportXMLAction);
						_iHTML.Action += new ToolItemAction(EH_ExportHTMLAction);
						_iHelp.Action += new ToolItemAction(EH_ShowAbout);
						_iRefresh.Action += new ToolItemAction(EH_RefreshReport);
						_iOffline.Action += new ToolItemAction(EH_Offline);
						_iGroups.Action += new ToolItemAction(EH_ShowGroups);
						_iZoomIn.Action += new ToolItemAction(EH_ZoomIn);
						_iZoomOut.Action += new ToolItemAction(EH_ZoomOut);
						_iZoomWnd.Action += new ToolItemAction(EH_ZoomFitwindow);
						_iZoomWidth.Action += new ToolItemAction(EH_ZoomFitwidth);
						_iFirst.Action += new ToolItemAction(EH_Paging);
						_iPrev.Action += new ToolItemAction(EH_Paging);
						_iNext.Action += new ToolItemAction(EH_Paging);
						_iLast.Action += new ToolItemAction(EH_Paging);
						
						// Other statements:
						_ctlToolStrip.PageSet = _setCollection;
						_ctlToolStrip.SelectedPage = _pEdit;
						
						_iGroups.Type = StripToolItemType.Button;
						_iZoomWnd.Type = StripToolItemType.Button;
						_iZoomWidth.Type = StripToolItemType.Button;
						
						_iXLS.Type = StripToolItemType.Multifunctional;
						_aExcelFormatted = _iXLS.AddAction();
						_aExcelFormatted.Action += new ToolItemAction(EH_ExportExcelAction);
						_aExcelDataOnly = _iXLS.AddAction();
						_aExcelDataOnly.Action += new ToolItemAction(EH_ExportExcelDataOnlyAction);
						
						
						this.Controls.Add(_ctlToolStrip);
						//this._ctlPagesHost.Location = new System.Drawing.Point(0, _ctlToolStrip.Bottom);
						
						this.ResumeLayout(false);
					}
					
			}
	
}

