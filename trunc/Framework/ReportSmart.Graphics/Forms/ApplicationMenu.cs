/*
 *
 * Licensing:			GPL
 * Original project:	Halassy.Graphics.csproj
 *
 * Copyright: Adam Halassy (2010.12.05.)
 * 
 * 
 */
using System;
using System.Windows.Forms;

using Halassy;
using Halassy.Special;
using Halassy.Special.WinApi;

namespace Halassy.Forms {
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
