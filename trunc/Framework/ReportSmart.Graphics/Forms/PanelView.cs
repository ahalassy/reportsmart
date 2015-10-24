#region Source information

//*****************************************************************************
//
//    PanelView.cs
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
using System.Windows.Forms;

using ReportSmart;
using ReportSmart.Graph.Drawing;

namespace ReportSmart.Controls {
		public struct SMargins {
				private int _all;
		
				public int Top, Left, Bottom, Right;
				
				public int All {
						get { return _all; }
						set {
								_all = value;
								Left = _all;
								Top = _all;
								Right = _all;
								Bottom = _all;
							}
					}
				
				public int HorizontalSize { get { return Left + Right; } }
				public int VerticalSize { get { return Top + Bottom; } }
				
				public SMargins(int aDefault) { 
						Top = Left = Bottom = Right = _all = aDefault;
					}
			}

		public class CPanelView: Panel {
		
				private SMargins _marg;
				
				public new SMargins Margin {
						get { return _marg; }
						set { 
								_marg = value;
								Invalidate();
							}
					}
		
				protected override void OnPaint(PaintEventArgs aPEA) {
						// base.OnPaint(e);
						
						Graphics lGraph = aPEA.Graphics;
						Pen lPen = new Pen(Color.Black);
						
						lGraph.SmoothingMode = SmoothingMode.AntiAlias;
						
						Draw.RoundedRect(
									lGraph,
									new Rectangle(
												_marg.Left,
												_marg.Top,
												this.Width - _marg.HorizontalSize,
												this.Height - _marg.VerticalSize
											),
									16,
									null,
									new SolidBrush(Color.Blue)
								);
						
					}
					
				public CPanelView(): base() {
						this.BackColor = Color.White;
						_marg= new SMargins(32);
						
						ResizeRedraw = true;
						DoubleBuffered = true;
					}
			}
	}