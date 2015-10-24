#region Source information

//*****************************************************************************
//
//    Wizard.cs
//    Created by Adam (2015-10-23, 8:59)
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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using ReportSmart.Controls;
using ReportSmart.Localization;

namespace ReportSmart.Forms
{

    public abstract class CWizardPanel : CLocalizedPanel
    {
        public CWizardDialog HostDialog;
        protected XmlNode LocaleData;
        private Label _lDescription;
        private EventHandler _EH_ControlChanged;
        public bool AllwaysOk = false;

        protected Label lDescription { get { return _lDescription; } }

        protected internal virtual void DoPageShow()
        {
            foreach (Control iControl in this.Controls)
            {
                iControl.TextChanged -= _EH_ControlChanged;
                iControl.TextChanged += _EH_ControlChanged;
            }
        }

        public abstract void ResetPage();

        public override void ApplyLocale(CLocalization aLocale)
        {
            XmlNode lData = null;

            if (HostDialog != null)
            {
                lData = aLocale.GetWizardData(HostDialog.Name);
                if (lData != null)
                {
                    lData = XmlTools.getXmlNodeByAttrVal("page", "name", this.Name, lData);
                    _lDescription.Text = XmlTools.getXmlNodeByName("description", lData).InnerText;
                }
            }

            LocaleData = lData;
        }

        public CWizardPanel() : base()
        {
            this.SuspendLayout();

            _lDescription = new Label();
            _lDescription.Name = "lDescription";
            _lDescription.Location = new Point(ControlProperties.ControlSpacing, ControlProperties.ControlSpacing);
            _lDescription.Size = new Size(this.ClientRectangle.Width - 2 * ControlProperties.ControlSpacing, 3 * ControlProperties.TextCtlHeight);
            _lDescription.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);

            this.Controls.Add(_lDescription);

            _EH_ControlChanged = new EventHandler(EH_ControlChanged);

            this.ResumeLayout();
        }

        protected virtual void EH_ControlChanged(object aSender, EventArgs aEArgs)
        {
            HostDialog.PageIsOk = true;
        }
    }

    public class CWizardDialog : CMyDialog
    {
        private ArrayList _Panels;
        private CWizardPanel _CurrentPage;
        private Button _bPrev, _bNext;

        public override string GetInstanceName() { return "CWizardDialog"; }

        protected override string buildTitle(CLocalization aLocale)
        {
            string lResult = "";

            XmlNode lNode = XmlTools.getXmlNodeByName("title", DialogLocale);
            lResult = lNode == null ? "" : lNode.InnerText;

            if (this._Panels.Count > 0)
                return lResult + " (" + (_Panels.IndexOf(_CurrentPage) + 1).ToString() + "/" + _Panels.Count.ToString() + ")";
            else
                return lResult;
        }

        public int VisiblePage
        {
            get { return _Panels.IndexOf(_CurrentPage); }
            set { SwapPage(value); }
        }

        public bool PageIsOk
        {
            set { _bNext.Enabled = value; bOk.Enabled = value; }
        }

        public void AddPage(CWizardPanel aPanel)
        {
            if (_Panels.IndexOf(aPanel) < 0)
            {
                _Panels.Add(aPanel);
                aPanel.HostDialog = this;
            }
        }

        public void FirstPage() { VisiblePage = 0; }
        public void NextPage() { VisiblePage++; }

        public void SwapPage(int aPageNo)
        {
            aPageNo = aPageNo >= 0 ? (aPageNo < _Panels.Count ? aPageNo : _Panels.Count - 1) : 0;

            CWizardPanel lNewPanel = _Panels.Count == 0 ? null : (CWizardPanel)(_Panels[aPageNo]);
            if (_CurrentPage == lNewPanel)
                return;

            if (lNewPanel != null)
            {
                this.SuspendLayout();

                lNewPanel.Location = new Point(0, this.CustomAreaTop);
                lNewPanel.Size = new Size(this.ClientRectangle.Width, this.CustomAreaBottom - this.CustomAreaTop);
                this.Controls.Add(lNewPanel);
                if (_CurrentPage != null)
                {
                    this.Controls.Remove(_CurrentPage);
                }

                this.ResumeLayout();
            }

            _CurrentPage = lNewPanel;

            bOk.Visible = _Panels.IndexOf(_CurrentPage) + 1 == _Panels.Count;
            _bNext.Visible = _Panels.IndexOf(_CurrentPage) + 1 != _Panels.Count;
            _bPrev.Visible = _Panels.IndexOf(_CurrentPage) != 0;

            lTitle.Text = buildTitle(null);

            _bNext.Enabled = false;
            bOk.Enabled = false;

            if (_CurrentPage != null)
            {
                _CurrentPage.DoPageShow();
                _bNext.Enabled = _bNext.Enabled || _CurrentPage.AllwaysOk;
                bOk.Enabled = bOk.Enabled || _CurrentPage.AllwaysOk;
            }
        }

        public void PrevPage() { VisiblePage--; }
        public void LastPage() { VisiblePage = _Panels.Count - 1; }


        public override void ApplyLocale(CLocalization aSender)
        {
            DialogLocale = aSender.GetWizardData(this.Name);
            bOk.Text = aSender.GetTagText("finish");
            bCancel.Text = aSender.GetTagText("cancel");
            lTitle.Text = buildTitle(aSender);

            _bPrev.Text = "<< " + aSender.GetTagText("prev");
            _bNext.Text = aSender.GetTagText("next") + " >>";
            if (DialogLocale != null)
            {
                this.Text = XmlTools.getXmlNodeByName("caption", DialogLocale).InnerText;
            }

            foreach (object iPanel in _Panels)
            {
                ((CWizardPanel)iPanel).ApplyLocale(aSender);
            }
        }

        public CWizardDialog() : base()
        {
            this.Size = new Size(this.Width, 512);
            this.DoubleBuffered = true;

            bCancel.Location = bOk.Location;
            bOk.Location = new Point(
                        this.Width - ControlProperties.ButtonWidth - ControlProperties.ControlSpacing * 2,
                        bCancel.Top
                    );

            _Panels = new ArrayList();

            _bPrev = new Button();
            _bPrev.Location = new Point(ControlProperties.ControlSpacing * 2, bOk.Top);
            _bPrev.Size = ControlProperties.ButtonSize();
            _bPrev.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Bottom);

            _bNext = new Button();
            _bNext.Location = bOk.Location;
            _bNext.Size = ControlProperties.ButtonSize();
            _bNext.Anchor = (AnchorStyles)(AnchorStyles.Right | AnchorStyles.Bottom);

            this.Controls.Add(_bPrev);
            this.Controls.Add(_bNext);

            _bPrev.Click += new EventHandler(EH_Prev);
            _bNext.Click += new EventHandler(EH_Next);

            ShowDescription = false;
        }

        protected virtual void EH_Prev(object aSender, System.EventArgs aEArgs) { PrevPage(); }
        protected virtual void EH_Next(object aSender, System.EventArgs aEArgs) { NextPage(); }

        protected override void dialogShow(object aSender, System.EventArgs aEArgs)
        {
            base.dialogShow(aSender, aEArgs);

            foreach (object iPage in _Panels)
            {
                ((CWizardPanel)iPage).ResetPage();
            }

            FirstPage();
        }
    }

}