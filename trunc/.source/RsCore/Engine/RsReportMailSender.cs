/*
 *
 * Licensing:			GPL
 * Original project:	RsReportProvider.csproj
 *
 * Copyright: Adam Halassy (2010.12.04.)
 * 
 * 
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Timers;
//using System.Windows.Forms;
using System.Xml;

//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
//using CrystalDecisions.Windows.Forms;

using Halassy;
using Halassy.Controls;
using Halassy.Network;
using Halassy.Network.Email;
using Halassy.Security;
using Halassy.Special;
using Halassy.Special.WinApi;

using ReportSmart;
using ReportSmart.Documents;

namespace ReportSmart.Engine {
		public class RsReportMailSender {
				protected List<string> AttachmentFiles;
				
				public CMapiMail Mail { get; protected set; }
				
				public RsReportProvider ReportProvider { get; protected set; }
		
				public void AddAttachment(RsExportFormat aFormat) {
						switch (aFormat) {
								case RsExportFormat.ExcelDocument:
										ReportProvider.ReportExporter = new RsRptToExcelExporter(ReportProvider); break;
										
								case RsExportFormat.HtmlDocument:
										ReportProvider.ReportExporter = new RsRptToHtmlExporter(ReportProvider); break;
										
								case RsExportFormat.WordDocument:
										ReportProvider.ReportExporter = new RsRptToWordExporter(ReportProvider); break;
										
								case RsExportFormat.XmlDocument:
										ReportProvider.ReportExporter = new RsRptToXmlExporter(ReportProvider); break;
								
								default:
										ReportProvider.ReportExporter = new RsRptToPortabeFormatDocumentExporter(ReportProvider); break;
							}
					}
		
				public void AddAttachment(RsReportExporter aExporter) {
						string lFileName = FileSystem.GetTempFileName(
									RsApplicationInfo.ApplicationName,
									aExporter.GetDocumentExtension()
								);
				
						aExporter.Export(lFileName);
						
						Mail.Attachments += lFileName;
					}
		
				public void Send() { Mail.StartMail(); }
		
				public RsReportMailSender(RsReportProvider aReportProvider) {
						AttachmentFiles = new List<string>();
						Mail = new CMapiMail();
					}
			}
	}
