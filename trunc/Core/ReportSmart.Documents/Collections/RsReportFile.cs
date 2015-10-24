#region Source information

//*****************************************************************************
//
//    RsReportFile.cs
//    Created by Adam (2015-10-23, 9:21)
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

using System.Xml;

namespace ReportSmart.Documents.Collections
{
    public class CReportFile : CReportItem
    {
        private string _File;

        public string ReportFile
        {
            get { return _File; }
            set
            {
                _File = value;
                edit();
            }
        }

        public override XmlNode BuildXML(XmlNode aParent, XmlDocument aDoc)
        {
            XmlNode lResult = base.BuildXML(aParent, aDoc);
            if (lResult == null) return null;
            XmlAttribute lPath = aDoc.CreateAttribute("path");
            lPath.Value = _File;
            lResult.Attributes.Append(lPath);
            return lResult;
        }

        public CReportFile(string aName, string aFile) : base(aName)
        {
            _File = aFile;
            _Type = "file";
            GUINode.SelectedImageIndex = 1;
            GUINode.ImageIndex = 1;
        }
    }
}
