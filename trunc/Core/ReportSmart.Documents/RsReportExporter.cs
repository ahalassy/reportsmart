#region Source information

//*****************************************************************************
//
//    RsReportExporter.cs
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


using CrystalDecisions.Shared;

namespace ReportSmart.Documents
{

    public enum RsHtmlVersion
    {
        Html40,
        Html32
    }

    public abstract class RsReportExporter
    {
        public RsReportProvider Source { get; protected set; }

        protected ExportOptions ExportOptions { get; set; }

        public abstract string GetDocumentExtension();

        public virtual void Export(string aFileName)
        {
            ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;

            DiskFileDestinationOptions lDestOpt = new DiskFileDestinationOptions();
            lDestOpt.DiskFileName = aFileName;
            ExportOptions.DestinationOptions = lDestOpt;

            if (Source.Opened)
                Source.Document.Export(ExportOptions);
        }

        public RsReportExporter(RsReportProvider aProvider)
        {
            ExportOptions = new ExportOptions();
            Source = aProvider;
        }
    }

    #region Excel:
    public class RsRptToExcelExporter : RsReportExporter
    {
        public bool DataOnly { get; set; }

        public override string GetDocumentExtension() { return "xls"; }

        public override void Export(string aFileName)
        {
            ExportOptions.ExportFormatType = DataOnly ?
                        ExportFormatType.ExcelRecord :
                        ExportFormatType.Excel;

            base.Export(aFileName);
        }

        public RsRptToExcelExporter(RsReportProvider aProvider) : base(aProvider)
        {
            DataOnly = false;
        }
    }
    #endregion

    #region PDF:
    public class RsRptToPortabeFormatDocumentExporter : RsReportExporter
    {
        public override void Export(string aFileName)
        {
            ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;

            base.Export(aFileName);
        }

        public override string GetDocumentExtension() { return "pdf"; }

        public RsRptToPortabeFormatDocumentExporter(RsReportProvider aProvider) : base(aProvider) { }
    }
    #endregion

    #region Html:
    public class RsRptToHtmlExporter : RsReportExporter
    {
        public RsHtmlVersion HtmlVersion { get; set; }

        public override string GetDocumentExtension() { return "html"; }

        public override void Export(string aFileName)
        {
            switch (HtmlVersion)
            {
                case RsHtmlVersion.Html32: ExportOptions.ExportFormatType = ExportFormatType.HTML32; break;
                default: ExportOptions.ExportFormatType = ExportFormatType.HTML40; break;
            }

            base.Export(aFileName);
        }

        public RsRptToHtmlExporter(RsReportProvider aProvider) : base(aProvider)
        {
            HtmlVersion = RsHtmlVersion.Html40;
        }
    }
    #endregion

    #region Word:
    public class RsRptToWordExporter : RsReportExporter
    {
        public override void Export(string aFileName)
        {
            ExportOptions.ExportFormatType = ExportFormatType.WordForWindows;

            base.Export(aFileName);
        }

        public override string GetDocumentExtension() { return "doc"; }

        public RsRptToWordExporter(RsReportProvider aProvider) : base(aProvider) { }
    }
    #endregion

    #region RichTextFormat:
    public class RsRptToRichTextExporter : RsReportExporter
    {
        public override void Export(string aFileName)
        {
            ExportOptions.ExportFormatType = ExportFormatType.RichText;

            base.Export(aFileName);
        }

        public override string GetDocumentExtension() { return "rtf"; }

        public RsRptToRichTextExporter(RsReportProvider aProvider) : base(aProvider) { }
    }
    #endregion

    #region Xml:
    public class RsRptToXmlExporter : RsReportExporter
    {
        public override void Export(string aFileName)
        {
            ExportOptions.ExportFormatType = ExportFormatType.Xml;

            base.Export(aFileName);
        }

        public override string GetDocumentExtension() { return "xml"; }

        public RsRptToXmlExporter(RsReportProvider aProvider) : base(aProvider) { }
    }
    #endregion
}
