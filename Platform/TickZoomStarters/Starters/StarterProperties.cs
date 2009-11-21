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
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

using TickZoom.Api;

namespace TickZoom.Common
{
	/// <summary>
	/// Description of StarterProperties.
	/// </summary>
	public class StarterProperties : PropertiesBase, TickZoom.Api.StarterProperties
	{
		TickZoom.Api.TimeStamp startTime;
		TickZoom.Api.TimeStamp endTime;
		ChartProperties chartProperties;
		EngineProperties engineProperties;
		List<TickZoom.Api.SymbolInfo> symbolInfo = new List<TickZoom.Api.SymbolInfo>();
		
		public StarterProperties(ChartProperties chartProperties, EngineProperties engineProperties)
		{
			this.chartProperties = chartProperties;
			this.engineProperties = engineProperties;
			startTime = TimeStamp.MinValue;
			endTime = TimeStamp.MaxValue;
			try {
				IntervalDefault = Api.Factory.Engine.DefineInterval(TickZoom.Api.BarUnit.Day,1);
			} catch {
				
			}
		}
		
		public string Symbols {
			set { 
				SymbolLibrary library = new SymbolLibrary();
				string[] symbols = value.Split(new char[] { ',' });
				for( int i=0; i<symbols.Length; i++) {
					SymbolInfo symbol = library.LookupSymbol(symbols[i].Trim());
					symbolInfo.Add(symbol);
				}
			}
		}
				
		public TickZoom.Api.TimeStamp StartTime {
			get { return startTime; }
			set { startTime = value; }
		}
		
		public TickZoom.Api.TimeStamp EndTime {
			get { return endTime; }
			set { endTime = value; }
		}
		
		TickZoom.Api.Interval intervalDefault;
		public TickZoom.Api.Interval IntervalDefault {
			get { return intervalDefault; }
			set { intervalDefault = value;
				  chartProperties.IntervalChartBar = value; 
				  chartProperties.IntervalChartDisplay = value; 
				  chartProperties.IntervalChartUpdate = value; 
				  engineProperties.IntervalDefault = value; 
			}
		}
		
		[Obsolete("Please use SymbolInfo instead.",true)]
		public TickZoom.Api.SymbolProperties[] SymbolProperties {
			get {
				List<SymbolProperties> list = new List<SymbolProperties>();
				foreach( var item in symbolInfo) {
					list.Add((SymbolProperties) item);
				}
				return list.ToArray();
			}
		}
		
		public TickZoom.Api.SymbolInfo[] SymbolInfo {
			get { return symbolInfo.ToArray(); }
		}
	}
}