/*
 *
 * Licensing:			GPL
 * Original project:	RsCore.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.09.)
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
		public enum RsCollectionType {
				Custom,
				Favorites,
				History
			}

		[XmlType(TypeName = "collection")]
		public class RsCollectionConfig: RsPathConfig {
		
				public bool IsCollectionExists() {
						return File.Exists(Path);
					}
		
				[XmlAttribute(AttributeName = "type")]
				public RsCollectionType Type { get; set; }
				
				[XmlAttribute(AttributeName = "name")]
				public string Name { get; set; }

			}
	}
