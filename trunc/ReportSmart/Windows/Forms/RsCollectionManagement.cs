/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam Halassy (2010.12.13.)
 * 
 * 
 */
using System;
using System.Windows.Forms;

using Halassy;
using Halassy.Localization;

using ReportSmart;
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;
using ReportSmart.Windows;
using ReportSmart.Windows.Forms;

namespace ReportSmart.Windows.Forms  {
		/// <summary>
		/// Description of RsCollectionManagement.
		/// </summary>
		public partial class RsCollectionManagement: Panel, ILocalizedControl {
				
				public CLocalization Localization { get; protected set; }
		
				public void AssignLocale(CLocalization aSender) {
						Localization = aSender;
						
						
						aSender.AddLocalizedControl(CollectionBrowser);
						aSender.AddLocalizedControl(ListContext);
					}
				
				public void ApplyLocale(CLocalization aSender) {}
				
				public void ReleaseLocale() {
						Localization = null;
					}
				
				public string GetInstanceName() {
						return "RsCollectionManagement";
					}
		
				public void DoAddCollection() {
					// TODO Implement DoAddCollection
					throw new NotImplementedException();
				}
				
				public void DoAddReport() {
					// TODO Implement DoAddCollection
					throw new NotImplementedException();
				}
			
				public void DoAddFolder() {
					// TODO Implement DoAddFolder
					throw new NotImplementedException();
				}
				
				public void DoModify() {
					// TODO Implement DoModify
					throw new NotImplementedException();
				}
			
				public void DoRemove() {
					// TODO Implement DoRemove
					throw new NotImplementedException();
				}
	
				public RsCollectionManagement() {
						InitializeControl();
					}
			}
	}
