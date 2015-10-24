/*
 *
 * Licensing:			GPL
 * Original project:	bin.csproj
 *
 * Copyright: Adam Halassy (2009.12.01.)
 * 
 * 
 */
 
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Halassy.Controls {
		public class CPasswdEditorItem {
				private CPasswdEditorItems _parent = null;
				private int _IconIndex;
				private string _Service, _Usr, _Passwd;
				
				public CPasswdEditorItems Parent {
						get { return _parent; }
						set {
								if (_parent != value) {
										if (_parent != null) _parent.Remove(this);
										_parent = value;
										if (_parent != null) _parent.Add(this);
									}
							}
					}
				
				private void _refreshEditor() {
						if (_parent != null)
								if (_parent.GUIClass != null)
										_parent.GUIClass.Invalidate();
					}
					
				public int IconIndex {
						get { return _IconIndex; } 
						set {
								_IconIndex = value;
								_refreshEditor();
							}
					}
					
				public string Service {
						get { return _Service; }
						set {
								_Service = value;
								_refreshEditor();
							}
					}
				
				public string UserName {
						get { return _Service; }
						set {
								_Usr = value;
								_refreshEditor();
							}
					}

				public string Password {
						get { return _Passwd; }
						set {
								_Passwd = value;
								_refreshEditor();
							}
					}

				public CPasswdEditorItem(int aIconIndex, string aService, string aUser, string aPassword) {
						_IconIndex = aIconIndex;
						_Service = aService;
						_Usr = aUser;
						_Passwd = aPassword;
					}
			}
			
		public class CPasswdEditorItems {
				private ArrayList _items;
				private CPasswdEditor _editor = null;
				
				private void _refreshEditor() {
						if (_editor != null) _editor.Invalidate();
					}
					
				public event EventHandler Change;
				
				public int Count { get { return _items.Count; }}
				
				public CPasswdEditorItem this[int aIndex] {
						get {
								if (aIndex > -1 && aIndex < _items.Count)
										return (CPasswdEditorItem)(_items[aIndex]);
									else
										return null;
							}
						set {
								if (aIndex > -1 && aIndex < _items.Count)
										_items[aIndex] = value;
							}
					}
				
				public CPasswdEditor GUIClass {
						get { return  _editor; }
						set {
								_editor = value;
								_refreshEditor();
							}
					}
				
				public void Add(CPasswdEditorItem aItem) {
						if (_items.IndexOf(aItem) < 0) {
								_items.Add(aItem);
								aItem.Parent = this;
								_refreshEditor();
							}
						if (Change != null)	 Change(this, null);
					}
					
				public void Remove(CPasswdEditorItem aItem) {
						if (_items.IndexOf(aItem) > -1) {
								aItem.Parent = null;
								_refreshEditor();
						}
						if (Change != null) Change(this, null);
					}
				
				public CPasswdEditorItems() {
						_items = new ArrayList();
					}
					
			}

		public class CPasswdEditor: Control {
				private ImageList _imgList;
				private CPasswdEditorItems _items;
				private int _rowHeight = 48;
				private string _noItemMsg = "NAN";
				private int _highlighted = -1;
				private int _HeaderHeight = 48;
				private int _Offset = 0;
				
				private VScrollBar _ctlScroll;
				
				private void _updateByPoint(Point aPoint) {
						if (aPoint.Y < _HeaderHeight) {
								_highlighted = -1;
							} else {
								_highlighted = (aPoint.Y - _HeaderHeight + _Offset) / _rowHeight;
							}
							
						Invalidate();
					}
					
				private void _updateScroll() {
						if (_items != null && (_items.Count * _rowHeight + _HeaderHeight) > this.ClientRectangle.Height) {
								_ctlScroll.Location = new Point(ClientRectangle.Width - _ctlScroll.Width, 0);
								_ctlScroll.Size = new Size(_ctlScroll.Width, this.ClientRectangle.Height);
										
								_ctlScroll.Minimum = 0;
								_ctlScroll.Maximum = (_items.Count * _rowHeight);
								_ctlScroll.LargeChange = ClientRectangle.Height - _HeaderHeight;
										
								_ctlScroll.Visible = true;
							} else {
								_ctlScroll.Visible = false;
							}
					}
		
				//private string _UsrTitle = "User name";
				//private string _PwdTitle = "Password";
				//private string _SrvTitle = "";
				
				public string MsgNoItems {
						get { return _noItemMsg; }
						set {
								_noItemMsg = value;
								Invalidate();
							}
					}
				
				public int RowHeight {
						get { return _rowHeight; }
						set {
								_rowHeight = value;
								Invalidate();
							}
					}
				
				public int HeaderHeight {
						get { return _HeaderHeight; }
						set {
								_HeaderHeight = value;
								Invalidate();
							}
					}

				public ImageList ItemIcons {
						get { return _imgList; }
						set {
								_imgList = value;
								Invalidate();
							}
					}
					
				public CPasswdEditorItems Items {
						get { return _items; }
						set {
								_items = value;
								if (_items != null) {
										_items.GUIClass = this;
										_items.Change += new EventHandler(ehScrolling);
									}
								_updateScroll();
								Invalidate();
							}
					}
					
				protected override void OnMouseMove(MouseEventArgs aEArgs) {
						base.OnMouseMove(aEArgs);
						_updateByPoint(this.PointToClient(Control.MousePosition));
						_updateScroll();
					}
					
				protected override void OnMouseEnter(System.EventArgs aEArgs) {
						base.OnMouseEnter(aEArgs);
						_updateByPoint(this.PointToClient(Control.MousePosition));
					}
					
				protected override void OnMouseLeave(System.EventArgs e) {
						base.OnMouseLeave(e);
						_highlighted = -1;
						Invalidate();
					}
		
				protected override void OnPaint(PaintEventArgs aPArgs) {
						_updateScroll();
				
						Graphics lGraphics = aPArgs.Graphics;
						lGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
						int lY = _HeaderHeight - _Offset;
						int lTop = 0;
						
						Pen lPen = new Pen(Color.Black, 2);
						Brush lBrush = new SolidBrush(this.BackColor);
						Brush lEvenRow = new SolidBrush(Graph.ColorTools.SetAlpha(80, Color.White));
						Brush lTextBrush = new SolidBrush(Color.Black);
				
						lGraphics.FillRectangle(lBrush, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
					
						if (_items != null && _items.Count > 0) {
								for (int i = 0; i < _items.Count; i++) {
										if (i == _highlighted)
												lGraphics.FillRectangle(lEvenRow, 0, lY, this.ClientRectangle.Width, _rowHeight);
										
										CPasswdEditorItem iItem = _items[i];
								
										if (_imgList != null) {
												Image img = iItem.IconIndex < _imgList.Images.Count ? _imgList.Images[iItem.IconIndex] : _imgList.Images[0];
												
												lTop = (_rowHeight - img.Height) / 2;
												lGraphics.DrawImage(img, 32, lY + lTop);
											}
											
										lTop = (_rowHeight - (int)(lGraphics.MeasureString(iItem.Service, this.Font).Height)) / 2;
								
										lGraphics.DrawString(
												iItem.Service,
												this.Font,
												lTextBrush,
												32 + _imgList.ImageSize.Width + 8,
												lY + lTop
											);
											
										lY += _rowHeight;
									}
								lBrush = new SolidBrush(this.BackColor);
									
								lGraphics.FillRectangle(lBrush, 0, 0, ClientRectangle.Width, _HeaderHeight);
								lGraphics.DrawLine(lPen, 0, _HeaderHeight, this.ClientRectangle.Width, _HeaderHeight);
							} else {
								lGraphics.DrawString(_noItemMsg, this.Font, lTextBrush, 32, 48);
							}
					}
					
				public CPasswdEditor() {
						_imgList = null;
						DoubleBuffered = true;
						
						_ctlScroll = new VScrollBar();
						_ctlScroll.Visible = false;
						_ctlScroll.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right);
						_ctlScroll.ValueChanged += new System.EventHandler(ehScrolling);
						this.Controls.Add(_ctlScroll);
						
						this.Resize += new EventHandler(ehResize);
						this.MouseWheel += new MouseEventHandler(ehMouseWheel);
						this.TabStop = true;
					}
					
				protected void ehMouseWheel(object sender, MouseEventArgs e) {
						MessageBox.Show("Scroll!");
						_ctlScroll.Value += e.Delta;
					}
					
				protected void ehResize(object sender, EventArgs e) { _updateScroll(); Invalidate(); }
					
				protected void ehScrolling(object aSender, EventArgs aEArgs) {
						_Offset = _ctlScroll.Value;			
						Invalidate();
					}
					
				protected void ehItemsChange(object aSender, EventArgs aEARgs) { _updateScroll(); Invalidate(); }
			}
	}