#region Source information

//*****************************************************************************
//
//    DocumentViewer.cs
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

namespace ReportSmart.Special.DocumentView
{
    public abstract class CDocumentEngine
    {

        private Control _DocControl;

        protected Control docControl
        {
            get { return _DocControl; }
            set { _DocControl = value; }
        }

        public Control DocumentControl { get { return _DocControl; } }

        public abstract void LoadDocument(string aDocument);
        public abstract bool IsOpenable();
    }

    public class CPDFDocEngine : CDocumentEngine
    {
        public AxHost PDFControl { get { return (DocumentControl as AxHost); } }

        public override void LoadDocument(string aDocument)
        {
            //System.	


        }

        public override bool IsOpenable()
        {
            return true;
        }
    }

    public class CDocViewer
    {
    }
}