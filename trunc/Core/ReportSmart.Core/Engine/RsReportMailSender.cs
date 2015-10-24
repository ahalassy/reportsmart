/*
 *
 * Licensing:			GPL
 * Original project:	RsReportProvider.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.04.)
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
using System.Xml;
using ReportSmart.Controls;
using ReportSmart.Network;
using ReportSmart.Network.Email;
using ReportSmart.Security;
using ReportSmart.Special;
using ReportSmart.Special.WinApi;

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
