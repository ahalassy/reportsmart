#region Source information

//*****************************************************************************
//
//    RsSettingsConfig.cs
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

using System.Xml.Serialization;

namespace ReportSmart.Engine.Config
{
    /// <summary>
    /// Description of RsSettings.
    /// </summary>

    [XmlTypeAttribute(TypeName = "settings")]
    public class RsSettingsConfig
    {

        [XmlElementAttribute(ElementName = "locale")]
        public RsLocaleConfig Locale;

        [XmlElementAttribute(ElementName = "userinterface")]
        public RsUserInterfaceConfig UserInterface { get; set; }

        [XmlElementAttribute(ElementName = "security")]
        public RsSecurityConfig Security;

        [XmlElementAttribute(ElementName = "offline")]
        public RsOfflineConfig Offline;

        [XmlElementAttribute(ElementName = "embeddedparamedit")]
        public RsEmbeddedParamEditConfig EmbeddedParamEdit;

        public RsSettingsConfig()
        {
            Locale = null;
            UserInterface = new RsUserInterfaceConfig();
            Security = null;
            Offline = new RsOfflineConfig();
            EmbeddedParamEdit = new RsEmbeddedParamEditConfig();
        }
    }
}
