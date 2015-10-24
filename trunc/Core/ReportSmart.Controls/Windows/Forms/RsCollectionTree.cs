#region Source information

//*****************************************************************************
//
//    RsCollectionTree.cs
//    Created by Adam (2015-10-23, 9:21)
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
using System.Windows.Forms;
using System.Xml;

using ReportSmart;
using ReportSmart.Controls;
using ReportSmart.Localization;

using ReportSmart.Documents;
using ReportSmart.Documents.Collections;
using ReportSmart.Windows.Forms;

namespace ReportSmart.Windows.Forms {

	public delegate void NodeSelectedEvent(RsCollectionTree aSender, RsCollectionTreeNode aNode);

	public delegate string NewNodeFolderEventHandler(RsCollectionTreeNode aSender);

	public partial class RsCollectionTree: CSpecialPanel {
			
			#region Static methods:
			
			#endregion
	
	
			#region Properties and fields:
			protected RsCollectionTreeNode selectedNode = null;
			
			internal RsCollectionTreeContextMenu FolderContext;
			
			internal protected int CurrentNodeTabIndex = 0;
			
			public RsCollectionBrowser Owner { get; protected set; }
			
			public int NodeHeight { get; set; }
	
			public int NodeIndent { get; set; }
			
			public Image FolderImage { get; set; }
			
			public RsCollectionTreeNode SelectedNode {
					get { return selectedNode; } 
					internal set {
							if (SelectedNode != null) SelectedNode.Editing = false;
							InvalidateSelectedNode();
							selectedNode = value;
							if (NodeSelected != null)
									NodeSelected(this, selectedNode);
							InvalidateSelectedNode();
						}
				}

			public List<RsCollectionTreeNode> RootNodes { get; protected set; }
				
			public RsCollectionProvider CollectionProvider { get; protected set; }
			
			public string DefaultNodeName { get { return Owner.DefaultFolderName; }}
			#endregion
			
			#region Events
			public event NodeSelectedEvent NodeSelected;
			#endregion
			
			protected void InvalidateSelectedNode() { if (selectedNode != null) selectedNode.Invalidate(); }
		
			protected void RefreshTree() {
					List<RsCollectionFolder> lFolders = CollectionProvider.RootFolder.GetSubFolders();
					
					foreach (RsCollectionFolder iFolder in lFolders)
							RsCollectionTreeNode.CreateNode(iFolder, null, this);
				}
			
			protected int SortNodes(RsCollectionTreeNode aNode, int aTop) {
					int lOffset = 0;
			
					if (aNode != null && aNode.Children != null)
							foreach (RsCollectionTreeNode iNode in aNode.Children) {							
									iNode.Location = new Point(this.BodyLeft, aTop + lOffset);
									iNode.Size = new Size(this.BodyWidth, iNode.Editing ? 2*NodeHeight : NodeHeight);
									iNode.TabIndex = CurrentNodeTabIndex;
									CurrentNodeTabIndex++;
									lOffset += iNode.Height;
									
									if (iNode.HasChildNodes()) {
											lOffset += SortNodes(iNode, aTop + lOffset);
										}
								}
								
					return lOffset;
				}
				
			internal void SortNodes() {
					CurrentNodeTabIndex = 0;
			
					int lTop = this.BodyTop + NodeHeight / 2;
					foreach (RsCollectionTreeNode iNode in RootNodes) {
							iNode.Location = new Point(this.BodyLeft, lTop);
							iNode.Size = new Size(this.BodyWidth, iNode.Editing ? 2*NodeHeight : NodeHeight);
							iNode.TabIndex = CurrentNodeTabIndex;
							CurrentNodeTabIndex++;
							lTop += iNode.Height;
							
						
							if (iNode.HasChildNodes())
									lTop += SortNodes(iNode, lTop);
						}
			
					
					this.DetermineHeight();
				}
				
			internal void DeselectNode() {
					InvalidateSelectedNode();
					selectedNode = null;
					InvalidateSelectedNode();
				}
			
			public void RefreshPageTitle() {
					PageTitle = CollectionProvider.GetCollectionType() == RsCollectionProviderType.Favorites ?
							Owner.FavoritesCollectionName :
							CollectionProvider.CollectionName;
				}
			
			public override void AssignLocale(CLocalization aLocale) {
					base.AssignLocale(aLocale);
					
					Locale.AddLocalizedControl(FolderContext);
					
					foreach (Control iControl in this.Controls)
							if (iControl is RsCollectionTreeNode)
									aLocale.AddLocalizedControl((RsCollectionTreeNode)iControl);
				}
					
			public void AddNode(RsCollectionTreeNode aNode) {
					aNode.ContextMenu = FolderContext;
					
					if (Locale != null)
							Locale.AddLocalizedControl(aNode);
			
					if (aNode.ParentNode == null)
							this.RootNodes.Add(aNode);
							
					this.Controls.Add(aNode);
					
					SortNodes();
				}
				
			public void RemoveNode(RsCollectionTreeNode aNode) {
					this.RootNodes.Remove(aNode);
					this.Controls.Remove(aNode);
					SortNodes();
				}
				
			public RsCollectionTree(RsCollectionBrowser aOwner, RsCollectionProvider aProvider) {
					Size = new Size(128, 512);
					ResizeRedraw = true;
					DoubleBuffered = true;
					Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
					Margin = new Padding(16);
					
					NodeHeight = 32;
					NodeIndent = 24;
			
					RootNodes = new List<RsCollectionTreeNode>();
					Owner = aOwner;
					CollectionProvider = aProvider;
					
					RefreshPageTitle();
					
					FolderContext = new RsCollectionTree.RsCollectionTreeContextMenu(this);
					
					RefreshTree();
				}
		}
}
