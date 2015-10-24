/*
 *
 * Licensing:			GPL
 * Original project:	cs.autheditor.csproj
 *
 * Copyright: Adam ReportSmart (2010.05.04.)
 * 
 * 
 */
 
using ReportSmart;

using System;
using System.Xml;

namespace ReportSmart.Security {
		public static class PasswordTools {
				private static string _ENCRYPT_DIGITS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ012356789";
				private static string _ENCRYPT_DIGITS_CAPITAL = "ABCDEFGHIJKLMNOPQRSTUVWXYZ012356789";
				
				private static int _ENCRYPT_LENGTH = 64;
				private static char _NONUSED_CHR = '*';
				
				private static string chrArrayToStr(char[] aArray) {
						string lResult = "";
						
						for (int i = 0; i < aArray.Length; i++)
								lResult += aArray[i];
								
						return lResult;						
					}
				
				private static string initEncDigits(string aUsr, bool aCapital) {
						// 1: Initialize digits basing on password:
						string lEncdigits = "", lOriginDigits = aCapital ? _ENCRYPT_DIGITS_CAPITAL : _ENCRYPT_DIGITS;
						int lShift = aUsr.Length, lPos = lShift;
						
						while (lOriginDigits.Length > 0) {
								lPos %= lOriginDigits.Length;
								lEncdigits += lOriginDigits.Substring(lPos, 1);
								lOriginDigits = lOriginDigits.Remove(lPos, 1);
								lPos += lShift;
							}
						
						return lEncdigits;
					}
				
				public static string DecryptPassword_1(string aUsr, string aPwdStr) {
						return DecryptPassword_1(aUsr, aPwdStr, _ENCRYPT_LENGTH);
					}
					
				public static string DecryptPassword_1(string aUsr, string aPwdStr, int aLen) {
						return DecryptPassword_1(aUsr, aPwdStr, aLen, false);
					}
					
				public static string DecryptPassword_1(string aUsr, string aPwdStr, int aLen, bool aCapital) {
						string lEncDigits = initEncDigits(aUsr, aCapital);
						
						string lPwd = "";
						for (int i = 0; i < aPwdStr.Length; i++) {
								int lIndex = lEncDigits.IndexOf(aPwdStr[i]) - i;
								if (lIndex < 0) 
										lPwd += lEncDigits[lIndex + lEncDigits.Length];
									else
										lPwd += lEncDigits[lIndex];
							}
							
						return lPwd;
					}
		
				public static string EncryptPassword_1(string aUsr, string aPwdStr) {
						return EncryptPassword_1(aUsr, aPwdStr, _ENCRYPT_LENGTH);
					}
					
				public static string EncryptPassword_1(string aUsr, string aPwd, int aLen) {
						return EncryptPassword_1(aUsr, aPwd, aLen, false);
					}
		
				public static string EncryptPassword_1(string aUsr, string aPwd, int aLen, bool aCapital) {
						string lEncDigits = initEncDigits(aUsr, aCapital);
						int lPos = aUsr.Length;
						Random lRandom = new Random();
						
						// 2: Transform password:
						string lNewStr = "";
						for (int i = 0; i < aPwd.Length; i++) {
								lNewStr += lEncDigits[(lEncDigits.IndexOf(aPwd[i]) + i) % lEncDigits.Length];
							}
							
						char[] lResult = new char[aLen];
						for (int i = 0; i < aLen; i++)
								lResult[i] = _NONUSED_CHR;
						
						for (int i = 0; i < lNewStr.Length; i++) {
								// Find the first suitable place:
								for (int g = 0; lPos < lResult.Length && lResult[lPos] != _NONUSED_CHR; lPos++, g++) {
										if (lPos >= lNewStr.Length) lPos -= lNewStr.Length;
										
										if (g > aLen)
												System.Windows.Forms.MessageBox.Show("Jaj, itt valami nem oké!\n" + chrArrayToStr(lResult));
									}
								
								// Then replace if ok:
								if (lPos < lResult.Length)
										lResult[lPos] = lNewStr[i];
								lPos += lNewStr.Length;
							}
							
						
						for (int i = 0; i < aLen; i++)
								if (lResult[i] == _NONUSED_CHR) lResult[i] = _ENCRYPT_DIGITS[lRandom.Next(_ENCRYPT_DIGITS.Length)];
						string lRes = chrArrayToStr(lResult);
						
						
						//while (lNewStr.Length < aLen) {
						//		lNewStr += _ENCRYPT_DIGITS[lRandom.Next(_ENCRYPT_DIGITS.Length)];
						//	}	
						//System.Windows.Forms.MessageBox.Show(aPwd + " => " + lRes);
						
						return lNewStr;
					}
			}
		
