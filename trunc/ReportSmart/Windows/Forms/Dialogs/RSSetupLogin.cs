#region Source information

//*****************************************************************************
//
//    RSSetupLogin.cs
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
using System.Drawing;
using System.Windows.Forms;
using ReportSmart.Application;
using ReportSmart.Forms;
using ReportSmart.Localization;

namespace ReportSmart.Controls
{
    internal enum TLogonInfoSave {
				lisNoSave = 0,
				lisSaveForReport = 1,
				lisSaveForDataSource = 2
			}

		internal class CdlgSetupLogin: CMyDialog {
		
				private Label _lName, _lPassword;
				private CExtendedTextBox _eName, _ePassword;
				private CheckBox _ctlSetLogon;
				private RadioButton _ctlNoSave, _ctlSaveForThisReport, _ctlSaveForDS;
				private bool _SetupLogon, _OnlyLogon = false;
				private string _Pwd, _UsrName;
				private TLogonInfoSave _LogonNfoSv;
				
				public string ConnectionName;
				
				public bool OnlyLogOn {
						get { return _OnlyLogon; }
						set { _OnlyLogon = value; }
					}
				public bool Authenticate {
						get { return _SetupLogon; }
						set {
								_SetupLogon = value;
								_ctlSetLogon.Checked = value;
							}
					}
				public string Password {
						get { return _Pwd; }
						set {
								_ePassword.Text = value;
							}
					}
				public string UserName {
						get { return _UsrName; }
						set {
								_eName.Text = value;
							}
					}
				public TLogonInfoSave SaveMode { get { return _LogonNfoSv; }}					
				
				protected bool getAuthenticate() {
						return _ctlSetLogon.Checked;
					}
					
				protected string getPassword() {
						return _ePassword.Text;
					}
					
				protected string getUserName() {
						return _eName.Text;
					}
					
				protected TLogonInfoSave getSaveMode() { 
						if (_ctlSaveForDS.Checked)
								return TLogonInfoSave.lisSaveForDataSource;
							else if (_ctlSaveForThisReport.Checked)
								return TLogonInfoSave.lisSaveForReport;
							else
								return TLogonInfoSave.lisNoSave;
					}
				
				
				protected override string buildTitle(CLocalization aLocale) {
						return XmlTools.getXmlNodeByName("title1", DialogLocale).InnerText +
									" \"" + ConnectionName + "\" " +
									XmlTools.getXmlNodeByName("title2", DialogLocale).InnerText;
					}
					
				public override string GetInstanceName() { return "CdlgSetupLogin"; }				
				public override void ApplyLocale(CLocalization aSender) {
						base.ApplyLocale(aSender);
						
						_lName.Text = XmlTools.getXmlNodeByName(_lName.Name, DialogLocale).InnerText;
						_lPassword.Text = XmlTools.getXmlNodeByName(_lPassword.Name, DialogLocale).InnerText;
						_eName.ContentHelp = XmlTools.getXmlNodeByName(_eName.Name, DialogLocale).InnerText;
						_ePassword.ContentHelp = XmlTools.getXmlNodeByName(_ePassword.Name, DialogLocale).InnerText;
						_ctlSetLogon.Text = XmlTools.getXmlNodeByName(_ctlSetLogon.Name, DialogLocale).InnerText;
						_ctlNoSave.Text = XmlTools.getXmlNodeByName(_ctlNoSave.Name, DialogLocale).InnerText;
						_ctlSaveForThisReport.Text = XmlTools.getXmlNodeByName(_ctlSaveForThisReport.Name, DialogLocale).InnerText;
						_ctlSaveForDS.Text = XmlTools.getXmlNodeByName(_ctlSaveForDS.Name, DialogLocale).InnerText;
					}
				
