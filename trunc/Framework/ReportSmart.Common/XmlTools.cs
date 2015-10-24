#region Source information

//*****************************************************************************
//
//    XmlTools.cs
//    Created by Adam (2015-10-24, 10:22)
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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ReportSmart
{
    public static partial class XmlTools
    {
        #region XML Serialization:
        /// <summary>
        /// Converts object to XML string
        /// </summary>
        /// <param name="aObject">Object to be serialized<</param>/
        /// <returns>string (XML format)</returns>
        public static string Serialize(object aObject)
        {
            string lRes = null;

            MemoryStream lMemStream = new MemoryStream();
            XmlSerializer lSerializer = new XmlSerializer(aObject.GetType());
            XmlTextWriter lWriter = new XmlTextWriter(lMemStream, Encoding.UTF8);
            lSerializer.Serialize(lWriter, aObject);
            lMemStream = (MemoryStream)lWriter.BaseStream;
            UTF8Encoding lEncoding = new UTF8Encoding();
            lRes = lEncoding.GetString(lMemStream.ToArray());

            return lRes;
        }

        public static void Serialize(object aObject, string aFileName)
        {
            FileStream lStream = new FileStream(aFileName, FileMode.Create);
            XmlSerializer lSerializer = new XmlSerializer(aObject.GetType());
            XmlTextWriter lWriter = new XmlTextWriter(lStream, Encoding.UTF8);
            lSerializer.Serialize(lWriter, aObject);
        }


        /// <summary>
        /// XML string to object
        /// </summary>
        /// <param name="aXMLString">A valid XML string<</param>/
        /// <param name="aType">Type of result</param>
        /// <returns>object</returns>
        public static object Deserialize(string aXMLString, Type aType)
        {
            XmlSerializer lSerializer = new XmlSerializer(aType);
            MemoryStream lMemStream = new MemoryStream((new UTF8Encoding()).GetBytes(aXMLString));
            XmlTextWriter lWriter = new XmlTextWriter(lMemStream, Encoding.UTF8);

            return lSerializer.Deserialize(lMemStream);
        }

        public static object DeserializeFromFile(string aFileName, Type aType)
        {
            XmlSerializer lSerializer = new XmlSerializer(aType);
            FileStream lStream = new FileStream(aFileName, FileMode.Open);
            XmlTextWriter lWriter = new XmlTextWriter(lStream, Encoding.UTF8);

            return lSerializer.Deserialize(lStream);
        }
        #endregion
    }
}