/*
 * 
 * Adam ReportSmart, 2009.06.20.
 * 
 */
 
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
 
namespace ReportSmart {
		namespace Graph {
				public struct SHSBColor {
						public int Hue;
						public byte Saturation, Brightness;
					}
		
    			public class ColorTools {
    					protected static byte triMin(byte a, byte b, byte c) {
    							byte lRes = a < b ? a : b;
    							return lRes < c ? lRes : c;
    						}
    						
    					protected static byte triMax(byte a, byte b, byte c) {
    							byte lRes = a > b ? a : b;
    							return c > lRes ? c : lRes;
    						}
    			
        	    		public static Color SetAlpha(byte aAlpha, Color aColor) {
                    			return Color.FromArgb(aAlpha, aColor.R, aColor.G, aColor.B);
	            	    	}
	            	    		
    		    		public static Color ScaleAlpha(int aVal, int aScale, Color aColor) {
        						double x = (aVal / aScale) * 255;
        						return SetAlpha((byte)x, aColor);
        					}
        			
        				public static Color HSB2RGB(SHSBColor aHsbColor) {
        						return HSB2RGB(aHsbColor.Hue, aHsbColor.Saturation, aHsbColor.Brightness);
        					}
        			
        				public static Color HSB2RGB(int aHue, byte aSaturation, byte aBrightness) {
								aHue = aHue % 360;
								if (aHue < 0) aHue = 360 + aHue;
								
								double h = aHue / (double)60;
								double s = aSaturation / (double)255;
								double b = aBrightness / (double)255;
								int i = (int)Math.Floor(h);
								double f = h - i;
								double p = b * (1 - s);
								double q = b * (1 - s * f);
								double t = b * (1 - s * (1 - f));
								int vb = (int)Math.Round(b * 255);
								int vp = (int)Math.Round(p * 255);
								int vq = (int)Math.Round(q * 255);
								int vt = (int)Math.Round(t * 255);
								
								switch (i) {
										case 0: return Color.FromArgb(vb, vt, vp);
										case 1: return Color.FromArgb(vq, vb, vp);
										case 2: return Color.FromArgb(vp, vb, vt);
										case 3: return Color.FromArgb(vp, vq, vb);
										case 4: return Color.FromArgb(vt, vp, vb);
										case 5: return Color.FromArgb(vb, vp, vq);
										default: return Color.Purple;
									}
        					}
        					
        				public static SHSBColor RGB2HSB(byte aR, byte aG, byte aB) {
        						SHSBColor lResult;
        						lResult.Hue = 0;
        						byte lMin = triMin(aR, aG, aB);
        						byte lMax = triMax(aR, aG, aB);
        						double lD = lMax - lMin;
        						double lBri = ((lMax + lMax) / (double)2) / 255;
        						double lSat = lMax != 0 ? lD / lMax : 0;
        						
        						if (lSat != 0) {
        								if (aR == lMax) lResult.Hue = (int)((aG - aB) / lD);
        								else if (aG == lMax) lResult.Hue = 2 + (int)((aB - aR) / lD);
        								else if (aB == lMax) lResult.Hue = 4 + (int)((aR - aG) / lD);
        							} else {
        								lResult.Hue = -1;
        							}
        							
        						lResult.Hue *= 60;
        						lResult.Hue = lResult.Hue < 0 ? lResult.Hue + 360: lResult.Hue;
        						lResult.Saturation = (byte)((lSat * 255));
        						lResult.Brightness = (byte)((lBri * 255));
        						
        						return lResult;
        								
        					}
        			
        				public static SHSBColor RGB2HSB(Color aColor) { return RGB2HSB(aColor.R, aColor.G, aColor.B); }
        			
        				public static Color SetBrightness(byte aLight, Color aColor) {
        						SHSBColor lhsb = RGB2HSB(aColor);
        						return HSB2RGB(lhsb.Hue, lhsb.Saturation, aLight);
        					}
        					
        				public static Color SetHue(byte aHue, Color aColor) {
        						SHSBColor lhsb = RGB2HSB(aColor);
        						return HSB2RGB(aHue, lhsb.Saturation, lhsb.Brightness);
        					}
        					
        				public static Color SetSaturation(byte aSaturation, Color aColor) {
        						SHSBColor lhsb = RGB2HSB(aColor);
        						return HSB2RGB(lhsb.Hue, aSaturation, lhsb.Brightness);
        					}
        			
        				public static Color Brighten(int aScale, Color aColor) {
        						aScale = aScale > 100 ? 100 : aScale;
        						SHSBColor lhsb = RGB2HSB(aColor);
        						double lLight = lhsb.Brightness + (aScale / 100.0f) * (double)(255 - lhsb.Brightness);
        						return HSB2RGB(lhsb.Hue, lhsb.Saturation, (byte)lLight);
        					}
        					
        				public static Color Darken(int aScale, Color aColor) {
        						aScale = aScale > 100 ? 100 : aScale;
        						SHSBColor lhsb = RGB2HSB(aColor);
        						double lLight = (1f - aScale / 100.0f) * (double)lhsb.Brightness;
        						return HSB2RGB(lhsb.Hue, lhsb.Saturation, (byte)lLight);
        					}
        			}
        
				namespace Drawing {
						[Serializable]
						public struct SShadow {
								internal Color ShadowColor;
								internal int Size;
								internal int Distance;
								
								internal SShadow(Color aColor, int aSize) {
										ShadowColor = aColor;
										Size = aSize;
										Distance = 0;
									}
							}
						
