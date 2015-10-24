/*
 * * $(DATE} (Adam Halassy)
 * 
 */

using ReportSmart.Application;

namespace ReportSmart.Controls
{
	partial class CfSplash
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
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
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CfSplash));
			this._lText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this._lText.BackColor = System.Drawing.Color.Transparent;
			this._lText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)), true);
			this._lText.Location = new System.Drawing.Point(12, 254);
			this._lText.Name = "label1";
			this._lText.Size = new System.Drawing.Size(287, 21);
			this._lText.TabIndex = 0;
			this._lText.Text = "label1";
			// 
			// CfSplash
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = (System.Drawing.Bitmap)(RsViewEngine.Resources.GetObject("Splash_BackgroundImage"));
			this.ClientSize = new System.Drawing.Size(480, 360);
			this.Controls.Add(this._lText);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = (System.Drawing.Icon)(RsViewEngine.Resources.GetObject("Application.Icon"));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximumSize = new System.Drawing.Size(480, 360);
			this.MinimumSize = new System.Drawing.Size(480, 360);
			this.Name = "CfSplash";
			this.Text = "CfSplash";
			this.ShowInTaskbar = false;			
			this.Shown += new System.EventHandler(this.CfSplashShown);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label _lText;
	}
}
