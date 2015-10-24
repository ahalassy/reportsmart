/*
 * Licensing:			GPL
 * Original project:	RsControls.csproj
 *
 * Copyright: Adam ReportSmart (2010.12.10.)
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
using ReportSmart.Graph;
using ReportSmart.Graph.Drawing;
using ReportSmart.Localization;

using ReportSmart;
using ReportSmart.Documents;
using ReportSmart.Documents.Collections;

namespace ReportSmart.Windows.Forms {
		/// <summary>
		/// Description of RsCollectionTreeNode.
		/// </summary>
		public class RsCollectionTreeNode: Control, ILocalizedControl {
				
				#region Static methods:
				public static RsCollectionTreeNode CreateNode(
							RsCollectionItem aItem,
							RsCollectionTreeNode aParentNode,
							RsCollectionTree aOwner
						) {
						
						RsCollectionTreeNode lResult = new RsCollectionTreeNode();
						lResult.IntegrateNode(aItem, aParentNode, aOwner);
						return lResult;
					}
				#endregion
		
				
				protected TextBox eFolderName;
				
				protected Button bApply, bCancel;
				
				internal bool DeleteOnCancel = false;
				
				#region Properties:
				public List<RsCollectionTreeNode> Children { get; protected set; }
				
				public bool Expanded { get; protected set; }
				
				public RsCollectionTreeNode ParentNode { get; protected set; }
				
				public RsCollectionTree Owner { get; protected set; }
				
				public string NodePath {
						get {
								string lResult = this.CollectionItem.ItemName;
						
								RsCollectionTreeNode lNode = this.ParentNode;
								while (lNode != null) {
										lResult = lNode.CollectionItem.ItemName + "." + lResult;
										lNode = lNode.ParentNode;
									}
									
								return lResult;
							}
					}
				
				public int NodeLevel {
						get {
								RsCollectionTreeNode lNode = this;
								int lResult = 0;
								while (lNode.ParentNode != null) {
										lResult ++;
										lNode = lNode.ParentNode;										
									}
								return lResult;
							}
					}
					
				public RsCollectionItem CollectionItem { get; protected set; }
				
				public CLocalization Localization { get; protected set; }
				
				public bool Editing {
						get { return eFolderName.Visible; }
						set {
								if (value) {
										eFolderName.Location = new Point(
													GetTextIndent(),
													(Owner.NodeHeight - eFolderName.Height) / 2
												);
												
										bCancel.Location = new Point(
													this.Width - bCancel.Width - Owner.NodeIndent,
													Owner.NodeHeight + (Owner.NodeHeight - bCancel.Height) / 2
												);
												
										bApply.Location = new Point(
													bCancel.Left - bApply.Width - 8,
													Owner.NodeHeight + (Owner.NodeHeight - bApply.Height) / 2
												);
												
										eFolderName.Size = new Size(
													this.Width - eFolderName.Left - Owner.NodeIndent,
													eFolderName.Height
												);
										eFolderName.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
										eFolderName.Text = CollectionItem.ItemName;
									}
									
								eFolderName.Visible = value;
								bApply.Visible = value;
								bCancel.Visible = value;
								if (eFolderName.Visible) eFolderName.Focus();
								Owner.SortNodes();
								Invalidate();
							}
					}
				#endregion
				
				#region MethodS:
				public bool HasChildNodes() {
						return Children != null && Children.Count > 0;
					}
				
				public void AssignLocale(CLocalization aSender) {
						Localization =aSender;
					}
					
				public void ApplyLocale(CLocalization aSender) {
						bCancel.Text = aSender.GetTagText("cancel");
						bApply.Text = aSender.GetTagText("apply");
						
					}
				
				public string GetInstanceName() { return "RsCollectionTreeNode"; }
				
				public void ReleaseLocale() { Localization = null; }
				
				protected int GetTextIndent() {
						return Owner.NodeIndent * (NodeLevel + 1) + 
								(Owner.FolderImage == null ? 0 : Owner.FolderImage.Width) +
								Owner.NodeIndent;
					}
					
				protected override void OnGotFocus(EventArgs e) {
						base.OnGotFocus(e);
						
						Owner.SelectedNode = this;
						Owner.Invalidate();
					}
					
				protected override void OnPaint(PaintEventArgs aE) {
						int lLeft = (NodeLevel + 1) * Owner.NodeIndent +
									(Owner.FolderImage == null ? 0 : Owner.FolderImage.Width) +
									Owner.NodeIndent;
				
						aE.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						aE.Graphics.CompositingQuality = CompositingQuality.HighQuality;
						Font lFont = Owner.SelectedNode == this ?
								new Font(Font, FontStyle.Bold) :
								new Font(Font, FontStyle.Regular);
						
						SizeF lTextSize = aE.Graphics.MeasureString(CollectionItem.ItemName, Font);
						
						int lBarLeft = Owner.FolderImage == null ? lLeft : lLeft - (Owner.NodeIndent + Owner.FolderImage.Width);
						if (Owner.SelectedNode == this) {
								Draw.RoundedRect(
											aE.Graphics,
											new Rectangle(lBarLeft-4, 0, (Width-lBarLeft-1), this.Height-1),
											8,
											null,
											new SolidBrush(Color.FromArgb(60, Owner.HeaderColor))
										);
							}

						if (Owner.FolderImage != null) {
								aE.Graphics.DrawImage(
											Owner.FolderImage,
											new Rectangle(
														lLeft - (Owner.NodeIndent + Owner.FolderImage.Width),
														(Owner.NodeHeight -Owner.FolderImage.Height) / 2,
														Owner.FolderImage.Width,
														Owner.FolderImage.Height
													)
										);
							}
				
						if (!Editing)
								aE.Graphics.DrawString(
											CollectionItem.ItemName,
											lFont,
											new SolidBrush(Color.Black),
											new Point(
														lLeft,
														(Owner.NodeHeight - (int)(lTextSize.Height)) / 2
													)
										);
								
					}
		
				public void AddChild(string aChildName)	{
						Expand();
				
						if (CollectionItem is RsCollectionFolder) {
								RsCollectionFolder lFolder =
											((RsCollectionFolder)CollectionItem).AddSubFolder(
														((RsCollectionFolder)CollectionItem).GetNextName(aChildName)
													);
													
								RsCollectionTreeNode lNode = RsCollectionTreeNode.CreateNode(lFolder, this, Owner);
								lNode.DeleteOnCancel = true;
								lNode.Editing = true;
							}
					}
		
				public void Delete() {
						RsReportCollection lCollection = CollectionItem.ReportItem.Collection;
						CollectionItem.ReportItem.Release();
						lCollection.QuickSave();
						Release();
					}
		
				public void Release() {
						if (Owner.SelectedNode == this)
								Owner.SelectedNode = null;
				
						RsCollectionTreeNode[] lNodes = Children.ToArray();
						for (int i = 0; i < lNodes.Length; i++) {
								//Owner.RemoveNode(lNodes[i]);
								lNodes[i].Release();
							}
				
						Owner.RemoveNode(this);
						ParentNode.Children.Remove(this);
						
						Owner.SortNodes();
					}
		
				public void Collapse() {
						if (Children != null) {
								RsCollectionTreeNode[] lNodes = Children.ToArray();
								
								for (int i = 0; i < lNodes.Length; i++)
										lNodes[i].Release();
						
								Children.Clear();
								Children = new List<RsCollectionTreeNode>();
							}
								
						Expanded = false;
						Owner.SortNodes();
					}
					
				public void Expand() {
						if (Expanded) return;
						Children = new List<RsCollectionTreeNode>();
				
						if (CollectionItem is RsCollectionFolder) {
								RsCollectionFolder lFolder = (RsCollectionFolder)CollectionItem;
								List<RsCollectionFolder> lChildren = lFolder.GetSubFolders();
								
								foreach (RsCollectionItem iItem in lChildren) {
										RsCollectionTreeNode.CreateNode(iItem, this, Owner);
									}
							}
						
						Expanded = true;
					}
				
				public void IntegrateNode(
							RsCollectionItem aItem,
							RsCollectionTreeNode aParentNode,
							RsCollectionTree aOwner
						) {
						
						ParentNode = aParentNode;
						Owner = aOwner;
						CollectionItem = aItem;
						
						if (ParentNode != null)
								ParentNode.Children.Add(this);
						
						Owner.AddNode(this);
					}
				
				#endregion
				
				#region Constructor:
				public RsCollectionTreeNode(): base() {
						ResizeRedraw = true;
						DoubleBuffered = true;
						
						Expanded = false;
						Children = new List<RsCollectionTreeNode>();
						
						Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
						
						#region Construct controls:
						this.TabStop = true;
						
						eFolderName = new TextBox();
						eFolderName.Visible = false;
						eFolderName.TabStop = true;
						eFolderName.TabIndex = 0;
						eFolderName.Anchor = (AnchorStyles)(AnchorStyles.Top |AnchorStyles.Left | AnchorStyles.Right);
						
						bApply = new Button();
						bApply.Size = new Size(72, 24);
						bApply.Text = "_apply";
						bApply.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Right);
						bApply.BackColor = SystemColors.ButtonFace;
						bApply.UseVisualStyleBackColor = true;
						bApply.Visible = false;
						bApply.TabStop = true;
						bApply.TabIndex = 1;
						
						bCancel = new Button();
						bCancel.Size = new Size(72, 24);
						bCancel.Text = "_cancel";
						bCancel.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Right);
						bCancel.BackColor = SystemColors.ButtonFace;
						bCancel.UseVisualStyleBackColor = true;
						bCancel.Visible = false;
						bCancel.TabStop = true;
						bCancel.TabIndex = 2;
						
						this.Controls.Add(eFolderName);						
						this.Controls.Add(bApply);
						this.Controls.Add(bCancel);
						#endregion
						
						
						this.Click += new EventHandler(ehClick);
						this.DoubleClick += new EventHandler(ehClick);
						
						bApply.Click += new EventHandler(ehApplyClick);
						bCancel.Click += new EventHandler(ehCancelClick);
					} 
				#endregion
				
				#region Event handlers:
				protected void ehClick(object aSender, EventArgs aE) {
						if (!Owner.FolderContext.IsMenuActive)
								if (Expanded) Collapse();	else Expand();
						
						Owner.SelectedNode = this;
						Owner.Invalidate();

					}
				
				protected void ehCancelClick(object aSender, EventArgs aE) {
						this.Editing = false;
						
						if (DeleteOnCancel)
								this.Delete();
					}
					
				protected void ehApplyClick(object aSender, EventArgs aE) {
						this.CollectionItem.ItemName = eFolderName.Text;
						this.Editing = false;
						
					}
				#endregion
			}
}
