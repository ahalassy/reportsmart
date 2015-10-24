/*
 * 2009-09-14 (Adam Halassy)
 * 
 */
 
using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Windows.Forms;

using Halassy;

namespace Halassy.Localization {
		public class ELocalization: Exception { public ELocalization(string aMessage): base(aMessage) {	}}
		
		public class ELocalizationTag: Exception { public ELocalizationTag(string aMessage): base(aMessage) {	}}

		public delegate void LocalizationChangeEventHandler(CLocalization aLocalizatoin);

		public class CLocaleInfo {
				public string LocaleName;
				public string Author;
				public string Version;
				public string FileName;
				public string ChooseInstruction;
				public string LocaleID;
				
				public override string ToString() {
						return LocaleName;
					}
			}

		public interface ILocalizedControl {
				void AssignLocale(CLocalization aSender);
				void ApplyLocale(CLocalization aSender);
				void ReleaseLocale();
				string GetInstanceName();
			}

		public abstract class CLocalizedPanel: Panel, ILocalizedControl {
				private CLocalization _Locale;
				
				public CLocalization Locale { get { return _Locale; }}
				
				public virtual void ReleaseLocale() {
						_Locale.RemoveLocalizedControl(this);
					}
					
				public virtual void AssignLocale(CLocalization aLocale) {
						_Locale = aLocale;
					}
				
				public abstract void ApplyLocale(CLocalization aLocale);
				public abstract string GetInstanceName();
			}
			
		public abstract class CLocalizedForm: Form, ILocalizedControl {
				private CLocalization _Locale;
				
				public CLocalization Locale { get { return _Locale; }}
				
				public void ReleaseLocale() {
						_Locale.RemoveLocalizedControl(this);
					}
					
				public void AssignLocale(CLocalization aLocale) {
						_Locale = aLocale;
					}
				
				public abstract void ApplyLocale(CLocalization aLocale);
				public abstract string GetInstanceName();
			}
		
		public class CLocalization {
				public static string XML_ROOTNODE = "ReportSmart-Locale";
				public static string XML_INFORMATIONS = "informations";
				public static string XML_CONTENT = "content";
				public static string XML_DIALOGS = "dialogs";
				public static string XML_FORMS = "forms";
				public static string XML_PANELS = "panels";
				public static string XML_TAGS = "tags";
				public static string XML_WIZARDS = "wizards";
				public static string XML_MESSAGES = "messageList";
				public static string XML_MESSAGE = "msgbox";
				public static string XML_MENUS = "menus";
				public static string XMLa_NAME = "name";
				public static string XMLa_TITLE = "title";
				
				public static bool IsLocale(string aFile) {
						try {
								GetLocaleInfo(aFile);
								return true;
							} catch {
								return false;
							}
					}
				
				public static CLocaleInfo GetLocaleInfo(string aFile) {
						CLocaleInfo lResult = new CLocaleInfo();
						lResult.FileName = aFile;
						XmlDocument lDoc = new XmlDocument();
						lDoc.Load(aFile);
						XmlNode lRoot = XmlTools.getXmlNodeByName("ReportSmart-Locale", lDoc);
						XmlNode lInfo = XmlTools.getXmlNodeByName("informations", lRoot);
						
						lResult.Author = XmlTools.getXmlNodeByName("author", lInfo).InnerXml;
						lResult.LocaleName = XmlTools.getXmlNodeByName("langName", lInfo).InnerXml;
						lResult.Version = XmlTools.getXmlNodeByName("version", lInfo).InnerXml;
						lResult.ChooseInstruction = XmlTools.getXmlNodeByName("chooseInstruction", lInfo).InnerXml;
						lResult.LocaleID = Path.GetFileNameWithoutExtension(lResult.FileName);
						
						return lResult;
					}
				
				private XmlDocument _Data;
				private XmlNode _XMLRoot;
				private XmlNode _Informations;
				private XmlNode _Dialogs;
				private XmlNode _Forms, _Panels;
				private XmlNode _Tags;
				private XmlNode _Wizards;
				private XmlNode _Root;
				private XmlNode _Messages;
				
				public bool IsApplied { get; protected set; }
				
				public XmlNode MenuLocaizations { get; protected set; }
				
				public event LocalizationChangeEventHandler LocalizationChanged;
				
				private ArrayList _LocalizedControls = null;
				
				public void AddLocalizedControl(ILocalizedControl aControl) {
						if (_LocalizedControls.IndexOf(aControl) < 0) {
								_LocalizedControls.Add(aControl);
								aControl.AssignLocale(this);
								if (IsApplied)
										aControl.ApplyLocale(this);
							}
					}
					
