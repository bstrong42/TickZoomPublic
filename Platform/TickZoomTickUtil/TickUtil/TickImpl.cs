#region Copyright
/*
 * Software: TickZoom Trading Platform
 * Copyright 2009 M. Wayne Walter
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, see <http://www.tickzoom.org/wiki/Licenses>
 * or write to Free Software Foundation, Inc., 51 Franklin Street,
 * Fifth Floor, Boston, MA  02110-1301, USA.
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TickZoom.Api;

namespace TickZoom.TickUtil
{
	/// <summary>
	/// Description of TickDOM.
	/// </summary>
	unsafe public struct TickImpl : TickIO
	{
		public const byte TickVersion = 7;
		public const int minTickSize = 256;
		
		public static long ToLong( double value) { return value.ToLong(); }
		public static double ToDouble( long value) { return value.ToDouble(); }
		public static double Round( double value) { return value.Round() ; }
		private static string TIMEFORMAT = "yyyy-MM-dd HH:mm:ss.fff";
		
		// Older formats were already multiplied by 1000.
		public const long OlderFormatConvertToLong = 1000000;
		private static Log log = Factory.Log.GetLogger(typeof(TickImpl));
//		public static double UtcOffset = TimeStamp.UtcOffset;

		byte dataVersion;
		TickBinary binary;
		
		public bool IsRealTime {
			get { return false; }
			set { }
		}
		
		public void init(TickBinary tick) {
			binary = tick;
		}
		
		public void init(TickIO tick) {
			if( tick is TickImpl) {
				this = (TickImpl) tick;
			} else {  
				init( tick, tick.ContentMask);
			}
		}
		
		public void init(TickIO tick, byte contentMask) {
			bool dom = (contentMask & ContentBit.DepthOfMarket) != 0;
			bool simulateTicks = (contentMask & ContentBit.SimulateTicks) != 0;
			bool quote = (contentMask & ContentBit.Quote) != 0;
			bool trade = (contentMask & ContentBit.TimeAndSales) != 0;
			binary.Symbol = tick.lSymbol;
			if( quote && !trade) {
				init(tick.UtcTime, tick.lBid, tick.lAsk);
			} else if( trade && !quote) {
				init(tick.UtcTime, tick.Side, tick.lPrice, tick.Size);
				IsSimulateTicks = simulateTicks;
			} else if( quote && trade) {
				init(tick.UtcTime, tick.Side, tick.lPrice, tick.Size, tick.lBid, tick.lAsk);
			} else {
				throw new ApplicationException( "Unknown tick content mask = " + contentMask);
				}
			if( dom) {
				fixed( ushort *b = binary.DepthBidLevels) {
				fixed( ushort *a = binary.DepthAskLevels) {
				for(int i=0;i<TickBinary.DomLevels;i++) {
					*(b+i) = tick.BidLevel(i);
					*(a+i) = tick.AskLevel(i);
				}
				}
				}
			}
			binary.ContentMask2 = contentMask;
			dataVersion = tick.DataVersion;
		}
		
		public void init(TimeStamp utcTime, double dBid, double dAsk) {
			init( utcTime, dBid.ToLong(), dAsk.ToLong());
		}
		
		private void ClearContentMask() {
			binary.ContentMask2 = 0;
		}
		
		public void init(TimeStamp utcTime, long lBid, long lAsk) {
			ClearContentMask();
			dataVersion = TickVersion;
			binary.UtcTime = utcTime;
			IsQuote=true;
			binary.Bid = lBid;
			binary.Ask = lAsk;
		}
		
		public void init(TimeStamp utcTime, double price, int size) {
			init( utcTime, TradeSide.None, price, size);
		}

		public void init(TimeStamp utcTime, byte side, double price, int size) {
			init( utcTime, side, price.ToLong(), size);
		}
		
		public void init(TimeStamp utcTime, byte side, long lPrice, int size) {
			init(utcTime,binary.Price,binary.Price);
			ClearContentMask();
			IsTrade=true;
			binary.Side = side;
			binary.Price = lPrice;
			binary.Size = size;
		}
		
		public void init(TimeStamp utcTime, byte side, double dPrice, int size, double dBid, double dAsk) {
			init(utcTime,side,dPrice.ToLong(),size,dBid.ToLong(),dAsk.ToLong());
		}
		
		public void init(TimeStamp utcTime, byte side, long lPrice, int size, long lBid, long lAsk) {
			init(utcTime,lBid,lAsk);
			ClearContentMask();
			IsQuote = true;
			IsTrade = true;
			binary.Side = side;
			binary.Price = lPrice;
			binary.Size = size;
		}

		public void init(TimeStamp utcTime, byte side, double price, int size, double dBid, double dAsk, ushort[] bidSize, ushort[] askSize) {
			init(utcTime,dBid,dAsk);
			ClearContentMask();
			IsQuote = true;
			IsTrade = true;
			HasDepthOfMarket = true;
			binary.Side = side;
			binary.Price = price.ToLong();
			
			binary.Size = size;
			fixed( ushort *b = binary.DepthBidLevels) {
			fixed( ushort *a = binary.DepthAskLevels) {
				for(int i=0;i<TickBinary.DomLevels;i++) {
					*(b+i) = bidSize[i];;
					*(a+i) = askSize[i];;
				}
			}
			}
		}
		
//		private static readonly Dictionary<byte,int> sizeOfList = new Dictionary<byte,int>();
//		private static object locker = new object();
//		public int Length {
//			get {
//				int retVal;
//				if( !sizeOfList.TryGetValue(binary.ContentMask,out retVal)) {
//					lock(locker) {
//						MemoryStream memory = new MemoryStream(256);
//						ToWriter(memory);
//						sizeOfList[binary.ContentMask] = (int) memory.Length;
//						retVal = (int) memory.Length;
//					}
//				}
//				return retVal;
//			}
//		}
		
		public int BidDepth {
			get { int total = 0;
				fixed( ushort *p = binary.DepthBidLevels) {
				    for(int i=0;i<TickBinary.DomLevels;i++) {
						total += *(p+i);
					}
				}
				return total;
			}
		}
		
		public int AskDepth {
			get { int total = 0;
				fixed( ushort *p = binary.DepthAskLevels) {
				 	for(int i=0;i<TickBinary.DomLevels;i++) {
						total += *(p+i);
					}
				}
				return total;
			}
		}
		
		public override string ToString() {
			string output = Time.ToString(TIMEFORMAT) + " " +
				(IsTrade ? Side + "," + Price.ToString(",0.000") + "," + binary.Size + ", " : "") +
				Bid.ToString(",0.000") + "/" + Ask.ToString(",0.000") + " ";
			fixed( ushort *p = binary.DepthBidLevels) {
				for(int i=TickBinary.DomLevels-1; i>=0; i--) {
					if( i!=0) { output += ","; }
					output += *(p + i);
				}
			}
			output += "|";
			fixed( ushort *p = binary.DepthAskLevels) {
				for(int i=0; i<TickBinary.DomLevels; i++) {
					if( i!=0) { output += ","; }
					output += *(p + i);
				}
			}
			return output;
		}
		
		
		unsafe public void ToWriter(MemoryStream writer) {
			dataVersion = TickVersion;
			writer.SetLength( writer.Position+minTickSize);
			byte[] buffer = writer.GetBuffer();
			fixed( byte *fptr = &buffer[writer.Position]) {
				byte *ptr = fptr;
				*(ptr) = dataVersion; ptr++;
				*(double*)(ptr) = binary.UtcTime.Internal; ptr+=sizeof(double);
				*(ptr) = binary.ContentMask2; ptr++;
				if( IsQuote) {
					*(long*)(ptr) = binary.Bid; ptr += sizeof(long);
					*(long*)(ptr) = binary.Ask; ptr += sizeof(long);
				}
				if( IsTrade) {
					*ptr = binary.Side; ptr ++;
					*(long*)(ptr) = binary.Price; ptr += sizeof(long);
					*(int*)(ptr) = binary.Size; ptr += sizeof(int);
				}
				if( HasDepthOfMarket ) {
					fixed( ushort *p = binary.DepthBidLevels) {
						for( int i=0; i<TickBinary.DomLevels; i++) {
							*(ushort*)(ptr) = *(p + i); ptr+=sizeof(ushort);
						}
					}
					fixed( ushort *p = binary.DepthAskLevels) {
						for( int i=0; i<TickBinary.DomLevels; i++) {
							*(ushort*)(ptr) = *(p + i); ptr+=sizeof(ushort);
						}
					}
				}
				writer.Position += ptr - fptr;
				writer.SetLength(writer.Position);
			}
		}
		private int FromFileVersion7(BinaryReader reader) {
			int position = 0;
			binary.UtcTime.Internal = reader.ReadDouble(); position += 8;
			binary.ContentMask2 = reader.ReadByte(); position += 1;
			if( IsQuote ) {
				binary.Bid = reader.ReadInt64(); position += 8;
				binary.Ask = reader.ReadInt64(); position += 8;
				if( !IsTrade) {
					binary.Price = (binary.Bid+binary.Ask)/2;
				}
			}
			if( IsTrade) {
				binary.Side = reader.ReadByte(); position += 1;
				binary.Price = reader.ReadInt64(); position += 8;
				binary.Size = reader.ReadInt32(); position += 4;
				if( binary.Price == 0) {
					binary.Price = (binary.Bid+binary.Ask)/2;
				}
				if( !IsQuote) {
					binary.Bid = binary.Ask = binary.Price;
				}
			}
			if( HasDepthOfMarket) {
				fixed( ushort *p = binary.DepthBidLevels) {
					for( int i=0; i<TickBinary.DomLevels; i++) {
						*(p+i) = reader.ReadUInt16(); position += 2;
					}
				}
				fixed( ushort *p = binary.DepthAskLevels) {
					for( int i=0; i<TickBinary.DomLevels; i++) {
						*(p+i) = reader.ReadUInt16(); position += 2;
					}
				}
			}
			return position;
		}
		private int FromFileVersion6(BinaryReader reader) {
			int position = 0;
			binary.UtcTime.Internal = reader.ReadDouble(); position += 8;
			binary.Bid = reader.ReadInt64(); position += 8;
			binary.Ask = reader.ReadInt64(); position += 8;
			ClearContentMask();
			IsQuote = true;
			bool dom = reader.ReadBoolean(); position += 1;
			if( dom) {
				IsTrade = true;
				HasDepthOfMarket = true;
				binary.Side = reader.ReadByte(); position += 1;
				binary.Price = reader.ReadInt64(); position += 8;
				if( binary.Price == 0) { binary.Price = (binary.Bid+binary.Ask)/2; }
				binary.Size = reader.ReadInt32(); position += 4;
				fixed( ushort *p = binary.DepthBidLevels) {
					for( int i=0; i<TickBinary.DomLevels; i++) {
						*(p+i) = reader.ReadUInt16(); position += 2;
					}
				}
				fixed( ushort *p = binary.DepthAskLevels) {
					for( int i=0; i<TickBinary.DomLevels; i++) {
						*(p+i) = reader.ReadUInt16(); position += 2;
					}
				}
			}
			return position;
		}

		private int FromFileVersion5(BinaryReader reader) {
			int position = 0;
			binary.UtcTime.Internal = reader.ReadDouble(); position += 8;
			binary.Bid = reader.ReadInt32(); position += 4;
			sbyte spread = reader.ReadSByte();	position += 1;
			binary.Ask = binary.Bid + spread;
			binary.Bid*=OlderFormatConvertToLong;
			binary.Ask*=OlderFormatConvertToLong;
			ClearContentMask();
			IsQuote = true;
			bool hasDOM = reader.ReadBoolean(); position += 1;
			if( hasDOM) {
				IsTrade = true;
				HasDepthOfMarket = true;
				binary.Price = reader.ReadInt32(); position += 4;
				binary.Price*=OlderFormatConvertToLong;
				if( binary.Price == 0) { binary.Price = (binary.Bid+binary.Ask)/2; }
				binary.Size = reader.ReadUInt16(); position += 2;
				fixed( ushort *p = binary.DepthBidLevels) {
					for( int i=0; i<TickBinary.DomLevels; i++) {
						*(p+i) = reader.ReadUInt16(); position += 2;
					}
				}
				fixed( ushort *p = binary.DepthAskLevels) {
					for( int i=0; i<TickBinary.DomLevels; i++) {
						*(p+i) = reader.ReadUInt16(); position += 2;
					}
				}
			}
			return position;
		}

		private int FromFileVersion4(BinaryReader reader) {
			int position = 0;
			reader.ReadByte(); position += 1;
 			// throw away symbol
			for( int i=0; i<TickBinary.SymbolSize; i++) {
				reader.ReadChar(); position += 2;
			}
			binary.UtcTime.Internal = reader.ReadDouble(); position += 8;
			binary.Bid = reader.ReadInt32(); position += 4;
			sbyte spread = reader.ReadSByte();	position += 1;
			binary.Ask = binary.Bid + spread;
			binary.Bid*=OlderFormatConvertToLong;
			binary.Ask*=OlderFormatConvertToLong;
			ClearContentMask();
			IsQuote = true;
			bool hasDOM = reader.ReadBoolean(); position += 1;
			if( hasDOM) {
				IsTrade = true;
				HasDepthOfMarket = true;
				binary.Side = reader.ReadByte(); position += 1;
				binary.Price = reader.ReadInt32(); position += 4;
				binary.Price*=OlderFormatConvertToLong;
				if( binary.Price == 0) { binary.Price = (binary.Bid+binary.Ask)/2; }
				binary.Size = reader.ReadUInt16(); position += 2;
				fixed( ushort *p = binary.DepthBidLevels) {
					for( int i=0; i<TickBinary.DomLevels; i++) {
						*(p+i) = reader.ReadUInt16(); position += 2;
					}
				}
				fixed( ushort *p = binary.DepthAskLevels) {
					for( int i=0; i<TickBinary.DomLevels; i++) {
						*(p+i) = reader.ReadUInt16(); position += 2;
					}
				}
			}
			return position;
		}

		private int FromFileVersion3(BinaryReader reader) {
			int position = 0;
			DateTime tickTime = DateTime.FromBinary(reader.ReadInt64()); position += 8;
			binary.UtcTime = new TimeStamp(tickTime.ToLocalTime());
			binary.Bid = reader.ReadInt32(); position += 4;
			sbyte spread = reader.ReadSByte();	position += 1;
			binary.Ask = binary.Bid+spread;
			binary.Bid*=OlderFormatConvertToLong;
			binary.Ask*=OlderFormatConvertToLong;
			binary.Side = reader.ReadByte(); position += 1;
			binary.Price = reader.ReadInt32(); position += 4;
			binary.Price*=OlderFormatConvertToLong;
			if( binary.Price == 0) { binary.Price = (binary.Bid+binary.Ask)/2; }
			binary.Size = reader.ReadUInt16(); position += 2;
			fixed( ushort *p = binary.DepthBidLevels) {
				for( int i=0; i<TickBinary.DomLevels; i++) {
					*(p+i) = reader.ReadUInt16(); position += 2;
				}
			}
			fixed( ushort *p = binary.DepthAskLevels) {
				for( int i=0; i<TickBinary.DomLevels; i++) {
					*(p+i) = reader.ReadUInt16(); position += 2;
				}
			}
			ClearContentMask();
			IsQuote = true;
			IsTrade = true;
			HasDepthOfMarket = true;
			return position;
		}
		
		private int FromFileVersion2(BinaryReader reader) {
			int position = 0;
			DateTime tickTime = DateTime.FromBinary(reader.ReadInt64()); position += 8;
			binary.UtcTime = new TimeStamp(tickTime.ToLocalTime());
			binary.Bid = reader.ReadInt32(); position += 4;
			sbyte spread = reader.ReadSByte();	position += 1;
			binary.Ask = binary.Bid+spread;
			binary.Bid*=OlderFormatConvertToLong;
			binary.Ask*=OlderFormatConvertToLong;
			fixed( ushort *p = binary.DepthBidLevels) {
				*p = (ushort) reader.ReadInt32(); position += 4;
			}
			fixed( ushort *p = binary.DepthAskLevels) {
				*p = (ushort) reader.ReadInt32(); position += 4;
			}
			ClearContentMask();
			IsQuote = true;
			HasDepthOfMarket = true;
			binary.Side = TradeSide.None;
			binary.Price = (binary.Bid+binary.Ask)/2;
			binary.Size = 0;
			return position;
		}
		
		private int FromFileVersion1(BinaryReader reader) {
			int position = 0;
			
			long int64 = reader.ReadInt64() ^ -9223372036854775808L;
			DateTime tickTime = DateTime.FromBinary(int64); position += 8;
			binary.UtcTime = (TimeStamp) tickTime.AddHours(-4);
			
			binary.Bid = reader.ReadInt32(); position += 4;
			sbyte spread = reader.ReadSByte();	position += 1;
			binary.Ask = binary.Bid+spread;
			binary.Bid*=OlderFormatConvertToLong;
			binary.Ask*=OlderFormatConvertToLong;
			ClearContentMask();
			IsQuote = true;
			binary.Price = (binary.Bid+binary.Ask)/2;
			return position;
		}
		
		private static readonly TickImpl Blank = new TickImpl();
		public int FromReader(BinaryReader reader) {
			this = Blank;
			int position = 0;
			dataVersion = reader.ReadByte(); position++;
			switch( dataVersion) {
				case 1:
					return FromFileVersion1(reader) + position;
				case 2:
					return FromFileVersion2(reader) + position;
				case 3:
					return FromFileVersion3(reader) + position;
				case 4:
					return FromFileVersion4(reader) + position;
				case 5:
					return FromFileVersion5(reader) + position;
				case 6:
					return FromFileVersion6(reader) + position;
				case 7:
					return FromFileVersion7(reader) + position;
				default:
					throw new ApplicationException("Unknown Tick Version Number " + dataVersion);
			}
		}
		
		public int CompareTo(object obj)
		{
			TickImpl other = (TickImpl) obj;
			return binary.UtcTime.CompareTo(other.binary.UtcTime);
		}
		
		public bool memcmp(ushort* array1, ushort* array2) {
			for( int i=0; i<TickBinary.DomLevels; i++) {
				if( *(array1+i) != *(array2+i)) return false;
			}
			return true;
		}
		
		public int CompareTo(ref TickImpl other)
		{
			fixed( ushort*a1 = binary.DepthAskLevels) {
			fixed( ushort*a2 = other.binary.DepthAskLevels) {
			fixed( ushort*b1 = binary.DepthBidLevels) {
			fixed( ushort*b2 = other.binary.DepthBidLevels) {
				return binary.ContentMask2 == other.binary.ContentMask2 &&
					binary.UtcTime == other.binary.UtcTime &&
					binary.Bid == other.binary.Bid &&
					binary.Ask == other.binary.Ask &&
					binary.Side == other.binary.Side &&
					binary.Price == other.binary.Price &&
					binary.Size == other.binary.Size &&
					memcmp( a1, a2) &&
					memcmp( b1, b2) ? 0 :
					binary.UtcTime > other.binary.UtcTime ? 1 : -1;
				}
			}
			}
			}
		}
		
		public byte DataVersion {
			get { return dataVersion; }
		}
		
		public double Bid {
			get { return binary.Bid.ToDouble(); }
		}
		
		public double Ask {
			get { return binary.Ask.ToDouble(); }
		}
		
		public byte Side {
			get { return binary.Side; }
		}
		
		public double Price {
			get {
				if( IsTrade) {
					return binary.Price.ToDouble();
				} else {
					throw new ApplicationException("Sorry. The Price property on a tick can only by accessed\n" +
					                               "if it has trade data. Please, check the IsTrade property.");
				}
			}
		}
		
		public int Size {
			get { return binary.Size; }
		}
		
		public int Volume {
			get { return Size; }
		}
		
		public ushort AskLevel(int level) {
			fixed( ushort *p = binary.DepthAskLevels) {
				return *(p+level);
			}
		}
		
		public ushort BidLevel(int level) {
			fixed( ushort *p = binary.DepthBidLevels) {
				return *(p+level);
			}
		}
		
		TimeStamp localTime;
		public TimeStamp Time {
			get { localTime = binary.UtcTime; localTime.AddDays(localTime.UtcOffset); return localTime; }
			set { binary.UtcTime = value; }
		}
		
		public TimeStamp UTCTime {
			get { return binary.UtcTime; }
			set { binary.UtcTime = value; }
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		
		public override bool Equals(object obj)
		{
			TickImpl other = (TickImpl) obj;
			return CompareTo(ref other) == 0;
		}
		
		public bool Equals(TickImpl other)
		{
			return CompareTo(ref other) == 0;
		}
		
		public byte ContentMask {
			get { return binary.ContentMask2; }
		}
		
		public long lBid {
			get { return binary.Bid; }
			set { binary.Bid = value; }
		}
		public long lAsk {
			get { return binary.Ask; }
			set { binary.Ask = value; }
		}
		
		public long lPrice {
			get { return binary.Price; }
			set { binary.Price = value; }
		}
		
		public TimeStamp UtcTime {
			get { return binary.UtcTime; }
			set { binary.UtcTime = value; }
		}

		public ulong lSymbol {
			get { return binary.Symbol; }
			set { binary.Symbol = value; }
		}
		
		public string Symbol {
			get { return binary.Symbol.ToSymbol(); }
			set { binary.Symbol = value.ToULong(); }
		}
		
		public int DomLevels {
			get { return TickBinary.DomLevels; }
		}
		
		public bool IsQuote {
			get { return (binary.ContentMask2 & ContentBit.Quote) > 0; }
			set {
				if( value ) {
					binary.ContentMask2 |= ContentBit.Quote;
				} else {
					binary.ContentMask2 &= ContentBit.Quote;
				}
			}
		}
		
		public bool IsSimulateTicks {
			get { return (binary.ContentMask2 & ContentBit.SimulateTicks) > 0; }
			set {
				if( value ) {
					binary.ContentMask2 |= ContentBit.SimulateTicks;
				} else {
					binary.ContentMask2 &= ContentBit.SimulateTicks;
				}
			}
		}
		
		public bool IsTrade {
			get { return (binary.ContentMask2 & ContentBit.TimeAndSales) > 0; }
			set {
				if( value ) {
					binary.ContentMask2 |= ContentBit.TimeAndSales;
				} else {
					binary.ContentMask2 &= ContentBit.TimeAndSales;
				}
			}
		}
		
		public bool HasDepthOfMarket {
			get { return (binary.ContentMask2 & ContentBit.DepthOfMarket) > 0; }
			set {
				if( value ) {
					binary.ContentMask2 |= ContentBit.DepthOfMarket;
				} else {
					binary.ContentMask2 &= ContentBit.DepthOfMarket;
				}
			}
		}
		
		public object ToPosition() {
			return binary.UtcTime;
		}
		
		#if DEBUG
		public ushort[] DebugBidDepth {
			get { ushort[] depth = new ushort[TickBinary.DomLevels];
				fixed( ushort *a = this.binary.DepthBidLevels) {
					for( int i= 0; i<TickBinary.DomLevels; i++) {
						depth[i] = *(a+i);
					}
				}
				return depth;
			}
		}
		public ushort[] DebugAskDepth {
			get { ushort[] depth = new ushort[TickBinary.DomLevels];
				fixed( ushort *a = this.binary.DepthAskLevels) {
					for( int i= 0; i<TickBinary.DomLevels; i++) {
						depth[i] = *(a+i);
					}
				}
				return depth;
			}
		}
		#endif
		
		public int Sentiment {
			get { return 0; }
		}
		
		public TickBinary Extract()
		{
			return binary;
		}
	}
}
