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

using TickZoom.Api;

namespace TickZoom.Common
{
	/// <summary>
	/// Description of StrategyManager.
	/// </summary>
	[Obsolete("Please, use PortfolioCommon instead.",true)]
	public class MultiStrategy : StrategyCommon, Portfolio
	{
		List<StrategyCommon> strategies = new List<StrategyCommon>();
		
		public MultiStrategy()
		{
		    Performance.GraphTrades = false;
		}
	
		public sealed override void OnBeforeInitialize() {
			for( int i=0; i<Chain.Dependencies.Count; i++) {
				Chain chain = Chain.Dependencies[i];
				StrategyCommon strategy = null;
				for( Chain link = chain.Tail; link.Model != null; link = link.Previous) {
					strategy = link.Model as StrategyCommon;
					if( strategy != null) {
						break;
					}
				}
				if( strategy != null) {
					strategies.Add(strategy);
				}
				chain = chain.Next;
			}
		}	
	
		public override void OnInitialize()
		{
		}
		
		public override bool OnProcessTick(Tick tick)
		{
			double internalSignal = 0;
			for( int i=0; i < Chain.Dependencies.Count; i++) {
				PerformanceCommon performance = Chain.Dependencies[i].Model as PerformanceCommon;
				if( performance != null) {
					internalSignal += performance.Position.Signal;
				}
			}
			Position.Signal = internalSignal;
			return true;
		}
		
		/// <summary>
		/// Shortcut to look at the data of and control any dependant strategies.
		/// </summary>
		public List<StrategyCommon> Strategies {
			get { return strategies; }
		}
		
		public List<StrategyCommon> Market {
			get { return strategies; }
		}
	}
}
