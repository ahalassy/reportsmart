/*
 * Adam Halassy, 2009.06.06.
 * 
 */

using System;
using System.Drawing;
using System.Windows.Forms;

using XavStud;
using XavStud.GraphicsTools;

namespace XavControls {
	public class CXavControl: System.Windows.Forms.UserControl {
            public const int DEF_WIDTH  = 240;
            public const int DEF_HEIGHT = 24;        
            
            public const int alLEFT     = 0x01;
            public const int alCENTER   = 0x02;
            public const int alRIGHT    = 0x03;
            public const int alTOP      = 0x01;
            public const int alMID      = 0x02;
            public const int alBOTTOM   = 0x03;

			private Color _Color = Color.Blue;
			private Color _TextColor = Color.White;
			
            private string _Caption = "XavControl";
            
            protected virtual void InitializeComponent() {
                    this.DoubleBuffered = true;
            
                    this.Name = "XavControl";
                    
                    this.Font = new Font("Arial", 10);
                    this.Size = new Size(DEF_WIDTH, DEF_HEIGHT);
                    this.Paint += new PaintEventHandler(this.DrawCtrl);
                    this.MouseEnter += new MouseEventHandler(this.EH_MouseEnter);
                }
                
            protected void DrawString(Graphics aGraph, string aStr, int aVAlign, int aHAlign, int aLeft, int aTop) {
                    Brush lBrush;
                    int lTTop, lTLeft;
                    
                    SizeF lTextSize = aGraph.MeasureString(Caption, Font);
                    Brush lTextBrush = new SolidBrush(this.TextColor);
                    
                    aGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    
                    switch (aVAlign) {
                            case alMID: lTTop = (int)(this.Height - lTextSize.Height) / 2; break;
                            case alBOTTOM: lTTop = (int)(this.Height - lTextSize.Height); break;
                            default: lTTop = (int)aTop; break;
                        }

                    lBrush = new SolidBrush(Color.FromArgb(0xAA, 0x00, 0x00, 0x00));
                    aGraph.DrawString(
                            Caption,
                            this.Font,
                            lBrush,
                            (Width - lTextSize.Width) / 2 + 2,
                            (Height - lTextSize.Height) / 2 + 2
                        );
                    lBrush = new SolidBrush(Color.FromArgb(0x80, 0x00, 0x00, 0x00));
                    aGraph.DrawString(
                            Caption,
                            this.Font,
                            lBrush,
                            (Width - lTextSize.Width) / 2 + 1,
                            (Height - lTextSize.Height) / 2 + 1
                        );
                    aGraph.DrawString(
                            Caption,
                            this.Font,
                            lTextBrush,
                            (Width - lTextSize.Width) / 2,
                            (Height - lTextSize.Height) / 2
                        );
                }

            protected virtual void DrawCtrl(object aSender, PaintEventArgs aPArgs) {
                }
                
            protected virtual void EH_MouseEnter(object aSender, MouseEventArgs aMArgs) {
                    Invalidate();
                }
		
			public CXavControl() {
                    InitializeComponent();
				}

            public string Caption { get { return _Caption; } set { _Caption = value; Invalidate(); } }
            public Color Color { get { return _Color; } set { _Color = value; Invalidate(); } }
            public Color TextColor { get { return _TextColor; } set { _TextColor = value; Invalidate(); } }
		}
	
	public class CXavButton: CXavControl {
	    // Constants:
            public const int xbACTION = 0;
            public const int xbMENU = 1;
            public const int xbGLYPH = 2;
            
        // Private:
            private byte _cAlpha = 30;
            private int _ButtonStyle = xbACTION;
            
        // Properties:
            public int ButtonStyle { get { return _ButtonStyle; } set { _ButtonStyle = value; Invalidate(); }}

        // Methods - private:
            private void _DrawAsBtn(PaintEventArgs aPArgs) {
			        Graphics lGraph = aPArgs.Graphics;
                    Pen lPen = new Pen(ColorTools.setAlpha(_cAlpha, this.Color));                    
                    Brush lBrush = new SolidBrush(ColorTools.setAlpha(_cAlpha, this.Color));

                    lGraph.FillPie(lBrush, (int)0, (int)0, (int)Height*2, (int)Height*2, 180, 90);
                    lGraph.FillPie(lBrush, (int)Width-(Height*2), (int)-Height-1, (int)Height*2, (int)Height*2, 0, 90);

                    lGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    //lGraph.DrawLine(lPen, 0, Height, Width-(Height*2), Height);
                    //lGraph.DrawLine(lPen, Height*2, 0, Width, 0);
                    lGraph.FillRectangle(lBrush, (int)(Height+1), (int)0, (int)(Width-(Height*2)), (int)Height);
                    
                    DrawString(lGraph, Caption, alCENTER, alMID, Width / 2, Height / 2);

                }
                
            private void _DrawAsMenuBtn(PaintEventArgs aPArgs) {
			        Graphics lGraph = aPArgs.Graphics;
                    Pen lPen = new Pen(ColorTools.setAlpha(_cAlpha, this.Color));                    
                    Brush lBrush = new SolidBrush(ColorTools.setAlpha(_cAlpha, this.Color));

                    lGraph.FillRectangle(lBrush, 0, 0, Width, Height);                    
                    DrawString(lGraph, Caption, alCENTER, alMID, Width / 2, Height / 2);
                }

        // Methods - protected:        
            protected override void InitializeComponent() {
                    base.InitializeComponent();
                    this.Name = "XavButton";
                }
                
            protected override void EH_MouseEnter(object aSender, MouseEventArgs aMArgs) {
			        switch (_ButtonStyle) {
			                case xbMENU:
			                        _cAlpha = 0;
			                    break;
			                default:
			                        _cAlpha = 255;
			                    break;
			            }                    
            
                    base.EH_MouseEnter(aSender, aMArgs);                    
                }

			protected override void DrawCtrl(object aSender, PaintEventArgs aPArgs) {
			        aPArgs.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			        switch (_ButtonStyle) {
			                case xbMENU:
			                        _DrawAsMenuBtn(aPArgs);
			                    break;
			                case xbGLYPH:
			                        //_DrawAsGlyphBtn(aPArgs);
			                        ;
			                    break;
			                default:
			                        _DrawAsBtn(aPArgs);
			                    break;
			            }
				}   
            
		}
	
}
