#region Source information

//*****************************************************************************
//
//    PageSelectorPage.cs
//    Created by Adam (2015-10-23, 8:59)
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
