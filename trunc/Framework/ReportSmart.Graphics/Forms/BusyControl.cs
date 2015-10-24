/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam Halassy (2010.09.19.)
 * 
 * 
 */
 
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Halassy.Controls {
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