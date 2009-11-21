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
 * Date: 5/25/2009
 * Time: 3:36 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion


using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using NUnit.Framework;
using TickZoom;
using TickZoom.Api;
using TickZoom.Common;
using TickZoom.TickUtil;
using ZedGraph;

namespace MiscTest
{
	public class GUIThread {
		Log log = Factory.Log.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private Form mainForm;
		public Thread thread;
		public Type type;
		public GUIThread(Type type) {
			this.type = type;
			log.Debug("Starting Chart Thread");
			ThreadStart job = new ThreadStart(Run);
			thread = new Thread(job);
			thread.Name = "ChartTest";
			thread.Start();
			while( mainForm == null) {
				Thread.Sleep(1);
			}
			log.Debug("Returning Chart Created by Thread");
		}
		
		private void Run() {
			try {
   				log.Debug("Chart Thread Started");
   				mainForm = (Form) Activator.CreateInstance(type);
   				mainForm.Show();
   				Thread.CurrentThread.IsBackground = true;
   				Application.Run();
			} catch( ThreadAbortException) {
				// Thread aborted.
			} catch( Exception ex) {
				log.Error("ERROR: Thread had an exception:",ex);
			}
		}
		
		public void Stop() {
			if(mainForm!=null) {
		   		mainForm.Invoke(new MethodInvoker(mainForm.Hide));
		   		mainForm=null;
			}
			if( thread!=null) {
				thread.Abort();
				thread=null;
			}
		}
		
		public Form MainForm {
			get { return mainForm; }
		}
	}
}
