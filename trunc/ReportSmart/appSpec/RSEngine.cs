/*
 *
 * Licensing:			GPL
 * Original project:	edit.csproj
 *
 * Copyright: Adam Halassy (2010.06.13.)
 * 
 * 
 */
 
using System;

namespace ReportSmart.Engine {
		internal class ERSException: Exception {
				private int _code = RSDMLEngine.RSR_OK;
			
				public int ErrorCode { get { return _code; } }
				
				protected void setErrorCode(int aCode) { _code = aCode;	}
					
				public ERSException(string aMsg, int aCode): base(aMsg) { _code = aCode; }
			}
			
		internal class ERSConversionException: ERSException {
				private string _faultValue;
				
				public string FaultValue { get { return _faultValue; }}
				
				public ERSConversionException(string aMsg, string aFaultVal): base(aMsg, RSDMLEngine.RSR_CONVERSION) {
						_faultValue = aFaultVal;
					}
			}
			
		internal class ERSConverBoolException: ERSConversionException {
				public ERSConverBoolException(string aMsg, string aFaultVal):
					base(aMsg, aFaultVal) {
						setErrorCode(RSDMLEngine.RSR_CONVERT_BOOL);
					}
			}
			
		internal static class RSDMLEngine {		
				private const string __DEF_STRTRUE = "true";
				private const string __DEF_STRFALSE = "false";
				
				/* Error codes: */
				public const int RSR_OK					= 0x00000000;
				public const int RSR_CONVERSION 		= 0x10000000;
				public const int RSR_CONVERT_BOOL 		= 0x10000001;
				
		
				private static string _strTrue = __DEF_STRTRUE;
				private static string _strFalse = __DEF_STRFALSE;
				//private static int _lastError = RSR_OK;
		
				public static void SetBoolStr(string aTrue, string aFalse) {
						_strFalse = aFalse == "" ? __DEF_STRFALSE : aFalse;
						_strTrue = aTrue == "" ? __DEF_STRTRUE : aTrue;
					}
					
				public static bool ParseAsBool(string aVal) {
						if (aVal.ToUpper() == _strTrue.ToUpper())
								return true;
							else if (aVal.ToUpper() == _strFalse.ToUpper())
								return false;
							else {
								throw new ERSConverBoolException("Value is not a valid bool value.", aVal);
							}
					}
			}

	}