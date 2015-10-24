/*
 *
 * Licensing:			GPL
 * Original project:	view.csproj
 *
 * Copyright: Adam Halassy (2010.12.13.)
 * 
 * 
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using Halassy.Localization;

using ReportSmart;
using ReportSmart.Application;
using ReportSmart.Windows;
using ReportSmart.Windows.Forms;

namespace ReportSmart.Windows.Forms {
		public partial class RsCollectionManagement: Panel, ILocalizedControl {
		
		
				
				public RsCollectionBrowser CollectionBrowser { get; protected set; }
				//protected Splitter Splitter { get; set; }
				protected SplitContainer ControlContainer { get; set;}
				public ListView ItemList { get; protected set; }
				protected RsCollectionMgmtContext ListContext { get; set; }
				
				protected void InitializeControl() {
						this.SuspendLayout();
						this.BackColor = Color.White;
						
						ControlContainer = new SplitContainer();
						ControlContainer.Dock = DockStyle.Fill;
				
						CollectionBrowser = new RsCollectionBrowser(RsViewEngine.CollectionManager);
						CollectionBrowser.Dock = DockStyle.Fill;
						CollectionBrowser.Size = new System.Drawing.Size(
									512,
									CollectionBrowser.Height
								);
								
						ListContext = new RsCollectionManagement.RsCollectionMgmtContext(this);
						
						ItemList = new ListView();
						ItemList.Dock = DockStyle.Fill;
						ItemList.View = View.LargeIcon;
						ItemList.ContextMenu = ListContext;
					
						this.Controls.Add(ControlContainer);

						ControlContainer.Panel1.Controls.Add(CollectionBrowser);
						ControlContainer.Panel2.Controls.Add(ItemList);
						
						this.ResumeLayout();
						
						AssignEventHandlers();
					}
			}
	}
