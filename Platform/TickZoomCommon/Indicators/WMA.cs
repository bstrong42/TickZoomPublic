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
using System.IO;

using TickZoom.Api;

namespace TickZoom.Common
{
	/// <summary>
	/// The Weighted Moving Average indicator weights the more recent value greater
	/// that prior value. For 5 bar WMA the current bar is multiplied by 5, the previous
	/// one by 4, and so on, down to 1 for the final value. WMA divides the result by the
	/// total of the multipliers for the average.
	/// </summary>
	public class WMA : IndicatorCommon
	{
		int period;
		Doubles price;
		object anyPrice;
		
		public WMA() : this( null, 5)
		{
			
		}
		
		public WMA(object anyPrice, int period)
		{
			this.anyPrice = anyPrice;
			this.period = period;
			Drawing.PaneType = PaneType.Secondary;
		}
		
		public override bool OnIntervalClose() {
			if( anyPrice == null) {
				price = Doubles(Bars.Close);
			} else {
				price = Doubles(anyPrice);
			}
			double sum = 0;
			int count = 0;
			for( int i = 0; i< period; i++) {
				int mult = period - i;
				sum += price[i] * (period - i);
				count += period - i;
			}
			this[0] = sum / count;
			return true;
		}
		
		public int Period {
			get { return period; }
			set { period = value; }
		}
	}
}
