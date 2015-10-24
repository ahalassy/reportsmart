#region Source information

//*****************************************************************************
//
//    RSMessageBox.cs
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
using ReportSmart.Engine;
using ReportSmart.Localization;

namespace ReportSmart.Controls
{

    internal interface IRSStatusBox {
				void SetMessage(string aMessage);
				void SetStatus(int aStatus);
				void HideBox();
			}
		
		internal class CRSMessageBox: CLocalizedForm, IRSStatusBox {
				private bool _details = false;
		
				public const int dlgbtn_WIDTH = 128;
				public const int dlgbtn_HEIGHT = 32;
				public const int dlgbtn_MARGIN = 16;
				
				protected int getNoX(int aNo) {
						return this.ClientRectangle.Width - (dlgbtn_WIDTH + dlgbtn_MARGIN) * aNo;
					}
		
				protected int getY() {
						return this.ClientRectangle.Height - dlgbtn_MARGIN - dlgbtn_HEIGHT;
					}
				
				// Just for debug causes:
				public static DialogResult ShowBox() {
						return ShowBox("This is a debugging message", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					
				// Only show a short message:
				public static DialogResult ShowBox(string aMessage) {
						return ShowBox(aMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					
				// Message with title:
				public static DialogResult ShowBox(string aMessage, string aTitle) {
						return ShowBox(aMessage, aTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					
				// Set up dialog buttons too:
				public static DialogResult ShowBox(string aMessage, string aTitle, MessageBoxButtons aMsgBoxBtns) {
						return ShowBox(aMessage, aTitle, aMsgBoxBtns, MessageBoxIcon.Information);
					}
				
				// Full set up of dialog (But no details):
				public static DialogResult ShowBox(string aMessage, string aTitle, MessageBoxButtons aMsgBoxBtns, MessageBoxIcon aIcon) {
						return ShowBox(aMessage, aTitle, aMsgBoxBtns, MessageBoxIcon.Information, "");
					}

				// Full set up of dialog:
				public static DialogResult ShowBox(string aMessage, string aTitle, MessageBoxButtons aMsgBoxBtns, MessageBoxIcon aIcon, string aDetails) {
						CRSMessageBox lMsgBox = new CRSMessageBox();
						lMsgBox.Title = aTitle;
						lMsgBox.MessageText = aMessage;
						lMsgBox.DialogButtons = aMsgBoxBtns;
						lMsgBox.DialogIcon = aIcon;
						lMsgBox.ApplyLocale(RsViewEngine.Locale);
						lMsgBox.Text = RsApplicationInfo.ApplicationName;
						lMsgBox.Details = aDetails;
						
						return lMsgBox.ShowMsgBox();
					}				
					
				// Show as statusbox:
				public static IRSStatusBox ShowStatusBox(string aTitle) {
						CRSMessageBox lMsgBox = new CRSMessageBox();
						lMsgBox.Title = aTitle;
						lMsgBox._Icon.Image = (Image)RsViewEngine.Resources.GetObject("gfx_mail_48x48");
						lMsgBox._Icon.BackColor = Color.White;
						lMsgBox.MessageText = "";
						lMsgBox.ShowAsStatusBox = true;
						lMsgBox.setLayout();
						lMsgBox.MinimizeBox = false;
						lMsgBox.MaximizeBox = false;
						lMsgBox.FormBorderStyle = FormBorderStyle.FixedDialog;
						lMsgBox.Show();
						lMsgBox.BringToFront();
						
						return lMsgBox;
					}

				private Button _bOk, _bCancel, _bYes, _bNo;
				private CProgressBar _progress;
				private Label _lTitle, _lMessage, _lDLnk;
				private PictureBox _Icon;
				private MessageBoxButtons _DlgButtons;
				private RichTextBox _lDet;
				private bool _isStatus = false;
				
				public bool ShowAsStatusBox {
						get { return _isStatus; }
						set { 
								_isStatus = value;
								if (_isStatus)
										showAsProgress();
							}
					}
					
				private void showAsProgress() {
						foreach (Control iCtl in Controls)
								if (iCtl is Button)
										iCtl.Hide();
						
						if (_progress == null)
								_progress = new CProgressBar();
								
						_progress.Location = new Point(_lMessage.Left, _lMessage.Bottom);
						_progress.Size = new Size(_lMessage.Width, 32);
						_progress.Minimum = 0;
						_progress.Maximum = 100;
						_progress.CurrentValue = 0;
						_progress.ForeColor = Color.Blue;
						_progress.ShowPercentage = true;
						_progress.ShowText = false;
						_progress.Anchor = (AnchorStyles)(AnchorStyles.Left| AnchorStyles.Right | AnchorStyles.Top);
						
						this.Controls.Add(_progress);
					}
				
				public string Title {
						set { _lTitle.Text = "          " + value; }
					}
				
				public string MessageText {
						get { return _lMessage.Text; }
						set { _lMessage.Text = value; }
					}
					
				public string Details {
						get { return _lDet.Text; }
						set { _lDet.Text = value; }
					}
				
				public MessageBoxIcon DialogIcon {
						set {
								switch (value) {
										case MessageBoxIcon.Exclamation: 
												_Icon.Image = (Image)(RsViewEngine.Resources.GetObject("gfx_warning_48x48"));	break;
												
										case MessageBoxIcon.Error: 
												_Icon.Image = (Image)(RsViewEngine.Resources.GetObject("gfx_error_48x48"));	break;
												
										case MessageBoxIcon.Question: 
												_Icon.Image = (Image)(RsViewEngine.Resources.GetObject("gfx_question_48x48"));	break;

										default:
												_Icon.Image = (Image)(RsViewEngine.Resources.GetObject("gfx_comment_48x48"));	break;
									}
									
								_Icon.BackColor = Color.White;
							}
					}
				
				public MessageBoxButtons DialogButtons {
						get { return _DlgButtons; }
						set { _DlgButtons = value; }
					}
					
				public void SetMessage(string aMessage) {
						_lMessage.Text = aMessage;
					}
					
				public void SetStatus(int aStatus) {
						_progress.CurrentValue = aStatus;
					}
					
				public void HideBox() {
						this.Hide();
						this.DestroyHandle();
					}
					
				protected void setLayout() {
						this.SuspendLayout();
						this.Controls.Clear();
						this.Controls.Add(_lTitle);
						this.Controls.Add(_lMessage);
						this.Controls.Add(_Icon);
						if (_lDet.Text != "") {
								this.Controls.Add(_lDLnk);
								this.Controls.Add(_lDet);
							}
							
						
						if (_isStatus) {
								showAsProgress();
							} else {
								switch (_DlgButtons) {
										case MessageBoxButtons.OKCancel:
												_bOk.Location = new Point(getNoX(2), getY());
												_bCancel.Location = new Point(getNoX(1), getY());
												
												this.Controls.Add(_bOk);
												this.Controls.Add(_bCancel);
											break;

										case MessageBoxButtons.YesNo:
												_bYes.Location = new Point(getNoX(2), getY());
												_bNo.Location = new Point(getNoX(1), getY());
										
												this.Controls.Add(_bYes);
												this.Controls.Add(_bNo);
											break;
									
										default: 
												_bOk.Location = new Point(getNoX(1), getY());
												
												this.Controls.Add(_bOk);
											break;
									}
								int lBWidth = this.Size.Width - this.ClientRectangle.Width;
								this.Size = new Size(
											(dlgbtn_WIDTH + dlgbtn_MARGIN) * (this.Controls.Count - 2) + dlgbtn_MARGIN + lBWidth,
											this.Size.Height
										);
										
								_lDLnk.Location = new Point(0, getY() - _lDLnk.Height);
								_lDet.Location = new Point(8, _lDLnk.Bottom + 8);
								_lDet.Size = new Size(this.Width - 16, 96); 
							}		
						
							
						_Icon.BringToFront();
						this.ResumeLayout();
					}
					
				protected Button createButton(string aName) {
						Button lResult = new Button();
						lResult.Size = new Size(dlgbtn_WIDTH, dlgbtn_HEIGHT);
						lResult.Name = aName;
						lResult.Click += new EventHandler(userDecision);
						lResult.Anchor = (AnchorStyles)(AnchorStyles.Right | AnchorStyles.Bottom);
						return lResult;
					}	
					
				protected void userDecision(object aSender, EventArgs aEArgs) {
						if (aSender == null || aSender == _bCancel)
								this.DialogResult = DialogResult.Cancel;
							else if (aSender == _bCancel)
								this.DialogResult = DialogResult.OK;
							else if (aSender == _bYes)
								this.DialogResult = DialogResult.Yes;
							else if (aSender == _bNo)
								this.DialogResult = DialogResult.No;
							else
								this.DialogResult = DialogResult.Cancel;
					}
				
				
				public override string GetInstanceName() { return "CRSMessageBox"; }
				public override void ApplyLocale(CLocalization aLocale) {
						if (aLocale != null) { 
								_bOk.Text = aLocale.GetTagText("ok");
								_bCancel.Text = aLocale.GetTagText("cancel");
								_bYes.Text = aLocale.GetTagText("yes");
								_bNo.Text = aLocale.GetTagText("no");
								_lDLnk.Text = aLocale.GetTagText("details") + " >>";
							} else { 
								_bOk.Text = "Ok";
								_bCancel.Text = "Cancel";
								_bYes.Text = "Yes";
								_bNo.Text = "No";
								_lDLnk.Text = "Details >>";
							}
					}
				
				public DialogResult ShowMsgBox() {
						setLayout();
						return ShowDialog();
					}
				
				public CRSMessageBox() {
						this.SuspendLayout();
						
						this.Size = new Size(320, 264);
						this.FormBorderStyle = FormBorderStyle.FixedDialog;
						this.MaximizeBox = false;
						this.MinimizeBox = false;
						this.Font = RsViewEngine.DefaultFont;
						this.Icon = (Icon)(RsViewEngine.Resources.GetObject("ReportSmartApp"));
						this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
						
						_lTitle = new Label();
						_lTitle.Location = new Point(0, 0);
						_lTitle.Size = new Size(this.ClientRectangle.Width, 64);
						_lTitle.TextAlign = ContentAlignment.MiddleLeft;
						_lTitle.Margin = new Padding(128, 8, 8, 8);
						_lTitle.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
						_lTitle.BackColor = Color.White;
						_lTitle.Name = "lTitle";
						_lTitle.Text = "This is the title.";
						_lTitle.Font = RsViewEngine.TitleFont;
						
						_lDLnk = new Label();
						_lDLnk.AutoSize = true;
						_lDLnk.Location = new Point(0, 0);
						_lDLnk.Size = new Size(this.ClientRectangle.Width, 24);
						_lDLnk.TextAlign = ContentAlignment.MiddleLeft;
						_lDLnk.BackColor = Color.Transparent;
						_lDLnk.Name = "lDlnk";
						_lDLnk.Text = "Details >>";
						_lDLnk.Font = new Font(RsViewEngine.DefaultFont, FontStyle.Underline); 
						_lDLnk.ForeColor = Color.Blue;
						_lDLnk.Click += ehSwitchDetails;
						_lDLnk.Cursor = Cursors.Hand;
						
						_lDet = new RichTextBox();
						_lDet.Multiline = true;
						_lDet.Size = new Size(304, 96);
						_lDet.Location = new Point(8, _lDLnk.Bottom + 8);
						_lDet.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
						_lDet.ReadOnly = true;
						_lDet.Visible = false;
						
						_lMessage = new Label();
						_lMessage.Location = new Point(dlgbtn_MARGIN, 64+dlgbtn_MARGIN);
						_lMessage.Size = new Size(
									this.ClientRectangle.Width - 2 * dlgbtn_MARGIN,
									this.ClientRectangle.Height - 2 * dlgbtn_MARGIN - dlgbtn_HEIGHT - _lTitle.Height - 24
								);
						_lMessage.Margin = new Padding(16, 8, 8, 8);
						_lMessage.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
						_lMessage.Name = "lMessage";
						_lMessage.Text = "This is the message.";
						
						_Icon = new PictureBox();
						_Icon.Size = new Size(48, 48);
						_Icon.Location = new Point(8, 8);
						
						_bOk = createButton("bOk");
						_bCancel = createButton("bCancel");
						_bYes = createButton("bYes");
						_bNo = createButton("bNo");
						
						this.ResumeLayout(false);
					}
					
					
				public void ehSwitchDetails(object aSender, EventArgs aEArgs) {
						if (!_details) {
								this.Size = new Size(this.Width, this.Height + _lDet.Height + 24);
								_lDet.Visible = true;
								_details = true;
							} else {
								this.Size = new Size(this.Width, this.Height - _lDet.Height - 24);
								_lDet.Visible = false;
								_details = false;
							}
					}
			}
	}

