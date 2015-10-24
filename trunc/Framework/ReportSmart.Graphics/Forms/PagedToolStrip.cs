#region Source information

//*****************************************************************************
//
//    PagedToolStrip.cs
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
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using ReportSmart.Controls;
using ReportSmart.Graph;

namespace ReportSmart.Forms
{
    public delegate void ToolItemAction(StripToolItem aSender);

    public class CPagedToolStrip : Control
    {
        // Constant:
        public const int DEF_PAGESHEIGHT = 32;
        public const int DEF_ITEMMARGIN = 8;
        public const int DEF_ICONSPACING = 16;

        // Data members:
        private int _PagesH, _Selected, _Highlighted;
        private int _IconSpacing = DEF_ICONSPACING;
        private ArrayList _ToolPages;
        private Color _PColor;
        private Size _IconSize;
        private StripToolPageset _PageSet = null;
        protected internal StripToolItem hlItem = null;
        private ToolTip _ToolTip;
        private Image _ToolBG;

        // Properties:
        public int PagesTop
        {
            get { return this.ClientSize.Height - _PagesH; }
        }
        protected Point TopLeft { get { return new Point(0, 0); } }
        protected Point TopRight { get { return new Point(this.ClientSize.Width - 1, 0); } }
        protected Point CenterLeft { get { return new Point(0, PagesTop); } }
        protected Point CenterRight { get { return new Point(this.ClientSize.Width - 1, PagesTop); } }
        protected Point BottomLeft { get { return new Point(0, this.ClientSize.Height - 1); } }
        protected Point BottomRight { get { return new Point(this.ClientSize.Width - 1, this.ClientSize.Height - 1); } }

        public Image StripBackground
        {
            get { return _ToolBG; }
            set
            {
                _ToolBG = value;
                Invalidate();
            }
        }
        public Color PagesColor
        {
            get { return _PColor; }
            set
            {
                _PColor = value;
                Invalidate();
            }
        }
        public int IconSpacing
        {
            get { return _IconSpacing; }
            set
            {
                _IconSpacing = value;
                Invalidate();
            }
        }
        public int PagesHeight
        {
            get { return _PagesH; }
            set
            {
                _PagesH = value;
                Invalidate();
            }
        }
        public int Highlighted
        {
            get { return _Highlighted; }
        }
        public StripToolItem HighlightedItem
        {
            get { return hlItem; }
        }
        public StripToolPage this[string aIndex]
        {
            get { return GetItem(aIndex); }
        }
        public StripToolPage SelectedPage
        {
            get
            {
                return _Selected < 0 ? null : _Selected >= _ToolPages.Count ? null : (StripToolPage)(_ToolPages[_Selected]);
            }

            set
            {
                if (_ToolPages.IndexOf(value) < 0)
                    this.Add(value);

                _Selected = _ToolPages.IndexOf(value);
                Invalidate();
            }
        }
        public Size IconSize
        {
            get { return _IconSize; }
            set
            {
                _IconSize = value;
                Invalidate();
            }
        }
        public StripToolPageset PageSet
        {
            get { return _PageSet; }
            set
            {
                _PageSet = value;
                if (_PageSet != null)
                    for (int i = 0; i < _PageSet.Count; i++)
                        this.Add(_PageSet[i]);

                Invalidate();
            }
        }

        // Methods:
        protected void setHiglightIcon(StripToolItem aIcon)
        {
            foreach (object iPage in _ToolPages)
            {
                if (getItem(_Selected) == iPage)
                    ((StripToolPage)iPage).setHighLighted(aIcon);
                else
                    ((StripToolPage)iPage).setHighLighted(null);
            }
        }

        protected StripToolPage getItem(int aIndex)
        {
            return aIndex < _ToolPages.Count && aIndex > -1 ? (StripToolPage)(_ToolPages[aIndex]) : null;
        }

        protected int getItemWidth(int aIndex)
        {
            return getItem(aIndex).Visible ? (int)(this.CreateGraphics().MeasureString(getItem(aIndex).Title, this.Font).Width) + 2 * DEF_ITEMMARGIN : 0;
        }

        protected int calculateItemLeft(int iNo)
        {
            int lResult = 0;
            Graphics lGraph = this.CreateGraphics();

            for (int i = 0; i < _ToolPages.Count && i <= iNo; i++)
                lResult += getItemWidth(i);

            return lResult;
        }

        protected int getItemAt(int aX)
        {
            int lW = 0;

            for (int i = 0; i < _ToolPages.Count; i++)
            {
                lW += getItemWidth(i);
                if (aX < lW)
                    return i;
            }

            return -1;
        }