						public class Draw {
								public static void ImageWithAlpha(Graphics aGraph, Image aImage, Rectangle aTarget, int aAlpha) {
										double lA = aAlpha / 100.0f > 1 ? 1 : aAlpha / 100.0f;
										float[][] lMItems ={ 
   													new float[] {1, 0, 0, 0, 0},
   													new float[] {0, 1, 0, 0, 0},
   													new float[] {0, 0, 1, 0, 0},
   													new float[] {0, 0, 0, aAlpha / 100.0f, 0}, 
   													new float[] {0, 0, 0, 0, 1}}; 
   											ColorMatrix lCMatrix = new ColorMatrix(lMItems);
   									
   										ImageAttributes lImgAttr = new ImageAttributes();
										lImgAttr.SetColorMatrix(
   													lCMatrix,
   													ColorMatrixFlag.Default,
   													ColorAdjustType.Bitmap);
   											
   										aGraph.DrawImage(
   												aImage,
   												aTarget,
   												0, 0,
   												aImage.Width, aImage.Height,
   												GraphicsUnit.Pixel,
   												lImgAttr
   											);
									}
									
								public static void RoundedRect(Graphics aGraph, Rectangle aRect, int aR, Pen aPen, Brush aBrush) {
										// First, build path:
										GraphicsPath lPath = new GraphicsPath();
																				
										lPath.StartFigure();
										lPath.AddArc(aRect.Left, aRect.Top, aR, aR, 180, 90);
										lPath.AddArc(aRect.Left + aRect.Width-aR, aRect.Top, aR, aR, 270, 90);
										lPath.AddArc(aRect.Left + aRect.Width - aR, aRect.Top + aRect.Height - aR, aR, aR, 0, 90);
										lPath.AddArc(aRect.Left + 0, aRect.Top + aRect.Height - aR, aR, aR, 90, 90);
										lPath.CloseFigure();
										
										
										if (aBrush != null)
												aGraph.FillPath(aBrush, lPath);

										if (aPen != null)
												aGraph.DrawPath(aPen, lPath);
									}
									
								public static void DrawShadowedText(Graphics aGraph, Point aLocation, Color aColor,	Color aShadowColor, Font aFont, string aText) {
										aGraph.DrawString(
													aText,
													aFont,
													new SolidBrush(aShadowColor),
													aLocation.X + 1,
													aLocation.Y + 1
												);
											
										aGraph.DrawString(
													aText,
													aFont,
													new SolidBrush(aColor),
													aLocation.X,
													aLocation.Y
												);
									}
							}
							
						public static class ShadowDraw {
								public enum TShadowMode {
										smRaise,
										smLower
									};
						
								public static void RoundedRect(
											Graphics aGraph,
											Rectangle aRect,
											int aR,
											Pen aPen,
											Brush aBrush,
											Color aShadowColor
									) { RoundedRect(aGraph, aRect, aR, aPen, aBrush, aShadowColor, TShadowMode.smLower, 1); }

								public static void RoundedRect(
											Graphics aGraph,
											Rectangle aRect,
											int aR,
											Pen aPen,
											Brush aBrush,
											Color aShadowColor,
											TShadowMode aShadowMode
									) { RoundedRect(aGraph, aRect, aR, aPen, aBrush, aShadowColor, aShadowMode, 1); }

								public static void RoundedRect(
											Graphics aGraph,
											Rectangle aRect,
											int aR,
											Pen aPen,
											Brush aBrush,
											Color aShadowColor,
											TShadowMode aShadowMode,
											int aDistance
									) {
										Rectangle lShRect, lRect;
										
										
										switch (aShadowMode) {
												case TShadowMode.smLower:
														lShRect = aRect;
														lRect = new Rectangle(
																	aRect.Left + aDistance,
																	aRect.Top + aDistance,
																	aRect.Width - aDistance,
																	aRect.Height - aDistance
																);
													break;
													
												case TShadowMode.smRaise:
														lRect = new Rectangle(
																	aRect.Left,
																	aRect.Top, 
																	aRect.Width - aDistance,
																	aRect.Height - aDistance
																);
														lShRect = new Rectangle(
																	aRect.Left + aDistance,
																	aRect.Top + aDistance,
																	aRect.Width - aDistance,
																	aRect.Height - aDistance
																);
														
													break;
													
												default: return;
											}
									
										// Drawing the shadow:
										Draw.RoundedRect(aGraph, lShRect, aR, null, new SolidBrush(aShadowColor));
												
										// Drawing the shape:
										Draw.RoundedRect(aGraph, lRect, aR, aPen, aBrush);
									}
							}
				
						public static class Shadows {
								public static void DrawHorizontalShadow(Graphics aGraph, int aLine, SShadow Shadow, Rectangle aClientRect) {
										Brush lBrush = new SolidBrush(Shadow.ShadowColor);
										aGraph.FillRectangle(lBrush, new Rectangle(0, aLine, aClientRect.Width, Shadow.Distance));
										lBrush = new LinearGradientBrush(
													new Point(0, aLine+Shadow.Distance + Shadow.Size),
													new Point(0, aLine+Shadow.Distance),
													ColorTools.SetAlpha(0, Shadow.ShadowColor),
													Shadow.ShadowColor
												);
										aGraph.FillRectangle(lBrush, 0, aLine + Shadow.Distance, aClientRect.Width, Shadow.Size);
									}
									
								public static void DrawShadowedShape() {
										
									}
							}
					}
			}
	}