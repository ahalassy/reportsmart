#region Source information

//*****************************************************************************
//
//    RsCollectionManagement.Context.cs
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
using System.Windows.Forms;
using ReportSmart.Application;
using ReportSmart.Documents.Collections;
using ReportSmart.Localization;

namespace ReportSmart.Windows.Forms
{
    public partial class RsCollectionManagement : Panel, ILocalizedControl
    {
        protected class RsCollectionMgmtContext : ContextMenu, ILocalizedControl
        {
            public CLocalization Locale { get; protected set; }

            public RsCollectionManagement Owner { get; protected set; }

            public MenuItem ItemOpen { get; protected set; }
            public MenuItem ItemRename { get; protected set; }
            public MenuItem ItemRemove { get; protected set; }

            public void AssignLocale(CLocalization aLocale)
            {
                Locale = aLocale;
            }

            public void ApplyLocale(CLocalization aLocale)
            {
                ItemOpen.Text = aLocale.GetTagText("open");
                ItemRename.Text = aLocale.GetTagText("rename");
                ItemRemove.Text = aLocale.GetTagText("remove");
            }

            public string GetInstanceName()
            {
                return "RsCollectionManagementContext";
            }

            public void ReleaseLocale()
            {
                Locale = null;
            }

            public RsCollectionMgmtContext(RsCollectionManagement aOwner)
            {
                Owner = aOwner;

                ItemOpen = this.MenuItems.Add("_open");
                ItemRename = this.MenuItems.Add("_rename");
                this.MenuItems.Add("-");
                ItemRemove = this.MenuItems.Add("_remove");

                ItemOpen.Click += new EventHandler(ehItemOpen);
            }

            public void ehItemOpen(object aSender, EventArgs aE)
            {
                foreach (RsListViewItem iItem in Owner.ItemList.SelectedItems)
                {
                    if (iItem.IsReportFile())
                    {
                        RsViewEngine.MainForm.OpenReport(
                                    iItem.CollectionItem.ItemName,
                                    ((CReportFile)(iItem.ReportItem)).ReportFile
                                );
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
        }
    }
}
