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
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.Diagnostics;
using System.Reflection;

namespace TickZoom.Api
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	[Serializable]
	public class TickZoomException : System.ApplicationException
	{
		private string stackTrace;
		private string message;
		public TickZoomException(string message) : base() {
			StackFrame sf;
			int frame = 0;
			do {
				frame++;
				sf = new StackFrame(frame,true);
			} while( sf.GetMethod().DeclaringType.Namespace == "TickZoom.Common");
			
			StackTrace st = new StackTrace(sf);
			stackTrace = st.ToString();
			this.message = message + " " + stackTrace;
		}
		
	    // Constructor needed for serialization 
	    // when exception propagates from a remoting server to the client.
	    public TickZoomException(System.Runtime.Serialization.SerializationInfo info,
	        System.Runtime.Serialization.StreamingContext context) { }
		
		public override string StackTrace {
	    	get { if( stackTrace == null) {
	    			return base.StackTrace;
	    		} else {
	    			return stackTrace;
	    		}
	    	}
		}
	    
		public override string Message {
			get { return message; }
		}

	    public override string ToString()
		{
			return Message + StackTrace;
		}
	}
}
