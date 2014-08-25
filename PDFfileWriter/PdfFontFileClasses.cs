/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfFontFileClasses
//	Support classes for the PdfFontFile classs.
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
using System.Text;

namespace PdfFileWriter
{
/////////////////////////////////////////////////////////////////////
// Font file header
/////////////////////////////////////////////////////////////////////

public class FontFileHeader
	{
	public UInt32	FileVersion;		// 0x00010000 for version 1.0.
	public UInt16	NumTables;			// Number of tables.

	// 16 * (maximum power of 2 <= numTables)
	public UInt16	SearchRange
		{
		get
			{
			Int32 Mask;
			for(Mask = 1; Mask <= NumTables; Mask <<= 1);
			return((UInt16) (Mask << 3));
			}
		}

	// Log2(maximum power of 2 <= numTables).
	public UInt16	EntrySelector
		{
		get
			{
			Int32 Power;
			for(Power = 1; (1 << Power) <= NumTables; Power++);
			return((UInt16) (Power - 1));
			}
		}

	// NumTables x 16-searchRange.
	public UInt16	RangeShift
		{
		get
			{
			return((UInt16) (16 * NumTables - SearchRange));
			}
		}
	}

/////////////////////////////////////////////////////////////////////
// Font file table record
/////////////////////////////////////////////////////////////////////

public class TableRecord
	{
	public UInt32	Tag;				// 4 -byte identifier
	public UInt32	Checksum;			// Checksum for this table
	public Int32	Offset;				// Offset from beginning of TrueType font file
	public Int32	Length;				// Length of this table
	public Byte[]	Data;				// table data in big endian format

	// constructor
	public TableRecord
				(
				UInt32 Tag
				)
			{
			this.Tag = Tag;
			return;
			}
	}

/////////////////////////////////////////////////////////////////////
// 'cmap' encoding sub-table
/////////////////////////////////////////////////////////////////////

public class cmapSubTbl : IComparable<cmapSubTbl>
	{
	public UInt16		PlatformID;				// Platform ID. Should be 3 for windows
	public UInt16		EncodingID;				// Platform-specific encoding ID. Should be 1 for Unicode and 0 for symbol
	public UInt16		Format;					// Format number (the program supports format 0 or 4)
	public UInt32		Offset;					// Byte offset from beginning of table to the sub-table for this encoding
	public UInt16		Length;					// This is the length in bytes of the sub-table.
	public UInt16		Language;				// this field is relevant to Macintosh (platform ID 1)
	public UInt16		SegCount;				// (Format 4) SegCount.
	public cmapSeg[]	SegArray;				// (Format 4) segment array
	public UInt16[]		GlyphArray;				// glyph array translate character for format 0 or index for format 4 to glyph code

	// default constructor
	public cmapSubTbl() {}

	// search constructor
	public cmapSubTbl
			(
			UInt16		PlatformID,
			UInt16		EncodingID,
			UInt16		Format
			)
		{
		this.PlatformID = PlatformID;
		this.EncodingID = EncodingID;
		this.Format = Format;
		return;
		}

	// compare two sub-tables for sort and binary search
	public Int32 CompareTo
			(
			cmapSubTbl	Other
			)
		{
		if(this.PlatformID != Other.PlatformID) return(this.PlatformID - Other.PlatformID);
		if(this.EncodingID != Other.EncodingID) return(this.EncodingID - Other.EncodingID);
		return(this.Format - Other.Format);
		}

	// 2 x segCount
	public UInt16 SegCountX2
		{
		get
			{
			return((UInt16) (2 * SegCount));
			}
		}

	// 2 * (maximum power of 2 <= numTables)
	public UInt16 SearchRange
		{
		get
			{
			Int32 Mask;
			for(Mask = 1; Mask <= SegCount; Mask <<= 1);
			return((UInt16) Mask);
			}
		}

