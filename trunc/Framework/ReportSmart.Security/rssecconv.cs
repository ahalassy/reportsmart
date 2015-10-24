/*
 *
 * Licensing:			GPL
 * Original project:	cs.autheditor.csproj
 *
 * Copyright: Adam Halassy (2010.09.28.)
 * 
 * 
 */

using Halassy;
using Halassy.Security;
using System.Xml;

namespace ReportSmart {
		public static class RsOldSecurity {
				public static bool IsOldFormat(string aFile) {
						XmlDocument lDoc = new XmlDocument();
						lDoc.Load(aFile);
						return XmlTools.IsNode(lDoc, "ReportSmartSecurity");
					}
					
				public static void ConvertToNew(string aFile) {
						CSecurityData lNewDoc = CSecurityData.CreateNew();
						XmlDocument lOldDoc = new XmlDocument();
						lOldDoc.Load(aFile);
						XmlNode lRootNode = XmlTools.getXmlNodeByName("ReportSmartSecurity", lOldDoc);
						foreach (XmlNode iNode in lRootNode) {
								CSecurityNode iSecNode = CSecurityData.AddSecNode(
										lNewDoc,
										XmlTools.GetAttrib(iNode, "datasource"),
										iNode.Name
									);
								iSecNode.UserName = XmlTools.GetAttrib(iNode, "userid");
								iSecNode.Password = XmlTools.GetAttrib(iNode, "passwd");
							}
						lNewDoc.Save(aFile);
					}
			}
	}