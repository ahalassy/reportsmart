/*
 * Adam Halassy
 * 2009.09.04.
 * 
 */

using System;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace ReportControl
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class CCRReportViewer {
			private string _ReportFile;
			private Control _Parent;
			private Panel _ctlBoard;
			private Panel _ctlMessages;
	
			public string ReportFile {
					get { return _ReportFile; }
					set { _ReportFile = value; }
				}
				
			public Control Parent {
					get { return _Parent; }
					set { _Parent = value; }
			}
			
			public CCRReportViewer() {
					
				}
				
			public void OpenReport() {
					_Parent.SuspendLayout();
					_ctlBoard = new Panel();
					_ctlBoard.Name = "CR_Board";
					_ctlBoard.Top = 0;
					_ctlBoard.Left = 0;
					_ctlBoard.Location = new System.Drawing.Point(0, 0);
					_ctlBoard.Size = new System.Drawing.Size(800, 600);
					//_ctlBoard.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
					_ctlBoard.BringToFront();
					_ctlBoard.Text = "Initializing report controls...";
					_Parent.Controls.Add(_ctlBoard);
					_Parent.ResumeLayout(false);
					_Parent.PerformLayout();
				}
		}
}
