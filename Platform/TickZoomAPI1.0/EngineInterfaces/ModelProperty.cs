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
 * Date: 4/3/2009
 * Time: 7:23 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;

namespace TickZoom.Api
{
	/// <summary>
	/// Description of Property.
	/// </summary>
	public interface ModelProperty
	{
		string Name {
			get;
//			set;
		}
		
		string Value {
			get;
			set;
		}
		
		double Start {
			get;
//			set;
		}
		
		double End {
			get;
//			set;
		}
		
		double Increment {
			get;
//			set;
		}
		
		bool Optimize {
			get;
//			set;
		}
		
		int Count {
			get;
		}
		
		ModelProperty Clone();
	}
}
