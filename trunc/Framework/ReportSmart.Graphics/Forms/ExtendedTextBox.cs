/*
 *
 * Licensing:			GPL
 * Original project:	ReportSmart
 *
 * Copyright: Adam Halassy (2009.09.24.)
 * 
 * 
 */
 
using Halassy.Graph;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Halassy.Controls {
		public class CExtendedTextBox: TextBox {
				private string _ContentHelp;
				private bool _ShowingContentHelp;
				private Font _NormalFont, _ContentHelpFont;
				private Color _ForeColor;
				private char _IsPwd;
				
				public Font ContentFont;
				
				public string ContentHelp {
						get { return _ContentHelp; }
						set {
								_ContentHelp = value;
								Invalidate();
							}
					}
		
				public void UpdateContentHelp() {
						if (Text == "") {
								_NormalFont = Font;
								_ForeColor = ForeColor;
								_IsPwd = PasswordChar;
								PasswordChar = ((char)0x00);
								if (ContentFont == null)
										_ContentHelpFont = new Font(
													_NormalFont.FontFamily,
													_NormalFont.Size,
													FontStyle.Italic,
													_NormalFont.Unit
												);
									else
										_ContentHelpFont = new Font(
													ContentFont.FontFamily,
													ContentFont.Size,
													FontStyle.Italic,
													ContentFont.Unit
												);
								Font = _ContentHelpFont;
								base.Text = _ContentHelp;
								ForeColor = Coloring.MergeColors(_ForeColor, this.BackColor, 0.5);
								_ShowingContentHelp = true;
						}
					}
				
				public CExtendedTextBox(): base() {
						DoubleBuffered = true;
						_ShowingContentHelp = false;
						base.Text = this.Name;
						ForeColor = _ForeColor;
						PasswordChar = _IsPwd;
						this.Enter += new EventHandler(EH_Enter);
						this.Leave += new EventHandler(EH_Leave);
					}
				
			
				public void EH_Enter(object aSender, EventArgs aEArgs) {
						if (_ShowingContentHelp) {
								Font = _NormalFont;
								base.Text = "";
								PasswordChar = _IsPwd;
								ForeColor = _ForeColor;
								_ShowingContentHelp = false;
							}
					}
					
				public void EH_Leave(object aSender, EventArgs aEArgs) {
						UpdateContentHelp();
					}
			}
	}
