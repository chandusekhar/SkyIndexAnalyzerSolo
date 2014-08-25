/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	FontApi
//	Support for Windows API functions related to fonts and glyphs.
//
//	Granotech Limited
//	Author: Uzi Granot
//	Version: 1.0
//	Date: April 1, 2013
//	Copyright (C) 2013 Granotech Limited. All Rights Reserved
//
//	PdfFileWriter C# class library and TestPdfFileWriter test/demo
//  application are free software.
//	They is distributed under the Code Project Open License (CPOL).
//	The document PdfFileWriterReadmeAndLicense.pdf contained within
//	the distribution specify the license agreement and other
//	conditions and notes. You must read this document and agree
//	with the conditions specified in order to use this software.
//
//	Version History:
//
//	Version 1.0 2013/04/01
//		Original revision
//	Version 1.1 2013/04/09
//		Allow program to be compiled in regions that define
//		decimal separator to be non period (comma)
//	Version 1.2 2013/07/21
//		The original revision supported image resources with
//		jpeg file format only.
//		Version 1.2 support all image files acceptable to Bitmap class.
//		See ImageFormat class. The program was tested with:
//		Bmp, Gif, Icon, Jpeg, Png and Tiff.
//
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace PdfFileWriter
{
////////////////////////////////////////////////////////////////////
// PDF Rectangle
// Note: Microsoft rectangle is left, top, width and height
// PDF rectangle is left, bottom, right and top
// PDF Memers precision is Double and Microsoft is Single
////////////////////////////////////////////////////////////////////

public class PdfRectangle
	{
	public Double		Left;
	public Double		Bottom;
	public Double		Right;
	public Double		Top;

	public PdfRectangle() {}

	public PdfRectangle
			(
			Double	Left,
			Double	Bottom,
			Double	Right,
			Double	Top
			)
		{
		this.Left = Left;
		this.Bottom = Bottom;
		this.Right = Right;
		this.Top = Top;
		return;
		}

	public PdfRectangle
			(
			PdfRectangle Rect
			)
		{
		this.Left = Rect.Left;
		this.Bottom = Rect.Bottom;
		this.Right = Rect.Right;
		this.Top = Rect.Top;
		return;
		}
	}

////////////////////////////////////////////////////////////////////
// Glyph Bounding Box
// The BoundingBox class is the output of GetGlyphMetricsApi method.
// It returns the glyph bounding box and character width.
////////////////////////////////////////////////////////////////////

public class BoundingBox
	{
	public PdfRectangle	Rect;
	public Double		Width;

	public BoundingBox() {}

	public BoundingBox
			(
			BoundingBox	Box
			)
		{
		Rect = new PdfRectangle(Box.Rect);
		Width = Box.Width;
		return;
		}

	public BoundingBox
			(
			FontApi DC
			)
		{
		// empty box
		Rect = new PdfRectangle();

		// save width in right
		Rect.Right = DC.WindowsToPdf(DC.ReadInt32());

		// save height
		Double Height = DC.WindowsToPdf(DC.ReadInt32());

		// left
		Rect.Left = DC.WindowsToPdf(DC.ReadInt32());

		// top
		Rect.Top = DC.WindowsToPdf(DC.ReadInt32());

		// adjust right = left + width
		Rect.Right += Rect.Left;

		// adjust bottom = top - height
		Rect.Bottom = Rect.Top - Height;

		// get advanced width
		Width = DC.WindowsToPdf(DC.ReadInt16());
		return;
		}
	}
////////////////////////////////////////////////////////////////////
// FontBox class is part of OUTLINETEXTMETRIC structure
////////////////////////////////////////////////////////////////////

public class FontBox
	{
	public Int32		Left; 
	public Int32		Top; 
	public Int32		Right; 
	public Int32		Bottom;

	public FontBox
			(
			FontApi DC
			)
		{
		Left = DC.ReadInt32();
		Top = DC.ReadInt32(); 
		Right = DC.ReadInt32(); 
		Bottom = DC.ReadInt32();
		return;
		}
	}

////////////////////////////////////////////////////////////////////
// The PANOSE structure describes the PANOSE font-classification
// values for a TrueType font. These characteristics are then
// used to associate the font with other fonts of similar
// appearance but different names.
////////////////////////////////////////////////////////////////////

public class WinPanose
	{
	public Byte			bFamilyType;
	public Byte			bSerifStyle;
	public Byte			bWeight;
	public Byte			bProportion;
	public Byte			bContrast;
	public Byte			bStrokeVariation;
	public Byte			bArmStyle;
	public Byte			bLetterform;
	public Byte			bMidline;
	public Byte			bXHeight;

	public WinPanose
			(
			FontApi DC
			)
		{
		bFamilyType = DC.ReadByte();
		bSerifStyle = DC.ReadByte();
		bWeight = DC.ReadByte();
		bProportion = DC.ReadByte();
		bContrast = DC.ReadByte();
		bStrokeVariation = DC.ReadByte();
		bArmStyle = DC.ReadByte();
		bLetterform = DC.ReadByte();
		bMidline = DC.ReadByte();
		bXHeight = DC.ReadByte();
		return;
		}
	}

////////////////////////////////////////////////////////////////////
// Kerning Pair
////////////////////////////////////////////////////////////////////

public class WinKerningPair : IComparable<WinKerningPair>
	{
	public Char			First;
	public Char			Second;
	public Double		KernAmount;

	public WinKerningPair
			(
			FontApi DC
			)
		{
		First = DC.ReadChar();
		Second = DC.ReadChar();
		KernAmount = DC.WindowsToPdf(DC.ReadInt32());
		return;
		}

	public WinKerningPair
			(
			Char	First,
			Char	Second
			)
		{
		this.First = First;
		this.Second = Second;
		return;
		}

	public Int32 CompareTo
			(
			WinKerningPair	Other
			)
		{
		return(this.First != Other.First ? this.First - Other.First : this.Second - Other.Second);
		}
	}

////////////////////////////////////////////////////////////////////
// The TEXTMETRIC structure contains basic information about a
// physical font. All sizes are specified in logical units;
// that is, they depend on the current mapping mode of the
// display context.
////////////////////////////////////////////////////////////////////

public class WinTextMetric
 	{
	public Int32		tmHeight;
	public Int32		tmAscent;
	public Int32		tmDescent;
	public Int32		tmInternalLeading;
	public Int32		tmExternalLeading;
	public Int32		tmAveCharWidth;
	public Int32		tmMaxCharWidth;
	public Int32		tmWeight;
	public Int32		tmOverhang;
	public Int32		tmDigitizedAspectX;
	public Int32		tmDigitizedAspectY;
	public UInt16		tmFirstChar;
	public UInt16		tmLastChar;
	public UInt16		tmDefaultChar;
	public UInt16		tmBreakChar;
	public Byte			tmItalic;
	public Byte			tmUnderlined;
	public Byte			tmStruckOut;
	public Byte			tmPitchAndFamily;
	public Byte			tmCharSet;

	public WinTextMetric
			(
			FontApi DC
			)
		{
		tmHeight = DC.ReadInt32();
		tmAscent = DC.ReadInt32();
		tmDescent = DC.ReadInt32();
		tmInternalLeading = DC.ReadInt32();
		tmExternalLeading = DC.ReadInt32();
		tmAveCharWidth = DC.ReadInt32();
		tmMaxCharWidth = DC.ReadInt32();
		tmWeight = DC.ReadInt32();
		tmOverhang = DC.ReadInt32();
		tmDigitizedAspectX = DC.ReadInt32();
		tmDigitizedAspectY = DC.ReadInt32();
		tmFirstChar = DC.ReadUInt16();
		tmLastChar = DC.ReadUInt16();
		tmDefaultChar = DC.ReadUInt16();
		tmBreakChar = DC.ReadUInt16();
		tmItalic = DC.ReadByte();
		tmUnderlined = DC.ReadByte();
		tmStruckOut = DC.ReadByte();
		tmPitchAndFamily = DC.ReadByte();
		tmCharSet = DC.ReadByte();
		return;
		}
	}

////////////////////////////////////////////////////////////////////
// The OUTLINETEXTMETRIC structure contains metrics describing
// a TrueType font.
////////////////////////////////////////////////////////////////////

public class WinOutlineTextMetric
	{
	public UInt32		otmSize;
	public WinTextMetric otmTextMetric;
	public WinPanose	otmPanoseNumber;
	public UInt32		otmfsSelection;
	public UInt32		otmfsType;
	public Int32		otmsCharSlopeRise;
	public Int32		otmsCharSlopeRun;
	public Int32		otmItalicAngle;
	public UInt32		otmEMSquare;
	public Int32		otmAscent;
	public Int32		otmDescent;
	public UInt32		otmLineGap;
	public UInt32		otmsCapEmHeight;
	public UInt32		otmsXHeight;
	public FontBox		otmrcFontBox;
	public Int32		otmMacAscent;
	public Int32		otmMacDescent;
	public UInt32		otmMacLineGap;
	public UInt32		otmusMinimumPPEM;
	public Point		otmptSubscriptSize;
	public Point		otmptSubscriptOffset;
	public Point		otmptSuperscriptSize;
	public Point		otmptSuperscriptOffset;
	public UInt32		otmsStrikeoutSize;
	public Int32		otmsStrikeoutPosition;
	public Int32		otmsUnderscoreSize;
	public Int32		otmsUnderscorePosition;
	public String		otmpFamilyName;
	public String		otmpFaceName;
	public String		otmpStyleName;
	public String		otmpFullName;

	public WinOutlineTextMetric
			(
			FontApi DC
			)
		{
		otmSize = DC.ReadUInt32();
		otmTextMetric = new WinTextMetric(DC);
		DC.Align4();
		otmPanoseNumber = new WinPanose(DC);
		DC.Align4();
		otmfsSelection = DC.ReadUInt32();
		otmfsType = DC.ReadUInt32();
		otmsCharSlopeRise = DC.ReadInt32();
		otmsCharSlopeRun = DC.ReadInt32();
		otmItalicAngle = DC.ReadInt32();
		otmEMSquare = DC.ReadUInt32();
		otmAscent = DC.ReadInt32();
		otmDescent = DC.ReadInt32();
		otmLineGap = DC.ReadUInt32();
		otmsCapEmHeight = DC.ReadUInt32();
		otmsXHeight = DC.ReadUInt32();
		otmrcFontBox = new FontBox(DC);
		otmMacAscent = DC.ReadInt32();
		otmMacDescent = DC.ReadInt32();
		otmMacLineGap = DC.ReadUInt32();
		otmusMinimumPPEM = DC.ReadUInt32();
		otmptSubscriptSize = DC.ReadWinPoint();
		otmptSubscriptOffset = DC.ReadWinPoint();
		otmptSuperscriptSize = DC.ReadWinPoint();
		otmptSuperscriptOffset = DC.ReadWinPoint();
		otmsStrikeoutSize = DC.ReadUInt32();
		otmsStrikeoutPosition = DC.ReadInt32();
		otmsUnderscoreSize = DC.ReadInt32();
		otmsUnderscorePosition = DC.ReadInt32();
		otmpFamilyName = DC.ReadString();
		otmpFaceName = DC.ReadString();
		otmpStyleName = DC.ReadString();
		otmpFullName = DC.ReadString();
		return;
		}
	}

////////////////////////////////////////////////////////////////////
// Font API class
// Windows API callable by C# program
////////////////////////////////////////////////////////////////////

public class FontApi : IDisposable
	{
	private Bitmap		BitMap;
	private Graphics	GDI;
	private IntPtr		GDIHandle;
	private IntPtr		Buffer;
	private Int32		BufPtr;
	private Int32		DesignHeight;

	////////////////////////////////////////////////////////////////////
	// Device context constructor
	////////////////////////////////////////////////////////////////////

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern IntPtr SelectObject(IntPtr GDIHandle, IntPtr FontHandle);

	public FontApi
			(
			Font	DesignFont,
			Int32	DesignHeight
			)
		{
		// save design height
		this.DesignHeight = DesignHeight;

		// define device context
		BitMap = new Bitmap(1, 1);
		GDI = Graphics.FromImage(BitMap);
		GDIHandle = (IntPtr) GDI.GetHdc();

		// select the font into the device context
		SelectObject(GDIHandle, DesignFont.ToHfont());

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Get characters width array as Int32[]
    // http://msdn.microsoft.com/en-us/library/windows/desktop/dd144862(v=vs.85).aspx
	////////////////////////////////////////////////////////////////////

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern Int32 GetCharWidth32(IntPtr GDIHandle, Int32 FirstChar, Int32 LastChar, IntPtr WidthArray);

	public Double[] GetCharWidthApi
			(
			Int32	FirstChar,
			Int32	LastChar
			)
		{
		// character count
		Int32 CharCount = LastChar - FirstChar + 1;

		// allocate character table buffer in global memory (four bytes per char)
		AllocateBuffer(4 * CharCount);

		// get characters width
		if(GetCharWidth32(GDIHandle, FirstChar, LastChar, Buffer) == 0) ThrowSystemErrorException("Calling GetCharWidth32 failed");

		// get characters width array to managed memory
		Int32[] Width = ReadInt32Array(CharCount);

		// free memory
		FreeBuffer();

		// convert to PDF format
		Double[] CharWidth = new Double[CharCount];
		for(Int32 Index = 0; Index < CharCount; Index++) CharWidth[Index] = WindowsToPdf(Width[Index]);

		// exit
		return(CharWidth);
		}

	////////////////////////////////////////////////////////////////////
	// Get single glyph metric
	////////////////////////////////////////////////////////////////////

    private const UInt32 GGO_METRICS = 0;
    private const UInt32 GGO_BITMAP = 1;
    private const UInt32 GGO_NATIVE = 2;
    private const UInt32 GGO_BEZIER = 3;

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern Int32 GetGlyphOutline(IntPtr GDIHandle, Int32 CharIndex,
        UInt32 GgoFormat, IntPtr GlyphMetrics, UInt32 Zero, IntPtr Null, IntPtr TransMatrix);

	public BoundingBox GetGlyphMetricsApi
			(
			Char	CharCode
			)
		{
		// build unit matrix
		IntPtr UnitMatrix = BuildUnitMarix();

		// allocate buffer to receive glyph metrics information
		AllocateBuffer(20);

		// get one glyph
		if(GetGlyphOutline(GDIHandle, (Int32) CharCode, GGO_METRICS, Buffer, 0, IntPtr.Zero, UnitMatrix) < 0)
			ThrowSystemErrorException("Calling GetGlyphOutline failed");

		// create WinOutlineTextMetric class
		BoundingBox Box = new BoundingBox(this);

		// free buffer for glyph metrics
		FreeBuffer();

		// free unit matrix buffer
		Marshal.FreeHGlobal(UnitMatrix);
		
		// exit
		return(Box);
		}

	////////////////////////////////////////////////////////////////////
	// Get array of glyph metrics
	////////////////////////////////////////////////////////////////////

	public BoundingBox[] GetGlyphMetricsApi
			(
			Int32	FirstChar,
			Int32	LastChar
			)
		{
		// build unit matrix
		IntPtr UnitMatrix = BuildUnitMarix();

		// allocate buffer to receive glyph metrics information
		AllocateBuffer(20);

		// result array
		BoundingBox[] BoxArray = new BoundingBox[LastChar - FirstChar + 1];

		// loop for all characters
		for(Int32 CharPtr = FirstChar; CharPtr <= LastChar; CharPtr++)
			{
			// get one glyph
			if(GetGlyphOutline(GDIHandle, CharPtr, GGO_METRICS, Buffer, 0, IntPtr.Zero, UnitMatrix) < 0)
				ThrowSystemErrorException("Calling GetGlyphOutline failed");

			// reset buffer pointer
			BufPtr = 0;

			// create WinOutlineTextMetric class
			BoxArray[CharPtr - FirstChar] = new BoundingBox(this);
			}

		// free buffer for glyph metrics
		FreeBuffer();

		// free unit matrix buffer
		Marshal.FreeHGlobal(UnitMatrix);
		
		// exit
		return(BoxArray);
		}

	////////////////////////////////////////////////////////////////////
	// Get kerning pairs array
	////////////////////////////////////////////////////////////////////

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern UInt32 GetKerningPairs(IntPtr GDIHandle, UInt32 NumberOfPairs, IntPtr PairArray);

	public WinKerningPair[] GetKerningPairsApi
			(
			Int32	FirstChar,
			Int32	LastChar
			)
		{
		// get number of pairs
		Int32 Pairs = (Int32) GetKerningPairs(GDIHandle, 0, IntPtr.Zero);
		if(Pairs == 0) return(null);

		// allocate buffer to receive outline text metrics information
		AllocateBuffer(8 * Pairs);

		// get outline text metrics information
		if(GetKerningPairs(GDIHandle, (UInt32) Pairs, Buffer) == 0) ThrowSystemErrorException("Calling GetKerningPairs failed");

		// create list because the program will ignore pairs that are outside char range
		List<WinKerningPair> TempList = new List<WinKerningPair>();

		// kerning pairs from buffer
		for(Int32 Index = 0; Index < Pairs; Index++)
			{
			WinKerningPair KPair = new WinKerningPair(this);
			if(KPair.First >= FirstChar && KPair.First <= LastChar && KPair.Second >= FirstChar && KPair.Second <= LastChar) TempList.Add(KPair);
			}

		// free buffer for outline text metrics
		FreeBuffer();

		// list is empty
		if(TempList.Count == 0) return(null);

		// sort list
		TempList.Sort();

		// exit
		return(TempList.ToArray());
		}

	////////////////////////////////////////////////////////////////////
	// Get OUTLINETEXTMETRICW structure
	////////////////////////////////////////////////////////////////////

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern Int32 GetOutlineTextMetrics(IntPtr GDIHandle, Int32 BufferLength, IntPtr Buffer);

	public WinOutlineTextMetric GetOutlineTextMetricsApi()
		{
		// get buffer size
		Int32 BufSize = GetOutlineTextMetrics(GDIHandle, 0, IntPtr.Zero);
		if(BufSize == 0) ThrowSystemErrorException("Calling GetOutlineTextMetrics (get buffer size) failed");

		// allocate buffer to receive outline text metrics information
		AllocateBuffer(BufSize);

		// get outline text metrics information
		if(GetOutlineTextMetrics(GDIHandle, BufSize, Buffer) == 0) ThrowSystemErrorException("Calling GetOutlineTextMetrics failed");

		// create WinOutlineTextMetric class
		WinOutlineTextMetric WOTM = new WinOutlineTextMetric(this);

		// free buffer for outline text metrics
		FreeBuffer();

		// exit
		return(WOTM);
		}

	////////////////////////////////////////////////////////////////////
	// Get TEXTMETRICW structure
	////////////////////////////////////////////////////////////////////

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern Int32 GetTextMetrics(IntPtr GDIHandle, IntPtr Buffer);

	public WinTextMetric GetTextMetricsApi()
		{
		// allocate buffer to receive outline text metrics information
		AllocateBuffer(57);

		// get outline text metrics information
		if(GetTextMetrics(GDIHandle, Buffer) == 0) ThrowSystemErrorException("Calling GetTextMetrics API failed.");

		// create WinOutlineTextMetric class
		WinTextMetric WTM = new WinTextMetric(this);

		// free buffer for outline text metrics
		FreeBuffer();

		// exit
		return(WTM);
		}

	////////////////////////////////////////////////////////////////////
	// Get font data tables
	////////////////////////////////////////////////////////////////////

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention=CallingConvention.StdCall, SetLastError = true)]
	private static extern UInt32 GetFontData(IntPtr DeviceContextHandle, UInt32 Table, UInt32 Offset, IntPtr Buffer, UInt32 BufferLength);

	public Byte[] GetFontDataApi
			(
			Int32	Offset,
			Int32	BufSize
			)
		{
		// empty table
		if(BufSize == 0) return(null);

		// allocate buffer to receive outline text metrics information
		AllocateBuffer((Int32) BufSize);

		// get outline text metrics information
		if((Int32) GetFontData(GDIHandle, 0, (UInt32) Offset, Buffer, (UInt32) BufSize) != BufSize) ThrowSystemErrorException("Get font data file header failed");

		// get result to managed memory
		return(CopyBuffer(BufSize));
		}

	////////////////////////////////////////////////////////////////////
	// Get glyph indices array
	////////////////////////////////////////////////////////////////////

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern Int32 GetGlyphIndices(IntPtr GDIHandle, IntPtr CharBuffer, Int32 CharCount,
        IntPtr GlyphArray, UInt32 GlyphOptions);

    public Int16[] GetGlyphIndicesApi
			(
			Int32		FirstChar,
			Int32		LastChar
			)
		{
		// character count
		Int32 CharCount = LastChar - FirstChar + 1;

		// allocate character table buffer in global memory (two bytes per char)
		IntPtr CharBuffer = Marshal.AllocHGlobal(2 * CharCount);

		// create array of all character codes between FirstChar and LastChar (we use Int16 because of Unicode)
		for(Int32 CharPtr = FirstChar; CharPtr <= LastChar; CharPtr++) Marshal.WriteInt16(CharBuffer, 2 * (CharPtr - FirstChar), (Int16) (CharPtr));

		// allocate memory for result
		AllocateBuffer(2 * CharCount);

		// get glyph numbers for all characters including non existing glyphs
		if(GetGlyphIndices(GDIHandle, CharBuffer, CharCount, Buffer, (UInt32) 0) != CharCount) ThrowSystemErrorException("Calling GetGlypeIndices failed");

		// get result array to managed code
		Int16[] GlyphCode = ReadInt16Array(CharCount);

		// free local buffer
		Marshal.FreeHGlobal(CharBuffer);

		// free result buffer
		FreeBuffer();

		// exit
		return(GlyphCode);
		}

	////////////////////////////////////////////////////////////////////
	// Allocate API result buffer
	////////////////////////////////////////////////////////////////////

	private void AllocateBuffer
			(
			Int32	Size
			)
		{
		// allocate memory for result
		Buffer = Marshal.AllocHGlobal(Size);
		BufPtr = 0;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Free API result buffer
	////////////////////////////////////////////////////////////////////

	private void FreeBuffer()
		{
		// free buffer
		Marshal.FreeHGlobal(Buffer);
		Buffer = IntPtr.Zero;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Copy result buffer to byte array
	////////////////////////////////////////////////////////////////////

	private Byte[] CopyBuffer
			(
			Int32 BufSize
			)
		{
		// copy api result buffer to managed memory buffer
		Byte[] DataBuffer = new Byte[BufSize];
		Marshal.Copy(Buffer, DataBuffer, 0, BufSize);
		BufPtr = 0;

		// free unmanaged memory buffer
		FreeBuffer();

		// exit
		return(DataBuffer);
		}

	////////////////////////////////////////////////////////////////////
	// Align buffer pointer to 4 bytes boundry
	////////////////////////////////////////////////////////////////////

	internal void Align4()
		{
		BufPtr = (BufPtr + 3) & ~3;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Read point (x, y) from data buffer
	////////////////////////////////////////////////////////////////////

	internal Point ReadWinPoint()
		{
		return(new Point(ReadInt32(), ReadInt32()));
		} 

	////////////////////////////////////////////////////////////////////
	// Read byte from data buffer
	////////////////////////////////////////////////////////////////////

	internal Byte ReadByte()
		{
		return(Marshal.ReadByte(Buffer, BufPtr++));
		}

	////////////////////////////////////////////////////////////////////
	// Read character from data buffer
	////////////////////////////////////////////////////////////////////

	internal Char ReadChar()
		{
		Char Value = (Char) Marshal.ReadInt16(Buffer, BufPtr);
		BufPtr += 2;
		return(Value);
		}

	////////////////////////////////////////////////////////////////////
	// Read short integer from data buffer
	////////////////////////////////////////////////////////////////////

	internal Int16 ReadInt16()
		{
		Int16 Value = Marshal.ReadInt16(Buffer, BufPtr);
		BufPtr += 2;
		return(Value);
		}

	////////////////////////////////////////////////////////////////////
	// Read unsigned short integer from data buffer
	////////////////////////////////////////////////////////////////////

	internal UInt16 ReadUInt16()
		{
		UInt16 Value = (UInt16) Marshal.ReadInt16(Buffer, BufPtr);
		BufPtr += 2;
		return(Value);
		}

	////////////////////////////////////////////////////////////////////
	// Read Int16 array from result buffer
	////////////////////////////////////////////////////////////////////

	internal Int16[] ReadInt16Array
			(
			Int32	Size
			)
		{
		// create active characters array
		Int16[] Result = new Int16[Size];
		Marshal.Copy(Buffer, Result, 0, Size);
		return(Result);
		}

	////////////////////////////////////////////////////////////////////
	// Read integers from data buffer
	////////////////////////////////////////////////////////////////////

	internal Int32 ReadInt32()
		{
		Int32 Value = Marshal.ReadInt32(Buffer, BufPtr);
		BufPtr += 4;
		return(Value);
		}

	////////////////////////////////////////////////////////////////////
	// Read Int32 array from result buffer
	////////////////////////////////////////////////////////////////////

	internal Int32[] ReadInt32Array
			(
			Int32	Size
			)
		{
		// create active characters array
		Int32[] Result = new Int32[Size];
		Marshal.Copy(Buffer, Result, 0, Size);
		return(Result);
		}

	////////////////////////////////////////////////////////////////////
	// Read unsigned integers from data buffer
	////////////////////////////////////////////////////////////////////

	internal UInt32 ReadUInt32()
		{
		UInt32 Value = (UInt32) Marshal.ReadInt32(Buffer, BufPtr);
		BufPtr += 4;
		return(Value);
		}

	////////////////////////////////////////////////////////////////////
	// Read string (null terminated0 from data buffer
	////////////////////////////////////////////////////////////////////

	internal String ReadString()
		{
		Int32 Ptr = Marshal.ReadInt32(Buffer, BufPtr);
		BufPtr += 4;
		StringBuilder Str = new StringBuilder();
		for(;;)
			{
			Char Chr = (Char) Marshal.ReadInt16(Buffer, Ptr);
			if(Chr == 0) break;
			Str.Append(Chr);
			Ptr += 2;
			}
		return(Str.ToString());
		}

	////////////////////////////////////////////////////////////////////
	// Windows design units to PDF design units
	////////////////////////////////////////////////////////////////////

	internal Double WindowsToPdf
			(
			Int32	Value
			)
		{
		return((Double) Value / (Double) DesignHeight);
		}

	////////////////////////////////////////////////////////////////////
	// Throw exception showing last system error
	////////////////////////////////////////////////////////////////////

	[DllImport("Kernel32.dll", CharSet = CharSet.Auto, CallingConvention=CallingConvention.StdCall, SetLastError = true)]
	private static extern UInt32 FormatMessage(UInt32 dwFlags, IntPtr lpSource, UInt32 dwMessageId, UInt32 dwLanguageId,
		IntPtr lpBuffer, UInt32 nSize, IntPtr Arguments);

	internal void ThrowSystemErrorException
			(
			String AppMsg
			)
		{
		const UInt32 FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;

		// error message
		StringBuilder ErrMsg = new StringBuilder(AppMsg);

		// get last system error
		UInt32 ErrCode = (UInt32) Marshal.GetLastWin32Error(); // GetLastError();
		if(ErrCode != 0)
			{
			// allocate buffer
			IntPtr ErrBuffer = Marshal.AllocHGlobal(1024);

			// add error code
			ErrMsg.AppendFormat("\r\nSystem error [0x{0:X8}]", ErrCode);

			// convert error code to text
			Int32 StrLen = (Int32) FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, ErrCode, 0, ErrBuffer, 1024, IntPtr.Zero);
			if(StrLen > 0)
				{
				ErrMsg.Append(" ");
				ErrMsg.Append(Marshal.PtrToStringAuto(ErrBuffer, StrLen));
				while(ErrMsg[ErrMsg.Length - 1] <= ' ') ErrMsg.Length--;
				}

			// free buffer
			Marshal.FreeHGlobal(ErrBuffer);
			}

		// unknown error
		else
			{
			ErrMsg.Append("\r\nUnknown error.");
			}

		// exit
		throw new ApplicationException(ErrMsg.ToString());
		}

	////////////////////////////////////////////////////////////////////
	// Build unit matrix in unmanaged memory
	////////////////////////////////////////////////////////////////////

	private IntPtr BuildUnitMarix()
		{
		// allocate buffer for transformation matrix
		IntPtr UnitMatrix = Marshal.AllocHGlobal(16);

		// set transformation matrix into unit matrix
		Marshal.WriteInt32(UnitMatrix, 0, 0x10000);			
		Marshal.WriteInt32(UnitMatrix, 4, 0);			
		Marshal.WriteInt32(UnitMatrix, 8, 0);			
		Marshal.WriteInt32(UnitMatrix, 12, 0x10000);			
		return(UnitMatrix);
		}

	////////////////////////////////////////////////////////////////////
	// Dispose
	////////////////////////////////////////////////////////////////////

	public void Dispose()
		{
		// free glyph matrix buffer
		Marshal.FreeHGlobal(Buffer);

		// release device context
		GDI.ReleaseHdc();

		// exit
		return;		
		}
	}
}
