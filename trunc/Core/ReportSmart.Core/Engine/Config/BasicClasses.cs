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
 using System.IO;
 using System.Text;
 using System.Xml;
 using System.Xml.Serialization;
 using System.Xml.Schema;
 using System.Drawing;
 using System.Windows.Forms;

namespace ReportSmart.Engine.Config {

		public class RsPathConfig {
				[XmlAttribute(AttributeName = "path", DataType = "string")]
				public string Path;
			}
			
		public class RsEnabledConfig {
				[XmlIgnoreAttribute]
				public bool Enabled {
						get { return EnabledStr == "yes"; }
						set { EnabledStr = value ? "yes" : "no"; }
					}
		
				[XmlAttribute(AttributeName = "enabled", DataType = "string")]
				public string EnabledStr;
		
			}
			
		[XmlTypeAttribute(TypeName = "security")]
		public class RsSecurityConfig: RsPathConfig {}
			
		[XmlTypeAttribute(TypeName = "embeddedparamedit")]
		public class RsEmbeddedParamEditConfig: RsEnabledConfig {}
		
		[XmlTypeAttribute(TypeName = "offline")]
		public class RsOfflineConfig: RsEnabledConfig {
				[XmlAttribute(AttributeName = "timeout", DataType = "int")]
				public int Timeout;
			}

		[XmlTypeAttribute(TypeName="locale")]
		public class RsLocaleConfig {
				[XmlAttribute(AttributeName = "lang", DataType = "string")]
				public string Language;
			}
			
		[XmlTypeAttribute(TypeName = "window")]
		public class RsWindowConfig {
				[XmlAttribute(AttributeName = "left", DataType = "int")]
				public int Left = 10;
				
				[XmlAttribute(AttributeName = "top", DataType = "int")]
				public int Top = 10;
				
				[XmlAttribute(AttributeName = "width", DataType = "int")]
				public int Width = 800;
				
				[XmlAttribute(AttributeName = "height", DataType = "int")]
				public int Height = 600;
				
				[XmlAttribute(AttributeName = "maximized", DataType = "string")]
				public string MaximizedStr = "no";
				
				[XmlIgnoreAttribute]
				public bool Maximized {
						get { return MaximizedStr == "yes"; }
						set { MaximizedStr = value ? "yes" : "no"; }
					}
					
				public Size GetwindowSize() {
						return new Size(Width, Height);
					}
					
				public Point GetwindowLocation() {
						Rectangle lworkarea = Screen.PrimaryScreen.WorkingArea;
				
						Left = Left + Width < lworkarea.Right ? Left : lworkarea.Right - Width;
						Left = Left < 0 ? 0 : Left;
						
						Top = Top + Height > lworkarea.Bottom ? lworkarea.Bottom - Height : Top;
						Top = Top < 0 ? 0 : Top;
				
				
						return new Point(Left, Top);
					}
			}
	 }