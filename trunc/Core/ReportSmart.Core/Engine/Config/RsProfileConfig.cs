/*
 *
 * Licensing:			GPL
 * Original project:	RsCore.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.08.)
 * 
 * 
 */
 
 using System;
 using System.Collections;
 using System.Collections.Generic;
 using System.IO;
 using System.Text;
 using System.Xml;
 using System.Xml.Serialization;
 using System.Xml.Schema;

namespace ReportSmart.Engine.Config {

		[XmlRootAttribute(ElementName="ReportSmartSettings", IsNullable=false)]
		public class RsProfileConfig {
		
				[XmlAttribute(AttributeName = "version", DataType = "string")]
				public string Version { get; set; }
				
				[XmlAttribute(AttributeName = "configVersion", DataType = "int")]
				public int ConfigVersion { get; set; }
				
				[XmlElementAttribute(ElementName = "settings")]
				public RsSettingsConfig Settings { get; set; }
				
				public List<RsCollectionConfig> Collections;
		
				public RsProfileConfig() {
						Collections = new List<RsCollectionConfig>();
						ConfigVersion = 0;
					}
			}
	}
