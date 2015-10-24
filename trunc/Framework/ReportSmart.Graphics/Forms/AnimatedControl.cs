#region Source information

//*****************************************************************************
//
//    AnimatedControl.cs
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
using System.Windows.Forms;

namespace ReportSmart.Controls
{
    internal abstract class CAnimatedControl : Control
    {
        private Timer _timer;

        public int FrameRate
        {
            get { return _timer.Interval / 1000; }
            set { _timer.Interval = value == 0 ? 1000 : 1000 / value; }
        }

        public bool Animation
        {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        protected abstract void onTick();

        protected void ehTick(object aSender, EventArgs aEArgs) { onTick(); }

        public CAnimatedControl()
        {
            _timer = new Timer();
            _timer.Enabled = false;
            _timer.Tick += new EventHandler(ehTick);

            FrameRate = 25;
            DoubleBuffered = true;
        }
    }
}

