#region Source information

//*****************************************************************************
//
//    CRSSettings.cs
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
using System.IO;
using System.Windows.Forms;
using System.Xml;

using ReportSmart;
using ReportSmart.Controls;
using ReportSmart.Localization;
using ReportSmart.Network;
using ReportSmart.Network.Email;
using ReportSmart.Security;

using ReportSmart.Application;
using ReportSmart.Application.Defaults;
using ReportSmart.Engine;
using ReportSmart.Engine.Config;

namespace ReportSmart.Controls {
		
		public class CRSSpecPanel: CSpecialPanel {
				public override string GetInstanceName() { return this.Name; }
		
				public CRSSpecPanel(): base() {
						HeaderColor = RsGfx.HeaderColor;
						HeaderFontColor = Color.White;
						ImageOpen = (Image)(RsViewEngine.Resources.GetObject("gfx_arrow_right_32x32"));
						ImageClose = (Image)(RsViewEngine.Resources.GetObject("gfx_arrow_down_32x32"));
					}
			}


        public class CRSGeneralPanel: CRSSpecPanel {
                private CheckBox _chEnabledParam;
                private Button _bApply;
        
                public override void ApplyLocale(CLocalization aLocale) {
                        base.ApplyLocale(Locale);
                        
                        XmlNode lLocNode = Locale.GetPanelData(this.Name);
                        
                        _bApply.Text = Locale.GetTagText("apply");
                        
                        if (lLocNode != null) {
                        		_chEnabledParam.Text = XmlTools.getXmlNodeByName("embeddedparam", lLocNode).InnerXml;
                        	}
                    }
        
                public CRSGeneralPanel(): base() {
                        this.Name = "general";
                        this.Size = new Size(this.Width, HeaderSize + ControlProperties.ControlSpacing * 3 + 2 * 32);
                
                        _chEnabledParam = new CheckBox();
                        _chEnabledParam.Size = new Size(512, _chEnabledParam.Height);
                        _chEnabledParam.BackColor = Color.Transparent;
                        _chEnabledParam.Location = new Point(
                                    BodyLeft + ControlProperties.ControlSpacing,
                                    HeaderSize + ControlProperties.ControlSpacing
                                );
                        _chEnabledParam.Text = "_EnabledParam";
                
                        _bApply = new Button();
                        _bApply.Size = new Size(128, 32);
                        _bApply.Location = new Point(
                        			this.Width - _bApply.Width - ControlProperties.ControlSpacing - Margin.Right,
                        			this.Height - _bApply.Height - ControlProperties.ControlSpacing
                                );
                        _bApply.BackColor = SystemColors.ButtonFace;
                        _bApply.Anchor = (AnchorStyles)(AnchorStyles.Right | AnchorStyles.Top);
                        _bApply.Text = "_apply";
                        
                        this.Controls.Add(_chEnabledParam);
                        this.Controls.Add(_bApply);
                        
                        this.PanelUpdate += new SpecialPanelEventNotify(ehUpdate);
                        _bApply.Click += new EventHandler(ehApply);
                        
                        RsViewEngine.Locale.AddLocalizedControl(this);
                        Collapsed = true;
                    }
            
                protected void ehApply(object aSender, EventArgs aE) {
                
                        RsViewEngine.ProfileManager.Profile.Settings.EmbeddedParamEdit.Enabled = _chEnabledParam.Checked;
                        RsViewEngine.ProfileManager.SaveProfile();
                    }
        
                protected void ehUpdate(CSpecialPanel aSender) {
                        _chEnabledParam.Checked = RsViewEngine.ProfileManager.Profile.Settings.EmbeddedParamEdit.Enabled;
                    }
            }


		public class CRSLangPanel: CRSSpecPanel {
				private ListBox _ctlLngList;
				
				public void LookupLanguages() {
						_ctlLngList.Items.Clear();
				
						foreach (string iFile in Directory.GetFiles(RsViewEngine.ProfileManager.LocalePath)) {
								if (CLocalization.IsLocale(iFile))
										_ctlLngList.Items.Add(CLocalization.GetLocaleInfo(iFile));
							}
					}
		
