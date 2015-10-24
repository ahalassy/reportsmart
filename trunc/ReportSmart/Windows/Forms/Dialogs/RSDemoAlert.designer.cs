/*
 *
 * Licensing:			GPL
 * Original project:	bin.csproj
 *
 * Copyright: Adam Halassy (2009.12.02.)
 * 
 * 
 */

using ReportSmart.Application;

namespace ReportSmart.Controls
{
	partial class CdlgDemoAlert {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CdlgDemoAlert));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this._bOk = new System.Windows.Forms.Button();
			this._lInfo = new System.Windows.Forms.Label();
			this._lTitle = new System.Windows.Forms.Label();
			this._lUseFor = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(505, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(128, 128);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
			this.pictureBox2.Location = new System.Drawing.Point(505, 146);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(128, 128);
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			// 
			// _bOk
			// 
			this._bOk.BackColor = System.Drawing.SystemColors.Control;
			this._bOk.Location = new System.Drawing.Point(500, 397);
			this._bOk.Name = "_bOk";
			this._bOk.Size = new System.Drawing.Size(128, 32);
			this._bOk.TabIndex = 2;
			this._bOk.Text = "_bOk";
			this._bOk.UseVisualStyleBackColor = false;
			this._bOk.Click += new System.EventHandler(this._bOkClick);
			// 
			// _lInfo
			// 
			this._lInfo.BackColor = System.Drawing.Color.Transparent;
			this._lInfo.Location = new System.Drawing.Point(12, 146);
			this._lInfo.Name = "_lInfo";
			this._lInfo.Size = new System.Drawing.Size(487, 128);
			this._lInfo.TabIndex = 3;
			this._lInfo.Text = "_lInfo";
			// 
			// _lTitle
			// 
			this._lTitle.BackColor = System.Drawing.Color.Transparent;
			this._lTitle.Font = new System.Drawing.Font("Arial", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this._lTitle.Location = new System.Drawing.Point(12, 12);
			this._lTitle.Name = "_lTitle";
			this._lTitle.Size = new System.Drawing.Size(487, 40);
			this._lTitle.TabIndex = 4;
			this._lTitle.Text = "_lTitle";
			// 
			// _lUseFor
			// 
			this._lUseFor.Location = new System.Drawing.Point(12, 52);
			this._lUseFor.Name = "_lUseFor";
			this._lUseFor.Size = new System.Drawing.Size(487, 88);
			this._lUseFor.TabIndex = 5;
			this._lUseFor.Text = "_lUserFor";
			this._lUseFor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CdlgDemoAlert
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(640, 480);
			this.Controls.Add(this._lUseFor);
			this.Controls.Add(this._lTitle);
			this.Controls.Add(this._lInfo);
			this.Controls.Add(this._bOk);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.pictureBox1);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = global::ReportSmart.Application.Resources.GFX.Application_Icon;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CdlgDemoAlert";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ReportSmart View (A) RC1";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label _lUseFor;
		private System.Windows.Forms.Label _lTitle;
		private System.Windows.Forms.Button _bOk;
		private System.Windows.Forms.Label _lInfo;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBox1;
		
		protected void _bOkClick(object sender, System.EventArgs e) {
				try {
						if (Mode == SH_EXPIRED)
								RsViewEngine.KillApplication();
						this.Size = new System.Drawing.Size((int)(this.Size.Width / Mode), this.Size.Height);
						this.DialogResult = System.Windows.Forms.DialogResult.OK;
					} catch {
						RsViewEngine.KillApplication();
					}
			}
	}
}
