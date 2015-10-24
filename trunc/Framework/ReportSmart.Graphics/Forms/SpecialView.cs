#region Source information

//*****************************************************************************
//
//    SpecialView.cs
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
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;
using ReportSmart.Graph;
using ReportSmart.Graph.Drawing;
using ReportSmart.Localization;

namespace ReportSmart.Controls
{
    public delegate void SpecialPanelEventNotify(CSpecialPanel aSender);

    public class CSpecialPanelView : Panel
    {
        private int _HeaderSize;
        private Font _HeaderFont;

        public Font HeaderFont
        {
            get { return _HeaderFont; }
            set
            {
                _HeaderFont = value;
                Invalidate();
            }
        }

        public void UpdatePositions()
        {
            int _CurrentTop = _HeaderSize;

            foreach (Control lControl in Controls)
            {
                lControl.Location = new Point(0, _CurrentTop);
                _CurrentTop += lControl.Height + 16;
            }
        }

        public virtual void UpdatePanel()
        {
            foreach (Control iControl in this.Controls)
                if (iControl is CSpecialPanel)
                {
                    ((CSpecialPanel)iControl).UpdatePanel();
                }
        }

        public void AddPanel(CSpecialPanel aPanel)
        {
            //this.SuspendLayout();

            aPanel.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            aPanel.Location = new Point(0, 0);
            aPanel.Size = new Size(this.ClientRectangle.Width, aPanel.Height);
            aPanel.Resize += new EventHandler(EH_CtlResized);
            this.Controls.Add(aPanel);

            //this.ResumeLayout();
        }

        protected override void OnPaint(PaintEventArgs aPArgs)
        {
            Graphics lGraph = aPArgs.Graphics;
            lGraph.TextRenderingHint = TextRenderingHint.AntiAlias;
            Brush lBrush = new SolidBrush(this.ForeColor);
            lGraph.DrawString(this.Text, _HeaderFont, lBrush, 8, 8);
            _HeaderSize = (int)(lGraph.MeasureString(this.Text, _HeaderFont).Height) + 8 * 2;
            UpdatePositions();
        }

        public CSpecialPanelView()
        {
            _HeaderSize = 48;
            _HeaderFont = this.Font;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        protected void EH_CtlResized(object aSender, EventArgs aEArgs)
        {
            UpdatePositions();
        }
    }

    public class CSpecialPanel : CLocalizedPanel
    {
        public const int DEF_HEADERSIZE = 48;
        public const int DEF_INTERNALMARGIN = 8;

        private bool _Collapsed;
        private Color _clHeader, _clHFont;
        private int _HeaderSize, _BodySize;
        private string _Title;
        private Image _imgOpen, _imgClose;
        private PictureBox _OpenClose;

        public Image ImageOpen
        {
            get { return _imgOpen; }
            set
            {
                _imgOpen = value;
                updateOpenClose();
            }
        }

        public Image ImageClose
        {
            get { return _imgClose; }
            set
            {
                _imgClose = value;
                updateOpenClose();
            }
        }

        public new Padding Margin
        {
            get { return base.Margin; }
            set
            {
                base.Margin = value;
                if (_OpenClose != null)
                    _OpenClose.Location = GetOpenCloseLocation();
            }
        }

        protected Point GetOpenCloseLocation()
        {
            return new Point(
                        Margin.Left + (this.HeaderSize - 32) / 2,
                        (this.HeaderSize - 32) / 2
                    );
        }

        protected void updateOpenClose()
        {
            _OpenClose.Image = _Collapsed ? _imgOpen : _imgClose;
            _OpenClose.BackColor = Color.Transparent;
            _OpenClose.Location = GetOpenCloseLocation();
        }

        protected int availWidth { get { return this.ClientRectangle.Width - Margin.Left - Margin.Right; } }

        public event SpecialPanelEventNotify PanelUpdate;

        public virtual void UpdatePanel()
        {
            if (PanelUpdate != null)
                PanelUpdate(this);
        }

        public override string GetInstanceName() { return "CSpecialPanel"; }

        public bool Collapsed
        {
            get { return _Collapsed; }
            set
            {
                DetermineHeight();
                _Collapsed = value;
                if (!_Collapsed)
                    this.Size = new Size(this.Width, _HeaderSize + _BodySize);
                else
                    this.Size = new Size(this.Width, _HeaderSize);

                updateOpenClose();
            }
        }

        public Color HeaderColor
        {
            get { return _clHeader; }
            set
            {
                _clHeader = value;
                Invalidate();
            }
        }

        public Color HeaderFontColor
        {
            get { return _clHFont; }
            set
            {
                _clHFont = value;
                Invalidate();
            }
        }

        public int HeaderSize
        {
            get { return _HeaderSize; }
            set
            {
                _HeaderSize = value;
                Invalidate();
            }
        }

