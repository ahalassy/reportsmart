#region Source information

//*****************************************************************************
//
//    CustomBrowser.cs
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
using System.Windows.Forms;

namespace ReportSmart.Controls
{
    internal class CCustomBrowserButton : CAnimatedControl
    {
        //private int _AniVal;

        private bool _checked;//, _mOver;
        private Color _textColor;

        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs aPEA)
        {
            Graphics lGraph = aPEA.Graphics;
            Pen lPen = new Pen(ForeColor);
            SolidBrush lBrush = new SolidBrush(BackColor);
            Font lTextFont = _checked ? new Font(this.Font, FontStyle.Bold) : this.Font;
            SizeF lTextSize = lGraph.MeasureString(Text, lTextFont);
            Point lTextPoint = new Point(
                        (int)(this.Width - lTextSize.Width) / 2,
                        (int)(this.Height - lTextSize.Height) / 2
                    );

            lGraph.SmoothingMode = SmoothingMode.AntiAlias;

            lGraph.FillRectangle(lBrush, ClientRectangle);
            Graph.Drawing.Draw.RoundedRect(
                        lGraph,
                        new Rectangle(0, 0, this.Width - 1, this.Height - 1),
                        16,
                        lPen,
                        null
                    );
            lGraph.DrawString(Text, lTextFont, new SolidBrush(_textColor), lTextPoint);
        }

        protected override void onTick()
        {
            Invalidate();
        }

        public CCustomBrowserButton()
        {
            ForeColor = Color.Blue;
            _textColor = Color.Black;
            _checked = false;
            //_AniVal = 0;

            Click += new EventHandler(ehClick);
        }

        protected void ehClick(object aSender, EventArgs aEArgs)
        {
            _checked ^= true;
            Invalidate();
        }

        protected void ehMouseEnter(object aSender, EventArgs aEArgs)
        {
            //_mOver = true;
            Animation = true;
        }

        protected void ehMouseLeave(object aSender, EventArgs aEArgs)
        {
            //_mOver = false;
            Animation = true;
        }
    }

    internal class CCustomBrowserTree
    {
    }

    internal class CCustomBrowser
    {

    }

}