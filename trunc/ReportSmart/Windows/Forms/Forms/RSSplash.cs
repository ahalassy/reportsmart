/*
 * 2009-09-15 (Adam Halassy)
 * 
 */

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
