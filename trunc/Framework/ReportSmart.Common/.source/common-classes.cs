/*
 * 
 * ${res:XML.StandardHeader.UserName} Sx Xavier
 * Dátum: 2009-01-19@nb-adam
 * Ido: 19:33
 * 
 * 
 */

using System;
using Halassy;

namespace Halassy.Classes {
		internal interface IItemClass {
				string toString();

				string Name {get;}
				string InstanceName{get; }
				
				int compareWith(IItemClass aItem);
			}
	}

