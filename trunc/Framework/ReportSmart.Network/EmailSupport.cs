#region Source information

//*****************************************************************************
//
//    EmailSupport.cs
//    Created by Adam (2015-10-24, 9:30)
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
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ReportSmart.Network
{
    //[Serializable]
    //internal struct SSecurityInfo {
    //			internal int Type;
    //			internal string UserName;
    //			internal string Password;
    //			internal string ReportFile;
    //			internal string DataSource;

    //		} 	

    public abstract class CSecurityItem : CXmlSettingsNode
    {
        private CSecurityManager _Security = null;

        public CSecurityManager Security
        {
            get { return _Security; }
        }

        protected internal void setSecurity(CSecurityManager aSecurity)
        {
            _Security = aSecurity;
        }

        protected override void initialize()
        {
            base.initialize();
            PropertyChanged += new XmlPropertyChanged(ehValueChanged);
        }

        public string UserName
        {
            get { return this[CSecurityManager.XMLa_USERID]; }
            set { this[CSecurityManager.XMLa_USERID] = value; }
        }

        public string Password
        {
            get { return this[CSecurityManager.XMLa_PASSWORD]; }
            set { this[CSecurityManager.XMLa_PASSWORD] = value; }
        }

        public string Service
        {
            get { return GetServiceName(); }
            set { SetServiceName(value); }
        }

        public void PostSettings()
        {
            if (_Security != null)
                _Security.QuickSave();
        }

        public abstract string GetServiceName();

        public abstract void SetServiceName(string aName);

        public int GetSecType()
        {
            return CSecurityManager.GetSecurityType(DataNode);
        }

        protected void ehValueChanged(CXmlSettingsNode aNode)
        {
            if (_Security != null)
                _Security.QuickSave();
        }

        public CSecurityItem(CSecurityManager aSecurity, string aName, XmlNode aParentNode, bool aIsNew) : base(aName, aParentNode, aIsNew)
        { _Security = aSecurity; }

        public CSecurityItem(CSecurityManager aSecurity, string aName, string aID, XmlNode aParentNode, bool aIsNew) : base(aName, aID, aParentNode, aIsNew)
        { _Security = aSecurity; }

        public CSecurityItem(CSecurityManager aSecurity, XmlNode aNode) : base(aNode)
        { _Security = aSecurity; }
    }

    public class CReportSecurityItem : CSecurityItem
    {
        public bool Authorize
        {
            get { return this.GetAsBool(CSecurityManager.XMLa_AUTH); }
            set
            {
                this.SetAsBool(CSecurityManager.XMLa_AUTH, value);
                PostSettings();
            }
        }

        public string DataSource
        {
            get { return this[CSecurityManager.XMLa_DATASOURCE]; }
            set
            {
                this[CSecurityManager.XMLa_DATASOURCE] = value;
                PostSettings();
            }
        }

        public string ReportFile
        {
            get { return this[CSecurityManager.XMLa_FILE]; }
            set
            {
                this[CSecurityManager.XMLa_FILE] = value;
                PostSettings();
            }
        }

        public override string GetServiceName() { return DataSource; }

        public override void SetServiceName(string aName) { DataSource = aName; }

        public CReportSecurityItem(CSecurityManager aSecurity, XmlNode aNode) : base(aSecurity, aNode) { }
    }

    public class CSecurityManager
    {
        public const int SEC_UNKNOWN = 0;
        public const int SEC_EMAIL = 1;
        public const int SEC_DATASOURCE = 2;
        public const int SEC_REPORT = 3;

        public const string XML_SECURITY = "ReportSmartSecurity";
        public const string XML_REPORTSEC = "ReportSecurity";
        public const string XML_DSSEC = "DatasourceSecurity";
        public const string XML_EMAILSEC = "EmailSecurity";

        public const string XMLa_FILE = "file";
        public const string XMLa_DATASOURCE = "datasource";
        public const string XMLa_USERID = "userid";
        public const string XMLa_PASSWORD = "passwd";
        public const string XMLa_AUTH = "auth";
        public const string XMLa_SERVER = "server";
        public const string XMLa_SSL = "ssl";

        public static int GetSecurityType(XmlNode aNode)
        {
            if (aNode.Name == XML_EMAILSEC) return SEC_EMAIL;
            if (aNode.Name == XML_REPORTSEC) return SEC_REPORT;
            if (aNode.Name == XML_DSSEC) return SEC_DATASOURCE;
            return SEC_UNKNOWN;
        }

        public CSecurityItem GetSecurityItem(XmlNode aNode)
        {
            switch (GetSecurityType(aNode))
            {
                case SEC_DATASOURCE:
                case SEC_REPORT:
                    return new CReportSecurityItem(this, aNode);

                case SEC_EMAIL:
                    return new Email.CEmailSecurityItem(this, aNode);

                default:
                    return null;
            }
        }

        private string _SecurityFile;
        private XmlDocument _SecurityDoc;
        private XmlNode _SecRoot;

        public XmlNode DataNode { get { return _SecRoot; } }

        public string SecurityFile { get { return _SecurityFile; } }

        public CSecurityItem this[int aIndex]
        {
            get { return GetSecurityItem(_SecRoot.ChildNodes[aIndex]); }
        }

        public int ItemCount { get { return _SecRoot.ChildNodes.Count; } }

        public void QuickSave()
        {
            _SecurityDoc.Save(_SecurityFile);
        }

        public static void BuildEmptySecurityFile(string aSecurityFile)
        {
            XmlDocument lEmptySecurity = XmlTools.CreateEmptyXML();
            XmlTools.CreateXmlNode(lEmptySecurity, XML_SECURITY, null);
            lEmptySecurity.Save(aSecurityFile);
        }

        public void AddReportSecurity(string aReportFile, string aDataSource, string aUserID, string aPassword)
        {
            XmlNode lNode = XmlTools.getXmlNodeByAttrVal(XML_REPORTSEC, XMLa_FILE, aReportFile, _SecRoot);

            if (lNode != null)
            {
                CReportSecurityItem lSec = new CReportSecurityItem(this, lNode);
                lSec.DataSource = aDataSource;
                lSec.UserName = aUserID;
                lSec.Password = aPassword;
                lSec.Authorize = true;
                lSec.PostSettings();

            }
            else
            {
                XmlNode lReportSecurity = XmlTools.CreateXmlNode(_SecurityDoc, XML_REPORTSEC, _SecRoot);

                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_AUTH, "yes");
                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_FILE, aReportFile);
                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_DATASOURCE, aDataSource);
                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_USERID, aUserID);
                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_PASSWORD, aPassword);

                _SecurityDoc.Save(_SecurityFile);
            }
        }

        public void AddReportSecurity(string aReportFile, string aDataSource)
        {
            XmlNode lNode = XmlTools.getXmlNodeByAttrVal(XML_REPORTSEC, XMLa_FILE, aReportFile, _SecRoot);

            if (lNode != null)
            {
                CReportSecurityItem lSec = new CReportSecurityItem(this, lNode);
                lSec.DataSource = aDataSource;
                lSec.UserName = "";
                lSec.Password = "";
                lSec.Authorize = false;
                lSec.PostSettings();

            }
            else
            {
                XmlNode lReportSecurity = XmlTools.CreateXmlNode(_SecurityDoc, XML_REPORTSEC, _SecRoot);

                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_AUTH, "no");
                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_FILE, aReportFile);
                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_DATASOURCE, aDataSource);
            }

            _SecurityDoc.Save(_SecurityFile);
        }

        public void AddDataSourceSecurity(string aDataSource, string aUserID, string aPassword)
        {
            XmlNode lNode = XmlTools.getXmlNodeByAttrVal(XML_DSSEC, XMLa_DATASOURCE, aDataSource, _SecRoot);

            if (lNode != null)
            {
                CReportSecurityItem lSec = new CReportSecurityItem(this, lNode);
                lSec.DataSource = aDataSource;
                lSec.UserName = aUserID;
                lSec.Password = aPassword;
                lSec.Authorize = true;
                lSec.PostSettings();

            }
            else
            {
                XmlNode lReportSecurity = XmlTools.CreateXmlNode(_SecurityDoc, XML_DSSEC, _SecRoot);

                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_AUTH, "yes");
                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_DATASOURCE, aDataSource);
                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_USERID, aUserID);
                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_PASSWORD, aPassword);

                _SecurityDoc.Save(_SecurityFile);
            }
        }

        public void AddDataSourceSecurity(string aDataSource)
        {
            XmlNode lNode = XmlTools.getXmlNodeByAttrVal(XML_DSSEC, XMLa_DATASOURCE, aDataSource, _SecRoot);

            if (lNode != null)
            {
                CReportSecurityItem lSec = new CReportSecurityItem(this, lNode);
                lSec.DataSource = aDataSource;
                lSec.UserName = "";
                lSec.Password = "";
                lSec.Authorize = false;

                lSec.PostSettings();

            }
            else
            {

                XmlNode lReportSecurity = XmlTools.CreateXmlNode(_SecurityDoc, XML_DSSEC, _SecRoot);

                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_AUTH, "no");
                XmlTools.AddNewAttr(_SecurityDoc, lReportSecurity, XMLa_DATASOURCE, aDataSource);

                _SecurityDoc.Save(_SecurityFile);
            }
        }

        public CReportSecurityItem GetReportSecurity(string aReport, string aDataSource)
        {
            aDataSource = aDataSource.ToUpper();
            aReport = aReport.ToUpper();

            foreach (XmlNode iNode in _SecRoot.ChildNodes)
            {
                if (iNode.Name == XML_DSSEC && XmlTools.GetAttrib(iNode, XMLa_DATASOURCE).ToUpper() == aDataSource)
                    return new CReportSecurityItem(this, iNode);
                else if (
                            iNode.Name == XML_REPORTSEC &&
                            XmlTools.GetAttrib(iNode, XMLa_FILE).ToUpper() == aReport &&
                            XmlTools.GetAttrib(iNode, XMLa_DATASOURCE).ToUpper() == aDataSource
                        )
                {
                    return new CReportSecurityItem(this, iNode);
                }
            }

            return null;
        }

        public Email.CEmailSecurityItem AddEmailSecurity(string aServer, string aUserName, string aPassword)
        {
            XmlNode lMailSecurity = XmlTools.CreateXmlNode(_SecurityDoc, XML_EMAILSEC, _SecRoot);

            XmlTools.AddNewAttr(_SecurityDoc, lMailSecurity, XMLa_SERVER, aServer);
            XmlTools.AddNewAttr(_SecurityDoc, lMailSecurity, XMLa_USERID, aUserName);
            XmlTools.AddNewAttr(_SecurityDoc, lMailSecurity, XMLa_PASSWORD, aPassword);

            _SecurityDoc.Save(_SecurityFile);

            return this.GetEmailSecurity(aServer);
        }

        public Email.CEmailSecurityItem GetEmailSecurity(string aID)
        {
            XmlNode lNode = XmlTools.getXmlNodeByAttrVal(XML_EMAILSEC, CXmlSettingsNode.XMLa_ID, aID, _SecRoot);
            return lNode == null ? null : new Email.CEmailSecurityItem(this, lNode);
        }

        public ArrayList GetEmailSecurityList()
        {
            ArrayList lResult = new ArrayList();

            foreach (XmlNode iNode in _SecRoot.ChildNodes)
            {
                if (iNode.Name.ToUpper() == XML_EMAILSEC.ToUpper())
                    lResult.Add(iNode);
            }

            return lResult;
        }

        public CSecurityManager(string aSecurityFile)
        {
            _SecurityFile = aSecurityFile;
            if (!File.Exists(_SecurityFile))
            {
                BuildEmptySecurityFile(_SecurityFile);
            }

            _SecurityDoc = new XmlDocument();
            _SecurityDoc.Load(_SecurityFile);

            _SecRoot = XmlTools.getXmlNodeByName(XML_SECURITY, _SecurityDoc);
        }
    }
}

