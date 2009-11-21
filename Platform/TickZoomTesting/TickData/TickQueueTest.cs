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
 * Date: 8/10/2009
 * Time: 3:40 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion


using System;
using NUnit.Framework;
using TickZoom.Api;
using TickZoom.TickUtil;

namespace TickZoom.TickData
{
	[TestFixture]
	public class TickQueueTest
	{
		private static readonly Log log = Factory.Log.GetLogger(typeof(TickQueueTest));
		TickQueue queue;
		
		[SetUp]
		public void Setup() {
			queue = Factory.TickUtil.TickQueue(typeof(TickQueueTest));
		}
		
		[Test]
		public void EnQueueItemTest()
		{
			TickBinary tick = new TickBinary();
			TickIO tickIO = new TickImpl();
			int start = Environment.TickCount;
			for(int i=0; i<10; i++) {
				queue.EnQueue(ref tick);
			}
			int stop = Environment.TickCount;
			log.Notice("Enqueue elapsed time is "+(stop-start)+"ms");
			
			start = Environment.TickCount;
			for(int i=0; i<10; i++) {
				queue.Dequeue(ref tick);
			}
			stop = Environment.TickCount;
			log.Notice("Dequeue elapsed time is "+(stop-start)+"ms");
			queue.LogStats();
		}
	}
}
