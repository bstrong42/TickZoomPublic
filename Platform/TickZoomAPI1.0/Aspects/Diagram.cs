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
 * Date: 8/19/2009
 * Time: 12:16 AM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace TickZoom.Api
{
	/// <summary>
	/// Description of LoggingHook.
	/// </summary>
	public class Diagram
	{
		public static readonly Log Log = Factory.Log.GetLogger(typeof(Diagram));
		public static readonly bool IsDebug = Log.IsDebugEnabled;
		public static readonly bool IsTrace = Log.IsTraceEnabled;
		private static Dictionary<object,string> instances;
		private static object locker = new object();
		private static Dictionary<Type,int> instanceCounters;
		
		public static void StateChange(object instance, object obj) {
			if( IsDebug) {
				bool isNew;
				string instanceName = GetInstanceName(instance, out isNew);
				Log.Debug( instanceName + " >>> " + obj);
			}
		}
		
		public static void Line(object obj) {
			if( IsDebug) {
				MethodBase caller = new StackFrame(1).GetMethod();
				Log.Debug( "# " + obj);
			}
		}

		public static string GetInstanceName(object instance, out bool isNew) {
			string callee;
			isNew = true;
    		if( !Instances.TryGetValue(instance,out callee)) {
	    		MethodBase method = new StackFrame(1).GetMethod();
	    		callee = GetTypeName(instance.GetType());
	    		int counter;
	    		lock(locker) {
		    		if( !InstanceCounters.TryGetValue(instance.GetType(),out counter)) {
		    			counter = 0;
		    			InstanceCounters[instance.GetType()] = counter;
		    		}
		    		InstanceCounters[instance.GetType()] = ++counter;
	    		}
	    		callee += counter;
	    		Instances[instance] = callee;
    		} else {
				isNew = false;
    		}
			return callee;
		}
		
		public static string GetTypeName(Type type) {
			Type[] generics = type.GetGenericArguments();
			if( generics.Length>0) {
				StringBuilder builder = new StringBuilder();
				builder.Append(StripTilda(type.Name));
				builder.Append("<");
				for( int j=0; j<generics.Length; j++) {
					if( j!=0) builder.Append(",");
					Type generic = generics[j];
					builder.Append(GetTypeName(generic));
				}
				builder.Append(">");
				return builder.ToString();
			} else {
				return type.Name;
			}
		}
		
		private static string StripTilda(string typeName) {
			return typeName.Substring(0,typeName.IndexOf('`'));
		}
		
		public static Dictionary<object, string> Instances {
			get { if( instances == null) {
	    			lock(locker) {
	    				if( instances == null) {
							instances = new Dictionary<object, string>();
	    				}
	    			}
				}
				return instances;
			}
		}
		
		public static Dictionary<Type, int> InstanceCounters {
	    	get { if( instanceCounters == null) {
	    			lock( locker) {
	    				if( instanceCounters == null) {
			    			instanceCounters = new Dictionary<Type, int>();
	    				}
	    			}
	    		}
	    		return instanceCounters;
	    	}
		}
	}
}
