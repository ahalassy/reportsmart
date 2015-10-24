#region Source information

//*****************************************************************************
//
//    ExtendedTextBox.cs
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
using ReportSmart.Graph;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ReportSmart.Controls {
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
