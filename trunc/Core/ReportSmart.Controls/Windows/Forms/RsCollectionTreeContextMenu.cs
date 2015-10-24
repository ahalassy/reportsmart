#region Source information

//*****************************************************************************
//
//    RsCollectionTreeContextMenu.cs
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
using System.Windows.Forms;
using System.Xml;
using ReportSmart.Localization;

namespace ReportSmart.Windows.Forms
{
    /// <summary>
    /// Description of RsCollectionTreeContextMenu.
    /// </summary>
    /// 
    public partial class RsCollectionTree {
				internal class RsCollectionTreeContextMenuItem: MenuItem {
				
						public RsCollectionTreeContextMenu Owner { get; protected set; }
						
				
						protected override void OnClick(EventArgs e) {
								try {
										base.OnClick(e);
										
									} finally {
										Owner.IsMenuActive = false;
										
									}
							}
							
						public RsCollectionTreeContextMenuItem(
									string aText,
									RsCollectionTreeContextMenu aOwner
								): base(aText) {
								Owner = aOwner;
							}
					}
		
				internal class RsCollectionTreeContextMenu: ContextMenu, ILocalizedControl {
				
						internal bool IsMenuActive;
						
						public CLocalization Localization { get; protected set; }
						
						public RsCollectionTree Owner { get; protected set; }
						
						public RsCollectionTreeNode SourceNode {
								get {
										return (SourceControl is RsCollectionTreeNode) ?
												(RsCollectionTreeNode)SourceControl : null;
									}
							}
							
						protected override void OnPopup(EventArgs e) {
								IsMenuActive = true;
						
								base.OnPopup(e);
							}
				
						public void AssignLocale(CLocalization aSender) { Localization = aSender; }
						
						public void ApplyLocale(CLocalization aSender) {
								XmlNode lMenuNode = aSender.GetMenuTexts("collectionFolderContext");
								
								if (lMenuNode != null) {
										ItemNewFolder.Text = XmlTools.GetNode(lMenuNode, "addFolder").InnerText;
										ItemRenameFolder.Text = XmlTools.GetNode(lMenuNode, "renameFolder").InnerText;
										ItemRemoveFolder.Text = XmlTools.GetNode(lMenuNode, "deleteFolder").InnerText;
									}
							}
						
						public void ReleaseLocale() { throw new NotImplementedException(); }
						
						public string GetInstanceName() { return "RsCollectionTreeContextMenu"; }
				
						internal RsCollectionTreeContextMenuItem ItemNewFolder { get; private set; }
						internal RsCollectionTreeContextMenuItem ItemRenameFolder { get; private set; }
						internal RsCollectionTreeContextMenuItem ItemRemoveFolder { get; private set; }
				
						public RsCollectionTreeContextMenu(RsCollectionTree Owner): base() {
								ItemNewFolder = new RsCollectionTree.RsCollectionTreeContextMenuItem("_newFolder", this);
								ItemRenameFolder = new RsCollectionTree.RsCollectionTreeContextMenuItem("_renameFolder", this);
								ItemRemoveFolder = new RsCollectionTree.RsCollectionTreeContextMenuItem("_removeFolder", this);
								
								this.MenuItems.Add(ItemNewFolder);
								this.MenuItems.Add(ItemRenameFolder);
								this.MenuItems.Add(new MenuItem("-"));
								this.MenuItems.Add(ItemRemoveFolder);
								
								ItemNewFolder.Click += new EventHandler(ehNewFolder);
								ItemRenameFolder.Click += new EventHandler(ehRenameFolder);
								ItemRemoveFolder.Click += new EventHandler(ehRemoveFolder);
								
								IsMenuActive = false;
							}
							
						#region Event handlers:						
						protected void ehNewFolder(object aSender, EventArgs aE) {
								// TODO Implement for root folders
						
								if (SourceNode != null)
										SourceNode.AddChild(SourceNode.Owner.DefaultNodeName);
							}
							
						protected void ehRenameFolder(object aSender, EventArgs aE) {
								// TODO Implement for collection
						
								SourceNode.Editing = true;
							}
							
						protected void ehRemoveFolder(object aSender, EventArgs aE) {
								// TODO Implement for collection
						
								SourceNode.Delete();
							}
						#endregion
							
					}
			}
}