	// Log2(maximum power of 2 <= numTables).
	public UInt16 EntrySelector
		{
		get
			{
			Int32 Power;
			for(Power = 1; (1 << Power) <= SegCount; Power++);
			return((UInt16) (Power - 1));
			}
		}

	// NumTables x 16-searchRange.
	public UInt16 RangeShift
		{
		get
			{
			return((UInt16) (2 * SegCount - SearchRange));
			}
		}

	// convert character code to glyph code
	public UInt16 CharToGlyph
			(
			Int32 CharCode
			)
		{
		// format 0
		if(Format == 0) return(CharCode < 256 ? GlyphArray[CharCode] : (UInt16) 0);

		// not format 4
		if(Format != 4) return(0);

		// search segment array for segment that CharCode >= Start && CharCode <= End
		Int32 Index = Array.BinarySearch(SegArray, new cmapSeg(CharCode));
		if(Index < 0) Index = ~Index;
		cmapSeg Seg = SegArray[Index];

		// non standard fonts
		// special case for symbol type fonts
		// Microsoft adds 0xf000 to character code
		if(Seg.StartChar >= 0xf000) CharCode += (Char) 0xf000;

		// not found
		if(CharCode < Seg.StartChar) return(0);

		// IDRangeOffset is zero. Glyph code is character code plus delta
		if(Seg.IDRangeOffset == 0) return((UInt16) (Seg.IDDelta + CharCode));

		// IDRangeOffset is not zero. Glyph code is taken out of GlyphArray 
		return(GlyphArray[Seg.IDRangeOffset + CharCode - Seg.StartChar - (SegCount - Index)]);
		}

	}

/////////////////////////////////////////////////////////////////////
// 'cmap' format 4 encoding sub-table segment record
/////////////////////////////////////////////////////////////////////

public class cmapSeg : IComparable<cmapSeg>
	{
	public UInt16	StartChar;				// Start character code for each segment. Array length=segCount
	public UInt16	EndChar;				// End characterCode for each segment, last=0xFFFF. Array length=segCount
	public Int16	IDDelta;				// Delta for all character codes in segment. Array length=segCount
	public UInt16	IDRangeOffset;			// Offsets (in byte) into glyphIdArray or 0. Array length=segCount

	// search constructor
	public cmapSeg
			(
			Int32	StartChar,
			Int32	EndChar,
			Int32	IDDelta,
			Int32	IDRangeOffset
			)
		{
		this.StartChar = (UInt16) StartChar;
		this.EndChar = (UInt16) EndChar;
		this.IDDelta = (Int16) IDDelta;
		this.IDRangeOffset = (UInt16) IDRangeOffset;
		return;
		}

	// search constructor
	public cmapSeg
			(
			Int32	EndCount
			)
		{
		this.EndChar = (UInt16) EndCount;
		return;
		}

