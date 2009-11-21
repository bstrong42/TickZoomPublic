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
 * Date: 4/4/2009
 * Time: 7:55 AM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.Reflection;

namespace TickZoom.Common
{
	/// <summary>
	/// Description of PropertiesBase.
	/// </summary>
	[Serializable]
	public class PropertiesBase
	{
		public PropertiesBase()
		{
		}
		
		public void CopyProperties( object otherObj) {
			Type type = this.GetType();
			PropertyInfo[] properties = type.GetProperties();
			for( int i=0; i<properties.Length; i++) {
				PropertyInfo property = properties[i];
				PropertyInfo otherProperty = otherObj.GetType().GetProperty(property.Name);
				if( otherProperty == null) {
					throw new ApplicationException( "Sorry, " + otherObj.ToString() + " doesn't have the property: "+property.Name);
				}
				if( !otherProperty.CanWrite) {
					throw new ApplicationException( "Sorry, " + otherObj.ToString() + " doesn't have a setter for property: "+property.Name);
				}
				otherProperty.SetValue(otherObj,property.GetValue(this,null), null);
			}
		}
	}
}
