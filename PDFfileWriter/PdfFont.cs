/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfFont
//	PDF Font resource.
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
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace PdfFileWriter
{
/////////////////////////////////////////////////////////////////////
// PDF font descriptor flags
/////////////////////////////////////////////////////////////////////

public enum PdfFontFlags
	{
	None = 0,
	FixedPitch = 1,
	Serif = 1 << 1,
	Symbolic = 1 << 2,
	Script = 1 << 3,
	Nonsymbolic = 1 << 5,
	Italic = 1 << 6,
	AllCap = 1 << 16,
	SmallCap = 1 << 17,
	ForceBold = 1 << 18
	}

/////////////////////////////////////////////////////////////////////
// Text position adjustment for TJ operator.
// The adjustment is for a font height of one point.
// Mainly used for font kerning
/////////////////////////////////////////////////////////////////////

public class KerningAdjust
	{
	public String	Text;
	public Double	Adjust;

	public KerningAdjust
			(
			String	Text,
			Double	Adjust
			)
		{
		this.Text = Text;
		this.Adjust = Adjust;
		return;
		}
	}

////////////////////////////////////////////////////////////////////
// PDF font object
////////////////////////////////////////////////////////////////////

public class PdfFont : PdfObject
	{
	internal	FontApi			FontInfo;
	internal	Boolean[]		ActiveChar;			// 0 to 255
	internal	Int32[]			CharSubArray;		// 0 to 255
	internal	Boolean			SymbolicFont;

	private		FontFamily		FontFamily;
	private		Boolean			EmbeddedFont;
	private		Font			DesignFont;
	private		PdfFontFlags	FontFlags;
	private		Double			PdfLineSpacing;
	private		Double			PdfAscent;
	private		Double			PdfDescent;
	private		Double			PdfLeading {get{return(PdfLineSpacing - PdfAscent - PdfDescent);}}
	private		Double			PdfCapHeight;
	private		Double			PdfStrikeoutWidth;
	private		Double			PdfStrikeoutPosition;
	private		Double			PdfUnderlineWidth;
	private		Double			PdfUnderlinePosition;
	private		Double			PdfSubscriptSize;
	private		Double			PdfSubscriptPosition;
	private		Double			PdfSuperscriptSize;
	private		Double			PdfSuperscriptPosition;
	private		Double			PdfItalicAngle;
	private		Int32			PdfFontWeight;
	private		Int32			DesignHeight;
	private		PdfObject		FontDescriptor;
	private		PdfObject		FontWidthArray;
	private		Double[]		CharWidthArray;		// 0 to 255
	private		BoundingBox[]	GlyphArray;

	////////////////////////////////////////////////////////////////////
	// Constructor
	////////////////////////////////////////////////////////////////////

	public PdfFont
			(
			PdfDocument		Document,			// PDF document main object
			String			FontFamilyName,		// font family name
			FontStyle		FontStyle,			// font style (Regular, Bold, Italic or Bold | Italic
			Boolean			EmbeddedFont		// embed font in PDF document file
			) : base(Document, false, "/Font")
		{
		// save embedded font flag
		this.EmbeddedFont = EmbeddedFont;
 
		// font style cannot be underline or strikeout
		if((FontStyle & (FontStyle.Underline | FontStyle.Strikeout)) != 0)
			throw new ApplicationException("Font resource cannot have underline or strikeout");

		// create resource code
		ResourceCode = Document.GenerateResourceNumber('F');

		// get font family structure
		FontFamily = new FontFamily(FontFamilyName);

		// test font style availability
		if(!FontFamily.IsStyleAvailable(FontStyle)) throw new ApplicationException("Font style not available for font family");

		// design height
		DesignHeight = FontFamily.GetEmHeight(FontStyle);

		// Ascent, descent and line spacing for a one point font
		PdfAscent = WindowsToPdf(FontFamily.GetCellAscent(FontStyle));
		PdfDescent = WindowsToPdf(FontFamily.GetCellDescent(FontStyle)); // positive number
		PdfLineSpacing = WindowsToPdf(FontFamily.GetLineSpacing(FontStyle));

		// create design font
		DesignFont = new Font(FontFamily, DesignHeight, FontStyle, GraphicsUnit.Pixel);

		// create windows sdk font info object
		FontInfo = new FontApi(DesignFont, DesignHeight);

		// get character width array
		CharWidthArray = FontInfo.GetCharWidthApi(0, 255);

		// get array of glyph bounding box and width
		GlyphArray = FontInfo.GetGlyphMetricsApi(0, 255);

		// reset active (used) characters to not used
		ActiveChar = new Boolean[256];

		// get outline text metrics structure
		WinOutlineTextMetric OTM = FontInfo.GetOutlineTextMetricsApi();

		// license
		if((OTM.otmfsType & 2) != 0) throw new ApplicationException("Font " + FontFamilyName + " is not licensed for embedding");

		// make sure we have true type font and not device font
		if((OTM.otmTextMetric.tmPitchAndFamily & 0xe) != 6) throw new ApplicationException("Font must be True Type and vector"); 

		// PDF font flags
		FontFlags = 0;
		if((OTM.otmfsSelection & 1) != 0) FontFlags |= PdfFontFlags.Italic;
		// roman font is a serif font
		if((OTM.otmTextMetric.tmPitchAndFamily >> 4) == 1) FontFlags |= PdfFontFlags.Serif;
		if((OTM.otmTextMetric.tmPitchAndFamily >> 4) == 4) FontFlags |= PdfFontFlags.Script;
		// #define SYMBOL_CHARSET 2
		if(OTM.otmTextMetric.tmCharSet == 2)
			{
			FontFlags |= PdfFontFlags.Symbolic;
			SymbolicFont = true;
			}
		else
			{
			FontFlags |= PdfFontFlags.Nonsymbolic;
			SymbolicFont = false;
			}

		// #define TMPF_FIXED_PITCH 0x01 (Note very carefully that those meanings are the opposite of what the constant name implies.)
		if((OTM.otmTextMetric.tmPitchAndFamily & 1) == 0) FontFlags |= PdfFontFlags.FixedPitch;

		// strikeout
		PdfStrikeoutPosition = WindowsToPdf(OTM.otmsStrikeoutPosition);
		PdfStrikeoutWidth = WindowsToPdf((Int32) OTM.otmsStrikeoutSize);

		// underline
		PdfUnderlinePosition = WindowsToPdf(OTM.otmsUnderscorePosition);
		PdfUnderlineWidth = WindowsToPdf(OTM.otmsUnderscoreSize);

		// subscript
		PdfSubscriptSize = WindowsToPdf(OTM.otmptSubscriptSize.Y);
		PdfSubscriptPosition = WindowsToPdf(OTM.otmptSubscriptOffset.Y);

		// superscript
		PdfSuperscriptSize = WindowsToPdf(OTM.otmptSuperscriptSize.Y);
		PdfSuperscriptPosition = WindowsToPdf(OTM.otmptSuperscriptOffset.Y);

		PdfItalicAngle = (Double) OTM.otmItalicAngle / 10.0;
		PdfFontWeight = OTM.otmTextMetric.tmWeight;

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Test for valid Characte
	// This program accepts character codes 32 to 126 and 160 to 255
	////////////////////////////////////////////////////////////////////

	public void ValidateChar
			(
			Char	TestChar
			)
		{
		if(TestChar < ' ' || TestChar > '~' && TestChar < 160 || TestChar > 255)
			throw new ApplicationException("No support for control characters or characters greater than 255");
		}

	////////////////////////////////////////////////////////////////////
	// Character substitution
	// This program accepts character codes 32 to 126 and 160 to 255
	// If the font supports characters above 255, you can map
	// this codes into the valid region.
	////////////////////////////////////////////////////////////////////

	public void CharSubstitution
			(
			Int32	OrigFrom,
			Int32	OrigTo,
			Int32	DestFrom
			)
		{
		// font must be embedded
		if(!EmbeddedFont) throw new ApplicationException("Character substitution is allowed only for embedded fonts");

		// create new character substitution array
		if(CharSubArray == null)
			{
			CharSubArray = new Int32[256];
			for(Int32 CharCode = 0; CharCode < 256; CharCode++) CharSubArray[CharCode] = CharCode;
			}

		// character count
		Int32 Count = OrigTo - OrigFrom + 1;

		// get character width
		Double[] SubWidthArray = FontInfo.GetCharWidthApi(OrigFrom, OrigTo);

		// get array of glyph bounding box and width
		BoundingBox[] SubGlyphArray = FontInfo.GetGlyphMetricsApi(OrigFrom, OrigTo);

		// replace existing character width and bounding box
		for(Int32 Ptr = 0; Ptr < Count; Ptr++)
			{
			CharSubArray[DestFrom + Ptr] = OrigFrom + Ptr;
			CharWidthArray[DestFrom + Ptr] = SubWidthArray[Ptr];
			GlyphArray[DestFrom + Ptr] = SubGlyphArray[Ptr];
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Convert C# text to PDF text
	////////////////////////////////////////////////////////////////////

	public String PdfText
			(
			String		Text
			)
		{
		Boolean NonPrintable = false;

		// update used characters array
		foreach(Char Chr in Text)
			{
			ValidateChar(Chr);
			ActiveChar[Chr] = true;
			if(Chr > '~') NonPrintable = true;
			}

		// text is all printable characters (' ' to '~')
		if(!NonPrintable)
			{
			// test for ( and ) and \
			Int32 Index = Text.IndexOfAny(new Char[] {'(', ')', '\\'});

			// none found
			if(Index < 0) return("(" + Text + ")");

			// add backslash
			StringBuilder NewText = new StringBuilder(Text);
			NewText.Replace("\\", "\\\\");
			NewText.Replace("(", "\\(");
			NewText.Replace(")", "\\)");
			NewText.Insert(0, "(");
			NewText.Append(")");
			return(NewText.ToString());
			}

		// convert to hex string if at least one character is over 0x80
		StringBuilder HexText = new StringBuilder("<");
		foreach(Char Chr in Text) HexText.AppendFormat("{0:x2}", (Int32) Chr);
		HexText.Append(">");
		return(HexText.ToString());
		}

	////////////////////////////////////////////////////////////////////
	// Windows design units to PDF design units
	// Font size is set to 1 point
	////////////////////////////////////////////////////////////////////

	public Double WindowsToPdf
			(
			Int32	Value
			)
		{
		return((Double) Value / (Double) DesignHeight);
		}

	////////////////////////////////////////////////////////////////////
	// font units to points
	////////////////////////////////////////////////////////////////////

	public Double FontUnitsToPoints
			(
			Double		FontSize,
			Double		Value
			)
		{
		return(Value * FontSize);
		}

	////////////////////////////////////////////////////////////////////
	// font units to user units
	////////////////////////////////////////////////////////////////////

	public Double FontUnitsToUserUnits
			(
			Double		FontSize,
			Double		Value
			)
		{
		return(Value * FontSize / ScaleFactor);
		}

	////////////////////////////////////////////////////////////////////
	// font units to PDF dictionary values
	////////////////////////////////////////////////////////////////////

	public Double FontUnitsToPdfDic
			(
			Double		Value
			)
		{
		return(Round(1000.0 * Value));
		}

	////////////////////////////////////////////////////////////////////
	// Line spacing in user units
	////////////////////////////////////////////////////////////////////

	public Double LineSpacing
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfLineSpacing));
		}

	////////////////////////////////////////////////////////////////////
	// Font ascent in user units
	////////////////////////////////////////////////////////////////////

	public Double Ascent
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfAscent));
		}

	////////////////////////////////////////////////////////////////////
	// Font ascent in user units
	////////////////////////////////////////////////////////////////////

	public Double AscentPlusLeading
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfAscent + PdfLeading / 2));
		}

	////////////////////////////////////////////////////////////////////
	// Font descent in user units
	////////////////////////////////////////////////////////////////////

	public Double Descent
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfDescent));
		}

	////////////////////////////////////////////////////////////////////
	// Font descent in user units
	////////////////////////////////////////////////////////////////////

	public Double DescentPlusLeading
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfDescent + PdfLeading / 2));
		}

	////////////////////////////////////////////////////////////////////
	// Font capital M height in user units
	////////////////////////////////////////////////////////////////////

	public Double CapHeight
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfCapHeight));
		}

	////////////////////////////////////////////////////////////////////
	// Strikeout Position in user units
	////////////////////////////////////////////////////////////////////

	public Double StrikeoutPosition
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfStrikeoutPosition));
		}

	////////////////////////////////////////////////////////////////////
	// Strikeout width in user units
	////////////////////////////////////////////////////////////////////

	public Double StrikeoutWidth
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfStrikeoutWidth));
		}

	////////////////////////////////////////////////////////////////////
	// Underline position in user units
	////////////////////////////////////////////////////////////////////

	public Double UnderlinePosition
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfUnderlinePosition));
		}

	////////////////////////////////////////////////////////////////////
	// Underline width in user units
	////////////////////////////////////////////////////////////////////

	public Double UnderlineWidth
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfUnderlineWidth));
		}

	////////////////////////////////////////////////////////////////////
	// Subscript position in user units
	////////////////////////////////////////////////////////////////////

	public Double SubscriptPosition
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfSubscriptPosition));
		}

	////////////////////////////////////////////////////////////////////
	// Subscript size in points
	////////////////////////////////////////////////////////////////////

	public Double SubscriptSize
			(
			Double		FontSize
			)
		{
		return(FontUnitsToPoints(FontSize, PdfSubscriptSize));
		}

	////////////////////////////////////////////////////////////////////
	// Superscript position in user units
	////////////////////////////////////////////////////////////////////

	public Double SuperscriptPosition
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfSuperscriptPosition));
		}

	////////////////////////////////////////////////////////////////////
	// Superscript size in points
	////////////////////////////////////////////////////////////////////

	public Double SuperscriptSize
			(
			Double		FontSize
			)
		{
		return(FontUnitsToPoints(FontSize, PdfSuperscriptSize));
		}

	////////////////////////////////////////////////////////////////////
	// Character width
	////////////////////////////////////////////////////////////////////

	public Double CharWidth
			(
			Double	FontSize,
			Char	CharValue
			)
		{
		return(FontUnitsToUserUnits(FontSize, CharWidthArray[(Byte) CharValue]));
		}

	////////////////////////////////////////////////////////////////////
	// Character width
	////////////////////////////////////////////////////////////////////

	public Double CharWidth
			(
			Double		FontSize,
			DrawStyle	DrawStyle,
			Char		CharValue
			)
		{
		if((DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == 0) return(FontUnitsToUserUnits(FontSize, CharWidthArray[(Byte) CharValue]));

		if((DrawStyle & DrawStyle.Superscript) != 0) return(FontUnitsToUserUnits(SubscriptSize(FontSize), CharWidthArray[(Byte) CharValue]));

		return(FontUnitsToUserUnits(SuperscriptSize(FontSize), CharWidthArray[(Byte) CharValue]));
		}

	////////////////////////////////////////////////////////////////////
	// Text width
	////////////////////////////////////////////////////////////////////

	public Double TextWidth
			(
			Double	FontSize,
			String	Text
			)
		{
		Double Width = 0;
		for(Int32 Index = 0; Index < Text.Length; Index++)
			{
			// current character
			Char Chr = Text[Index];

			// validate
			ValidateChar(Chr);

			// character width
			Width += CharWidthArray[(Byte) Chr];
			}

		// to user unit of measure
		return(FontUnitsToUserUnits(FontSize, Width));
		}

	////////////////////////////////////////////////////////////////////
	// Word spacing to stretch text to given width
	////////////////////////////////////////////////////////////////////

	public Boolean TextFitToWidth
			(
			Double		FontSize,
			Double		ReqWidth,
			out Double	WordSpacing,
			out Double	CharSpacing,
			String		Text
			)
		{
		WordSpacing = 0;
		CharSpacing = 0;
		if(Text == null || Text.Length < 2) return(false);

		Double Width = 0;
		Int32 SpaceCount = 0;
		foreach(Char Chr in Text)
			{
			// validate
			ValidateChar(Chr);

			// character width
			Width += CharWidthArray[(Byte) Chr];

			// space count
			if(Chr == ' ') SpaceCount++;
			}

		// to user unit of measure
		Width = FontUnitsToUserUnits(FontSize, Width);

		// extra spacing required
		Double ExtraSpace = ReqWidth - Width;

		// highest possible resolution (12000 dots per inch)
		Double MaxRes = 0.006 / ScaleFactor;

		// string is too wide
		if(ExtraSpace < (-MaxRes)) return(false);

		// string is just right
		if(ExtraSpace < MaxRes) return(true);

		// String does not have any space
		if(SpaceCount == 0)
			{
			CharSpacing = ExtraSpace / (Text.Length - 1);
			return(true);
			}

		// extra space per word
		WordSpacing = ExtraSpace / SpaceCount;

		// extra space is equal or less than one blank
		if(WordSpacing <= FontUnitsToUserUnits(FontSize, CharWidthArray[(Byte) ' '])) return(true);

		// extra space is larger that one blank
		// increase character and word spacing
		CharSpacing = ExtraSpace / (10 * SpaceCount + Text.Length - 1);
		WordSpacing = 10 * CharSpacing;
		return(true);
		}

	////////////////////////////////////////////////////////////////////
	// Text bounding box in user coordinate units
	////////////////////////////////////////////////////////////////////

	public BoundingBox TextBoundingBox
			(
			Double	FontSize,
			String	Text
			)
		{
		if(String.IsNullOrEmpty(Text)) return(null);

		// validate first character
		ValidateChar(Text[0]);

		// initialize result box to first character
		BoundingBox Box = new BoundingBox(GlyphArray[(Byte) Text[0]]);

		// loop from second character
		for(Int32 Index = 1; Index < Text.Length; Index++)
			{
			// current character
			Char Chr = Text[Index];

			// validate
			ValidateChar(Chr);

			// get bounding box for current character
			BoundingBox TempBox = GlyphArray[(Byte) Chr];

			// update bottom
			if(TempBox.Rect.Bottom < Box.Rect.Bottom) Box.Rect.Bottom = TempBox.Rect.Bottom;

			// update top
			if(TempBox.Rect.Top > Box.Rect.Top) Box.Rect.Top = TempBox.Rect.Top;

			// update overall advanced width
			Box.Width += TempBox.Width;
			}

		// update right side
		if(Text.Length > 1)
			{
			// last character
			BoundingBox TempBox = GlyphArray[(Byte) Text[Text.Length - 1]];
			Box.Rect.Right = Box.Width - TempBox.Width + TempBox.Rect.Right;
			}

		// convert to user coordinate units
		Double Factor = FontSize / ScaleFactor;
		Box.Rect.Left = Factor * Box.Rect.Left;
		Box.Rect.Bottom = Factor * Box.Rect.Bottom;
		Box.Rect.Right = Factor * Box.Rect.Right;
		Box.Rect.Top = Factor * Box.Rect.Top;
		Box.Width = Factor * Box.Width;
		return(Box);
		}

	////////////////////////////////////////////////////////////////////
	// Text Kerning
	////////////////////////////////////////////////////////////////////

	public KerningAdjust[] TextKerning
			(
			String	Text
			)
		{
		// string is empty or one character
		if(String.IsNullOrEmpty(Text) || Text.Length == 1) return(null);

		// find first and last characters of the text
		Int32 First = Text[0];
		Int32 Last = Text[0];
		foreach(Char Chr in Text)
			{
			ValidateChar(Chr);
			if(Chr < First) First = Chr;
			else if(Chr > Last) Last = Chr;
			}

		// get kerning information
		WinKerningPair[] KP = FontInfo.GetKerningPairsApi(First, Last);

		// no kerning info available for this font or for this range
		if(KP == null) return(null);

		// prepare a list of kerning adjustments
		List<KerningAdjust> KA = new List<KerningAdjust>();

		// look for pairs with adjustments
		Int32 Ptr1 = 0;
		for(Int32 Ptr2 = 1; Ptr2 < Text.Length; Ptr2++)
			{
			// search for a pair of characters
			Int32 Index = Array.BinarySearch(KP, new WinKerningPair(Text[Ptr2 - 1], Text[Ptr2]));

			// not kerning information for this pair
			if(Index < 0) continue;

			// add kerning adjustment in PDF font units (windows design units divided by windows font design height)
			KA.Add(new KerningAdjust(Text.Substring(Ptr1, Ptr2 - Ptr1), KP[Index].KernAmount));

			// adjust pointer
			Ptr1 = Ptr2;
			}

		// list is empty
		if(KA.Count == 0) return(null);

		// add last
		KA.Add(new KerningAdjust(Text.Substring(Ptr1, Text.Length - Ptr1), 0));

		// exit
		return(KA.ToArray());
		}

	////////////////////////////////////////////////////////////////////
	// Text kerning width
	////////////////////////////////////////////////////////////////////

	public Double TextKerningWidth
			(
			Double			FontSize,		// in points
			KerningAdjust[]	KerningArray
			)
		{
		// text is null or empty
		if(KerningArray == null || KerningArray.Length == 0) return(0);

		// total width
		Double Width = 0;

		// draw text
		Int32 LastStr = KerningArray.Length - 1;
		for(Int32 Index = 0; Index < LastStr; Index++)
			{
			KerningAdjust KA = KerningArray[Index];
			Width += TextWidth(FontSize, KA.Text) + FontUnitsToUserUnits(FontSize, KA.Adjust);
			}

		// last string
		Width += TextWidth(FontSize, KerningArray[LastStr].Text);
		return(Width);
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

	internal override void WriteObjectToPdfFile
			(
			BinaryWriter PdfFile
			)
		{
		// look for first and last character
		Int32 FirstChar;
		Int32 LastChar;
		for(FirstChar = 0; FirstChar < 256 && !ActiveChar[FirstChar]; FirstChar++);
		if(FirstChar == 256) return;
		for(LastChar = 255; !ActiveChar[LastChar]; LastChar--);

		// pdf font name
		StringBuilder PdfFontName = new StringBuilder("/");

		// for embedded font add 6 alpha characters prefix
		if(EmbeddedFont)
			{
			PdfFontName.Append("PFWAAA+");
			Int32 Ptr1 = 6;
			for(Int32 Ptr2 = ResourceCode.Length - 1; Ptr2 >= 0 && Char.IsDigit(ResourceCode[Ptr2]); Ptr2--)
				{
				PdfFontName[Ptr1--] =  (Char) ((Int32) ResourceCode[Ptr2] + ('A' - '0'));
				}
			}

		// PDF readers are not happy with space in font name
		PdfFontName.Append(FontFamily.Name.Replace(" ", "#20"));

		// font name
		if((DesignFont.Style & FontStyle.Bold) != 0)
			{
			if((DesignFont.Style & FontStyle.Italic) != 0)
				PdfFontName.Append(",BoldItalic");
			else
				PdfFontName.Append(",Bold");
			}
		else if((DesignFont.Style & FontStyle.Italic) != 0)
			{
			PdfFontName.Append(",Italic");
			}

		// add items to dictionary
		AddToDictionary("/Subtype", "/TrueType");
		AddToDictionary("/BaseFont", PdfFontName.ToString());

		// add first and last characters
		AddToDictionary("/FirstChar", FirstChar.ToString());
		AddToDictionary("/LastChar", LastChar.ToString());

		// create font descriptor
		FontDescriptor = new PdfObject(Document, false, "/FontDescriptor");

		// add link to font object
		AddToDictionary("/FontDescriptor", FontDescriptor);

		// font descriptor dictionary
		FontDescriptor.AddToDictionary("/FontName", PdfFontName.ToString());	// must be the same as BaseFont above
		FontDescriptor.AddToDictionary("/Flags", ((Int32) FontFlags).ToString());
		FontDescriptor.AddToDictionary("/ItalicAngle", String.Format(NFI.DecSep, "{0}", (Single) PdfItalicAngle));
		FontDescriptor.AddToDictionary("/FontWeight", PdfFontWeight.ToString());
		FontDescriptor.AddToDictionary("/Leading", String.Format(NFI.DecSep, "{0}", (Single) FontUnitsToPdfDic(PdfLeading)));
		FontDescriptor.AddToDictionary("/Ascent", String.Format(NFI.DecSep, "{0}", (Single) FontUnitsToPdfDic(PdfAscent)));
		FontDescriptor.AddToDictionary("/Descent", String.Format(NFI.DecSep, "{0}", (Single) FontUnitsToPdfDic(-PdfDescent)));

		// alphabetic (non symbolic) fonts
		if((FontFlags & PdfFontFlags.Symbolic) == 0)
			{
			AddToDictionary("/Encoding", "/WinAnsiEncoding");
			BoundingBox Box = FontInfo.GetGlyphMetricsApi('x');
			FontDescriptor.AddToDictionary("/XHeight", String.Format(NFI.DecSep, "{0}", (Single) FontUnitsToPdfDic(Box.Rect.Top)));
			FontDescriptor.AddToDictionary("/AvgWidth", String.Format(NFI.DecSep, "{0}", (Single) FontUnitsToPdfDic(Box.Width)));
			Box = FontInfo.GetGlyphMetricsApi('M');
			PdfCapHeight = Box.Rect.Top;
			FontDescriptor.AddToDictionary("/CapHeight", String.Format(NFI.DecSep, "{0}", (Single) FontUnitsToPdfDic(Box.Rect.Top)));
			FontDescriptor.AddToDictionary("/StemV", String.Format(NFI.DecSep, "{0}", (Single) StemV()));
			}

		// create width object array
		FontWidthArray = new PdfObject(Document, false);

		// add link to font object
		AddToDictionary("/Widths", FontWidthArray);

		// build bounding box and width array
		Double Left = Double.MaxValue;
		Double Bottom = Double.MaxValue;
		Double Right = Double.MinValue;
		Double Top = Double.MinValue;
		Double MaxWidth = Double.MinValue;
		FontWidthArray.ContentsString = new StringBuilder("[");

		Int32 EolLength = 100;
		for(Int32 Index = FirstChar; Index <= LastChar; Index++)
			{
			Double CharWidth;

			// not used
			if(!ActiveChar[Index])
				{
				CharWidth = 0;
				}

			// used
			else
				{
				// bounding box
				BoundingBox GM = GlyphArray[Index];
				if(GM.Rect.Left < Left) Left = GM.Rect.Left;
				if(GM.Rect.Bottom < Bottom) Bottom = GM.Rect.Bottom;
				if(GM.Rect.Right > Right) Right = GM.Rect.Right;
				if(GM.Rect.Top > Top) Top = GM.Rect.Top;

				// character advance width
				CharWidth = GM.Width;

				// max width
				if(CharWidth > MaxWidth) MaxWidth = CharWidth;
				}

			// add width to width array
			if(FontWidthArray.ContentsString.Length > EolLength)
				{
				FontWidthArray.ContentsString.Append('\n');
				EolLength = FontWidthArray.ContentsString.Length + 100;
				}

			// add width to width array
			FontWidthArray.ContentsString.AppendFormat(NFI.DecSep, "{0} ", FontUnitsToPdfDic(CharWidth));
			}

		// add to font descriptor array
		FontDescriptor.AddToDictionary("/MaxWidth", String.Format(NFI.DecSep, "{0}", (Single) FontUnitsToPdfDic(MaxWidth)));
		FontDescriptor.AddToDictionary("/FontBBox", String.Format(NFI.DecSep, "[{0} {1} {2} {3}]",
			FontUnitsToPdfDic(Left), FontUnitsToPdfDic(Bottom), FontUnitsToPdfDic(Right), FontUnitsToPdfDic(Top)));

		// terminate width array
		FontWidthArray.ContentsString.Length--;
		FontWidthArray.ContentsString.Append("]");

		// create font file
		if(EmbeddedFont)
			{
			// create font file stream
			PdfFontFile EmbeddedFontObj = new PdfFontFile(this);

			// add link to font object
			FontDescriptor.AddToDictionary("/FontFile2", EmbeddedFontObj);
			}

		// call base write PdfObject to file method
		base.WriteObjectToPdfFile(PdfFile);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Calculate StemV from capital I
	////////////////////////////////////////////////////////////////////

	private Double StemV()
		{
		// convert I to graphics path
		GraphicsPath GP = new GraphicsPath();
		GP.AddString("I", FontFamily, (Int32) DesignFont.Style, 1000, Point.Empty, StringFormat.GenericDefault);

		// center point of I
		RectangleF Rect = GP.GetBounds();
		Int32 X = (Int32) ((Rect.Left + Rect.Right) / 2);
		Int32 Y = (Int32) ((Rect.Bottom + Rect.Top) / 2);

		// bounding box converted to integer
		Int32 LeftLimit = (Int32) Rect.Left;
		Int32 RightLimit = (Int32) Rect.Right;

		// make sure we are within the I
		if(!GP.IsVisible(X, Y)) return((Double) (RightLimit - LeftLimit));

		// look for left edge
		Int32 Left;
		for(Left = X - 1; Left >= LeftLimit && GP.IsVisible(Left, Y); Left--);

		// look for right edge
		Int32 Right;
		for(Right = X + 1; Right < RightLimit && GP.IsVisible(Right, Y); Right++);

		// exit
		return((Double) (Right - Left - 1));
		}
	}
}
