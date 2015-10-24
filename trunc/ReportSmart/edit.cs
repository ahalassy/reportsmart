/*
 *
 * Licensing:			GPL
 * Original project:	
 *
 * Copyright: Adam Halassy (2010.06.10.)
 * 
 * 
 */
using System;
using System.Windows.Forms;

namespace ReportSmart.Application {
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program {
			/// <summary>
			/// Program entry point.
			/// </summary>
			[STAThread]
			private static void Main(string[] args) {
					System.Windows.Forms.Application.EnableVisualStyles();
					System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
					System.Windows.Forms.Application.Run(new MainForm());
				}
		
		}
}
