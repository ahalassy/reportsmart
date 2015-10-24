/*
 * 2009-09-21 (Adam ReportSmart)
 * 
 */
 
using ReportSmart;
using ReportSmart.Controls;
using ReportSmart.Forms;
using ReportSmart.Localization;
using System.Drawing;
using System.Windows.Forms;

namespace ReportSmart.Controls {
		internal class CdlgCreateCollection: CMyDialog {
				private Label _lCollectionName;
				private TextBox _eCollectionName;
		
				public string CollectionName {
						get { return _eCollectionName.Text; }
						set { _eCollectionName.Text = value; }
					}
				public override string GetInstanceName() { return "CdlgCreateCollection"; }
				public override void ApplyLocale(CLocalization aSender) {
						base.ApplyLocale(aSender);
						
						_lCollectionName.Text = XmlTools.getXmlNodeByName(_lCollectionName.Name, DialogLocale).InnerText;
					}
		
				protected override string buildTitle(CLocalization aLocale) {
						return XmlTools.getXmlNodeByName("title", DialogLocale).InnerText;
					}
			
				public CdlgCreateCollection(): base() {
				
						this.SuspendLayout();
						_lCollectionName = new Label();
						_lCollectionName.Location = new Point(LABEL_MARGIN, CustomAreaTop);
						_lCollectionName.Size = new Size(CustomAreaWidth, 16);
						_lCollectionName.Name = "lCollectionName";
						
						_eCollectionName = new TextBox();
						_eCollectionName.Location = new Point(LABEL_MARGIN, CustomAreaTop + LABEL_MARGIN + _lCollectionName.Size.Height);
						_eCollectionName.Size = new Size(CustomAreaWidth, 14);
						_eCollectionName.Name = "eCollectionName";
						
						this.Controls.Add(_lCollectionName);
						this.Controls.Add(_eCollectionName);
						this.ResumeLayout(false);
						
						this.Name = "dlgCreateCollection";
					}
			}
	}
