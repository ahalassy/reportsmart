/*
 *
 * Licensing:			GPL
 * Original project:	ReportSmart.Graphics.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.05.)
 * 
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

using ReportSmart;
using ReportSmart.Controls;
using ReportSmart.Graph;
using ReportSmart.Graph.Drawing;


namespace ReportSmart.Forms {
		public class StripToolGroup {
				// Members:
				private ArrayList _HostPages;
				private List<StripToolItem> _Items;
				private Color _Color = Color.LightBlue;
				protected internal StripToolItem _Highlighted = null;
				
				#region Properties:
				public StripToolItem this[int iIndex] {
						get { return (StripToolItem)_Items[iIndex]; }
						set { _Items[iIndex] = value; }
					}
				
				public Color GroupColor {
						get { return _Color; }
						set {
								_Color = value;
								Redraw();
							}
					}
				
				public bool Enabled {
						set {
								for (int i = 0; i < _Items.Count; i++)
										this[i].Enabled = value;
							}
					}
				
				public CPagedToolStrip ToolStrip { get; protected set; }
				#endregion
				
				#region Methods:
				public Size GetGroupSize(CPagedToolStrip lToolStrip) {
						return new Size(getWidth(lToolStrip), lToolStrip.IconSize.Height + 8);
					}
				
				protected int getWidth(CPagedToolStrip lToolStrip) {
						int lResult = lToolStrip.IconSpacing;
						
						for (int i = 0; i < _Items.Count; i++) {
								lResult += this[i].GetWidth();
							}
							
						lResult += (_Items.Count-1) * lToolStrip.IconSpacing;
							
						return lResult;
					}
					
				protected internal void setHighlight(StripToolItem aItem) {
						_Highlighted = aItem;
					}
					
				protected internal void updateByPoint(Point aPoint, int aLeft, StripToolPage aSender) {
						aSender.hlItem = null;
						_Highlighted = null;
						int lX = aPoint.X - aLeft;
						int lLeft = aSender.ToolStrip.IconSpacing / 2;
						
						for (int i = 0; i < _Items.Count && lX > lLeft; i++) {
								int lWidth = this[i].GetWidth();
								if (lX > lLeft && lX < lLeft + lWidth) {
										_Highlighted = this[i];
										if (aSender != null)
											aSender.hlItem = this[i];
										_Highlighted.Redraw();
										break;
									}
									
								lLeft += lWidth + aSender.ToolStrip.IconSpacing;
							}
						
					}
				
				protected internal void setPage(StripToolPage aPage, bool aAdd) {
						if (aAdd) {
								if (_HostPages.IndexOf(aPage) < 0)
										_HostPages.Add(aPage);
							} else {
								if (_HostPages.IndexOf(aPage) > -1)
										_HostPages.Remove(aPage);
							}
					}
					
				public void Redraw() {
						ArrayList lDrawed = new ArrayList();
						foreach (object iItem in _HostPages) {
								StripToolPage lPage = (StripToolPage)iItem;
								if (lPage != null && lPage.ToolStrip != null && lDrawed.IndexOf(lPage.ToolStrip) < 0) {
										lPage.ToolStrip.Invalidate();
										lDrawed.Add(lPage.ToolStrip);
									}
							}
						
					}
				
				public void DrawGroup(Graphics aGraph, Point aLocation, StripToolPage aSender) {
						Pen lPen = new Pen(Color.Black);
						int lLeft = aLocation.X + aSender.ToolStrip.IconSpacing / 2; 
						int lTop = aLocation.Y + aSender.ToolStrip.IconSpacing / 2;
						int lSpacing = aSender.ToolStrip.IconSpacing;
						int lHeight = aSender.ToolStrip.IconSize.Height;
				
						foreach (StripToolItem iItem in _Items) {
								int lWidth = iItem.GetWidth();
								
								
								iItem.DrawTo(
											aGraph,
											new Point(lLeft, lTop),
											new Size(lWidth, aSender.ToolStrip.IconSize.Height),
											lSpacing,
											this._Highlighted == iItem
										);
						
								lLeft += lWidth + aSender.ToolStrip.IconSpacing;
							}
					}
				
				public void UpdateControl() {
						foreach (object iPage in _HostPages)
								((StripToolGroup)iPage).UpdateControl();
					}
					
				public StripToolItem Add(StripToolItem aItem) {
						if (_Items.IndexOf(aItem) < 0) {
								_Items.Add(aItem);
								aItem.setGroup(this, true);
							}
							
						return aItem;
					}
				
				public StripToolItem Add() {
						return Add(new StripToolItem(this.ToolStrip));
					}
					
				public void Remove(StripToolItem aItem) {
						if (_Items.IndexOf(aItem) > -1) {
								_Items.Remove(aItem);
								aItem.setGroup(this, false);
							}
					}
					
				public void Release() {
						foreach (object iItem in _Items) {
								Remove((StripToolItem)iItem);
							}
							
						foreach (object iPage in _HostPages) {
								((StripToolPage)iPage).Remove(this);
							}
					}
				#endregion
				
				#region Constructors:
				public StripToolGroup(CPagedToolStrip aToolStrip) {
						ToolStrip = aToolStrip;
						_HostPages = new ArrayList();
						_Items = new List<StripToolItem>();
					}
				#endregion
			}
}
