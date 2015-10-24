/*
 *
 * Licensing:			GPL
 * Original project:	
 *
 * Copyright: Adam Halassy (2010.11.26.)
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


using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;

using Halassy;
using Halassy.Controls;
using Halassy.Network;
using Halassy.Network.Email;
using Halassy.Security;
using Halassy.Special;
using Halassy.Special.WinApi;

using ReportSmart;

namespace ReportSmart.Documents {

		public enum RsExportFormat {
				PortableFormatDocument,
				ExcelDocument,
				WordDocument,
				XmlDocument,
				HtmlDocument
			}

		public class RsReportProvider {		
				public static RsReportProvider OpenReport(string aFileName) {
						RsReportProvider lResult = new RsReportProvider();
						lResult.Open(aFileName);
						
						return lResult;
					}
					
				#region Properties:
				public ReportDocument Document { get; protected set; }
				public bool Opened { get; protected set; }
				public int ParamCount { get { return Opened ? Document.ParameterFields.Count : -1; }}
				public DataSourceConnections Connections {	get { return Opened ? Document.DataSourceConnections : null; }}
				public RsReportExporter ReportExporter { get; set; }
				public string ReportTitle { get; set; }
				#endregion
				
				#region GetReportSet
				/// <summary>
				/// Returns with all subreports, including the subreports
				/// </summary>
				/// <param name="aFileName"></param>
				public List<ReportDocument> GetReportSet() {
						if (!Opened) return null;
				
						List<ReportDocument> lResult = new List<ReportDocument>();
						
						lResult.Add(Document);
						foreach (ReportDocument iDoc in Document.Subreports)
								lResult.Add(iDoc);
							
						return lResult;
					}
				#endregion
				
				public List<InternalConnectionInfo> GetAllConnections() {
						if (!Opened) return null;
				
						List<ReportDocument> lAllReports = GetReportSet();
						List<InternalConnectionInfo> lResult = new List<InternalConnectionInfo>();
						
						foreach (ReportDocument iDoc in lAllReports)
							foreach (InternalConnectionInfo iInfo in iDoc.DataSourceConnections)
								lResult.Add(iInfo);
								
						return lResult;
					}
				
				public void ExportReport(string aFileName) {
						if (ReportExporter != null)
							ReportExporter.Export(aFileName);
					}
				
				public void Open(string aFileName) {
						Document = new ReportDocument();
						Document.Load(aFileName);
						Opened = true;
					}
					
				public void Close() {
						Opened = false;
						Document.Close();
					}
					
				public RsReportProvider() {
						Opened = false;
					}
					
				public RsReportProvider(string aFileName) {
						Opened = false;
						Open(aFileName);
					}
					
			}
	}