        protected void updateByPoint(Point aPoint)
        {
            StripToolItem lLastItem = HighlightedItem;

            setHiglightIcon(null);
            if (aPoint.Y > PagesTop)
            {
                _Highlighted = getItemAt(aPoint.X);
                _ToolTip.Active = false;
            }
            else
            {
                _Highlighted = -1;
                if (getItem(_Selected) != null)
                    getItem(_Selected).updateByPoint(aPoint);

                if (HighlightedItem != null && HighlightedItem.Hint != "" && lLastItem != HighlightedItem)
                    _ToolTip.SetToolTip(this, HighlightedItem.Hint);
                _ToolTip.Active = HighlightedItem != null;
            }



            Invalidate();
        }

        protected StripToolPage GetItem(string aIndex)
        {
            int lIndex = IndexOf(aIndex);
            return lIndex < 0 ? null : (StripToolPage)(_ToolPages[lIndex]);
        }

        protected GraphicsPath buildArrow(int aSize, int aLeft, int aWidth, int aTop)
        {
            int lTriLeft = (aWidth - aSize) / 2 + aLeft;

            Point lA = new Point(lTriLeft, aTop);
            Point lB = new Point(lTriLeft + aSize / 2, aTop + aSize / 2);
            Point lC = new Point(lTriLeft + aSize, aTop);

            GraphicsPath lResult = new GraphicsPath();
            lResult.AddLine(lA, lB);
            lResult.AddLine(lB, lC);

            return lResult;
        }

        protected GraphicsPath buildBorder(GraphicsPath aArrow)
        {
            GraphicsPath lResult = new GraphicsPath();

            lResult.AddLine(TopLeft, BottomLeft);
            lResult.AddLine(BottomLeft, CenterLeft);
            lResult.AddPath(aArrow, true);
            lResult.AddLine(CenterRight, TopRight);
            lResult.AddLine(TopRight, BottomRight);

            return lResult;
        }

