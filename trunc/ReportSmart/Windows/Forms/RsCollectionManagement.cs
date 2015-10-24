#region Source information

//*****************************************************************************
//
//    RsCollectionManagement.cs
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

using System;
using System.Windows.Forms;

using ReportSmart;
using ReportSmart.Localization;

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
