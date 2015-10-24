/*
 * * $(DATE} (Adam ReportSmart)
 * 
 */

using ReportSmart.Application;

namespace ReportSmart
{
    partial class CReportSmartForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private ReportSmart.Controls.CPageSelector _ctlPageSelector = null;
        private ReportSmart.Controls.CPageSelectorPage _pgCollections, _pgSettings;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CReportSmartForm));
            this._ctlPageSelector = new ReportSmart.Controls.CPageSelector();
            this.CollectionManagement = new ReportSmart.Windows.Forms.RsCollectionManagement();
            this._ctlSettingsPanel = new ReportSmart.Controls.CRSSettingsPanel();
            this._ctlPagesHost = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // _ctlPageSelector
            // 
            this._ctlPageSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((
                        (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                        System.Windows.Forms.AnchorStyles.Right
                    )));
            this._ctlPageSelector.Font = new System.Drawing.Font("Arial", _FontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._ctlPageSelector.Host = null;
            this._ctlPageSelector.Location = new System.Drawing.Point(0, 0);
            this._ctlPageSelector.Name = "_ctlPageSelector";
            this._ctlPageSelector.PageWidth = 200;
            this._ctlPageSelector.SelectedPage = -1;
            this._ctlPageSelector.SelectedPageColor = System.Drawing.Color.White;
            this._ctlPageSelector.TabIndex = 0;
            this._ctlPageSelector.PageColor = ReportSmart.Controls.ControlProperties.ColorItemInBack();
            this._ctlPageSelector.AppIcon = (System.Drawing.Icon)(ReportSmart.RSResources.GFX.GetObject("Application_Icon"));
            this._ctlPageSelector.Text = "ReportSmart View© (v" + RsViewEngine.Version.ToString() + ")";
            this._ctlPageSelector.Glass = true;
            // 
            // _ctlCollectionPanel
            // 
            this.CollectionManagement.Name = "_ctlCollectionPanel";
            this.CollectionManagement.TabIndex = 1;
            RsViewEngine.Locale.AddLocalizedControl(CollectionManagement);
            //
            // _ctlPagesHost
            // 
            this._ctlPagesHost.Location = new System.Drawing.Point(0, 48 * 3);
            this._ctlPagesHost.Name = "_ctlPagesHost";
            this._ctlPagesHost.BackColor = System.Drawing.Color.Red;
            this._ctlPagesHost.Anchor = ((System.Windows.Forms.AnchorStyles)((
                        (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                        (System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom)
                    )));
            //
            // ctlCollectionEditor
            //
            this.CollectionManagement.CollectionBrowser.ImageOpen = (System.Drawing.Image)(RsViewEngine.Resources.GetObject("gfx_arrow_right_32x32"));
            this.CollectionManagement.CollectionBrowser.ImageClose = (System.Drawing.Image)(RsViewEngine.Resources.GetObject("gfx_arrow_down_32x32"));
            this.CollectionManagement.CollectionBrowser.FolderImage = (System.Drawing.Image)(RsViewEngine.Resources.GetObject("gfx_folder_32x32"));
            this.CollectionManagement.CollectionBrowser.HeaderColor = ReportSmart.Application.Defaults.RsGfx.HeaderColor;
            this.CollectionManagement.CollectionBrowser.Font = this._ctlPageSelector.Font = new System.Drawing.Font("Arial", _FontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            // 
            // CReportSmartForm
            // 	
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Size = RsViewEngine.ProfileManager.Profile.Settings.UserInterface.WindowConfig.GetwindowSize();
            this.Location = RsViewEngine.ProfileManager.Profile.Settings.UserInterface.WindowConfig.GetwindowLocation();

            if (RsViewEngine.ProfileManager.Profile.Settings.UserInterface.WindowConfig.Maximized)
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            this.MinimumSize = new System.Drawing.Size(768, 480);
            this.Controls.Add(this._ctlPagesHost);
            this.Controls.Add(this._ctlPageSelector);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CReportSmartForm";
            this.Text = "ReportSmart";

            this._ctlPageSelector.Size = new System.Drawing.Size(this.ClientRectangle.Width, 36);
            this._ctlPagesHost.Size = new System.Drawing.Size(this.ClientRectangle.Width, this.ClientRectangle.Height - 3 * 48);

            this._ctlPageSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this._ctlPagesHost.Dock = System.Windows.Forms.DockStyle.Fill;

            this.ResumeLayout(false);

            //_ctlPageSelector.ControlHostForm();
        }

        public ReportSmart.Windows.Forms.RsCollectionManagement CollectionManagement { get; protected set; }
        private ReportSmart.Controls.CRSSettingsPanel _ctlSettingsPanel;
        private System.Windows.Forms.Panel _ctlPagesHost;
    }
}