	// compare two records for sort and binary search
	public Int32 CompareTo
			(
			cmapSeg Other
			)
		{
		return(this.EndChar - Other.EndChar);
		}
	}

/////////////////////////////////////////////////////////////////////
// 'head' font file header table
/////////////////////////////////////////////////////////////////////

public class headTable
	{
	public UInt32	TableVersion;			// 0x00010000 for version 1.0.
	public UInt32	FontRevision;			// Set by font manufacturer.
	public UInt32	ChecksumAdjustment;		// font file overall checksum. To compute: set it to 0, sum the entire font, then store 0xB1B0AFBA - sum.
	public UInt32	MagicNumber;			// Set to 0x5F0F3CF5.
	public UInt16	Flags;					// Bit 0: Baseline for font at y=0;
											// Bit 1: Left sidebearing point at x=0;
											// Bit 2: Instructions may depend on point size; 
											// Bit 3: Force ppem to integer values for all internal scaler math; may use fractional
											//        ppem sizes if this bit is clear; 
											// Bit 4: Instructions may alter advance width (the advance widths might not scale linearly); 
											// Bits 5-10: These should be set according to Apple's specification.
											//        However, they are not implemented in OpenType. 
											// Bit 11: Font data is 'lossless,' as a result of having been compressed and decompressed
											//         with the Agfa MicroType Express engine.
											// Bit 12: Font converted (produce compatible metrics)
											// Bit 13: Font optimized for ClearType™. Note, fonts that rely on embedded bitmaps (EBDT)
											//         for rendering should not be considered optimized for ClearType,
											//		   and therefore should keep this bit cleared.
											// Bit 14: Reserved, set to 0
											// Bit 15: Reserved, set to 0 
	public UInt16	UnitsPerEm;				// Valid range is from 16 to 16384. This value should be a power of 2 for fonts that have TrueType outlines.
	public Int64	TimeCreated;			// Number of seconds since 12:00 midnight, January 1, 1904. 64-bit integer
	public Int64	TimeModified;			// Number of seconds since 12:00 midnight, January 1, 1904. 64-bit integer
	public Int16	xMin;					// For all glyph bounding boxes.
	public Int16	yMin;					// For all glyph bounding boxes.
	public Int16	xMax;					// For all glyph bounding boxes.
	public Int16	yMax;					// For all glyph bounding boxes.
	public UInt16	MacStyle;				// Bit 0: Bold (if set to 1); 
											// Bit 1: Italic (if set to 1) 
											// Bit 2: Underline (if set to 1) 
											// Bit 3: Outline (if set to 1) 
											// Bit 4: Shadow (if set to 1) 
											// Bit 5: Condensed (if set to 1) 
											// Bit 6: Extended (if set to 1) 
											// Bits 7-15: Reserved (set to 0).
	public UInt16	LowestRecPPEM;			// Smallest readable size in pixels.
	public Int16	FontDirectionHint;		// Deprecated (Set to 2). 
											// 0: Fully mixed directional glyphs; 
											// 1: Only strongly left to right; 
											// 2: Like 1 but also contains neutrals; 
											// -1: Only strongly right to left; 
											// -2: Like -1 but also contains neutrals. 1
	public Int16	IndexToLocFormat;		// 0 for short offsets, 1 for long.
	public Int16	glyphDataFormat;		// 0 for current format.
	}

/////////////////////////////////////////////////////////////////////
// 'head' horizontal header table
/////////////////////////////////////////////////////////////////////

public class hheaTable
	{
	public UInt32	TableVersion;			// 0x00010000 for version 1.0.
	public Int16	Ascender;				// Typographic ascent. (Distance from baseline of highest ascender)
	public Int16	Descender;				// Typographic descent. (Distance from baseline of lowest descender)
	public Int16	LineGap;				// Typographic line gap. Negative LineGap values are treated as zero
											// in Windows 3.1, System 6, and System 7.
	public UInt16	advanceWidthMax;		// Maximum advance width value in 'hmtx' table.
	public Int16	minLeftSideBearing;		// Minimum left sidebearing value in 'hmtx' table.
	public Int16	minRightSideBearing;	// Minimum right sidebearing value; calculated as Min(aw - lsb - (xMax - xMin)).
	public Int16	xMaxExtent;				// Max(lsb + (xMax - xMin)).
	public Int16	caretSlopeRise;			// Used to calculate the slope of the cursor (rise/run); 1 for vertical.
	public Int16	caretSlopeRun;			// 0 for vertical.
	public Int16	caretOffset;			// The amount by which a slanted highlight on a glyph needs to be shifted
											// to produce the best appearance. Set to 0 for non-slanted fonts
	public Int16	Reserved1;				// set to 0
	public Int16	Reserved2;				// set to 0
	public Int16	Reserved3;				// set to 0
	public Int16	Reserved4;				// set to 0
	public Int16	metricDataFormat;		// 0 for current format.
	public UInt16	numberOfHMetrics;		// Number of hMetric entries in 'hmtx' table
	}

/////////////////////////////////////////////////////////////////////
// 'maxp' font maximum values
/////////////////////////////////////////////////////////////////////

public class maxpTable
	{
	public UInt32	TableVersion;			// 0x00010000 for version 1.0.
	public UInt16	numGlyphs;				// The number of glyphs in the font.
	public UInt16	maxPoints;				// Maximum points in a non-composite glyph.
	public UInt16	maxContours;			// Maximum contours in a non-composite glyph.
	public UInt16	maxCompositePoints;		// Maximum points in a composite glyph.
	public UInt16	maxCompositeContours;	// Maximum contours in a composite glyph.
	public UInt16	maxZones;				// 1 if instructions do not use the twilight zone (Z0), or
											// 2 if instructions do use Z0; should be set to 2 in most cases.
	public UInt16	maxTwilightPoints;		// Maximum points used in Z0.
	public UInt16	maxStorage;				// Number of Storage Area locations.
	public UInt16	maxFunctionDefs;		// Number of FDEFs.
	public UInt16	maxInstructionDefs;		// Number of IDEFs.
	public UInt16	maxStackElements;		// Maximum stack depth2.
	public UInt16	maxSizeOfInstructions;	// Maximum byte count for glyph instructions.
	public UInt16	maxComponentElements;	// Maximum number of components referenced at “top level” for any composite glyph.
	public UInt16	maxComponentDepth;		// Maximum levels of recursion; 1 for simple components.
	}

/////////////////////////////////////////////////////////////////////
// Glyph table support
/////////////////////////////////////////////////////////////////////

// glyph flags for comosite glyphs
public enum CompFlag
	{
	Arg1AndArg2AreWords = 1,			// bit0	If this is set, the arguments are words; otherwise, they are bytes.
	ArgsAreXYValues = 2,				// bit1	If this is set, the arguments are xy values; otherwise, they are points.
	RoundXYToGrid = 4,					// bit2	For the xy values if the preceding is true.
	WeHaveAScale = 8,					// bit3	This indicates that there is a simple scale for the component. Otherwise, scale = 1.0.
	Reserve = 0x10,						// bit4	This bit is reserved. Set it to 0.
	MoreComponents = 0x20,				// bit5	Indicates at least one more glyph after this one.
	WeHaveXYScale = 0x40,				// bit6	The x direction will use a different scale from the y direction.
	WeHave2By2 = 0x80,					// bit7	There is a 2 by 2 transformation that will be used to scale the component.
	WeHaveInstructions = 0x100,			// bit8	Following the last component are instructions for the composite character.
	UseMyMetrics = 0x200,				// bit9	If set, this forces the aw and lsb (and rsb) for the composite to be equal
										// to those from this original glyph. This works for hinted and unhinted characters.
	OverlapCompound = 0x400,			// bit10 Used by Apple in GX fonts.
	ScaledComponentOffset = 0x800,		// bit11 Composite designed to have the component offset scaled (designed for Apple rasterizer).
	UnscaledComponentOffset = 0x1000,	// bit12 Composite designed not to have the component offset scaled (designed for the Microsoft TrueType rasterizer).
	}

/////////////////////////////////////////////////////////////////////
// Glyph base class
/////////////////////////////////////////////////////////////////////

public class Glyph : IComparable<Glyph>
	{
	public Int32		GlyphCode;
	public Byte[]		GlyphData;
	public UInt16		AdvanceWidth;
	public Int16		LeftSideBearing;
	public Boolean		Composite;

	// search constructor
	public Glyph
			(
			Int32	GlyphCode
			)
		{
		this.GlyphCode = GlyphCode;
		return;
		}

	// compare two glyphs for sort and binary search
	public Int32 CompareTo
			(
			Glyph Other
			)
		{
		return(this.GlyphCode - Other.GlyphCode);
		}
	}
}
