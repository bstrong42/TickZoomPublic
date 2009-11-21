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
 * Date: 8/7/2009
 * Time: 12:01 PM
 * <http://www.tickzoom.org/wiki/Licenses>.
 */
#endregion


using System;
using NUnit.Framework;
using TickZoom.Api;

namespace TickZoom.Utilities
{
	[TestFixture]
	public class SymbolTest
	{
		[Test]
		public void TestSymbol()
		{
			string symbol = "USD/JPY";
			ulong uSymbol = ExtensionMethods.SymbolToULong(symbol);
			string symbolConverted = ExtensionMethods.ULongToSymbol(uSymbol);
			Assert.AreEqual(symbol,symbolConverted);
//			Assert.AreEqual(1515607893785,uSymbol);
		}
		
		[Test]
		public void TestSpecific()
		{
			string symbol = "Daily4Tic";
			ulong uSymbol = ExtensionMethods.SymbolToULong(symbol);
			string symbolConverted = ExtensionMethods.ULongToSymbol(uSymbol);
			Assert.AreEqual(symbol,symbolConverted);
//			Assert.AreEqual(1515607893785,uSymbol);
		}
		
		[Test]
		public void TestMaxString2()
		{
			string expectedSymbol = "AAAAAAAAA";
			ulong uSymbol = ExtensionMethods.SymbolToULong(expectedSymbol);
			string symbol = ExtensionMethods.ULongToSymbol(uSymbol);
			Assert.AreEqual(expectedSymbol,symbol);
		}
		
		[Test]
		public void TestCharToInt()
		{
			Assert.AreEqual(1,ExtensionMethods.CharToInt('A'));
			Assert.AreEqual(26,ExtensionMethods.CharToInt('Z'));
			Assert.AreEqual(27,ExtensionMethods.CharToInt('a'));
			Assert.AreEqual(52,ExtensionMethods.CharToInt('z'));
			Assert.AreEqual(53,ExtensionMethods.CharToInt('0'));
			Assert.AreEqual(62,ExtensionMethods.CharToInt('9'));
			Assert.AreEqual(63,ExtensionMethods.CharToInt('/'));
		}
		
		[Test]
		public void TestIntToChar()
		{
			Assert.AreEqual('A',ExtensionMethods.IntToChar(1));
			Assert.AreEqual('Z',ExtensionMethods.IntToChar(26));
			Assert.AreEqual('a',ExtensionMethods.IntToChar(27));
			Assert.AreEqual('z',ExtensionMethods.IntToChar(52));
			Assert.AreEqual('0',ExtensionMethods.IntToChar(53));
			Assert.AreEqual('9',ExtensionMethods.IntToChar(62));
			Assert.AreEqual('/',ExtensionMethods.IntToChar(63));
		}
	}
}
