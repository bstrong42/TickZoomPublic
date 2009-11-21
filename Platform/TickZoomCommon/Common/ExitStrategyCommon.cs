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
using System.Diagnostics;
using System.Drawing;
using TickZoom.TickUtil;

using TickZoom.Api;

namespace TickZoom.Common
{
	/// <summary>
	/// Description of StrategySupport.
	/// </summary>
	public class ExitStrategyCommon : StrategySupport
	{
		private bool controlStrategy = false;
		private double strategySignal = 0;
		private LogicalOrder stopLossOrder;
		private double stopLoss = 0;
		private double targetProfit = 0;
		private double breakEven = 0;
		private double entryPrice = 0;
		private double trailStop = 0;
		private double dailyMaxProfit = 0;
		private double dailyMaxLoss = 0;
		private double weeklyMaxProfit = 0;
		private double weeklyMaxLoss = 0;
		private double monthlyMaxProfit = 0;
		private double monthlyMaxLoss = 0;
		private LogicalOrder breakEvenStopOrder;
		private double breakEvenStop = 0;
		private double pnl = 0;
		private double maxPnl = 0;
		bool stopTradingToday = false;
		bool stopTradingThisWeek = false;
		bool stopTradingThisMonth = false;
		
		public ExitStrategyCommon(StrategyCommon strategy) : base( strategy) {
			RequestUpdate(Intervals.Day1);
			RequestUpdate(Intervals.Week1);
			RequestUpdate(Intervals.Month1);
		}
		
		public override void OnInitialize()
		{
			stopLossOrder = Data.CreateOrder();
			stopLossOrder.TradeDirection = TradeDirection.Exit;
			Strategy.OrderManager.Add(stopLossOrder);
			breakEvenStopOrder = Data.CreateOrder();
			breakEvenStopOrder.TradeDirection = TradeDirection.Exit;
			Strategy.OrderManager.Add(breakEvenStopOrder);
//			log.WriteFile( LogName + " chain = " + Chain.ToChainString());
			if( IsTrace) Log.Trace(FullName+".Initialize()");
			Drawing.Color = Color.Black;
		}
	
		public sealed override bool OnProcessTick(Tick tick)
		{
			if( IsTrace) Log.Trace("ProcessTick() Previous="+Strategy+" Previous.Signal="+Strategy.Position.Signal);
			
			if( stopTradingToday || stopTradingThisWeek || stopTradingThisMonth ) {
				return true;
			}
			if( (strategySignal>0) != Strategy.Position.IsLong || (strategySignal<0) != Strategy.Position.IsShort ) {
				strategySignal = Strategy.Position.Signal;
				if( strategySignal > 0) {
					entryPrice = tick.Ask;
				} else {
					entryPrice = tick.Bid;
				}
				maxPnl = 0;
				Position.Signal = strategySignal;
				trailStop = 0;
				breakEvenStop = 0;
				CancelOrders();
			} 
			
			if( Position.HasPosition ) {
				// copy signal in case of increased position size
				Position.Signal = Strategy.Position.Signal;
				double exitPrice;
				if( strategySignal > 0) {
					exitPrice = tick.Bid;
					pnl = (exitPrice - entryPrice).Round();
				} else {
					exitPrice = tick.Ask;
					pnl = (entryPrice - exitPrice).Round();
				}
				maxPnl = pnl > maxPnl ? pnl : maxPnl;
				if( stopLoss > 0) processStopLoss(tick);
				if( trailStop > 0) processTrailStop(Ticks[0].Bid);
				if( breakEven > 0) processBreakEven();
				if( targetProfit > 0) processTargetProfit();
				if( dailyMaxProfit > 0) processDailyMaxProfit();
				if( dailyMaxLoss > 0) processDailyMaxLoss();
				if( weeklyMaxProfit > 0) processWeeklyMaxProfit();
				if( weeklyMaxLoss > 0) processWeeklyMaxLoss();
				if( monthlyMaxProfit > 0) processMonthlyMaxProfit();
				if( monthlyMaxLoss > 0) processMonthlyMaxLoss();
			}
			
			return true;
		}
	
		private void processDailyMaxProfit() {
			if( Strategy.Performance.Equity.ProfitToday >= dailyMaxProfit) {
				stopTradingToday = true;
				LogExit("DailyMaxProfit Exit at " + dailyMaxProfit);
				flattenSignal();
			}
		}
		
		private void processDailyMaxLoss() {
			if( - Strategy.Performance.Equity.ProfitToday >= dailyMaxLoss) {
				stopTradingToday = true;
				LogExit("DailyMaxLoss Exit at " + dailyMaxLoss);
				flattenSignal();
			}
		}
		
		private void processWeeklyMaxProfit() {
			if( Strategy.Performance.Equity.ProfitForWeek >= weeklyMaxProfit) {
				stopTradingThisWeek = true;
				LogExit("WeeklyMaxProfit Exit at " + weeklyMaxProfit);
				flattenSignal();
			}
		}
		
		private void processWeeklyMaxLoss() {
			if( - Strategy.Performance.Equity.ProfitForWeek >= weeklyMaxLoss) {
				stopTradingThisWeek = true;
				LogExit("WeeklyMaxLoss Exit at " + weeklyMaxLoss);
				flattenSignal();
			}
		}
		
		private void processMonthlyMaxProfit() {
			if( Strategy.Performance.Equity.ProfitForMonth >= monthlyMaxProfit) {
				stopTradingThisMonth = true;
				LogExit("MonthlyMaxProfit Exit at " + monthlyMaxProfit);
				flattenSignal();
			}
		}
		