namespace ReportSmart.Network.Email
{
    //[Serializable]
    //internal struct SEmailProfile {
    //		internal string ProfileName, Name, UserName, Password, EmailAddress, SMTPServer, Port;
    //	}

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public class SMapiMessage
    {
        public int reserved;
        public string subject;
        public string noteText;
        public string messageType;
        public string dateReceived;
        public string conversationID;
        public int flags;
        public IntPtr originator;
        public int recipCount;
        public IntPtr recips;
        public int fileCount;
        public IntPtr files;
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public class SMapiRecipDesc
    {
        internal int reserved;
        internal int recipClass;
        internal string name;
        internal string address;
        internal int eIDSize;
        internal IntPtr entryID;
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public class SMapiFileDesc
    {
        internal int reserved;
        internal int flags;
        internal int position;
        internal string path;
        internal string name;
        internal IntPtr type;
    }

    public enum TSMTPGroups
    {
        sgGlobal = 0,
        sgConnection = 1,
        sgPersonal = 2,
        sgSecurity = 3
    }

    public class CEmailSecurityItem : CSecurityItem
    {
        public string XMLa_SERVER = "server";
        public string XMLa_SSL = "ssl";

        public string Server
        {
            get { return this["ID"]; }
            set { this["ID"] = value; }
        }

        public bool SSL
        {
            get { return GetAsBool(XMLa_SSL); }
            set { SetAsBool(XMLa_SSL, value); }
        }

        public override string GetServiceName() { return Server; }
        public override void SetServiceName(string aName) { Server = aName; }

        public CEmailSecurityItem(CSecurityManager aSecurity, bool aIsNew) : base(aSecurity, CSecurityManager.XML_EMAILSEC, aSecurity.DataNode, aIsNew) { }
        public CEmailSecurityItem(CSecurityManager aSecurity, string aID, bool aIsNew) : base(aSecurity, CSecurityManager.XML_EMAILSEC, aID, aSecurity.DataNode, aIsNew) { }
        public CEmailSecurityItem(CSecurityManager aSecurity, XmlNode aNode) : base(aSecurity, aNode) { }
    }

    public class CEmailProfile : CXmlSettingsContainer
    {
        public const string XML_SMPTPROFILE = "profile";
        public const string XML_PERSONAL = "personality";
        public const string XML_CONNECTION = "connection";

        public const string XMLa_MAIL = "email";
        public const string XMLa_NAME = "name";
        public const string XMLa_PORT = "port";
        public const string XMLa_SERVER = "server";
        public const string XMLa_AUTH = "auth";
        public const string XMLa_SSL = "SSL";

        private CEmailSecurityItem _Security;
        private CXmlSettingsNode _Personal = null, _Connection = null;
        private CEmailProfileManager _ProfileManager;

        public CEmailProfileManager ProfileManager { get { return _ProfileManager; } }

        public override string ToString()
        {
            return this.ProfileName;
        }

        public CEmailSecurityItem Security
        {
            get
            {
                if (_Security == null)
                    initSecurity();
                return _Security;
            }
        }

        public override string NodeID
        {
            get { return base.NodeID; }
            set
            {
                base.NodeID = value;
                if (_Security == null && ProfileManager != null) initSecurity();
            }
        }

        public string ProfileName
        {
            get { return this[XMLa_NAME]; }
            set { this[XMLa_NAME] = value; }
        }

        public string SMTPServer
        {
            get { return _Connection[XMLa_SERVER]; }
            set { _Connection[XMLa_SERVER] = value; }
        }

        public string SMTPPort
        {
            get { return _Connection[XMLa_PORT]; }
            set { _Connection[XMLa_PORT] = value; }
        }

        public string SenderName
        {
            get { return _Personal[XMLa_NAME]; }
            set { _Personal[XMLa_NAME] = value; }
        }

        public string SenderMail
        {
            get { return _Personal[XMLa_MAIL]; }
            set { _Personal[XMLa_MAIL] = value; }
        }

        public string UserName
        {
            get { return Security.UserName; }
            set { Security.UserName = value; }
        }

        public string Password
        {
            get { return Security.Password; }
            set { Security.Password = value; }
        }

        public bool SSL
        {
            get { return Security.SSL; }
            set { Security.SSL = value; }
        }

        public bool Authorize
        {
            get { return _Connection.GetAsBool(XMLa_AUTH); }
            set { _Connection.SetAsBool(XMLa_AUTH, value); }
        }

        public override bool CheckIntegrity()
        {
            bool lResult = XmlTools.getXmlNodeByName(XML_CONNECTION, DataNode) != null;
            lResult &= XmlTools.getXmlNodeByName(XML_PERSONAL, DataNode) != null;

            return lResult;
        }

        public override void Release()
        {
            ProfileManager.Profiles.Remove(this);
            Security.Release();
            base.Release();
            ProfileManager.ScanForProfiles();
        }

        protected override void buildDefaultStructure()
        {
            _Connection = new CXmlSettingsNode(XML_CONNECTION, DataNode, true);
            _Personal = new CXmlSettingsNode(XML_PERSONAL, DataNode, true);
        }

        protected override void initialize()
        {
            base.initialize();

            _Connection = _Connection == null ? new CXmlSettingsNode(XML_CONNECTION, DataNode, false) : _Connection;
            _Personal = _Personal == null ? new CXmlSettingsNode(XML_PERSONAL, DataNode, false) : _Personal;
        }

        protected void initSecurity()
        {
            if (ProfileManager == null)
                MessageBox.Show("Nincs profil menedzser!");

            if (ProfileManager.Security == null)
                MessageBox.Show("Nincs biztonság menedzser!");

            _Security = new CEmailSecurityItem(ProfileManager.Security, base.NodeID, false);
        }

        public CEmailProfile(CEmailProfileManager aManager, bool aIsNew) : base(XML_SMPTPROFILE, aManager.DataNode, aIsNew)
        { _ProfileManager = aManager; }

        public CEmailProfile(CEmailProfileManager aManager, string aID, bool aIsNew) : base(XML_SMPTPROFILE, aID, aManager.DataNode, aIsNew)
        { _ProfileManager = aManager; }

        public CEmailProfile(CEmailProfileManager aManager, XmlNode aNode) : base(aNode)
        { _ProfileManager = aManager; }
    }

    public class CEmailProfileManager : CXmlSettingsNode
    {
        public const int DEFAULT_PORT = 25;
        public const string XML_MAILPROFILES = "emailProfiles";

        private CXmlSettingsList _Profiles;
        private CSecurityManager _SecMan;

        protected override void initialize()
        {
            base.initialize();

            _Profiles = new CXmlSettingsList();
            ScanForProfiles();
        }

        public CXmlSettingsList Profiles { get { return _Profiles; } }

        public CSecurityManager Security { get { return _SecMan; } }

        public void ScanForProfiles()
        {
            foreach (XmlNode aNode in DataNode.ChildNodes)
            {
                if (aNode.Name.ToUpper() == CEmailProfile.XML_SMPTPROFILE.ToUpper())
                {
                    CEmailProfile lProfile = new CEmailProfile(this, aNode);
                    if (IndexOf(lProfile) < 0)
                        this.AddProfile(lProfile);
                }
            }
        }

        public int IndexOf(CEmailProfile aProfile)
        {
            return IndexOf(aProfile.NodeID);
        }

        public int IndexOf(string aNodeID)
        {
            CEmailProfile iProfile;
            int lResult = -1;
            aNodeID = aNodeID.ToUpper();

            for (int i = 0; i < _Profiles.Count; i++)
            {
                iProfile = ((CEmailProfile)_Profiles[i]);
                if (iProfile.NodeID.ToUpper() == aNodeID)
                {
                    lResult = i;
                    break;
                }
            }

            return lResult;
        }

        public void AddProfile(CEmailProfile aProfile)
        {
            if (IndexOf(aProfile) < 0)
                _Profiles.Add(aProfile);
        }

        public CEmailProfile CreateNewProfile(string aProfileName, string aSMTP_Server)
        {
            string lNewId = CXmlSettingsNode.ValidateNewID(aSMTP_Server, DataNode);

            CEmailProfile lResult = new CEmailProfile(this, lNewId, true);

            lResult.ProfileName = aProfileName;
            lResult.SMTPServer = aSMTP_Server;
            lResult.SMTPPort = DEFAULT_PORT.ToString();
            lResult.SenderName = "";
            lResult.SenderMail = "";

            this._Profiles.Add(lResult);

            return lResult;
        }

        public CEmailProfileManager(CSecurityManager aManager, XmlNode aParentNode, bool aIsNew) : base(XML_MAILPROFILES, aParentNode, aIsNew)
        { _SecMan = aManager; }

        public CEmailProfileManager(CSecurityManager aManager, string aID, XmlNode aParentNode, bool aIsNew) : base(XML_MAILPROFILES, aID, aParentNode, aIsNew)
        { _SecMan = aManager; }

        public CEmailProfileManager(CSecurityManager aManager, XmlNode aNode) : base(aNode)
        { _SecMan = aManager; }
    }

    public class CMapiMailAttachment
    {
        public const int MAPI_MAXATTACHMENT = 20;

        private ArrayList _Attachments;

        public string this[int aIndex]
        {
            get { return (string)(_Attachments[aIndex]); }
            set { _Attachments[aIndex] = value; }
        }
        public int Count
        {
            get { return _Attachments.Count; }
        }

        public static CMapiMailAttachment operator +(CMapiMailAttachment aObject, string aNewRecipient)
        {
            aObject.Add(aNewRecipient);
            return aObject;
        }

        public static CMapiMailAttachment operator -(CMapiMailAttachment aObject, string aRecipient)
        {
            aObject.Remove(aRecipient);
            return aObject;
        }

        protected internal IntPtr getAttachments(out int aAttachmentCount)
        {
            aAttachmentCount = 0;
            if (_Attachments.Count == 0)
                return IntPtr.Zero;

            if ((_Attachments.Count <= 0) || (_Attachments.Count > MAPI_MAXATTACHMENT))
                return IntPtr.Zero;

            int lLen = Marshal.SizeOf(typeof(SMapiFileDesc));
            IntPtr lResult = Marshal.AllocHGlobal(_Attachments.Count * lLen);

            SMapiFileDesc lMapiFile = new SMapiFileDesc();
            lMapiFile.position = -1;
            int lPtr = (int)lResult;

            foreach (object iItem in _Attachments)
            {
                lMapiFile.name = Path.GetFileName((string)iItem);
                lMapiFile.path = (string)iItem;

                Marshal.StructureToPtr(lMapiFile, (IntPtr)lPtr, false);
                lPtr += lLen;
            }

            aAttachmentCount = _Attachments.Count;
            return lResult;
        }

        public void Add(string aNewItem)
        {
            _Attachments.Add(aNewItem);
        }

        public void Remove(string aItem)
        {
            int lIndex;
            if ((lIndex = IndexOf(aItem)) > -1)
                _Attachments.RemoveAt(lIndex);
        }

        public int IndexOf(string aRecipient)
        {
            aRecipient = aRecipient.ToUpper();

            for (int i = 0; i < _Attachments.Count; i++)
                if (aRecipient == this[i].ToUpper())
                    return i;

            return -1;
        }

        public CMapiMailAttachment()
        {
            _Attachments = new ArrayList();
        }
    }

    public class CMapiMail
    {
        private ArrayList _To;
        private CMapiMailAttachment _Attachments;

        public const int MAPI_LOGON_UI = 0x0001;
        public const int MAPI_NEW_SESSION = 0x0002;
        public const int MAPI_DIALOG = 0x0008;

        public const int MAPI_ORIG = 0;
        public const int MAPI_TO = 1;
        public const int MAPI_CC = 2;
        public const int MAPI_BCC = 3;

        [DllImport("MAPI32.DLL")]
        public static extern int MAPISendMail(
                    IntPtr aSession,
                    IntPtr aHandle,
                    SMapiMessage aMessage,
                    int aFlags,
                    int aReserved
                );

        public string Body, Subject = "";
        public string this[int aIndex]
        {
            get { return (string)(_To[aIndex]); }
            set { _To[aIndex] = value; }
        }
        public int RecipientCount
        {
            get { return _To.Count; }
        }
        public CMapiMailAttachment Attachments
        {
            get { return _Attachments; }
            set { _Attachments = value; }
        }

        protected IntPtr getRecipients(out int aRecipCount)
        {
            aRecipCount = 0;
            if (_To.Count == 0)
                return IntPtr.Zero;


            int lLen = Marshal.SizeOf(typeof(SMapiRecipDesc));
            IntPtr lResult = Marshal.AllocHGlobal(_To.Count * lLen);

            int lPtr = (int)lResult;
            foreach (object iItem in _To)
            {
                string iRecip = (string)iItem;
                SMapiRecipDesc lRecip = new SMapiRecipDesc();

                lRecip.address = iRecip;
                lRecip.recipClass = iRecip == this[0] ? MAPI_TO : MAPI_CC;

                Marshal.StructureToPtr(lRecip, (IntPtr)lPtr, false);
                lPtr += lLen;
            }

            aRecipCount = _To.Count;
            return lResult;
        }

        public static CMapiMail operator +(CMapiMail aObject, string aNewRecipient)
        {
            aObject.Add(aNewRecipient);
            return aObject;
        }

        public static CMapiMail operator -(CMapiMail aObject, string aRecipient)
        {
            aObject.Remove(aRecipient);
            return aObject;
        }

        public void Add(string aNewItem)
        {
            _To.Add(aNewItem);
        }

        public void Remove(string aItem)
        {
            int lIndex;
            if ((lIndex = IndexOf(aItem)) > -1)
                _To.RemoveAt(lIndex);
        }

        public int IndexOf(string aRecipient)
        {
            aRecipient = aRecipient.ToUpper();

            for (int i = 0; i < _To.Count; i++)
                if (aRecipient == this[i].ToUpper())
                    return i;

            return -1;
        }

        public virtual void StartMail()
        {
            if (_To.Count == 0) _To.Add("");

            SMapiMessage lMessage = new SMapiMessage();

            lMessage.subject = Subject;
            lMessage.noteText = Body;
            lMessage.recips = getRecipients(out lMessage.recipCount);
            lMessage.files = Attachments.getAttachments(out lMessage.fileCount);

            MAPISendMail(new IntPtr(0), new IntPtr(0), lMessage, MAPI_DIALOG, 0);
        }

        public CMapiMail()
        {
            _To = new ArrayList();
            _Attachments = new CMapiMailAttachment();
            Body = "";
        }
    }

    public class CEmailing
    {
        public static void BugReport(string aMessage)
        {
            SmtpClient lSMTP = new SmtpClient("smtp.gmail.com");
            MailMessage lMessage = new MailMessage();
            lMessage.From = new MailAddress("hstudio.sender@gmail.com");
            lMessage.To.Add("support.reportsmart@gmail.com");
            lMessage.Subject = "BUG REPORT";
            lMessage.BodyEncoding = new UTF8Encoding();
            lMessage.Body = aMessage;
            lSMTP.Port = 587;
            lSMTP.EnableSsl = true;
            lSMTP.Credentials = new NetworkCredential("hstudio.sender@gmail.com", "reportsmart");
            lSMTP.Send(lMessage);
        }
    }
}