		public class CSecurityData: XmlDocument {
				public const string XML_NODE_SECURITY = "security";
				public const string XML_NODE_KEY = "key";
				public const string XML_ATTR_SERVICE = "srv";
				public const string XML_ATTR_USER = "usr";
				public const string XML_ATTR_PASSWD = "passwd";
				public const string XML_ATTR_TYPE = "type";
				public const string XML_ATTR_METATAG = "meta";
				
				public static CSecurityNode AddSecNode(CSecurityData aSecDoc, string aService, string aType) {
						CSecurityNode lResult = new CSecurityNode(XmlTools.CreateXmlNode(aSecDoc, XML_NODE_KEY, aSecDoc.SecRootNode));
						
						lResult.Service = aService;
						lResult.Type = aType;
						
						return lResult;
					}

				public static CSecurityNode AddSecNode(CSecurityData aSecDoc, string aService, string aType, string aUsr, string aPwd) {
						CSecurityNode lResult = new CSecurityNode(XmlTools.CreateXmlNode(aSecDoc, XML_NODE_KEY, aSecDoc.SecRootNode));
						
						lResult.Service = aService;
						lResult.Type = aType;
						lResult.UserName = aUsr;
						lResult.Password = aPwd;
						
						return lResult;	
					}
					
				public static CSecurityNode AddSecNode(CSecurityData aSecDoc, string aService, string aType, string aUsr, string aPwd, string aMetaTag) {
						CSecurityNode lResult = new CSecurityNode(XmlTools.CreateXmlNode(aSecDoc, XML_NODE_KEY, aSecDoc.SecRootNode));
						
						lResult.Service = aService;
						lResult.Type = aType;
						lResult.UserName = aUsr;
						lResult.Password = aPwd;
						lResult.MetaTag = aMetaTag;
						
						return lResult;	
					}

				public static CSecurityData CreateNew() {
						CSecurityData lResult = new CSecurityData();
						lResult.AppendChild(lResult.CreateXmlDeclaration("1.0", "utf-8", ""));
						XmlTools.CreateXmlNode(lResult, XML_NODE_SECURITY, lResult);
						return lResult;
					}
					
				public XmlNode SecRootNode {
						get { return XmlTools.getXmlNodeByName(XML_NODE_SECURITY, this); }
					}
		
				public CSecurityNode GetSecurityNode(string aType, string aService) {
						foreach (XmlNode iNode in SecRootNode) {
								CSecurityNode iSecNode = new CSecurityNode(iNode);
						
								if (iSecNode.Type == aType && iSecNode.Service.ToUpper() == aService.ToUpper())
										return iSecNode;
							}
						
						return null;
					}
				
				public CSecurityNode GetSecurityNode(string aType, string aService, string aMetaTag) {
						aMetaTag = aMetaTag.ToUpper();
						aService = aService.ToUpper();
				
						foreach (XmlNode iNode in SecRootNode) {
								CSecurityNode iSecNode = new CSecurityNode(iNode);
						
								if (iSecNode.Type == aType && iSecNode.Service.ToUpper() == aService && aMetaTag == iSecNode.MetaTag.ToUpper())
										return iSecNode;
							}
						
						return null;						
					}
			}
			
		public class CSecurityNode {
				private XmlNode _node;
				
				public string Type {
						get { return XmlTools.GetAttrib(_node, CSecurityData.XML_ATTR_TYPE); }
						set { XmlTools.SetAttrib(_node, CSecurityData.XML_ATTR_TYPE, value); }
					}
					
				public string Service {
						get { return XmlTools.GetAttrib(_node, CSecurityData.XML_ATTR_SERVICE); }
						set { XmlTools.SetAttrib(_node, CSecurityData.XML_ATTR_SERVICE, value); }
					}
					
				public string UserName {
						get { return XmlTools.GetAttrib(_node, CSecurityData.XML_ATTR_USER); }
						set { XmlTools.SetAttrib(_node, CSecurityData.XML_ATTR_USER, value); }
					}
				
				public string MetaTag {
						get { return XmlTools.GetAttrib(_node, CSecurityData.XML_ATTR_METATAG); }
						set { XmlTools.SetAttrib(_node, CSecurityData.XML_ATTR_METATAG, value); }
					}				
				
				public string Password {
						get {
								return PasswordTools.DecryptPassword_1(
											UserName,
											XmlTools.GetAttrib(_node, CSecurityData.XML_ATTR_PASSWD)
										);
							}
						set {
								XmlTools.SetAttrib(
											_node,
											CSecurityData.XML_ATTR_PASSWD,
											PasswordTools.EncryptPassword_1(UserName, value)
										);
							}
					}

				public void Delete() {
						_node.ParentNode.RemoveChild(_node);
					}

				public CSecurityNode(XmlNode aNode) {
						_node = aNode;
					}
					
			}
	}