				public CRSLangPanel(): base() {
						this.Name = "language";
						this.Size = new Size(512, 256);
						
						_ctlLngList = new ListBox();
						_ctlLngList.Location = new Point(
									ControlProperties.ControlSpacing + BodyLeft,
									ControlProperties.ControlSpacing + HeaderSize
								);
						_ctlLngList.Size = new Size(
									this.Width - Margin.Size.Width - ControlProperties.ControlSpacing*2,
									128
								);
						_ctlLngList.BorderStyle = BorderStyle.None;
						_ctlLngList.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);
						
						this.Controls.Add(_ctlLngList);
						
						RsViewEngine.Locale.AddLocalizedControl(this);
						
						_ctlLngList.DoubleClick += new EventHandler(ehSelectLang);
						this.PanelUpdate += new SpecialPanelEventNotify(ehUpdate);
						
						this.DetermineHeight();
						
						Collapsed = true;
					}
					
				protected void ehUpdate(CSpecialPanel aSender) {
						LookupLanguages();
					}
					
				protected void ehSelectLang(object aSender, EventArgs aEArgs) {
						CLocaleInfo lLocaleNfo = (CLocaleInfo)(_ctlLngList.Items[_ctlLngList.SelectedIndex]);
						
						RsViewEngine.Locale.LoadLocalization(lLocaleNfo.FileName);
						RsViewEngine.Locale.ApplyLocalization();
						
						RsViewEngine.ProfileManager.Profile.Settings.Locale.Language = FileSystem.NameOf(lLocaleNfo.FileName);
						RsViewEngine.ProfileManager.SaveProfile();
					}
			}


		public class CRSExportPanel: CRSSpecPanel {		
				public CRSExportPanel(): base() {
						this.Name = "Export";
						this.Size = new Size(512, 512);
						
						RsViewEngine.Locale.AddLocalizedControl(this);
						
						this.DetermineHeight();
						
						Collapsed = true;
					}
			}


		public class CRSSecurityPanel: CRSSpecPanel {
				private CAuthEditor _ctlAuthEdit;
				
				public override void AssignLocale(CLocalization aLocale) {
						base.AssignLocale(aLocale);
						
						if (aLocale != null)
								aLocale.AddLocalizedControl(_ctlAuthEdit);
					}
					
				public void LookupSecurity() {
						_ctlAuthEdit.FileName = RsViewEngine.ProfileManager.SecurityFile;
						_ctlAuthEdit.SecurityDocument = RsViewEngine.RSSecurity;
					}
					
				public CRSSecurityPanel(): base() {
						this.Name = "Security";
						this.Size = new Size(512, 512);
						
						_ctlAuthEdit = new CAuthEditor();
						_ctlAuthEdit.Size = new Size(
						            this.Width - Margin.Right - Margin.Left - ControlProperties.ControlSpacing*2,
						            300
						        );
						_ctlAuthEdit.Location = new Point(
									ControlProperties.ControlSpacing + BodyLeft,
									ControlProperties.ControlSpacing + HeaderSize
								);
						_ctlAuthEdit.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
						_ctlAuthEdit.IconClose = (Image)(RsViewEngine.Resources.GetObject("gfx_arrow_down_32x32"));
						_ctlAuthEdit.IconExpand = (Image)(RsViewEngine.Resources.GetObject("gfx_arrow_right_32x32"));
						this.Controls.Add(_ctlAuthEdit);
						RsViewEngine.Locale.AddLocalizedControl(this);
						
						this.PanelUpdate += new SpecialPanelEventNotify(ehUpdate);
						
						this.DetermineHeight();
						Collapsed = true;
					}
					
				public void ehUpdate(CSpecialPanel aSender) {
						LookupSecurity();
					}
			}
			

        public class CRSOfflinePanel: CRSSpecPanel {
                private CheckBox _chEnabled;
                private Label _lTimeout, _lFormat;
                private ComboBox _ctlFormat, _ctlUnit;
                private TextBox _eTimeout;
                private Button _bApply;
        
                public override void ApplyLocale(CLocalization aLocale) {
                		CLocalization lLocale = aLocale;

                        base.ApplyLocale(aLocale);
                		
                  		_chEnabled.Text = lLocale.GetTagText("enabled");
						_lTimeout.Text = lLocale.GetTagText("timeout") + ":";
						_lFormat.Text = lLocale.GetTagText("format") + ":";
						_bApply.Text = lLocale.GetTagText("apply");
                        
						_ctlFormat.Items[0] = lLocale.GetTagText("filePDF");
						_ctlFormat.Items[1] = lLocale.GetTagText("fileDOC");
						_ctlFormat.Items[2] = lLocale.GetTagText("fileHTM");
                        
						_ctlUnit.Items[0] = lLocale.GetTagText("uMin");
						_ctlUnit.Items[1] = lLocale.GetTagText("uHour");
                        	
                    }
        
                public CRSOfflinePanel(): base() {
                        this.Name = "Offline";
                        this.Size = new Size(this.Width, HeaderSize + ControlProperties.ControlSpacing * 3 + 2 * 32);
                
                        _chEnabled = new CheckBox();
                        _chEnabled.Size = new Size(128, _chEnabled.Height);
                        _chEnabled.Anchor = (AnchorStyles)(AnchorStyles.Right | AnchorStyles.Top);
                        _chEnabled.BackColor = Color.Transparent;
                        _chEnabled.Location = new Point(
                                    this.Width - _chEnabled.Width - 32 - this.Margin.Right,
                                    (this.HeaderSize - _chEnabled.Height) / 2
                                );
                        _chEnabled.Text = "_Enabled";
                        _chEnabled.ForeColor = Color.White;
                
                        _lTimeout = new Label();
                        _lTimeout.Size = new Size(128, 32);
                        _lTimeout.TextAlign = ContentAlignment.MiddleRight;
                        _lTimeout.Location = new Point(
                                    BodyLeft + ControlProperties.ControlSpacing,
                                    HeaderSize + ControlProperties.ControlSpacing
                                );
                        _lTimeout.Text = "_Timeout";
                        
                        _lFormat = new Label();
                        _lFormat.Size = new Size(128, 32);
                        _lFormat.TextAlign = ContentAlignment.MiddleRight;
                        _lFormat.Location = new Point(
                                    _lTimeout.Left,
                                    _lTimeout.Bottom + ControlProperties.ControlSpacing
                                );
                        _lFormat.Text = "_Format";
                        
                        _ctlFormat = new ComboBox();
                        _ctlFormat.Size = new Size(128 * 2 + ControlProperties.ControlSpacing, 32);
                        _ctlFormat.Items.Add("_pdf");
                        _ctlFormat.Items.Add("_doc");
                        _ctlFormat.Items.Add("_html");
                        _ctlFormat.SelectedIndex = 0;
                        _ctlFormat.Enabled = false;
                        _ctlFormat.Location = new Point(
                                    _lFormat.Right + ControlProperties.ControlSpacing,
                                    _lFormat.Top
                                );
                                
                        _eTimeout = new TextBox();
                        _eTimeout.Size = new Size(128, 32);
                        _eTimeout.Location = new Point(
                                    _lTimeout.Right + ControlProperties.ControlSpacing,
                                    _lTimeout.Top
                                );
                                
                        _ctlUnit = new ComboBox();
                        _ctlUnit.Items.Add("_min");
                        _ctlUnit.Items.Add("_hour");
                        _ctlUnit.Size = new Size(128, 32);
                        _ctlUnit.SelectedIndex = 0;
                        _ctlUnit.Location = new Point(
                                    _eTimeout.Right + ControlProperties.ControlSpacing,
                                    _eTimeout.Top
                                );
                        _ctlUnit.DropDownStyle = ComboBoxStyle.DropDownList;
                                
                        _bApply = new Button();
                        _bApply.Size = new Size(128, 32);
                        _bApply.Location = new Point(
                                    this.Width - _bApply.Width - ControlProperties.ControlSpacing - Margin.Right,
                                    _ctlFormat.Top - (_bApply.Height - _ctlFormat.Height)
                                );
                        _bApply.BackColor = SystemColors.ButtonFace;
                        _bApply.Anchor = (AnchorStyles)(AnchorStyles.Right | AnchorStyles.Top);
                        _bApply.Text = "_apply";
                                
                        _lFormat.Size = new Size(_lFormat.Width, _ctlFormat.Height);
                        _lTimeout.Size = new Size(_lTimeout.Width, _eTimeout.Height);
                        
                        this.Controls.Add(_chEnabled);
                        this.Controls.Add(_lTimeout);
                        this.Controls.Add(_lFormat);
                        this.Controls.Add(_ctlFormat);
                        this.Controls.Add(_eTimeout);
                        this.Controls.Add(_ctlUnit);
                        this.Controls.Add(_bApply);
                        
                        this.PanelUpdate += new SpecialPanelEventNotify(ehUpdate);
                        _chEnabled.Click += new EventHandler(ehCheckClick);
                        _chEnabled.Click += new EventHandler(ehApply);
                        _bApply.Click += new EventHandler(ehApply);
                        
                        RsViewEngine.Locale.AddLocalizedControl(this);
                        
                        Collapsed = true;
                    }
            
                protected void ehCheckClick(object aSender, EventArgs aE) {
                        _bApply.Enabled = _chEnabled.Checked;
                        _eTimeout.Enabled = _chEnabled.Checked;
                        _ctlUnit.Enabled = _chEnabled.Checked;
                    }
                    
                protected void ehApply(object aSender, EventArgs aE) {
                		RsViewEngine.ProfileManager.Profile.Settings.Offline.Timeout = int.Parse(_eTimeout.Text) * (_ctlUnit.SelectedIndex == 1 ? 60 : 1);
                        RsViewEngine.ProfileManager.Profile.Settings.Offline.Enabled = _chEnabled.Checked;
                        
                        RsViewEngine.ProfileManager.SaveProfile();
                    }
        
                protected void ehUpdate(CSpecialPanel aSender) {
                        RsOfflineConfig lData = RsViewEngine.ProfileManager.Profile.Settings.Offline;
                        
                        _chEnabled.Checked = lData.Enabled;
                        _ctlUnit.SelectedIndex = lData.Timeout % 60 == 0 ? 1 : 0;
                        _eTimeout.Text = (lData.Timeout % 60 == 0 ? lData.Timeout / 60 : lData.Timeout).ToString();
                        
                        _bApply.Enabled = _chEnabled.Checked;
                        _eTimeout.Enabled = _chEnabled.Checked;
                        _ctlUnit.Enabled = _chEnabled.Checked;
                    }
            }
            

		public class CRSSettingsPanel: CSpecialPanelView, ILocalizedControl {
				private CLocalization _Locale;
				
				public void ReleaseLocale() {
						_Locale.RemoveLocalizedControl(this);
					}
					
				public void AssignLocale(CLocalization aLocale) {
						_Locale = aLocale;
					}
				
				public virtual string GetInstanceName() { return "CRSSettingsPanel"; }
				
				public void ApplyLocale(CLocalization aLocale) {
						this.Text = aLocale.GetTagText("settings");
						Invalidate();
					}
		
				public CRSSettingsPanel() {
						this.Font = RsViewEngine.DefaultFont;
						this.HeaderFont = RsViewEngine.TitleFont;
						this.AutoScroll = true;
				
						this.BackColor = Color.White;
						
						this.AddPanel(new CRSGeneralPanel());
						this.AddPanel(new CRSLangPanel());
						this.AddPanel(new CRSOfflinePanel());
						this.AddPanel(new CRSSecurityPanel());
						//this.AddPanel(new CRSExportPanel());
						
						RsViewEngine.Locale.AddLocalizedControl(this);
					}
			}
	}
	