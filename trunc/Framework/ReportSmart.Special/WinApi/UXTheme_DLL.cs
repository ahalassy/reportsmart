/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam Halassy (2010.06.26.)
 * 
 * 
 */
 
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Halassy.Special.WinApi {
		public struct SUxTextProperties {
				public Color color;
				public int glowSize;
				public Rectangle bounds;
				public TextFormatFlags flags;
			}
				
		public static class UxTheme {
				public const int DTT_COMPOSITED = 8192;
				public const int DTT_GLOWSIZE = 2048;
				public const int DTT_TEXTCOLOR = 1;
		
				[StructLayout(LayoutKind.Sequential)]
				public struct DTTOPTS {
						public int dwSize;
						public int dwFlags;
						public int crText;
						public int crBorder;
						public int crShadow;
						public int iTextShadowType;
						public POINT ptShadowOffset;
						public int iBorderSize;
						public int iFontPropId;
						public int iColorPropId;
						public int iStateId;
						public bool fApplyOverlay;
						public int iGlowSize;
						public int pfnDrawTextCallback;
						public IntPtr lParam;
					}
					
				[StructLayout(LayoutKind.Sequential)]
				public struct RECT {
						public int left, top, right, bottom;

						public RECT(int left, int top, int right, int bottom) {
								this.left = left; this.top = top; 
								this.right = right; this.bottom = bottom;
							}
    				}
    				
    			[StructLayout(LayoutKind.Sequential)]
				public struct POINT {
						public POINT(int x, int y) {
								this.x = x;
								this.y = y;
							}

						public int x;
						public int y;
					}
    				

				
				[DllImport("coredll.dll", SetLastError = true)]
				public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
		
				[DllImport("UxTheme.dll")]
        		public static extern int DrawThemeTextEx(
        					IntPtr hTheme,
        					IntPtr hdc,
        					int iPartId,
        					int iStateId,
        					string text,
        					int iCharCount,
        					int dwFlags,
        					ref RECT pRect,
        					ref DTTOPTS pOptions
        				);
        				
        		public static void DrawThemeTextActive(
        					Graphics aGraph,
        					string aText,
        					Point aLocation,
        					SUxTextProperties aProp
        				) {
        				IntPtr lPriHdc = aGraph.GetHdc();
        				VisualStyleRenderer lRenderer = new VisualStyleRenderer(VisualStyleElement.Window.Caption.Active);
        				DTTOPTS lOpts = new DTTOPTS();
        				lOpts.dwSize = Marshal.SizeOf(lOpts);
        				lOpts.dwFlags = DTT_COMPOSITED | DTT_GLOWSIZE | DTT_TEXTCOLOR;
        				lOpts.crText = ColorTranslator.ToWin32(aProp.color);
        				lOpts.iGlowSize = aProp.glowSize;
        				RECT ltBounds = new RECT(0,	0, aProp.bounds.Width, aProp.bounds.Height);
        				DrawThemeTextEx(
        							lRenderer.Handle,
        							lPriHdc,
        							0, 0,
        							aText,
        							-1,
        							(int)(aProp.flags),
        							ref ltBounds,
        							ref lOpts
        						);
        							
        			}
			}
	}