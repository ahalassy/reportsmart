/*
 *
 * Licensing:			GPL
 * Original project:	
 *
 * Copyright: Adam Halassy (2010.06.10.)
 * 
 * 
 */
namespace ReportSmart.Application
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		private Halassy.Controls.CPageSelector _ctlPageSelector;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
				//
				// _ctlPageSelector:
				//
				this._ctlPageSelector = new Halassy.Controls.CPageSelector();
				this._ctlPageSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((
							(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
							System.Windows.Forms.AnchorStyles.Right
						)));
				this._ctlPageSelector.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
				this._ctlPageSelector.Host = null;
				this._ctlPageSelector.Location = new System.Drawing.Point(0, 0);
				this._ctlPageSelector.Name = "_ctlPageSelector";
				this._ctlPageSelector.PageWidth = 200;
				this._ctlPageSelector.SelectedPage = -1;
				this._ctlPageSelector.SelectedPageColor = System.Drawing.Color.White;
				this._ctlPageSelector.TabIndex = 0;			
				this._ctlPageSelector.PageColor = Halassy.Controls.ControlProperties.ColorItemInBack();
				this._ctlPageSelector.AppIcon = (System.Drawing.Icon)(ReportSmart.RSResources.GFX.GetObject("Application_Icon"));
				//this._ctlPageSelector.Text = "ReportSmart Edit© (v" + CReportSmartCore.Core.Version.ToString() + ")";
				this._ctlPageSelector.Text = "ReportSmart Edit© (GUI preview)";
				this._ctlPageSelector.Glass = true;
				// 
				// MainForm
				// 
				this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
				this.Text = "bin";
				this.Name = "MainForm";
				this.Controls.Add(_ctlPageSelector);
				this.Size = new System.Drawing.Size(640, 480);
				this.Location = new System.Drawing.Point(32, 32);
				
				_ctlPageSelector.Size = new System.Drawing.Size(this.ClientRectangle.Width, 54);
				_ctlPageSelector.ControlHostForm();
			}
	}
}
