/*
 *
 * Licensing:			GPL
 * Original project:	cs.ReportHandler.csproj
 *
 * Copyright: Adam Halassy (2010.08.30.)
 * 
 * 
 */
 
using System;
using System.Windows.Forms;
 
 namespace Halassy.Controls {
 		internal abstract class CAnimatedControl: Control {
 				private Timer _timer;
 				
 				public int FrameRate {
 						get { return _timer.Interval / 1000; }
 						set { _timer.Interval = value == 0 ? 1000 : 1000 / value; }
 					}
 					
 				public bool Animation {
 						get { return _timer.Enabled; }
 						set { _timer.Enabled = value; }
 					}
 					
 				protected abstract void onTick();
 				
 				protected void ehTick(object aSender, EventArgs aEArgs) { onTick(); }
 				
 				public CAnimatedControl() {
 						_timer = new Timer();
 						_timer.Enabled = false;
 						_timer.Tick += new EventHandler(ehTick);
 						
 						FrameRate = 25;
 						DoubleBuffered = true;
 					}
 			}
 	}

