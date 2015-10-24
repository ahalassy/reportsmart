#region Source information

//*****************************************************************************
//
//    RSInitLang.cs
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
using System.Collections.Generic;
using System.Windows.Forms;
using ReportSmart.Application;
using ReportSmart.Localization;

namespace ReportSmart.Controls
{
    /// <summary>
    /// Description of Form1.
    /// </summary>
    internal partial class CRSInitLang : Form
    {
        public CRSInitLang()
        {
            InitializeComponent();

            _ctlLangList.Click += new EventHandler(ehLangChange);
            _ctlLangList.DoubleClick += new EventHandler(ehLangChoose);
        }

        public void GatherLanguages()
        {
            List<CLocaleInfo> lLangs = RsViewEngine.ProfileManager.LookupLanguages();
            _ctlLangList.Items.Clear();

            foreach (object iObj in lLangs)
            {
                _ctlLangList.Items.Add(iObj);
            }

            _ctlLangList.SelectedIndex = 0;
            _lChooseLang.Text = ((CLocaleInfo)(lLangs[0])).ChooseInstruction;
        }

        public void Execute()
        {
            GatherLanguages();

            ShowDialog();

            RsViewEngine.ProfileManager.Profile.Settings.Locale.Language = ((CLocaleInfo)(_ctlLangList.Items[_ctlLangList.SelectedIndex])).LocaleID;
            RsViewEngine.ProfileManager.SaveProfile();
        }

        void ehLangChange(object sender, EventArgs e)
        {
            if (_ctlLangList.SelectedIndex > -1)
                _lChooseLang.Text = ((CLocaleInfo)(_ctlLangList.Items[_ctlLangList.SelectedIndex])).ChooseInstruction;
        }

        void ehLangChoose(object sender, EventArgs e)
        {
            if (_ctlLangList.SelectedIndex > -1)
            {
                _lChooseLang.Text = ((CLocaleInfo)(_ctlLangList.Items[_ctlLangList.SelectedIndex])).ChooseInstruction;
                this.DialogResult = DialogResult.OK;
            }
        }

        void _bOkClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
