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
 * Date: 3/19/2009
 * Time: 12:02 AM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace TickZoom.Common
{
	/// <summary>
	/// Description of EngineProperties.
	/// </summary>
	public class EngineProperties : PropertiesBase, TickZoom.Api.EngineProperties
	{
		public EngineProperties()
		{
			try {
				intervalDefault = TickZoom.Api.Factory.Engine.DefineInterval(TickZoom.Api.BarUnit.Default,0);
			} catch {
				
			}
		}
		
		bool enableTickFilter = false;
		
		public bool EnableTickFilter {
			get { return enableTickFilter; }
			set { enableTickFilter = value; }
		}
		
		TickZoom.Api.Interval intervalDefault;
		
		public TickZoom.Api.Interval IntervalDefault {
			get { return intervalDefault; }
			set { intervalDefault = value; }
		}
		
		int breakAtBar = 0;
		
		public int BreakAtBar {
			get { return breakAtBar; }
			set { breakAtBar = value; }
		}
		
		int maxBarsBack = 1000;
		
		public int MaxBarsBack {
			get { return maxBarsBack; }
			set { maxBarsBack = value; }
		}
		
		int maxTicksBack = 10000;
		
		public int MaxTicksBack {
			get { return maxTicksBack; }
			set { maxTicksBack = value; }
		}
		
		int tickReplaySpeed = 0;
		
		public int TickReplaySpeed {
			get { return tickReplaySpeed; }
			set { tickReplaySpeed = value; }
		}

		int barReplaySpeed = 0;
		
		public int BarReplaySpeed {
			get { return barReplaySpeed; }
			set { barReplaySpeed = value; }
		}
		
		bool realtimeOutput = true;
		
		public bool RealtimeOutput {
			get { return realtimeOutput; }
			set { realtimeOutput = value; }
		}
		
		public override string ToString()
		{
			return "";
		}
		
	}
}
