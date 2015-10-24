#region Source information

//*****************************************************************************
//
//    CRSRptView.cs
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

/*
 * 2009-09-23 (Adam ReportSmart)
 * 
 */
 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml;

// using AcroPDFLib;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;

using ReportSmart;
using ReportSmart.Controls;
using ReportSmart.Network;
using ReportSmart.Network.Email;
using ReportSmart.Security;
using ReportSmart.Special;
using ReportSmart.Special.WinApi;
using ReportSmart.Special.ActiveX.AcroPDF;

using ReportSmart.Application;
using ReportSmart.Documents;
using ReportSmart.Engine;

namespace ReportSmart.Controls {
        internal enum TShowPage {
                spFirst = 0,
                spPrev = 1,
                spNext = 2,
                spLast = 3
            }



        internal static class RSAppMessages {
                #region CONSTANTS:
                public const int WM_PRINT = Messaging.WM_APPSPECIFIC + 0x0001;
                public const int WM_EXPORT = Messaging.WM_APPSPECIFIC + 0x0002;
                public const int WM_REFRESH = Messaging.WM_APPSPECIFIC + 0x0003;
                public const int WM_GROUP = Messaging.WM_APPSPECIFIC + 0x0004;
                public const int WM_ZOOM = Messaging.WM_APPSPECIFIC + 0x0005;
                public const int WM_THUMB = Messaging.WM_APPSPECIFIC + 0x0006;
                public const int WM_MAIL = Messaging.WM_APPSPECIFIC + 0x0007;
                public const int WM_OFFLINE = Messaging.WM_APPSPECIFIC + 0x0008;
                public const int WM_UI = Messaging.WM_APPSPECIFIC + 0x0009;
                
                public const int EX_PDF = 0;
                public const int EX_DOC = 1;
                public const int EX_XLS = 2;
                public const int EX_XLS_DATAONLY = 3;
                public const int EX_XML = 4;
                public const int EX_HTM32 = 5;
                public const int EX_HTM40 = 6;
                public const int EX_FIRST = 0;
                public const int EX_PREV = 1;
                public const int EX_NEXT = 2;
                public const int EX_LAST = 3;
                
                public const int EX_HIDE = 0x0000;
                public const int EX_SHOW = 0x0001;
                public const int EX_SET  = 0xfffe;
                public const int EX_GET  = 0xffff;
                
                public const uint FLAG_OPENED = 1;
                public const uint FLAG_OFFLINE = 2;
                public const uint FLAG_GROUP = 4;
                #endregion

                #region METHODS:				
                public static uint EncodeEmbedded(CRSReportViewerEmbedded aObj) {
                        uint lResult = 0;
                        lResult += aObj.IsOpened ? FLAG_OPENED : 0;
                        lResult += aObj.Offline ? FLAG_OFFLINE : 0;
                        lResult += aObj.GroupShow ? FLAG_GROUP : 0;
                            
                        return lResult;
                    }
                        
                public static bool CheckFlag(uint aVal, uint aFlag) { return (aVal & aFlag) == aFlag;	}
                
                #endregion
            }

        

        internal class CRSReportViewerHost: Panel {
                #region PUBLIC DELEGATES:
                public delegate void MessageNotify(CRSReportViewerHost aRptHost);
                
                #endregion
                
                #region PUBLIC STATIC:
                public const int ZOOM_FITWIDTH = 1;
                public const int ZOOM_FITWINDOW = 2;
                public const int ZOOM_INCREMENT = 25;
                #endregion
        
                #region PRIVATE FIELDS:
                private bool _Opened = false;
                private bool _GroupsOn = false;
                private bool _Offline = false;
                private int _Zoom;
                private string _ReportFile;
                private CPageSelectorPage _Page = null;
                private Process _ChildProcess;
                private IntPtr _ChildHandle;
                private Label _ErrorLabel;
                //private Timer _ChkTimer;
                #endregion

                #region PUBLIC EVENTS:
                public event MessageNotify UIInfoGet;

                #endregion
                
                #region PUBLIC PROPERTIES:
                public bool Opened { get { return _Opened; }}

                public bool GroupsOn { get { return _GroupsOn; }}

                public bool Offline { get { return _Offline; }}

                public bool FittingWidth { get { return _Zoom == ZOOM_FITWIDTH; }	}

                public bool FittingWindow {	get { return _Zoom == ZOOM_FITWINDOW; }	}

                public string ReportFile {
                        get { return _ReportFile; }
                        set { 
                                if (!_Opened)
                                    _ReportFile = value;
                            }
                    }
                    
                public CPageSelectorPage SelectorPage {
                        get {
                                if (_Page == null)
                                        _Page = new CPageSelectorPage(this);
                                return _Page;
                            }
                    }					
                    
                public string Title {
                        get { return SelectorPage.PageTitle; }
                        set { SelectorPage.PageTitle = value; }
                    }
                    
                public int Zoom {
                        get { return _Zoom; }
                        set { RptZoom(value); }
                    }
                
                #endregion
                
