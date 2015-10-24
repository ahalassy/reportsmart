
/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam ReportSmart (2010.09.18.)
 * 
 * REQUIRED SOURCES:
 *  Special/ActiveX/AcroPDF
 */
 
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace ReportSmart.Special.DocumentView {
		public abstract class CDocumentEngine {

				private Control _DocControl;
				
				protected Control docControl {
						get { return _DocControl; }
						set { _DocControl = value; }
					}
				
				public Control DocumentControl { get { return _DocControl; }}
						
				public abstract void LoadDocument(string aDocument);
				public abstract bool IsOpenable();
			}
			
		public class CPDFDocEngine: CDocumentEngine {		
				public AxHost PDFControl { get { return (DocumentControl as AxHost); }}
				
				public override void LoadDocument(string aDocument) {
						//System.	
						
						
					}

				public override bool IsOpenable() {
						return true;
					}
			}

		public class CDocViewer {
			}
	}