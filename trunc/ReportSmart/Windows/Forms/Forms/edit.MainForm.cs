/*
 *
 * Licensing:			GPL
 * Original project:	
 *
 * Copyright: Adam Halassy (2010.06.10.)
 * 
 * 
 */
using Halassy.Controls;
 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ReportSmart.Application {
		/// <summary>
		/// Description of MainForm.
		/// </summary>
		public partial class MainForm : Form {
				public MainForm() {
						InitializeComponent();

						CPageSelectorPage lPage = new CPageSelectorPage(null);
						lPage.PageTitle = "Kezdőlap";
						lPage.CloseAble = false;
						
						_ctlPageSelector.AddPage(lPage);
					}
			}
	}
