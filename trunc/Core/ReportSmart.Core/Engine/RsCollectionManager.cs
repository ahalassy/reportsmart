#region Source information

//*****************************************************************************
//
//    RsCollectionManager.cs
//    Created by Adam (2015-10-23, 9:21)
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
using ReportSmart.Documents.Collections;
using ReportSmart.Engine.Config;

namespace ReportSmart.Engine
{
    public class RsCollectionManager
    {

        public static RsReportCollection CreateReportCollection(RsCollectionConfig aConfig)
        {
            RsReportCollection lResult = new RsReportCollection();
            lResult.LoadFromXML(aConfig.Path);
            return lResult;
        }

        public RsProfileManager ProfileManager { get; protected set; }

        public List<RsCollectionConfig> CollectionList { get { return ProfileManager.Profile.Collections; } }

        public void AddCollection(RsReportCollection aCollection)
        {
            RsCollectionConfig lConfig = new RsCollectionConfig();
            lConfig.Path = aCollection.FileName;
            lConfig.Name = aCollection.CollectionName;
            lConfig.Type = RsCollectionType.Custom;

            ProfileManager.Profile.Collections.Add(lConfig);
            ProfileManager.SaveProfile();
        }

        public void RemoveCollection(RsReportCollection aCollection)
        {
            foreach (RsCollectionConfig iConfig in ProfileManager.Profile.Collections)
            {
                if (aCollection.FileName.ToUpper().Equals(iConfig.Path.ToUpper()))
                {
                    ProfileManager.Profile.Collections.Remove(iConfig);
                    break;
                }
            }
        }

        public RsReportCollection GetCollection(string aCollectionName)
        {
            foreach (RsCollectionConfig iConfig in ProfileManager.Profile.Collections)
            {
                if (iConfig.Name.ToUpper().Equals(aCollectionName.ToUpper()))
                {
                    RsReportCollection lResult = new RsReportCollection();
                    lResult.LoadFromXML(iConfig.Path);
                    return lResult;
                }
            }

            return null;
        }

        public RsReportCollection CreateCollection(string aCollectionName, string aFile)
        {
            // TODO Create a method to handle more than one collection with the same name

            RsReportCollection lResult;

            RsReportCollection lCollection = GetCollection(aCollectionName);
            if (lCollection != null)
                throw new Exception("Collection named \"" + aCollectionName + "\" already linked.");

            else
            {
                lResult = new RsReportCollection();
                lResult.CollectionName = aCollectionName;
                lResult.SaveToXML(aFile);
                AddCollection(lResult);
            }

            return lResult;
        }

        public RsCollectionManager(RsProfileManager aProfileManager)
        {
            ProfileManager = aProfileManager;
        }
    }
}
