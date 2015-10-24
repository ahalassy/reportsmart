#region Source information

//*****************************************************************************
//
//    RsViewEngine.cs
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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ReportSmart.Controls;
using ReportSmart.Documents.Collections;
using ReportSmart.Engine;
using ReportSmart.Engine.Config;
using ReportSmart.Forms;
using ReportSmart.Localization;
using ReportSmart.Network.Email;
using ReportSmart.Security;
using ReportSmart.Special;
using ReportSmart.Special.WinApi;
using ReportSmart.Windows.Forms;

namespace ReportSmart.Application
{

    public class CFavoritesCollection: RsReportCollection {
				protected override CReportRootFolder createReportRootFolder(string aCollectionName) {
						return new CReportFavoritesRoot(RsViewEngine.ALIAS_FAVORITES, this);
					}
				
			}
		
		internal static class RsViewEngine{
				#if DEMO
				private const int _YM     = 140;
				private const int _MM     =  38;
				private const int _DM     =   6;
				private const int _PERIOD =  30;
				#endif
		
				public static bool EmergencyDown = false;
		
				private const string DEFAULT_LOCALE = "hun";
		
				public const string ALIAS_FAVORITES = "favorites";
		
				public static RsViewProfileManager ProfileManager = null;
				
				public static DateTime ReferenceTime {
						get { return new DateTime(2010, 5, 23); }
					}
				
				public static string GetProjectID() {
						string lResult = "MY-" + 2009 + "0";
						lResult +=  + 8 + "-0" + 02;
				
						return lResult;
					}
		
				private static bool _IsEmbedded, _IsInitialized = false;
				private static CChildApplications _ChildApps;
				private static CEmbeddedApplication _EmbApp;
				private static Font _DefFont, _TitleFont, _PwdFont, _editFont;
				private static CLocalization _CurrentLocale;
				private static OpenFileDialog _dlgOpenRpt, _dlgOpenCollection;
				private static SaveFileDialog _dlgSaveCollection, _dlgExport;
				private static CFavoritesCollection _Favorites;
				private static CApplicationVersion _Version;
				private static CfSplash _Splash;
				private static CfAddReport _AddReport;
				private static CdlgAddFolder _dlgAddFolder;
				private static CdlgSetupLogin _dlgSetupLogin;
				private static CdlgSendMail _dlgSendMail;
				private static CReportSmartForm _MainForm;
				private static ResourceManager _ResMan, _ResManSpec;
				private static CSecurityData _Security;
			
				public static bool IsEmbedded { get { return _IsEmbedded; }}
				public static CApplicationVersion Version { get { return _Version; }}
				public static RsLocalization Locale { get { return RsLocalization.Current; }}
				public static string FullApplicationName {	get { return RsApplicationInfo.ApplicationName + " (v" + _Version.ToString() + ")"; }}
				public static ResourceManager Resources { get { return _ResMan; }}
				public static ResourceManager SpecialResources { get { return _ResManSpec; }}
				public static Font DefaultFont { get { return _DefFont; }}
				public static Font TitleFont { get { return _TitleFont; }}
				public static Font PasswordFont { get { return _PwdFont; }}
				public static Font EditFont { get { return _editFont; }}
				public static CChildApplications ChildApplications { get { return _ChildApps; }}
				public static CSecurityData RSSecurity { get { return _Security; }}
				
				public static OpenFileDialog dlgOpenReport { get { return _dlgOpenRpt; }}
				public static OpenFileDialog dlgOpenCollection { get { return _dlgOpenCollection; }}
				public static SaveFileDialog dlgExport { get { return _dlgExport; }}
				public static CReportSmartForm MainForm { get { return _MainForm; }}
				public static CfSplash Splash { get { return _Splash; }}
				public static CfAddReport dlgAddReport { get { return _AddReport; }}
				public static CdlgAddFolder dlgAddFolder { get { return _dlgAddFolder; }}
				public static CdlgSetupLogin dlgSetupLogin { get { return _dlgSetupLogin; }}
				public static CdlgSendMail dlgSendMail { get { return _dlgSendMail; }}
				public static RsCollectionManager CollectionManager { get; private set; }
				
