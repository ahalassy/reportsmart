#region Source information

//*****************************************************************************
//
//    MyDialog.cs
//    Created by Adam (2015-10-23, 8:59)
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
using ReportSmart.Localization;

namespace ReportSmart.Forms
{
    public abstract class CMyDialog : CLocalizedForm
    {
        public static Font DefaultTitleFont { get; set; }
        public static Font MessageFont { get; set; }
        public static Icon ApplicationIcon { get; set; }

        protected const int LABEL_MARGIN = 11;

        private Button _bOk, _bCancel;
        private Label _lTitle, _lDescription;
        private bool _ShDesc;

        public bool Modify;

        internal delegate DialogResult dShowMyDialog();

        public bool ShowDescription
        {
            get { return _ShDesc; }
            set
            {
                lDescription.Visible = value;
                _ShDesc = value;
                Invalidate();
            }
        }

        protected XmlNode DialogLocale;

        protected int CustomAreaTop { get { return _lDescription.Location.Y + (ShowDescription ? _lDescription.Size.Height : 0) + 2 * LABEL_MARGIN; } }
        protected int CustomAreaBottom { get { return _bOk.Location.Y - LABEL_MARGIN * 2; } }
        protected int CustomAreaWidth { get { return _lDescription.Width; } }

        protected Button bOk { get { return _bOk; } }
        protected Button bCancel { get { return _bCancel; } }
        protected Label lTitle { get { return _lTitle; } }
        protected Label lDescription { get { return _lDescription; } }

        public string Title
        {
            get { return _lTitle.Text; }
            set { _lTitle.Text = value; }
        }

        public string Description
        {
            get { return _lDescription.Text; }
            set { _lDescription.Text = value; }
        }

        public Font TitleFont
        {
            get { return _lTitle.Font; }
            set { _lTitle.Font = value; }
        }

        protected abstract string buildTitle(CLocalization aLocale);

        public override void ApplyLocale(CLocalization aSender)
        {
            DialogLocale = aSender.GetDialogData(this.Name);
            _bOk.Text = aSender.GetTagText("ok");
            _bCancel.Text = aSender.GetTagText("cancel");
            _lTitle.Text = buildTitle(aSender);
            if (DialogLocale != null)
            {
                _lDescription.Text = XmlTools.getXmlNodeByName("description", DialogLocale).InnerText;
                this.Text = XmlTools.getXmlNodeByName("caption", DialogLocale).InnerText;
            }
        }

        public CMyDialog() : base()
        {
            Modify = false;

            this.SuspendLayout();

            this.ClientSize = new Size(620, 350);
            this.Icon = ApplicationIcon;
            this.Font = MessageFont;
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.DoubleBuffered = true;

            _bCancel = new Button();
            _bCancel.Size = new Size(128, 32);
            _bCancel.Location = new Point(this.ClientSize.Width - 128 - LABEL_MARGIN, this.ClientSize.Height - 32 - LABEL_MARGIN);
            _bCancel.Name = "_bCancel";
            _bCancel.Text = _bCancel.Name;
            _bCancel.Anchor = (AnchorStyles)(AnchorStyles.Right | AnchorStyles.Bottom);

            _bOk = new Button();
            _bOk.Size = new Size(128, 32);
            _bOk.Location = new Point(this.ClientSize.Width - (128 + LABEL_MARGIN) * 2, this.ClientSize.Height - 32 - LABEL_MARGIN);
            _bOk.Name = "_bOk";
            _bOk.Text = _bOk.Name;
            _bOk.Click += new EventHandler(dialogAccept);
            _bOk.Anchor = (AnchorStyles)(AnchorStyles.Right | AnchorStyles.Bottom);

            _lTitle = new Label();
            _lTitle.Location = new Point(LABEL_MARGIN, LABEL_MARGIN);
            _lTitle.Size = new Size(594, 64);
            _lTitle.Font = DefaultTitleFont;
            _lTitle.TextAlign = ContentAlignment.MiddleLeft;
            _lTitle.BackColor = Color.Transparent;
            _lTitle.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);

            _lDescription = new Label();
            _lDescription.Location = new Point(LABEL_MARGIN, 2 * LABEL_MARGIN + 64);
            _lDescription.Size = new Size(594, 64);
            _lDescription.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            _lDescription.BackColor = Color.Transparent;

            this.AcceptButton = _bOk;
            this.CancelButton = _bCancel;

            this.Paint += new PaintEventHandler(dialogPaint);
            this.Shown += new EventHandler(dialogShow);

            this.Controls.Add(_bOk);
            this.Controls.Add(_bCancel);
            this.Controls.Add(_lDescription);
            this.Controls.Add(_lTitle);

            this.ResumeLayout(false);

            _ShDesc = true;
        }

        protected virtual void dialogShow(object aSender, EventArgs aEArgs)
        {
            _lTitle.Text = buildTitle(null);
        }

        protected virtual void dialogAccept(object Sender, EventArgs aEArgs)
        {
            this.DialogResult = DialogResult.OK;
        }

        protected void dialogPaint(object Sender, PaintEventArgs aPArgs)
        {
            int lVBorder1 = ((_lDescription.Location.Y) + (_lTitle.Location.Y + _lTitle.Size.Height)) / 2;
            int lVSize2 = _lDescription.Size.Height + LABEL_MARGIN;
            Graphics lGraph = aPArgs.Graphics;
            Brush lBrush = new SolidBrush(Color.White);
            lGraph.FillRectangle(
                        lBrush,
                        0,
                        0,
                        this.ClientRectangle.Width,
                        lVBorder1
                    );
            if (lDescription.Visible)
            {
                lBrush = new SolidBrush(Color.FromArgb(0xCC, 0xCC, 0xFF));
                lGraph.FillRectangle(
                            lBrush,
                            0,
                            lVBorder1,
                            this.ClientRectangle.Width,
                            lVSize2
                        );
                for (int i = 4; i >= 0; i--)
                    lGraph.DrawLine(
                                new Pen(Color.FromArgb(i * i * 255 / 16, 0, 0, 0)),
                                0,
                                lVBorder1 + lVSize2 + (4 - i),
                                this.ClientRectangle.Width,
                                lVBorder1 + lVSize2 + (4 - i)
                            );
            }
            else
            {
                for (int i = 4; i >= 0; i--)
                    lGraph.DrawLine(
                                new Pen(Color.FromArgb(i * i * 255 / 16, 0, 0, 0)),
                                0,
                                lVBorder1 + (4 - i),
                                this.ClientRectangle.Width,
                                lVBorder1 + (4 - i)
                            );
            }
        }
    }
}
