#region Source information

//*****************************************************************************
//
//    RsReportMailSender.cs
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

using System.Collections.Generic;
using ReportSmart.Documents;
using ReportSmart.Network.Email;

namespace ReportSmart.Engine
{
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
