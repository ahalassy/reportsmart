/*
 *
 * Licensing:			GPL
 * Original project:	bin.csproj
 *
 * Copyright: Adam Halassy (2009.10.05.)
 * 
 * 
 */

using Halassy;
using Halassy.Controls;
using Halassy.Controls.Forms;
using Halassy.Localization;
using ReportSmart;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ReportSmart.Controls {

		internal class CdlgNewSMTPProfile: CWizardDialog {
				
				private class CSMTPProfGlobal: CWizardPanel {
						private Label _lProfileName;
						private CExtendedTextBox _eProfileName;
						
						public override void ResetPage() {
								_eProfileName.Text = "";
							}
						
						public string ProfileName { get { return _eProfileName.Text; }}
						
						public override string GetInstanceName() { return "CSMTPProfGlobal"; }
						public override void ApplyLocale(CLocalization aLocale) {
								base.ApplyLocale(aLocale);
								
								_lProfileName.Text = XmlTools.getXmlNodeByName(_lProfileName.Name, LocaleData).InnerText + ":";
								_eProfileName.ContentHelp = "[" + XmlTools.getXmlNodeByName(_eProfileName.Name, LocaleData).InnerText + "]";
							}
						
						public CSMTPProfGlobal(): base() {
								this.Name = "Global";
								
								this.SuspendLayout();
								
								_lProfileName = new Label();
								_lProfileName.Name = "lProfileName";
								_lProfileName.Location = new Point(lDescription.Left + ControlProperties.ButtonWidth, lDescription.Bottom + ControlProperties.ControlSpacing);
								_lProfileName.Size = ControlProperties.TextCtlSize(); 
								
								_eProfileName = new CExtendedTextBox();
								_eProfileName.Name = "eProfileName";
								_eProfileName.Location = new Point(_lProfileName.Left, _lProfileName.Bottom);
								_eProfileName.Size = ControlProperties.TextCtlSize(); 
								
								this.Controls.Add(_lProfileName);
								this.Controls.Add(_eProfileName);

								this.ResumeLayout();
							}
							
						protected override void EH_ControlChanged(object aSender, System.EventArgs aEArgs) {
								HostDialog.PageIsOk = _eProfileName.Text != "";
							}
					}
					
				private class CSMTPProfUser: CWizardPanel {
						private Label _lName, _lEmail;
						private CExtendedTextBox _eName, _eEmail;
						
						public string PersonalName { get { return _eName.Text; }}
						public string PersonalMail { get { return _eEmail.Text; }}
				
						public override void ResetPage() {
								_eName.Text = "";
								_eEmail.Text = "";
							}
						
						public override string GetInstanceName() { return "CSMTPProfUser"; }
						public override void ApplyLocale(CLocalization aLocale) {
								base.ApplyLocale(aLocale);
								
								_lName.Text = XmlTools.getXmlNodeByName(_lName.Name, LocaleData).InnerText + ":";
								_eName.ContentHelp = "[" + XmlTools.getXmlNodeByName(_eName.Name, LocaleData).InnerText + "]";
								_lEmail.Text = XmlTools.getXmlNodeByName(_lEmail.Name, LocaleData).InnerText + ":";
								_eEmail.ContentHelp = "[" + XmlTools.getXmlNodeByName(_eEmail.Name, LocaleData).InnerText + "]";
							}				
				
						public CSMTPProfUser(): base() {
								this.Name = "Personal";
								
								_lName = new Label();
								_lName.Name = "lName";
								_lName.Location = new Point(lDescription.Left + ControlProperties.ButtonWidth, lDescription.Bottom + ControlProperties.ControlSpacing);
								_lName.Size = ControlProperties.TextCtlSize(); 
								
								_eName = new CExtendedTextBox();
								_eName.Name = "eName";
								_eName.Location = new Point(_lName.Left, _lName.Bottom);
								_eName.Size = ControlProperties.TextCtlSize(); 
								
								_lEmail = new Label();
								_lEmail.Name = "lEmail";
								_lEmail.Location = new Point(lDescription.Left + ControlProperties.ButtonWidth, _eName.Bottom + ControlProperties.HeaderSpacing);
								_lEmail.Size = ControlProperties.TextCtlSize(); 
								
								_eEmail = new CExtendedTextBox();
								_eEmail.Name = "eEmail";
								_eEmail.Location = new Point(_lEmail.Left, _lEmail.Bottom);
								_eEmail.Size = ControlProperties.TextCtlSize(); 

								this.Controls.Add(_lName);
								this.Controls.Add(_eName);
								this.Controls.Add(_lEmail);
								this.Controls.Add(_eEmail);
							}
							
						protected override void EH_ControlChanged(object aSender, System.EventArgs aEArgs) {
								HostDialog.PageIsOk = _eName.Text != "" && _eEmail.Text != "";
							}
					}
					
				private class CSMTPProfSMTP: CWizardPanel {
						private Label _lServer, _lPort;
						private CExtendedTextBox _eServer, _ePort;
				
						public string SMTPServer { get { return _eServer.Text; }}
						public string SMTPPort {
								get { return _ePort.Text == "" ? "25" : _ePort.Text; }
							}

						public override void ResetPage() {
								_eServer.Text = "";
								_ePort.Text = "";
							}

						public override string GetInstanceName() { return "CSMTPProfSMTP"; }
						public override void ApplyLocale(CLocalization aLocale) {
								base.ApplyLocale(aLocale);
								
								_lServer.Text = XmlTools.getXmlNodeByName(_lServer.Name, LocaleData).InnerText + ":";
								_eServer.ContentHelp = "[" + XmlTools.getXmlNodeByName(_eServer.Name, LocaleData).InnerText + "]";
								_lPort.Text = XmlTools.getXmlNodeByName(_lPort.Name, LocaleData).InnerText + ":";
								_ePort.ContentHelp = "[" + XmlTools.getXmlNodeByName(_ePort.Name, LocaleData).InnerText + "]";
							}				
		
						public CSMTPProfSMTP(): base() {
								this.Name = "SMTP";
								
								_lServer = new Label();
								_lServer.Name = "lServer";
								_lServer.Location = new Point(lDescription.Left + ControlProperties.ButtonWidth, lDescription.Bottom + ControlProperties.ControlSpacing);
								_lServer.Size = ControlProperties.TextCtlSize(); 
								
								_eServer = new CExtendedTextBox();
								_eServer.Name = "eServer";
								_eServer.Location = new Point(_lServer.Left, _lServer.Bottom);
								_eServer.Size = ControlProperties.TextCtlSize(); 
								
								_lPort = new Label();
								_lPort.Name = "lPort";
								_lPort.Location = new Point(lDescription.Left + ControlProperties.ButtonWidth, _eServer.Bottom + ControlProperties.HeaderSpacing);
								_lPort.Size = ControlProperties.TextCtlSize(); 
								
								_ePort = new CExtendedTextBox();
								_ePort.Name = "ePort";
								_ePort.Location = new Point(_lPort.Left, _lPort.Bottom);
								_ePort.Size = ControlProperties.TextCtlSize(); 

								this.Controls.Add(_lServer);
								this.Controls.Add(_eServer);
								this.Controls.Add(_lPort);
								this.Controls.Add(_ePort);
							}

						protected override void EH_ControlChanged(object aSender, System.EventArgs aEArgs) {
								HostDialog.PageIsOk = _eServer.Text != "";
							}
					}
				
				private class CSMTPProfSec: CWizardPanel {
				
						private CheckBox _ctlAuth, _ctlSSL;
						private Label _lPassword, _lUserName;
						private CExtendedTextBox _eUserName, _ePassword;
				
						public string UserName { get { return _ctlAuth.Checked ? _eUserName.Text : ""; }}
						public string Password { get { return _ctlAuth.Checked ? _ePassword.Text : ""; }}
						public bool Authorize { get { return _ctlAuth.Checked; }}
						public bool SSL { get { return _ctlAuth.Checked ? _ctlSSL.Checked : false; }}

						public override void ResetPage() {
								_eUserName.Text = "";
								_ePassword.Text = "";
								_ctlAuth.Checked = false;
								_ctlSSL.Checked = false;
								
								_ctlSSL.Enabled = _ctlAuth.Checked;
								_lPassword.Enabled = _ctlAuth.Checked;
								_lUserName.Enabled = _ctlAuth.Checked;
								_eUserName.Enabled = _ctlAuth.Checked;
								_ePassword.Enabled = _ctlAuth.Checked;
							}

						public override string GetInstanceName() { return "CSMTPProfSec"; }
						public override void ApplyLocale(CLocalization aLocale) {
								base.ApplyLocale(aLocale);
								
								_ctlAuth.Text = XmlTools.getXmlNodeByName(_ctlAuth.Name, LocaleData).InnerText;
								_ctlSSL.Text = XmlTools.getXmlNodeByName(_ctlSSL.Name, LocaleData).InnerText;
								_lUserName.Text = XmlTools.getXmlNodeByName(_lUserName.Name, LocaleData).InnerText + ":";
								_eUserName.ContentHelp = "[" + XmlTools.getXmlNodeByName(_eUserName.Name, LocaleData).InnerText + "]";
								_lPassword.Text = XmlTools.getXmlNodeByName(_lPassword.Name, LocaleData).InnerText + ":";
								_ePassword.ContentHelp = "[" + XmlTools.getXmlNodeByName(_ePassword.Name, LocaleData).InnerText + "]";
								_ctlSSL.Text = XmlTools.getXmlNodeByName(_ctlSSL.Name, LocaleData).InnerText;
							}				
		
						public CSMTPProfSec(): base() {
								this.Name = "Security";
								
								_ctlAuth = new CheckBox();
								_ctlAuth.Name = "ctlAuth";
								_ctlAuth.Size = ControlProperties.TextCtlSize();
								_ctlAuth.Location = new Point(lDescription.Left + ControlProperties.ButtonWidth, lDescription.Bottom + ControlProperties.ControlSpacing);
								
								_lUserName = new Label();
								_lUserName.Name = "lUserName";
								_lUserName.Location = new Point(_ctlAuth.Left, _ctlAuth.Bottom + ControlProperties.ControlSpacing);
								_lUserName.Size = ControlProperties.TextCtlSize(); 
								
								_eUserName = new CExtendedTextBox();
								_eUserName.Name = "eUserName";
								_eUserName.Location = new Point(_lUserName.Left, _lUserName.Bottom);
								_eUserName.Size = ControlProperties.TextCtlSize(); 
								
								_lPassword = new Label();
								_lPassword.Name = "lPasswd";
								_lPassword.Location = new Point(lDescription.Left + ControlProperties.ButtonWidth, _eUserName.Bottom + ControlProperties.HeaderSpacing);
								_lPassword.Size = ControlProperties.TextCtlSize(); 
								
								_ePassword = new CExtendedTextBox();
								_ePassword.Name = "ePasswd";
								_ePassword.Location = new Point(_lPassword.Left, _lPassword.Bottom);
								_ePassword.Size = ControlProperties.TextCtlSize(); 
								_ePassword.Font = CReportSmartCore.Core.PasswordFont;
								_ePassword.PasswordChar = '—';

								_ctlSSL = new CheckBox();
								_ctlSSL.Name = "ctlSSL";
								_ctlSSL.Size = ControlProperties.TextCtlSize();
								_ctlSSL.Location = new Point(lDescription.Left + ControlProperties.ButtonWidth, _ePassword.Bottom + ControlProperties.HeaderSpacing);
								
								_ctlAuth.Click += new EventHandler(EH_AuthClick);
								
								this.Controls.Add(_lUserName);
								this.Controls.Add(_eUserName);
								this.Controls.Add(_lPassword);
								this.Controls.Add(_ePassword);
								this.Controls.Add(_ctlAuth);
								this.Controls.Add(_ctlSSL);
							}
					
						public void EH_AuthClick(object aSender, EventArgs aEArgs) {
								_ctlSSL.Enabled = _ctlAuth.Checked;
								_lPassword.Enabled = _ctlAuth.Checked;
								_lUserName.Enabled = _ctlAuth.Checked;
								_eUserName.Enabled = _ctlAuth.Checked;
								_ePassword.Enabled = _ctlAuth.Checked;
							}
					}
					
					
				private CSMTPProfGlobal _ctlGlobal;
				private CSMTPProfUser _ctlUser;
				private CSMTPProfSMTP _ctlSMTP;
				private CSMTPProfSec _ctlSec;
				
				public string ProfileName { get {return _ctlGlobal.ProfileName; }}
				public string PersonalName { get {return _ctlUser.PersonalName; }}
				public string PersonalMail { get {return _ctlUser.PersonalMail; }}
				public string SMTPServer { get {return _ctlSMTP.SMTPServer; }}
				public string SMTPPort { get { return _ctlSMTP.SMTPPort; }}
				public bool Authorize { get { return _ctlSec.Authorize; }}				
				public bool SSLAuth { get { return _ctlSec.SSL; }}
				public string UserName { get { return _ctlSec.UserName; }}
				public string Password { get { return _ctlSec.Password; }}
					
				public CdlgNewSMTPProfile(): base() {
						this.Name = "dlgNewSMTPProfile";
				
						this.SuspendLayout();
						
						_ctlGlobal = new CSMTPProfGlobal();
						_ctlUser = new CSMTPProfUser();
						_ctlSMTP = new CSMTPProfSMTP();
						_ctlSec = new CSMTPProfSec();
						
						this.AddPage(_ctlGlobal);
						this.AddPage(_ctlUser);
						this.AddPage(_ctlSMTP);
						this.AddPage(_ctlSec);
						
						this.ResumeLayout();
					}
			}
		
	}