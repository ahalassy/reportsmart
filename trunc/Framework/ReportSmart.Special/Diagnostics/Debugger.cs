/*
 *
 * Licensing:			GPL
 * Original project:	Halassy.Special.csproj
 *
 * Copyright: Adam Halassy (2010.12.13.)
 * 
 * 
 */
using System;

namespace Halassy.Diagnostics {
	/// <summary>
	/// Description of Debugger.
	/// </summary>
	public static class Debugger {
	
			public static void DropLog(string aMsg) {
					System.Diagnostics.Debugger.Log(0, "halassy", aMsg + "\n");
				}
		}
}
