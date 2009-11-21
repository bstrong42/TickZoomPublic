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
	/// Description of Trade.
	/// </summary>
	public class ComboTradex 
	{
//	
//		public ComboTrade()
//		{
//		}
//		
//		public ComboTrade(double slippage, double commission) : base( slippage, commission)
//		{
//		}
//		
//		public ComboTrade(RoundTurn other) : base( other) {
//			ComboTrade otherCombo = other as ComboTrade;
//			averagePrice = otherCombo.averagePrice;
//		}
//		
//		public override double ProfitInPosition( double price) {
//			if( Direction == 0) {
//				throw new ApplicationException("Direction not set for profit loss calculation.");
//			}
//			return ((price - averagePrice) * ConversionFactor * Direction) - TransactionCosts;
//		}
//		
//		public void ChangeSize( double newSize, double price) {
//			double sizeChange = newSize - Direction;
//			double sum = averagePrice * Direction;
//			sum += sizeChange * price;
//			averagePrice = sum / newSize;
//			Direction = newSize;
//		}
//		
//		public override double EntryPrice {
//			get { return base.EntryPrice; }
//			set { base.EntryPrice = value;
//			      averagePrice = value; }
//		}
//		
//		public double AveragePrice {
//			get { return averagePrice; }
//		}
	}
}
