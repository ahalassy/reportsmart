#region Source information

//*****************************************************************************
//
//    rssecconv.cs
//    Created by Adam (2015-10-23, 8:57)
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

using System.Xml;
using ReportSmart.Security;

namespace ReportSmart
{
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