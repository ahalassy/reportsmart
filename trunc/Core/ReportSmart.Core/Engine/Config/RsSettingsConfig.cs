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
		/// <summary>
		/// Description of RsSettings.
		/// </summary>
		
		[XmlTypeAttribute(TypeName="settings")]
		public class RsSettingsConfig {
		
				[XmlElementAttribute(ElementName = "locale")]
				public RsLocaleConfig Locale;
		
				[XmlElementAttribute(ElementName = "userinterface")]
				public RsUserInterfaceConfig UserInterface { get; set; }
				
				[XmlElementAttribute(ElementName = "security")]
				public RsSecurityConfig Security;
				
				[XmlElementAttribute(ElementName = "offline")]
				public RsOfflineConfig Offline;
				
				[XmlElementAttribute(ElementName = "embeddedparamedit")]
				public RsEmbeddedParamEditConfig EmbeddedParamEdit;
		
				public RsSettingsConfig() {
						Locale = null;
						UserInterface = new RsUserInterfaceConfig();
						Security = null;
						Offline = new RsOfflineConfig();
						EmbeddedParamEdit = new RsEmbeddedParamEditConfig();
					}
			}
	}
