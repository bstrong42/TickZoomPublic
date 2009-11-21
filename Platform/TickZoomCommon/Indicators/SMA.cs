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
	/// Description of SMA.
	/// </summary>
	public class SMA : IndicatorCommon
	{
		int period;
		Doubles price;
		object anyPrice;
		Interval fastInterval = Intervals.Year1;
		
		public SMA(object anyPrice, int period)
		{
			this.anyPrice = anyPrice;
			this.period = period;
			RequestUpdate(fastInterval);
		}
		
		public override void OnInitialize()
		{
			if( anyPrice == null) {
				price = Doubles(Bars.Close);
			} else {
				price = Doubles(anyPrice);
			}
		}
		
		public override bool OnProcessTick(Tick tick)
		{
			if( Chart.IsDynamicUpdate) {
				Update();
			}
			return true;
		}

		public void Reset() {
			if( Count > 1) {
				double level = price[0];
				for( int i=1; i<price.Count && i<period; i++) {
					price[i] = (int) level;
				}
				this[1] = this[0] = level;
			}
		}
		
		public override bool OnIntervalClose(Interval period)
		{
			if( period.Equals(fastInterval) && price.Count > 0) {
				Update();
			}
			return true;
		}
		
		public override bool OnIntervalClose() {
			Update();
			return true;
		}
		
		public override void Update() {
			if (double.IsNaN(this[0])) {
				this[0] = price[0];
			} else {
				
				double last = this[1];
				double sum = last * Math.Min(Count-1, period);

				if (Count > period && price.BarCount > period) {
					double b = price[0];
					double p = price[period];
					int d = Math.Min(Count, period);
					double x = (sum + price[0] - price[period]) / Math.Min(Count, period);
					this[0] = x;
					if( IsTrace) Log.Trace(Name+": Count="+Count+",price.BarCount="+price.BarCount+",last="+last+",sum="+sum+",b="+price[0]+",price["+period+"]="+price[period]+",d="+d+",x="+x+",this[0]="+this[0]);
				} else if( price.BarCount > 0 ) {
					this[0] = (sum + price[0]) / (Math.Min(Count, period));
					if( IsTrace) Log.Trace(Name+": Count="+Count+",price.BarCount="+price.BarCount+",last="+last+",sum="+sum+",b="+price[0]+",this[0]="+this[0]);
				}
			}
		}
		
		public int Period {
			get { return period; }
			set { period = value; }
		}
	}
}
