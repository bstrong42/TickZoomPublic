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
 * Date: 3/17/2009
 * Time: 10:10 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

using TickZoom.Api;

/// <summary>
/// Description of TimestampTypeConverter.
/// </summary>
public class IntervalTypeConverter : TypeConverter
{
    // Methods
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string)
        {
        	return Parse( (string) value);
        }
        return base.ConvertFrom(context, culture, value);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == null)
        {
            throw new ArgumentNullException("destinationType");
        }
        if ((destinationType == typeof(InstanceDescriptor)) && (value is Interval))
        {
        	MethodInfo method = GetType().GetMethod("Parse", new Type[] { typeof(string) });
            if (method != null)
            {
                return new InstanceDescriptor(method, new object[] { value.ToString() });
            }
        }
        if ((destinationType == typeof(string)) && (value is Interval))
        {
        	Interval interval = (Interval) value;
        	return interval.ToString();
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
    
    public Interval Parse(string text) {
        string s = ((string) text).Trim();
        try
        {
        	Interval interval;
        	string[] parts = s.Split( ",".ToCharArray() );
        	string[] strings1 = parts[0].Split( " ".ToCharArray());
        	int period = Convert.ToInt32(strings1[0]);
        	BarUnit unit = (BarUnit) Enum.Parse(typeof(BarUnit),strings1[1],true);
        	if( parts.Length > 1) {
            	string[] strings2 = parts[1].Split( " ".ToCharArray());
            	int secondaryPeriod = Convert.ToInt32(strings2[0]);
            	BarUnit secondaryUnit = (BarUnit) Enum.Parse(typeof(BarUnit),strings2[1],true);
            	interval = Factory.Engine.DefineInterval(unit,period,secondaryUnit,secondaryPeriod);
        	} else {
            	interval = Factory.Engine.DefineInterval(unit,period);
        	}
        	return interval;
        }
        catch (FormatException exception)
        {
            throw new FormatException("ConvertInvalidPrimitive = Interval: " + exception);
        }
    }
}
