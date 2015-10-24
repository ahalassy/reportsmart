#region Source information

//*****************************************************************************
//
//    StripToolItem.cs
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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ReportSmart.Graph.Drawing;

namespace ReportSmart.Forms
{
    public enum StripToolItemType {
				Icon,
				Button,
				Separator,
				Multifunctional
			}
			
		public class StripToolItem {
		
				#region Classes:
				public class StripToolItemAction {
						public event ToolItemAction Action;
						
						public string Text;
						public string Hint;
						
						public StripToolItem Item { get; protected set; }
						
						public void ActionEventHandler(object aSender, EventArgs aE) {
								DoAction();
							}
						
						public void DoAction() {
								if (Action != null)
										Action(Item);
							}
							
						public StripToolItemAction(StripToolItem aItem) {
								Item = aItem;
							}
					}
				#endregion
		
				#region Fields:
				private ArrayList _Hosts;
				private bool _Enabled = true;
				private bool _Pushed = false;
	
				protected StripToolItemType _Type = StripToolItemType.Icon;
				
				public Image Glyph = null;
				public string Hint = "";
				
				public event ToolItemAction Action;
				#endregion
				
				#region Properties:
				public List<StripToolItemAction> Actions { get; protected set; }
				public CPagedToolStrip ToolStrip { get; protected set; }
				
				public bool Enabled {
						get { return _Enabled; }
						set {
								_Enabled = value;
								Redraw();
							}
					}
					
				public StripToolItemType Type {
						get { return _Type; }
						set { 
								_Type = value;
								Redraw();
							}
					}
					
				public bool Pushed {
						get { return _Pushed; }
						set {
								_Pushed = value;
								Redraw();
							}
					}
				
				public Point LastLocation { get; protected set; }
				#endregion
			
				#region Methods:
				protected internal void setGroup(StripToolGroup aGroup, bool aAdd) {
						if (aAdd) {
								if (_Hosts.IndexOf(aGroup) < 0)
										_Hosts.Add(aGroup);
							} else {
								if (_Hosts.IndexOf(aGroup) > -1) {
										_Hosts.Remove(aGroup);
										aGroup.Remove(this);
									}
							}
					}
				
				protected void Initialize(CPagedToolStrip aToolStrip) {
						ToolStrip = aToolStrip;
						_Hosts = new ArrayList();
						Actions = new List<StripToolItem.StripToolItemAction>();
					}
				
				protected void ShowContextMenu() {				
						ContextMenu lMenu = new ContextMenu();
						Point lMenuPoint = new Point(
									LastLocation.X,
									LastLocation.Y + ToolStrip.IconSize.Height
								);
						
						foreach (StripToolItemAction iAction in Actions)
								lMenu.MenuItems.Add(iAction.Text, iAction.ActionEventHandler);
							
						lMenu.Show(ToolStrip, lMenuPoint);
					}
					
				public StripToolItemAction AddAction() {
						StripToolItemAction lAction = new StripToolItemAction(this);
						Actions.Add(lAction);
						return lAction;
					}
					
				public bool IsDirectAction() {
						return Type != StripToolItemType.Separator && Type != StripToolItemType.Multifunctional;
					}
				
				public int GetWidth() {
						switch (Type) {
								case StripToolItemType.Separator:
										return ToolStrip.IconSize.Width / 2;
								
								case StripToolItemType.Multifunctional:
										return ToolStrip.IconSize.Width + ToolStrip.IconSize.Width / 2;
										
								default:
										return ToolStrip.IconSize.Width;
							}						
					}
								
				public void Redraw() {
						foreach (object iGroup in _Hosts) {
								StripToolGroup lGroup = (StripToolGroup)iGroup;
								lGroup.Redraw();
							}
					}
					
				public void DrawTo(Graphics aGraph, Point aLocation, Size aSize, int aSpacing, bool aHighlighted) {
						LastLocation = aLocation;
						
						int lWidth = aSize.Width;
						int lHeight = aSize.Height;
						int lIconWidth = aSize.Height > aSize.Width ? aSize.Width : aSize.Height;
						int lLeft = aLocation.X;
						int lTop = aLocation.Y;
						
						Brush lbrSwOn = new LinearGradientBrush(
									new Point(lLeft, lTop),
									new Point(lLeft, lTop + lWidth),
									Color.FromArgb(80, Color.Black),
									Color.FromArgb(80, Color.White)
								);
								
						if (_Pushed)
								Draw.RoundedRect(
											aGraph,
											new Rectangle(lLeft - aSpacing/4, lTop - aSpacing/4, lWidth + aSpacing/2, lHeight + aSpacing/2),
											aSpacing/2,
											new Pen(Color.White),
											new SolidBrush(Color.FromArgb(96, Color.White))
										);


						if (aHighlighted && Enabled)
								Draw.RoundedRect(
											aGraph,
											new Rectangle(lLeft - aSpacing/4, lTop - aSpacing/4, lWidth + aSpacing/2, lHeight + aSpacing/2),
											aSpacing/2,
											null,
											new SolidBrush(Color.FromArgb(128, Color.White))
										);
							
								
						if (Glyph != null)
								if (!Enabled)
										Draw.ImageWithAlpha(
												aGraph,
												Glyph,
												new Rectangle(lLeft, lTop, lWidth, lHeight),
												40
											);
									else
										aGraph.DrawImage(Glyph, new RectangleF(lLeft, lTop, Glyph.Width, Glyph.Height));
					}
				
				public void Release() {
						foreach (object iHost in _Hosts)
								((StripToolGroup)iHost).Remove(this);
					}
					
				public virtual void DoAction() {
						switch (Type) {
								case StripToolItemType.Button:
										_Pushed ^= true;
										Redraw();
									break;
									
								case StripToolItemType.Multifunctional:
										ShowContextMenu();
									break;
							}
				
						if (Action != null && IsDirectAction())
								Action(this);
					}
				#endregion
			
				#region Constructors:
				public StripToolItem(CPagedToolStrip aToolStrip) {
						Initialize(aToolStrip);
					}
					
				public StripToolItem(CPagedToolStrip aToolStrip, Image aImage) {
						Initialize(aToolStrip);
						Glyph = aImage;
					}
				#endregion
			}
	}
