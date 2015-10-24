#region Source information

//*****************************************************************************
//
//    BusyControl.cs
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ReportSmart.Controls {
		internal class CBusyControl: Control {
				private Control _ctl;
				
				public void ShowMessage(string aMsg) {
						_ctl.Controls.Add(this);
						this.Size = _ctl.Size;
						this.Location = new Point(0, 0);
						this.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
					}
		
				public CBusyControl(Control aControl) {
						_ctl = aControl;
						
						ResizeRedraw = true;
						this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
					}
					
				protected override void OnPaint(PaintEventArgs aPArgs) {
						Graphics lGrap = aPArgs.Graphics;
						SolidBrush lBrush = new SolidBrush(Color.Transparent);
						lGrap.FillRectangle(lBrush, this.ClientRectangle);
						
						Pen lPenA = new Pen(Color.FromArgb(80, 0, 20, 0));
						Pen lPenB = new Pen(Color.FromArgb(90, 0, 20, 0));
						
						for (int y = 0; y < this.Height; y++) {
								Pen lPen = y % 2 == 0 ? lPenA : lPenB;
								lGrap.DrawLine(lPen, 0, y, this.Width, y);
							}
					}
			}
	}