#region Source information

//*****************************************************************************
//
//    RSSplash.cs
//    Created by Adam (2009-09-17, 8:57)
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
using System.Windows.Forms;

namespace ReportSmart.Controls
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	internal partial class CfSplash : Form
	{
		public CfSplash()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public void SetText(string aText) {
				_lText.Text = aText;
				System.Windows.Forms.Application.DoEvents();
			}
		
		void CfSplashShown(object sender, EventArgs e) {
				Point lPoint = new Point(
							((Screen.PrimaryScreen.Bounds.Right - Screen.PrimaryScreen.Bounds.Left) - this.Width) / 2,
							((Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.Bounds.Top) - this.Height) / 2
						);
				this.Location = lPoint;
				System.Windows.Forms.Application.DoEvents();
			}
	}
}
