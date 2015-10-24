/*
 * * $(DATE} (Adam Halassy)
 * 
 */

using ReportSmart.Application;

namespace ReportSmart.Controls
{
	partial class CfAddReport
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CfAddReport));
			this._bCancel = new System.Windows.Forms.Button();
			this._bOk = new System.Windows.Forms.Button();
			this._lMainTitle = new System.Windows.Forms.Label();
			this._lDetails = new System.Windows.Forms.Label();
			this._lReportFile = new System.Windows.Forms.Label();
			this._lAlias = new System.Windows.Forms.Label();
			this._eAlias = new System.Windows.Forms.TextBox();
			this._eReportFile = new System.Windows.Forms.TextBox();
			this._bBrowse = new System.Windows.Forms.Button();
			this._ctlEditor = new System.Windows.Forms.GroupBox();
			this._ctlEditor.SuspendLayout();
			this.SuspendLayout();
			// 
			// _bCancel
			// 
			this._bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._bCancel.Location = new System.Drawing.Point(478, 305);
			this._bCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this._bCancel.Name = "_bCancel";
			this._bCancel.Size = new System.Drawing.Size(128, 32);
			this._bCancel.TabIndex = 0;
			this._bCancel.Text = "Mégse";
			this._bCancel.UseVisualStyleBackColor = true;
			// 
			// _bOk
			// 
			this._bOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._bOk.Location = new System.Drawing.Point(344, 305);
			this._bOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this._bOk.Name = "_bOk";
			this._bOk.Size = new System.Drawing.Size(128, 32);
			this._bOk.TabIndex = 1;
			this._bOk.Text = "Ok";
			this._bOk.UseVisualStyleBackColor = true;
			this._bOk.Click += new System.EventHandler(this.clickOk);
			// 
			// _lMainTitle
			// 
			this._lMainTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this._lMainTitle.BackColor = System.Drawing.Color.Transparent;
			this._lMainTitle.Font = RsViewEngine.TitleFont;
			this._lMainTitle.Location = new System.Drawing.Point(14, 11);
			this._lMainTitle.Name = "_lMainTitle";
			this._lMainTitle.Size = new System.Drawing.Size(592, 64);
			this._lMainTitle.TabIndex = 2;
			this._lMainTitle.Text = "_lMainTitle";
			this._lMainTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _lDetails
			// 
			this._lDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this._lDetails.Location = new System.Drawing.Point(12, 79);
			this._lDetails.BackColor = System.Drawing.Color.Transparent;
			this._lDetails.Name = "_lDetails";
			this._lDetails.Size = new System.Drawing.Size(594, 64);
			this._lDetails.TabIndex = 3;
			this._lDetails.Text = "_lDetails";
			this._lDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _lReportFile
			// 
			this._lReportFile.Location = new System.Drawing.Point(147, 23);
			this._lReportFile.Name = "_lReportFile";
			this._lReportFile.Size = new System.Drawing.Size(250, 20);
			this._lReportFile.TabIndex = 4;
			this._lReportFile.Text = "_lReportFile";
			// 
			// _lAlias
			// 
			this._lAlias.Location = new System.Drawing.Point(6, 23);
			this._lAlias.Name = "_lAlias";
			this._lAlias.Size = new System.Drawing.Size(107, 20);
			this._lAlias.TabIndex = 5;
			this._lAlias.Text = "_lAlias";
			// 
			// _eAlias
			// 
			this._eAlias.Location = new System.Drawing.Point(6, 46);
			this._eAlias.Name = "_eAlias";
			this._eAlias.Size = new System.Drawing.Size(135, 22);
			this._eAlias.TabIndex = 6;
			// 
			// _eReportFile
			// 
			this._eReportFile.Location = new System.Drawing.Point(147, 46);
			this._eReportFile.Name = "_eReportFile";
			this._eReportFile.Size = new System.Drawing.Size(440, 22);
			this._eReportFile.TabIndex = 7;
			// 
			// _bBrowse
			// 
			this._bBrowse.Location = new System.Drawing.Point(459, 82);
			this._bBrowse.Name = "_bBrowse";
			this._bBrowse.Size = new System.Drawing.Size(128, 32);
			this._bBrowse.TabIndex = 8;
			this._bBrowse.Text = "_bBrowse";
			this._bBrowse.UseVisualStyleBackColor = true;
			this._bBrowse.Click += new System.EventHandler(clickBrowse);
			// 
			// _ctlEditor
			// 
			this._ctlEditor.Controls.Add(this._eAlias);
			this._ctlEditor.Controls.Add(this._bBrowse);
			this._ctlEditor.Controls.Add(this._lAlias);
			this._ctlEditor.Controls.Add(this._eReportFile);
			this._ctlEditor.Controls.Add(this._lReportFile);
			this._ctlEditor.Location = new System.Drawing.Point(12, 160);
			this._ctlEditor.Name = "_ctlEditor";
			this._ctlEditor.Size = new System.Drawing.Size(593, 120);
			this._ctlEditor.TabIndex = 9;
			this._ctlEditor.TabStop = false;
			this._ctlEditor.Text = "_ctlEditor";
			// 
			// CfAddReport
			// 
			this.AcceptButton = this._bOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._bCancel;
			this.ClientSize = new System.Drawing.Size(620, 350);
			this.Controls.Add(this._ctlEditor);
			this.Controls.Add(this._lDetails);
			this.Controls.Add(this._lMainTitle);
			this.Controls.Add(this._bOk);
			this.Controls.Add(this._bCancel);
			this.DoubleBuffered = true;
			this.Font = RsViewEngine.DefaultFont;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = (System.Drawing.Icon)(RsViewEngine.Resources.GetObject("Application.Icon"));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CfAddReport";
			this.Text = "Riport hozzáadása";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.CfAddReportPaint);
			this._ctlEditor.ResumeLayout(false);
			this._ctlEditor.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label _lDetails;
		private System.Windows.Forms.GroupBox _ctlEditor;
		private System.Windows.Forms.Button _bBrowse;
		private System.Windows.Forms.TextBox _eReportFile;
		private System.Windows.Forms.TextBox _eAlias;
		private System.Windows.Forms.Label _lAlias;
		private System.Windows.Forms.Label _lReportFile;
		private System.Windows.Forms.Button _bOk;
		private System.Windows.Forms.Label _lMainTitle;
		private System.Windows.Forms.Button _bCancel;
	}
}
