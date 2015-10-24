#region Source information

//*****************************************************************************
//
//    dwm.cs
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
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ReportSmart.GUI {
		public static class DwmApi {
				public const int
							SWP_FRAMECHANGED = 0x0020,
							
							NC_RENDERINGPOLICY = 2;
		
				public enum TDwmNCRenderingPolicy {
							UseWindowStyle,
            				Disabled,
            				Enabled,
							Last
						}
		
				[DllImport("dwmapi.dll", PreserveSig = false)]
    			public static extern void DwmEnableBlurBehindWindow(IntPtr hWnd, DWM_BLURBEHIND pBlurBehind);
				
				[DllImport("dwmapi.dll", PreserveSig = false)]
				public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, MARGINS pMargins);
				
				[DllImport("dwmapi.dll", PreserveSig = false)]
				public static extern bool DwmIsCompositionEnabled();

				[DllImport("dwmapi.dll", PreserveSig = false)]
				public static extern void DwmEnableComposition(bool bEnable);

				[DllImport("dwmapi.dll", PreserveSig = false)]
				public static extern void DwmGetColorizationColor(
							out int pcrColorization,
							[MarshalAs(UnmanagedType.Bool)]out bool pfOpaqueBlend
						);
						
				[DllImport("dwmapi.dll", PreserveSig = false)]
        		public static extern int DwmSetWindowAttribute(
	            			IntPtr hwnd,
    				        int dwAttributeToSet,
				            ref int pvAttributeValue,
            				int cbAttribute
            			);

				[DllImport("dwmapi.dll", PreserveSig = false)]
				public static extern IntPtr DwmRegisterThumbnail(IntPtr dest, IntPtr source);

				[DllImport("dwmapi.dll", PreserveSig = false)]
				public static extern void DwmUnregisterThumbnail(IntPtr hThumbnail);

				[DllImport("dwmapi.dll", PreserveSig = false)]
				public static extern void DwmUpdateThumbnailProperties(
							IntPtr hThumbnail,
							DWM_THUMBNAIL_PROPERTIES props
						);

				[DllImport("dwmapi.dll", PreserveSig = false)]
				public static extern void DwmQueryThumbnailSourceSize(IntPtr hThumbnail, out Size size);
				
				[DllImport("dwmapi.dll", PreserveSig = false)]
				public static extern int DwmDefWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, out IntPtr result);

				[DllImport("User32.dll", SetLastError = true)]
        		private static extern bool GetWindowRect( IntPtr handle, ref RECT rect );
        
        		[DllImport("User32.dll", SetLastError = true)]
        		private static extern bool SetWindowPos(  IntPtr hWnd,
                			Int32 hWndInsertAfter,
                			Int32 X,
                			Int32 Y,
                			Int32 cx,
                			Int32 cy,
                			uint uFlags
                		);
        
        
				public static bool DwmAvailable() {						
						if (Environment.OSVersion.Version.Major >= 6)
								return DwmIsCompositionEnabled();
							else
								return false;
					}

    			public static void SetRenderingPolicy(IntPtr aHandle, TDwmNCRenderingPolicy aPolicy) {
    					int lVal = (int)aPolicy;
    					DwmSetWindowAttribute(aHandle, NC_RENDERINGPOLICY, ref lVal, sizeof(int));
    				}
    			
    			[StructLayout(LayoutKind.Sequential)]
				public class DWM_THUMBNAIL_PROPERTIES {
						public uint dwFlags;
						public RECT rcDestination;
						public RECT rcSource;
						public byte opacity;
						[MarshalAs(UnmanagedType.Bool)] public bool fVisible;
						[MarshalAs(UnmanagedType.Bool)] public bool fSourceClientAreaOnly;
						public const uint DWM_TNP_RECTDESTINATION = 0x00000001;
						public const uint DWM_TNP_RECTSOURCE = 0x00000002;
						public const uint DWM_TNP_OPACITY = 0x00000004;
						public const uint DWM_TNP_VISIBLE = 0x00000008;
						public const uint DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010;
					}

				[StructLayout(LayoutKind.Sequential)]
				public class MARGINS {
						public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight;
						public MARGINS(int left, int top, int right, int bottom) {
								cxLeftWidth = left; cyTopHeight = top; 
								cxRightWidth = right; cyBottomHeight = bottom;
							}
					}

				[StructLayout(LayoutKind.Sequential)]
				public class DWM_BLURBEHIND {
						public uint dwFlags;
						[MarshalAs(UnmanagedType.Bool)] public bool fEnable;
						public IntPtr hRegionBlur;
						[MarshalAs(UnmanagedType.Bool)] public bool fTransitionOnMaximized;

						public const uint DWM_BB_ENABLE = 0x00000001;
						public const uint DWM_BB_BLURREGION = 0x00000002;
						public const uint DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004;
					}

				[StructLayout(LayoutKind.Sequential)]
				public struct RECT {
						public int left, top, right, bottom;

						public RECT(int left, int top, int right, int bottom) {
								this.left = left; this.top = top; 
								this.right = right; this.bottom = bottom;
							}
    				}
    				
    			public static void ExtendFrame(IntPtr aHandle) {
    						RECT lRect = new RECT();
    			
    						GetWindowRect(aHandle, ref lRect);
    					
    						SetWindowPos(
    									aHandle,
    									0,
    									lRect.left,
    									lRect.top,
    									lRect.right - lRect.left,
    									lRect.bottom - lRect.top,
    									SWP_FRAMECHANGED
    								);
    				}
    				
    		}
    		
    	public class CDwmFormControl {
    			private Form _form;
    	
    			public CDwmFormControl(Form aForm) {
    					_form = aForm;
    					
    					ApplyDwmFeatures();
    				}
    				
    			public bool ApplyDwmFeatures() {
    					if (DwmApi.DwmAvailable()) {
    							_form.BackColor = Color.White;
    							
    							//_form.TransparencyKey = null;
    							_form.TransparencyKey = Color.White;
    					
    							DwmApi.DwmEnableComposition(true);
    					
    							DwmApi.DWM_BLURBEHIND BlurBehind = new DwmApi.DWM_BLURBEHIND();
    							BlurBehind.dwFlags = 
    									DwmApi.DWM_BLURBEHIND.DWM_BB_ENABLE + 
    									DwmApi.DWM_BLURBEHIND.DWM_BB_BLURREGION +
    									DwmApi.DWM_BLURBEHIND.DWM_BB_TRANSITIONONMAXIMIZED;
    									
    					
    							DwmApi.DwmEnableBlurBehindWindow(_form.Handle, BlurBehind);
    							DwmApi.DwmExtendFrameIntoClientArea(
    									_form.Handle,
    									new DwmApi.MARGINS(50, 50, 50, 50)
    								);
    							return true;
    						} else
    							return false;
    				}
    		}
	}