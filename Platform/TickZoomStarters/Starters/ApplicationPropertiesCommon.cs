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
 * Time: 7:27 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Xml;

namespace TickZoom.Common
{
	/// <summary>
	/// Description of ProjectProperties.
	/// </summary>
	public class ApplicationPropertiesCommon
	{
		TickZoom.Api.Log log = TickZoom.Api.Factory.Log.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private Dictionary<string,string> properties = new Dictionary<string,string>();

		private ApplicationPropertiesCommon()
		{
		}
		
		public static void Create(TextReader projectXML) {
			ApplicationPropertiesCommon properties = new ApplicationPropertiesCommon();
			properties.Init(projectXML);
		}
			
			
		private void Init(TextReader projectXML) {
		
			ApplicationPropertiesCommon project = new ApplicationPropertiesCommon();
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.IgnoreComments = true;
			settings.IgnoreWhitespace = true;
			
			using (XmlReader reader = XmlReader.Create(projectXML))
			{
				try {
					// Read nodes one at a time  
					while (reader.Read())  
					{  
					    switch( reader.NodeType) {
					    	case XmlNodeType.Element:
					    		if( "settings".Equals(reader.Name)) {
						    		project.HandleConfiguration(reader);
						    		projectXML.Close();
						    		break;
					    		} else {
					    			project.Error(reader,"Tag " + reader.Name + " was unexpected");
					    			break;
								}
					    	default:
								project.Error( reader, "Unknown nodetype for: " + reader.Name);
								projectXML.Close();
								break;
					    }
					}  				
				} catch( Exception ex) {
					project.Error( reader, ex.ToString());
					projectXML.Close();
				}
			}
			projectXML.Close();
		}
		
		private void HandleConfiguration(XmlReader reader) {
			log.Debug("Handle Starter properties");
			log.Indent();
			while( reader.Read()) {
			    // Print out info on node  
			    switch( reader.NodeType) {
			    	case XmlNodeType.Element:
			    		if( "add".Equals(reader.Name) ) {
			    			HandleProperty(reader.GetAttribute("name"),reader.GetAttribute("value"));
			    		} else {
			    			Error(reader,"Tag " + reader.Name + " was unexpected");
			    			break;
						}
			    		break;
			    	case XmlNodeType.EndElement:
			    		if( "settings".Equals(reader.Name)) {
			    			log.Outdent();
				    		return;
			    		} else {
			    			Error(reader,"End of " + reader.Name + " tag in xml was unexpected");
			    		}
			    		break;
			    	default:
						Error( reader, "Unknown nodetype: " + reader.Name);
						break;
			    }
			}
   			Error(reader,"End of file unexpected");
		}
		
		private void Error( XmlReader reader, string msg) {
			IXmlLineInfo lineInfo = reader as IXmlLineInfo;
			string lineStr = "";
			if( lineInfo != null) {
				lineStr += " on line " + lineInfo.LineNumber + " at position " + lineInfo.LinePosition;
			}
			log.Debug(msg + lineStr);
			throw new ApplicationException(msg + lineStr);
		}
		
		private void HandleProperty( string name, string value) {
			properties[name] = value;
			log.Debug("Property " + name + " = " + value);
		}
		
		public Dictionary<string, string> Properties {
			get { return properties; }
		}
	}
}