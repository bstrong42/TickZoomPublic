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
using TickZoom.Api;

namespace TickZoom.Common
{
	/// <summary>
	/// Description of TEMA.
	/// </summary>
	public class TEMA : IndicatorCommon
	{
		EMA E1;
		EMA E2;
		EMA E3;
		int period = 14;
		Doubles price;
		object anyPrice;
		
		public TEMA( ) : this( null, 5) {
			
		}
		
		public TEMA( object anyPrice, int period) {
			this.anyPrice = anyPrice;
			this.period = period;
			RequestUpdate(Intervals.Second10);
		}

		public override void OnInitialize() {
			if( anyPrice == null) {
				price = Doubles(Bars.Close);
			} else {
				price = Doubles(anyPrice);
			}
			E1 = new EMA(period);
			E2 = new EMA(period);
			E3 = new EMA(period);
			AddIndicator(E1);
			AddIndicator(E2);
			AddIndicator(E3);
		}
		
		public override bool OnProcessTick(Tick tick)
		{
			if( Chart.IsDynamicUpdate) {
				UpdateAverage();
			}
			return true;
		}
		
		public void Reset() {
			double close = Bars.Close[0];
			this[0] = close;
			E1.Reset(close);
			E2.Reset(close);
			E3.Reset(close);
		}
		
		public override bool OnIntervalClose(Interval interval)
		{
			if( interval.Equals(Intervals.Second10)) {
				UpdateAverage();
			}
			return true;
		}
		
		public override bool OnIntervalClose() {
			UpdateAverage();
			return true;
		}
		
		private void UpdateAverage() {
			double close = price[0];
			E1.Set(close);
			E2.Set(E1[0]);
			E3.Set(E2[0]);
			double e1 = E1[0];
			double e2 = E2[0];
			double e3 = E3[0];
			double x = (3 * E1[0] - 3 * E2[0] + E3[0]);
			this[0] = x;
		}

		public int Period {
			get { return period; }
			set { period = value; }
		}
	}
}
