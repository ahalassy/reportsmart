/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam Halassy (2010.06.04.)
 * 
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
 
using CrystalDecisions.Shared;

using Halassy.Localization;
using Halassy.Graph;

using ReportSmart;
using ReportSmart.Application;

namespace ReportSmart.Controls {
		internal enum RsParameterEditingStyle {
				Plain,
				Alias,
				Explained
			}

		internal class CRSParameterBox {
				private Label _lParam;
				private ComboBox _eParam;
				private Control _parent;
				
				public ParameterField ParameterField { get; protected set; }
				
				public Point Location {
						get { return _eParam.Location; }
						set {
								_eParam.Location = value;
								_lParam.Location = new Point(_eParam.Left, _eParam.Top - 24);
							}
					}
				
				public Size Size {
						get { return _eParam.Size; }
						set {
								_eParam.Size = value;
								_lParam.Size = new Size(_eParam.Width, 24);
							}
					}
				
				public int Left { get { return _eParam.Left; }}
				public int Top { get { return _eParam.Top; }}
				
				public string ParamName { get { return ParameterField.Name; }}
				
				public string Caption {
						get { return _lParam.Text; }
						set { _lParam.Text = value; }
					}
					
				public string Text {
						get { return _eParam.Text; }
						set { _eParam.Text = value; }
					}
				
				public Control Parent {
						get { return _parent; }
						set {
								_parent = value;
								_parent.Controls.AddRange(new Control[2] {_lParam, _eParam});
							}
					}
				
				public Font EditFont {
						get { return _eParam.Font; }
						set {_eParam.Font = value; }
					}
				
				public CRSParameterBox(ParameterField aField) {
						ParameterField = aField;
				
						_eParam = new ComboBox();
						foreach (ParameterValue iValue in aField.DefaultValues) {
								if (iValue.Kind == DiscreteOrRangeKind.DiscreteValue) {
										ParameterDiscreteValue lValue = (ParameterDiscreteValue)iValue;
										_eParam.Items.Add(lValue.Value.ToString());
										//aField.
									}
							}
							
						foreach (ParameterValue iValue in aField.CurrentValues) {
								if (iValue.Kind == DiscreteOrRangeKind.DiscreteValue) {
										ParameterDiscreteValue lValue = (ParameterDiscreteValue)iValue;
										_eParam.Text = lValue.Value.ToString();
										break;
									}
							}
						
						_lParam = new Label();
						
						_lParam.BackColor = Color.Transparent;
						_eParam.TabStop = true;
					}
			}

		internal class CRSParamEditor: Panel, ILocalizedControl {		
				private ParameterFields _paramFields;
				private Button _bOk, _bClear;
				private int _hMargin = 16, _vMargin = 32, _vHeaderSize = 64, _fieldHeight = 64;
				private int _preferredWidth = 256;
				private Color _headerColor;
				private CLocalization _locale;
				private string _Title;
				private int _waBottom {
						get {
								int lwaSize = _paramFields == null ? 0 : ColumnCount == 0 ? 0 : ((_paramFields.Count-1) / ColumnCount + 1) * _fieldHeight;
								return lwaSize + _vMargin * 3 + _vHeaderSize;
							}
					}
				private int _waTop { get { return _vMargin*2 + _vHeaderSize; }}
				private int _waHeight { get { return _waBottom - _waTop; }}
				private List<CRSParameterBox> _paramControls;
					
				public int ParameterCount { get { return _paramFields == null ? 0 : _paramFields.Count; }}
				public int ColumnCount { get { return (this.Width - _hMargin) / (_preferredWidth + _hMargin); }}
				protected int UsedColumnCount { get { return ColumnCount > ParameterCount ? ParameterCount : ColumnCount; }}
				protected int columnWidth {	get { return ColumnCount == 0 ? _preferredWidth : (this.Width - _hMargin) / ColumnCount -_hMargin; }}
					
				public event EventHandler AcceptParams;
				
				public Color HeaderColor {
						get { return _headerColor; }
						set {
								_headerColor = value;
								this.Invalidate();
							}
					}
					
				protected void updatePositions() {
						this.SuspendLayout();
						this.Controls.Clear();
						_bOk.Location = new Point(this.ClientRectangle.Width - ((128+16)*2) - _hMargin*2, _waBottom + (_vHeaderSize - _bOk.Height) / 2);
						_bClear.Location = new Point(this.ClientRectangle.Width - ((128+16)*1) - _hMargin*2, _waBottom + (_vHeaderSize - _bClear.Height) / 2);
						
						if (_paramControls.Count != 0)						
								for (int i=0; i < _paramControls.Count; i++) {
										int xPos = UsedColumnCount == 0 ? 0 : i % UsedColumnCount;
										int yPos = UsedColumnCount == 0 ? 0 : i / UsedColumnCount;
						
										CRSParameterBox iCtl = _paramControls[i];
						
										iCtl.Location = new Point(
													_hMargin * 2 + xPos * (columnWidth + _hMargin),
													_waTop + _vMargin + yPos*_fieldHeight
												);
										iCtl.Size = new Size (
													columnWidth - _vMargin*2,
													24
												);
										
										iCtl.Parent = this;
									}
						
						
						this.Controls.Add(_bOk);
						this.Controls.Add(_bClear);
					}
				
