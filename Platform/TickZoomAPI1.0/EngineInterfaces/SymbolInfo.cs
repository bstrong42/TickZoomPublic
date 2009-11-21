#region Copyright
/*
 * Copyright 2008 M. Wayne Walter
 * Software: TickZoom Trading Platform
 * User: Wayne Walter
 * 
 * You can use and modify this software under the terms of the
 * TickZOOM General Public License Version 1.0 or (at your option)
 * any later version.
 * 
 * Businesses are restricted to 30 days of use.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * TickZOOM General Public License for more details.
 *
 * You should have received a copy of the TickZOOM General Public
 * License along with this program.  If not, see
 * 
 * 
 *
 * User: Wayne Walter
 * Date: 4/2/2009
 * Time: 8:16 AM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.Collections.Generic;

namespace TickZoom.Api
{
	public interface SymbolInfo
	{
		string Symbol {
			get;
		}
	
		SymbolInfo UniversalSymbol {
			get;
		}
	
		ulong BinaryIdentifier {
			get;
		}
		
//		[Obsolete("Please use the symbol dictionary to set this value.")]
		Elapsed SessionStart {
			get;
			set;
		}
		
//		[Obsolete("Please use the symbol dictionary to set this value.")]
		Elapsed SessionEnd {
			get;
			set;
		}
		
		double MinimumTick {
			get;
		}
		
		double FullPointValue {
			get;
		}
		
		void CopyProperties(object obj);
	
		[Obsolete("Please create your data with the IsSimulateTicks flag set to true instead of this property.",true)]
		bool SimulateTicks {
			get;
		}
		
		int Level2LotSize {
			get;
		}
		
		double Level2Increment {
			get;
		}
		
		int Level2LotSizeMinimum {
			get;
		}
	}
}