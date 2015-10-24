#region Source information

//*****************************************************************************
//
//    RSAddExCollection.cs
//    Created by Adam (2009-09-17, 8:57)
//
// ---------------------------------------------------------------------------
//
//    Report Smart View
//    Copyright (C) 2009-2015, Adam Halassy
//
// ---------------------------------------------------------------------------
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
//*****************************************************************************

#endregion

using System.Drawing;
using System.Windows.Forms;
using ReportSmart.Forms;
using ReportSmart.Localization;

namespace ReportSmart.Controls
{
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
