/*
 *
 * Licensing:			GPL
 * Original project:	Halassy.Graphics.csproj
 *
 * Copyright: Adam Halassy (2010.12.05.)
 * 
 * 
 */
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

using Halassy;
using Halassy.Controls;
using Halassy.Graph;
using Halassy.Graph.Drawing;

namespace Halassy.Forms {
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
