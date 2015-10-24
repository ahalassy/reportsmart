#region Source information

//*****************************************************************************
//
//    RsProfileConfig.cs
//    Created by Adam (2015-10-23, 9:21)
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