		private void processMonthlyMaxLoss() {
			if( - Strategy.Performance.Equity.ProfitForMonth >= monthlyMaxLoss) {
				stopTradingThisMonth = true;
				LogExit("MonthlyMaxLoss Exit at " + monthlyMaxLoss);
				flattenSignal();
			}
		}
		
		private void CancelOrders() {
			stopLossOrder.IsActive = false;
			breakEvenStopOrder.IsActive = false;
		}
		
		private void flattenSignal() {
			Position.Signal = 0;
			CancelOrders();
			if( controlStrategy) {
				Strategy.Exit.GoFlat();
				strategySignal = 0;
			}
		}
	
		private void processTargetProfit() {
			if( pnl >= targetProfit) {
				LogExit("TargetProfit Exit at " + targetProfit);
				flattenSignal();
			}
		}
		
		private void processStopLoss(Tick tick) {
			if( Position.IsLong && !stopLossOrder.IsActive) {
				stopLossOrder.Type = OrderType.SellStop;
				stopLossOrder.Price = entryPrice - stopLoss;
				stopLossOrder.IsActive = true;
			}
			if( Position.IsShort && !stopLossOrder.IsActive) {
				stopLossOrder.Type = OrderType.BuyStop;
				stopLossOrder.Price = entryPrice + stopLoss;
				stopLossOrder.IsActive = true;
			}
			if( pnl <= -stopLoss) {
				LogExit("StopLoss Exit at " + stopLoss);
				flattenSignal();
			}
		}
		
		private void processTrailStop(double price) {
			if( maxPnl - pnl >= trailStop) {
				LogExit("TailStop Exit at " + trailStop);
				flattenSignal();
			}
		}
		
		public override bool OnIntervalOpen(Interval interval) {
			if( interval.Equals(Intervals.Day1)) {
				stopTradingToday = false;
			}
			if( interval.Equals(Intervals.Week1)) {
				stopTradingThisWeek = false;
			}
			if( interval.Equals(Intervals.Month1)) {
				stopTradingThisMonth = false;
			}
			return true;
		}
	
		private void processBreakEven() {
			if( pnl >= breakEven) {
				if( Position.IsLong && !breakEvenStopOrder.IsActive) {
					breakEvenStopOrder.Type = OrderType.SellStop;
					breakEvenStopOrder.Price = entryPrice + breakEvenStop;
					breakEvenStopOrder.IsActive = true;
				}
				if( Position.IsShort && !breakEvenStopOrder.IsActive) {
					breakEvenStopOrder.Type = OrderType.BuyStop;
					breakEvenStopOrder.Price = entryPrice - breakEvenStop;
					breakEvenStopOrder.IsActive = true;
				}
			}
			if( breakEvenStopOrder.IsActive && pnl <= breakEvenStop) {
				LogExit("Break Even Exit at " + breakEvenStop);
				flattenSignal();
			}
		}
		
		private void LogExit(string description) {
			if( Chart.IsDynamicUpdate) {
				Log.Notice(Ticks[0].Time + ", Bar="+Chart.DisplayBars.CurrentBar+", " + description);
			} else if( !IsOptimizeMode) {
				if( IsTrace) Log.Trace(Ticks[0].Time + ", Bar="+Chart.DisplayBars.CurrentBar+", " + description);
			}
		}

        #region Properties
        
        [DefaultValue(0d)]
        public double StopLoss
        {
            get { return stopLoss; }
            set { // log.WriteFile(GetType().Name+".StopLoss("+value+")");
            	  stopLoss = Math.Max(0, value); }
        }		

        [DefaultValue(0d)]
		public double TrailStop
        {
            get { return trailStop; }
            set { trailStop = Math.Max(0, value); }
        }		
		
        [DefaultValue(0d)]
		public double TargetProfit
        {
            get { return targetProfit; }
            set { if( IsTrace) Log.Trace(GetType().Name+".TargetProfit("+value+")");
            	  targetProfit = Math.Max(0, value); }
        }		
		
        [DefaultValue(0d)]
		public double BreakEven
        {
            get { return breakEven; }
            set { breakEven = Math.Max(0, value); }
        }	
		
        [DefaultValue(false)]
		public bool ControlStrategy {
			get { return controlStrategy; }
			set { controlStrategy = value; }
		}
		
        [DefaultValue(0d)]
		public double WeeklyMaxProfit {
			get { return weeklyMaxProfit; }
			set { weeklyMaxProfit = value; }
		}
		
        [DefaultValue(0d)]
		public double WeeklyMaxLoss {
			get { return weeklyMaxLoss; }
			set { weeklyMaxLoss = value; }
		}
		
        [DefaultValue(0d)]
		public double DailyMaxProfit {
			get { return dailyMaxProfit; }
			set { dailyMaxProfit = value; }
		}
		
        [DefaultValue(0d)]
		public double DailyMaxLoss {
			get { return dailyMaxLoss; }
			set { dailyMaxLoss = value; }
		}
		
        [DefaultValue(0d)]
		public double MonthlyMaxLoss {
			get { return monthlyMaxLoss; }
			set { monthlyMaxLoss = value; }
		}
		
        [DefaultValue(0d)]
		public double MonthlyMaxProfit {
			get { return monthlyMaxProfit; }
			set { monthlyMaxProfit = value; }
		}
		#endregion
	
		public override string ToString()
		{
			return FullName;
		}
		
	}
}
