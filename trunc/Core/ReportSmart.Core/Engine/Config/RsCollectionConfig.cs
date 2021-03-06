#region Source information

//*****************************************************************************
//
//    RsCollectionConfig.cs
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

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ReportSmart.Engine.Config
{
    public enum RsCollectionType
    {
        Custom,
        Favorites,
        History
    }

    [XmlType(TypeName = "collection")]
    public class RsCollectionConfig : RsPathConfig
    {

        public bool IsCollectionExists()
        {
            return File.Exists(Path);
        }

        [XmlAttribute(AttributeName = "type")]
        public RsCollectionType Type { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

    }
}
