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
 * Time: 7:28 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using TickZoom.Api;

namespace TickZoom.Common
{
	/// <summary>
	/// Description of ModelProperty.
	/// </summary>
	public class ModelPropertyCommon : ModelProperty
	{
		string name;
		string value;
		double start = 0D;
		double end = 0D;
		double increment = 0D;
		int count = 0;
		bool optimize = false;
		
		public ModelProperty Clone() {
			return new ModelPropertyCommon(name, value, start, end, increment, optimize);
		}
		
		public ModelPropertyCommon(string name, string value)
		{
			this.name = name;
			this.value = value;
		}
		
		public ModelPropertyCommon(string name, string value, double start, double end, double increment, bool optimize)
			: this( name, value)
		{
			this.start = start;
			this.end = end;
			this.increment = increment;
			if( increment == 0D) {
				this.count = 0;
				this.optimize = false;
			} else {
				this.count = (int) ((end - start) / increment + 1);
				this.optimize = optimize;
			}
		}
		
		public string Name {
			get { return name; }
//			set { name = value; }
		}
		
		public string Value {
			get { return this.value; }
			set { this.value = value; }
		}
		
		public double Start {
			get { return start; }
//			set { start = value; }
		}
		
		public double End {
			get { return end; }
//			set { end = value; }
		}
		
		public double Increment {
			get { return increment; }
//			set { increment = value; }
		}
		
		public int Count {
			get { return count; }
		}
		
		public bool Optimize {
			get { return optimize; }
//			set { optimize = value; }
		}
	}
}
