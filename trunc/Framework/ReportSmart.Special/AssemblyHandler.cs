/*
 *
 * Licensing:			GPL
 * Original project:	cs.FindCRPlugin.csproj
 *
 * Copyright: Adam Halassy (2010.07.29.)
 * 
 * 
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Halassy.Special {
		public static class DotNetAsmInfo {
				[DllImport("DotNetAsmInfo.dll", CharSet= CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
				public static extern string GetAssemblyInfoString(string aAssemblyFile);
				
				[DllImport("DotNetAsmInfo.dll", CharSet=CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  				public static extern string GetAssemblyName(string aAsmInfoStr);
  				
  				[DllImport("DotNetAsmInfo.dll", CharSet=CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  				public static extern string GetAssemblyVersion(string aAsmInfoStr);
  				
  				[DllImport("DotNetAsmInfo.dll", CharSet=CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  				public static extern string GetAssemblyCulture(string aAsmInfoStr);
  				
  				[DllImport("DotNetAsmInfo.dll", CharSet=CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
  				public static extern string GetAssemblyPublicKeyToken(string aAsmInfoStr);
  			}

		public class CAssemblyInfo {
  				string _infoStr, _iniFile;
				
				public string Culture { get { return DotNetAsmInfo.GetAssemblyCulture(_infoStr); }}
				public string DisplayName { get { return DotNetAsmInfo.GetAssemblyName(_infoStr); }}
				public string PublicKeyToken { get { return DotNetAsmInfo.GetAssemblyPublicKeyToken(_infoStr); }}
				public string Version { get { return DotNetAsmInfo.GetAssemblyVersion(_infoStr); }}
		
				public override string ToString() {
						return DisplayName + " (" + Version + ')';
					}
				
				public CAssemblyInfo(string aAssemblyIni) {
						_iniFile = aAssemblyIni;
						_infoStr = DotNetAsmInfo.GetAssemblyInfoString(_iniFile);					
					}
			}

		public static class GACUtil {

		
				private static void gatherAssemblyInfo(string aPath, List<CAssemblyInfo> aList) {
						foreach (string iDir in Directory.GetDirectories(aPath)) {
								gatherAssemblyInfo(iDir, aList);
							}
						
						foreach (string iFile in Directory.GetFiles(aPath, "*.ini")) {
								bool lOk = true;
								CAssemblyInfo lAsmInfo = null;
								
								try {
										lAsmInfo = new CAssemblyInfo(iFile);
										
									} catch {
										lOk = false;
									}
									
								if (lOk)	
										aList.Add(lAsmInfo);
							}
					}
		
				public static List<CAssemblyInfo> GatherAssemblies() {
						List<CAssemblyInfo> lResult = new List<CAssemblyInfo>();
						
						#if DOTNET40
						string lPath = Environment.GetEnvironmentVariable("WINDIR") + "\\Microsoft.NET\\assembly";
						#else
						string lPath = Environment.GetEnvironmentVariable("WINDIR") + "\\assembly";
						#endif
						gatherAssemblyInfo(lPath, lResult);
						
						return lResult;
						
					}
					
				public static CAssemblyInfo GetAssembly(string aClass) {
						List<CAssemblyInfo> lAssemblies = FindAssemblies(aClass);
						if (lAssemblies.Count > 0)
								return lAssemblies[0];
							else
								return null;
						
					}
					
				public static List<CAssemblyInfo> FindAssemblies(string aClass) {
						aClass = aClass.ToUpper();
						List<CAssemblyInfo> lAssemblies = new List<CAssemblyInfo>();
						List<CAssemblyInfo> lResult = new List<CAssemblyInfo>();
						
						lAssemblies = GatherAssemblies();
						
						if ("*" == aClass)
								return lAssemblies;
							else {
								foreach (CAssemblyInfo iAsm in lAssemblies) {
										string lDspName = iAsm.DisplayName.ToUpper();
										//System.Windows.Forms.MessageBox.Show(iAsm.DisplayName + " - " + aClass);
										if (lDspName.IndexOf(aClass) > -1) {
												lResult.Add(iAsm);
											}
									}
								return lResult;
							}
					}
			}
	}