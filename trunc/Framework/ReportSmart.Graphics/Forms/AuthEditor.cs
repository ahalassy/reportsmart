#region Source information

//*****************************************************************************
//
//    AuthEditor.cs
//    Created by Adam (2015-10-23, 8:59)
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using ReportSmart.Graph;
using ReportSmart.Graph.Drawing;
using ReportSmart.Localization;
using ReportSmart.Security;

namespace ReportSmart.Controls
{

    public class CAuthEditor: Panel, ILocalizedControl {
				protected class CAuthItem: Control {
						private const int __HEIGHT = 48;
						private const int __EXP_HEIGHT = 128;
						private const int __EDIT_POS = 256;
				
						private Button _Del, _Apply, _Reset;
						private TextBox _ePwd, _eUsr;
						private bool _Focused = false, _Expanded = false;
						private Color _AreaColor = Color.FromArgb(0x00, 0x80, 0xff);
						private PictureBox _ctlOpenClose;
						private Image _iExpand, _iClose;
						private string _Service, _Passwd = "Password", _UsrName = "User name";
						private CSecurityNode _dNode;
						private CAuthEditor _parent;
						private XmlNode _tags;
						
						public XmlNode Tags {
								get { return _tags; }
								set {
										_tags = value;
										Invalidate();
									}
							}
							
						public string TagRemove { set { _Del.Text = value; }}
						
						public string TagReset { set { _Reset.Text = value; }}
						
						public string TagApply { set { _Apply.Text = value; }}
				
						public CAuthEditor Editor { get { return _parent; }}
						
						public CSecurityNode Node {
								get { return _dNode; }
								set {
										_dNode = value;
										parseNode();
									}
							}
						
						public Image IconExpand {
								set {
										_iExpand = value;
										_ctlOpenClose.Image = _Expanded ? _iClose : _iExpand;
										_ctlOpenClose.BackColor = Color.Transparent;
									}
							}
							
						public Image IconClose {
								set {
										_iClose = value;
										_ctlOpenClose.Image = _Expanded ? _iClose : _iExpand;
										_ctlOpenClose.BackColor = Color.Transparent;
									}
							}
						
						public string TagPassword {
								set { _Passwd = value; Invalidate(); }
								get { return _Passwd; }
							}
							
						public string TagUser {
								set { _UsrName = value; Invalidate(); }
								get { return _UsrName; }
							}
						
						public bool Expanded {
								get { return _Expanded; }
								set {
										_Expanded = value;
										this.Size = new Size(this.Width, _Expanded ? __EXP_HEIGHT : __HEIGHT);
										_ctlOpenClose.Image = _Expanded ? _iClose : _iExpand;
										
										_eUsr.Visible = _Expanded;
										_ePwd.Visible = _Expanded;
										_Apply.Visible = _Expanded;
										_Reset.Visible = _Expanded;
											
										_parent.SetPositions();
									}
							}
						
						public string Service {
								get { return _Service; }
								set {
										_Service = value;
										Invalidate();
									}
							}
						
						protected void parseNode() {
								_ePwd.Text = _dNode.Password;
								_eUsr.Text = _dNode.UserName;
								
								_Service = _dNode.Service;
								Text = _dNode.Type;
							}
						
						protected void applyNode() {
								_dNode.UserName = _eUsr.Text;
								_dNode.Password = _ePwd.Text;
							}
						
						public override Color BackColor {
								set {
										_AreaColor = value;
										this.Invalidate();
									}
								get { return _AreaColor; }
							}
				
						public CAuthItem(CAuthEditor aParent) {
								_parent = aParent;
						
								this.TabStop = true;
								this.DoubleBuffered = true;
								this.Size = new Size(aParent.Width, __HEIGHT);
								this.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right |AnchorStyles.Top);
								this.Click += new EventHandler(ehClick);
						
								_Del = new Button();
								_Del.BackColor = SystemColors.Control;
								_Del.Size = new Size(128, 32);
								_Del.Location = new Point(this.Width - (_Del.Width + 8), 8);
								_Del.Text = "Remove";
								_Del.Click += new EventHandler(ehClick);
								_Del.GotFocus += new EventHandler(ehGotFocus);
								_Del.LostFocus += new EventHandler(ehLostFocus);
								_Del.UseVisualStyleBackColor = true;
								_Del.Anchor = (AnchorStyles)(
											AnchorStyles.Top |
											AnchorStyles.Right
										);
								_Del.Click += new EventHandler(ehRemove);
								
