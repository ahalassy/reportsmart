#region Source information

//*****************************************************************************
//
//    User32_DLL.cs
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
using System.Runtime.InteropServices;

namespace ReportSmart.Special.WinApi
{
    public static class libUser32
    {

        #region PUBLIC CONSTANTS:
        public const int GWL_STYLE = -16;

        public const ulong
            WS_BORDER = 0x00800000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_DLGFRAME = 0x00400000,
            WS_SIZEBOX = 0x00040000;

        public const int // Getting system metrics:
            SM_CXBORDER = 5,
            SM_CXSIZEFRAME = 32,
            SM_CXEDGE = 45;

        public const int // Other values:
            BYPOSITION = 0x400;

        public const ulong MY_SIZEABLEBOX = WS_SIZEBOX;
        #endregion
        //--------------------------------------------------------------------------------------------------------------

        #region PUBLIC METHODS:
        #region PUBLIC METHODS - DLL IMPORTS:
        [DllImport("User32.dll")]
        public static extern ulong SetWindowLong(IntPtr aHandle, int aIndex, ulong dwNewLong);

        [DllImport("User32.dll")]
        public static extern ulong GetWindowLong(IntPtr aHandle, int aIndex);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int aIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string windowTitle);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(
                    IntPtr handleParent,
                    IntPtr handleChild,
                    string className,
                    string WindowName
                );

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr HWND);

        [DllImport("user32.dll")]
        public static extern IntPtr AppendMenu(
                    IntPtr MenuHandle,
                    int Props,
                    int FlagsW,
                    string text
                );
        #endregion

        #endregion
        //--------------------------------------------------------------------------------------------------------------

    }
}