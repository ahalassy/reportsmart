/*
 * 
 * Sx Xavier
 * 2009-01-25@nb-adam
 * 
 * 
 * 
 */
 
 using System;
 
 namespace CommonClasses {

	internal class EError: System.Exception {
			private string _Message;
			private Object _Sender;
			private bool _KnowSender;
	
			public bool KnowObject { get { return _KnowSender; } }
			public Object Sender { get {return _Sender; } }
			
			public EError() {
					_Message = DefaultMessage();
					_KnowSender = false;
				}
				
			public EError(string aMessage) {
					_Message = aMessage;
					_KnowSender = false;
				}
				
			public EError(string aMessage, Object aSender) {
					_Message = aMessage;
					_Sender = aSender;
					_KnowSender = true;
				}
				
			protected virtual string DefaultMessage() {
					return "Unknown exception from an unknown class.";
				}
			
		}
	}
