#region Source information

//*****************************************************************************
//
//    WindowManagement.cs
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
using System.Diagnostics;

using ReportSmart.Special.WinApi;
 
namespace ReportSmart.Special {
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