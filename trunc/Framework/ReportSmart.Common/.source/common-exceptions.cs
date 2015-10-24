#region Source information

//*****************************************************************************
//
//    common-exceptions.cs
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

namespace CommonClasses
{

    internal class EError : System.Exception
    {
        private string _Message;
        private Object _Sender;
        private bool _KnowSender;

        public bool KnowObject { get { return _KnowSender; } }
        public Object Sender { get { return _Sender; } }

        public EError()
        {
            _Message = DefaultMessage();
            _KnowSender = false;
        }

        public EError(string aMessage)
        {
            _Message = aMessage;
            _KnowSender = false;
        }

        public EError(string aMessage, Object aSender)
        {
            _Message = aMessage;
            _Sender = aSender;
            _KnowSender = true;
        }

        protected virtual string DefaultMessage()
        {
            return "Unknown exception from an unknown class.";
        }

    }
}
