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
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ReportSmart.Engine.Collections {
		public class CReportRootFolder: CReportFolder {
				public new RsReportCollection Collection {
						get { return GetCollection(); }
						set { SetCollection(value); }
					}
		
				public CReportRootFolder(string aName, RsReportCollection aCollection): base(aName) {
						GUINode.ImageIndex = 3;
						GUINode.SelectedImageIndex = 3;
				
						SetCollection(aCollection);
					}
			}
	}
