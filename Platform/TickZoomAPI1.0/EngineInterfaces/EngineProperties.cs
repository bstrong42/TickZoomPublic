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
 * Date: 4/4/2009
 * Time: 8:41 AM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;

namespace TickZoom.Api
{
	/// <summary>
	/// Description of EngineProperties.
	/// </summary>
	public interface EngineProperties
	{
		bool EnableTickFilter {
			get;
			set;
		}
		
		bool RealtimeOutput {
			get;
			set;
		}
		
		Interval IntervalDefault {
			get;
			set;
		}
		
		int BreakAtBar {
			get;
			set;
		}
		
		int MaxBarsBack {
			get;
			set;
		}
		
		int MaxTicksBack {
			get;
			set;
		}
		
		int TickReplaySpeed {
			get;
			set;
		}

		int BarReplaySpeed {
			get;
			set;
		}
		
		void CopyProperties( object obj);
	}
}
