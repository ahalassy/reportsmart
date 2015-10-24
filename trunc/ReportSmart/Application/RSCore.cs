#region Source information

//*****************************************************************************
//
//    RSCore.cs
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

using ReportSmart;

using System;
using System.Reflection;
using System.Resources;
using System.Xml;

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