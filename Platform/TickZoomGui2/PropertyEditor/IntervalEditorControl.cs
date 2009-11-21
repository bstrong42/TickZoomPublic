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
 * Date: 3/18/2009
 * Time: 11:06 AM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using TickZoom.Api;

namespace TickZoom.PropertyEditor
{
	/// <summary>
	/// Description of IntervalEditorControl.
	/// </summary>
	public partial class IntervalEditorControl : UserControl
	{
        public bool IsCanceled = false;
        Interval interval = Factory.Engine.DefineInterval(BarUnit.Day,1);
		
		public IntervalEditorControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			timeUnitCombo.DataSource = System.Enum.GetValues(typeof(BarUnit));
			List<int> numbers = new List<int>();
			numbers.Add(1);
			numbers.Add(4);
			numbers.Add(5);
			numbers.Add(10);
			numbers.Add(15);
			numbers.Add(20);
			numbers.Add(30);
			numbers.Add(50);
			numbers.Add(100);
			periodCombo.DataSource = numbers;
		}
		
		private void UpdateControl() {
			timeUnitCombo.Text = interval.BarUnit.ToString();
			periodCombo.Text = interval.Period.ToString();
		}
		
		private void UpdateInterval() {
			BarUnit barUnit = (BarUnit) Enum.Parse(typeof(BarUnit),timeUnitCombo.Text,true);
			int period = Convert.ToInt32( periodCombo.Text);
			interval = Factory.Engine.DefineInterval(barUnit,period);
		}
		
		public Interval Interval {
			get { UpdateInterval(); return interval; }
			set { interval = value; UpdateControl(); }
		}
	}
}
