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
using TickZoom.Api;

namespace TickZoom.Common
{
	[Obsolete("Please use TransactionPairsBinary instead.")]
	public class RoundTurnsBinary : TransactionPairsBinary
	{
		
	}
	
	public class TransactionPairsBinary
	{
		string name = "TradeList";
		List<TransactionPairBinary> transactionPairs = new List<TransactionPairBinary>();
		int totalProfit = 0;
		public TransactionPairsBinary()
		{
		}
		
		public void Add(TransactionPairBinary trade) {
			CalcTotalProfit();
			transactionPairs.Add(trade);
		}
		public TransactionPairsBinary GetCompletedList(TimeStamp time, double price, int bar) {
			if( transactionPairs.Count > 0) {
				TransactionPairBinary pair = transactionPairs[transactionPairs.Count-1];
				if( !pair.Completed ) {
					pair.ExitPrice = price;
					pair.ExitTime = time;
					pair.ExitBar = bar;
					transactionPairs[transactionPairs.Count-1] = pair;
				}
			}
			return this;
		}
		
		public TransactionPairBinary Current {
			get { return transactionPairs[transactionPairs.Count-1]; }
			set { transactionPairs[transactionPairs.Count-1] = value; }
		}
		
		public int Count { 
			get { return transactionPairs.Count; }
		}
		
		public TransactionPairBinary this [int index] {
		    get { 
				return transactionPairs[index];
			}
		    set { transactionPairs[index] = value;  }
		}
		private void CalcTotalProfit() {
			totalProfit = 0;
		}
		public int TotalProfit {
			get { return totalProfit; }
		}
		
		public override string ToString()
		{
			if( name != "") {
				return base.ToString() + ": " + name;
			} else {
				return base.ToString();
			}
			
		}
		
		public string Name {
			get { return name; }
			set { name = value; }
		}
	}
}
