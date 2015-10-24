/*
 *
 * Licensing:			GPL
 * Original project:	ReportSmart.Special.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.13.)
 * 
 * 
 */
using System;

namespace ReportSmart.Diagnostics {
	/// <summary>
	/// Description of Debugger.
	/// </summary>
	public static class Debugger {
	
			public static void DropLog(string aMsg) {
					System.Diagnostics.Debugger.Log(0, "ReportSmart", aMsg + "\n");
				}
		}
}
