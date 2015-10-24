/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam Halassy (2010.10.21.)
 * 
 * SOURCES:
 * Special/WinAp/User32_DLL
 * 
 */
using System;
using System.Diagnostics;

using Halassy.Special.WinApi;
 
namespace Halassy.Special {
 		public static class WindowHelper {
 		
 				#region STATIC:
 				#region class SystemMetrics
 				public static class SystemMetrics {
 						public static int GetwindowEdgeWidth() {
 								return libUser32.GetSystemMetrics(libUser32.SM_CXEDGE);
 							}
 					}
 				#endregion
 				
 				#region PUBLIC METHODS:
				public static bool IsThereWindow(string aTitle) {
						return libUser32.FindWindow(null, aTitle) != IntPtr.Zero;
					}
					
 				#endregion
 				#endregion
 				//--------------------------------------------------------------------------------------------------------------
 			}
 	}