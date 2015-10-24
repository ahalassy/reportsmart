/*
 *
 * Licensing:			GPL
 * Original project:	bin.csproj
 *
 * Copyright: Adam ReportSmart (2009.11.13.)
 * 
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ReportSmart.Localization;
 
using ReportSmart;
using ReportSmart.Application;

namespace ReportSmart.Controls {
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
		
		public void GatherLanguages() {
				List<CLocaleInfo> lLangs = RsViewEngine.ProfileManager.LookupLanguages();
				_ctlLangList.Items.Clear();
				
				foreach (object iObj in lLangs) {
						_ctlLangList.Items.Add(iObj);
					}
					
				_ctlLangList.SelectedIndex = 0;
				_lChooseLang.Text = ((CLocaleInfo)(lLangs[0])).ChooseInstruction;
			}
		
		public void Execute() {
				GatherLanguages();
				
				ShowDialog();
				
				RsViewEngine.ProfileManager.Profile.Settings.Locale.Language = ((CLocaleInfo)(_ctlLangList.Items[_ctlLangList.SelectedIndex])).LocaleID;
				RsViewEngine.ProfileManager.SaveProfile();
			}
		
		void ehLangChange(object sender, EventArgs e) {
				if (_ctlLangList.SelectedIndex > -1)
		 				_lChooseLang.Text = ((CLocaleInfo)(_ctlLangList.Items[_ctlLangList.SelectedIndex])).ChooseInstruction;
			}
			
		void ehLangChoose(object sender, EventArgs e) {
				if (_ctlLangList.SelectedIndex > -1) {
						_lChooseLang.Text = ((CLocaleInfo)(_ctlLangList.Items[_ctlLangList.SelectedIndex])).ChooseInstruction;
						this.DialogResult = DialogResult.OK;
					}
			}
		
		void _bOkClick(object sender, EventArgs e) {
				this.DialogResult = DialogResult.OK;
			}
	}
}
