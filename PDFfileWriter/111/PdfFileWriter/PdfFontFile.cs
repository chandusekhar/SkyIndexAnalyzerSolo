/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfFontFile
//	Support Class to embed font with the PDF File.
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
using System.IO;
using System.Text;

namespace PdfFileWriter
{
public class PdfFontFile : PdfObject
	{
	private FontApi			FontInfo;
	private Int32			FirstChar;
	private Int32			LastChar;
	private Boolean[]		ActiveChar;		// 0 to 255
	private Int32[]			CharSubArray;
	private Boolean			SymbolicFont;

	private FontFileHeader	FileHeader;
	private cmapSubTbl		cmapSubTbl;
	private headTable		headTable;
	private hheaTable		hheaTable;
	private UInt16[]		hmtxTable;
	private Int32[]			locaTable;
	private maxpTable		maxpTable;
	private UInt16[]		CharToGlyphArray;
	private Glyph[]			GlyphArray;
	private Int32			OldGlyphTableOffset;

	private Byte[]			Buffer;
	private Int32			BufPtr;

	// table tags
	private const UInt32 cmapTag = 0x636d6170;	// "cmap"
	private const UInt32 cvtTag =  0x63767420;	// "cvt"
	private const UInt32 fpgmTag = 0x6670676d;	// "fpgm"
	private const UInt32 glyfTag = 0x676c7966;	// "glyf"
	private const UInt32 headTag = 0x68656164;	// "head"
	private const UInt32 hheaTag = 0x68686561;	// "hhea"
	private const UInt32 hmtxTag = 0x686d7478;	// "hmtx"
	private const UInt32 locaTag = 0x6c6f6361;	// "loca"
	private const UInt32 maxpTag = 0x6d617870;	// "maxp"
	private const UInt32 prepTag = 0x70726570;	// "prep"

	// this array must be in sorted order
	private TableRecord[] TableRecordArray = new TableRecord[]
		{
		new TableRecord(cmapTag),
		new TableRecord(cvtTag),
		new TableRecord(fpgmTag),
		new TableRecord(glyfTag),
		new TableRecord(headTag),
		new TableRecord(hheaTag),
		new TableRecord(hmtxTag),
		new TableRecord(locaTag),
		new TableRecord(maxpTag),
		new TableRecord(prepTag)
		};

	private enum Tag
		{
		cmap,
		cvt,
		fpgm,
		glyf,
		head,
		hhea,
		hmtx,
		loca,
		maxp,
		prep
		}
	
