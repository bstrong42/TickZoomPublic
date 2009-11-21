#region Copyright
/*
 * Copyright 2008 M. Wayne Walter
 * Software: TickZoom Trading Platform
 * User: Wayne Walter
 * 
 * You can use and modify this software under the terms of the
 * TickZOOM General Public License Version 1.0 or (at your option)
 * any later version.
 * 
 * Businesses are restricted to 30 days of use.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * TickZOOM General Public License for more details.
 *
 * You should have received a copy of the TickZOOM General Public
 * License along with this program.  If not, see
 * 
 * 
 *
 * User: Wayne Walter
 * Date: 8/10/2009
 * Time: 5:07 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion


using System;
using TickZoom.Api;
using TickZoom.Common;

namespace TickZoom.Examples.Indicators
{
	public class MQ_ValueChart : IndicatorCommon
	{
	               
	        /// <summary>
	        /// Four nested indicators to be displayed as a price bar chart
	        /// </summary>
	        IndicatorCommon vcOpen;
	        IndicatorCommon vcHigh;
	        IndicatorCommon vcLow;
	        IndicatorCommon vcClose;
	               
	        public MQ_ValueChart()
	        {
	        }
	
	        public override void OnInitialize() {
	
	            vcOpen = new IndicatorCommon();
	            vcOpen.Drawing.GroupName = "ValueChart";
	            vcOpen.Drawing.PaneType = PaneType.Secondary;
	            vcOpen.Drawing.IsVisible = true;
	            vcOpen.Drawing.GraphType = GraphType.Histogram;
	            AddIndicator(vcOpen);
	
		        vcHigh = new IndicatorCommon();
	            vcHigh.Drawing.GroupName = "ValueChart";
	            vcHigh.Drawing.PaneType = PaneType.Secondary;
	            vcHigh.Drawing.IsVisible = true;
	            vcHigh.Drawing.GraphType = GraphType.Histogram;
	            AddIndicator(vcHigh);
	
	            vcLow = new IndicatorCommon();
	            vcLow.Drawing.GroupName = "ValueChart";
	            vcLow.Drawing.PaneType = PaneType.Secondary;
	            vcLow.Drawing.IsVisible = true;
	            vcLow.Drawing.GraphType = GraphType.Histogram;
	            AddIndicator(vcLow);
	
		        vcClose = new IndicatorCommon();
	            vcClose.Drawing.GroupName = "ValueChart";
	            vcClose.Drawing.PaneType = PaneType.Secondary;
	            vcClose.Drawing.IsVisible = true;
	            vcClose.Drawing.GraphType = GraphType.Histogram;
	            AddIndicator(vcClose);
	        }
	               
	        public override bool OnIntervalClose()
	        {
	            vcOpen[0] = Bars.Open[0];
	            vcHigh[0] = Bars.High[0];
	            vcLow[0] = Bars.Low[0];
	            vcClose[0] = Bars.Close[0];
	           
	            return true;
	        }
	       
	} 
}
