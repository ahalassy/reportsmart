/*
 *
 * Licensing:			GPL
 * Original project:	bin.csproj
 *
 * Copyright: Adam ReportSmart (2009.10.14.)
 * 
 * 
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;

using ReportSmart;
using ReportSmart.Localization;

using ReportSmart.Application;

namespace ReportSmart.Controls {
	/// <summary>
	/// Description of Form1.
	/// </summary>
	internal partial class CRSAbout: CLocalizedForm {
		public override string GetInstanceName() { return "CRSAbout"; }
	
		public override void ApplyLocale(CLocalization aLocale) {
				XmlNode lData = aLocale.GetFormData("About");
				
				this.Text = XmlTools.getXmlNodeByName("caption", lData).InnerText;
				_bOk.Text = aLocale.GetTagText("Ok");
			}
	
		public CRSAbout()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();			
			
			_bOk.Click += new EventHandler(EH_Ok);
		}
		
		protected override void OnShown(EventArgs e) {
				base.OnShown(e);
				
				this.ApplyLocale(RsViewEngine.Locale);
			}
		
		protected override void OnPaint(PaintEventArgs e) {
					base.OnPaint(e);
					
					Graphics aGraph = e.Graphics;
					
					aGraph.TextRenderingHint = TextRenderingHint.AntiAlias;
					Brush lTextBrush = new SolidBrush(Color.White);
					
					aGraph.DrawString(RsViewEngine.Version.ToString(), this.Font, lTextBrush, 410, 103);
				}
				
		protected void EH_Ok(object aSender, EventArgs aEArgs) {
				this.DialogResult = DialogResult.OK;
			}
	}
}
