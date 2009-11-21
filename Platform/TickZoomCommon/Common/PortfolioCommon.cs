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
	public class PortfolioCommon : StrategyCommon, PortfolioInterface
	{
		List<StrategyCommon> strategies = new List<StrategyCommon>();
		PortfolioType portfolioType = PortfolioType.None;
		double closedEquity = 0;
		double openEquity;
		
		public PortfolioCommon()
		{
		    Performance.GraphTrades = false;
		}
	
		public sealed override void OnBeforeInitialize() {
			do {
				// Count all the unique symbols used by dependencies and
				// get a list of all the strategies.
				strategies = new List<StrategyCommon>();
				Dictionary<string,List<StrategyCommon>> symbolMap = new Dictionary<string,List<StrategyCommon>>();
				for( int i=0; i<Chain.Dependencies.Count; i++) {
					Chain chain = Chain.Dependencies[i];
					StrategyCommon strategy = null;
					for( Chain link = chain.Tail; link.Model != null; link = link.Previous) {
						strategy = link.Model as StrategyCommon;
						if( strategy != null) {
							break;
						}
						chain = chain.Next;
					}
					if( strategy != null) {
						List<StrategyCommon> tempStrategies;
						if( symbolMap.TryGetValue(strategy.SymbolDefault,out tempStrategies)) {
							tempStrategies.Add(strategy);
						} else {
							tempStrategies = new List<StrategyCommon>();
							tempStrategies.Add(strategy);
							symbolMap[strategy.SymbolDefault] = tempStrategies;
						}
						strategies.Add(strategy);
					}
				}
				if(symbolMap.Count == 1) {
					portfolioType = PortfolioType.SingleSymbol;
				} else if( symbolMap.Count == strategies.Count) {
					portfolioType = PortfolioType.MultiSymbol;
				} else {
					// Remove all dependencies.
					Chain.Dependencies.Clear();
					// There is a mixture of multi symbols and multi strategies per symbol.
					// Insert additional Portfolios for each symbol.
					foreach( var kvp in symbolMap) {
						string symbol = kvp.Key;
						List<StrategyCommon> tempStrategies = kvp.Value;
						if( tempStrategies.Count > 1) {
							PortfolioCommon portfolio = new PortfolioCommon();
							portfolio.Name = "Portfolio-"+symbol;
							portfolio.SymbolDefault = symbol;
							foreach( var strategy in tempStrategies) {
								portfolio.Chain.Dependencies.Add(strategy.Chain);
							}
							Chain.Dependencies.Add( portfolio.Chain);
						} else {
							StrategyCommon strategy = tempStrategies[0];
							Chain.Dependencies.Add( strategy.Chain);
						}
					}
				}
			} while( portfolioType == PortfolioType.None);
		}	
	
		public override bool OnProcessTick(Tick tick)
		{
			if( portfolioType == PortfolioType.SingleSymbol) {
				double internalSignal = 0;
				foreach( var strategy in strategies) {
					internalSignal += strategy.Performance.Position.Signal;
				}
				Position.Signal = internalSignal;
				return true;
			} else if( portfolioType == PortfolioType.MultiSymbol) {
				double tempClosedEquity = 0;
				double tempOpenEquity = 0;
				foreach( var strategy in strategies) {
					tempOpenEquity += strategy.Performance.Equity.OpenEquity;
					tempClosedEquity += strategy.Performance.Equity.ClosedEquity;
					tempClosedEquity -= strategy.Performance.Equity.StartingEquity;
				}
				if( tempClosedEquity != closedEquity) {
					double change = tempClosedEquity - closedEquity;
					Performance.Equity.OnChangeClosedEquity(change);
					closedEquity = tempClosedEquity;
				}
				if( tempOpenEquity != openEquity) {
					Performance.Equity.OnSetOpenEquity(tempOpenEquity);
					openEquity = tempOpenEquity;
				}
				return true;
			} else {
				throw new ApplicationException("PortfolioType was never set.");
			}
		}
		
		/// <summary>
		/// Shortcut to look at the data of and control any dependant strategies.
		/// </summary>
		public List<StrategyCommon> Strategies {
			get { return strategies; }
		}
		
		public List<StrategyCommon> Markets {
			get { return strategies; }
		}
		
		public PortfolioType PortfolioType {
			get { return portfolioType; }
			set { portfolioType = value; }
		}
	}
}
