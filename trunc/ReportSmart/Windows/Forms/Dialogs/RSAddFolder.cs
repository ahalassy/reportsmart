/*
 * 2009-09-17 (Adam Halassy)
 * 
 */
 
using System.Drawing;
using System.Windows.Forms;

using Halassy;
using Halassy.Controls;
using Halassy.Forms;
using Halassy.Localization;

using ReportSmart;
using ReportSmart.Application;

namespace ReportSmart.Controls {
		internal class CdlgAddFolder: CMyDialog {
				private Label _lFolderName;
				private TextBox _eFolderName;
		
				public string CollectionName;
				
				public string FolderName {
						get { return _eFolderName.Text; }
						set { _eFolderName.Text = value; }
					}
				
				public override string GetInstanceName() { return "CdlgAddFolder"; }
				public override void ApplyLocale(CLocalization aSender) {
						base.ApplyLocale(aSender);
						
						_lFolderName.Text = XmlTools.getXmlNodeByName(_lFolderName.Name, DialogLocale).InnerText;
					}
		
				protected override string buildTitle(CLocalization aLocale) {
							return XmlTools.getXmlNodeByName("title1", DialogLocale).InnerText +
										" \"" + CollectionName + "\" " +
										XmlTools.getXmlNodeByName("title2", DialogLocale).InnerText;
					}
				
				protected override void dialogAccept(object Sender, System.EventArgs aEArgs) {
						if (_eFolderName.Text == "")
								CRSMessageBox.ShowBox(
											XmlTools.getXmlNodeByAttrVal("name", "noName", DialogLocale).InnerText,
											RsViewEngine.Locale.GetTagText("error"),
											MessageBoxButtons.OK,
											MessageBoxIcon.Exclamation
										);
							else
								base.dialogAccept(Sender, aEArgs);
							
					}
				
				public CdlgAddFolder(): base() {
						this.Name = "dlgAddFolder";
		
						this.SuspendLayout();
						_lFolderName = new Label();
						_lFolderName.Location = new Point(LABEL_MARGIN, this.CustomAreaTop);
						_lFolderName.Size = new Size(CustomAreaWidth, 14);
						_lFolderName.Name = "lFolderName";
						
						_eFolderName = new TextBox();
						_eFolderName.Location = new Point(LABEL_MARGIN, _lFolderName.Top + _lFolderName.Height + LABEL_MARGIN);
						_eFolderName.Size = new Size(CustomAreaWidth, 14);
						_eFolderName.Name = "eFolderName";
						
						this.Controls.Add(_lFolderName);
						this.Controls.Add(_eFolderName);
						this.ResumeLayout(false);
					}
			}
	}
