/*
 *
 * Licensing:			GPL
 * Original project:	ReportSmart.Graphics.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.05.)
 * 
 * 
 */
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
