#region Source information

//*****************************************************************************
//
//    ApplicationMenu.cs
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
using System.Windows.Forms;

using ReportSmart;
using ReportSmart.Special;
using ReportSmart.Special.WinApi;

namespace ReportSmart.Forms {
		public class ApplicationMenu {
				public IntPtr MenuHandle { get; protected set; }
				
				public void AppendItem(string aText) {
						libUser32.AppendMenu(MenuHandle, libUser32.BYPOSITION, 12345, aText);
					}
		
				public ApplicationMenu(Form aForm) {
						MenuHandle = libUser32.GetSystemMenu(aForm.Handle);
					}
			}
	}
