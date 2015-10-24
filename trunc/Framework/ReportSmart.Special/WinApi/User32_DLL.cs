/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam ReportSmart (2010.07.30.)
 * 
 * 
 */
 
using System;
using System.Runtime.InteropServices;

namespace ReportSmart.Special.WinApi {
		public static class libUser32 {

				#region PUBLIC CONSTANTS:
				public const int GWL_STYLE = -16;
				
				public const ulong
					WS_BORDER = 		0x00800000,
					WS_MAXIMIZEBOX = 	0x00010000,
					WS_DLGFRAME = 		0x00400000,
					WS_SIZEBOX = 		0x00040000;
					
				public const int // Getting system metrics:
					SM_CXBORDER =	 	5,
					SM_CXSIZEFRAME =	32,
					SM_CXEDGE = 		45;
					
				public const int // Other values:
					BYPOSITION =		0x400;
				
				public const ulong MY_SIZEABLEBOX = WS_SIZEBOX;
				#endregion
				//--------------------------------------------------------------------------------------------------------------
				
				#region PUBLIC METHODS:
				#region PUBLIC METHODS - DLL IMPORTS:
				[DllImport("User32.dll")]
				public static extern ulong SetWindowLong(IntPtr aHandle, int aIndex, ulong dwNewLong);

				[DllImport("User32.dll")]
				public static extern ulong GetWindowLong(IntPtr aHandle, int aIndex);

				[DllImport("user32.dll")]
				public static extern int GetSystemMetrics(int aIndex);
				
				[DllImport("user32.dll")]
				public static extern IntPtr FindWindow(string className, string windowTitle);
				
				[DllImport("user32.dll")]
				public static extern IntPtr FindWindowEx(
							IntPtr handleParent,
							IntPtr handleChild,
							string className,
							string WindowName
						);
				
				[DllImport("user32.dll")]
				public static extern IntPtr GetSystemMenu(IntPtr HWND);
				
				[DllImport("user32.dll")]
				public static extern IntPtr AppendMenu(
							IntPtr MenuHandle,
							int Props,
							int FlagsW,
							string text
						);
				#endregion
				
				#endregion
				//--------------------------------------------------------------------------------------------------------------

			}
	}