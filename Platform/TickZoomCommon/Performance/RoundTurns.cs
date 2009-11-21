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
using System.Collections;
using TickZoom.Api;

namespace TickZoom.Common
{
	[Obsolete("Please use TransactionPairs collection instead.")]
	public class RoundTurns : TransactionPairs {
		public RoundTurns(ProfitLoss pnl) : base(pnl)
		{
		}
		public RoundTurns(ProfitLoss pnl,TransactionPairsBinary pairs) : base(pnl,pairs)
		{
		}
	}
	public class TransactionPairs : IEnumerable
	{
		string name = "TradeList";
		TransactionPairsBinary transactionPairs;
		int totalProfit = 0;
		ProfitLoss profitLossCalculation;
		
		public TransactionPairs(ProfitLoss pnl)
		{
			profitLossCalculation = pnl;
			this.transactionPairs = new TransactionPairsBinary();
		}
		public TransactionPairs(ProfitLoss pnl,TransactionPairsBinary transactionPairs)
		{
			profitLossCalculation = pnl;
			this.transactionPairs = transactionPairs;
		}
		
		public int Current {
			get {
				return transactionPairs.Count - 1;
			}
		}
		
		public int Count { 
			get { return transactionPairs.Count; }
		}
		
		public double CurrentProfitLoss {
			get { return Count == 0 ? 0 : CalcProfitLoss(Current); }
		}
		
		public double CalcProfitLoss(int index) {
			double value = ProfitInPosition(index,transactionPairs[index].ExitPrice);
			return value;
		}
		
		public double CalcMaxAdverse(int index) {
			double value = ProfitInPosition(index,transactionPairs[index].MaxPrice);
			return Math.Round(value,3);
		}
		
		public double CalcMaxFavorable(int index) {
			double value = ProfitInPosition(index,transactionPairs[index].MaxPrice);
			return Math.Round(value,3);
		}
		
		public double ProfitInPosition( int index, Tick tick) {
			TransactionPairBinary binary = transactionPairs[index];
			double price = 0;
			if( binary.Direction > 0) {
				price = tick.Ask;	
			} else if( binary.Direction < 0) {
				price = tick.Bid;
			}
			return ProfitInPosition(index,price);
		}
		
		public double ProfitInPosition( int index, double price) {
			TransactionPairBinary binary = transactionPairs[index];
			if( binary.Direction == 0) {
				throw new ApplicationException("Direction not set for profit loss calculation.");
			}
			return profitLossCalculation.CalculateProfit( binary.Direction, binary.EntryPrice, price);
		}
		
		public TransactionPair this [int index] {
			get { return new TransactionPairImpl(transactionPairs[index]);  }
		}
		
		public TransactionPairBinary GetBinary(int index) {
			return transactionPairs[index];
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
		
		public ProfitLoss ProfitLossCalculation {
			get { return profitLossCalculation; }
			set { profitLossCalculation = value; }
		}
		
		public IEnumerator GetEnumerator()
		{
			for( int i=0; i<transactionPairs.Count; i++) {
				TransactionPairBinary binary = transactionPairs[i];
				yield return binary;
			}
		}
	}
}
