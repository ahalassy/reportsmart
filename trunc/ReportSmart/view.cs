#region Source information

//*****************************************************************************
//
//    view.cs
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

using System;
using System.Windows.Forms;

using ReportSmart;
using ReportSmart.Special;
using ReportSmart.Special.WinApi;

using ReportSmart.Application;
using ReportSmart.Controls;

namespace ReportSmart {
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program {
		#if DEMO
		private const int _YM     = 140;
		private const int _MM     =  38;
		private const int _DM     =   6;
		private const int _PERIOD =  30;
		#endif
	
		/// <summary>
		/// Program entry point.
		/// </summary>
		/// 
		
		[STAThread]
		private static void Main(string[] args) {		
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(true);
			
			if (!CEmbeddedApplication.IsEmbeddedApplication(args)) {			
					RsViewEngine.InitializeCore();
					RsViewEngine.InitializeApplication();
					
					string lRpt = "";
				
					// Argument processing:
					for (int i = 0; i < args.Length; i++) {
							if (args[i] == "-o" || args[i] == "--open")
									lRpt = args.Length > i+1 ? args[i+1] : "";
						}

					#if DEBUG
					#warning RS View: DEBUG build
					ReportSmart.Controls.CRSMessageBox.ShowBox("ReportSmart View now running in DEBUG mode.", "Debug");
					#elif (DEMO)
					#warning RS View: DEMO build
					#else
					#warning RS View: RELEASE build
					#endif
					
					#if DEMO
					try {
							string lProjID = RsViewEngine.GetProjectID();
							
							long lVal = AppAuth.chkshwauthfrw(AppAuth.crdtstamp(RsViewEngine.ReferenceTime, _YM, _MM, _DM), _YM, _MM, _DM, _PERIOD);
							long lChkval = AppAuth.gnchkval(_YM, _MM, _DM, _PERIOD);
							
							CdlgDemoAlert.ShowDemoAlert(lVal, lChkval, _PERIOD);
			
							if ((lVal - lChkval) <= 0) {
									RsViewEngine.KillApplication();
								}
								
							Math.Sqrt((lVal - lChkval));
						} catch {
							RsViewEngine.KillApplication();
						}
					#endif
					
					if (lRpt != "")
							RsViewEngine.MainForm.OpenReport(FileSystem.NameOf(lRpt), lRpt);
					
					System.Windows.Forms.Application.Run(RsViewEngine.MainForm);					
					
				} else {
					CEmbeddedApplication lEmbApp = new CEmbeddedApplication(args[1]);
					RsViewEngine.InitializeCore();
					RsViewEngine.InitializeEmbedded(lEmbApp);
					CRSReportViewerEmbedded lViewer = new CRSReportViewerEmbedded();
					lViewer.EmbeddedApplication = lEmbApp;
					lViewer.ReportFile = args[2];
					lViewer.ReportTitle = args[3];
					lViewer.Location = new System.Drawing.Point(0, 0);
					lViewer.Size = new System.Drawing.Size(640, 480);
					lViewer.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
					lViewer.EmbeddedApplication.IntegrateControl(lViewer);
					Messaging.SendMessage(lViewer.EmbeddedApplication.Handle, Messaging.WM_REGISTERME, (uint)lViewer.Handle, 0);
					lViewer.OpenReport();
					System.Windows.Forms.Application.ApplicationExit += new EventHandler(RsViewEngine.EH_ApplicationExit);
					System.Windows.Forms.Application.Run();
				}

		}
		
	}
}
