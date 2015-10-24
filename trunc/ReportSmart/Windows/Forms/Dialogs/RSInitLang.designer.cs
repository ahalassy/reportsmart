/*
 *
 * Licensing:			GPL
 * Original project:	bin.csproj
 *
 * Copyright: Adam Halassy (2009.11.13.)
 * 
 * 
 */
namespace ReportSmart.Controls {
	partial class CRSInitLang
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
			this._lChooseLang = new System.Windows.Forms.Label();
			this._ctlLangList = new System.Windows.Forms.ListBox();
			this._bOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _lChooseLang
			// 
			this._lChooseLang.Location = new System.Drawing.Point(14, 11);
			this._lChooseLang.Name = "_lChooseLang";
			this._lChooseLang.Size = new System.Drawing.Size(313, 54);
			this._lChooseLang.TabIndex = 0;
			this._lChooseLang.Text = "label1";
			// 
			// _ctlLangList
			// 
			this._ctlLangList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this._ctlLangList.FormattingEnabled = true;
			this._ctlLangList.ItemHeight = 16;
			this._ctlLangList.Location = new System.Drawing.Point(14, 68);
			this._ctlLangList.Name = "_ctlLangList";
			this._ctlLangList.Size = new System.Drawing.Size(313, 84);
			this._ctlLangList.TabIndex = 1;
			// 
			// _bOk
			// 
			this._bOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._bOk.Location = new System.Drawing.Point(199, 168);
			this._bOk.Name = "_bOk";
			this._bOk.Size = new System.Drawing.Size(128, 32);
			this._bOk.TabIndex = 2;
			this._bOk.Text = "Ok";
			this._bOk.UseVisualStyleBackColor = true;
			this._bOk.Click += new System.EventHandler(this._bOkClick);
			// 
			// CRSInitLang
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(341, 212);
			this.Controls.Add(this._bOk);
			this.Controls.Add(this._ctlLangList);
			this.Controls.Add(this._lChooseLang);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CRSInitLang";
			this.ShowIcon = false;
			this.Text = "ReportSmart";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button _bOk;
		private System.Windows.Forms.ListBox _ctlLangList;
		private System.Windows.Forms.Label _lChooseLang;
	}
}
