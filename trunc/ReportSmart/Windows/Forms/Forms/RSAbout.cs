#region Source information

//*****************************************************************************
//
//    RSAbout.cs
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

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;
using ReportSmart.Application;
using ReportSmart.Localization;

namespace ReportSmart.Controls
{
    /// <summary>
    /// Description of Form1.
    /// </summary>
    internal partial class CRSAbout : CLocalizedForm
    {
        public override string GetInstanceName() { return "CRSAbout"; }

        public override void ApplyLocale(CLocalization aLocale)
        {
            XmlNode lData = aLocale.GetFormData("About");

            this.Text = XmlTools.getXmlNodeByName("caption", lData).InnerText;
            _bOk.Text = aLocale.GetTagText("Ok");
        }

        public CRSAbout()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            _bOk.Click += new EventHandler(EH_Ok);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.ApplyLocale(RsViewEngine.Locale);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics aGraph = e.Graphics;

            aGraph.TextRenderingHint = TextRenderingHint.AntiAlias;
            Brush lTextBrush = new SolidBrush(Color.White);

            aGraph.DrawString(RsViewEngine.Version.ToString(), this.Font, lTextBrush, 410, 103);
        }

        protected void EH_Ok(object aSender, EventArgs aEArgs)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
