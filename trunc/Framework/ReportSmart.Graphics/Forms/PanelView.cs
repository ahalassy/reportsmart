/*
 *
 * Licensing:			GPL
 * Original project:	bin.csproj
 *
 * Copyright: Adam ReportSmart (2010.06.07.)
 * 
 * 
 */
 
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