								_Reset = new Button();
								_Reset.BackColor = SystemColors.Control;
								_Reset.Size = new Size(128, 32);
								_Reset.Location = new Point(this.Width - (_Reset.Width + 8), _Del.Bottom + 4);
								_Reset.Text = "Reset";
								_Reset.Click += new EventHandler(ehClick);
								_Reset.GotFocus += new EventHandler(ehGotFocus);
								_Reset.LostFocus += new EventHandler(ehLostFocus);
								_Reset.UseVisualStyleBackColor = true;
								_Reset.Anchor = (AnchorStyles)(
											AnchorStyles.Top |
											AnchorStyles.Right
										);
								_Reset.Visible = false;
								_Reset.Click += new EventHandler(ehReset);
								
								_Apply = new Button();
								_Apply.BackColor = SystemColors.Control;
								_Apply.Size = new Size(128, 32);
								_Apply.Location = new Point(this.Width - (_Apply.Width + 8), _Reset.Bottom + 4);
								_Apply.Text = "Apply";
								_Apply.Click += new EventHandler(ehClick);
								_Apply.GotFocus += new EventHandler(ehGotFocus);
								_Apply.LostFocus += new EventHandler(ehLostFocus);
								_Apply.UseVisualStyleBackColor = true;
								_Apply.Anchor = (AnchorStyles)(
											AnchorStyles.Top |
											AnchorStyles.Right
										);
								_Apply.Visible = false;
								_Apply.Click += new EventHandler(ehApply);
										
								_eUsr = new TextBox();
								_eUsr.TabStop = true;
								_eUsr.Size = new Size(128, 24);
								_eUsr.Location = new Point(__EDIT_POS, _Del.Bottom + 4);
								_eUsr.GotFocus += new EventHandler(ehGotFocus);
								_eUsr.LostFocus += new EventHandler(ehLostFocus);
								_eUsr.Visible = false;
								
								_ePwd = new TextBox();
								_ePwd.TabStop = true;
								_ePwd.Size = new Size(128, 24);
								_ePwd.Location = new Point(__EDIT_POS, _eUsr.Bottom + 4);
								_ePwd.PasswordChar = '*';
								_ePwd.GotFocus += new EventHandler(ehGotFocus);
								_ePwd.LostFocus += new EventHandler(ehLostFocus);
								_ePwd.Visible = false;
								
								_ctlOpenClose = new PictureBox();
								_ctlOpenClose.TabStop = true;
								_ctlOpenClose.Size = new Size(32, 32);
								_ctlOpenClose.BackColor = Color.Transparent;
								_ctlOpenClose.Location = new Point(4, (this.Height - 32)/2);
								_ctlOpenClose.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left);
								_ctlOpenClose.Click += new EventHandler(ehExpand);
								
								_ctlOpenClose.TabIndex = 0;
								_eUsr.TabIndex = 1;
								_ePwd.TabIndex = 2;
								_Del.TabIndex = 3;
								_Reset.TabIndex = 4;
								_Apply.TabIndex = 5;
										
								this.SuspendLayout();
								this.Controls.Add(_Del);
								this.Controls.Add(_Apply);
								this.Controls.Add(_Reset);
								this.Controls.Add(_ePwd);
								this.Controls.Add(_eUsr);
								this.Controls.Add(_ctlOpenClose);
								this.ResumeLayout();
								
								this.IconClose = aParent.IconClose;
								this.IconExpand = aParent.IconExpand;
								this.ResizeRedraw = true;
							}
							
						protected void ehGotFocus(object aSender, EventArgs aE) { _Focused = true; this.Invalidate(); }
						
						protected void ehLostFocus(object aSender, EventArgs aE) { _Focused = false; this.Invalidate();	}
							
						protected void ehClick(object aSender, EventArgs aE) { this.Focus(); }
						
						protected void ehExpand(object aSender, EventArgs aE) {
								Expanded = !Expanded;
								this.Focus();
							}
					
						protected void ehApply(object aSender, EventArgs aE) {
								applyNode();
								Editor.QuickSave();
							}
					
						protected void ehReset(object aSender, EventArgs aE) {
								parseNode();
							}
					
						protected void ehRemove(object aSender, EventArgs aE) {
								_dNode.Delete();
								Editor.QuickSave();
								Editor.Controls.Remove(this);
								this.Dispose();
							}
					
