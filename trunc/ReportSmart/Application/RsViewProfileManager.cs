/*
 *
 * Licensing:			GPL
 * Original project:	bin.csproj
 *
 * Copyright: Adam ReportSmart (2009.11.11.)
 * 
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
 
using ReportSmart;
using ReportSmart.Localization;

using ReportSmart.Engine;
using ReportSmart.Engine.Config;
using ReportSmart.Controls;
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;
 
namespace ReportSmart.Application {
        internal class RsViewProfileManager: RsProfileManager {
        
        		public const int CURRENT_PROFILE_VERSION = 1;

				public static string SETTINGSDIR = ReportSmart.CommonTools.MY_NAME + "\\" + RsApplicationInfo.ApplicationName;
				public static string SETTINGSFILENAME = "reportsmart.profile";
				public static string FAVORITESFILENAME = "favorites.rfc";				
				public static string SECURITYFILENAME = "security";

        		public string ApplicationPath { get; protected set; }
        		public string WinUserProfilePath { get; protected set; }
        		public string RsViewProfilePath { get; protected set; }

        		public string SecurityFile { get { return string.Concat(FileSystem.ToPath(this.RsViewProfilePath), SECURITYFILENAME); } }
        		public string FavoritesFile { get { return FileSystem.ToPath(this.RsViewProfilePath) + FAVORITESFILENAME; } }
        		public string LocalePath { get { return FileSystem.ToPath(this.ApplicationPath) + "locale\\"; }}
        		public string RsViewProfile { get { return FileSystem.ToPath(this.RsViewProfilePath) + "reportsmart.profile"; }}
        
        		public string GetLocalizationFile() {
						return ApplicationPath + "locale\\" + Profile.Settings.Locale.Language + ".xml";
					}
					
				public RsCollectionConfig GetFavoritesCollection() {
						foreach (RsCollectionConfig iConfig in Profile.Collections) {
								if (iConfig.Type == RsCollectionType.Favorites)
										return iConfig;
							}
							
						return null;
					}
					
				public void RegisterCollections(List<string> aPathList) {
						foreach (RsCollectionConfig iConfig in Profile.Collections) {
								if (iConfig.Type == RsCollectionType.Custom)
										Profile.Collections.Remove(iConfig);
							}
							
						foreach (string iPath in aPathList) {
								RsCollectionConfig lCfg = new RsCollectionConfig();
								lCfg.Path = iPath;
								Profile.Collections.Add(lCfg);
							}
					}
					
				public override void OpenProfile(string aProfile) {
						if (!RsViewProfileManager.IsProfileExists(aProfile)) {
								InitializeAsNewProfile(aProfile);	
						
						
							} else {				
								if ( RsViewProfileManager.CURRENT_PROFILE_VERSION > GetProfileVersion(aProfile))
										UpdateProfile(aProfile);
				
								base.OpenProfile(aProfile);
						
								Profile.ConfigVersion = RsViewProfileManager.CURRENT_PROFILE_VERSION;
							}
					}
					
				protected void InitializeCollections() {
						RsCollectionConfig lFavCfg = new RsCollectionConfig();
						lFavCfg.Type = RsCollectionType.Favorites;
						lFavCfg.Path = FavoritesFile;
						lFavCfg.Name = "favorites";
						Profile.Collections.Add(lFavCfg);

						if (!lFavCfg.IsCollectionExists()) {
								RsReportCollection lCollection = new RsReportCollection();
								lCollection.CollectionName = "favorites";
								lCollection.SaveToXML(lFavCfg.Path);
							}
					}
					
				protected void InstallProfile() {
						
					}
				
				protected void InitializeAsNewProfile(string aPath) {
						Profile = new RsProfileConfig();
						base.ProfileXmlFile = aPath;
						
						InitializeCollections();
						
						Profile.Version = RsApplicationInfo.VersionString;		
						Profile.Settings = new RsSettingsConfig();
								
						CRSInitLang lLangDlg = new CRSInitLang();			
						Profile.Settings.Locale = new RsLocaleConfig();
						lLangDlg.Execute();			
												
					}
					
				
				public List<CLocaleInfo> LookupLanguages() {
						List<CLocaleInfo> lResult = new List<CLocaleInfo>();
				
						foreach (string iFile in Directory.GetFiles(LocalePath)) {
								if (CLocalization.IsLocale(iFile))
										lResult.Add(CLocalization.GetLocaleInfo(iFile));
							}
							
						return lResult;
					}
					
				public RsViewProfileManager(): base() {
				
						FileInfo lBinInfo = new FileInfo(System.Windows.Forms.Application.ExecutablePath);
						this.ApplicationPath = FileSystemTools.toPath(lBinInfo.Directory.Parent.FullName);
						
						this.WinUserProfilePath = FileSystemTools.toPath(System.Environment.GetEnvironmentVariable("APPDATA"));
						this.RsViewProfilePath = FileSystemTools.toPath(WinUserProfilePath + SETTINGSDIR);
						FileSystemTools.forcePath(this.RsViewProfilePath);
					}
        		
        	}

		#region CReportSmartSettings
//		internal class CReportSmartSettings {		
//				public const string XML_RSSETTINGS = "ReportSmartSettings";
//				public const string XML_DATAROOT = "ReportSmartSettings";
//				public const string XML_SETTINGS = "settings";
//				public const string XML_LOCALE = "locale";
//				public const string XML_COLLECTIONS = "collections";
//				public const string XML_COLLCETION = "collection";
//				public const string XML_FAVORITES = RsViewEngine.ALIAS_FAVORITES;
//				public const string XML_SECURITY = "security";
//				public const string XML_UI = "userinterface";
//				public const string XML_WINDOW = "window";
//				public const string XML_OFFLINE = "offline";
//				public const string XML_EMBEDDEDPARAM = "embeddedparamedit";
//				
//				public const string XMLa_LEFT = "left";
//				public const string XMLa_TOP = "top";
//				public const string XMLa_WIDTH = "width";
//				public const string XMLa_HEIGHT = "height";
//				public const string XMLa_LANG = "lang";
//				public const string XMLa_PATH = "path";
//				public const string XMLa_VERSION = "version";
//				public const string XMLa_MAXIMIZED = "maximized";				
//				public const string XMLa_ENABLED = "enabled";
//				public const string XMLa_TIMEOUT = "timeout";
//				
//				public const string SECURITYFILENAME = "security";
//				public const string SETTINGSDIR = ReportSmart.CommonTools.MY_NAME + "\\" + RsApplicationInfo.ApplicationName;
//				public const string SETTINGSFILENAME = "reportsmart.profile";
//				public const string FAVORITESFILENAME = "favorites.rfc";				
//				
//				public const int RB_OK  		=  0;
//				public const int RB_ALL 		= -1;
//				public const int RB_LOCALE		= -2;
//				public const int RB_SETTINGS	= -3;
//				public const int RB_FAVORITES	= -4;
//				public const int RB_COLLECTIONS	= -5;
//				public const int RB_UI			= -6;
//				public const int RB_WINDOW		= -7;
//				public const int RB_SECURITY	= -8;
//				public const int RB_OFFLINE     = -9;
//				
//				public const int DEF_FRMTOP = 32;
//				public const int DEF_FRMLEFT = 32;
//				public const int DEF_FRMWIDTH = 700;
//				public const int DEF_FRMHEIGHT = 500;
//		
//				private XmlDocument _Doc;
//				private XmlNode _Root;
//				private XmlNode _Settings, _UINode, _Locale, _Security;
//				private CXmlSettingsNode _Offline, _EmbeddedParam;
//				private XmlNode _Collections;
//				
//				private bool _LocaleRebuilt = false;
//
//				private string _AppPath, _UsrProfilePath, _RsProfilePath, _RsSettingsFile, _RsSecFile, _RsFavFile;
//				
//				public string LocalePath { get { return FileSystem.ToPath(_AppPath) + "locale\\"; }}
//				
//				public string Locale {
//						get { return XmlTools.GetAttrib(_Locale,  XMLa_LANG); }
//						set { XmlTools.SetAttrib(_Locale, XMLa_LANG, value); }
//					}
//					
//				public bool EnabledEmbeddedParamedit {
//						get { return _EmbeddedParam.GetAsBool(XMLa_ENABLED); }
//						set { _EmbeddedParam.SetAsBool(XMLa_ENABLED, value); }
//					}
//					
//				public XmlNode Collections {
//						get { return _Collections; }
//					}
//				
//				public bool MainFormIsMaximized {
//						get {
//								CXmlSettingsNode lWnd = new CXmlSettingsNode(XmlTools.getXmlNodeByName(XML_WINDOW, _UINode));
//								return lWnd.GetAsBool(XMLa_MAXIMIZED);
//							}
//							
//						set {
//								CXmlSettingsNode lWnd = new CXmlSettingsNode(XmlTools.getXmlNodeByName(XML_WINDOW, _UINode));
//								lWnd.SetAsBool(XMLa_MAXIMIZED, value);
//							}
//					}
//				
//				public Point MainFormLocation {
//						get {
//								CXmlSettingsNode lWnd = new CXmlSettingsNode(XmlTools.getXmlNodeByName(XML_WINDOW, _UINode));
//								return new Point(lWnd.GetAsInt(XMLa_LEFT), lWnd.GetAsInt(XMLa_TOP));
//							}
//							
//						set {
//								CXmlSettingsNode lWnd = new CXmlSettingsNode(XmlTools.getXmlNodeByName(XML_WINDOW, _UINode));
//								lWnd.SetAsInt(XMLa_LEFT, value.X);
//								lWnd.SetAsInt(XMLa_TOP, value.Y);
//							}
//					}	
//					
//				public Size MainFormSize {
//						get {
//								CXmlSettingsNode lWnd = new CXmlSettingsNode(XmlTools.getXmlNodeByName(XML_WINDOW, _UINode));
//								return new Size(lWnd.GetAsInt(XMLa_WIDTH), lWnd.GetAsInt(XMLa_HEIGHT));
//							}
//							
//						set {
//								CXmlSettingsNode lWnd = new CXmlSettingsNode(XmlTools.getXmlNodeByName(XML_WINDOW, _UINode));
//								lWnd.SetAsInt(XMLa_WIDTH, value.Width);
//								lWnd.SetAsInt(XMLa_HEIGHT, value.Height);
//							}
//					}
//					
//				public TRSOffLineSettingsRec OfflineSettings {
//				        get {
//				                TRSOffLineSettingsRec lResult = new TRSOffLineSettingsRec();
//				                lResult.Enabled = _Offline.GetAsBool(XMLa_ENABLED);
//				                lResult.Timeout = _Offline.GetAsInt(XMLa_TIMEOUT);
//				                return lResult;
//				            }
//				            
//				         set {
//				                _Offline.SetAsBool(XMLa_ENABLED, value.Enabled);
//				                _Offline.SetAsInt(XMLa_TIMEOUT, value.Timeout);
//				            }
//				    }
//					
//				public string ApplicationRoot { get { return _AppPath; }}
//				public string UserProfile { get { return _UsrProfilePath; }}
//				public string RSProfile { get { return _RsProfilePath; }}
//				public string SettingsFile { get { return _RsSettingsFile; }}
//				public string SecurityFile { get { return XmlTools.GetAttrib(_Security, XMLa_PATH); }}
//				public string FavoritesFile { get { return _RsFavFile; }}
//					
//				public string GetLocalizationFile() {
//						return _AppPath + "locale\\" + Locale + ".xml";
//					}
//					
//				public XmlNode GetCollections() {
//						return XmlTools.getXmlNodeByName(XML_COLLECTIONS, _Root);
//					}
//				
//				public XmlNode GetFavoritesNode() {
//						XmlNode lCollections = XmlTools.getXmlNodeByName(XML_COLLECTIONS, _Root);
//						return XmlTools.getXmlNodeByName(XML_FAVORITES, lCollections);
//					}
//				
//				public string GetFavoritesFile() {
//						XmlNode lCollections = XmlTools.getXmlNodeByName(XML_COLLECTIONS, _Root);
//						return XmlTools.GetAttrib(XmlTools.getXmlNodeByName(XML_FAVORITES, lCollections), XMLa_PATH);
//					}
//					
//				public ArrayList LookupLanguages() {
//						ArrayList lResult = new ArrayList();
//				
//						foreach (string iFile in Directory.GetFiles(LocalePath)) {
//								if (CLocalization.IsLocale(iFile))
//										lResult.Add(CLocalization.GetLocaleInfo(iFile));
//							}
//							
//						return lResult;
//					}
//					
//				protected int CheckAttrib(XmlNode aNode, string aAttr, string aDefVal, int aCurrResult, int aErrRes) {
//						if (!XmlTools.IsAttr(aNode, aAttr)) {
//								XmlTools.SetAttrib(aNode, aAttr, aDefVal);
//								return aErrRes;
//							}
//							
//						return aCurrResult;
//					}
//					
//				protected void RebuildWindowNode() {
//						XmlNode lWindow = XmlTools.getXmlNodeByName(XML_WINDOW, _UINode);
//						if (lWindow == null)
//								lWindow = XmlTools.CreateXmlNode(_Doc, XML_WINDOW, _UINode);
//								
//						XmlTools.SetAttrib(lWindow, XMLa_LEFT, DEF_FRMLEFT.ToString());
//						XmlTools.SetAttrib(lWindow, XMLa_TOP, DEF_FRMTOP.ToString());
//						XmlTools.SetAttrib(lWindow, XMLa_WIDTH, DEF_FRMWIDTH.ToString());
//						XmlTools.SetAttrib(lWindow, XMLa_HEIGHT, DEF_FRMHEIGHT.ToString());
//						XmlTools.SetAttrib(lWindow, XMLa_MAXIMIZED, "no");
//					}
//
//				public int VerifyWindowNode() {
//						XmlNode lWindow = XmlTools.getXmlNodeByName(XML_WINDOW, _UINode);
//						if (lWindow == null) {
//								RebuildWindowNode();
//								return RB_WINDOW;
//							}
//						
//						int lResult = RB_OK;
//						
//						lResult = CheckAttrib(lWindow, XMLa_LEFT, DEF_FRMLEFT.ToString(), lResult, RB_WINDOW);
//						lResult = CheckAttrib(lWindow, XMLa_TOP, DEF_FRMTOP.ToString(), lResult, RB_WINDOW);
//						lResult = CheckAttrib(lWindow, XMLa_WIDTH, DEF_FRMWIDTH.ToString(), lResult, RB_WINDOW);
//						lResult = CheckAttrib(lWindow, XMLa_HEIGHT, DEF_FRMHEIGHT.ToString(), lResult, RB_WINDOW);
//						lResult = CheckAttrib(lWindow, XMLa_MAXIMIZED, "no", lResult, RB_WINDOW);
//						
//						return lResult;
//					}
//					
//				public void RebuildUINode() {
//						_UINode= XmlTools.getXmlNodeByName(XML_UI, _Settings);
//						if (_UINode == null)
//								_UINode = XmlTools.CreateXmlNode(_Doc, XML_UI, _Settings);
//								
//						RebuildWindowNode();
//					}
//					
//				public int VerifyUINode() {
//						_UINode = XmlTools.getXmlNodeByName(XML_UI, _Settings);
//						if (_UINode == null) {
//								RebuildUINode();
//								return RB_UI;
//							 }
//							 
//						return VerifyWindowNode();
//					}
//					
//				protected void RebuildFavorites() {
//						XmlNode lFav = XmlTools.getXmlNodeByName(XML_FAVORITES, _Collections);
//						if (lFav == null)
//								lFav = XmlTools.CreateXmlNode(_Doc, XML_FAVORITES, _Collections);
//						XmlTools.AddNewAttr(_Doc, lFav, XMLa_PATH, _RsFavFile);
//					}
//					
//				public int VerifyFavorites() {
//						XmlNode lFav = XmlTools.getXmlNodeByName(XML_FAVORITES, _Collections);
//						if (lFav == null) {
//								RebuildFavorites();
//								return RB_FAVORITES;
//							}
//								
//						if (XmlTools.IsAttr(lFav, XML_FAVORITES)) {
//								RebuildFavorites();
//								return RB_FAVORITES;
//							 }
//						
//						return RB_OK;
//					}
//					
//				protected void RebuildCollections() {
//						_Collections = XmlTools.CreateXmlNode(_Doc, XML_COLLECTIONS, _Root);
//						RebuildFavorites();
//
//					}
//				
//				public int VerifyCollactions() {
//						_Collections = XmlTools.getXmlNodeByName(XML_COLLECTIONS, _Root);
//						if (_Collections == null) {
//								RebuildCollections();
//								return RB_COLLECTIONS;
//							}
//							
//						return VerifyFavorites();
//					}
//					
//				protected void RebuildSettings() {
//						RebuildSettings(true);
//					}
//				
//				protected void RebuildSettings(bool aRecoursive) {
//						if (_Settings == null)
//								_Settings = XmlTools.CreateXmlNode(_Doc, XML_SETTINGS, _Root);
//								
//						if (aRecoursive) {
//								RebuildLocale();
//								RebuildOffline();
//							}
//						
//						XmlNode lNode = XmlTools.getXmlNodeByName(XML_EMBEDDEDPARAM, _Settings);
//						if (lNode == null)
//								lNode = XmlTools.CreateXmlNode(_Doc, XML_EMBEDDEDPARAM, _Settings);
//						
//						_EmbeddedParam = new CXmlSettingsNode(lNode);
//						if (!XmlTools.IsAttr(lNode, XMLa_ENABLED))
//								_EmbeddedParam.SetAsBool(XMLa_ENABLED, false);
//					}
//					
//				public int VerifySettingsNode() {
//						int lResult = RB_OK;
//						int lVerifyResult;
//						
//						_Settings = XmlTools.getXmlNodeByName(XML_SETTINGS, _Root);
//						if (_Settings == null) {
//								RebuildSettings();
//								return RB_SETTINGS;
//							}
//						
//						lResult = (lVerifyResult = VerifyLocale()) == RB_OK ? lResult : lVerifyResult;
//						lResult = (lVerifyResult = VerifyUINode()) == RB_OK ? lResult : lVerifyResult;
//						lResult = (lVerifyResult = VerifySecurity()) == RB_OK ? lResult : lVerifyResult;
//						lResult = (lVerifyResult = VerifyOffline()) == RB_OK ? lResult : lVerifyResult;
//						
//						XmlNode lNode = XmlTools.getXmlNodeByName(XML_EMBEDDEDPARAM, _Settings);
//						
//						if (lNode == null) {
//								RebuildSettings(false);
//								return RB_SETTINGS;
//							} else {
//								_EmbeddedParam = new CXmlSettingsNode(lNode);
//							}
//						
//						return lResult;
//					}
//					
//				protected void RebuildLocale() {
//						if (_Locale == null)
//								_Locale = XmlTools.CreateXmlNode(_Doc, XML_LOCALE, _Settings);
//						XmlTools.SetAttrib(_Locale, XMLa_LANG, "hu-hu");
//						_LocaleRebuilt = true;
//					}
//				
//				public int VerifyLocale() {
//						_Locale = XmlTools.getXmlNodeByName(XML_LOCALE, _Settings);
//						if (_Locale == null) { RebuildLocale(); return RB_LOCALE; }
//						if (!XmlTools.IsAttr(_Locale, XMLa_LANG)) { RebuildLocale(); return RB_LOCALE; }
//							
//						return RB_OK;
//					}
//					
//				protected void RebuildOffline() {
//				        XmlNode lNode = XmlTools.getXmlNodeByName(XML_OFFLINE, _Settings);
//				        if (lNode == null)
//				                _Offline = new CXmlSettingsNode(XmlTools.CreateXmlNode(_Doc, XML_OFFLINE, _Settings));
//				            else
//				                _Offline = new CXmlSettingsNode(lNode);
//				                
//				        TRSOffLineSettingsRec lDefault = new TRSOffLineSettingsRec();
//				        lDefault.Timeout = 10;
//				        lDefault.Enabled = true;
//				        OfflineSettings = lDefault;
//				    }
//					
//				public int VerifyOffline() {
//				        XmlNode lNode = XmlTools.getXmlNodeByName(XML_OFFLINE, _Settings);
//				        if (lNode == null) { RebuildOffline(); return RB_OFFLINE; }
//				        if (!XmlTools.IsAttr(lNode, XMLa_ENABLED)) { RebuildOffline(); return RB_OFFLINE; }
//				        if (!XmlTools.IsAttr(lNode, XMLa_TIMEOUT)) { RebuildOffline(); return RB_OFFLINE; }
//				        
//				        _Offline = new CXmlSettingsNode(lNode);
//				        
//				        return RB_OK;
//				    }
//					
//				protected void RebuildSecurity() {
//						if (_Security == null)
//								_Security = XmlTools.CreateXmlNode(_Doc, XML_SECURITY, _Settings);
//						XmlTools.SetAttrib(_Security, XMLa_PATH, _RsSecFile);
//					}
//				
//				public int VerifySecurity() {
//						_Security = XmlTools.getXmlNodeByName(XML_SECURITY, _Settings);
//						if (_Security == null) { RebuildSecurity(); return RB_SECURITY; }
//						if (!XmlTools.IsAttr(_Security, XMLa_PATH)) { RebuildSecurity(); return RB_SECURITY; }
//							
//						return RB_OK;
//					}
//					
//				protected void RebuildAll() {
//						_Root = XmlTools.CreateXmlNode(_Doc, XML_RSSETTINGS, _Doc);
//						RebuildSettings();
//						RebuildCollections();
//						SaveSettings();
//					}
//					
//				public int VerifySettings() {
//						int lResult = RB_OK;
//						int lVerifyResult;
//				
//						_Root = XmlTools.getXmlNodeByName(XML_RSSETTINGS, _Doc);
//						if (_Root == null) {
//								RebuildAll();
//								
//								CRSInitLang lLangInit = new CRSInitLang();
//								lLangInit.Execute();
//								
//								return RB_ALL;
//							}
//							
//						lResult = (lVerifyResult = VerifySettingsNode()) == RB_OK ? lResult : lVerifyResult;
//						lResult = (lVerifyResult = VerifyCollactions()) == RB_OK ? lResult : lVerifyResult;
//						
//						XmlTools.SetAttrib(_Root, XMLa_VERSION, RsViewEngine.Version.ToString());
//						
//						if (_LocaleRebuilt) {
//								CRSInitLang lLangInit = new CRSInitLang();
//								lLangInit.Execute();
//							}
//						
//						
//						SaveSettings();
//						return RB_OK;
//					}
//					
//				public void SaveMainFormPosition(Form aForm) {
//						MainFormIsMaximized = aForm.WindowState == FormWindowState.Maximized;
//						if (!MainFormIsMaximized) {
//								MainFormLocation = aForm.Location;
//								MainFormSize = aForm.Size;
//							}
//						
//					}
//					
//				public void RegisterCollections(ArrayList aList) {
//						for (int i = _Collections.ChildNodes.Count-1; i >=0; i--) {
//								if (_Collections.ChildNodes[i].Name.ToUpper() == XML_COLLCETION.ToUpper())
//										_Collections.RemoveChild(_Collections.ChildNodes[i]);
//							}
//							
//						foreach (object iObj in aList) if (iObj is CReportCollection) {
//								CReportCollection iCollection = (CReportCollection)iObj;
//								XmlNode lNewitem = XmlTools.CreateXmlNode(_Doc, XML_COLLCETION, _Collections);
//								XmlTools.AddNewAttr(_Doc, lNewitem, XMLa_PATH, iCollection.FileName);
//							}
//					}
//					
//				public void SaveSettings() {
//						_Doc.Save(_RsSettingsFile);
//					}
//					
//				public void LoadSettings() {
//						LoadSettings(_RsSettingsFile);
//					}
//					
//				public void LoadSettings(string aPath) {
//						_RsSettingsFile = aPath;
//						_Doc.Load(_RsSettingsFile);
//						if (VerifySettings() != RB_OK)
//								SaveSettings();
//					}
//					
//				public void InstallProfile() {
//						FileInfo lBinInfo = new FileInfo(System.Windows.Forms.Application.ExecutablePath);
//						_AppPath = FileSystemTools.toPath(lBinInfo.Directory.Parent.FullName);
//						
//						_UsrProfilePath = FileSystemTools.toPath(System.Environment.GetEnvironmentVariable("APPDATA"));
//						_RsProfilePath = _UsrProfilePath + SETTINGSDIR;
//						FileSystemTools.forcePath(_RsProfilePath);
//						
//						_RsSettingsFile = FileSystemTools.toPath(_RsProfilePath) + SETTINGSFILENAME;
//						_RsSecFile = FileSystemTools.toPath(_RsProfilePath) + SECURITYFILENAME;
//						_RsFavFile = FileSystemTools.toPath(_RsProfilePath) + FAVORITESFILENAME;
//					}
//					
//				public CReportSmartSettings(XmlDocument aDoc, string aPath) {
//						_RsSettingsFile = aPath;
//						_Doc = aDoc;
//					}
//					
//				public CReportSmartSettings() {
//						_Root = null;
//						_Settings = null;
//						_Locale = null;
//						_UINode = null;
//						_Collections = null;
//				
//						InstallProfile();
//						_Doc = new XmlDocument();
//						if (File.Exists(_RsSettingsFile))
//								_Doc.Load(_RsSettingsFile);
//							else {
//								_Doc.AppendChild(_Doc.CreateXmlDeclaration("1.0", "utf-8", ""));
//								_Doc.AppendChild(_Doc.CreateDocumentType("ReportSmart-Settings", null, null, null));
//								RebuildAll();
//							}
//					}
//			}
		#endregion
	}