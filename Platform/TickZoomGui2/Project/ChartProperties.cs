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
 * Date: 3/18/2009
 * Time: 6:52 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.ComponentModel;
using System.Drawing.Design;

using TickZoom.Api;
using TickZoom.Common;

namespace TickZoom
{
	/// <summary>
	/// Description of ChartProperties.
	/// </summary>
	public class ChartProperties
	{
		Interval intervalChartBar;
		Interval intervalChartDisplay;
		Interval intervalChartUpdate;
		
		public ChartProperties()
		{
			// Avoid exceptions during design mode.
			try {
				intervalChartUpdate = Factory.Engine.DefineInterval(BarUnit.Default,0);
				intervalChartDisplay = Factory.Engine.DefineInterval(BarUnit.Default,0);
				intervalChartBar = Factory.Engine.DefineInterval(BarUnit.Default,0);
			} catch {
				
			}
		}

		bool showPriceGraph = true;
		
		[DefaultValue(true)]
		public bool ShowPriceGraph {
			get { return showPriceGraph; }
			set { showPriceGraph = value; }
		}
		
		[Editor(typeof(IntervalPropertyEditor),typeof(UITypeEditor))]
		[TypeConverter(typeof(IntervalTypeConverter))]
		public Interval IntervalChartDisplay {
			get { return intervalChartDisplay; }
			set { intervalChartDisplay = value; }
		}
		
		[Editor(typeof(IntervalPropertyEditor),typeof(UITypeEditor))]
		[TypeConverter(typeof(IntervalTypeConverter))]
		public Interval IntervalChartBar {
			get { return intervalChartBar; }
			set { intervalChartBar = value; }
		}
		
		[Editor(typeof(IntervalPropertyEditor),typeof(UITypeEditor))]
		[TypeConverter(typeof(IntervalTypeConverter))]
		public Interval IntervalChartUpdate {
			get { return intervalChartUpdate; }
			set { intervalChartUpdate = value; }
		}

		ChartType chartType = ChartType.Bar;
		[DefaultValue(ChartType.Bar)]
		public ChartType ChartType {
			get { return chartType; }
			set { chartType = value; }
		}
		
		public override string ToString()
		{
			return "";
		}
	}
}
