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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

using ReportSmart;
using ReportSmart.Controls;
using ReportSmart.Graph;
using ReportSmart.Graph.Drawing;

namespace ReportSmart.Forms {
		public class StripToolPage {
				// Data members:
				private string _Title;
				private ArrayList _Groups;
				private bool _Visible;
				protected internal StripToolItem hlItem = null;
				
				#region Properties:
				public CPagedToolStrip ToolStrip { get; protected set; }
				
				public string Title {
						get { return _Title; }
						set {
								_Title = value;
								UpdateControl();
							}
					}
					
				public bool Visible {
						get {
								return _Visible && (ToolStrip.PageSet == null ? true : ToolStrip.PageSet.Includes(this));
							}
						set { 
								_Visible = value;
								if (ToolStrip != null)
										ToolStrip.Invalidate();
							}
					}
					
				public StripToolGroup this[int iIndex] {
						get { return (StripToolGroup)_Groups[iIndex]; }
						set {
								_Groups[iIndex] = value;
								if (ToolStrip != null)
										ToolStrip.Invalidate();
							}
					}
				#endregion
				
				// Methods:
				protected internal void setHighLighted(StripToolItem aItem) {
						foreach (object iItem in _Groups)
								((StripToolGroup)iItem).setHighlight(aItem);
					}
				
				protected internal void updateByPoint(Point aPoint) {
						// Check if height in icon-range:
						int lHeight = ToolStrip.IconSize.Height;
						int lTop = (ToolStrip.PagesTop - lHeight) / 2;
						
						if (aPoint.Y > lTop && aPoint.Y < lTop + lHeight) {
								// In this case, must be found the group:
								int lAllW = ToolStrip.IconSpacing;
								int lCWidth = 0;
								
								for (int i = 0; i < _Groups.Count; i++) {
										lCWidth = this[i].GetGroupSize(this.ToolStrip).Width;
										if (aPoint.X > lAllW && aPoint.X < lAllW + lCWidth) {
												this[i].updateByPoint(aPoint, lAllW, this);
											}
										lAllW += lCWidth + ToolStrip.IconSpacing;
									}
							}
							
						if (ToolStrip != null)
							ToolStrip.hlItem = this.hlItem;
					}
				
				protected internal void setHost(CPagedToolStrip aHost) {
						ToolStrip = aHost;
						UpdateControl();
					}
					
				protected internal void DrawPage(Graphics aGraph, Size aClientSize) {
						if (ToolStrip == null)
								return;
								
						int lAllW = ToolStrip.IconSpacing;
						int lCWidth = 0;
						int lHeight = ToolStrip.IconSize.Height + ToolStrip.IconSpacing;
						int lTop = (aClientSize.Height - lHeight) / 2;
								
						for (int i = 0; i < _Groups.Count && lAllW < aClientSize.Width; i++) {
								Brush lbrBackGround = new SolidBrush(this[i].GroupColor);
						
								lCWidth = this[i].GetGroupSize(this.ToolStrip).Width;
															
								//aGraph.FillRectangle(lbrBackGround, lAllW, lTop, lCWidth, lHeight);
								Draw.RoundedRect(aGraph, new Rectangle(lAllW, lTop, lCWidth, lHeight), ToolStrip.IconSpacing, new Pen(Color.Black), lbrBackGround);
								
								this[i].DrawGroup(aGraph, new Point(lAllW, lTop), this);
								
								lAllW += lCWidth + ToolStrip.IconSpacing;
							}
					}
					
				public void UpdateControl() {
						if (ToolStrip != null)
								ToolStrip.Invalidate();
					}
					
				public void Release() {
						if (ToolStrip != null) {
								ToolStrip.Remove(this);
							}
							
						for (int i = 0; i < _Groups.Count; i++) {
								((StripToolGroup)_Groups[i]).Release();
							}
						UpdateControl();
					}
					
				public StripToolGroup Add(StripToolGroup aGroup) {
						if (_Groups.IndexOf(aGroup) < 0) {
								_Groups.Add(aGroup);
								aGroup.setPage(this, true);
							}
						UpdateControl();
						return aGroup;
					}
					
				public StripToolGroup Add() {
						return Add(new StripToolGroup(this.ToolStrip));
					}
					
				public void Remove(StripToolGroup aGroup) {
						if (_Groups.IndexOf(aGroup) >= 0) {
								_Groups.Remove(aGroup);
							}
						UpdateControl();
					}
				
				#region Constructors:
				public StripToolPage(CPagedToolStrip aToolStrip, string aTitle) {
						ToolStrip = aToolStrip;
						_Title = aTitle;
						_Groups = new ArrayList();
						_Visible = true;
					}
				#endregion
			}
	}
