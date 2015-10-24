/*
 *
 * Licensing:			GPL
 * Original project:	cs.RSDMLEdit.csproj
 *
 * Copyright: Adam Halassy (2010.06.13.)
 * 
 * 
 */

namespace ReportSmart.Engine {
		internal enum TRSVarType {
				rsvtString,
				rsvtUInt,
				rsvtInt,
				rsvtFloat,
				rsvtBool,
				rsvtObject	
			}
				
		internal class CRSVariable {
				private string _val;
				private bool _null = true;
				private TRSVarType _type;
				
				public uint UIntValue {
						get { return uint.Parse(_val); }
						set { _val = value.ToString(); _type = TRSVarType.rsvtUInt; }
					}
				public int IntValue {
						get { return int.Parse(_val); }
						set { _val = value.ToString(); _type = TRSVarType.rsvtInt; }						
					}
				public float FloatValue {
						get { return float.Parse(_val); }
						set { _val = value.ToString(); _type = TRSVarType.rsvtFloat; }
					}
				//public bool BoolValue {
						
				//	}
					
				public bool IsNull() { return _null; }
				public bool IsNumber() {
						return _type == TRSVarType.rsvtFloat ||
									_type == TRSVarType.rsvtInt ||
									_type == TRSVarType.rsvtUInt;
					}
				
				public CRSVariable () {
						_null = true;
						_val = "";
					}
			}

		internal class CRSDMLInterpreter {
			}
	}