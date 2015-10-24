#region Source information

//*****************************************************************************
//
//    RsCollectionBrowser.cs
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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ReportSmart.Controls;
using ReportSmart.Documents.Collections;
using ReportSmart.Engine;
using ReportSmart.Engine.Config;
using ReportSmart.Localization;

namespace ReportSmart.Windows.Forms
{
    /// <summary>
    /// Description of RsCollectionBrowser.
    /// </summary>
    /// 
    public class RsCollectionBrowser : CSpecialPanelView, ILocalizedControl
    {

        protected Image _ImgOpen, _ImgClose, _FolderImage;

        protected Color _HeaderColor, _FavoritesHeaderColor;


        #region Members to control RsCollectionTree items
        public Image ImageOpen
        {
            get { return _ImgOpen; }
            set { _ImgOpen = value; ApplyToPanels(); }
        }

        public Image ImageClose
        {
            get { return _ImgClose; }
            set { _ImgClose = value; ApplyToPanels(); }
        }

        public Image FolderImage
        {
            get { return _FolderImage; }
            set
            {
                //_FolderImage.
                _FolderImage = value;
                ApplyToPanels();
            }
        }

        public Color HeaderColor
        {
            get { return _HeaderColor; }
            set { _HeaderColor = value; ApplyToPanels(); }
        }

        public Color FavoritesHeaderColor
        {
            get { return _FavoritesHeaderColor; }
            set { _FavoritesHeaderColor = value; ApplyToPanels(); }
        }

        protected void ApplyToPanels()
        {
            foreach (Control iCtl in this.Controls)
                if (iCtl is RsCollectionTree)
                {
                    ((RsCollectionTree)iCtl).ImageOpen = _ImgOpen;
                    ((RsCollectionTree)iCtl).ImageClose = _ImgClose;
                    ((RsCollectionTree)iCtl).HeaderColor =
                                ((RsCollectionTree)iCtl).CollectionProvider.GetCollectionType() == RsCollectionProviderType.Favorites ?
                                    _HeaderColor : _FavoritesHeaderColor;
                    ((RsCollectionTree)iCtl).FolderImage = _FolderImage;
                }
        }
        #endregion

        public string FavoritesCollectionName { get; protected set; }

        public string DefaultFolderName { get; protected set; }

        public CLocalization Localization { get; protected set; }

        public RsCollectionManager CollectionManager;

        public event NodeSelectedEvent CollectionNodeSelected;

        public RsCollectionTreeNode SelectedCollectionNode { get; protected set; }

        public RsReportCollection CurrentCollection
        {
            get
            {
                // TODO Implement RsReportcollection.CurrentCollection
                throw new NotImplementedException();
            }
        }

        public void ApplyLocale(CLocalization aLocale)
        {
            FavoritesCollectionName = aLocale.GetTagText("favorites");
            DefaultFolderName = aLocale.GetTagText("newFolder");

            foreach (Control iControl in Controls)
            {
                if (iControl is RsCollectionTree)
                    ((RsCollectionTree)iControl).RefreshPageTitle();
            }
        }

        public void AssignLocale(CLocalization aSender)
        {
            Localization = aSender;
        }

        public void ReleaseLocale()
        {
            Localization = null;
        }

        public string GetInstanceName() { return this.Name; }

        public void RefreshControl()
        {
            this.Controls.Clear();

            List<RsCollectionConfig> lCollections = CollectionManager.CollectionList;
            foreach (RsCollectionConfig iConfig in lCollections)
            {
                RsCollectionProvider iProvider = new RsReportCollectionProvider(iConfig.Path);
                RsCollectionTree lTree = new RsCollectionTree(this, iProvider);
                lTree.NodeSelected += new NodeSelectedEvent(ehNodeSelected);
                lTree.ImageOpen = ImageOpen;
                lTree.ImageClose = ImageClose;
                lTree.HeaderColor = iProvider.GetCollectionType() == RsCollectionProviderType.Favorites ? FavoritesHeaderColor : HeaderColor;
                lTree.HeaderFontColor = Color.White;
                lTree.FolderImage = FolderImage;

                if (Localization != null)
                    Localization.AddLocalizedControl(lTree);
                this.AddPanel(lTree);
            }
        }

        public RsCollectionBrowser(RsCollectionManager aManager) : base()
        {
            this.BackColor = Color.White;

            CollectionManager = aManager;

            FavoritesCollectionName = "*Favorites";

            DefaultFolderName = "_new folder";
        }

        #region EventHandlers:
        protected void ehNodeSelected(RsCollectionTree aSender, RsCollectionTreeNode aNode)
        {
            foreach (Control iCtl in this.Controls)
            {
                if (iCtl is RsCollectionTree && iCtl != aSender)
                {
                    ((RsCollectionTree)iCtl).DeselectNode();
                }
            }

            this.SelectedCollectionNode = aNode;

            if (CollectionNodeSelected != null)
                CollectionNodeSelected(aSender, aNode);
        }
        #endregion
    }
}
