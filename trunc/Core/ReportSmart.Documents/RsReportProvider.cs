#region Source information

//*****************************************************************************
//
//    RsReportProvider.cs
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
//using System.Windows.Forms;


using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ReportSmart.Documents
{

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