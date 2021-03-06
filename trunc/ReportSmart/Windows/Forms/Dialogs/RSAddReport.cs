﻿#region Source information

//*****************************************************************************
//
//    RSAddReport.cs
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
using System.Windows.Forms;
using System.Xml;
using ReportSmart.Application;
using ReportSmart.Localization;

namespace ReportSmart.Controls
{
    /// <summary>
    /// Description of Form1.
    /// </summary>
    internal partial class CfAddReport : CLocalizedForm
    {
        private string _CollName;

        public bool Modify;

        public string Alias
        {
            get { return _eAlias.Text; }
            set { _eAlias.Text = value; }
        }

        public string ReportFile
        {
            get { return _eReportFile.Text; }
            set { _eReportFile.Text = value; }
        }

        public string CollectionName
        {
            get { return _CollName; }
            set
            {
                _CollName = value;
                _lMainTitle.Text = RsViewEngine.Locale.GetAddReportTitle(_CollName);
            }
        }

        public override string GetInstanceName() { return "CfAddReport"; }
        public override void ApplyLocale(CLocalization aLocale)
        {
            XmlNode lData = aLocale.GetDialogData("AddReport");

            _bOk.Text = aLocale.GetTagText("ok");
            _bCancel.Text = aLocale.GetTagText("cancel");
            _bBrowse.Text = aLocale.GetTagText("browse");
            _lAlias.Text = XmlTools.getXmlNodeByName("lAlias", lData).InnerText;
            _lReportFile.Text = XmlTools.getXmlNodeByName("lReportFile", lData).InnerText;
            _lDetails.Text = XmlTools.getXmlNodeByAttrVal("name", "details", lData).InnerText;
            _ctlEditor.Text = XmlTools.getXmlNodeByAttrVal("name", "group", lData).InnerText;
        }

        public CfAddReport()
        {
            _CollName = "";
            Modify = false;
            InitializeComponent();

            this.Shown += new EventHandler(showDialog);

            RsViewEngine.Locale.AddLocalizedControl(this);
        }

        private void showDialog(object aSender, EventArgs aEArgs)
        {
            if (!Modify)
            {
                _eAlias.Text = "";
                _eReportFile.Text = "";
            }
        }

        private void clickOk(object aSender, EventArgs aEArgs)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void clickBrowse(object aSender, EventArgs aEArgs)
        {
            DialogResult lDlgRes = RsViewEngine.dlgOpenReport.ShowDialog();
            if (lDlgRes == DialogResult.OK)
            {
                _eReportFile.Text = RsViewEngine.dlgOpenReport.FileName;
                if (_eAlias.Text == "")
                {
                    _eAlias.Text = FileSystem.NameOf(_eReportFile.Text);
                }
            }
        }

        void CfAddReportPaint(object aSender, PaintEventArgs aPArgs)
        {
            int lVBorder1 = ((_lDetails.Location.Y) + (_lMainTitle.Location.Y + _lMainTitle.Size.Height)) / 2;
            //int lVBorder2 = ((_ctlEditor.Location.Y) + (_lDetails.Location.Y + _lDetails.Size.Height)) / 2;
            Graphics lGraph = aPArgs.Graphics;
            Brush lBrush = new SolidBrush(Color.White);
            lGraph.FillRectangle(
                        lBrush,
                        0,
                        0,
                        this.ClientRectangle.Width,
                        lVBorder1
                    );

            lBrush = new SolidBrush(Color.FromArgb(0xCC, 0xCC, 0xFF));
            lGraph.FillRectangle(
                        lBrush,
                        0,
                        lVBorder1,
                        this.ClientRectangle.Width,
                        _lDetails.Size.Height + (_lDetails.Location.Y - lVBorder1)
                    );
            for (int i = 4; i >= 0; i--)
                lGraph.DrawLine(
                            new Pen(Color.FromArgb(i * i * 255 / 16, 0, 0, 0)),
                            0,
                            lVBorder1 + _lDetails.Size.Height + (_lDetails.Location.Y - lVBorder1) + (4 - i),
                            this.ClientRectangle.Width,
                            lVBorder1 + _lDetails.Size.Height + (_lDetails.Location.Y - lVBorder1) + (4 - i)
                        );
        }
    }
}
