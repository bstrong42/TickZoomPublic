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
 * Date: 3/29/2009
 * Time: 11:04 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace TickZoom.Api
{
	/// <summary>
	/// Description of Converters.
	/// </summary>
	public class Converters
	{
		private static Dictionary<Type,TypeConverter> converters = new Dictionary<Type,TypeConverter>();
		static Converters()
		{
			converters.Add(typeof(Interval),new IntervalTypeConverter());
			converters.Add(typeof(TimeStamp),new TimestampTypeConverter());
			converters.Add(typeof(Elapsed),new ElapsedTypeConverter());
		}
		
		public static TypeConverter GetConverter(Type type) {
			TypeConverter converter;
			if( !converters.TryGetValue(type, out converter)) {
				converter = TypeDescriptor.GetConverter(type);
			}
			return converter;
		}
		
		public static object Convert( Type type, string str) {
			Assembly assembly = Assembly.GetAssembly(typeof(Converters));
			CultureInfo cultureInfo = assembly.GetName().CultureInfo;
			TypeConverter converter = Converters.GetConverter(type);
			object obj = converter.ConvertFrom(new VoidContext(),cultureInfo,str);
			return obj;
		}		
		
		
#region VoidContext
		public class VoidContext : ITypeDescriptorContext {
			public IContainer Container {
				get {
					throw new NotImplementedException();
				}
			}
			
			public object Instance {
				get {
					throw new NotImplementedException();
				}
			}
			
			public PropertyDescriptor PropertyDescriptor {
				get {
					throw new NotImplementedException();
				}
			}
			
			public bool OnComponentChanging()
			{
				throw new NotImplementedException();
			}
			
			public void OnComponentChanged()
			{
				throw new NotImplementedException();
			}
			
			public object GetService(Type serviceType)
			{
				throw new NotImplementedException();
			}
		}
#endregion

	}
}
