#region Source information

//*****************************************************************************
//
//    RsFileDialogFilters.cs
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

using System.Xml;

namespace ReportSmart.Application
{
    internal static class FileDialogFilters {
		
				public static string BuildCollectionFilter(XmlNode aDialogLocale) {
						string lResult = XmlTools.getXmlNodeByAttrVal("name", "allFiles", aDialogLocale).InnerText + " (*.*)|*.*";
						lResult += "|" + XmlTools.getXmlNodeByAttrVal("name", "collection", aDialogLocale).InnerText + " (*.rfc)|*.rfc";
						return lResult;
					}
					
				public static string BuildReportFilter(XmlNode aDialogLocale) {
						string lResult = XmlTools.getXmlNodeByAttrVal("name", "allFiles", aDialogLocale).InnerText + " (*.*)|*.*";
						lResult += "|" + XmlTools.getXmlNodeByAttrVal("name", "reportFile", aDialogLocale).InnerText + " (*.rpt)|*.rpt";
						return lResult;
					}
					
				public static string BuildExcelFilter(XmlNode aDialogLocale) {
						string lResult = XmlTools.getXmlNodeByAttrVal("name", "allFiles", aDialogLocale).InnerText + " (*.*)|*.*";
						lResult += "|" + XmlTools.getXmlNodeByAttrVal("name", "excel", aDialogLocale).InnerText + " (*.xls)|*.xls";
						return lResult;
					}
					
				public static string BuildHTMLFilter(XmlNode aDialogLocale) {
						string lResult = XmlTools.getXmlNodeByAttrVal("name", "allFiles", aDialogLocale).InnerText + " (*.*)|*.*";
						lResult += "|" + XmlTools.getXmlNodeByAttrVal("name", "html", aDialogLocale).InnerText + " (*.htm, *.html)|*.html;*.htm";
						return lResult;
					}
					
				public static string BuildPDFFilter(XmlNode aDialogLocale) {
						string lResult = XmlTools.getXmlNodeByAttrVal("name", "allFiles", aDialogLocale).InnerText + " (*.*)|*.*";
						lResult += "|" + XmlTools.getXmlNodeByAttrVal("name", "pdf", aDialogLocale).InnerText + " (*.pdf)|*.pdf";
						return lResult;
					}

				public static string BuildWordFilter(XmlNode aDialogLocale) {
						string lResult = XmlTools.getXmlNodeByAttrVal("name", "allFiles", aDialogLocale).InnerText + " (*.*)|*.*";
						lResult += "|" + XmlTools.getXmlNodeByAttrVal("name", "word", aDialogLocale).InnerText + " (*.doc)|*.doc";
						return lResult;
					}

				public static string BuildXMLFilter(XmlNode aDialogLocale) {
						string lResult = XmlTools.getXmlNodeByAttrVal("name", "allFiles", aDialogLocale).InnerText + " (*.*)|*.*";
						lResult += "|" + XmlTools.getXmlNodeByAttrVal("name", "xml", aDialogLocale).InnerText + " (*.xml)|*.xml";
						return lResult;
					}

			}
	}
