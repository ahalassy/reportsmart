#region Source information

//*****************************************************************************
//
//    AcroPDF.cs
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

using System.Windows.Forms;
using System.Runtime.InteropServices;
using AcroPDFLib;

namespace ReportSmart.Special.ActiveX.AcroPDF
{
    public class CAcroPDFCtl : AxHost
    {
        private static int _counter = 0;
        private AcroPDFLib.AcroPDF _pdfOcx = null;
        private int _zoom;

        public AcroPDFLib.AcroPDF AxAcroPDF { get { return _pdfOcx; } }

        protected override void AttachInterfaces()
        {
            _pdfOcx = ((AcroPDFLib.AcroPDF)(this.GetOcx()));
        }

        public void LoadFile(string aFileName)
        {
            _pdfOcx.LoadFile(aFileName);
            _zoom = 100;
        }

        public bool ShowBookmarks
        {
            set
            {
                if (value)
                    _pdfOcx.setPageMode("bookmarks");
                else
                    _pdfOcx.setPageMode("none");
            }
        }

        public bool ShowThumbnails
        {
            set
            {
                if (value)
                    _pdfOcx.setPageMode("thumbs");
                else
                    _pdfOcx.setPageMode("none");
            }
        }


        public void ShowToolBar(bool aShow) { _pdfOcx.setShowToolbar(aShow); }

        public void FirstPage() { _pdfOcx.gotoFirstPage(); }
        public void PrevPage() { _pdfOcx.gotoPreviousPage(); }
        public void NextPage() { _pdfOcx.gotoNextPage(); }
        public void LastPage() { _pdfOcx.gotoLastPage(); }

        public void ZoomIn()
        {
            _zoom += 10;
            _pdfOcx.setZoom(_zoom);
        }

        public void ZoomOut()
        {
            _zoom -= 10;
            _pdfOcx.setZoom(_zoom);
        }

        public void SetZoom(int aPercentage)
        {
            _zoom = aPercentage;
            _pdfOcx.setZoom(_zoom);
        }

        public void FitPage() { _pdfOcx.setView("Fit"); }

        public void FitWidth() { _pdfOcx.setView("FitH"); }

        public void Print() { _pdfOcx.Print(); }

        public CAcroPDFCtl() : base("CA8A9780-280D-11CF-A24D-444553540000")
        {
            Name = "CAcroPDFCtl" + _counter;
            _counter++;
        }
    }
}