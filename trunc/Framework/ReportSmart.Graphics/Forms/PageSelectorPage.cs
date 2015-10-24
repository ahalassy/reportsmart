/*
 *
 * Licensing:			GPL
 * Original project:	ReportSmart.Graphics.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.06.)
 * 
 * 
 */

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Timers;
using System.Windows.Forms;

using ReportSmart;
using ReportSmart.Forms;
using ReportSmart.Graph.Drawing;
using ReportSmart.GUI;
using ReportSmart.Special;
using ReportSmart.Special.WinApi;


namespace ReportSmart.Controls {
	public class CPageSelectorPage {
			public event PageCloseNotify CloseNotify;
			public event PageEventNotify PageSelected;
	
			private bool _CloseAble;
			private string _PageTitle;
			private Control _Page;
			private CPageSelector _HostSelector;
			
			protected internal CPageSelector HostSelector {
					get { return _HostSelector; }
					set { _HostSelector = value; }
				}
			protected internal int RequiredLeft, CurrentLeft;
			protected internal int RequiredWidth, CurrentWidth;
			
			public bool CloseAble {
					get { return _CloseAble; }
					set { _CloseAble = value; }
				}
				
			public string PageTitle {
					get { return _PageTitle; }
					set { _PageTitle = value; }
				}
				
			public Control Page {
					get { return _Page; }
					set { _Page = value; }
				}			
			
			public virtual void DoSelect() {
					if (this.PageSelected != null)
							PageSelected(this);
				}
			
			public virtual void ClosePage() {
					bool lCloseAble = true;
					if (this.CloseNotify != null)
							lCloseAble = CloseNotify(this, lCloseAble);
							
					if (lCloseAble && _HostSelector != null) {
							_HostSelector.RemovePage(this);
						}
				}
			
			public CPageSelectorPage(Control aControl) {
					_Page = aControl;
					_PageTitle = aControl == null ? "-" : aControl.Text;
					_CloseAble = true;
					_HostSelector = null;
				}
		}
	
	}
