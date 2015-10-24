#region Source information

//*****************************************************************************
//
//    PageSelector.cs
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
    public enum TSnapping{
            snapNone,
            snapLeft,
            snapRight
        }
        
    public enum THighlightedCtl {
            ctlPage,
            ctlPrev,
            ctlNext
        }
        
    public enum TBorderIcon {
            biNone,
            biMinimize,
            biMaximize,
            biResize,
            biNormalize,
            biClose,
            biHelp
        }

    public delegate bool PageCloseNotify(CPageSelectorPage aPage, bool aCloseAble);
    public delegate void PageEventNotify(CPageSelectorPage aPage);
    
    public class CPageSelector: Control {
            private struct _sFormInfo {
                    private int _w, _h, _l, _t;
                    public bool Assigned;
                    
                    public int Left { get { return _l; }}
                    public int Top { get { return _t; }}
                    public int Right { get { return _l+_w; }}
                    public int Bottom { get { return _t+_h; }}
                    
                    public int Width { get { return _w; }}
                    public int Height { get { return _h; }}
                    
                    public Point TopLeft { get { return new Point(_l, _t); }}
                    
                    public void RestoreForm(Form aForm, Point aMousePos) {
                            aForm.Size = new Size(_w, _h);
                            int lLeft = aMousePos.X > _l + _w ? aMousePos.X - (_w - 128) : aMousePos.X < _l ? aMousePos.X - 32 : _l;
                            int lTop = aMousePos.Y - 32;
                            
                            aForm.Location = new Point(lLeft, lTop);
                        }
                    
                    public _sFormInfo(Form aForm) { _l = aForm.Left; _h = aForm.Height;	_t = aForm.Top;	_w = aForm.Width; Assigned = true; }
                }
    
            private static class borderHelper {
                    public static TBorderIcon ConvertBorderIcon(TBorderIcon aVal) {
                            switch (aVal) {
                                    case TBorderIcon.biNormalize:
                                    case TBorderIcon.biMaximize: return TBorderIcon.biResize;
                                    default: return aVal;
                                }
                        }
                
                    public static bool IsEqualBorderIcon(TBorderIcon aVal1, TBorderIcon aVal2) {
                            return ConvertBorderIcon(aVal1) == ConvertBorderIcon(aVal2);
                        }
                }
        
        private const int _ARCRAD = 24;
        private const int _RIGHTMARGIN = _ARCRAD * 3;
        private const int _BORDERICON_SIZE = 20;
        
        private bool _CloseHLighted, _Dragging, _Glass, _MouseDown;
        private int _PageWidth = 200;
        private int _Selected = 0, _HighLighted = 0, _Offset = 0;
        private int _PageHeight = 32;
        private _sFormInfo _windowInfo;
        private THighlightedCtl _HlCtl = THighlightedCtl.ctlPage;
        private Pen _BorderPen, _ClosePen;
        private SolidBrush _brNoSelected, _brSelected,  _brText, _brClose;
        private Point _dsMouse, _dsForm;
        private Icon _AppIcon;
        private ArrayList _Pages;
        private Panel _Host;
        private Control _CurrentControl;
        private System.Timers.Timer _Timer;
        private Form _hostForm;
        private TBorderIcon _hlBrIcon;
        
        public Color PageColor {
                get { return _brNoSelected.Color; }
                set { _brNoSelected.Color = value; }
            }
        public Color SelectedPageColor { get; set; }
        
        public int PageWidth {
                get { return _PageWidth; }
                set { _PageWidth = value; Refresh(); }
            }
        public int SelectedPage {
                get { return _Selected; }
                set {
                        _Selected = value;
                        if (_Selected < 0)
                                _Selected = 0;
                        if (_Selected >= _Pages.Count)
                                _Selected = _Pages.Count - 1;
                        Invalidate();
                        DropToHost();
                    }
            }
        public Panel Host {
                get { return _Host; }
                set { _Host = value; }
            }
        public CPageSelectorPage VisiblePage {
                get { return (_Selected < 0 | _Selected >= _Pages.Count) ? null : (CPageSelectorPage)(_Pages[_Selected]); }
                set {
                        if (_Pages.IndexOf(value) < 0)
                                this.AddPage(value);
                        SelectedPage = _Pages.IndexOf(value);
                    }
            }
        public Icon AppIcon {
                get { return _AppIcon; }
                set {
                        _AppIcon = value;
                        _PageHeight = 32;
                        Invalidate();
                    }
            }
        public Rectangle LastNormalSize { get; set; }
        
        protected bool FirstVisible { get { return getFirstLeft() >= 0; }}
        protected bool LastVisible { get { return getLastLeft() <= this.ClientSize.Width - 2*_ARCRAD; }}
        
        public ApplicationMenu FormMenu { get; protected set; }
        
        public CPageSelectorPage HighlightedPage {
                get { return (_HighLighted < 0 | _HighLighted >= _Pages.Count) ? null : (CPageSelectorPage)(_Pages[_HighLighted]); }
            }
        
        public Form HostForm {
                get {
                        Control lCtl = this.Parent;
                        while (lCtl != null && !(lCtl is Form))
                                lCtl = lCtl.Parent;
                                    
                        if (lCtl != null && (lCtl is Form))
                                return (Form)lCtl;
                            else
                                return null;
                    }
            }
        
        public bool Glass {
                get { return _Glass && ReportSmart.GUI.DwmApi.DwmAvailable(); }
                set {
                        _Glass = value;
                        Invalidate();
                    }
            }			
        
        public bool HostFormControlled { get; protected set; }
        
        protected int getFirstLeft() {
                return _Pages.Count > 0 ? ((CPageSelectorPage)(_Pages[0])).RequiredLeft :  (_AppIcon == null ? 0 : _AppIcon.Width +  32);
            }			
        
        protected int getLastLeft() {
                return _Pages.Count > 0 ? ((CPageSelectorPage)(_Pages[_Pages.Count-1])).RequiredLeft : 0;
            }
            
        protected int getPageAtPoint(Point aPoint) {
                if (aPoint.Y < this.ClientSize.Height - _PageHeight)
                        return -1;
                        
                for (int i = 0; i < _Pages.Count; i++) {
                        CPageSelectorPage iPage = (CPageSelectorPage)(_Pages[i]);
                        if (iPage.CurrentLeft <= aPoint.X && (iPage.CurrentLeft + iPage.CurrentWidth) > aPoint.X)
                                return i;
                    }
                    
                return -1;
            }
            
        protected bool ModifyFrame() {
                // Check if all pages are viewable in the screen:
                int ifitW = 0;
                foreach (object iObj in _Pages) {
                        CPageSelectorPage iPage = (CPageSelectorPage)iObj;
                        ifitW += iPage.CurrentWidth;
                    }
                if (ifitW < this.ClientSize.Width && ((CPageSelectorPage)(_Pages[0])).RequiredLeft < 0) {
                        int iFitOffs = 0 - ((CPageSelectorPage)(_Pages[0])).RequiredLeft;
                
                        foreach (object iObj in _Pages)
                                ((CPageSelectorPage)iObj).RequiredLeft += iFitOffs;
                    }
        
                bool lResult = false;
        
                foreach (object iObj in _Pages) {
                        CPageSelectorPage iPage = (CPageSelectorPage)iObj;
                        
                        if (iPage.CurrentLeft != iPage.RequiredLeft) {
                                lResult = true;
                                iPage.CurrentLeft = (iPage.RequiredLeft + iPage.CurrentLeft) / 2;
                            }

                        if (iPage.CurrentWidth != iPage.RequiredWidth) {
                                lResult = true;
                                iPage.CurrentWidth = (iPage.RequiredWidth + iPage.CurrentWidth) / 2;
                            }
                    }
                    
                return lResult;
            }
        
        protected void DropToHost() {
                if (_Host != null && _Selected >= 0) {
                        Control lControl = ((CPageSelectorPage)_Pages[_Selected]).Page;
                        _Host.SuspendLayout();
                        lControl.Location = new Point(0, 0);
                        lControl.Size = new Size(
                                    _Host.ClientRectangle.Width,
                                    _Host.ClientRectangle.Height
                                );
                        lControl.Anchor = (AnchorStyles)(
                                    AnchorStyles.Left | AnchorStyles.Top |
                                    AnchorStyles.Right | AnchorStyles.Bottom
                                );
                        _Host.Controls.Add(lControl);
                        if (_CurrentControl != null && _CurrentControl != lControl) {
                                _Host.Controls.Remove(_CurrentControl);
                            }
                        _CurrentControl = lControl;
                        _CurrentControl.BringToFront();
                        _Host.ResumeLayout(false);
                        UpdateByCursorPos(Control.MousePosition);
                        ((CPageSelectorPage)_Pages[_Selected]).DoSelect();
                    }
            }
        
        protected void startDraggingHost(Point aMousePos) { if (HostForm != null) {
                _Dragging = true;
                _dsMouse = aMousePos;
                _dsForm = HostForm.Location;
            }}
            
        protected void updateDragging(Point aMousePos) { if (HostForm != null) {
                if (_Dragging)
                        HostForm.Location = new Point (
                                    aMousePos.X - _dsMouse.X + _dsForm.X,
                                    aMousePos.Y - _dsMouse.Y + _dsForm.Y
                                );
            }}
            
        protected void endDraggingHost() { _Dragging = false; }
        
        protected void UpdateByCursorPos(Point aLocation) {
                _HlCtl = THighlightedCtl.ctlPage;
                _hlBrIcon = TBorderIcon.biNone;
                
                // Determine if mouse cursor is over Window control button:
                if (HostForm != null) {
                        int lLeft = this.Width - (3 * (_BORDERICON_SIZE*2 + 4));
                        int lTop = 0;
                        int lWidth = 2 *_BORDERICON_SIZE; 
                        int lHeight = _BORDERICON_SIZE;
                        
                        if (aLocation.Y >= lTop && aLocation.Y <= lTop+lHeight && lLeft <= aLocation.X) {
                                int lIndex = (aLocation.X - lLeft) / (lWidth + 4);
                                lIndex = ((lIndex+1) * (lWidth+4))-4 >= aLocation.X-lLeft ? lIndex : -1; 
                                
                                if (lIndex == 0) _hlBrIcon = TBorderIcon.biMinimize; else
                                if (lIndex == 1) _hlBrIcon = TBorderIcon.biResize; else
                                if (lIndex == 2) _hlBrIcon = TBorderIcon.biClose;
                            }
                    }
                
                if (aLocation.Y < this.Height - _PageHeight) {
                        _HighLighted = -1;
                        return;
                    }
        
                _HighLighted = getPageAtPoint(aLocation);
                

                
                // Determine if mouse cursor is over prev/next button				
                if (aLocation.X > this.ClientSize.Width - (_ARCRAD + 8)) {
                        _HlCtl = THighlightedCtl.ctlPrev;
                        if (aLocation.X > this.ClientSize.Width - (_ARCRAD / 2 + 4))
                                _HlCtl = THighlightedCtl.ctlNext;
                    }
                
                // Determine if close is highlighted:
                if (_HighLighted > -1 && _HighLighted < _Pages.Count) {
                        CPageSelectorPage lPage = (CPageSelectorPage)(_Pages[_HighLighted]);
                        int lClLeft = lPage.CurrentLeft + getVisibleWidth(lPage) - _PageHeight;
                
                        _CloseHLighted = (aLocation.X > lClLeft) && (aLocation.X < (lPage.CurrentLeft + getVisibleWidth(lPage)));
                    } else
                        _CloseHLighted = false;
                
                Invalidate();
            }
            
        protected int getVisibleWidth(CPageSelectorPage aPage) {
                return aPage.CurrentLeft + aPage.CurrentWidth < (this.ClientSize.Width - _RIGHTMARGIN)
                        ? aPage.CurrentWidth
                        : (this.ClientSize.Width - _RIGHTMARGIN) - aPage.CurrentLeft;
            }
    
        protected void MaximizeHostForm() {
                LastNormalSize = new Rectangle(
                        );
        
                HostForm.MaximumSize = new Size(
                            Screen.PrimaryScreen.WorkingArea.Width +
                                    2*WindowHelper.SystemMetrics.GetwindowEdgeWidth(),
                            Screen.PrimaryScreen.WorkingArea.Height +
                                    2*WindowHelper.SystemMetrics.GetwindowEdgeWidth()
                        );

            }
    
        protected GraphicsPath getTabStroke(int aX, int aY, int aWidth, int aHeight) {
                int lWidth = aWidth + aX + 1 < this.ClientSize.Width ? aWidth : this.ClientSize.Width - aX;
                int lRad = aHeight > _ARCRAD ? _ARCRAD : aHeight;
                int lVer = aHeight > (_ARCRAD) ? aHeight - _ARCRAD : 0;
                int lLeftW = aX > (_ARCRAD) ? _ARCRAD : aX;
                int lRightW = (this.ClientSize.Width - (aX + lWidth)) > (_ARCRAD / 2) ? _ARCRAD : this.ClientSize.Width - (aX + lWidth);
        
                GraphicsPath lResult = new GraphicsPath();
                lResult.StartFigure();
                if (lLeftW > 0)
                        lResult.AddArc(aX-_ARCRAD, aY + (aHeight - _ARCRAD), lLeftW, _ARCRAD, 90, -90);
                    else
                        if (lVer > 0)
                                lResult.AddLine(aX, aY + aHeight, 0, _ARCRAD);
                lResult.AddArc(aX, aY, _ARCRAD, _ARCRAD, 180, 90);
                lResult.AddArc(aX + lWidth - _ARCRAD-1, aY, _ARCRAD, _ARCRAD, 270, 90);
                if (lRightW > 0)
                        lResult.AddArc(aX + lWidth-1, aY + (aHeight - _ARCRAD), lRightW-1, _ARCRAD, 180, -90);
                    else
                        if (lVer > 0)
                            lResult.AddLine(aX + lWidth-1, aY + _ARCRAD, aX + lWidth-1, aY + aHeight);
                    lResult.CloseFigure();
                return lResult;
            }
            
        protected GraphicsPath getTabFill(int aX, int aY, int aWidth, int aHeight) {
                return getTabStroke(aX, aY, aWidth, aHeight);
            }
            
        protected GraphicsPath getCloseFill(int aX, int aY, int aWidth, int aHeight) {
                int lRad = aHeight > _ARCRAD ? _ARCRAD : aHeight;
                int lVer = aHeight > (_ARCRAD) ? aHeight - _ARCRAD : 0;
                int lLeftW = aX > (_ARCRAD) ? _ARCRAD : aX;
                int lRightW = (this.ClientSize.Width - aX + aWidth) > (_ARCRAD / 2) ? _ARCRAD : this.ClientSize.Width - aX + aWidth;
        
                GraphicsPath lResult = new GraphicsPath();
                lResult.StartFigure();
                lResult.AddLine(aX + aWidth - _PageHeight, aY + aHeight, aX + aWidth - _PageHeight, aY);
                lResult.AddArc(aX + aWidth - _ARCRAD, aY, _ARCRAD, _ARCRAD, 270, 90);
                if (lRightW > 0)
                        lResult.AddArc(aX + aWidth, aY + (aHeight - _ARCRAD), lRightW, _ARCRAD, 180, -90);
                    else
                        if (lVer > 0)
                            lResult.AddLine(aX + aWidth, aY + _ARCRAD, aX + aWidth, aY + aHeight);
                    lResult.CloseFigure();
                return lResult;
            }
            
        protected GraphicsPath getControlContour(int aX, int aY, TSnapping aSnap) {
                int lWidth = _ARCRAD/2;
                int lHeight = _ARCRAD;
        
                GraphicsPath lResult = new GraphicsPath();
                lResult.StartFigure();
                
                switch (aSnap) {
                        case TSnapping.snapRight:
                                lResult.AddArc(aX, aY, lWidth*2, lHeight, 90, 180);
                            break;
                        case TSnapping.snapLeft:
                                lResult.AddArc(aX, aY, lWidth*2, lHeight, 270, 180);
                            break;
                        default:
                                lResult.AddRectangle(new Rectangle(aX, aY, lWidth, lHeight));
                            break;
                    }
                
                lResult.CloseFigure();
                return lResult;
            }
            
        protected void DrawControls(Graphics aGraph) {
                int lTop = this.ClientSize.Height - _PageHeight + ((_PageHeight - _ARCRAD) / 2);
                Pen lPen = new Pen(Color.Black);
                
                if (!FirstVisible)
                        aGraph.FillPath(_brNoSelected, getControlContour(this.ClientSize.Width - _ARCRAD - 8, lTop, TSnapping.snapRight));
                        
                if (!LastVisible)
                        aGraph.FillPath(_brNoSelected, getControlContour(this.ClientSize.Width - _ARCRAD - 4, lTop, TSnapping.snapLeft));

                switch (_HlCtl) {
                        case THighlightedCtl.ctlPrev:
                                if (!FirstVisible)
                                        aGraph.FillPath(
                                                    new SolidBrush(Color.FromArgb(ControlProperties.HiglightingRate, Color.White)),
                                                    getControlContour(this.ClientSize.Width - _ARCRAD - 8, lTop, TSnapping.snapRight)
                                                );
                            break;
                        case THighlightedCtl.ctlNext:
                                if (!LastVisible)
                                        aGraph.FillPath(
                                                    new SolidBrush(Color.FromArgb(ControlProperties.HiglightingRate, Color.White)),
                                                    getControlContour(this.ClientSize.Width - _ARCRAD - 4, lTop, TSnapping.snapLeft)
                                                );
                            break;
                    }

                if (!FirstVisible)
                        aGraph.DrawPath(new Pen(Color.Black), getControlContour(this.ClientSize.Width - _ARCRAD - 8, lTop, TSnapping.snapRight));
                        
                if (!LastVisible)
                        aGraph.DrawPath(new Pen(Color.Black), getControlContour(this.ClientSize.Width - _ARCRAD - 4, lTop, TSnapping.snapLeft));
            }
            
        protected void DrawPage(Graphics aGraph, CPageSelectorPage aPage) {		
                // #0: calculate positioning and etc.
                int lTabMargin = this.ClientSize.Height - _PageHeight;
                int lLeft = aPage.CurrentLeft;
                int lPageWidth = getVisibleWidth(aPage);
                int lTextAreaWidth = lPageWidth - ((_ARCRAD / 4) + _PageHeight);
                int lVisNo = _Pages.IndexOf(aPage);
                string lPageTitle = aPage.PageTitle;
                Font lDrawFont = this.Font;
                
                SizeF lTextSize = aGraph.MeasureString(lPageTitle, this.Font);
                if (lTextSize.Width > lTextAreaWidth) {
                        while (lTextSize.Width > lTextAreaWidth) {
                                if (lPageTitle.Length > 0)
                                        lPageTitle = lPageTitle.Substring(0, lPageTitle.Length-1);
                                lTextSize = aGraph.MeasureString(lPageTitle + "...", this.Font);
                                if (lPageTitle.Length <= 0)
                                        break;
                            }
                        lPageTitle += "...";
                    }
                
                if (lLeft > (this.ClientSize.Width - _RIGHTMARGIN))
                        return;
            
                // #1: Draw the page graphics
                if (lVisNo == _Selected) {
                        aGraph.FillPath(new SolidBrush(Graph.ColorTools.SetAlpha(128, Color.Black)), this.getTabFill(lLeft + 2, lTabMargin + 2, lPageWidth, _PageHeight));
                        aGraph.FillPath(_brSelected, this.getTabFill(lLeft, lTabMargin, lPageWidth, _PageHeight));
                    } else if (lVisNo == _HighLighted) {
                        aGraph.FillPath(new SolidBrush(Graph.ColorTools.SetAlpha(128, Color.Black)), this.getTabFill(lLeft + 1, lTabMargin + 1, lPageWidth, _PageHeight));
                        aGraph.FillPath(_brNoSelected, this.getTabFill(lLeft, lTabMargin, lPageWidth, _PageHeight));
                        aGraph.FillPath(new SolidBrush(Color.FromArgb(ControlProperties.HiglightingRate, Color.White)), this.getTabFill(lLeft, lTabMargin, lPageWidth, _PageHeight));
                    } else {
                        aGraph.FillPath(new SolidBrush(Graph.ColorTools.SetAlpha(128, Color.Black)), this.getTabFill(lLeft + 1, lTabMargin + 1, lPageWidth, _PageHeight));
                        aGraph.FillPath(_brNoSelected, this.getTabFill(lLeft, lTabMargin, lPageWidth, _PageHeight));
                    }
                
                
                // #2: Draw text
                if (lVisNo == _Selected) {
                        _brText.Color = Color.Black;
                        lDrawFont = new Font(this.Font, FontStyle.Bold);
                        LinearGradientBrush lbrLight = new LinearGradientBrush(
                                    new Rectangle(0, 0, lDrawFont.Height, lDrawFont.Height),
                                    Color.FromArgb(0xaa, 0x00, 0x00, 0x00),
                                    Color.FromArgb(0x00, 0x00, 0x00, 0x00),
                                    0, false
                                );
    
                        aGraph.DrawString(
                                    lPageTitle,
                                    lDrawFont,
                                    lbrLight,
                                    lLeft + _ARCRAD / 2,
                                    lTabMargin + (_PageHeight - lTextSize.Height) / 2
                                );
                        
                        
                    } else
                        _brText.Color = Color.White;
                        
                if (lVisNo != _Selected)
                        aGraph.DrawString(
                                    lPageTitle,
                                    lDrawFont,
                                    new SolidBrush(Graph.ColorTools.SetAlpha(128, Color.Black)),
                                    lLeft + _ARCRAD / 2 + 1,
                                    lTabMargin + (_PageHeight - lTextSize.Height) / 2 + 1
                                );
                
                aGraph.DrawString(
                            lPageTitle,
                            lDrawFont,
                            _brText,
                            lLeft + _ARCRAD / 2,
                            lTabMargin + (_PageHeight - lTextSize.Height) / 2
                        );

                // #3: Draw Closing icon:
                if (aPage.CloseAble) {
                        int lSide = (_PageHeight / 4) * 3;
                        int lMargin = (lSide / 3) * 2;
                                
                        Point lXTopLeft = new Point(
                                    lLeft + lPageWidth - _PageHeight + (_PageHeight - lSide)/2,
                                    lTabMargin + (_PageHeight - lSide)/2
                                );
                                        
                        Point lXTopRight = new Point(
                                    lXTopLeft.X + lSide,
                                    lXTopLeft.Y
                                );
                                
                        Point lXBottomRight = new Point(
                                    lXTopLeft.X + lSide,
                                    lXTopLeft.Y + lSide
                                );
                                        
                        Point lXBottomLeft = new Point(
                                    lXTopLeft.X,
                                    lXTopLeft.Y + lSide
                                );
                                
                        if (_CloseHLighted && _HighLighted == lVisNo) {
                                Draw.RoundedRect(aGraph,
                                            new Rectangle(lXTopLeft, new Size(lSide, lSide)),
                                            16,
                                            null,
                                            _brClose
                                        );
                            }
                
                        _ClosePen.Width = 2;
                        if (lVisNo == _Selected)
                                _ClosePen.Color = Color.Black;
                            else
                                _ClosePen.Color = Color.White;
                                
                        aGraph.DrawLine(
                                    _ClosePen,
                                    lXTopLeft.X + lMargin,
                                    lXTopLeft.Y + lMargin,
                                    lXBottomRight.X - lMargin,
                                    lXBottomRight.Y - lMargin
                                );
                        aGraph.DrawLine(
                                    _ClosePen,
                                    lXTopRight.X - lMargin,
                                    lXTopRight.Y + lMargin,
                                    lXBottomLeft.X + lMargin,
                                    lXBottomLeft.Y - lMargin
                                );
                        

                    }
                    
            }
            
        protected void DrawBorderIcon(Graphics aGraph, int aLeft, int aTop, int aWidth, int aHeight, TBorderIcon aIcon) {
                if (HostForm != null && _MouseDown && borderHelper.IsEqualBorderIcon(_hlBrIcon,aIcon)) {
                        aLeft++;
                        aTop++;
                    }
        
                Pen lPen;
                int lSize = (aWidth < aHeight ? aWidth : aHeight) - 10;
                int lx = (aWidth - lSize) / 2 + aLeft;
                int ly = (aHeight - lSize) / 2 + aTop;
                Color lHlColor = aIcon == TBorderIcon.biClose ? Color.Red : Color.Blue;						
        
                // Background: (If highlighted)
                if (borderHelper.IsEqualBorderIcon(_hlBrIcon,aIcon)) {
                        if (!(_MouseDown && borderHelper.IsEqualBorderIcon(_hlBrIcon,aIcon))) {
                                LinearGradientBrush lBrush = new LinearGradientBrush(
                                            new Point(aLeft, aTop),
                                            new Point(aLeft, aTop+aHeight),
                                            Color.FromArgb(0x80, Graph.ColorTools.Darken(40, lHlColor)),
                                            Color.FromArgb(0x80, lHlColor)
                                        );
                                Draw.RoundedRect(
                                            aGraph,
                                            new Rectangle(aLeft+1, aTop+1, aWidth-1, aHeight-1),
                                            8,
                                            null,
                                            new SolidBrush(Color.FromArgb(0x80, Color.Black))
                                        );
                                Draw.RoundedRect(
                                            aGraph,
                                            new Rectangle(aLeft, aTop, aWidth-1, aHeight-1),
                                            8,
                                            null,
                                            lBrush
                                        );
                            } else {
                                LinearGradientBrush lBrush = new LinearGradientBrush(
                                            new Point(aLeft, aTop),
                                            new Point(aLeft, aTop+aHeight),
                                            Color.FromArgb(0x80, Graph.ColorTools.Darken(40, lHlColor)),
                                            Color.FromArgb(0x80, lHlColor)
                                        );
                                Draw.RoundedRect(
                                            aGraph,
                                            new Rectangle(aLeft, aTop, aWidth-1, aHeight-1),
                                            8,
                                            null,
                                            lBrush
                                        );
                                Draw.RoundedRect(
                                            aGraph,
                                            new Rectangle(aLeft, aTop, aWidth-1, aHeight-1),
                                            8,
                                            null,
                                            new SolidBrush(Color.FromArgb(0x80, Color.Black))
                                        );

                            }
                    }

                        
                // Drawing shape:
                switch (aIcon) {
                        case TBorderIcon.biClose: 
                                lPen = new Pen(Color.FromArgb(0x80, Color.Black), 4);
                                aGraph.DrawLine(lPen, lx+1, ly+1, lx+lSize+1, ly+lSize+1);
                                aGraph.DrawLine(lPen, lx+1, ly+lSize+1, lx+lSize+1, ly+1);
                                lPen = new Pen(Color.White, 4);
                                aGraph.DrawLine(lPen, lx, ly, lx + lSize, ly + lSize);
                                aGraph.DrawLine(lPen, lx, ly+lSize, lx + lSize, ly);
                            break;
                            
                        case TBorderIcon.biMinimize: 
                                lPen = new Pen(Color.FromArgb(0x80, Color.Black), 4);
                                aGraph.DrawLine(lPen, lx+1, ly+lSize-1, lx+lSize+1, ly+lSize-1);
                                lPen = new Pen(Color.White, 4);
                                aGraph.DrawLine(lPen, lx, ly+lSize-2, lx+lSize, ly+lSize-2);
                            break;
                            
                        case TBorderIcon.biMaximize:
                                lPen = new Pen(Color.FromArgb(0x80, Color.Black), 2);
                                aGraph.DrawRectangle(lPen, new Rectangle(lx+1, ly+1, lSize-1, lSize-1));
                                lPen = new Pen(Color.FromArgb(0x80, Color.Black), 4);
                                aGraph.DrawLine(lPen, lx+1, ly, lx+lSize+1, ly);
                                
                                lPen = new Pen(Color.White, 2);
                                aGraph.DrawRectangle(lPen, new Rectangle(lx, ly, lSize-1, lSize-1));
                                lPen = new Pen(Color.White, 4);
                                aGraph.DrawLine(lPen, lx, ly, lx+lSize-1, ly);
                            break;
                            
                        case TBorderIcon.biNormalize:
                                lPen = new Pen(Color.FromArgb(0x80, Color.Black), 2);
                                aGraph.DrawRectangle(lPen, new Rectangle(lx+1, ly+5, lSize-4, lSize-4));
                                aGraph.DrawLine(lPen, lx+3, ly+1, lx+lSize+1, ly+1);
                                aGraph.DrawLine(lPen, lx+lSize+1, ly+1, lx+lSize+1, ly+lSize-2);
                                lPen = new Pen(Color.FromArgb(0x80, Color.Black), 4);
                                aGraph.DrawLine(lPen, lx+1, ly+5, lx+lSize-3, ly+5);
                                
                                lPen = new Pen(Color.White, 2);
                                aGraph.DrawRectangle(lPen, new Rectangle(lx, ly+4, lSize-4, lSize-4));
                                aGraph.DrawLine(lPen, lx+2, ly, lx+lSize, ly);
                                aGraph.DrawLine(lPen, lx+lSize, ly, lx+lSize, ly+lSize-3);
                                lPen = new Pen(Color.White, 4);
                                aGraph.DrawLine(lPen, lx, ly+4, lx+lSize-4, ly+4);
                            break;
                    }
            }
            
        protected void RollLeft() {
                if (!LastVisible) {
                        foreach (object iObj in _Pages)
                                ((CPageSelectorPage)iObj).RequiredLeft -= _PageWidth;
                        _Offset++;
                    }
            }
            
        protected void RollRight() {
                if (!FirstVisible) {
                        foreach (object iObj in _Pages)
                                ((CPageSelectorPage)iObj).RequiredLeft += _PageWidth;
                        _Offset--;
                    }
            }
            
        public void ControlHostForm() {
                if (HostForm != null) {
                        int lWinBorder = 2*libUser32.GetSystemMetrics(libUser32.SM_CXSIZEFRAME);
                        
                        if (Glass) {
                                DwmApi.DwmEnableComposition(true);
                                DwmApi.DWM_BLURBEHIND BlurBehind = new DwmApi.DWM_BLURBEHIND();
                                BlurBehind.dwFlags = DwmApi.DWM_BLURBEHIND.DWM_BB_ENABLE;
                                BlurBehind.fEnable = true;
                                BlurBehind.hRegionBlur = IntPtr.Zero;
                                BlurBehind.fTransitionOnMaximized = false;
    
                                DwmApi.SetRenderingPolicy(HostForm.Handle, DwmApi.TDwmNCRenderingPolicy.Enabled);
                                DwmApi.DwmExtendFrameIntoClientArea(
                                            HostForm.Handle,
                                            new DwmApi.MARGINS(0, this.Height, 0, 0)
                                        );
                                        
                                HostForm.ControlBox = true;
                                
                                
                                
                            } else {
                                HostForm.FormBorderStyle = FormBorderStyle.None;
                                HostForm.MaximizeBox = true;
                                HostForm.MinimizeBox = true;
                                HostForm.ControlBox = true;
                            }
                            
                        libUser32.SetWindowLong(HostForm.Handle, libUser32.GWL_STYLE, libUser32.MY_SIZEABLEBOX);
                        DwmApi.ExtendFrame(HostForm.Handle);
                        
                        FormMenu = new ApplicationMenu(HostForm);
                        FormMenu.AppendItem("Restore");
                        FormMenu.AppendItem("Minimize");
                        FormMenu.AppendItem("Maximize");
                        FormMenu.AppendItem("-");
                        FormMenu.AppendItem("Close");
                        
                        HostForm.MaximumSize = new Size(
                                    lWinBorder + Screen.PrimaryScreen.WorkingArea.Width,
                                    lWinBorder + Screen.PrimaryScreen.WorkingArea.Height
                                );
                    }
                    
                HostFormControlled = true;
            }
        
        public void AddPage(CPageSelectorPage aPage) {
                if (_Pages.IndexOf(aPage) < 0) {
                        SelectedPage = _Pages.Add(aPage);
                        aPage.HostSelector = this;
                        aPage.CurrentWidth = 0;
                        aPage.RequiredWidth = _PageWidth;
                        aPage.RequiredLeft = _Pages.IndexOf(aPage) * _PageWidth + ((_AppIcon == null ? 0 : _AppIcon.Width +  32)) + 2;
                        aPage.CurrentLeft = _Pages.IndexOf(aPage) * _PageWidth + ((_AppIcon == null ? 0 : _AppIcon.Width +  32)) + 2;
                    }
                
                Invalidate();
            }
            
        public void RemovePage(CPageSelectorPage aPage) {
                int lPage = _Pages.IndexOf(aPage);
                if (lPage >= 0) {
                        if (_Selected == lPage)
                            _Selected--;
                        
                        for (int i = lPage; i < _Pages.Count; i++)
                                ((CPageSelectorPage)(_Pages[i])).RequiredLeft -= PageWidth;
                            
                        _Pages.Remove(aPage);
                        aPage.HostSelector = null;
                        this.SelectedPage = 0;
                    }
                Invalidate();
            }
        
        public CPageSelector(): base() {
                _Selected = -1;
                _CurrentControl = null;
        
                _Pages = new ArrayList();
            
                _brNoSelected = new SolidBrush(Color.FromArgb(0xFF, 97, 124, 156));
                _brSelected = new SolidBrush(Color.White);
                                
                _brText = new SolidBrush(Color.Black);
                _brClose = new SolidBrush(Color.FromArgb(64, Color.Red));
                _BorderPen = new Pen(Color.Black);
                _ClosePen = new Pen(Color.White);
                _Timer = new System.Timers.Timer();
                _Timer.Interval = 1000/30;
                _Timer.Enabled = true;
                _Timer.Elapsed += new ElapsedEventHandler(ehTimer);
                
                ResizeRedraw = true;
                DoubleBuffered = true;
            }
        
        protected override void OnDoubleClick(EventArgs e) {
                if (_HighLighted > -1)
                        base.OnDoubleClick(e);
                        
                    else if (HostForm != null) {
                            if (HostForm.WindowState == FormWindowState.Maximized) {
                                    _windowInfo.Assigned = false;
                                    HostForm.WindowState = FormWindowState.Normal;
                                    
                                } else {
                                
                                    MaximizeHostForm();
                                }
                        }
                                
            }		
            
        protected override void OnMouseDown(MouseEventArgs aMEA) {
                _MouseDown = true;
                if (_HighLighted > -1)
                        base.OnMouseDown(aMEA);
                    else {
                            if (HostForm != null && _hlBrIcon == TBorderIcon.biNone) {
                                    _hostForm = HostForm;
                                    startDraggingHost(this.PointToScreen(aMEA.Location));
                                }
                        }
            }
            
        protected override void OnMouseUp(MouseEventArgs aMEA) {
                _MouseDown = false;
                if (_Dragging)
                        _Dragging = false;
                    else
                        base.OnMouseUp(aMEA);
            }
        
        protected override void OnClick(EventArgs aArgs) {
                base.OnClick(aArgs);
                
                if (HostForm != null)
                switch (_hlBrIcon) {
                        case TBorderIcon.biClose: HostForm.Close(); break;
                        case TBorderIcon.biMinimize: HostForm.WindowState = FormWindowState.Minimized; break;
                        case TBorderIcon.biResize:
                                if (HostForm.WindowState == FormWindowState.Maximized) {
                                        _windowInfo.Assigned = false;
                                        HostForm.WindowState = FormWindowState.Normal;
                                    } else {
                                        _windowInfo = new _sFormInfo(this.HostForm);
                                        HostForm.WindowState = FormWindowState.Maximized;
                                    }
                            break;
                    }
                
                if (_HighLighted > -1) {
                        switch (_HlCtl) {
                                case THighlightedCtl.ctlNext: RollLeft(); break;
                                case THighlightedCtl.ctlPrev: RollRight(); break;
                                default:
                                        if (_CloseHLighted && HighlightedPage.CloseAble)
                                                HighlightedPage.ClosePage();
                                            else
                                                SelectedPage = _HighLighted;
                                    break;
                            }
                        
                        Invalidate();
                    }
            }
            
        protected override void OnPaint(PaintEventArgs aPArgs) {
        
                Graphics lGraph = aPArgs.Graphics;
                lGraph.SmoothingMode = SmoothingMode.AntiAlias;
                lGraph.CompositingMode = CompositingMode.SourceOver;
                lGraph.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                Point lBotRight = new Point(this.ClientSize.Width, this.ClientSize.Height-1);
                Pen lPen = new Pen(Color.Black);
                Brush lBrush = new SolidBrush(Color.White);
                lGraph.Clear(Graph.ColorTools.SetAlpha(0, Color.FromArgb(0, 0, 0)));
                
                if (!Glass) {
                        lGraph.SmoothingMode = SmoothingMode.None;
                        Color lBackColor = this.Parent.BackColor;
                        Brush lBgBrush = new SolidBrush(Color.FromArgb(0xff, lBackColor));
                        lGraph.FillRectangle(lBgBrush, 0, 0, this.Width, this.Height);
                        lGraph.SmoothingMode = SmoothingMode.AntiAlias;
                    }
                    
                if (_AppIcon != null && HostFormControlled) {
                        lGraph.DrawIcon(_AppIcon, 12, (this.Height - _AppIcon.Height) / 2);
                        Brush lTextBrush;
                        Font lTitleFont = new Font("Times New Roman", 8, FontStyle.Bold, GraphicsUnit.Point, ((byte)(238)));
                        lTextBrush = new SolidBrush(Graph.ColorTools.SetAlpha(128, Color.Black));
                        
                        
                        if (!Glass) {
                                lGraph.DrawString(
                                            this.Text,
                                            lTitleFont,
                                            lTextBrush,
                                            new Point(
                                                        _AppIcon.Width + 33,
                                                        (int)((((this.Height - _PageHeight) - lGraph.MeasureString(Text, lTitleFont).Height)) / 2) + 1
                                                    )
                                        );
                                lTextBrush = new SolidBrush(Graph.ColorTools.SetAlpha(160, Color.White));
                            } else
                        
                                lTextBrush = new SolidBrush(Color.White);

                        lGraph.DrawString(
                                    this.Text,
                                    lTitleFont,
                                    lTextBrush,
                                    new Point(
                                                _AppIcon.Width + 31,
                                                (int)((((this.Height - _PageHeight) - lGraph.MeasureString(Text, lTitleFont).Height)) / 2)
                                            )
                                );
                    }
                    
                if (HostForm != null && HostFormControlled) {
                        int lBorderIconTop = 0;
                        DrawBorderIcon(lGraph, this.Width - (1 * (_BORDERICON_SIZE*2 + 4)), lBorderIconTop, _BORDERICON_SIZE*2, _BORDERICON_SIZE, TBorderIcon.biClose);
                        DrawBorderIcon(
                                    lGraph,
                                    this.Width - (2 * (_BORDERICON_SIZE*2 + 4)),
                                    lBorderIconTop,
                                    _BORDERICON_SIZE*2,
                                    _BORDERICON_SIZE,
                                    HostForm.WindowState == FormWindowState.Maximized ? TBorderIcon.biNormalize : TBorderIcon.biMaximize
                                );
                        DrawBorderIcon(lGraph, this.Width - (3 * (_BORDERICON_SIZE*2 + 4)), lBorderIconTop, _BORDERICON_SIZE*2, _BORDERICON_SIZE, TBorderIcon.biMinimize);
                    }
                
                for (int i = 0; i < _Pages.Count; i++)
                        if (i != _Selected)
                                DrawPage(lGraph, ((CPageSelectorPage)_Pages[i]));
                                
                if (_Selected >= 0)
                        DrawPage(lGraph, ((CPageSelectorPage)_Pages[_Selected]));
                        
                DrawControls(lGraph);
            }
        
        protected override void OnMouseMove(MouseEventArgs aMArgs) {
                if (!_Dragging) {
                        base.OnMouseMove(aMArgs);
                        UpdateByCursorPos(aMArgs.Location);
                    } else {
                        Point lPoint = this.PointToScreen(aMArgs.Location);
                    
                        if (lPoint.Y == 0) {
                                if (!_windowInfo.Assigned) _windowInfo = new _sFormInfo(HostForm);
                                HostForm.WindowState = FormWindowState.Maximized;
                            } else {
                                if (HostForm.WindowState == FormWindowState.Maximized && lPoint.Y > 24) {
                                        // Now restoring window size and position:
                                        HostForm.WindowState = FormWindowState.Normal;
                                        if (_windowInfo.Assigned) {
                                                _windowInfo.RestoreForm(HostForm, lPoint);
                                                _windowInfo.Assigned = false;
                                            }
                                        startDraggingHost(lPoint);
                                    } else if (!_windowInfo.Assigned) {
                                        updateDragging(lPoint);
                                    }
                            }
                    }
            }
            
        protected override void OnMouseEnter(EventArgs e) {
                base.OnMouseEnter(e);
                UpdateByCursorPos(Control.MousePosition);
            }
            
        protected override void OnMouseLeave(EventArgs e) {
                base.OnMouseLeave(e);
                _hlBrIcon = TBorderIcon.biNone;
                _HighLighted = -1;
                _CloseHLighted = false;
                _HlCtl = THighlightedCtl.ctlPage;
                Invalidate();
            }
            
        protected void ehTimer(object aSender, ElapsedEventArgs aEArgs) {
                if (ModifyFrame() && Visible)
                        Invalidate();
            }

    }
}