				public static RsReportCollection Favorites { get { return _Favorites; }}
				
				public static string GetInstanceName() { return "RsViewEngine"; }
				
				private static void LoadLocalization() {
						RsLocalization.loadRSLocalization(ProfileManager.GetLocalizationFile());
					}
				
				private static void loadFavorites() {
						RsCollectionConfig lFavCfg = ProfileManager.GetFavoritesCollection();
				
						string lFavFile = ProfileManager.GetFavoritesCollection().Path;
						if (File.Exists(lFavFile)) {
								_Favorites = new CFavoritesCollection();
								_Favorites.LoadFromXML(lFavFile);
							} else {
								_Favorites = new CFavoritesCollection();
								_Favorites.SaveToXML(lFavFile);
							}
					}
				
				private static void loadCollections() {
						List<RsCollectionConfig> lCollections = ProfileManager.Profile.Collections;
						foreach (RsCollectionConfig iConfig in lCollections)
								if (iConfig.Type == RsCollectionType.Custom)
										LoadCollection(iConfig.Path);
					}
				
				private static void buildUserInterface() {
						_DefFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
						_TitleFont = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
						_PwdFont = new System.Drawing.Font("Wingdings 2", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
						_editFont = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
				
						_dlgOpenRpt = new OpenFileDialog();
						//_dlgOpenCollection = new OpenFileDialog();
						//_dlgSaveCollection = new SaveFileDialog();
						_dlgExport = new SaveFileDialog();
						
						_AddReport = new CfAddReport();
						_dlgAddFolder = new CdlgAddFolder();
						//_dlgCreateCollection = new CdlgCreateCollection();
						_dlgSetupLogin = new CdlgSetupLogin();
						//_dlgSMTPProfile = new CdlgNewSMTPProfile();

						Locale.AddLocalizedControl(_dlgSetupLogin);
						Locale.AddLocalizedControl(_AddReport);
						#if DEMO
						try {
								long lVal = ReportSmart.Authorization.AppAuth.chkshwauthfrw(ReportSmart.Authorization.AppAuth.crdtstamp(new DateTime(2010, 5, 23), _YM, _MM, _DM), _YM, _MM, _DM, _PERIOD);
								long lChkval = ReportSmart.Authorization.AppAuth.gnchkval(_YM, _MM, _DM, _PERIOD);
								Math.Sqrt(lVal - lChkval);
							} catch {
								KillApplication();
							}
						#endif	
						Locale.AddLocalizedControl(_dlgAddFolder);
						//Locale.AddLocalizedControl(_dlgCreateCollection);
						Locale.AddLocalizedControl(_dlgSetupLogin);
						//Locale.AddLocalizedControl(_dlgSMTPProfile);
						
						//loadFavorites();
						_MainForm.InitializeForm();
						//loadCollections();
						
						RsCollectionControl.Initialize();
						
						//Locale.AddLocalizedControl(this);
						Locale.ApplyLocalization();
					}
					
				private static void buildUIToDefault() {
						MainForm.Size = new Size(
									(int)(Screen.PrimaryScreen.WorkingArea.Width * 0.7),
									(int)(Screen.PrimaryScreen.WorkingArea.Height * 0.7)
								);
								
						MainForm.Location = new Point(
									(Screen.PrimaryScreen.WorkingArea.Width - MainForm.Width) / 2,
									(Screen.PrimaryScreen.WorkingArea.Height - MainForm.Height) / 2
								);
					}

				private static void buildEmbeddedUserInterface() {
						_DefFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
						_TitleFont = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
						_PwdFont = new System.Drawing.Font("Wingdings 2", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
						
						_dlgOpenRpt = new OpenFileDialog();
						_dlgOpenCollection = new OpenFileDialog();
						_dlgSaveCollection = new SaveFileDialog();
						_dlgExport = new SaveFileDialog();

						_dlgSetupLogin = new CdlgSetupLogin();
						_dlgSendMail = new CdlgSendMail();

						Locale.AddLocalizedControl(_dlgSetupLogin);
						Locale.AddLocalizedControl(_dlgSendMail);

						//Locale.AddLocalizedControl(this);
						Locale.ApplyLocalization();
						
					}
				
				private static void loadSecurity() {
						string lFile = RsViewEngine.ProfileManager.SecurityFile;
								
						if (!File.Exists(lFile)) {
								_Security = CSecurityData.CreateNew();
								_Security.Save(lFile);
							} else {
								if (RsOldSecurity.IsOldFormat(lFile))
										RsOldSecurity.ConvertToNew(lFile);
				
								_Security = new CSecurityData();						
								_Security.Load(lFile);
							}
					}
				
				public static bool SaveCollection(RsReportCollection aCollection) {
						if (aCollection.FileName == "") {
								bool lNoSave = false;
						
								while (RsViewEngine._dlgSaveCollection.ShowDialog() != DialogResult.OK) {
										if (CRSMessageBox.ShowBox(
													RsViewEngine.Locale.GetMessage("collectionmustsave"),
													RsViewEngine.Locale.GetMessageTitle("collectionmustsave"),
													MessageBoxButtons.OKCancel,
													MessageBoxIcon.Asterisk
											) == DialogResult.Cancel) {
												lNoSave = true;
												break;
											}
									}
									
								if (!lNoSave)
										aCollection.SaveToXML(RsViewEngine._dlgSaveCollection.FileName);
									else {
										RsViewEngine.CloseCollection(aCollection);	
										return false;
									}
							} else
								aCollection.QuickSave();					
								
						return true;
					}
					
				public static bool CheckReferencedAssemblies() {				
						Assembly lEntryAssembly = Assembly.GetEntryAssembly();
						foreach (AssemblyName iAsmName in lEntryAssembly.GetReferencedAssemblies()) {
								if (iAsmName.Name.IndexOf("System") == 0) continue;
								if (iAsmName.Name.ToUpper().IndexOf("MSCORLIB") == 0) continue;
								if (GACUtil.GetAssembly(iAsmName.Name) == null) {
										CRSMessageBox.ShowBox(
													Locale.GetMessage("assemblyMissing") + '\n' + iAsmName.FullName,
													Locale.GetMessageTitle("assemblyMissing"),
													MessageBoxButtons.OK,
													MessageBoxIcon.Error
												);
												
												
										//return false;
										
									}
							}
						
						return true;
					}
					
				public static void EH_ApplicationExit(object sender, EventArgs e) {
						ShutdownApplication();
					}
					
				public static void SaveSecurity() {
						_Security.Save(RsViewEngine.ProfileManager.SecurityFile);
					}
				
				public static void InitializeCore() {
						System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(EH_ThreadException);
						AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(EH_UnhandledException);
						
						_Security = null;
						_Version = CApplicationVersion.StringToApplicationVersion(RsApplicationInfo.VersionString);
						//_Collections = new ArrayList();
						_ChildApps = new CChildApplications();
					}
					
				public static void InitializeApplication() {
						CMyDialog.DefaultTitleFont = RsViewEngine.TitleFont;
						CMyDialog.MessageFont = RsViewEngine.DefaultFont;
				
						_MainForm = new CReportSmartForm();
				
						_ResMan = RSResources.GFX;
						_ResManSpec = RSResources.Special;
						
						_Splash = new CfSplash();
						_Splash.Show();
						_Splash.SetText("");

						ProfileManager = new RsViewProfileManager();
						ProfileManager.OpenProfile(ProfileManager.RsViewProfile);
						CollectionManager = new RsCollectionManager(ProfileManager);
						LoadLocalization();
						
						_Splash.SetText(Locale.GetMessage("splash_settings"));
					
						loadSecurity();

						_Splash.SetText(Locale.GetMessage("splash_ui"));
						buildUserInterface();
						FileSystem.ClearDefaultTemp(RsApplicationInfo.ApplicationName);
						
						_IsInitialized = true;
						_MainForm.CollectionManagement.CollectionBrowser.RefreshControl();
					}
				
				public static void InitializeEmbedded(CEmbeddedApplication aHostApp) {
						_EmbApp = aHostApp;
						_IsEmbedded = true;
				
						_ResMan = new ResourceManager("ReportSmart.Application.Resources.GFX", Assembly.GetExecutingAssembly());
						
						RsViewEngine.ProfileManager = new RsViewProfileManager();
						RsViewEngine.ProfileManager.OpenProfile(ProfileManager.RsViewProfile);
						LoadLocalization();
						loadSecurity();
						buildEmbeddedUserInterface();

						_IsInitialized = true;
					}
				
				public static void ShutdownApplication() {
						RsViewEngine.ProfileManager.SaveMainFormPosition(_MainForm);
						RsViewEngine.ProfileManager.SaveProfile();
						_ChildApps.TerminateAll();
						
						FileSystem.ClearDefaultTemp(RsApplicationInfo.ApplicationName);
						
						System.Windows.Forms.Application.Exit();
					}
					
				public static void KillApplication() {
						ShutdownApplication();
						System.Environment.Exit(0);
					}
				
				public static void AssignLocale(CLocalization aSender) {
							_CurrentLocale = aSender;
						}
						
				public static void ApplyLocale(CLocalization aSender) {
							XmlNode lGeneralDialogs = Locale.GetDialogData("generalDialogs");
				
							//_dlgOpenRpt:
							_dlgOpenRpt.Filter = FileDialogFilters.BuildReportFilter(lGeneralDialogs);
							_dlgOpenRpt.FilterIndex = 2;
							
							//_dlgOpenCollection:
							_dlgOpenCollection.Filter = FileDialogFilters.BuildCollectionFilter(lGeneralDialogs);
							_dlgOpenCollection.FilterIndex = 2;
							
							#if DEMO
							try {
									long lVal = ReportSmart.Authorization.AppAuth.chkshwauthfrw(ReportSmart.Authorization.AppAuth.crdtstamp(new DateTime(2010, 5, 23), _YM, _MM, _DM), _YM, _MM, _DM, _PERIOD);
									long lChkval = ReportSmart.Authorization.AppAuth.gnchkval(_YM, _MM, _DM, _PERIOD);
									Math.Sqrt(lVal - lChkval);
								} catch {
									KillApplication();
								}
							#endif							
							
							//_dlgSaveCollection:
							_dlgSaveCollection.Filter = FileDialogFilters.BuildCollectionFilter(lGeneralDialogs);
							_dlgSaveCollection.FilterIndex = 2;
							
							// Favorites:
							if (!_IsEmbedded)
									_Favorites.RootFolder.GUINode.Text = Locale.GetTagText("favorites");
						}
						
				public static void ReleaseLocale() {
						//if (_CurrentLocale != null)
						//		_CurrentLocale.RemoveLocalizedControl(this);
					}				
					
				public static RsReportCollection LoadCollection(string aCollectionName) {
						if (!File.Exists(aCollectionName)) {
								if (CRSMessageBox.ShowBox(
											Locale.GetMessageTitle("collectionMissing"),
											Locale.GetMessage("collectionMissing"),
											MessageBoxButtons.YesNo,
											MessageBoxIcon.Question
										) == DialogResult.No) return null;
							}
							
						return CollectionManager.GetCollection(aCollectionName);
				
						//RsReportCollection lNewCollection = CollectionManager.GetCollection(aCollectionName);
						
						#if DEMO
						try {
								long lVal = ReportSmart.Authorization.AppAuth.chkshwauthfrw(ReportSmart.Authorization.AppAuth.crdtstamp(new DateTime(2010, 5, 23), _YM, _MM, _DM), _YM, _MM, _DM, _PERIOD);
								long lChkval = ReportSmart.Authorization.AppAuth.gnchkval(_YM, _MM, _DM, _PERIOD);
								Math.Sqrt(lVal - lChkval);
								
							} catch {
								KillApplication();
							}
						#endif	
						
						//lNewCollection.LoadFromXML(aCollectionName);
						//return lNewCollection;
					}
					
				public static void TreatCriticalError(Exception aException) {
						bool lSendBug = false;
						string lBugMessage = "Technical details:\n";
						if (aException != null) {
								lBugMessage = lBugMessage + aException.Message + "\nIn object \"" + aException.Source + "\"\n";
							} else {
								lBugMessage = lBugMessage + "Unknown error.";
							}
							
						lBugMessage = Version.ToString() + "\n" + lBugMessage;
				
						if (Locale != null)
								if (aException != null) {
										if (aException is NotImplementedException)	{
												CRSMessageBox.ShowBox(
															Locale.GetMessage("debug_notimplemented"),
															Locale.GetMessageTitle("debug_notimplemented"),
															MessageBoxButtons.OK,
															MessageBoxIcon.Error,
															aException.StackTrace
														);
												
												return;

											} else
												lSendBug = CRSMessageBox.ShowBox(
															Locale.GetTagText("appCrashed") + "\n" + aException.Message + "\n" + aException.Source,
															Locale.GetTagText("appCritical"),
															MessageBoxButtons.YesNo,
															MessageBoxIcon.Error,
															aException.StackTrace
														) == DialogResult.Yes;

									} else
										lSendBug = CRSMessageBox.ShowBox(
													Locale.GetTagText("appCrashed"),
													Locale.GetTagText("appCritical"),
													MessageBoxButtons.YesNo,
													MessageBoxIcon.Error,
													aException.StackTrace
												) == DialogResult.Yes;
							else
								lSendBug = MessageBox.Show(
											"An unexpected error caused ReportSmart crash before or during load of localization.\n" +
												"\"" + aException.Message + "\"\n" + aException.Source + "\n\nDo you want to send an error report to the vendor?\n" +
												"\nStack trace: " + aException.StackTrace,
											"ReportSmart",
											MessageBoxButtons.YesNo, MessageBoxIcon.Error
										) == DialogResult.Yes;
						if (_IsInitialized)
								lBugMessage = "Problem after initalization has been finished.\n" + lBugMessage;
							else
								lBugMessage = "Problem before initalization has benn finished.\n" + lBugMessage;
						
						if (_IsEmbedded) {
								lBugMessage = "Embedded instance caused error.\n" + lBugMessage ;
							} else {
								lBugMessage = "Framework caused error.\n" + lBugMessage ;
							}
							
						lBugMessage += "\nStack trace:\n" + aException.StackTrace;
						
						if (lSendBug) {
								IRSStatusBox lStatus;
						
								if (Locale != null)
										lStatus = CRSMessageBox.ShowStatusBox(RsViewEngine.Locale.GetMessage("bugreport"));
									else
										lStatus = CRSMessageBox.ShowStatusBox("Sending bugreport...");
										
								lStatus.SetStatus(20);
								System.Windows.Forms.Application.DoEvents();
								
								CEmailing.BugReport(
										lBugMessage
									);
									
								lStatus.HideBox();
							}
							
						if (_IsEmbedded)
								Messaging.SendMessage(_EmbApp.Handle, Messaging.WM_SUICIDED, 0, 0);
						
						#if (!DEBUG)
						System.Environment.Exit(0);
						#else
						if (_IsEmbedded)
								System.Environment.Exit(0);
							else
								System.Windows.Forms.Application.Exit();
						#endif
					}
					
				public static void CloseCollection(RsReportCollection aCollection) {
						CollectionManager.RemoveCollection(aCollection);
						aCollection.QuickSave();
						aCollection.Release();
					}
					
				private static void EH_ThreadException(object sender, ThreadExceptionEventArgs aEArgs) {
						TreatCriticalError(aEArgs.Exception);
					}
					
				private static void EH_UnhandledException(object sender, UnhandledExceptionEventArgs aEArgs) {
				
						if (aEArgs.ExceptionObject is Exception)
								TreatCriticalError((Exception)(aEArgs.ExceptionObject));
							else
								TreatCriticalError(new Exception("Unknown error occured.\nObject string is \"" + aEArgs.ToString() + "\""));
					}

			}
	}


