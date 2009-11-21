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
 * Time: 7:27 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;

using TickZoom.Api;
using TickZoom.Common;

namespace TickZoom
{
	/// <summary>
	/// Description of StarterProperties.
	/// </summary>
	public class StarterProperties 
	{
		string symbol = "USD/JPY";
		TimeStamp startTime;
		TimeStamp endTime;
		
		public StarterProperties()
		{
			startTime = new TimeStamp(2000,1,1);
			endTime = new TimeStamp(DateTime.Now);
		}
		
		public string Symbol {
			get { return symbol; }
			set { symbol = value; }
		}
		
		[Editor(typeof(TimestampPropertyEditor),typeof(UITypeEditor))]
		[TypeConverter(typeof(TimestampTypeConverter))]
		public TimeStamp StartTime {
			get { return startTime; }
			set { startTime = value; }
		}
		
		[Editor(typeof(TimestampPropertyEditor),typeof(UITypeEditor))]
		[TypeConverter(typeof(TimestampTypeConverter))]
		public TimeStamp EndTime {
			get { return endTime; }
			set { endTime = value; }
		}
		
		Interval intervalDefault;
		[Editor(typeof(IntervalPropertyEditor),typeof(UITypeEditor))]
		[TypeConverter(typeof(IntervalTypeConverter))]
		public Interval IntervalDefault {
			get { return intervalDefault; }
			set { intervalDefault = value; }
		}
		
		public override string ToString()
		{
			return "";
		}
	}
}