				public void RemoveLocalizedControl(ILocalizedControl aControl) {
						if (_LocalizedControls.IndexOf(aControl) > -1)
								_LocalizedControls.Remove(aControl);
					}
				
				public XmlNode Tags { get { return _Tags; }}
		
				public void LoadLocalization(string aFile) {
						IsApplied = false;
						
						FileStream lStream = new FileStream(aFile, FileMode.Open, FileAccess.Read, FileShare.None); 
						TextReader lReader = new StreamReader(lStream);
						
						_Data = new XmlDocument();
						_Data.Load(lReader);
						lStream.Close();
						
						_XMLRoot = XmlTools.getXmlNodeByName(XML_ROOTNODE, _Data);
						_Informations = XmlTools.getXmlNodeByName(XML_INFORMATIONS, _XMLRoot);
						_Root = XmlTools.getXmlNodeByName(XML_CONTENT, _XMLRoot);
						_Dialogs = XmlTools.getXmlNodeByName(XML_DIALOGS, _Root);
						_Forms = XmlTools.getXmlNodeByName(XML_FORMS, _Root);
						_Panels = XmlTools.getXmlNodeByName(XML_PANELS, _Root);
						_Tags = XmlTools.getXmlNodeByName(XML_TAGS, _Root);
						_Wizards = XmlTools.getXmlNodeByName(XML_WIZARDS, _Root);
						_Messages = XmlTools.getXmlNodeByName(XML_MESSAGES, _Root);
						
						MenuLocaizations = XmlTools.GetNode(_Root, XML_MENUS);
						
						if ( _Tags == null ) throw new Exception("Tags not found in file: " + aFile);
					}
			
				public void ApplyLocalization() {
						foreach (object aControl in _LocalizedControls) {
								try {
										((ILocalizedControl)aControl).ApplyLocale(this);

									} catch(ELocalizationTag aException) {
										MessageBox.Show(
													"The following class could not use the selected localzation.\n" + 
													((ILocalizedControl)aControl).GetInstanceName() + "\n" +
													aException.Message + "\n" +
													"Please reinstall the application or contact with the vendor!",
													"Localization error"
												);

									} catch(Exception aException) {
										MessageBox.Show(
													"The following class could not use the selected localzation.\n" + 
													((ILocalizedControl)aControl).GetInstanceName() + "\n" +
													aException.Message + "\n" +
													aException.InnerException + Environment.NewLine +
													aException.StackTrace,
													"Localization error"
												);
									}
							} 
							
						IsApplied = true;
							
						if (LocalizationChanged != null)
								LocalizationChanged(this);
					}
			
				public string GetTagText(string aTagName) {
						if (_Tags == null)
								throw new ELocalizationTag("Tags node not found!");
				
						 XmlNode lTag = XmlTools.getXmlNodeByAttrVal(XMLa_NAME, aTagName, _Tags);
						
						if (lTag == null)
								throw new ELocalizationTag("Tag not found: \"" + aTagName + "\"");							
						
						return lTag == null ? "-" : lTag.InnerText;
					}
					
				public XmlNode GetDialogData(string aDialogName) {
						return XmlTools.getXmlNodeByName(aDialogName, _Dialogs);
					}
			
				public XmlNode GetFormData(string aFormName) {
						return XmlTools.getXmlNodeByName(aFormName, _Forms);
					}
			
				public XmlNode GetPanelData(string aPanelName) {
						return XmlTools.getXmlNodeByName(aPanelName, _Panels);
					}
					
				public XmlNode GetWizardData(string aWizardName) {
						return XmlTools.getXmlNodeByName(aWizardName, _Wizards);
					}
					
				public string GetMessage(string aMsgNam) {
						XmlNode lMsg = XmlTools.getXmlNodeByAttrVal(XML_MESSAGE, XMLa_NAME, aMsgNam, _Messages);
						return lMsg == null ? "" : lMsg.InnerText;
					}

				public string GetMessageTitle(string aMsgNam) {
						XmlNode lMsg = XmlTools.getXmlNodeByAttrVal(XML_MESSAGE, XMLa_NAME, aMsgNam, _Messages);
						return lMsg == null ? "" : XmlTools.GetAttrib(lMsg, XMLa_TITLE);
					}
			
				public XmlNode GetMenuTexts(string aMenuName) {
						return XmlTools.GetNode(MenuLocaizations, aMenuName);
					}
			
				public CLocalization() {
						_LocalizedControls = new ArrayList();
						IsApplied = false;
					}
			}
					

	}

