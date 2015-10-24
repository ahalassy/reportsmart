/*
 *2009-09-14 (Adam ReportSmart)
 * 
 */
 
using System;

namespace ReportSmart {
		// Manage application version like "#1.#2.x.#3" (eg.: 0.1.a.0)
		// #1: Major
		// #2: Minor
		// x: Alpha/Beta/RC/RTM (a/b/rc/rtm)
		// #3: Release
		public class CApplicationVersion: IComparable {
				public const byte ALPHA = 0;
				public const byte BETA = 1;
				public const byte RELEASECANDIDATE = 2;
				public const byte RELEASETOMANUFACTURE = 3;
		
				public const string sALPHA = "a";
				public const string sBETA = "b";
				public const string sRELEASECANDIDATE = "rc";
				public const string sRELEASETOMANUFACTURE = "rtm";
				
				public byte Major, Minor, Release, State;
				
				public static CApplicationVersion StringToApplicationVersion(string aVerString) {
						string lCode;
						CApplicationVersion lResult = new CApplicationVersion();
						
						//Decode Major:
						lResult.Major = byte.Parse(aVerString.Substring(0, aVerString.IndexOf('.')));
						aVerString = aVerString.Substring(aVerString.IndexOf('.') + 1);
						
						//Decode Minor:
						lResult.Minor = byte.Parse(aVerString.Substring(0, aVerString.IndexOf('.')));
						aVerString = aVerString.Substring(aVerString.IndexOf('.') + 1);
						
						//Decode State:
						lCode = aVerString.Substring(0, aVerString.IndexOf('.'));
						lCode = lCode.ToLower();
						if (lCode == sALPHA)
								lResult.State = ALPHA;
							else if (lCode == sBETA)
								lResult.State = BETA;
							else if (lCode == sRELEASECANDIDATE)
								lResult.State = RELEASECANDIDATE;
							else if (lCode == sRELEASETOMANUFACTURE)
								lResult.State = RELEASETOMANUFACTURE;
							else
								lResult.State = ALPHA;
								
						aVerString = aVerString.Substring(aVerString.IndexOf('.') + 1);
						lResult.Release = byte.Parse(aVerString);
						
						return lResult;
					}
				
				public CApplicationVersion() {
						Major = 0;
						Minor = 1;
						State = ALPHA;
						Release = 0;
					}
					
				public override string ToString() {
						string lResult = Major + "." + Minor;
						switch (State) {
								case ALPHA: lResult += ".a."; break;
								case BETA: lResult += ".b."; break;
								case RELEASECANDIDATE: lResult += ".rc."; break;
								case RELEASETOMANUFACTURE: lResult += ".rtm."; break;
								default: lResult += ".a."; break;
							}
						return lResult + Release;
					}
			
				public int CompareTo(object aObj) {
						if (aObj is CApplicationVersion) {
							 	return this.ToString().CompareTo(((CApplicationVersion)aObj).ToString());
							} else {
								return -1;
							}
					}
			}
	}
