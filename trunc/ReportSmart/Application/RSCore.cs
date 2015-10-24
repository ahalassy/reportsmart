/*
 *
 * Licensing:			GPL
 * Original project:	edit.csproj
 *
 * Copyright: Adam Halassy (2010.06.10.)
 * 
 * 
 */
using Halassy;

using System;
using System.Reflection;
using System.Resources;
using System.Xml;

using ReportSmart;

namespace ReportSmart {
		internal static class RSResources {
				private static ResourceManager
							_resGFX = null,
							_resSpec = null,
							_resRaw = null;
		
				public static ResourceManager GFX {
						get {
								if (_resGFX == null)
										_resGFX = new ResourceManager("ReportSmart.Application.Resources.GFX", Assembly.GetExecutingAssembly());
									
								return _resGFX;
							}
					}
				
				public static ResourceManager Special {
						get {
								if (_resSpec == null)
										_resSpec = new ResourceManager("ReportSmart.Application.Resources.Special", Assembly.GetExecutingAssembly());
									
								return _resSpec;
							}
					}
					
				public static ResourceManager RawData {
						get {
								if (_resRaw == null)
										_resRaw = new ResourceManager(
													"ReportSmart.Application.Resources.RawData",
													Assembly.GetExecutingAssembly()
												);
								return _resRaw;
							}
					}
			}
			
		internal class CRSPaperManager {
				private XmlDocument _paperSet;
		
				public CRSPaperManager() {
						_paperSet = new XmlDocument();
						
						//RsViewEngine.
					}
			}
	}