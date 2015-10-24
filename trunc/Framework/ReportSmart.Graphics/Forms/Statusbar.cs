/*
 *
 * Licensing:			GPL
 * Original project:	dev_statusbar.csproj
 *
 * Copyright: Adam ReportSmart (2010.07.24.)
 * 
 * 
 */
 
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using ReportSmart.Graph;

namespace ReportSmart.Controls {

		public class CStatusControl: Control {
				private int _min, _max, _val, _cmin, _cmax, _cval;
				private int _smin, _smax, _sval, _scmin, _scmax, _scval;
				private int _leftMargin, _rightMargin, _barHeight;
				
				private bool _shSub, _neverinitiated;
		
				private Color _barColor;
				private Color _textColor;
				
				private Timer _timer;
		
				private string _mainCaption, _subCaption;
				
				public string Caption {
						get { return _mainCaption; }
						set {
								_mainCaption = value;
								this.Invalidate();
							}
					}
					
				public string SubCaption {
						get { return _subCaption; }
						set {
								_subCaption = value;
								this.Invalidate();
							}
					}
				
				public Color BarColor {
						get { return _barColor; }
						set {
								_barColor = value;
								Invalidate();
							}
						
					}
					
				public Color TextColor {
						get { return _textColor; }
						set {
								_textColor = value;
								Invalidate();
							}
						
					}
					
				public bool ShowSubBar {
						get { return _shSub; }
						set {
								_shSub = value;
								Invalidate();
							}
					}
					
				protected void ehFrame(object aSender, EventArgs aEArgs) {
						if (!calculateFrame())
								_timer.Enabled = false;
						Invalidate();

					}
					
				protected bool calculateFrame() {
						int lmin = (_min + _cmin) / 2;
						int lmax = (_max + _cmax) / 2;
						int lval = (_val + _cval) / 2;
						int lsmin = (_smin + _scmin) / 2;
						int lsmax = (_smax + _scmax) / 2;
						int lsval = (_sval + _scval) / 2;
						
						bool changed = !(
									lmin == _cmin &&
									lmax == _cmax &&
									lval == _cval &&
									lsmin == _scmin &&
									lsmax == _scmax &&
									lsval == _scval
								);
								
						_cmin = lmin;
						_cmax = lmax;
						_cval = lval;
						_scval = lsval;
						_scmin = lsmin;
						_scmax = lsmax;
						
						_neverinitiated = false;
							
						if (changed)
								_timer.Enabled = true;
						
						return changed;
					}
					
				public CStatusControl() {
						_neverinitiated = true;
				
						_max = 100;
						_min = 0;
						_val = 0;	
						
						_smax = 100;
						_smin = 0;
						_sval = 0;
						
						_leftMargin = 32;
						_rightMargin = 32;
						_barHeight = 32;
						
						_shSub = false;
						
						ResizeRedraw = true;
						DoubleBuffered = true;
						
						_timer = new Timer();
						_timer.Enabled = false;
						_timer.Tick += ehFrame;
						_timer.Interval = 1000 / 25;
						
						_mainCaption = "Main caption";
						_subCaption = "This is a sub caption";
					}
				
				protected override void OnPaint(PaintEventArgs aPArgs) {
						if (_neverinitiated) calculateFrame();
						Graphics lGraph = aPArgs.Graphics;
						lGraph.SmoothingMode = SmoothingMode.HighQuality;
						int lMainTop = (this.ClientRectangle.Height - _barHeight) / 2 - _barHeight * 2;
						int lSubTop = (this.ClientRectangle.Height - _barHeight) / 2 - _barHeight;
						int lBarTop = (this.ClientRectangle.Height - _barHeight) / 2;
						int lSubBarTop = (this.ClientRectangle.Height - _barHeight) / 2 + _barHeight + _barHeight/2;
						int lBarWidth = this.ClientRectangle.Width - (_leftMargin + _rightMargin);
						int lMainW = (int)(lBarWidth * ((_cval - _cmin) / (double)(_cmax - _cmin)));
						int lSubW = (int)(lBarWidth * ((_scval - _scmin) / (double)(_scmax - _scmin)));
						
						// The background:
						Pen lPen = new Pen(Color.Black);
						Brush lTextBrush = new SolidBrush(_textColor);
						LinearGradientBrush lBrush = new LinearGradientBrush(
									new Point(0, 0),
									new Point(0, this.ClientRectangle.Height),
									ColorTools.Darken(20, BackColor),
									ColorTools.Brighten(20, BackColor)
								);
						lGraph.FillRectangle(lBrush, ClientRectangle);
						
						// Main caption:
						lGraph.DrawString(
									"Itt vagyok!",
									new Font(Font, FontStyle.Bold),
									lTextBrush,
									_leftMargin*2,
									lMainTop
								);
						
						// Sub caption:
						lGraph.DrawString(
									"Itt vagyok!",
									new Font(Font, FontStyle.Regular),
									lTextBrush,
									_leftMargin*3,
									lSubTop
								);
								
						_cval = 50;
						_scval = 25;
						// The main status bar
						lBrush = new LinearGradientBrush(
									new Point(_leftMargin, lBarTop-2),
									new Point(_leftMargin, lBarTop + _barHeight+2),
									ColorTools.Darken(50, BackColor),
									ColorTools.Darken(20, BackColor)
								);
						lGraph.FillRectangle(
								lBrush,
								new Rectangle(
											_leftMargin, lBarTop,
											lBarWidth, _barHeight
										)
							);
						lBrush = new LinearGradientBrush(
									new Point(_leftMargin, lBarTop-2),
									new Point(_leftMargin, lBarTop + _barHeight+2),
									ColorTools.SetBrightness(80, _barColor),
									ColorTools.SetBrightness(20, _barColor)
								);
						lGraph.FillRectangle(
								lBrush,
								new Rectangle(
											_leftMargin, lBarTop,
											lMainW, _barHeight
										)
							);
							
						// The sub status bar (if visible)
						if (_shSub) {
								lBrush = new LinearGradientBrush(
											new Point(_leftMargin, lSubBarTop-2),
											new Point(_leftMargin, lSubBarTop + _barHeight+2),
											ColorTools.Darken(50, BackColor),
											ColorTools.Darken(20, BackColor)
										);
								lGraph.FillRectangle(
										lBrush,
										new Rectangle(
													_leftMargin, lSubBarTop,
													lBarWidth, _barHeight
												)
									);
								lBrush = new LinearGradientBrush(
											new Point(_leftMargin, lSubBarTop-2),
											new Point(_leftMargin, lSubBarTop + _barHeight+2),
											ColorTools.SetBrightness(80, _barColor),
											ColorTools.SetBrightness(20, _barColor)
										);
								lGraph.FillRectangle(
										lBrush,
										new Rectangle(
													_leftMargin, lSubBarTop,
													lSubW, _barHeight
												)
									);
							}
					}
			}
	}