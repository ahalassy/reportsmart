/*
 *
 * Licensing:			GPL
 * Original project:	RsReportProvider.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.09.)
 * 
 * 
 */
using System;

namespace ReportSmart.Documents.Collections {
	
	public enum RsCollectionProviderType {
			Custom,
			Favorites,
			FileSystem,
			History,
			InvalidCollection
		}
	
	/// <summary>
	/// Abstract class wich represents a report collection
	/// </summary>
	public abstract class RsCollectionProvider {
	
			public abstract RsCollectionProviderType GetCollectionType();
	
			public abstract RsCollectionFolder RootFolder { get; protected set; }
			
			public abstract string CollectionName { get; }
	
			public RsCollectionProvider() {
				}
		}
}