	public PdfFontFile
			(
			PdfFont PdfFont
			) : base(PdfFont.Document, true)
		{
		this.FontInfo = PdfFont.FontInfo;
		ActiveChar = PdfFont.ActiveChar;
		CharSubArray = PdfFont.CharSubArray;
		SymbolicFont = PdfFont.SymbolicFont;

		// look for first and last character
		for(FirstChar = 0; FirstChar < 256 && !ActiveChar[FirstChar]; FirstChar++);
		for(LastChar = 255; !ActiveChar[LastChar]; LastChar--);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

	internal override void WriteObjectToPdfFile
			(
			BinaryWriter PdfFile
			)
		{
		// font file
		Byte[] FontData = CreateFontFile();

		// add font file length (uncompressed)
		AddToDictionary("/Length1", FontData.Length.ToString());

		// compress font file
		DeflateZLib Deflate = new DeflateZLib();
		Byte[] FontDataComp = Deflate.Compress(FontData);
		if(FontDataComp != null)
			{
			FontData = FontDataComp;
			AddToDictionary("/Filter", "/FlateDecode");
			}

		// add compressed file length
		AddToDictionary("/Length", FontData.Length.ToString());

		// write object header
		PdfFile.Write(Encoding.ASCII.GetBytes(String.Format("{0} 0 obj\n", ObjectNumber)));

		// write dictionary
		DictionaryToPdfFile(PdfFile);

		// output stream
		PdfFile.Write(Encoding.ASCII.GetBytes("stream\n"));

        // debug
		if(Document.Debug)
			{
			PdfFile.Write(Encoding.ASCII.GetBytes("*** FONTFILE PLACE HOLDER ***"));
			}

		// output embedded font
		else
			{
			PdfFile.Write(FontData);
			}

		// output stream
		PdfFile.Write(Encoding.ASCII.GetBytes("\nendstream\n"));

		// output object trailer
		PdfFile.Write(Encoding.ASCII.GetBytes("endobj\n"));
		return;
		}


	////////////////////////////////////////////////////////////////////
	// Create font file
	////////////////////////////////////////////////////////////////////

	public Byte[] CreateFontFile()
		{
		// get file signature
		GetFontFileHeaderApi();

		// get head table
		GetheadTable();

		// get horizontal head table
		GethheaTable();

		// get maximum profile table
		GetmaxpTable();

		// get character code to glyph code table
		GetcmapTable();

		// get horizontal metrics table
		GethmtxTable();

		// get glyph code to glyph data location in the table
		GetlocaTable();

		// get glyph data
		BuildGlyphArray();

		// replace old glyph codes with new ones for composite glyphs
		ReplaceGlyphCode();

		// calculate glyph table checksum
		CalculateGlyphChksum();

		// build new glyph location table
		BuildLocaTable();

		// build new character map table
		BuildCharMapTable();

		// build new horizontal metrics table
		BuildhmtxTable();

		// build new head table
		BuildHeadTable();

		// build new hhea table
		BuildHheaTable();

		// build new maxp table
		BuildMaxpTable();

		// load ctv, fpgm and prep tables
		BuildFontProgramTables();

		// build font file
		BuildEmbeddedFile();

		// exit
		return(Buffer);
		}

	////////////////////////////////////////////////////////////////////
	// Get Font Data File header and table records
	////////////////////////////////////////////////////////////////////

	private void GetFontFileHeaderApi()
		{
		// read font file header
		Buffer = FontInfo.GetFontDataApi(0, 12);
		BufPtr = 0;
		FileHeader = new FontFileHeader();
		FileHeader.FileVersion = ReadUInt32BigEndian();
		FileHeader.NumTables = ReadUInt16BigEndian();

		// number of bytes to retrieve
		Int32 BufSize = 16 * FileHeader.NumTables;

		// read all table records from input file
		Buffer = FontInfo.GetFontDataApi(12, BufSize);
		BufPtr = 0;

		// load table records
		Int32 Index = 0;
		for(Int32 Table = 0; Table < FileHeader.NumTables; Table++)
			{
			// get one table record
			UInt32 TableTag = ReadUInt32BigEndian();

			// we do not need this table
			if(TableTag != TableRecordArray[Index].Tag)
				{
				// skip 12 bytes
				BufPtr += 12;
				continue;
				}

			// read info for this table
			TableRecord TR = TableRecordArray[Index++];
			TR.Checksum = ReadUInt32BigEndian();
			TR.Offset = (Int32) ReadUInt32BigEndian();
			TR.Length = (Int32) ReadUInt32BigEndian();
			}

		// make sure all required tables are available
		if(Index != TableRecordArray.Length) throw new ApplicationException("Required font file table is missing");

		// load all tables except for glyf table
		foreach(TableRecord TR in TableRecordArray)
			{
			// ignore glyf table for now
			// save file offset of glyph table
			if(TR.Tag == glyfTag) OldGlyphTableOffset = TR.Offset;

			// load all other tables
			else TR.Data = FontInfo.GetFontDataApi(TR.Offset, TR.Length);
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Read "head" table
	////////////////////////////////////////////////////////////////////

	private void GetheadTable()
		{
		// set buffer for decoding
		Buffer = TableRecordArray[(Int32) Tag.head].Data;
		BufPtr = 0;

		// decode head table
		headTable = new headTable();
		headTable.TableVersion = ReadUInt32BigEndian();
		headTable.FontRevision = ReadUInt32BigEndian();
		headTable.ChecksumAdjustment = ReadUInt32BigEndian();
		headTable.MagicNumber = ReadUInt32BigEndian();
		headTable.Flags = ReadUInt16BigEndian();
		headTable.UnitsPerEm = ReadUInt16BigEndian();
		headTable.TimeCreated = ReadInt64BigEndian();
		headTable.TimeModified = ReadInt64BigEndian();
		headTable.xMin = ReadInt16BigEndian();
		headTable.yMin = ReadInt16BigEndian();
		headTable.xMax = ReadInt16BigEndian();
		headTable.yMax = ReadInt16BigEndian();
		headTable.MacStyle = ReadUInt16BigEndian();
		headTable.LowestRecPPEM = ReadUInt16BigEndian();
		headTable.FontDirectionHint = ReadInt16BigEndian();
		headTable.IndexToLocFormat = ReadInt16BigEndian();
		headTable.glyphDataFormat = ReadInt16BigEndian();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Read "hhea" table
	////////////////////////////////////////////////////////////////////

	private void GethheaTable()
		{
		// set buffer for decoding
		Buffer = TableRecordArray[(Int32) Tag.hhea].Data;
		BufPtr = 0;

		// decode head table
		hheaTable = new hheaTable();
		hheaTable.TableVersion = ReadUInt32BigEndian();
		hheaTable.Ascender = ReadInt16BigEndian();
		hheaTable.Descender = ReadInt16BigEndian();
		hheaTable.LineGap = ReadInt16BigEndian();
		hheaTable.advanceWidthMax = ReadUInt16BigEndian();
		hheaTable.minLeftSideBearing = ReadInt16BigEndian();
		hheaTable.minRightSideBearing = ReadInt16BigEndian();
		hheaTable.xMaxExtent = ReadInt16BigEndian();
		hheaTable.caretSlopeRise = ReadInt16BigEndian();
		hheaTable.caretSlopeRun = ReadInt16BigEndian();
		hheaTable.caretOffset = ReadInt16BigEndian();
		hheaTable.Reserved1 = ReadInt16BigEndian();
		hheaTable.Reserved2 = ReadInt16BigEndian();
		hheaTable.Reserved3 = ReadInt16BigEndian();
		hheaTable.Reserved4 = ReadInt16BigEndian();
		hheaTable.metricDataFormat = ReadInt16BigEndian();
		hheaTable.numberOfHMetrics = ReadUInt16BigEndian();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Read "maxp" table
	////////////////////////////////////////////////////////////////////

	private void GetmaxpTable()
		{
		// set buffer for decoding
		Buffer = TableRecordArray[(Int32) Tag.maxp].Data;
		BufPtr = 0;

		// decode maxp table
		maxpTable = new maxpTable();
		maxpTable.TableVersion = ReadUInt32BigEndian();
		maxpTable.numGlyphs = ReadUInt16BigEndian();
		maxpTable.maxPoints = ReadUInt16BigEndian();
		maxpTable.maxContours = ReadUInt16BigEndian();
		maxpTable.maxCompositePoints = ReadUInt16BigEndian();
		maxpTable.maxCompositeContours = ReadUInt16BigEndian();
		maxpTable.maxZones = ReadUInt16BigEndian();
		maxpTable.maxTwilightPoints = ReadUInt16BigEndian();
		maxpTable.maxStorage = ReadUInt16BigEndian();
		maxpTable.maxFunctionDefs = ReadUInt16BigEndian();
		maxpTable.maxInstructionDefs = ReadUInt16BigEndian();
		maxpTable.maxStackElements = ReadUInt16BigEndian();
		maxpTable.maxSizeOfInstructions = ReadUInt16BigEndian();
		maxpTable.maxComponentElements = ReadUInt16BigEndian();
		maxpTable.maxComponentDepth = ReadUInt16BigEndian();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Read "cmap" table
	////////////////////////////////////////////////////////////////////

	private void GetcmapTable()
		{
		// set buffer for decoding
		Buffer = TableRecordArray[(Int32) Tag.cmap].Data;
		BufPtr = 0;

		// create cmap object
		if(ReadUInt16BigEndian() != 0) throw new ApplicationException("CMAP table version number is not zero");
		Int32 NumberOfTables = ReadUInt16BigEndian();
		cmapSubTbl[] SubTblArray = new cmapSubTbl[NumberOfTables]; 

		// loop for tables
		for(Int32 Index = 0; Index < NumberOfTables; Index++)
			{
			cmapSubTbl SubTbl = new cmapSubTbl();
			SubTblArray[Index] = SubTbl;
			SubTbl.PlatformID = ReadUInt16BigEndian();
			SubTbl.EncodingID = ReadUInt16BigEndian();
			SubTbl.Offset = ReadUInt32BigEndian();

			// save buffer pointer
			Int32 SaveBufPtr = BufPtr;

			// set offset
			BufPtr = (Int32) SubTbl.Offset;

			// read format code
			SubTbl.Format = ReadUInt16BigEndian();

			// process format 0
			if(SubTbl.Format == 0)
				{
				SubTbl.Length = ReadUInt16BigEndian();
				SubTbl.Language = ReadUInt16BigEndian();
				SubTbl.GlyphArray = new UInt16[256];
				for(Int32 Code = 0; Code < 256; Code++) SubTbl.GlyphArray[Code] = Buffer[BufPtr++];
				}

			// process format 4
			else if(SubTbl.Format == 4)
				{
				SubTbl.Length = ReadUInt16BigEndian();
				SubTbl.Language = ReadUInt16BigEndian();
				SubTbl.SegCount = (UInt16) (ReadUInt16BigEndian() / 2);
				BufPtr += 6;	// skip search range, entry selector and range shift
				SubTbl.SegArray = new cmapSeg[SubTbl.SegCount];
				for(Int32 Seg = 0; Seg < SubTbl.SegCount; Seg++) SubTbl.SegArray[Seg] = new cmapSeg(ReadUInt16BigEndian()); // EndChar
				ReadUInt16BigEndian(); // skip reserved padding
				for(Int32 Seg = 0; Seg < SubTbl.SegCount; Seg++) SubTbl.SegArray[Seg].StartChar = ReadUInt16BigEndian();
				for(Int32 Seg = 0; Seg < SubTbl.SegCount; Seg++) SubTbl.SegArray[Seg].IDDelta = ReadInt16BigEndian();
				for(Int32 Seg = 0; Seg < SubTbl.SegCount; Seg++) SubTbl.SegArray[Seg].IDRangeOffset = (UInt16) (ReadUInt16BigEndian() / 2);
				Int32 GlyphCount = (SubTbl.Length - 16 - 8 * SubTbl.SegCount) / 2;
				SubTbl.GlyphArray = new UInt16[GlyphCount];
				for(Int32 Glyph = 0; Glyph < GlyphCount; Glyph++) SubTbl.GlyphArray[Glyph] = ReadUInt16BigEndian();
				}

			// restore buffer pointer
			BufPtr = SaveBufPtr;
			}

		// sort table
		Array.Sort(SubTblArray);

		// select 'best' sub-table for character code to glyph code translation
		cmapSubTbl = SelectcmapSubTable(SubTblArray);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Select best sub-table in "cmap" table
	////////////////////////////////////////////////////////////////////

	private cmapSubTbl SelectcmapSubTable
			(
			cmapSubTbl[]	SubTblArray
			)
		{
		// search for platform ID = 3 Windows, encoding ID = 0 or 1 Unicode and format 4
		cmapSubTbl SearchSubTbl = new cmapSubTbl(3, (UInt16) (SymbolicFont ? 0 : 1), 4);
		Int32 Index = Array.BinarySearch(SubTblArray, SearchSubTbl);
		if(Index >= 0) return(SubTblArray[Index]);

		// search for platform ID = 3 Windows, encoding ID = 0 or 1 Unicode and format 0
		SearchSubTbl.Format = 0;
		Index = Array.BinarySearch(SubTblArray, SearchSubTbl);
		if(Index >= 0) return(SubTblArray[Index]);

		// not found
		throw new ApplicationException("Required cmap sub-table is missing");
		}

	////////////////////////////////////////////////////////////////////
	// Read "hmtx" table
	////////////////////////////////////////////////////////////////////

	private void GethmtxTable()
		{
		// set buffer for decoding
		Buffer = TableRecordArray[(Int32) Tag.hmtx].Data;
		BufPtr = 0;

		// create table for advance width 
		hmtxTable = new UInt16[hheaTable.numberOfHMetrics];

		// read long horizontal metric array
		// the program ignores the left side bearing values
		// in the new table the left side bearing will be taken from xMin
		Int32 Index;
		for(Index = 0; Index < hheaTable.numberOfHMetrics; Index++)
			{
			hmtxTable[Index] = ReadUInt16BigEndian();
			BufPtr += 2;
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Read "loca" table
	////////////////////////////////////////////////////////////////////

	private void GetlocaTable()
		{
		// set buffer for decoding
		Buffer = TableRecordArray[(Int32) Tag.loca].Data;
		BufPtr = 0;

		// calculate size based on table length
		Int32 TblSize = headTable.IndexToLocFormat == 0 ? Buffer.Length / 2 : Buffer.Length / 4;

		// allocate array
		locaTable = new Int32[TblSize];

		// load short table
		if(headTable.IndexToLocFormat == 0)
			{
			for(Int32 Index = 0; Index < TblSize; Index++) locaTable[Index] = 2 * ReadUInt16BigEndian();
			}

		// long format
		else
			{
			for(Int32 Index = 0; Index < TblSize; Index++) locaTable[Index] = (Int32) ReadUInt32BigEndian();
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Build glyph array for character range
	////////////////////////////////////////////////////////////////////

	private void BuildGlyphArray()
		{
		// create character code to glyph code array
		CharToGlyphArray = new UInt16[LastChar - FirstChar + 1];

		// create a glyph data list
		List<Glyph> GlyphList = new List<Glyph>();

		// create a temp list of components glyph codes of composite glyphs
		List<Int32> CompList = new List<Int32>();

		// reset bounding box in head table
		headTable.xMin = Int16.MaxValue;
		headTable.yMin = Int16.MaxValue;
		headTable.xMax = Int16.MinValue;
		headTable.yMax = Int16.MinValue;

		// reset some values in horizontal matrix header table
		hheaTable.advanceWidthMax = UInt16.MinValue;
		hheaTable.minLeftSideBearing = Int16.MaxValue;
		hheaTable.minRightSideBearing = Int16.MaxValue;
		hheaTable.xMaxExtent = Int16.MinValue;

		// glyphs zero, one and two are reserved
		for(Int32 GlyphCode = 0; GlyphCode < 3; GlyphCode++) AddGlyph(GlyphCode, GlyphList, CompList);

		// loop for all characters
		for(Int32 CharCode = FirstChar; CharCode <= LastChar; CharCode++)
			{
			// character is not active
			if(!ActiveChar[CharCode]) continue;

			// translate character code to glyph code
			Int32 GlyphCode = CharSubArray == null ? cmapSubTbl.CharToGlyph(CharCode) : cmapSubTbl.CharToGlyph(CharSubArray[CharCode]);

			// save it in CharToGlyph array
			CharToGlyphArray[CharCode - FirstChar] = (UInt16) GlyphCode;

			// load one glyph
			AddGlyph(GlyphCode, GlyphList, CompList);
			}

		// add composite glyphs
		if(CompList.Count != 0) AddCompositeGlyphs(GlyphList, CompList);

		// convert list to array		
		GlyphArray = GlyphList.ToArray();

		// save number of glyphs in maxpTable
		maxpTable.numGlyphs = (UInt16) GlyphArray.Length;

		// translate old glyph code to new one
		for(Int32 CharCode = 0; CharCode < CharToGlyphArray.Length; CharCode++)
			{
			Int32 Index = Array.BinarySearch(GlyphArray, new Glyph(CharToGlyphArray[CharCode]));
			if(Index < 0) throw new ApplicationException("Replace glyph number problem");
			CharToGlyphArray[CharCode] = (UInt16) Index;
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// add additional glyphs from the composite glyphs to the list
	////////////////////////////////////////////////////////////////////

	private void AddCompositeGlyphs
			(
			List<Glyph>	GlyphList,
			List<Int32> ExtraList
			)
		{
		// create a temp list of components of composite glyphs
		List<Int32> CompList = new List<Int32>();

		// loop for all characters
		foreach(Int32 GlyphCode in ExtraList) AddGlyph(GlyphCode, GlyphList, CompList);

		// add extra glyphs
		if(CompList.Count != 0) AddCompositeGlyphs(GlyphList, CompList);
		
		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// add additional glyphs to the list
	////////////////////////////////////////////////////////////////////

	private void AddGlyph
			(
			Int32		GlyphCode,
			List<Glyph>	GlyphList,
			List<Int32>	CompList
			)
		{
		// make sure it is available in glyph location table
		if(GlyphCode + 1 >= locaTable.Length) return;

		// create new glyph object
		Glyph NewGlyph = new Glyph(GlyphCode);

		// test for duplication
		Int32 GlyphIndex = GlyphList.BinarySearch(NewGlyph);
		if(GlyphIndex >= 0) return;

		// add it to the table
		GlyphList.Insert(~GlyphIndex, NewGlyph);

		// find glyph location and length within this table
		Int32 GlyphLoc = locaTable[GlyphCode];
		Int32 GlyphLen = locaTable[GlyphCode + 1] - GlyphLoc;

		// load glyph data
		Buffer = FontInfo.GetFontDataApi(OldGlyphTableOffset + GlyphLoc, GlyphLen);
		BufPtr = 0;

		// get advance width
		UInt16 AdvanceWidth = GlyphCode < hmtxTable.Length ? hmtxTable[GlyphCode] : hmtxTable[hmtxTable.Length - 1];
		NewGlyph.AdvanceWidth = AdvanceWidth;

		// save glyph data block
		NewGlyph.GlyphData = Buffer;

		// blank glyph
		if(Buffer == null) return;

		// decode number of contours
		Int16 Contours = ReadInt16BigEndian();
		NewGlyph.Composite = Contours < 0;

		// decode bounding box
		Int16 xMin = ReadInt16BigEndian();
		Int16 yMin = ReadInt16BigEndian();
		Int16 xMax = ReadInt16BigEndian();
		Int16 yMax = ReadInt16BigEndian();

		// save left side bearing
		NewGlyph.LeftSideBearing = xMin;

		// update head table
		if(xMin < headTable.xMin) headTable.xMin = xMin;
		if(yMin < headTable.yMin) headTable.yMin = yMin;
		if(xMax > headTable.xMax) headTable.xMax = xMax;
		if(yMax > headTable.yMax) headTable.yMax = yMax;

		// update hhea table
		if(AdvanceWidth > hheaTable.advanceWidthMax) hheaTable.advanceWidthMax = AdvanceWidth;
		if(xMin < hheaTable.minLeftSideBearing) hheaTable.minLeftSideBearing = xMin;
		Int16 Rsb = (Int16) ((Int32) AdvanceWidth - (Int32) xMax);
		if(Rsb < hheaTable.minRightSideBearing) hheaTable.minRightSideBearing = Rsb;
		if(xMax > hheaTable.xMaxExtent) hheaTable.xMaxExtent = xMax;

		// add component glyphs of a composite glyph to the list
		if(Contours < 0) GetCompositeGlyph(GlyphList, CompList);			

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Read one composite glyph from "glyf" table
	////////////////////////////////////////////////////////////////////

	public void GetCompositeGlyph
			(
			List<Glyph>	MainList,
			List<Int32> CompList
			)
		{
		// loop for components glyphs
		for(;;)
			{
			// read flags and glyph code
			CompFlag Flags = (CompFlag) ReadUInt16BigEndian();
			Int32 GlyphCode = ReadUInt16BigEndian();

			// do we have it in the main list
			Int32 Index;
			if(MainList.BinarySearch(new Glyph(GlyphCode)) < 0 && (Index = CompList.BinarySearch(GlyphCode)) < 0) CompList.Insert(~Index, GlyphCode);

			// read argument1 and 2
			if((Flags & CompFlag.Arg1AndArg2AreWords) == 0) BufPtr += 2;
			else BufPtr += 4;

			// we have one scale factor
			if((Flags & CompFlag.WeHaveAScale) != 0) BufPtr += 2; 

			// we have two scale factors
			else if((Flags & CompFlag.WeHaveXYScale) != 0) BufPtr += 4;

			// we have a transformation matrix
			else if((Flags & CompFlag.WeHave2By2) != 0) BufPtr += 8;

			// no more components
			if((Flags & CompFlag.MoreComponents) == 0) break;
			}
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Read one composite glyph from "glyf" table
	////////////////////////////////////////////////////////////////////

	public void ReplaceGlyphCode()
		{
		// loop looking for composite glyphs
		foreach(Glyph G in GlyphArray)
			{
			// not a composite glyph
			if(!G.Composite) continue;

			// get buffer
			Buffer = G.GlyphData;
			BufPtr = 10;

			// loop for components glyphs
			for(;;)
				{
				// read flags and old glyph code
				CompFlag Flags = (CompFlag) ReadUInt16BigEndian();
				Int32 GlyphCode = ReadUInt16BigEndian();

				// translate old glyph code to new one
				Int32 Index = Array.BinarySearch(GlyphArray, new Glyph(GlyphCode));
				if(Index < 0) throw new ApplicationException("Composite glyph number change");

				// replace glyph code
				BufPtr -= 2;
				WriteUInt16BigEndian((UInt16) Index);

				// read argument1 and 2
				if((Flags & CompFlag.Arg1AndArg2AreWords) == 0) BufPtr += 2;
				else BufPtr += 4;

				// we have one scale factor
				if((Flags & CompFlag.WeHaveAScale) != 0) BufPtr += 2; 

				// we have two scale factors
				else if((Flags & CompFlag.WeHaveXYScale) != 0) BufPtr += 4;

				// we have a transformation matrix
				else if((Flags & CompFlag.WeHave2By2) != 0) BufPtr += 8;

				// no more components
				if((Flags & CompFlag.MoreComponents) == 0) break;
				}
			}

		return;
		}

	////////////////////////////////////////////////////////////////////
	// Calculate "glyf" table checksum
	////////////////////////////////////////////////////////////////////

	public void CalculateGlyphChksum()
		{
		UInt32 Checksum = 0;
		Int32 Ptr = 0;

		// loop for all glyphs
		foreach(Glyph G in GlyphArray)
			{
			if(G.GlyphData != null) foreach(Byte B in G.GlyphData) Checksum += (UInt32) B << (24 - 8 * (Ptr++ & 3));
			}

		// save total length in table record array
		TableRecordArray[(Int32) Tag.glyf].Length = Ptr;

		// save checksum
		TableRecordArray[(Int32) Tag.glyf].Checksum = Checksum;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// build new glyph data file location table
	////////////////////////////////////////////////////////////////////

	private void BuildLocaTable()
		{
		// create location array
		Int32[] LocArray = new Int32[GlyphArray.Length + 1];

		// reset new glyph table length
		Int32 GlyphTableLength = 0;

		// current location
		Int32 Index = 0;

		// loop for all glyphs
		foreach(Glyph G in GlyphArray)
			{
			// save file location in array
			LocArray[Index++] = GlyphTableLength;

			if((GlyphTableLength & 1) != 0) throw new ApplicationException("Big problem");

			// update file location (for non blank glyphs)
			if(G.GlyphData != null) GlyphTableLength += G.GlyphData.Length;
			}

		// save final length at the last array location
		LocArray[Index] = GlyphTableLength;

		// save it in table record
		if(TableRecordArray[(Int32) Tag.glyf].Length != GlyphTableLength) throw new ApplicationException("Glyph table length");

		// test if the table can be stored in short integer
		headTable.IndexToLocFormat = (GlyphTableLength & 0xfffe0000) == 0 ? (Int16) 0 : (Int16) 1;

		// replace location array
		if(headTable.IndexToLocFormat == 0)
			{
			// short format
			Buffer = new Byte[2 * LocArray.Length];
			BufPtr = 0;
			foreach(Int32 Loc in LocArray) WriteInt16BigEndian(Loc >> 1);
			}
		else
			{
			// long format
			Buffer = new Byte[4 * LocArray.Length];
			foreach(Int32 Loc in LocArray) WriteUInt32BigEndian((UInt32) Loc);
			}

		// save in table record array
		TableRecordArray[(Int32) Tag.loca].Data = Buffer;

		// calculate checksum
		TableRecordArray[(Int32) Tag.loca].Checksum = TableChecksum(Buffer);

		// exit 
		return;
		}

	////////////////////////////////////////////////////////////////////
	// build new cmap table
	////////////////////////////////////////////////////////////////////

	private void BuildCharMapTable()
		{
		// create a new cmap sub table
		cmapSubTbl NewSubTbl = new cmapSubTbl(cmapSubTbl.PlatformID, cmapSubTbl.EncodingID, 4);
		NewSubTbl.Language = cmapSubTbl.Language;
		NewSubTbl.SegCount = 2;
		NewSubTbl.SegArray = new cmapSeg[2];
		NewSubTbl.GlyphArray = CharToGlyphArray;

		// test type of font
		if(cmapSubTbl.EncodingID != 0)
			// alphabetic font
			NewSubTbl.SegArray[0] = new cmapSeg(FirstChar, LastChar, 0, 2);
		else
			// symbolic font
			NewSubTbl.SegArray[0] = new cmapSeg(0xf000 + FirstChar, 0xf000 + LastChar, 0, 2);
		NewSubTbl.SegArray[1] = new cmapSeg(0xffff, 0xffff, 1, 0);

		// table size
		Int32 TblSize = 4 + 8 + 16 + 8 * NewSubTbl.SegCount + 2 * CharToGlyphArray.Length;
		Buffer = new Byte[TblSize];
		BufPtr = 0;

		// table version number is 0
		WriteUInt16BigEndian(0);

		// number of tables is 1
		WriteUInt16BigEndian(1);

		// platform id
		WriteUInt16BigEndian(NewSubTbl.PlatformID);

		// encoding id
		WriteUInt16BigEndian(NewSubTbl.EncodingID);

		// offset
		WriteUInt32BigEndian(4 + 8);

		// format
		WriteUInt16BigEndian(NewSubTbl.Format);

		// table length
		WriteInt16BigEndian((16 + 8 * NewSubTbl.SegCount + 2 * CharToGlyphArray.Length));		

		// language
		WriteUInt16BigEndian(NewSubTbl.Language);

		// segment count times 2
		WriteInt16BigEndian((NewSubTbl.SegCount * 2));

		// search range
		WriteUInt16BigEndian(NewSubTbl.SearchRange);
	
		// entry selector
		WriteUInt16BigEndian(NewSubTbl.EntrySelector);

		// range shift
		WriteUInt16BigEndian(NewSubTbl.RangeShift);

		// end character
		for(Int32 Seg = 0; Seg < NewSubTbl.SegCount; Seg++) WriteUInt16BigEndian(NewSubTbl.SegArray[Seg].EndChar);

		// padding
		WriteUInt16BigEndian(0);

		// start character
		for(Int32 Seg = 0; Seg < NewSubTbl.SegCount; Seg++) WriteUInt16BigEndian(NewSubTbl.SegArray[Seg].StartChar);

		// IDDelta
		for(Int32 Seg = 0; Seg < NewSubTbl.SegCount; Seg++) WriteInt16BigEndian(NewSubTbl.SegArray[Seg].IDDelta);

		// IDRangeOffset
		for(Int32 Seg = 0; Seg < NewSubTbl.SegCount; Seg++) WriteUInt16BigEndian((UInt16) (NewSubTbl.SegArray[Seg].IDRangeOffset * 2));

		// char to glyph translation
		for(Int32 Glyph = 0; Glyph < NewSubTbl.GlyphArray.Length; Glyph++) WriteUInt16BigEndian(NewSubTbl.GlyphArray[Glyph]);

		// save
		TableRecordArray[(Int32) Tag.cmap].Data = Buffer;

		// calculate checksum
		TableRecordArray[(Int32) Tag.cmap].Checksum = TableChecksum(Buffer);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Build new hmtx table
	////////////////////////////////////////////////////////////////////

	private void BuildhmtxTable()
		{
		// number of advance width and left bearing pairs
		Int32 HMSize = GlyphArray.Length - 1;
		UInt16 AdvanceWidth = GlyphArray[HMSize].AdvanceWidth;
		for(HMSize--; HMSize >= 0 && GlyphArray[HMSize].AdvanceWidth == AdvanceWidth; HMSize--);
		HMSize += 2;

		// calculate size of new table
		Int32 TableSize = 4 * HMSize;
		if(HMSize < GlyphArray.Length) TableSize += 2 * (GlyphArray.Length - HMSize);

		// allocate buffer
		Buffer = new Byte[TableSize];
		BufPtr = 0;

		// output advance width and left bearing pairs
		Int32 Index;
		for(Index = 0; Index < HMSize; Index++)
			{
			WriteUInt16BigEndian(GlyphArray[Index].AdvanceWidth);
			WriteInt16BigEndian(GlyphArray[Index].LeftSideBearing);
			}

		// output left bearing pairs
		for(; Index < GlyphArray.Length; Index++)
			{
			WriteInt16BigEndian(GlyphArray[Index].LeftSideBearing);
			}

		// save number of advance width and left bearing pairs
		hheaTable.numberOfHMetrics = (UInt16) HMSize;

		// save in table record array
		TableRecordArray[(Int32) Tag.hmtx].Data = Buffer;

		// calculate checksum
		TableRecordArray[(Int32) Tag.hmtx].Checksum = TableChecksum(Buffer);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// build new header table
	// must be after BuildGlyphLocationTable()
	////////////////////////////////////////////////////////////////////

	private void BuildHeadTable()
		{
		// allocate buffer for head table
		Buffer = new Byte[54];
		BufPtr = 0;

		// move info into buffer
		WriteUInt32BigEndian(headTable.TableVersion);
		WriteUInt32BigEndian(headTable.FontRevision);
		WriteUInt32BigEndian(0);
		WriteUInt32BigEndian(headTable.MagicNumber);
		WriteUInt16BigEndian(headTable.Flags);
		WriteUInt16BigEndian(headTable.UnitsPerEm);
		WriteInt64BigEndian(headTable.TimeCreated);
		WriteInt64BigEndian(headTable.TimeModified);
		WriteInt16BigEndian(headTable.xMin);
		WriteInt16BigEndian(headTable.yMin);
		WriteInt16BigEndian(headTable.xMax);
		WriteInt16BigEndian(headTable.yMax);
		WriteUInt16BigEndian(headTable.MacStyle);
		WriteUInt16BigEndian(headTable.LowestRecPPEM);
		WriteInt16BigEndian(headTable.FontDirectionHint);
		WriteInt16BigEndian(headTable.IndexToLocFormat);
		WriteInt16BigEndian(headTable.glyphDataFormat);

		// save in table record array
		TableRecordArray[(Int32) Tag.head].Data = Buffer;

		// calculate checksum
		TableRecordArray[(Int32) Tag.head].Checksum = TableChecksum(Buffer);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Build new "hhea" table
	////////////////////////////////////////////////////////////////////

	private void BuildHheaTable()
		{
		// allocate buffer
		Buffer = new Byte[36];
		BufPtr = 0;

		// build new hhea table
		WriteUInt32BigEndian(hheaTable.TableVersion);
		WriteInt16BigEndian(hheaTable.Ascender);
		WriteInt16BigEndian(hheaTable.Descender);
		WriteInt16BigEndian(hheaTable.LineGap);
		WriteUInt16BigEndian(hheaTable.advanceWidthMax);
		WriteInt16BigEndian(hheaTable.minLeftSideBearing);
		WriteInt16BigEndian(hheaTable.minRightSideBearing);
		WriteInt16BigEndian(hheaTable.xMaxExtent);
		WriteInt16BigEndian(hheaTable.caretSlopeRise);
		WriteInt16BigEndian(hheaTable.caretSlopeRun);
		WriteInt16BigEndian(hheaTable.caretOffset);
		WriteInt16BigEndian(hheaTable.Reserved1);
		WriteInt16BigEndian(hheaTable.Reserved2);
		WriteInt16BigEndian(hheaTable.Reserved3);
		WriteInt16BigEndian(hheaTable.Reserved4);
		WriteInt16BigEndian(hheaTable.metricDataFormat);
		WriteUInt16BigEndian(hheaTable.numberOfHMetrics);

		// save in table record array
		TableRecordArray[(Int32) Tag.hhea].Data = Buffer;

		// calculate checksum
		TableRecordArray[(Int32) Tag.hhea].Checksum = TableChecksum(Buffer);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Read "maxp" table
	////////////////////////////////////////////////////////////////////

	private void BuildMaxpTable()
		{
		// allocate buffer
		Buffer = new Byte[32];
		BufPtr = 0;

		// build new hhea table
		WriteUInt32BigEndian(maxpTable.TableVersion);
		WriteUInt16BigEndian(maxpTable.numGlyphs);
		WriteUInt16BigEndian(maxpTable.maxPoints);
		WriteUInt16BigEndian(maxpTable.maxContours);
		WriteUInt16BigEndian(maxpTable.maxCompositePoints);
		WriteUInt16BigEndian(maxpTable.maxCompositeContours);
		WriteUInt16BigEndian(maxpTable.maxZones);
		WriteUInt16BigEndian(maxpTable.maxTwilightPoints);
		WriteUInt16BigEndian(maxpTable.maxStorage);
		WriteUInt16BigEndian(maxpTable.maxFunctionDefs);
		WriteUInt16BigEndian(maxpTable.maxInstructionDefs);
		WriteUInt16BigEndian(maxpTable.maxStackElements);
		WriteUInt16BigEndian(maxpTable.maxSizeOfInstructions);
		WriteUInt16BigEndian(maxpTable.maxComponentElements);
		WriteUInt16BigEndian(maxpTable.maxComponentDepth);

		// save in table record array
		TableRecordArray[(Int32) Tag.maxp].Data = Buffer;

		// calculate checksum
		TableRecordArray[(Int32) Tag.maxp].Checksum = TableChecksum(Buffer);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// build new font program tables
	////////////////////////////////////////////////////////////////////

	private void BuildFontProgramTables()
		{
		// recalculate checksum
		// in some cases the calculated checksum does not agree with the one returned by the api
		TableRecordArray[(Int32) Tag.cvt].Checksum = TableChecksum(TableRecordArray[(Int32) Tag.cvt].Data);
		TableRecordArray[(Int32) Tag.fpgm].Checksum = TableChecksum(TableRecordArray[(Int32) Tag.fpgm].Data);
		TableRecordArray[(Int32) Tag.prep].Checksum = TableChecksum(TableRecordArray[(Int32) Tag.prep].Data);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// build new font file
	////////////////////////////////////////////////////////////////////

	private Byte[] BuildEmbeddedFile()
		{
		// replace number of tables in file header
		FileHeader.NumTables = (UInt16) TableRecordArray.Length;

		// allocate buffer for file header plus table records
		Int32 HeaderSize = 12 + 16 * TableRecordArray.Length;
		Buffer = new Byte[HeaderSize];
		BufPtr = 0;		

		// write file header to embedded file
		WriteUInt32BigEndian(FileHeader.FileVersion);
		WriteUInt16BigEndian(FileHeader.NumTables);
		WriteUInt16BigEndian(FileHeader.SearchRange);
		WriteUInt16BigEndian(FileHeader.EntrySelector);
		WriteUInt16BigEndian(FileHeader.RangeShift);

		// table offset
		Int32 FileLength = HeaderSize;

		// reset file checksum
		UInt32 ChecksumAdjustment = 0;

		// write table record array
		foreach(TableRecord TR in TableRecordArray)
			{
			// table tag
			WriteUInt32BigEndian(TR.Tag);

			// table checksum
			WriteUInt32BigEndian(TR.Checksum);
			ChecksumAdjustment += TR.Checksum;

			// file offset
			WriteUInt32BigEndian((UInt32) FileLength);
			TR.Offset = FileLength;

			// length of actual data
			Int32 Length = TR.Tag != glyfTag ? TR.Data.Length : TR.Length;
			WriteUInt32BigEndian((UInt32) Length);

			// make sure offset is on 4 bytes boundry
			FileLength += (Length + 3) & ~3;
			}

		// calculate checksum of header plus table records
		ChecksumAdjustment = 0xb1b0afba - (ChecksumAdjustment + TableChecksum(Buffer));

		// save header buffer
		Byte[] Header = Buffer;

		// allocate buffer for full size file
		Buffer = new Byte[FileLength];

		// copy header to buffer
		Array.Copy(Header, Buffer, Header.Length);
		BufPtr = Header.Length;

		// we do not need header buffer
		Header = null;

		// write tables
		foreach(TableRecord TR in TableRecordArray)
			{
			if(BufPtr != TR.Offset) throw new ApplicationException("Table offset");

			// all tables but glyph
			if(TR.Tag != glyfTag)
				{
				Array.Copy(TR.Data, 0, Buffer, BufPtr, TR.Data.Length);
				BufPtr += TR.Data.Length;
				}

			// glyph table
			else
				{
				foreach(Glyph G in GlyphArray)
					{
					if(G.GlyphData == null) continue;
					Array.Copy(G.GlyphData, 0, Buffer, BufPtr, G.GlyphData.Length);
					BufPtr += G.GlyphData.Length;
					}
				}

			// make sure buffer pointer is on 4 bytes boundry
			for(; (BufPtr & 3) != 0; BufPtr++) Buffer[BufPtr] = 0;
			}

		if(BufPtr != FileLength) throw new ApplicationException("Table offset");

		// insert checksum adjustment to head table
		BufPtr = TableRecordArray[(Int32) Tag.head].Offset + 8;
		WriteUInt32BigEndian(ChecksumAdjustment);

		// write 
		return(Buffer);
		}

	////////////////////////////////////////////////////////////////////
	// Read Int16 from byte array big endian style
	////////////////////////////////////////////////////////////////////

	private Int16 ReadInt16BigEndian()
		{
		return((Int16) (((Int32) Buffer[BufPtr++] << 8) | (Int32) Buffer[BufPtr++]));
		}

	////////////////////////////////////////////////////////////////////
	// Read UInt16 from byte array big endian style
	////////////////////////////////////////////////////////////////////

	private UInt16 ReadUInt16BigEndian()
		{
		return((UInt16) (((UInt32) Buffer[BufPtr++] << 8) | (UInt32) Buffer[BufPtr++]));
		}

	////////////////////////////////////////////////////////////////////
	// Read UInt32 from byte array big endian style
	////////////////////////////////////////////////////////////////////

	private UInt32 ReadUInt32BigEndian()
		{
		return(((UInt32) Buffer[BufPtr++] << 24) | ((UInt32) Buffer[BufPtr++] << 16) | ((UInt32) Buffer[BufPtr++] << 8) | (UInt32) Buffer[BufPtr++]);
		}

	////////////////////////////////////////////////////////////////////
	// Read Int64 from byte array big endian style
	////////////////////////////////////////////////////////////////////

	private Int64 ReadInt64BigEndian()
		{
		return(((UInt32) Buffer[BufPtr++] << 56) | ((UInt32) Buffer[BufPtr++] << 48) | ((UInt32) Buffer[BufPtr++] << 40) | ((UInt32) Buffer[BufPtr++] << 32) | 
			((UInt32) Buffer[BufPtr++] << 24) | ((UInt32) Buffer[BufPtr++] << 16) | ((UInt32) Buffer[BufPtr++] << 8) | (UInt32) Buffer[BufPtr++]);
		}

	////////////////////////////////////////////////////////////////////
	// Write Int16 to byte list big endian style
	////////////////////////////////////////////////////////////////////

	private void WriteInt16BigEndian
			(
			Int32	Value
			)
		{
		Buffer[BufPtr++] = (Byte) (Value >> 8);
		Buffer[BufPtr++] = (Byte) Value;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Write Int16 or UInt16 to byte list big endian style
	////////////////////////////////////////////////////////////////////

	private void WriteUInt16BigEndian
			(
			UInt32	Value
			)
		{
		Buffer[BufPtr++] = (Byte) (Value >> 8);
		Buffer[BufPtr++] = (Byte) Value;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Write Int32 or UInt32 to byte list big endian style
	////////////////////////////////////////////////////////////////////

	private void WriteUInt32BigEndian
			(
			UInt32	Value
			)
		{
		Buffer[BufPtr++] = (Byte) (Value >> 24);
		Buffer[BufPtr++] = (Byte) (Value >> 16);
		Buffer[BufPtr++] = (Byte) (Value >> 8);
		Buffer[BufPtr++] = (Byte) Value;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Write Int64 or UInt64 to byte list big endian style
	////////////////////////////////////////////////////////////////////

	private void WriteInt64BigEndian
			(
			Int64	Value
			)
		{
		Buffer[BufPtr++] = (Byte) (Value >> 56);
		Buffer[BufPtr++] = (Byte) (Value >> 48);
		Buffer[BufPtr++] = (Byte) (Value >> 40);
		Buffer[BufPtr++] = (Byte) (Value >> 32);
		Buffer[BufPtr++] = (Byte) (Value >> 24);
		Buffer[BufPtr++] = (Byte) (Value >> 16);
		Buffer[BufPtr++] = (Byte) (Value >> 8);
		Buffer[BufPtr++] = (Byte) Value;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Calculate table checksum
	////////////////////////////////////////////////////////////////////

	private UInt32 TableChecksum
			(
			Byte[]	Table
			)
		{
		UInt32 ChkSum = 0;
		for(Int32 Ptr = 0; Ptr < Table.Length; Ptr++) ChkSum += (UInt32) Table[Ptr] << (24 - 8 * (Ptr & 3));
		return(ChkSum);
		}

	////////////////////////////////////////////////////////////////////
	// convert table tag from binary to string
	////////////////////////////////////////////////////////////////////

	private static String TagBinToStr
			(
			UInt32	BinTag
			)
		{
		StringBuilder StrTag = new StringBuilder("????");
		for(Int32 Index = 0; Index < 4; Index++)
			{
			Byte Ch = (Byte) (BinTag >> (24 - 8 * Index));
			if(Ch >= 32 && Ch <= 126) StrTag[Index] = (Char) Ch;
			}
		return(StrTag.ToString());			
		}
	}
}
