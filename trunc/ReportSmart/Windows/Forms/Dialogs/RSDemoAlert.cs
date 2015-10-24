#region Source information

//*****************************************************************************
//
//    RSDemoAlert.cs
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

using System.Windows.Forms;
using ReportSmart.Application;
using ReportSmart.Localization;

namespace ReportSmart.Controls
{
    /// <summary>
    /// Description of Form1.
    /// </summary>
    internal partial class CdlgDemoAlert : Form, ILocalizedControl
    {
        private const int SH_REMAIN = 1;
        private const int SH_EXPIRED = 0;

        private CLocalization _Locale;
        public int Mode;

        public static void ShowDemoAlert(long aVal, long aChkVal, int aPerid)
        {
            CdlgDemoAlert lDialog = new CdlgDemoAlert();
            RsViewEngine.Locale.AddLocalizedControl(lDialog);

            lDialog.Mode = ((aVal - aChkVal) > 0) ? SH_REMAIN : SH_EXPIRED;
            if (lDialog.Mode == SH_REMAIN)
                lDialog.UseForTag = RsViewEngine.Locale.GetTagText("rc1info1") + " " + (aVal - aChkVal) + " " + RsViewEngine.Locale.GetTagText("rc1info2");
            else
                lDialog.UseForTag = "";
            lDialog.ApplyLocale(null);
            lDialog.ShowDialog();
            lDialog.ReleaseLocale();
        }

        public string UseForTag
        {
            get { return _lUseFor.Text; }
            set { _lUseFor.Text = value; }
        }

        public void ApplyLocale(CLocalization aSender)
        {
            CLocalization lLocale = aSender == null ? _Locale : aSender;

            if (lLocale != null)
            {
                _bOk.Text = lLocale.GetTagText("next");

                switch (Mode)
                {
                    case SH_REMAIN:
                        _lTitle.Text = lLocale.GetMessageTitle("auth_remain");
                        _lInfo.Text = lLocale.GetMessage("auth_remain");
                        break;

                    default:
                        _lTitle.Text = lLocale.GetMessageTitle("auth_expired");
                        _lInfo.Text = lLocale.GetMessage("auth_expired");
                        break;
                }
            }
        }


        public void ReleaseLocale()
        {
            _Locale.RemoveLocalizedControl(this);
            _Locale = null;
        }


        public void AssignLocale(CLocalization aLocale) { _Locale = aLocale; }


        public string GetInstanceName() { return "dlgDemoAlert"; }

        public CdlgDemoAlert()
        {
            InitializeComponent();

        }
    }
}
