#region Copyright
/*
 * Software: TickZoom Trading Platform
 * Copyright 2009 M. Wayne Walter
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, see <http://www.tickzoom.org/wiki/Licenses>
 * or write to Free Software Foundation, Inc., 51 Franklin Street,
 * Fifth Floor, Boston, MA  02110-1301, USA.
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TickZoom.Api
{
	/// <summary>
	/// Description of Bars.
	/// </summary>
	public interface Data
	{
		double MinimumTick {
			get;
		}
		
		int PricePrecision {
			get;
		}
		
		Ticks Ticks {
			get;
		}
		
		LogicalOrder CreateOrder();
		
		Bars Get(Interval interval);
		
		/// <summary>
		/// Used to convert profits and losses from points to currency amount.
		/// The RoundTurn class uses the formula: Point profits or loss times position size times conversion factor.
		/// </summary>
		[Obsolete("Please use FullPointValue instead.",true)]
		double ConversionFactor {
			get;
		}
		
		double FullPointValue {
			get;
		}
		
		[Obsolete("Please create your data with the IsSimulateTicks flag set to true instead of this property.",true)]
		bool SimulateTicks {
			get;
		}
		
		string Symbol {
			get;
		}
		
		SymbolInfo UniversalSymbol {
			get;
		}
	}
}
