/*
 *
 * Licensing:			GPL
 * Original project:	EmbeddedApp.csproj
 *
 * Copyright: Adam ReportSmart (2009.09.28.)
 * 
 * 
 */
 
 using System;
 using System.Collections;
 using System.Diagnostics;
 using System.Runtime.CompilerServices;
 using System.Runtime.InteropServices;
 using System.Windows.Forms; 
 
 namespace ReportSmart.Special {
 		namespace WinApi {
 				public static class WindowManagement {
 						#region FIELDS:
 						#region FIELDS - CONSTANTS:
 						public const int SWP_NOSIZE = 0x0001;
						public const int SWP_NOMOVE = 0x0002;
						public const int SWP_NOZORDER = 0x0004;
						public const int SWP_NOREDRAW = 0x0008;
						public const int SWP_NOACTIVATE = 0x0010;
						public const int SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
						public const int SWP_SHOWWINDOW = 0x0040;
						public const int SWP_HIDEWINDOW = 0x0080;
						public const int SWP_NOCOPYBITS = 0x0100;
						public const int SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
						public const int SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */
						#endregion
 				
 						#region FIELDS - READ ONLY:
 						public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
						public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
						public static readonly IntPtr HWND_TOP = new IntPtr(0);
						public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
						#endregion
						#endregion
 						
 						#region METHODS:
 						#region METHODS - EXTERNAL:
 						[DllImport("user32.dll")]
 						public static extern int SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
 						
 						[DllImport("user32.dll")]
 						public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
 					
 						[DllImport("user32.dll")]
						public static extern int SetParent(IntPtr hWndChild, int hWndNewParent);
						
						[DllImport("user32.dll")]
						public static extern bool GetClientRect(int hWnd, out Rect lpRect);
    					
						[DllImport("user32.dll")]
						public static extern bool IsWindow(int hWnd);
						#endregion
 						#endregion
 					}
 		
 				public static class Messaging {
 						public const int WM_APP = 0x8000;
 						public const int WM_REGISTERME = WM_APP + 0x0001;
 						public const int WM_REGISTERACCEPTED = WM_APP + 0x0002;
 						public const int WM_PARENTRESIZED = WM_APP + 0x0003;
 						public const int WM_SUICIDED = WM_APP + 0x0004;
 						public const int WM_APPSPECIFIC = WM_APP + 0x0080;
 						
 						// Standard Windows messages:
 						public const int WM_CREATE = 0x0001;
						public const int WM_NCHITTEST = 0x0084;
						public const int WM_NCCALCSIZE = 0x0083;

 						[DllImport("user32.dll")]
 						public static extern int SendMessage(int hWnd, uint Msg, uint wParam, long lParam);
 							
 						[DllImport("user32.dll")]
 						public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, long lParam);

 						[DllImport("user32.dll")]
 						public static extern int PostMessage(int hWnd, uint Msg, uint wParam, long lParam);

 						[DllImport("user32.dll")]
 						public static extern int PostMessage(IntPtr hWnd, uint Msg, uint wParam, long lParam);

						[DllImport("kernel32.dll")]
						public static extern int GetProcessId(IntPtr aProcess);
 					}

				[Serializable, StructLayout(LayoutKind.Sequential)]
 				public struct Rect {
      					public int Left;
      					public int Top;
      					public int Right;
      					public int Bottom;
      					
      					public int Width {
      							get { return Right - Left; }
      							set { Right = Left + value; }
      						}
      						
      					public int Height {
      							get { return Bottom - Top; }
      							set { Bottom = Top + value; }
      						}
      				} 				
 			}
 
 		public class CChildApplications {
 				private ArrayList _StartedApps;
 			
 				public void TerminateApplication(Process aProcess) {
 						int i = _StartedApps.IndexOf(aProcess);
 						if (i > -1) {
 								_StartedApps.Remove(i);
 								if (!aProcess.HasExited)
 										aProcess.Kill();
 							}
 					}
 					
 				public void AddChildApplication(Process aProcess) {
 						_StartedApps.Add(aProcess);
 					}
 				
 				public void TerminateAll() {
 						for (int i = 0; i < _StartedApps.Count; i++) {
 								if (_StartedApps[i] != null)
 										if (!((Process)_StartedApps[i]).HasExited)
 												((Process)_StartedApps[i]).Kill();
 							}
 						_StartedApps.Clear();
 					}
 					
 				public Process StartApplication(string aApplication, string aParameters) {
 						Process lNewApp = Process.Start(aApplication, aParameters);
 						_StartedApps.Add(lNewApp);
 						return lNewApp;
 					}
 					
 				public CChildApplications() {
 						_StartedApps = new ArrayList();
 					}
 			}
 
 		public class CHostApplication {
 				public const string strEMBEDDED_MARKER = "--embedded";
 				
 				private Control _HostControl;
 				
 				public Control HostControl { get { return _HostControl; }}

 				public void StartEmbeddedApp(string aApplication) { StartEmbeddedApp(aApplication, ""); }
 				
 				public Process StartEmbeddedApp(string aApplication, string aParameters) {
 						return Process.Start(aApplication, strEMBEDDED_MARKER + " " + _HostControl.Handle + " " + aParameters);
 					}

 				public CHostApplication(Control aHostControl) {
 						_HostControl = aHostControl;
 					}
 			}
 			
 		public class CEmbeddedApplication {
 				public static bool IsEmbeddedApplication(string[] aAppArgs) {
 						return aAppArgs.Length >= 2 && aAppArgs[0] == CHostApplication.strEMBEDDED_MARKER;
 					}
 					
 				private int _Handle;
 				private Form _MainForm;
 				
 				public int Handle { get { return _Handle; }}
 				
 				public Form MainForm {
 						get { return _MainForm; }
 						set { _MainForm = value; }
 					}
 				
 				public void IntegrateMainForm() {
 						if (_MainForm != null) {
 								MainForm.FormBorderStyle = FormBorderStyle.None;
 								WinApi.WindowManagement.SetParent(MainForm.Handle, _Handle);
 								MainForm.WindowState = FormWindowState.Maximized;
 								MainForm.Visible = true;
 							}
 					}
 				
 				public void IntegrateControl(Control aControl) {
 						WinApi.Rect lRect;
 				
 						WinApi.WindowManagement.SetParent(aControl.Handle, _Handle);
 						WinApi.WindowManagement.GetClientRect(_Handle, out lRect);
 						aControl.Size = new System.Drawing.Size(lRect.Width, lRect.Height);
 					}
 				
 				public CEmbeddedApplication(string aHandleStr) {
 						_Handle = int.Parse(aHandleStr);
 					}
 			}
 	}

