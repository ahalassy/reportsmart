#region Source information

//*****************************************************************************
//
//    StripToolPageset.cs
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
using System.Windows.Forms;

using ReportSmart;
using ReportSmart.Controls;
using ReportSmart.Graph;
using ReportSmart.Graph.Drawing;

namespace ReportSmart.Forms {
		public class StripToolPageset {
				private CPagedToolStrip _Host;	
				private ArrayList _List;
				
				public CPagedToolStrip Host {
						get { return _Host; }
					}
				
				public int Count { get { return _List.Count; }}
				
				public StripToolPage this[int aIndex] {
						get { return (StripToolPage)_List[aIndex]; }
						set { _List[aIndex] = value; }
					}
				
				public void Add(string aIndex) {
						if (_List.IndexOf(Host[aIndex]) < 0)
								_List.Add(Host[aIndex]);
					}
					
				public void Add(StripToolPage aPage) {
						Host.Add(aPage);
						if (_List.IndexOf(aPage) < 0)
								_List.Add(aPage);
					}
					
				public void Remove(StripToolPage aPage) {
						if (_List.IndexOf(aPage) > -1)
								_List.Remove(aPage);
					}
					
				public bool Includes(StripToolPage aPage) {
						return _List.IndexOf(aPage) > -1;
					}
					
				public void Release() {
						_List.Clear();
						_Host = null;
					}
					
				public StripToolPageset(CPagedToolStrip aHost) {
						_Host = aHost;
						_List = new ArrayList();
					}
				
			}

	}
