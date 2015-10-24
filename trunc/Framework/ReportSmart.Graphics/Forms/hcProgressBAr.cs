#region Source information

//*****************************************************************************
//
//    hcProgressBAr.cs
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ReportSmart.Graph;
using ReportSmart.Graph.Drawing;

namespace ReportSmart.Controls
{
    public class CProgressBar: Control {
 				private int _min, _max, _val;
 				private bool _shText, _shPercent, _reflection;
 				private Color _textColor, _textBG;
 				
 				[BrowsableAttribute(true), Category("Appearance")]
 				public Color TextBackColor {
 						get { return _textBG; }
 						set { _textBG = value; Invalidate(); }
 					}	
 				
 				[BrowsableAttribute(true), Category("Appearance")]
 				public Color TextColor {
 						get { return _textColor; }
 						set { _textColor = value; Invalidate(); }
 					}			
 				
 				[BrowsableAttribute(true), Category("Appearance")]
 				public int Minimum {
 						get { return _min; }
 						set { _min = value; Invalidate(); }
 					}
 					
 				[BrowsableAttribute(true), Category("Appearance")]
 				public int Maximum {
 						get { return _max; }
 						set { _max = value; Invalidate(); }
 					}
 					
 				[BrowsableAttribute(true), Category("Appearance")]
 				public int CurrentValue {
 						get { return _val; }
 						set { _val = value; Invalidate(); }
 				
 				}

 				[BrowsableAttribute(true), Category("Appearance")]
 				public bool ShowText {
 						get { return _shText; }
 						set { _shText = value; Invalidate(); }
 					}

 				[BrowsableAttribute(true), Category("Appearance")]
 				public bool ShowPercentage {
 						get { return _shPercent; }
 						set { _shPercent = value; Invalidate(); }
 					}
 					
 				[BrowsableAttribute(true), Category("Appearance")]
 				public bool Reflection {
 						get { return _reflection; }
 						set {
 								_reflection = value;
 								Invalidate();
 							}
 					}
 					
 				[BrowsableAttribute(false)]
 				public int Percent {
 						get { return (int)((_val / (float)(_max - _min))*100); }
 					}
 					
 				protected void DrawBar(
 							Graphics aGraph,
 							Rectangle aRect,
 							bool aReflect
 					) {
						aGraph.SmoothingMode = SmoothingMode.HighQuality;
						int lHeight = aRect.Height, lWidth = aRect.Width;
						Rectangle lRect = new Rectangle(
									aRect.Left, aRect.Top,
									(int)((_val / (float)(_max - _min)) * (lWidth-1)),
									lHeight-1
								);
				
						ShadowDraw.RoundedRect(
									aGraph,
									new Rectangle(
												aRect.Left + 0, aRect.Top + 0,
												lWidth -1, lHeight - 1
											),
									8,
									null,
									new SolidBrush(ColorTools.Darken(40, BackColor)),
									Color.FromArgb(0xa0, Color.Black),
									ShadowDraw.TShadowMode.smLower,
									2
								);
						if (lRect.Width != 0) {
								LinearGradientBrush lBrush = new LinearGradientBrush(
											lRect,
											Color.FromArgb(0xa0, Color.White),
											Color.FromArgb(0xa0, Color.Black),
											90
										);
										
								Draw.RoundedRect(aGraph, lRect, 8, null, new SolidBrush(ForeColor));
								Draw.RoundedRect(aGraph, lRect, 8, null, lBrush);
							}
								
						if (_shText || _shPercent) {
								string lText = _shPercent ? Percent.ToString() + "%" : "";
								if (_shText) {
										lText = _shPercent ? " (" + lText + ")" : "";
										lText = Text + lText;
									}
								lText = "  " + lText + "  ";
									
								SizeF lTSize = aGraph.MeasureString(lText, Font);
								
								Draw.RoundedRect(
										aGraph,
										new Rectangle(
													aRect.Left + (int)((lWidth - lTSize.Width) / 2),
													aRect.Top + (int)((lHeight - (lTSize.Height + lTSize.Height/2)) / 2),
													(int)(lTSize.Width),
													(int)(lTSize.Height + lTSize.Height/2)
												),
										8,
										null,
										new SolidBrush(Color.FromArgb(0xa0, _textBG))
									);
								

								aGraph.DrawString(
											lText,
											Font,
											new SolidBrush(TextColor),
											new PointF(
														aRect.Left + (lWidth - lTSize.Width) / 2,
														aRect.Top + (lHeight - lTSize.Height) / 2
													)
										);
									
							}
 					}
 		
				protected override void OnPaint(PaintEventArgs e) {
						int lWidth = _reflection ? (int)(this.Width * 0.9) : this.Width;
				
						Rectangle lRect = new Rectangle(
								(this.Width - lWidth) / 2,
								0,
								lWidth, 
								_reflection ? (int)((float)(this.Height) / 3 * 2) : this.Height
							);
						DrawBar(e.Graphics, lRect, false);
						lRect.Location = new Point(lRect.Left, lRect.Height + 2);
						DrawBar(e.Graphics, lRect, true);
						lRect.Size = new Size(this.Width, this.Height - lRect.Height - 2);
						LinearGradientBrush lBrush = new LinearGradientBrush(
									lRect,
									Color.FromArgb(0x80, this.BackColor),
									this.BackColor,
									90
								);
						e.Graphics.FillRectangle(lBrush, lRect);
					}
					
				public CProgressBar():base() {
						_min = 0;
						_max = 100;
						_val = 50;
						
						_textColor = Color.Black;
						_textBG = Color.White;
						_shPercent = true;
						_shText = false;
						_reflection = true;
						
						ResizeRedraw = true;
						DoubleBuffered = true;
					}
 			}
 	}