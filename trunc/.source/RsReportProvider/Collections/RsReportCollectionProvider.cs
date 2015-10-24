/*
 *
 * Licensing:			GPL
 * Original project:	RsReportProvider.csproj
 *
 * Copyright: Adam Halassy (2010.12.10.)
 * 
 * 
 */
using System;

using Halassy;
using Halassy.Controls;

using ReportSmart;
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;

namespace ReportSmart.Documents.Collections {
		/// <summary>
		/// Description of RsReportCollectionProvider.
		/// </summary>
		public class RsReportCollectionProvider: RsCollectionProvider {
		
				public RsReportCollection ReportCollection { get; protected set; }
				
				public override RsCollectionProviderType GetCollectionType() {
						if (ReportCollection == null)
								return RsCollectionProviderType.InvalidCollection;
							else
								return ReportCollection.CollectionName == "favorites" ?
										RsCollectionProviderType.Favorites :
										RsCollectionProviderType.Custom;
					}
				
				public override RsCollectionFolder RootFolder {
						get { return new RsCollectionFolder(ReportCollection.RootFolder); }
						protected set { throw new NotImplementedException(); }
					}
					
				public override string CollectionName {
						get { return ReportCollection.CollectionName; }
					}
		
				public RsReportCollectionProvider(string aCollectionFile): base() {
						try {
								ReportCollection = new RsReportCollection();
								ReportCollection.LoadFromXML(aCollectionFile);
								
							} catch {
								ReportCollection = null;
							
							}
					}
			}
	}
