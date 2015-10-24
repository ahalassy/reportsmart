#region Source information

//*****************************************************************************
//
//    RSSendMailWizard.cs
//    Created by Adam (2009-09-17, 8:57)
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
using System.Drawing;
using System.Windows.Forms;

using ReportSmart;
using ReportSmart.Controls;
using ReportSmart.Forms;
using ReportSmart.Localization;
using ReportSmart.Network.Email;

using ReportSmart.Application;

namespace ReportSmart.Controls {
		internal class CdlgSendMail: CWizardDialog {
		
				private class CSetupExport: CWizardPanel {
						private ImageList _DocIcons;
						private TreeView _ctlSelector;
						private TreeNode _nPDF, _nXLS, _nDOC, _nXML, _nHTM;
						private bool _PDF = true, _XLS = false, _DOC = false, _XML = false, _HTM = false;
						
						public bool DOC { get { _SyncGUI(); return _DOC; }}
						public bool HTM { get { _SyncGUI(); return _HTM; }}
						public bool PDF { get { _SyncGUI(); return _PDF; }}
						public bool XLS { get { _SyncGUI(); return _XLS; }}
						public bool XML { get { _SyncGUI(); return _XML; }}

						public override string GetInstanceName() { return "CSetupExport"; }
						
						public override void ResetPage() {
								_nPDF.Checked = true;
								_nXLS.Checked = false;
								_nDOC.Checked = false;
								_nXML.Checked = false;
								_nHTM.Checked = false;
								
								_SyncGUI();
							}
							
						public override void ApplyLocale(CLocalization aLocale) {
								base.ApplyLocale(aLocale);
								
								_nDOC.Text = aLocale.GetTagText("fileDOC");
								_nHTM.Text = aLocale.GetTagText("fileHTM");
								_nPDF.Text = aLocale.GetTagText("filePDF");
								_nXLS.Text = aLocale.GetTagText("fileXLS");
								_nXML.Text = aLocale.GetTagText("fileXML");
								
							}
							
						private void _SyncGUI() {
								_DOC = _nDOC.Checked;
								_PDF = _nPDF.Checked;
								_XLS = _nXLS.Checked;
								_XML = _nXML.Checked;
								_HTM = _nHTM.Checked;
							}
							
						private bool _getValByNode(TreeNode aNode) {
								     if (aNode == _nPDF) return _PDF;
								else if (aNode == _nDOC) return _DOC;
								else if (aNode == _nXLS) return _XLS;
								else if (aNode == _nXML) return _XML;
								else if (aNode == _nHTM) return _HTM;
								else return false;
								
							}
							
						public CSetupExport(): base() {
								this.Name = "pExport";
								this.AllwaysOk = true;
						
								_DocIcons = new ImageList();
								_DocIcons.ImageSize = new Size(32, 32);
								_DocIcons.ColorDepth = ColorDepth.Depth32Bit;
								_DocIcons.Images.Add((Bitmap)RsViewEngine.Resources.GetObject("icon_pdf_32x32"));
								_DocIcons.Images.Add((Bitmap)RsViewEngine.Resources.GetObject("icon_html_32x32"));
								_DocIcons.Images.Add((Bitmap)RsViewEngine.Resources.GetObject("icon_excel_32x32"));
								_DocIcons.Images.Add((Bitmap)RsViewEngine.Resources.GetObject("icon_word_32x32"));
								_DocIcons.Images.Add((Bitmap)RsViewEngine.Resources.GetObject("icon_xml_32x32"));
								
								_ctlSelector = new TreeView();
								_ctlSelector.Location = new Point(
											lDescription.Left + ControlProperties.ControlSpacing*2,
											lDescription.Bottom + ControlProperties.ControlSpacing
										);
								_ctlSelector.Size = new Size(
											lDescription.Width - ControlProperties.ControlSpacing*4,
											40*8
										);
								_ctlSelector.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);
								_ctlSelector.ImageList = _DocIcons;
								_ctlSelector.ItemHeight = 38;
								_ctlSelector.CheckBoxes = true;
								_ctlSelector.FullRowSelect = true;
								_ctlSelector.ShowLines = false;
								_ctlSelector.Indent = 0;
								
								_nPDF = _ctlSelector.Nodes.Add("PDF");
								_nXLS = _ctlSelector.Nodes.Add("XLS");
								_nDOC = _ctlSelector.Nodes.Add("DOC");
								_nXML = _ctlSelector.Nodes.Add("XML");
								//_nHTM = _ctlSelector.Nodes.Add("HTM");
								_nHTM = new TreeNode();
								
								_nPDF.ImageIndex = 0; _nPDF.SelectedImageIndex = 0;
								_nHTM.ImageIndex = 1; _nHTM.SelectedImageIndex = 1;
								_nXLS.ImageIndex = 2; _nXLS.SelectedImageIndex = 2;
								_nDOC.ImageIndex = 3; _nDOC.SelectedImageIndex = 3;
								_nXML.ImageIndex = 4; _nXML.SelectedImageIndex = 4;
							
								_ctlSelector.NodeMouseClick += new TreeNodeMouseClickEventHandler(EH_NodeClick);
								
								this.Controls.Add(_ctlSelector);
							}
							
						void EH_NodeClick(object aSender, TreeNodeMouseClickEventArgs aNArgs) {
								if (aNArgs.Node != null) {
										if (_getValByNode(aNArgs.Node) == aNArgs.Node.Checked)
												aNArgs.Node.Checked = !aNArgs.Node.Checked;
										_SyncGUI();
									}
							}
							
					}
			
				private class CRecipients: CWizardPanel {
				
						public override string GetInstanceName() { return "CRecipients"; }
						public override void ResetPage() {
								throw new NotImplementedException();
							}
					}
			
				private class CCheck: CWizardPanel {
				
						public override string GetInstanceName() { return "CCheck"; }
						public override void ResetPage() {
								throw new NotImplementedException();
							}
					
					}
					

					
				public bool DOC { get { return _pExport.DOC; }}
				public bool HTM { get { return _pExport.HTM; }}
				public bool PDF { get { return _pExport.PDF; }}
				public bool XLS { get { return _pExport.XLS; }}
				public bool XML { get { return _pExport.XML; }}
				
				private CSetupExport _pExport;
			
				public CdlgSendMail(): base() {
						this.Name = "dlgSendMail";
						this.SuspendLayout();
						
						this.AddPage(_pExport = new CSetupExport());
						
						this.ResumeLayout(true);
					}
			}
	}