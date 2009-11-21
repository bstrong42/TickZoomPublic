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
 * Time: 8:34 AM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;

namespace TickZoom.Api
{
	/// <summary>
	/// Description of ProjectProperties.
	/// </summary>
    public interface ProjectProperties {
		StarterProperties Starter {
			get;
			set;
		}
		
		ChartProperties Chart {
			get;
			set;
		}
		
		EngineProperties Engine {
			get;
			set;
		}
		
		ModelProperties Model {
			get;
			set;
		}
		
    }
}