						protected override void OnPaint(PaintEventArgs ePaint) {
								Graphics lGraph = ePaint.Graphics;
								Color lTextColor = _Focused ? Color.White : Color.Black;
								Color lShadow = Color.FromArgb(_Focused ? 200 : 100, Color.Black);
								Font lFont = new Font(Font.FontFamily, Font.Size + 4, FontStyle.Bold, Font.Unit);
								lGraph.CompositingQuality = CompositingQuality.HighQuality;
								Pen lPen = new Pen(ColorTools.SetAlpha(0x80, ColorTools.SetBrightness(0x50, _AreaColor)));
								SolidBrush lTextBr = new SolidBrush(_Focused ? Color.White : Color.Black);
								lGraph.FillRectangle(new SolidBrush(Color.White), ClientRectangle);
								string lType = Text;
								int lHeight, lWidth;
								byte lAlpha;
								Color lColor;
								
								if (this.Focused || _Focused) {
										lAlpha = 0xff;
										lColor = ColorTools.SetAlpha(0x30, _AreaColor);
										lColor = ColorTools.SetSaturation(0xcc, lColor);
									} else {
										lAlpha = 0x30;
										lColor = ColorTools.SetSaturation(0, _AreaColor);
									}
										
								LinearGradientBrush lBrushA = new LinearGradientBrush(
											new Rectangle(0, 0, this.Width, this.Height/2),
											ColorTools.SetAlpha(lAlpha, ColorTools.SetBrightness(0xc0, lColor)),
											ColorTools.SetAlpha(lAlpha, ColorTools.SetBrightness(0x81, lColor)),
											90
										);
								LinearGradientBrush lBrushB = new LinearGradientBrush(
											this.ClientRectangle,
											ColorTools.SetAlpha(lAlpha, ColorTools.SetBrightness(0x80, lColor)),
											ColorTools.SetAlpha(lAlpha, ColorTools.SetBrightness(0x30, lColor)),
											90
										);
										
								lGraph.FillRectangle(lBrushA, 0, 0, this.Width, this.Height/2);
								lGraph.FillRectangle(lBrushB, 0, this.Height/2, this.Width, this.Height);
								lGraph.DrawLine(
											lPen,
											0, this.Height-1,
											this.Width, this.Height-1
										);
										
								if (_tags != null) {
										XmlNode lSrvNode = XmlTools.getXmlNodeByName(lType, _tags);
										lType = lSrvNode != null ? lSrvNode.InnerXml : lType;
									}
								lHeight = (int)(lGraph.MeasureString(lType, Font).Height);
								if (_Focused)
										lGraph.DrawString(
													lType,
													Font,
													new SolidBrush(Color.FromArgb(200, 0, 0, 0)),
													37,
													(__HEIGHT - lHeight) / 2+1
												);
								lGraph.DrawString(lType, Font, lTextBr, 36, (__HEIGHT - lHeight) / 2);
								
								lHeight = (int)(lGraph.MeasureString(Service, lFont).Height);
								Draw.DrawShadowedText(
											lGraph,
											new Point(_eUsr.Left, (__HEIGHT - lHeight) / 2),
											lTextColor,
											lShadow,
											lFont,
											_Service
										);
									
								lHeight = (int)(lGraph.MeasureString(_UsrName + ":", Font).Height);
								lWidth = (int)(lGraph.MeasureString(_UsrName + ":", Font).Width);
								Draw.DrawShadowedText(
											lGraph,
											new Point(_eUsr.Left - 8 - lWidth, _eUsr.Top + (_eUsr.Height - lHeight) / 2),
											lTextColor,
											lShadow,
											Font,
											_UsrName + ":"
										);
										
								lHeight = (int)(lGraph.MeasureString(_Passwd + ":", Font).Height);
								lWidth = (int)(lGraph.MeasureString(_Passwd + ":", Font).Width);
								Draw.DrawShadowedText(
											lGraph,
											new Point(_ePwd.Left - 8 - lWidth, _ePwd.Top + (_ePwd.Height - lHeight) / 2),
											lTextColor,
											lShadow,
											Font,
											_Passwd + ":"
										);
								
							}
							
						protected override void OnGotFocus(EventArgs e) {
								base.OnGotFocus(e);
								_Focused = true;
								this.Invalidate();
							}
							
