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
 * Date: 3/13/2009
 * Time: 10:33 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using TickZoom.Api;

namespace TickZoom
{
	/// <summary>
	/// Description of PortfolioDoc.
	/// </summary>
	public partial class PortfolioDoc : Form
	{
		Log log = Factory.Log.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SynchronizationContext context;
        
		public PortfolioDoc()
		{
			context = SynchronizationContext.Current;
            if(context == null)
            {
                context = new SynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(context);
            }
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public ChartControl ChartControl {
			get { return chartControl1; } 
		}
		
		void PortfolioDocLoad(object sender, System.EventArgs e)
		{
//			chartControl1.ChartLoad(sender,e);
		}
		
		void PortfolioDocResize(object sender, EventArgs e)
		{
			chartControl1.Size = new Size( ClientRectangle.Width, ClientRectangle.Height);
		}
		

		public new void Show() {
			ShowInvoke();
		}
		
//			if( InvokeRequired) {
//				BeginInvoke(new MethodInvoker(Show));
//			} else {
//				base.Show();
//			}

		public void ShowInvoke() {
       		context.Send(new SendOrPostCallback(
       		delegate(object state)
       	    {
				base.Show();       		
       		}), null);
		}
		
//			if( InvokeRequired) {
//				BeginInvoke(new MethodInvoker(Hide));
//			} else {
//				Hide();
//			}
		
		public void HideInvoke() {
       		context.Send(new SendOrPostCallback(
       		delegate(object state)
       	    {
				base.Hide();       		
       		}), null);
		}
		
	    public delegate void ShowDelegate();
	    public delegate void HideDelegate();
	}
}