        public int BodyTop { get { return HeaderSize; } }

        public int BodyWidth
        {
            get { return this.availWidth; }
            set
            {
                _BodySize = value;
                if (!_Collapsed)
                {
                    this.Size = new Size(this.Width, _HeaderSize + _BodySize);
                }
            }
        }

        public int BodyLeft
        {
            get { return Margin.Left; }
        }

        public string PageTitle
        {
            get { return _Title; }
            set
            {
                _Title = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Determines the neccessary height for the panel
        /// </summary>
        public void DetermineHeight()
        {
            int lHeight = HeaderSize + Margin.Top + Margin.Bottom;
            foreach (Control lControl in Controls)
                lHeight = lHeight < (lControl.Bottom + Margin.Bottom) ? lControl.Bottom + Margin.Bottom : lHeight;

            this.Size = new Size(this.Width, lHeight);
            _BodySize = lHeight - _HeaderSize;
        }

        public override void ApplyLocale(CLocalization aLocale)
        {
            XmlNode lPanelInfo = aLocale.GetPanelData(this.Name);

            Locale.GetDialogData("valami");
            Locale.GetTagText("enabled");
            if (lPanelInfo != null)
            {
                PageTitle = XmlTools.getXmlNodeByName("title", lPanelInfo).InnerText;
            }
        }

        protected override void OnPaint(PaintEventArgs aPArgs)
        {

            Graphics lGraph = aPArgs.Graphics;
            lGraph.TextRenderingHint = TextRenderingHint.AntiAlias;
            Pen lPen = new Pen(Color.Black);
            lPen.Width = 2;
            Brush lbrHeader = new LinearGradientBrush(new Point(0, 0), new Point(0, _HeaderSize), this.BackColor, _clHeader);
            Brush lbrText = new SolidBrush(this.ForeColor);
            Font lHeaderFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold, this.Font.Unit);
            SizeF lTextSize = lGraph.MeasureString(_Title, lHeaderFont);

            lGraph.SmoothingMode = SmoothingMode.AntiAlias;

            Brush lBrush = new LinearGradientBrush(
                        new Point(Margin.Left, 0),
                        new Point(Margin.Left, _HeaderSize),
                        ColorTools.Darken(50, _clHeader),
                        _clHeader
                    );
            ReportSmart.Graph.Drawing.Draw.RoundedRect(
                    lGraph,
                    new Rectangle(Margin.Left + 1, 1, this.Width - (Margin.Size.Width) - 1, _HeaderSize - 1),
                    15,
                    null,
                    new SolidBrush(Color.FromArgb(0xa0, 0, 0, 0))
                );
            ReportSmart.Graph.Drawing.Draw.RoundedRect(
                    lGraph,
                    new Rectangle(Margin.Left, 0, this.Width - (Margin.Size.Width) - 1, _HeaderSize - 1),
                    15,
                    null,
                    lBrush
                );

            Draw.DrawShadowedText(
                        lGraph,
                        new Point(
                                    Margin.Left * 2 + (_OpenClose != null ? _OpenClose.Width : 0),
                                    (int)(_HeaderSize - lTextSize.Height) / 2
                                ),
                        _clHFont,
                        Color.FromArgb(0xa0, 0, 0, 0),
                        lHeaderFont,
                        _Title
                    );

            if (this.Parent != null)
            {
                lBrush = new SolidBrush(Parent.BackColor);
                lGraph.FillRectangle(lBrush, 0, 0, Margin.Left, this.ClientRectangle.Height);
                lGraph.FillRectangle(lBrush, this.ClientRectangle.Width - Margin.Right, 0, Margin.Right, this.ClientRectangle.Height);
            }


        }

        public CSpecialPanel() : base()
        {
            _HeaderSize = DEF_HEADERSIZE;
            _clHeader = Color.LightGray;
            _Collapsed = false;
            Margin = new Padding(_HeaderSize, 0, _HeaderSize, DEF_INTERNALMARGIN);

            _OpenClose = new PictureBox();
            _OpenClose.Size = new Size(32, 32);
            _OpenClose.Location = new Point(
                        Margin.Left + (this.HeaderSize - 32) / 2,
                        (this.HeaderSize - 32) / 2
                    );
            _OpenClose.Cursor = System.Windows.Forms.Cursors.Hand;

            this.Click += new EventHandler(ehClick);
            _OpenClose.Click += new EventHandler(ehOpenClose);

            DoubleBuffered = true;
            ResizeRedraw = true;

            this.Controls.Add(_OpenClose);
        }

        protected void ehOpenClose(object aSender, EventArgs aE)
        {
            Collapsed = !Collapsed;
        }

        protected void ehClick(object aSender, EventArgs aE)
        {
            if (this.PointToClient(MousePosition).Y < this.HeaderSize)
                ehOpenClose(aSender, aE);
        }
    }
}

