/*
 *
 * Licensing:			GPL
 * Original project:	RsCore.csproj
 *
 * Copyright: Adam Halassy (2010.12.08.)
 * 
 * 
 */
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
 using System.Xml.Schema;

namespace ReportSmart.Engine.Config {
		
		[XmlTypeAttribute(TypeName = "userinterface")]
		public class RsUserInterfaceConfig {
		
				[XmlElementAttribute(ElementName = "window")]
				public RsWindowConfig WindowConfig;
		
				public RsUserInterfaceConfig() {
						WindowConfig = new RsWindowConfig();
					}
			}
	}
