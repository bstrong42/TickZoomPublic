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
using log4net.Core;
using TickZoom.Api;
using log4net.Filter;

namespace TickZoom.Logging
{
	/// <summary>
	/// Description of TimeStampFilter.
	/// </summary>
	public class TimeStampFilter : FilterSkeleton
	{
		private string beginTimeStr;
		private string endTimeStr;
		
		private TimeStamp beginTime;
		private TimeStamp endTime;
		
		private void ConvertTime() {
			beginTime = TimeStamp.MinValue;
			if( beginTimeStr != null) {
				beginTimeStr = beginTimeStr.Trim();
				if( beginTimeStr.Length > 0) {
					beginTime = new TimeStamp(beginTimeStr);
				}
			}
			endTime = TimeStamp.MaxValue;
			if( endTimeStr != null) {
				endTimeStr = endTimeStr.Trim();
				if( endTimeStr.Length > 0) {
					endTime = new TimeStamp(endTimeStr);
				}
			}
		}

		public override FilterDecision Decide(LoggingEvent loggingEvent)
		{
			string timeStampStr = (string) loggingEvent.Properties["TimeStamp"];
			if( timeStampStr != null && timeStampStr.Length>0) {
				TimeStamp timeStamp = new TimeStamp(timeStampStr);
				if( timeStamp >= beginTime && timeStamp <= endTime) {
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
		
		public string BeginTime {
			get { return beginTimeStr; }
			set { beginTimeStr = value; 
				ConvertTime(); }
		}
		
		public string EndTime {
			get { return endTimeStr; }
			set { endTimeStr = value;
				ConvertTime(); }
		}
	}
}
