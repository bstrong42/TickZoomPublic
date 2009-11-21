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
 * Date: 10/12/2009
 * Time: 6:26 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion

using System;
using TickZoom.Api;

namespace TickZoom.Common
{

	/// <summary>
	/// Description of SymbolFactory.
	/// </summary>
	public class SymbolFactoryImpl : SymbolFactory
	{
		SymbolLibrary library;
		
		public SymbolLibrary Library {
			get { 
				if( library == null) {
					lock( locker) {
						library = new SymbolLibrary();
					}
				}
				return library;
			}
		}
		object locker = new object();
		public SymbolFactoryImpl()
		{
		}
		
		public SymbolInfo LookupSymbol(string symbol)
		{
			return Library.LookupSymbol(symbol);
		}
		
		public SymbolInfo LookupSymbol(ulong identifier)
		{
			return Library.LookupSymbol(identifier);
		}
	}
}
