/*
 *
 * Licensing:			GPL
 * Original project:	cs.PLiCGen.csproj
 *
 * Copyright: Adam Halassy (2010.08.05.)
 * 
 * 
 * Required sources:
 *  Common\common-tools.cs
 * 	Common\Versioning.cs
 * 	Security\AuthStore.cs
 * 
 */
 
using Halassy;

using System;
using System.Xml;

namespace Halassy.Security {
		public static class PublicLicenseCode {
				private const string __VERCH = "ABCDEFGHIJKLMNOPQRSTVWXYZ";
		
				public static string GeneratePID(string aProjectId, string aAppId, string aAppVer) {
						aProjectId = aProjectId.ToUpper();
						aAppId = aAppId.ToUpper();
						aAppVer = aAppVer.ToUpper();						
						
						CApplicationVersion lAppVer = CApplicationVersion.StringToApplicationVersion(aAppVer);
						
						string
								lPID1 = aProjectId.Substring(0, 5),
								lPID2 = aProjectId.Substring(5),
								lPID3 = aAppId + __VERCH[lAppVer.Major] + __VERCH[lAppVer.Minor],
								lResult = "";
						
						lResult = PasswordTools.EncryptPassword_1(lPID2, lPID1, 5, true);
						lResult += PasswordTools.EncryptPassword_1(lPID3, lPID2, 5, true);
						lResult += PasswordTools.EncryptPassword_1(lPID1, lPID3, 5, true);
						
						return lResult;
					}
				
				public static string GetProductKey(string aProjectId, string aAppId, string aAppVer) {
						aProjectId = aProjectId.ToUpper();
						aAppId = aAppId.ToUpper();
						aAppVer = aAppVer.ToUpper();						
						
						CApplicationVersion lAppVer = CApplicationVersion.StringToApplicationVersion(aAppVer);
						
						return aProjectId + aAppId + __VERCH[lAppVer.Major] + __VERCH[lAppVer.Minor];
					}
				
				public static string GenerateCID(string aCustomer, DateTime aRegTime) {
						aCustomer = aCustomer.ToUpper();
				
						string DateStr = aRegTime.Year.ToString() + aRegTime.Month.ToString() + aRegTime.Day.ToString();
						
						if (aCustomer.Length > 10)
								aCustomer = aCustomer.Substring(0, 10);
							else
								for (int i = 0; aCustomer.Length < 10; i++)
										aCustomer += aCustomer[i];
						
						return PasswordTools.EncryptPassword_1(DateStr, aCustomer, 10, true);
						
					}
			
				public static string Generate(string aProjectId, string aAppId, string aAppVer, string aCustomer, DateTime aRegTime) {
						string aPID = GeneratePID(aProjectId, aAppId, aAppVer);
						string aCID = GenerateCID(aCustomer, aRegTime);
						string[] aLicList = new string[5];
						string aLicKey = "";
						
						for (int i = 0; i < 5; i++) {
								aLicList[i] = aPID.Substring(i*3, 3);
								aLicList[i] += aCID.Substring(i*2, 2);
							}
							
						for (int i = 0; i < 5; i++)
								aLicKey += PasswordTools.EncryptPassword_1(aLicList[((4-i)+1)%5], aLicList[(4-i)], 5, true);
						
						return aLicKey;
					}
			}
			
		public enum TLicServerType {
				Central,
				Proxy
			}
			
		public class CLicenseFile {
				protected const string KEY_ROOT = "pliccode";
				protected const string KEY_SERVER = "server";
				protected const string KEY_KEY = "key";
				protected const string KEY_INFO = "info";
				
				protected const string ATTR_TYPE = "type";
				protected const string ATTR_ADDR = "addr";
				protected const string ATTR_VALUE = "value";
				protected const string ATTR_PRODUCT = "product";
				
				protected const string VAL_PROXY = "proxy";
				protected const string VAL_CENTRAL = "central";
		
				private XmlDocument _doc;
				
				protected XmlNode rootNode {
						get {
								if (!XmlTools.IsNode(_doc, KEY_ROOT))
										return XmlTools.CreateXmlNode(_doc, KEY_ROOT, null);
									else
										return XmlTools.getXmlNodeByName(KEY_ROOT, _doc);
							}
					}
				
				protected XmlNode serverNode {
						get {
								if (XmlTools.IsNode(rootNode, KEY_SERVER))
										return XmlTools.getXmlNodeByName(KEY_SERVER, rootNode);
									else
										return XmlTools.CreateXmlNode(_doc, KEY_SERVER, rootNode);
							}
					}
					
				protected XmlNode infoNode {
						get {
								if (XmlTools.IsNode(rootNode, KEY_INFO))
										return XmlTools.getXmlNodeByName(KEY_INFO, rootNode);
									else
										return XmlTools.CreateXmlNode(_doc, KEY_INFO, rootNode);
							}
					}
					
				protected XmlNode keyNode {
						get {
								if (XmlTools.IsNode(rootNode, KEY_KEY))
										return XmlTools.getXmlNodeByName(KEY_KEY, rootNode);
									else
										return XmlTools.CreateXmlNode(_doc, KEY_KEY, rootNode);
							}
					}
				
				public TLicServerType ServerType {
						get {
								if (XmlTools.GetAttrib(serverNode, ATTR_TYPE).ToLower() == VAL_PROXY)
										return TLicServerType.Proxy;
									else
										return TLicServerType.Central;
							}
							
						set {
								switch (value) {
										case TLicServerType.Proxy: XmlTools.SetAttrib(serverNode, ATTR_TYPE, VAL_PROXY); break;
										case TLicServerType.Central: XmlTools.SetAttrib(serverNode, ATTR_TYPE, VAL_CENTRAL); break;
									}
								
							}
					}
				
				public string ProxyAddr {
						get { return XmlTools.GetAttrib(serverNode, ATTR_ADDR);	}
						set { XmlTools.SetAttrib(serverNode, ATTR_ADDR, value); }
					}
					
				public string ProductID {
						get { return XmlTools.GetAttrib(infoNode, ATTR_PRODUCT);	}
						set { XmlTools.SetAttrib(infoNode, ATTR_PRODUCT, value); }
					}
				
				public string LicenseKey {
						get { return XmlTools.GetAttrib(keyNode, ATTR_VALUE);	}
						set { XmlTools.SetAttrib(keyNode, ATTR_VALUE, value); }
					}
					
				public static CLicenseFile CreateNew(
							string aProjectId,
							string aAppId,
							string aAppVer,
							string aCustomer,
							DateTime aRegTime
						) {
						CLicenseFile lLic = new CLicenseFile();
						lLic.LicenseKey = PublicLicenseCode.Generate(
									aProjectId,
									aAppId,
									aAppVer,
									aCustomer,
									aRegTime
								);
						lLic.ProductID = PublicLicenseCode.GetProductKey(aProjectId, aAppId, aAppVer);
						return lLic;
					}
					
				public static CLicenseFile CreateNew(string aProductId, string aLicKey) {
						CLicenseFile lLic = new CLicenseFile();
						lLic.LicenseKey = aLicKey;
						lLic.ProductID = aProductId;
						
						return lLic;
					}
				
				public void LoadFromFile(string aFileName) { _doc.Load(aFileName); }
				public void SaveToFile(string aFileName) { _doc.Save(aFileName); }
				
				public CLicenseFile() { _doc = new XmlDocument(); }
			}
	}