        protected override void OnClick(EventArgs e)
        {
            if (_Highlighted >= 0)
            {
                _Selected = _Highlighted;
                Invalidate();
            }
            else
            {
                if (hlItem != null)
                    hlItem.DoAction();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            updateByPoint(Control.MousePosition);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _Highlighted = -1;
            setHiglightIcon(null);
            _ToolTip.Active = false;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs aMArgs)
        {
            base.OnMouseMove(aMArgs);

            updateByPoint(aMArgs.Location);
        }

        protected override void OnPaint(PaintEventArgs aPArgs)
        {
            int lCurrW = 0, lAllW = 0;
            GraphicsPath lPath = lPath = buildArrow(12, 0, 64, this.ClientSize.Height - _PagesH);
            Graphics lGraph = aPArgs.Graphics;
            lGraph.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            StringFormat lStrFormat = new StringFormat();
            lStrFormat.Alignment = StringAlignment.Center;
            lStrFormat.LineAlignment = StringAlignment.Center;

            // Brush lbrPages = new SolidBrush(_PColor);
            LinearGradientBrush lbrPages = new LinearGradientBrush(
                    new Rectangle(0, 0, this.Width, _PagesH + 16),
                    Graph.ColorTools.SetBrightness((byte)(Graph.ColorTools.RGB2HSB(_PColor).Brightness / 3), _PColor),
                    _PColor,
                    90
                );
            Brush lbrHlPage = new SolidBrush(_PColor);
            Brush lbrToolStrip = new SolidBrush(Color.White);
            Brush lTextBrush = new SolidBrush(Color.Black);
            Pen lPen = new Pen(Color.Black);


            lGraph.SmoothingMode = SmoothingMode.None;

            if (_ToolBG != null)
            {
                for (int iX = 0; iX < this.Width; iX += _ToolBG.Size.Width)
                    lGraph.DrawImage(
                                _ToolBG,
                                new Rectangle(iX, 0, _ToolBG.Width, _ToolBG.Height),
                                new Rectangle(0, 0, _ToolBG.Width, _ToolBG.Height),
                                GraphicsUnit.Pixel
                            );

            }
            else
            {
                LinearGradientBrush lbrStrip = new LinearGradientBrush(
                            new Rectangle(0, 0, this.ClientSize.Height - PagesHeight, this.ClientSize.Height - PagesHeight),
                            Color.Black, Color.Black,
                            90,
                            false
                    );
                ColorBlend lbrBlend = new ColorBlend(4);
                lbrBlend.Colors = new Color[] {
                                            Color.FromArgb(0xff, 0xff, 0xff),
                                            Color.FromArgb(0xe9, 0xe9, 0xe9),
                                            Color.FromArgb(0xb9, 0xb9, 0xb9),
                                            Color.FromArgb(0x7b, 0x7b, 0x7b)
                                    };
                lbrBlend.Positions = new float[] { 0.0f, 0.45f, 0.45f, 1f };
                lbrStrip.InterpolationColors = lbrBlend;
                lGraph.FillRectangle(lbrStrip, 0, 0, this.ClientSize.Width, this.ClientSize.Height - PagesHeight);
            }

#if DEMO
						Font myFont = new Font("Arial", 20.25F, ((FontStyle)((FontStyle.Bold | FontStyle.Italic))), GraphicsUnit.Point, ((byte)(238)));
						Brush myBrush = new SolidBrush(Color.FromArgb(20, Color.Black));
						SizeF myStrSize = lGraph.MeasureString("This is a non-commercial DEMO", myFont);
						
						int lLeft = this.ClientSize.Width - (int)(myStrSize.Width) - 32;
						int lTop = (this._ToolBG.Height - (int)(myStrSize.Height)) / 2;
						
						lGraph.DrawString("This is a non-commercial DEMO", myFont, myBrush, lLeft, lTop);
						
#endif

            lGraph.SmoothingMode = SmoothingMode.HighQuality;
            lGraph.FillRectangle(lbrPages, 0, this.ClientSize.Height - PagesHeight, this.ClientSize.Width, PagesHeight);
            // Draw pages:
            lTextBrush = new SolidBrush(Color.White);
            for (int i = 0; i < _ToolPages.Count; i++)
            {
                StripToolPage lCurrPage = getItem(i);
                if (!lCurrPage.Visible)
                    continue;

                lCurrW = (int)((lGraph.MeasureString(lCurrPage.Title, this.Font)).Width);
                lCurrW += 2 * DEF_ITEMMARGIN;
                RectangleF lLayout = new Rectangle();
                lLayout.Size = new Size(lCurrW, _PagesH);
                lLayout.Location = new Point(lAllW + 1, this.ClientSize.Height - _PagesH + 1);

                if (_Highlighted == i)
                {
                    Brush lbrHLighted = new SolidBrush(ColorTools.SetAlpha(ControlProperties.HiglightingRate, Color.White));
                    lGraph.FillRectangle(lbrHLighted, lAllW, PagesTop, lCurrW, _PagesH);
                }

                lGraph.SmoothingMode = SmoothingMode.AntiAlias;
                lGraph.DrawString(lCurrPage.Title, this.Font, new SolidBrush(Graph.ColorTools.SetAlpha(128, Color.Black)), lLayout, lStrFormat);
                lLayout.Location = new Point(lAllW, this.ClientSize.Height - _PagesH);
                lGraph.DrawString(lCurrPage.Title, this.Font, lTextBrush, lLayout, lStrFormat);

                if (_Selected == i)
                {
                    lPath = buildArrow(12, lAllW, lCurrW, this.ClientSize.Height - _PagesH);
                }

                lAllW += lCurrW;
            }

            lGraph.SmoothingMode = SmoothingMode.None;
            lGraph.FillPath(lbrToolStrip, lPath);
            lGraph.SmoothingMode = SmoothingMode.HighQuality;
            //lGraph.DrawPath(lPen, buildBorder(lPath));

            //lGraph.DrawLine(lPen, 0, 0, 0, this.Height);
            //lGraph.DrawLine(lPen, this.ClientRectangle.Width, 0, this.ClientRectangle.Width, this.Height);

            if (getItem(_Selected) != null)
                getItem(_Selected).DrawPage(lGraph, new Size(this.ClientSize.Width, PagesTop));
        }

        public StripToolPage Add(StripToolPage aPage)
        {
            if (_ToolPages.IndexOf(aPage) < 0)
            {
                _ToolPages.Add(aPage);
                aPage.setHost(this);
            }

            if (_Selected < 0)
                _Selected = 0;

            Invalidate();

            return aPage;
        }

        public StripToolPage Add(string aPage)
        {
            return this.Add(new StripToolPage(this, aPage));
        }

        public void Remove(StripToolPage aPage)
        {
            if (_ToolPages.IndexOf(aPage) > -1)
                _ToolPages.Remove(aPage);
        }

        public int IndexOf(string aPageTitle)
        {
            StripToolPage iItem = null;
            aPageTitle = aPageTitle.ToUpper();

            for (int i = 0; i < _ToolPages.Count; i++)
            {
                iItem = ((StripToolPage)_ToolPages[i]);
                if (aPageTitle == iItem.Title.ToUpper())
                    return i;
            }

            return -1;
        }

        // Constructors:
        public CPagedToolStrip() : base()
        {
            _PagesH = DEF_PAGESHEIGHT;
            _ToolPages = new ArrayList();
            _PColor = ControlProperties.ColorItemInBack();
            _Selected = -1;
            _Highlighted = -1;
            _IconSize = new Size(16, 16);
            _ToolBG = null;

            _ToolTip = new ToolTip();
            _ToolTip.SetToolTip(this, "toolTip");

            ResizeRedraw = true;
            DoubleBuffered = true;
        }

        // Event handlers:
    }
}