				protected void updateParamFields() {
						if (_paramControls == null)
								_paramControls = new List<CRSParameterBox>();
							else
								_paramControls.Clear();
						
						foreach (ParameterField iParam in _paramFields) {
								CRSParameterBox lnParamBox = new CRSParameterBox(iParam);
								
								lnParamBox.Caption = iParam.PromptText;
								//lnParamBox.Text = iParam.DefaultValues[0].ToString();
								lnParamBox.EditFont = RsViewEngine.EditFont;
								
								_paramControls.Add(lnParamBox);
							}
						
						updatePositions();
					}
				
				public ParameterFields ParamFields {
						set {
								_paramFields = value;
								updateParamFields();
							}
					}
					
				protected override void OnPaint(PaintEventArgs aPArgs) {
						Graphics lGraph = aPArgs.Graphics;
						lGraph.SmoothingMode = SmoothingMode.AntiAlias;
						lGraph.FillRectangle(new SolidBrush(Color.White), this.ClientRectangle);
						
						Brush lBrush = new LinearGradientBrush(
									new Point(0, _vMargin),
									new Point(0, _vMargin + _vHeaderSize),
									ColorTools.Darken(50, _headerColor),
									_headerColor
								);
						Halassy.Graph.Drawing.Draw.RoundedRect(
								lGraph,
								new Rectangle(_hMargin+1, _vMargin+1, this.Width - _vMargin+1, _vHeaderSize+1),
								15,
								null,
								new SolidBrush(Color.FromArgb(0xa0, 0, 0, 0))
							);
						Halassy.Graph.Drawing.Draw.RoundedRect(
								lGraph,
								new Rectangle(_hMargin, _vMargin, this.Width - _vMargin, _vHeaderSize),
								15,
								null,
								lBrush
							);
							
						lBrush = new LinearGradientBrush(
									new Point(0, _waBottom),
									new Point(0, _waBottom + _vHeaderSize),
									ColorTools.Darken(50, _headerColor),
									_headerColor
								);
						Halassy.Graph.Drawing.Draw.RoundedRect(
								lGraph,
								new Rectangle(_hMargin+1, _waBottom+1, this.Width - _vMargin+1, _vHeaderSize+1),
								15,
								null,
								new SolidBrush(Color.FromArgb(0xa0, 0, 0, 0))
							);
						Halassy.Graph.Drawing.Draw.RoundedRect(
								lGraph,
								new Rectangle(_hMargin, _waBottom, this.Width - _vMargin, _vHeaderSize),
								15,
								null,
								lBrush
							);
													
						Font lFont = new Font(RsViewEngine.DefaultFont.FontFamily, 14, FontStyle.Bold, GraphicsUnit.Point);
						int lTextHeight = (int)(lGraph.MeasureString(_Title, lFont).Height);
						lGraph.DrawString(
									_Title,
									lFont,
									new SolidBrush(Color.White),
									new Point(_hMargin * 2, _hMargin * 2 + (_vHeaderSize - lTextHeight)/2)
								);
								
						// Drawing columns:
						for (int i = 0; i < UsedColumnCount; i++) {
								Halassy.Graph.Drawing.Draw.RoundedRect(
										lGraph,
										new Rectangle(_hMargin + (columnWidth + _hMargin)*i, _waTop - (_vMargin/2), columnWidth, _waHeight),
										15,
										null,
										new SolidBrush(Color.FromArgb(0x80, _headerColor))
									);
								
							}
					}
					
				protected override void OnResize(EventArgs eventargs) {
						base.OnResize(eventargs);
						updatePositions();
					}
					
				public void AssignLocale(CLocalization aSender) { _locale = aSender; }
					
				public void ApplyLocale(CLocalization aSender) {
						_bOk.Text = aSender.GetTagText("ok");
						_bClear.Text = aSender.GetTagText("clear");
						_Title = aSender.GetTagText("paramEdit");
					}
					
				public void ReleaseLocale() { _locale = null; }
					
				public string GetInstanceName() { return this.Name;	}
				
				public CRSParamEditor() {
						_headerColor = Color.FromArgb(0xFF, 97, 124, 156);
						ResizeRedraw = true;
						DoubleBuffered = true;
				
						_bOk = new Button();
						_bOk.BackColor = Color.Transparent;
						_bOk.Size = new Size(128, 32);
						_bOk.Text = "_OK";
						_bOk.Anchor = (AnchorStyles)(AnchorStyles.Right | AnchorStyles.Bottom);
						_bOk.BackColor = SystemColors.ButtonFace;
						_bOk.Click += ehOk;
						
						_bClear = new Button();
						_bClear.BackColor = Color.Transparent;
						_bClear.Size = new Size(128, 32);
						_bClear.Text = "_CLEAR";
						_bClear.Anchor = (AnchorStyles)(AnchorStyles.Right | AnchorStyles.Bottom);
						_bClear.BackColor = SystemColors.ButtonFace;
						
						RsViewEngine.Locale.AddLocalizedControl(this);
						this.ApplyLocale(RsViewEngine.Locale);
						_paramControls = new List<CRSParameterBox>();
					}				
					
				protected void ehOk(object aSender, EventArgs aEArgs) {
						foreach (CRSParameterBox iCtl in _paramControls) {
								ParameterValues lPValues = _paramFields[iCtl.ParamName].CurrentValues;
								ParameterDiscreteValue lNewVal = new ParameterDiscreteValue();
								lNewVal.Value = iCtl.Text;
								lPValues.Clear();
								lPValues.Add(lNewVal);
							}
						
						AcceptParams(this, aEArgs);
					}
			}
	}