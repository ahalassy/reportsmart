#region Source information

//*****************************************************************************
//
//    CollectionEditor.cs
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
using System.Xml;
using ReportSmart.Application;
using ReportSmart.Documents.Collections;
using ReportSmart.Localization;
using ReportSmart.Windows.Forms;

namespace ReportSmart.Controls
{
    internal class CCollectionEditor : CLocalizedPanel
    {
        private const string LOCALE_ALIAS = "CollectionEditor";

        private const string NOCOLLECTION_FILE = "collectionNotSelected";
        private const string NOCOLLECTION_FOLDER = "collectionNotSelected-Folder";

        public const int BUTTONMARGIN = 8;
        public const int BUTTONWIDTH = 128;
        public const int BUTTONHEIGHT = 32;

        private TreeView _eCollection;
        private CfAddReport _dlgAddReport;
        private CdlgAddFolder _dlgAddFolder;
        private ImageList _IconList;
        private OpenFileDialog _dlgOpenCollection;
        //private SaveFileDialog _dlgSaveCollection;

        public override string GetInstanceName() { return "CCollectionEditor"; }

        public void DoAddReport()
        {
            XmlNode lThisData = RsViewEngine.Locale.GetFormData(LOCALE_ALIAS);

            RsReportCollection lCurrColl = getSelectedCollection();
            if (lCurrColl == null)
                CRSMessageBox.ShowBox(
                            XmlTools.getXmlNodeByAttrVal("name", NOCOLLECTION_FILE, lThisData).InnerText,
                            RsViewEngine.Locale.GetTagText("error"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation
                        );

            else
            {
                if (lCurrColl is CFavoritesCollection)
                    _dlgAddReport.CollectionName = RsViewEngine.Locale.GetTagText(RsLocalization.TAG_FAVORITES);
                else
                    _dlgAddReport.CollectionName = lCurrColl.CollectionName;
                _dlgAddReport.Modify = false;
                DialogResult lDlgRes = _dlgAddReport.ShowDialog();
                if (lDlgRes == DialogResult.OK)
                {
                    CReportFolder lParent = getCurrentParent();
                    if (lParent == null)
                    {
                        CRSMessageBox.ShowBox(
                                    RsViewEngine.Locale.GetTagText("collectionNotSelected"),
                                    RsViewEngine.Locale.GetTagText("error"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                        return;
                    }
                    CReportFile lNewFile = new CReportFile(_dlgAddReport.Alias, _dlgAddReport.ReportFile);
                    lNewFile.Parent = lParent;
                    lParent.Collection.ApplyToGUI();
                    lParent.Collection.QuickSave();
                }
            }
        }

        public void DoModify()
        {
            CReportItem lCurrentReport = getSelectedItem();
            if (lCurrentReport is CReportFile)
            {
                _dlgAddReport.Modify = true;
                _dlgAddReport.ReportFile = ((CReportFile)lCurrentReport).ReportFile;
                _dlgAddReport.Alias = ((CReportFile)lCurrentReport).ItemName;
                if (_dlgAddReport.ShowDialog() == DialogResult.OK)
                {
                    ((CReportFile)lCurrentReport).ItemName = _dlgAddReport.Alias;
                    ((CReportFile)lCurrentReport).ReportFile = _dlgAddReport.ReportFile;
                }
            }
        }

        public void DoRemove()
        {
            XmlNode lThisData = RsViewEngine.Locale.GetFormData(LOCALE_ALIAS);
            CReportItem lSelected = getSelectedItem();
            RsReportCollection lCollection = getSelectedCollection();
            string lVerifyMsg;
            string lItemName;

            if (lSelected == null)
            {
                CRSMessageBox.ShowBox(
                            XmlTools.getXmlNodeByAttrVal("name", "itemNotSelected_del", lThisData).InnerText,
                            RsViewEngine.Locale.GetTagText("error"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation
                        );
                return;
            }

            lItemName = lSelected.ItemName;
            if (lSelected is CReportRootFolder)
            {
                if (lSelected is CReportFavoritesRoot)
                {
                    lVerifyMsg = XmlTools.getXmlNodeByAttrVal("name", "verify_eraseFavs", lThisData).InnerText;
                }
                else
                {
                    lVerifyMsg = XmlTools.getXmlNodeByAttrVal("name", "verify_rmCollection", lThisData).InnerText + " " + lCollection.CollectionName + "?";
                }
                DialogResult lDlgRes = CRSMessageBox.ShowBox(
                            lVerifyMsg,
                            RsViewEngine.Locale.GetTagText("verify"),
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation
                        );
                if (lDlgRes == DialogResult.Yes)
                    if (lSelected is CReportFavoritesRoot)
                    {
                        lCollection.ClearCollection();
                        lCollection.QuickSave();
                        lCollection.ApplyToGUI();
                        _eCollection.SelectedNode = lCollection.RootFolder.GUINode;
                    }
                    else
                    {
                        if (lCollection.Modified)
                        {
                            lDlgRes = CRSMessageBox.ShowBox(
                                        XmlTools.getXmlNodeByAttrVal("name", "nosaved_collection", lThisData).InnerText + " " + lCollection.CollectionName + "?",
                                        RsViewEngine.Locale.GetTagText("save"),
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question
                                    );
                            if (lDlgRes == DialogResult.Yes)
                                RsViewEngine.SaveCollection(lCollection);
                        }
                        RsViewEngine.CloseCollection(lCollection);
                        _eCollection.SelectedNode = RsViewEngine.Favorites.RootFolder.GUINode;
                    }
            }
            else
            {
                if (lSelected is CReportFolder)
                {
                    lVerifyMsg = XmlTools.getXmlNodeByAttrVal("name", "verify_rmFolder", lThisData).InnerText + " " + lItemName + "?";
                }
                else
                {
                    lVerifyMsg = XmlTools.getXmlNodeByAttrVal("name", "verify_rmFile", lThisData).InnerText + " " + lItemName + "?";
                }

                DialogResult lDlgRes = CRSMessageBox.ShowBox(
                            lVerifyMsg,
                            RsViewEngine.Locale.GetTagText("verify"),
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation
                        );

                if (lDlgRes == DialogResult.Yes)
                {
                    _eCollection.SelectedNode = lSelected.GUINode;
                    lSelected.Release();
                    lCollection.QuickSave();
                }
            }
        }

        public void DoAddFolder()
        {
            XmlNode lThisData = RsViewEngine.Locale.GetFormData(LOCALE_ALIAS);

            RsReportCollection lCurrColl = getSelectedCollection();
            if (getSelectedItem() == null)
                CRSMessageBox.ShowBox(
                            XmlTools.getXmlNodeByAttrVal("name", NOCOLLECTION_FOLDER, lThisData).InnerText,
                            RsViewEngine.Locale.GetTagText("error"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation
                        );
            else
            {
                if (lCurrColl is CFavoritesCollection)
                    _dlgAddFolder.CollectionName = RsViewEngine.Locale.GetTagText(RsLocalization.TAG_FAVORITES);
                else
                    _dlgAddFolder.CollectionName = lCurrColl.CollectionName;

                _dlgAddFolder.FolderName = "";
                DialogResult lDlgRes = _dlgAddFolder.ShowDialog();
                if (lDlgRes == DialogResult.OK)
                {
                    CReportFolder lParent = getCurrentParent();
                    CReportFolder lNewFolder = new CReportFolder(_dlgAddFolder.FolderName);
                    lNewFolder.Parent = lParent;
                    lParent.Collection.ApplyToGUI();
                    lParent.Collection.QuickSave();
                }
            }
        }

        public void DoAddCollection()
        {
            RsCollectionControl.ImportCollection();
        }

        public TreeView ctlTreeView { get { return _eCollection; } }

        public RsReportCollection SelectedCollection
        {
            get { return getSelectedCollection(); }
        }

        protected CReportItem getSelectedItem()
        {
            TreeNode lCurrItem = _eCollection.SelectedNode;
            if (lCurrItem == null)
                return null;
            else
                return (CReportItem)(lCurrItem.Tag);
        }

        protected CReportFolder getCurrentParent()
        {
            CReportItem lCurrItem = getSelectedItem();

            if (lCurrItem == null)
                return null;
            else if (lCurrItem is CReportFolder)
                return (CReportFolder)lCurrItem;
            else
                return lCurrItem.Parent;
        }

        protected RsReportCollection getSelectedCollection()
        {
            TreeNode lCurrItem = _eCollection.SelectedNode;
            if (lCurrItem == null)
                return null;
            else
                return ((CReportItem)(lCurrItem.Tag)).Collection;
        }

        public override void ApplyLocale(CLocalization aLocale)
        {
            XmlNode lData = aLocale.GetFormData(LOCALE_ALIAS);
            if (lData == null) return;
        }

        public CCollectionEditor() : base()
        {
            this.SuspendLayout();

            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;

            _eCollection = new TreeView();
            _eCollection.Name = "_eCollection";
            _eCollection.Anchor = ((AnchorStyles)((
                        (AnchorStyles.Top | AnchorStyles.Left) |
                        (AnchorStyles.Right | AnchorStyles.Bottom)
                    )));
            _eCollection.Location = new Point(0, 0);
            _eCollection.Size = new Size(
                        this.ClientRectangle.Width,
                        this.ClientRectangle.Height
                    );
            _eCollection.BorderStyle = BorderStyle.None;
            _eCollection.ShowLines = false;

            _IconList = new ImageList();
            _IconList.ImageSize = new Size(32, 32);
            _IconList.ColorDepth = ColorDepth.Depth32Bit;
            _IconList.Images.Add((Bitmap)RsViewEngine.Resources.GetObject("gfx_folder_32x32"));
            _IconList.Images.Add((Bitmap)RsViewEngine.Resources.GetObject("gfx_rpt_32x32"));
            _IconList.Images.Add((Bitmap)RsViewEngine.Resources.GetObject("gfx_favorites_32x32"));
            _IconList.Images.Add((Bitmap)RsViewEngine.Resources.GetObject("gfx_ReportCollection_32x32"));


            _eCollection.ImageList = _IconList;

            this.Controls.Add(_eCollection);

            this.ResumeLayout(false);

            _dlgAddReport = RsViewEngine.dlgAddReport;
            _dlgAddFolder = RsViewEngine.dlgAddFolder;
            //_dlgCreateCollection = RsViewEngine.dlgCreateCollection;
            _dlgOpenCollection = RsViewEngine.dlgOpenCollection;
            //_dlgSaveCollection = RsViewEngine.dlgSaveCollection;

            RsViewEngine.Favorites.GUITreeView = _eCollection;

            _eCollection.DoubleClick += new EventHandler(this.EH_DblClickEditor);
        }

        protected void EH_AddReport(object aSender, EventArgs aEArgs)
        {
            DoAddReport();
        }

        protected void EH_AddFolder(object aSender, EventArgs aEArgs)
        {
            DoAddFolder();
        }

        protected void EH_Remove(object aSender, EventArgs aEArgs)
        {
            DoRemove();
        }

        protected void EH_ModifyReport(object aSender, EventArgs aEArgs)
        {
            DoModify();
        }

        protected void EH_AddCollection(object aSender, EventArgs aEArgs)
        {
            DoAddCollection();
        }

        protected void EH_LoadCollection(object aSender, EventArgs aEArgs)
        {
            if (_dlgOpenCollection.ShowDialog() == DialogResult.OK)
            {
                RsViewEngine.LoadCollection(_dlgOpenCollection.FileName);
            }
        }

        protected void EH_DblClickEditor(object aSender, EventArgs aEArgs)
        {
            CReportItem lItem = getSelectedItem();
            if (lItem is CReportFile)
            {
                CReportFile lRptFile = (CReportFile)lItem;
                RsViewEngine.MainForm.OpenReport(lRptFile.ItemName, lRptFile.ReportFile);
            }
        }
    }
}