						protected override void OnLostFocus(EventArgs e) {
								base.OnLostFocus(e);
								_Focused = false;
								this.Invalidate();
							}
					}
						
				int _RowHeight = 48;
				string _file;
				ArrayList _ItemsGUI;
				XmlDocument _Data;
				XmlNode _Security;
				Image _iconClose, _iconExpand;
				CLocalization _Locale;
				
				public Image IconExpand {
						get { return _iconExpand; }
						set {
								foreach (CAuthItem iItem in _ItemsGUI) {
										iItem.IconExpand = value;
										iItem.Invalidate();
									}
								_iconExpand = value;
							}
					}
		
				public Image IconClose {
						get { return _iconClose; }
						set {
								foreach (CAuthItem iItem in _ItemsGUI) {
										iItem.IconClose = value;
										iItem.Invalidate();
									}
								_iconClose = value;
							}
					}
					
				protected void InsertItem(CAuthItem aItem) {
						SuspendLayout();
						aItem.Location = new Point(0, _ItemsGUI.Count*_RowHeight);
						aItem.Size = new Size(this.Width, _RowHeight);
						aItem.Anchor = (System.Windows.Forms.AnchorStyles) (
									System.Windows.Forms.AnchorStyles.Top |
									System.Windows.Forms.AnchorStyles.Left |
									System.Windows.Forms.AnchorStyles.Right
								);
						//aItem.Resize += new EventHandler(ehResize);
						aItem.Expanded = false;
						
						applyLocaleToItem(aItem);
						
						Controls.Add(aItem);
						_ItemsGUI.Add(aItem);
						ResumeLayout();
					}
					
				protected void parseXML() {
						Controls.Clear();
						_ItemsGUI.Clear();
						_Security = XmlTools.getXmlNodeByName("security", _Data);
						
						if (_Security != null)
								foreach (XmlNode iNode in _Security) {								
										CAuthItem lItem = new CAuthEditor.CAuthItem(this);
										applyLocaleToItem(lItem);
										lItem.Node = new CSecurityNode(iNode);					
										InsertItem(lItem);
									}
									
						SetPositions();
					}
					
				protected void applyLocaleToItem(CAuthItem lItem) {
						XmlNode lPanelData;
					
				        if (_Locale != null && (lPanelData = _Locale.GetPanelData("authitem")) != null) {
		        				try { lItem.Tags = XmlTools.getXmlNodeByName("typelist", lPanelData); }
		        						catch (Exception E) { throw new Exception("Typelist not found.", E); };
		        				lItem.TagRemove = _Locale.GetTagText("remove");
		        				lItem.TagApply = _Locale.GetTagText("apply");
		        				lItem.TagReset = _Locale.GetTagText("reset");
		        				lItem.TagPassword = _Locale.GetTagText("password");
		        				lItem.TagUser = _Locale.GetTagText("user");
				            }
					}
				
				public XmlDocument SecurityDocument {
						get { return _Data; }
						set {
								_Data = value;
								parseXML();
							}
					}
					
				public string FileName {
						get { return _file; }
						set { _file = value; }
					}
							
				public void SetPositions() {
						int lTop = 0;
				
						foreach (CAuthItem iItem in _ItemsGUI) {
								iItem.Location = new Point(0, lTop);
								lTop += iItem.Height;
							}
					}

				public void LoadFile(string aFile) {
						_Data.Load(aFile);
						_file = aFile;
						parseXML();
					}
					
				public void SaveFile(string aFile) {
						_file = aFile;
						_Data.Save(_file);
					}
		
				public void QuickSave() { _Data.Save(_file); } 
		
				public void AssignLocale(CLocalization aSender) { _Locale = aSender; }				
				
				public void ApplyLocale(CLocalization aSender) {				
		        		foreach (CAuthItem iItem in _ItemsGUI)
		        				applyLocaleToItem(iItem);
				    }
				
				public void ReleaseLocale() { _Locale.RemoveLocalizedControl(this); }
				
				public string GetInstanceName() { return "CAuthItem"; }		
		
				public CAuthEditor() {
						_Data = new XmlDocument();
						
				
						this.VScroll = true;
						this.HScroll = true;
						this.AutoScroll = true;
						this.BorderStyle = BorderStyle.Fixed3D;
						//this.MouseWheel += new MouseEventHandler(ehMouseWheel);
						_ItemsGUI = new ArrayList();
						
						ResumeLayout();
					}
					
				/*
				protected void ehResize(object aSender, EventArgs aE) {
						SetPositions();
					}
				*/
				
				protected void ehMouseWheel(object aSender, MouseEventArgs aMEArgs) {
							this.AutoScrollPosition = new Point (
										this.AutoScrollPosition.X,
										this.AutoScrollPosition.Y - aMEArgs.Delta
									);
						}
			}
	}