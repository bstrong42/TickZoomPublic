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
 * Date: 4/2/2009
 * Time: 8:16 AM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using TickZoom.Api;

namespace TickZoom.Common
{
	public class ModelPropertiesCommon : ModelProperties
	{
		Log log = Factory.Log.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		Dictionary<string, ModelProperty> properties = new Dictionary<string, ModelProperty>();
		Dictionary<string, ModelProperties> models = new Dictionary<string, ModelProperties>();
		ModelType modelType = ModelType.None;
		string name;
		string type;
		
		public ModelProperties Clone() {
			ModelPropertiesCommon model = new ModelPropertiesCommon();
			model.ModelType = modelType;
			model.Name = name;
			model.Type = type;
			foreach(KeyValuePair<string, ModelProperty> kvp in properties) {
				model.AddProperty(kvp.Key,kvp.Value.Clone());
			}
			foreach(KeyValuePair<string, ModelProperties> kvp in models) {
				model.AddModel(kvp.Key,kvp.Value.Clone());
			}
			return model;
		}
		
		public void AddProperty(string name, string value)
		{
			properties.Add(name, new ModelPropertyCommon(name,value));
		}

		public void AddProperty(string name, string value, string startStr, string endStr, string incrementStr, string optimizeStr)
		{
			double start = Convert.ToDouble(startStr);
			double end = Convert.ToDouble(endStr);
			double increment = Convert.ToDouble(incrementStr);
			bool optimize = Convert.ToBoolean(optimizeStr);
			
			properties.Add(name, new ModelPropertyCommon(name,value,start,end,increment,optimize));
		}
		
		public void AddProperty(string name, ModelProperty property)
		{
			properties.Add(name, property);
		}
		
		public void AddModel(string name, ModelProperties model)
		{
			models.Add(name, model);
		}
		
		public ModelProperties GetModel(string key) {
			return models[key];
		}

		public ModelProperty GetProperty(string key) {
			return properties[key];
		}
		
		public string[] GetPropertyKeys()
		{
			string[] keyArray = new string[properties.Keys.Count];
			properties.Keys.CopyTo(keyArray, 0);
			return keyArray;
		}

		public string[] GetModelKeys()
		{
			string[] keyArray = new string[models.Keys.Count];
			models.Keys.CopyTo(keyArray, 0);
			return keyArray;
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}
		
		public string Type {
			get { return type; }
			set { type = value; }
		}
		
		public ModelType ModelType {
			get { return modelType; }
			set { modelType = value; }
		}
	}
}
