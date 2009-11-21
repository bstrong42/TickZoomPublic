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
 * Date: 3/15/2009
 * Time: 2:50 AM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.Windows.Forms;
using TickZoom.Api;
using TickZoom.Common;

namespace TickZoom.Portfolio
{
	/// <summary>
	/// Description of Portfolio.
	/// </summary>
	public class ChartNode : TreeNode
	{
		public ChartNode() : base()
		{
			Tag = GetChart();
			this.Text = "Chart Settings";
		}
		
		private object GetChart() {
			ChartProperties chart = new ChartProperties();
			PropertyTable properties = new PropertyTable(chart);
//			Starter starter = new DesignStarter();
//			starter.Model = engine;
//			starter.Run();
//			properties.SetAfterInitialize();
//			LoadIndicators(engine);
			return properties;
		}
		
	}
}
