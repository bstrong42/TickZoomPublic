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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using TickZoom;
using TickZoom.Api;
using TickZoom.Common;
//using TickZoom.Provider;
using TickZoom.TickUtil;

namespace TickZoom.BlackBox
{
	public class BlackBoxService : ServiceBase
	{
		Log log = Factory.Log.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public const string MyServiceName = "OrderService";
       	BackgroundWorker processWorker = new BackgroundWorker();
// 		ProviderProxy dataFeed = null;
        
		public BlackBoxService()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			this.ServiceName = MyServiceName;
			log.FileName = @"OrderServer.log";
        	this.processWorker.WorkerSupportsCancellation = true;
        	this.processWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ProcessWorkerRunWorkerCompleted);
        	this.processWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ProcessWorkerDoWork);
		}
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
            if( processWorker != null) { processWorker.CancelAsync(); }
//        	if( dataFeed != null) { dataFeed.Stop(this); }
		}
		
		/// <summary>
		/// Start this service.
		/// </summary>
		protected override void OnStart(string[] args)
		{
           	processWorker.RunWorkerAsync(2);
		}
		
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
            if( processWorker != null) { processWorker.CancelAsync(); }
//        	if( dataFeed != null) { dataFeed.Stop(); }
		}
		
        void ProcessWorkerDoWork(object sender, DoWorkEventArgs e)
        {
        	BackgroundWorker bw = sender as BackgroundWorker;
            int arg = (int)e.Argument;
        	log.Notice("Started thread to do work.");
            
       		OrderServer(bw);
        }
        
		public void OrderServer(BackgroundWorker bw)
		{
#if REALTIME
			Starter starter = new RealTimeStarter();
    		dataFeed = new ProviderProxy();
    		dataFeed.BackgroundWorker = bw;
    		
    		// Only difference with Order Service
			run.ShowChartCallback = new ShowChartCallback(ShowChartInvoke);
			run.Chart = chart;
			
			string symbol = "USD/JPY";
    		starter.BackgroundWorker = bw;
    		starter.AddDataFeed(dataFeed);
    		starter.ProjectProperties.Starter.Symbols = symbol;
    		starter.Run();
//    		dataFeed.StartSymbol(symbol,new TimeStamp());
    		
            while(!bw.CancellationPending) {
            	Application.DoEvents();
            	Thread.Sleep(5);
    		}
#endif    		
		}
    		
        void ProcessWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
			if(e.Error !=null)
			{ // analyze error here
				log.Notice(e.Error.ToString());
				Thread.Sleep(5000);
				log.Notice("Restarting Order Service.");
			}
        }
	}
}