                #region PRIVATE METHODS:
                private void _DisplayErrorLabel() {
                        this._Page.PageTitle = _Page.PageTitle + " (" + RsViewEngine.Locale.GetTagText("error") + ")";
                
                        this.SuspendLayout();
                
                        _ErrorLabel = new Label();
                        _ErrorLabel.Name = "lErrorLabel";
                        _ErrorLabel.BackColor = Color.FromArgb(255, 255, 200, 200);
                        _ErrorLabel.TextAlign = ContentAlignment.MiddleCenter;
                        _ErrorLabel.Text = RsViewEngine.Locale.GetTagText("appCrashedNotify");
                        _ErrorLabel.BorderStyle = BorderStyle.FixedSingle;
                        _ErrorLabel.Location = new Point(0, 0);
                        _ErrorLabel.Size = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height);
                        _ErrorLabel.Font = RsViewEngine.TitleFont;
                        _ErrorLabel.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
                        
                        this.Controls.Add(_ErrorLabel);
                        _ErrorLabel.BringToFront();
                        
                        this.ResumeLayout();
                        
                        WindowManagement.SetParent(_ChildHandle, 0);
                    }
                
                #endregion
                
                #region PROTECTED METHODS:
                protected override void WndProc(ref Message m) {
                        switch (m.Msg) {
                                case Messaging.WM_REGISTERME:
                                        _ChildHandle = new IntPtr((int)m.WParam);
                                        Messaging.PostMessage(_ChildHandle, Messaging.WM_REGISTERACCEPTED, 0, 0);
                                    break;
                                
                                case Messaging.WM_SUICIDED:
                                        _DisplayErrorLabel();
                                    break;
                                    
                                case RSAppMessages.WM_GROUP:
                                        _GroupsOn = (int)(m.WParam) == RSAppMessages.EX_SHOW;
                                    break;
                                    
                                case RSAppMessages.WM_ZOOM:
                                        _Zoom = (int)(m.LParam);
                                    break;
                                    
                                case RSAppMessages.WM_UI:
                                        _Offline = RSAppMessages.CheckFlag((uint)m.WParam, RSAppMessages.FLAG_OFFLINE);
                                        _Opened = RSAppMessages.CheckFlag((uint)m.WParam, RSAppMessages.FLAG_OPENED);
                                        _GroupsOn = RSAppMessages.CheckFlag((uint)m.WParam, RSAppMessages.FLAG_GROUP);
                                        
                                        if (UIInfoGet != null) UIInfoGet(this);
                                    break;
                                
                                default:
                                        base.WndProc(ref m);
                                    break;
                            }
                    }
                
                #endregion
                
                #region PUBLIC METHODS:
                public void OpenReport() {
                        if (File.Exists(_ReportFile)) {
                                CHostApplication lHostApp = new CHostApplication(this);
                                
                                _ChildProcess = lHostApp.StartEmbeddedApp(
                                            System.Windows.Forms.Application.ExecutablePath,
                                            " \"" + _ReportFile + "\" \"" + this.Title + "\""
                                        );
                                RsViewEngine.ChildApplications.AddChildApplication(_ChildProcess);
                                _ChildProcess.Exited += new EventHandler(EH_ChildExited);
                            } else {
                                CRSMessageBox.ShowBox(
                                        RsViewEngine.Locale.GetMessage("rptNotFound") +
                                            "\n" + _ReportFile,
                                        RsViewEngine.Locale.GetMessageTitle("rptNotFound"),
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error
                                    );
                            }
                    }
                    
