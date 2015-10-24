/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.09.)
 * 
 * 
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using ReportSmart;
using ReportSmart.Forms;
using ReportSmart.Localization;
using ReportSmart.Network;
using ReportSmart.Network.Email;
using ReportSmart.Security;
using ReportSmart.Special;
using ReportSmart.Special.WinApi;

using ReportSmart.Engine;
using ReportSmart.Engine.Config;
using ReportSmart.Controls;
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;
 
namespace ReportSmart.Application {
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
