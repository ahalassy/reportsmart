/*
 *
 * Licensing:			GPL
 * Original project:	RsDocuments.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.18.)
 * 
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;

using ReportSmart;
using ReportSmart.Documents;

namespace ReportSmart.Documents.Collections {
		/// <summary>
		/// Description of RsCollectionReport.
		/// </summary>
		public class RsCollectionReport: RsCollectionItem {
		
			public CReportFile ReportFile {
					get { return ReportItem as CReportFile; }
				}
		
			public RsCollectionReport(CReportFile aReportFile): base(aReportFile) {}
	}
}