                public void RefreshReport() {
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_REFRESH, 0, 0);
                    }
                    
                public void SwitchToOffline() {
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_OFFLINE, 0, 0);
                    }
                    
                public void PrintReport() {
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_PRINT, 0, 0);
                    }
                    
                public void ExportToPDF() {
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_EXPORT, RSAppMessages.EX_PDF, 0);
                    }
                
                public void ExportToWord() {
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_EXPORT, RSAppMessages.EX_DOC, 0);
                    }

                public void ExportToExcel() {
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_EXPORT, RSAppMessages.EX_XLS, 0);
                    }
                    
                public void ExportToExcelDataOnly() {
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_EXPORT, RSAppMessages.EX_XLS_DATAONLY, 0);
                    }

                public void ExportToXML() {
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_EXPORT, RSAppMessages.EX_XML, 0);
                    }

                public void ExportToHTML() {
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_EXPORT, RSAppMessages.EX_HTM32, 0);
                    }
                    
                public void ExportToHTML40() {
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_EXPORT, RSAppMessages.EX_HTM40, 0);
                    }
                    
                public void RptZoomIn() {
                        Zoom += ZOOM_INCREMENT;
                    }
                    
                public void RptZoomOut() {
                        Zoom -= ZOOM_INCREMENT;
                    }
                    
                public void RptZoomFitWidth() {
                        Zoom = ZOOM_FITWIDTH;
                    }
                    
                public void RptZoomFitWindow() {
                        Zoom = ZOOM_FITWINDOW;
                    }
                    
                public void RptZoom(int aPercentage) {
                        switch (aPercentage) {
                                case 1: case 2:
                                        _Zoom = aPercentage;
                                    break;
                                    
                                default:
                                        _Zoom = aPercentage < 25 ? 25 : (aPercentage / 25) * 25;
                                    break;
                            }
                            
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_ZOOM, RSAppMessages.EX_SET, _Zoom);
                    }
                
                public void SendEmail() {
                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_MAIL, 0, 0);
                    }
                    
                public void ShowPage(TShowPage aPage) {
                        switch (aPage) {
                                case TShowPage.spFirst:
                                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_THUMB, RSAppMessages.EX_FIRST, 0);
                                    break;
                                case TShowPage.spPrev:
                                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_THUMB, RSAppMessages.EX_PREV, 0);
                                    break;
                                case TShowPage.spNext:
                                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_THUMB, RSAppMessages.EX_NEXT, 0);
                                    break;
                                case TShowPage.spLast:
                                        Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_THUMB, RSAppMessages.EX_LAST, 0);
                                    break;
                            }
                    }
                    
                public void ShowGroups(bool aShow) {
                        if (aShow) 
                                Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_GROUP, RSAppMessages.EX_SHOW, 0);
                            else
                                Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_GROUP, RSAppMessages.EX_HIDE, 0);
                    }
                
                public void RequestUIState() { Messaging.PostMessage(_ChildHandle, RSAppMessages.WM_UI, 0, 0); }
                
                public bool GetGroupPanelState() {
                        Messaging.SendMessage(_ChildHandle, RSAppMessages.WM_GROUP, RSAppMessages.EX_GET, 0);
                        System.Windows.Forms.Application.DoEvents();
                        return _GroupsOn;
                    }
                    
                public int GetZoomFactor() {
                        Messaging.SendMessage(_ChildHandle, RSAppMessages.WM_ZOOM, RSAppMessages.EX_GET, 0);
                        System.Windows.Forms.Application.DoEvents();
                        return _Zoom;
                    }

                public CRSReportViewerHost() {
                        _Zoom = 100;
                        _Page = new CPageSelectorPage(this);
                        this.BackColor = Color.White;
                        this.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
                        this._Page.CloseNotify += new PageCloseNotify(this.EH_ClosePage);
                        this._ChildHandle = IntPtr.Zero;
                        this.Resize += new EventHandler(EH_Resize);
                    }
            
                #endregion
            
                #region EVENT HANDLER METHODDS:
                protected void EH_Resize(object aSender, EventArgs aEArgs) {
                        if (_ChildHandle != IntPtr.Zero) {
                                Messaging.PostMessage(
                                        _ChildHandle,
                                        Messaging.WM_PARENTRESIZED,
                                        320,
                                        240
                                    );
                            }
                    }
                    
                protected void EH_ChildExited(object aSender, EventArgs aEArgs) {
                        _DisplayErrorLabel();
                    }
                
                protected bool EH_ClosePage(CPageSelectorPage aPage, bool aCloseAble) {
                        RsViewEngine.ChildApplications.TerminateApplication(_ChildProcess);
                        return true;
                    }
                
                #endregion				
            }



        internal class CRSReportViewerEmbedded: Panel {		
                    
                #region FIELDS:
                #region PRIVATE FIELDS:
                private int _Zoom;
                private int _timeout;
                private string _RptFile, _PdfFile;
                private Label _lDisplay;
                private Button _lOk;
                private SaveFileDialog _dlgExport;
                private System.Timers.Timer _Timer;
                private CRSParamEditor _ParamEditor;
                private CAcroPDFCtl _pdfCtl;
                private System.Timers.Timer _offlineTimer;
                //private ReportDocument _Document;
                
                #endregion
    
                #region PROTECTED FIELDS:
                protected bool opened, _groupState, _offline;
                protected DialogResult lastDialogResult = DialogResult.Cancel;
                protected CrystalReportViewer ctlViewer;
                protected internal CdlgSetupLogin dlgSetupLogin;
                
                #endregion
                
                #region PUBLIC FIELDS:
                public string ReportTitle;
            
                #endregion
                #endregion
                //--------------------------------------------------------------------------------------------------------------

                #region PROPERTIES:				
                #region PUBLIC PROPERTIES:
                public RsReportProvider ReportProvider { get; protected set; }

                public CEmbeddedApplication EmbeddedApplication;

                public string ReportFile {
                        get { return _RptFile; }
                        set {
                                if (!opened)
                                        _RptFile = value;
                            }
                    }

                public bool IsOpened { get { return opened; }}

                public bool GroupShow { get { return opened ? _groupState : false; }}

                public bool Offline { get { return _offline; }}	
                
                #endregion
                #endregion
                //--------------------------------------------------------------------------------------------------------------
            
                #region METHODS:
                #region PRIVATE METHODS:
                private int calcStatus(float aMax, float aVal, int aScale) {
                        float lResult = (aVal / aMax) * aScale;
                        return (int)(Math.Round(lResult));
                    }
                
                private void hideDisplay() {
                        _lDisplay.Visible = false;
                        _lOk.Visible = false;
                    }
                
                private void createCrystalViewer() {
                        ctlViewer = new CrystalReportViewer();
                        this.SuspendLayout();
                        ctlViewer.Location = new Point(0, 0);
                        ctlViewer.Size = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height);
                        ctlViewer.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
                        ctlViewer.ToolPanelView = ToolPanelViewType.None;
                        ctlViewer.ShowLogo = false;
                        ctlViewer.ShowPageNavigateButtons = false;
                        ctlViewer.DisplayStatusBar = false;
                        ctlViewer.DisplayToolbar = false;
                        ctlViewer.ForeColor = Color.FromArgb(0xff, 0x23, 0x23, 0xff); 
                        ctlViewer.BackColor = Color.FromArgb(0xff, 0x23, 0x23, 0xff);
                        this.Controls.Add(ctlViewer);
                        this.ResumeLayout();
                        
                        _lOk = new Button();
                        _lOk.Size = new Size(256, 64);
                        _lOk.Visible = false;
                        _lOk.BackColor = SystemColors.ButtonFace;
                        _lDisplay.Controls.Add(_lOk);
                    } 
                    
                private void updateDisplay(string aStr) {
                        _lDisplay.Text = aStr;
                        _lDisplay.Invalidate();
                    }

                private void addRptToViewer() {
                        DoSendUIState();
                        _lDisplay.Text = RsViewEngine.Locale.GetTagText("wait_Build");
                        
                        if (RsViewEngine.ProfileManager.Profile.Settings.EmbeddedParamEdit.Enabled && ReportProvider.ParamCount > 0) {
                                _ParamEditor.ParamFields = ReportProvider.Document.ParameterFields;
                                _ParamEditor.Visible = true; 
                            } else {
                                activateReport();
                            }
                            
                        _offline = false;
                        DoSendUIState();
                    } 
                                
                #endregion
                
                
                #region PROTECTED METHODS:
                protected void activateOfflineTimer() {
                                #if DEBUG
                                _timeout = 30;
                                #else
                                _timeout = RsViewEngine.Settings.OfflineSettings.Timeout * 60;
                                #endif
                        _offlineTimer.Enabled = RsViewEngine.ProfileManager.Profile.Settings.Offline.Enabled;
                    }
                    
                protected void activateReport() {
                  ctlViewer.ReportSource = ReportProvider.Document;
                        ctlViewer.MouseMove += new MouseEventHandler(ehMouseMove);
                        
                        activateOfflineTimer();
                    }
                
                protected int getGroupState() {
                        return opened ? (_groupState ? RSAppMessages.EX_SHOW : RSAppMessages.EX_HIDE) : RSAppMessages.EX_HIDE;
                    }
                    
                protected bool isFirstPage() {
                        return opened ? ctlViewer.GetCurrentPageNumber() < 2 : true;
                    }
                    
                protected void showOkOffline() {
                        _lOk.Visible = true;
                        _lOk.Location = new Point((this.Width - _lOk.Width) / 2, 2 * (this.Height / 3));
                        _lOk.Text = RsViewEngine.Locale.GetTagText("ok");
                        _lOk.Click += ehOffLineOk;
                        _lOk.Font = _lDisplay.Font;
                        _lOk.BringToFront();
                    }

                protected string ShowExportDialog(RsExportFormat aFormat) {
                        XmlNode lGeneralDlg = RsViewEngine.Locale.GetDialogData("generalDialogs");
                        string lFilter;
                        
                        switch (aFormat) {
                                case RsExportFormat.ExcelDocument:
                                        lFilter = FileDialogFilters.BuildExcelFilter(lGeneralDlg); break;
                                        
                                case RsExportFormat.HtmlDocument:
                                        lFilter = FileDialogFilters.BuildHTMLFilter(lGeneralDlg); break;
                
                                case RsExportFormat.WordDocument:
                                        lFilter = FileDialogFilters.BuildWordFilter(lGeneralDlg);break;
                                        
                                case RsExportFormat.XmlDocument:
                                        lFilter = FileDialogFilters.BuildXMLFilter(lGeneralDlg);break;
                                        
                                default:
                                        lFilter = FileDialogFilters.BuildPDFFilter(lGeneralDlg); break;
                            }
                            
                        _dlgExport.Filter = lFilter;
                        
                        return _dlgExport.ShowDialog() == DialogResult.OK ? _dlgExport.FileName : null;
                    }

                #region ShowAuthDialog()
                /// <summary>
                /// Returns true, if connection logon accepted by user, owtherwise false.
                /// If user requires the authentication info also stored
                /// </summary>
                /// <param name="aConnectionInfo">The connection info</param>
                /// <returns>True if dialog accepted, false if not</returns>
                protected bool ShowAuthDialog(string aConnectionName, InternalConnectionInfo aConnectionInfo) {
                        dlgSetupLogin.ConnectionName = aConnectionName;
                                                            
                        if (dlgSetupLogin.ShowDialog() == DialogResult.OK) {									
                        
                                if (dlgSetupLogin.Authenticate)
                                        aConnectionInfo.SetLogon(dlgSetupLogin.UserName, dlgSetupLogin.Password);
                                                
                                switch (dlgSetupLogin.SaveMode) {
                                        case TLogonInfoSave.lisSaveForDataSource:
                                                if (dlgSetupLogin.Authenticate) {
                                                        CSecurityData.AddSecNode(
                                                                    RsViewEngine.RSSecurity,
                                                                    dlgSetupLogin.ConnectionName,
                                                                    "DatasourceSecurity",
                                                                    dlgSetupLogin.UserName,
                                                                    dlgSetupLogin.Password
                                                                );
                                                        RsViewEngine.SaveSecurity();
                                                    }
                                            break;
                                        case TLogonInfoSave.lisSaveForReport:
                                                if (dlgSetupLogin.Authenticate) {
                                                        CSecurityData.AddSecNode(
                                                                    RsViewEngine.RSSecurity,
                                                                    dlgSetupLogin.ConnectionName,
                                                                    "report",
                                                                    dlgSetupLogin.UserName,
                                                                    dlgSetupLogin.Password,
                                                                    _RptFile
                                                                );
                                                        RsViewEngine.SaveSecurity();
                                                    }
                                            break;
                                    }
                                    
                                return true;
                                
                            } else
                                return false;
                                
                    }
                #endregion

                protected override void WndProc(ref Message m) {
                        bool lDecTimeout = false;
                
                        switch (m.Msg) {
                                case Messaging.WM_REGISTERACCEPTED:
                                        _Timer.Enabled = true;
                                    break;
                        
                                case Messaging.WM_PARENTRESIZED:
                                        if (EmbeddedApplication != null) {
                                                Rect lRect;
                                                WindowManagement.GetClientRect(EmbeddedApplication.Handle, out lRect);
                                                this.Size = new Size(lRect.Width, lRect.Height);
                                            }
                                    break;
                                    
                                case RSAppMessages.WM_PRINT:
                                        if (opened)
                                                if (!_offline)
                                                        ctlViewer.PrintReport();
                                                    else
                                                        _pdfCtl.Print();
                                    break;
                                    
                                case RSAppMessages.WM_REFRESH:
                                        DoRefresh();
                                    break;
                                    
                                case RSAppMessages.WM_MAIL:
                                        DoSendEmail();
                                    break;
                                    
                                case RSAppMessages.WM_OFFLINE:
                                        if (_offline) DoOnline(); else DoOffline();
                                    break;
                                    
                                case RSAppMessages.WM_THUMB:
                                        switch((int)(m.WParam)) {
                                                case RSAppMessages.EX_FIRST: DoFirstPage(); break;
                                                case RSAppMessages.EX_PREV: DoPrevPage(); break;
                                                case RSAppMessages.EX_NEXT: DoNextPage(); break;
                                                case RSAppMessages.EX_LAST: DoLastPage(); break;
                                            }
                                    break;
                                    
                                case RSAppMessages.WM_ZOOM:
                                        switch ((int)(m.WParam)) {
                                                case RSAppMessages.EX_SET:
                                                        DoZoom((int)(m.LParam));
                                                    break;
                                                case RSAppMessages.EX_GET:
                                                        Messaging.PostMessage(EmbeddedApplication.Handle, RSAppMessages.WM_ZOOM, RSAppMessages.EX_SET, _Zoom);
                                                    break;
                                            }
                                    break;
                                    
                                case RSAppMessages.WM_GROUP:
                                        switch ((int)(m.WParam)) {
                                                case RSAppMessages.EX_SHOW:
                                                        DoShowGroup(true);
                                                    break;
                                                case RSAppMessages.EX_HIDE:
                                                        DoShowGroup(false);
                                                    break;
                                                case RSAppMessages.EX_GET:
                                                        
                                                        Messaging.PostMessage(EmbeddedApplication.Handle, RSAppMessages.WM_GROUP, (uint)getGroupState(), 0);
                                                    break;
                                            }
                                        
                                    break;
                                    
                                case RSAppMessages.WM_UI:
                                        uint aFlags = RSAppMessages.EncodeEmbedded(this);
                                        Messaging.PostMessage(EmbeddedApplication.Handle, RSAppMessages.WM_UI, aFlags, 0);
                                    break;
                                    
                                case RSAppMessages.WM_EXPORT:
                                        switch ((int)(m.WParam)) {
                                                case RSAppMessages.EX_PDF:
                                                        DoExportPDF();
                                                    break;
                                                case RSAppMessages.EX_XLS:
                                                        DoExportExcel();
                                                    break;
                                                case RSAppMessages.EX_XLS_DATAONLY:
                                                        DoExportToExcelDataOnly();
                                                    break;
                                                case RSAppMessages.EX_DOC:
                                                        DoExportWord();
                                                    break;
                                                case RSAppMessages.EX_XML:
                                                        DoExportXML();
                                                    break;
                                                case RSAppMessages.EX_HTM32:
                                                        DoExportHTML32();
                                                    break;
                                                case RSAppMessages.EX_HTM40:
                                                        DoExportHTML40();
                                                    break;
                                            }
                                    break;
                                    
                                default:
                                        lDecTimeout = true;
                                        base.WndProc(ref m);
                                    break;
                            }
                            
                        if (!lDecTimeout)
                                _timeout = RsViewEngine.ProfileManager.Profile.Settings.Offline.Timeout * 60;
                    }
                    
                #endregion
                
                
                #region PUBLIC METHODS:
                public void DoFirstPage() {
                        if (opened) 
                                if (!_offline)
                                        ctlViewer.ShowFirstPage();
                                    else
                                        _pdfCtl.FirstPage();
                    }
                    
                public void DoNextPage() {
                        if (opened)
                                if (!_offline)
                                        ctlViewer.ShowNextPage();
                                    else
                                        _pdfCtl.NextPage();
                    }
                    
                public void DoPrevPage() {
                        if (opened)
                                if (!_offline)
                                        ctlViewer.ShowPreviousPage();
                                    else
                                        _pdfCtl.PrevPage();
                    }
                    
                public void DoLastPage() {
                        if (opened)
                                if (!_offline)
                                        ctlViewer.ShowLastPage();
                                    else
                                        _pdfCtl.LastPage();
                    }
                
                public void DoExportPDF() {
                        string lFileName = ShowExportDialog(RsExportFormat.PortableFormatDocument);
                        if (lFileName != null) {
                                ReportProvider.ReportExporter = new RsRptToPortabeFormatDocumentExporter(ReportProvider);
                                ReportProvider.ExportReport(lFileName);
                            }
                    }

                public void DoExportExcel() {
                        string lFileName = ShowExportDialog(RsExportFormat.ExcelDocument);
                        if (lFileName != null) {
                                ReportProvider.ReportExporter = new RsRptToExcelExporter(ReportProvider);
                                ReportProvider.ExportReport(lFileName);
                            }
                    }
                    
                public void DoExportToExcelDataOnly() {
                        string lFileName = ShowExportDialog(RsExportFormat.ExcelDocument);
                        if (lFileName != null) {
                                ReportProvider.ReportExporter = new RsRptToExcelExporter(ReportProvider);
                                ((RsRptToExcelExporter)(ReportProvider.ReportExporter)).DataOnly = true;
                                ReportProvider.ExportReport(lFileName);
                            }
                    }
                    
                public void DoExportWord() {
                        string lFileName = ShowExportDialog(RsExportFormat.PortableFormatDocument);
                        if (lFileName != null) {
                                ReportProvider.ReportExporter = new RsRptToWordExporter(ReportProvider);
                                ReportProvider.ExportReport(lFileName);
                            }	
                    }
                    
                public void DoExportXML() {
                        string lFileName = ShowExportDialog(RsExportFormat.PortableFormatDocument);
                        if (lFileName != null) {
                                ReportProvider.ReportExporter = new RsRptToXmlExporter(ReportProvider);
                                ReportProvider.ExportReport(lFileName);
                            }		
                    }
                    
                public void DoExportHTML40() {
                        string lFileName = ShowExportDialog(RsExportFormat.PortableFormatDocument);
                        if (lFileName != null) {
                                ReportProvider.ReportExporter = new RsRptToHtmlExporter(ReportProvider);
                                ((RsRptToHtmlExporter)(ReportProvider.ReportExporter)).HtmlVersion = RsHtmlVersion.Html40;
                                ReportProvider.ExportReport(lFileName);
                            }	
                    }
                    
                public void DoExportHTML32() {
                        string lFileName = ShowExportDialog(RsExportFormat.PortableFormatDocument);
                        if (lFileName != null) {
                                ReportProvider.ReportExporter = new RsRptToHtmlExporter(ReportProvider);
                                ((RsRptToHtmlExporter)(ReportProvider.ReportExporter)).HtmlVersion = RsHtmlVersion.Html32;
                                ReportProvider.ExportReport(lFileName);
                            }	
                    }
                    
                public void DoRefresh() {
                        if (opened) 
                                if (!_offline) {
                                        if (RsViewEngine.ProfileManager.Profile.Settings.EmbeddedParamEdit.Enabled)
                                                addRptToViewer();
                                            else
                                                ctlViewer.RefreshReport();
                                    } else {
                                        DoOnline();
                                    }
                    }
                    
                public void DoShowGroup(bool aShow) {
                        _groupState = aShow;
                        
                        if (!_offline) {
                                if (!aShow)
                                        ctlViewer.ToolPanelView = ToolPanelViewType.None;
                                    else
                                        ctlViewer.ToolPanelView = ToolPanelViewType.GroupTree;
                            } else
                                _pdfCtl.ShowBookmarks = aShow;
                    }
                    
                public void DoZoom(int aPercentage) {
                        if (opened) 
                                if (!_offline) {
                                        _Zoom = aPercentage;
                                        ctlViewer.Zoom(aPercentage);
                                    } else {
                                        switch (aPercentage) {
                                                case 1: _pdfCtl.FitWidth(); break;
                                                case 2: _pdfCtl.FitPage(); break;
                                                default: _pdfCtl.SetZoom(aPercentage); break;
                                            }
                                    }
                    }
                
                public void DoSendEmail() {
                        if (_offline) {
                                CMapiMail lMail = new CMapiMail();
                                lMail.Attachments += _PdfFile;
                                lMail.StartMail();
                                
                            } else {
                                if (RsViewEngine.dlgSendMail.ShowDialog() == DialogResult.OK) {
                                        int lStepCount = 0, lDone = 0;
                                        IRSStatusBox lStatus = CRSMessageBox.ShowStatusBox(RsViewEngine.Locale.GetMessage("sendMail"));
                                
                                        RsReportMailSender lSender = new RsReportMailSender(ReportProvider);
                                
                                        lStepCount += (RsViewEngine.dlgSendMail.DOC) ? 1 : 0;
                                        lStepCount += (RsViewEngine.dlgSendMail.HTM) ? 1 : 0;
                                        lStepCount += (RsViewEngine.dlgSendMail.PDF) ? 1 : 0;
                                        lStepCount += (RsViewEngine.dlgSendMail.XLS) ? 1 : 0;
                                        lStepCount += (RsViewEngine.dlgSendMail.XML) ? 1 : 0;
                                    
                                        if (lStepCount == 0) lStepCount = 1;
                                    
                                        if (RsViewEngine.dlgSendMail.DOC) {
                                                lSender.AddAttachment(RsExportFormat.WordDocument);
                                                lDone++;
                                                lStatus.SetStatus(calcStatus(lStepCount, lDone, 100));
                                            }
                                    
                                        if (RsViewEngine.dlgSendMail.HTM) {
                                                lSender.AddAttachment(RsExportFormat.HtmlDocument);
                                                lDone++;
                                                lStatus.SetStatus(calcStatus(lStepCount, lDone, 100));
                                            }

                                        if (RsViewEngine.dlgSendMail.PDF) {
                                                lSender.AddAttachment(RsExportFormat.PortableFormatDocument);
                                                lDone++;
                                                lStatus.SetStatus(calcStatus(lStepCount, lDone, 100));
                                            }
                                    
                                        if (RsViewEngine.dlgSendMail.XLS) {
                                                lSender.AddAttachment(RsExportFormat.ExcelDocument);
                                                lDone++;
                                                lStatus.SetStatus(calcStatus(lStepCount, lDone, 100));
                                            }

                                        if (RsViewEngine.dlgSendMail.XML) {
                                                lSender.AddAttachment(RsExportFormat.XmlDocument);
                                                lDone++;
                                                lStatus.SetStatus(calcStatus(lStepCount, lDone, 100));
                                            }
                                
                                        lSender.Send();
                                
                                        lStatus.SetStatus(100);
                                        lStatus.HideBox();
                                    }
                            }
                    }
                
                public void DoOnline() {
                        if (opened && _offline) {
                                _pdfCtl.Dispose();
                                _pdfCtl = null;
                                _offline = false;
                                if (_lOk != null)
                                        _lOk.Visible = false;
                                _lDisplay.Visible = true;
                                _lDisplay.BringToFront();
                                        
                                OpenReport();
                            }
                    }
                
                public void DoOffline() {
                        if (opened && !_offline) {
                                _Timer.Enabled = false;
                        
                                _offline = true;
                                _lDisplay.Visible = true;
                                updateDisplay(RsViewEngine.Locale.GetMessage("status_swoffline"));
                                System.Windows.Forms.Application.DoEvents();
                                
                                _PdfFile = FileSystem.GetTempFileName(RsApplicationInfo.ApplicationName, ".pdf");
                                RsRptToPortabeFormatDocumentExporter lExporter = new RsRptToPortabeFormatDocumentExporter(ReportProvider);
                                lExporter.Export(_PdfFile);
                                
                                _pdfCtl = new CAcroPDFCtl();
                                _pdfCtl.BeginInit();
                                this.Controls.Add(_pdfCtl);
                                _pdfCtl.Size = new Size(
                                            this.ClientRectangle.Width,
                                            this.ClientRectangle.Height
                                        );
                                _pdfCtl.Anchor = Anchor = ((System.Windows.Forms.AnchorStyles)((
                                            (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                            (System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)
                                        )));
                                _pdfCtl.EndInit();
                                _pdfCtl.Show();
                                
                                _pdfCtl.LoadFile(_PdfFile);
                                _pdfCtl.ShowToolBar(false);
                                _pdfCtl.ShowBookmarks = _groupState;
                                
                                ctlViewer.Visible = false;
                                ReportProvider.Close();
                                updateDisplay(RsViewEngine.Locale.GetMessage("status_nowoffline"));
                                showOkOffline();
                                
                                _Timer.Enabled = true;
                            }
                    }
                    
                public void DoSendUIState() {
                        uint aFlags = RSAppMessages.EncodeEmbedded(this);
                        Messaging.PostMessage(EmbeddedApplication.Handle, RSAppMessages.WM_UI, aFlags, 0);
                    }

                public void OpenReport() {
                        try {
                                updateDisplay(RsViewEngine.Locale.GetTagText("wait_Init"));
                                createCrystalViewer();
                                //_Document.RefreshReport += new EventHandler(EH_RefreshReport);

                                opened = true;
                        
                                updateDisplay(RsViewEngine.Locale.GetTagText("wait_Open") + "\n" + _RptFile);
                                ReportProvider = new RsReportProvider(_RptFile);
                                
                            } catch (Exception E) {
                                RsViewEngine.TreatCriticalError(E);
                            }
                        
                        /// <summary>
                        ///   Collecting and if possible setting up datasource logon info
                        /// </summary>
                        updateDisplay(RsViewEngine.Locale.GetTagText("wait_DSBuild"));
                        
                        List<InternalConnectionInfo> lConnections = ReportProvider.GetAllConnections();
                        
                        foreach (InternalConnectionInfo iConnInfo in lConnections) {
                                string lConnectionName = iConnInfo.DatabaseName == "" ?
                                            iConnInfo.ServerName :
                                            iConnInfo.DatabaseName;
                                
                                if (FileFormats.IsExcel(lConnectionName)) { continue; }
                                
                                CSecurityNode lSec = RsViewEngine.RSSecurity.GetSecurityNode("report", lConnectionName, _RptFile);
                                if (lSec == null)
                                        lSec = RsViewEngine.RSSecurity.GetSecurityNode("DatasourceSecurity", lConnectionName);
                                
                                if (lSec != null) {
                                        iConnInfo.SetLogon(lSec.UserName, lSec.Password);
                                                
                                    } else {
                                        if (!ShowAuthDialog(lConnectionName, iConnInfo)) 
                                                System.Windows.Forms.Application.Exit();
                                    }
                            }
                                    
                        addRptToViewer();
                        hideDisplay();
                    }

                public void KillOrphaned() {
                        _Timer.Enabled = false;
                
                        Form lForm = new Form();
                        WindowManagement.SetParent(this.Handle, (int)(lForm.Handle));
                        System.Environment.Exit(0);
                    }

                #endregion


                #region CONSTRUCTORS:
                public CRSReportViewerEmbedded() {
                        opened = false;
                        _groupState = false;
                        _offline = true;
                        dlgSetupLogin = RsViewEngine.dlgSetupLogin;
                        _dlgExport = RsViewEngine.dlgExport;
                        
                        this.SuspendLayout();
                        
                        _lDisplay = new Label();
                        _lDisplay.BackColor = Color.White;
                        _lDisplay.Location = new Point(0, 0);
                        _lDisplay.Size = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height);
                        _lDisplay.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
                        _lDisplay.Name = "lDisplay";
                        _lDisplay.Font = RsViewEngine.TitleFont;
                        _lDisplay.TextAlign = ContentAlignment.MiddleCenter;
                        
                        _ParamEditor = new CRSParamEditor();
                        _ParamEditor.Location = new Point(0, 0);
                        _ParamEditor.Size = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height);
                        _ParamEditor.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
                        _ParamEditor.Visible = false;
                        _ParamEditor.BackColor = Color.White;
                        _ParamEditor.HeaderColor = ReportSmart.Controls.ControlProperties.ColorItemInBack();
                        _ParamEditor.AcceptParams += ehParamsAccepted;
                        
                        this.Controls.Add(_lDisplay);
                        this.Controls.Add(_ParamEditor);
                                        
                        this.ResumeLayout();
                        _Timer = new System.Timers.Timer(1000);
                        _Timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                        _Timer.Enabled = false;
                        
                        _offlineTimer = new System.Timers.Timer(1000);
                        _offlineTimer.Enabled = false;
                        _offlineTimer.Elapsed += new ElapsedEventHandler(ehOfflineTimer);
                    }
                    
                #endregion


                #region DESTRUCTORS:
                #endregion
                #endregion
                //--------------------------------------------------------------------------------------------------------------

                #region EVENT HANDLERS:
                protected void OnTimedEvent(object aSender, ElapsedEventArgs aEArgs) {
                        try {
                                if (!WindowManagement.IsWindow(EmbeddedApplication.Handle))
                                        KillOrphaned();
                            } catch {
                                KillOrphaned();
                            }
                    }
                    
                protected void ehParamsAccepted(object aSender, EventArgs aEArgs) {
                        _ParamEditor.Visible = false;
                         activateReport();
                    }
                    
              protected void ehOfflineTimer(object aSender, ElapsedEventArgs aE) {
                        if (WindowHelper.IsThereWindow("Enter Parameter Values")) {
                                #if DEBUG
                                _timeout = 30;
                                #else
                                _timeout = RsViewEngine.Settings.OfflineSettings.Timeout * 60;
                                #endif
                                
                            } else {
                                _timeout--;
                                if (_timeout <= 0) {
                                        _offlineTimer.Enabled = false;
                                        Messaging.PostMessage(this.Handle, RSAppMessages.WM_OFFLINE, 0, 0);
                                    }
                            }
                    }
                    
                protected void ehMouseMove(object aSender, MouseEventArgs aE) {
                        _timeout = RsViewEngine.ProfileManager.Profile.Settings.Offline.Timeout * 60;
                    }			        
                    
                protected void ehOffLineOk(object aSender, EventArgs aEArgs) {
                        hideDisplay();
                    }
                    
                #endregion
                //--------------------------------------------------------------------------------------------------------------
                    
            }
    }
