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
public class TimestampTypeConverter : DateTimeConverter
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
            string s = ((string) value).Trim();
            try
            {
            	DateTime time = DateTime.Parse(s);
            	return new TimeStamp(time);
            }
            catch (FormatException exception)
            {
                throw new FormatException("ConvertInvalidPrimitive = Timestamp" + exception);
            }
        }
        return base.ConvertFrom(context, culture, value);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == null)
        {
            throw new ArgumentNullException("destinationType");
        }
        if ((destinationType == typeof(InstanceDescriptor)) && (value is TimeStamp))
        {
        	MethodInfo method = GetType().GetMethod("Parse", new Type[] { typeof(string) });
            if (method != null)
            {
                return new InstanceDescriptor(method, new object[] { value.ToString() });
            }
        }
        if ((destinationType == typeof(string)) && (value is TimeStamp))
        {
        	TimeStamp timestamp = (TimeStamp) value;
        	DateTime date = timestamp.DateTime;
        	return date.ToString("d");
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
    
    public TimeStamp Parse(string text) {
    	DateTime time = DateTime.Parse(text);
    	return  new TimeStamp(time);
    }
}
