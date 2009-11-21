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
 * Date: 8/4/2009
 * Time: 4:21 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.Collections.Generic;
using log4net.Core;
using log4net.Filter;
using TickZoom.Api;

namespace TickZoom.Logging
{
	/// <summary>
	/// Description of TimeStampFilter.
	/// </summary>
	public class SymbolFilter : FilterSkeleton
	{
		private string symbols;
		private Dictionary<string,string> symbolMap;
		
		private void ConvertSymbols() {
			symbolMap = new Dictionary<string, string>();
			string[] array = symbols.Split(',');
			for( int i=0;i<array.Length; i++) {
				string symbol = array[i].Trim();
				if( symbol.Length>0) {
					symbolMap[symbol] = null;
				}
			}
		}

		public override FilterDecision Decide(LoggingEvent loggingEvent)
		{
			string symbol = (string) loggingEvent.Properties["Symbol"];
			if( symbol != null && symbol.Length>0 && symbolMap.Count > 0) {
				if( symbolMap.ContainsKey(symbol)) {
					return FilterDecision.Neutral;
				}
				else
				{
					return FilterDecision.Deny;
				}
			} else {
				return FilterDecision.Accept;
			}
		}
		
		public string Symbols {
			get { return symbols; }
			set { symbols = value; 
				ConvertSymbols(); }
		}
	}
}