				public CdlgSetupLogin(): base(){
						this.Size = new Size(this.Size.Width, this.Size.Height + 64);
						this.SuspendLayout();
						this.Name = "dlgSetupLogin";
						
						_ctlSetLogon = new CheckBox();
						_ctlSetLogon.Name = "ctlSetLogon";
						_ctlSetLogon.Location = new Point(LABEL_MARGIN, CustomAreaTop);
						_ctlSetLogon.Size = new Size(CustomAreaWidth / 2, 24);
						
						_lName = new Label();
						_lName.Name = "lName";
						_lName.Location = new Point(LABEL_MARGIN, _ctlSetLogon.Top + _ctlSetLogon.Height + LABEL_MARGIN);
						_lName.Size = new Size((this.CustomAreaWidth - LABEL_MARGIN) / 2 , 14);
						
						_lPassword = new Label();
						_lPassword.Name = "lPassword";
						_lPassword.Location = new Point(LABEL_MARGIN*2 + _lName.Width, _ctlSetLogon.Top + _ctlSetLogon.Height + LABEL_MARGIN);
						_lPassword.Size = new Size((this.CustomAreaWidth - LABEL_MARGIN) / 2 , 14);
						
						_eName = new CExtendedTextBox();
						_eName.Location = new Point(LABEL_MARGIN, _lName.Location.Y + _lName.Height + LABEL_MARGIN/2);
						_eName.Size = _lName.Size;
						_eName.Name = "eName";
						
						_ePassword = new CExtendedTextBox();
						_ePassword.Location = new Point(_lPassword.Location.X, _lName.Location.Y + _lName.Height + LABEL_MARGIN/2);
						_ePassword.Size = _lPassword.Size;
						_ePassword.Name = "ePassword";
						_ePassword.Font = RsViewEngine.PasswordFont;
						_ePassword.PasswordChar = '—';
						
						_ctlNoSave = new RadioButton();
						_ctlNoSave.Location = new Point(LABEL_MARGIN * 3, _eName.Location.Y + _eName.Height + LABEL_MARGIN);
						_ctlNoSave.Name = "ctlNoSave";
						_ctlNoSave.Size = new Size(_lName.Width, 24);
						
						_ctlSaveForThisReport = new RadioButton();
						_ctlSaveForThisReport.Location = new Point(LABEL_MARGIN * 3, _ctlNoSave.Location.Y + _ctlNoSave.Height);
						_ctlSaveForThisReport.Name = "ctlSaveForThisReport";
						_ctlSaveForThisReport.Size = new Size(_lName.Width, 24);
						
						_ctlSaveForDS = new RadioButton();
						_ctlSaveForDS.Location = new Point(LABEL_MARGIN * 3, _ctlSaveForThisReport.Location.Y + _ctlSaveForThisReport.Height);
						_ctlSaveForDS.Name = "ctlSaveForDS";
						_ctlSaveForDS.Size = new Size(_lName.Width, 24);
						
						this.Controls.Add(_ctlSetLogon);
						this.Controls.Add(_lName);
						this.Controls.Add(_lPassword);
						this.Controls.Add(_eName);
						this.Controls.Add(_ePassword);
						this.Controls.Add(_ctlNoSave);
						this.Controls.Add(_ctlSaveForThisReport);
						this.Controls.Add(_ctlSaveForDS);
						
						
						this.Shown += new EventHandler(EH_Shown);
						this._ctlSetLogon.CheckStateChanged += new EventHandler(EH_ChSetLogonState);
						this._ctlNoSave.CheckedChanged += new EventHandler(EH_CtlChange);
						this._ctlSaveForDS.CheckedChanged += new EventHandler(EH_CtlChange);
						this._ctlSaveForThisReport.TextChanged += new EventHandler(EH_CtlChange);
						this._ePassword.TextChanged += new EventHandler(EH_CtlChange);
						this._eName.TextChanged += new EventHandler(EH_CtlChange);
						
						
						this.ResumeLayout();
					}
			
				protected void EH_ChSetLogonState(object aSender, EventArgs aEArgs) {
						_ePassword.Enabled = _ctlSetLogon.Checked;
						_eName.Enabled = _ctlSetLogon.Checked;
						_lPassword.Enabled = _ctlSetLogon.Checked;
						_lName.Enabled = _ctlSetLogon.Checked;
						_SetupLogon = this._ctlSetLogon.Checked;
					}
			
				protected void EH_Shown(object aSender, EventArgs aEArgs) {
				
						if (!_OnlyLogon) {
								_ePassword.Text = "";
								_eName.Text = "";
								_ctlSetLogon.Checked = false;
								_OnlyLogon = false;
							}
							
						_ctlSaveForThisReport.Enabled = !_OnlyLogon;
						_ctlSaveForDS.Enabled = !_OnlyLogon;
						_ctlNoSave.Enabled = !_OnlyLogon;
						
						_ePassword.ContentFont = _eName.Font;
						_ePassword.UpdateContentHelp();
						_eName.UpdateContentHelp();
						_ctlNoSave.Checked = true;
				
						EH_ChSetLogonState(this, aEArgs);
					}
					
				protected void EH_CtlChange(object aSender, EventArgs aEArgs) {
						_SetupLogon = getAuthenticate();
						_Pwd = getPassword();
						_UsrName = getUserName();
						_LogonNfoSv = getSaveMode();
					}
			}
	}
