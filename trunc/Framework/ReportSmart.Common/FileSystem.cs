#region Source information

//*****************************************************************************
//
//    FileSystem.cs
//    Created by Adam (2015-10-24, 10:21)
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
using System.IO;

namespace ReportSmart
{
    public static class FileSystem
    {
        public const string GROUP_DIR = "ReportSmart";

        public static string DefaultFileName = "document";

        //public static 

        public static string GetDriveLetterOfPath(string aPath)
        {
            if (aPath.Length >= 2)
            {
                if (aPath[1] == ':')
                    return aPath.Substring(0, 2);
                else
                    return "";
            }
            else
            {
                return "";
            }
        }

        public static string ToPath(string aPath)
        {
            // Converts aPath string to "C:\ ... \folder\" form.
            if (aPath[aPath.Length - 1] != '\\')
                return aPath += '\\';
            else
                return aPath;
        }

        public static string GetTempDirectory(string aPool) { return GetTempDirectory(aPool, true); }

        public static string GetTempDirectory(string aPool, bool aBuild)
        {
            string lResult = Environment.GetEnvironmentVariable("TEMP");
            BuildPath(lResult + '\\' + GROUP_DIR + (aPool == "" ? "" : '\\' + aPool));
            return lResult + '\\' + GROUP_DIR + (aPool == "" ? "" : '\\' + aPool);
        }

        public static string GetSettingsDirectory(string aPool, bool aBuild)
        {
            string lResult = Environment.GetEnvironmentVariable("APPDATA" + "");

            if (aBuild) BuildPath(lResult + '\\' + GROUP_DIR + (aPool == "" ? "" : '\\' + aPool));

            return lResult + '\\' + GROUP_DIR + (aPool == "" ? "" : '\\' + aPool);
        }

        public static int BuildPath(string aPath)
        {
            // Creates the full path
            int Result = 0;
            int lPos = -1;
            int lStrPos;

            do
            {
                lPos = aPath.IndexOf('\\', lPos + 1);
                //lPos = StringTools.IndexOf(lPos + 1, "\\", aPath);

                lStrPos = lPos == -1 ? aPath.Length : lPos;
                if (!Directory.Exists(aPath.Substring(0, lStrPos)))
                    Directory.CreateDirectory(aPath.Substring(0, lStrPos));
            } while (lPos != -1);
            return Result;
        }

        public static string GetTempFileName(string aPool, string aExtension)
        {
            return GetTempFileName(aPool, "", aExtension);
        }

        public static string GetTempFileName(string aPool, string aPath, string aExtension)
        {
            string lPath = aPath == "" ? GetTempDirectory(aPool) : aPath;
            int lHex = 0;
            lPath = ToPath(lPath);

            while (File.Exists(lPath + DefaultFileName + ((lHex > 0) ? ("." + lHex.ToString()) : ("")) + "." + aExtension))
                lHex++;

            return lPath + DefaultFileName + ((lHex > 0) ? ("." + lHex.ToString()) : ("")) + "." + aExtension;
        }

        public static void ClearDefaultTemp(string aPool)
        {
            string lPath = GetTempDirectory(aPool);

            if (Directory.Exists(lPath))
                foreach (string iFile in Directory.GetFiles(lPath))
                    try
                    {
                        File.Delete(iFile);
                    }
                    catch {; }
        }

        public static string NameOf(string aPath)
        {
            int i, j;
            for (i = -1; aPath.IndexOf('\\', i + 1) > -1; i = aPath.IndexOf('\\', i + 1)) ;
            for (j = -1; aPath.IndexOf('.', j + 1) > -1; j = aPath.IndexOf('.', j + 1)) ;
            i++;
            return aPath.Substring(i, j - i);
        }
    }
}