/*
 *
 * Licensing:			GPL
 * Original project:	bin.csproj
 *
 * Copyright: Adam ReportSmart (2009.10.14.)
 * 
 * 
 */
using System.Windows.Forms;

using ReportSmart.Application;

namespace ReportSmart.Controls
{
    partial class CRSAbout
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
            if (disposing)
            {
                if (components != null)
                {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CRSAbout));
            this._bOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = RsViewEngine.DefaultFont;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(481, 365);
            this.Controls.Add(this._bOk);
            this.Icon = global::ReportSmart.Application.Resources.GFX.Application_Icon;
            this.Name = "CRSAbout";
            this.Text = "Form1";
            // 
            // _bOk
            // 
            this._bOk.Location = new System.Drawing.Point(341, 296);
            this._bOk.Name = "_bOk";
            this._bOk.Size = new System.Drawing.Size(128, 32);
            this._bOk.TabIndex = 0;
            this._bOk.Text = "_bOk";
            this._bOk.UseVisualStyleBackColor = true;
            // 
            // CRSAbout
            // 

            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Button _bOk;
    }
}
