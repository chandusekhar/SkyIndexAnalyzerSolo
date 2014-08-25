/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfDocument
//	The main class of PDF object.
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
using System.Globalization;

namespace PdfFileWriter
{
/////////////////////////////////////////////////////////////////////
// User unit of measure enumeration
/////////////////////////////////////////////////////////////////////

public enum UnitOfMeasure
	{
	Point,
	Inch,
	cm,
	mm,
	}

/////////////////////////////////////////////////////////////////////
// standard paper size enumeration
/////////////////////////////////////////////////////////////////////

public enum PaperType
	{
	Letter,
	Legal,
	A4,			// 210mm 297mm
	}

/////////////////////////////////////////////////////////////////////
// Number Format Info
// Adobe readers expect decimal separator to be period.
// Some countries define decimal separator as comma.
// The project uses NFI.DecSep to force period for all regions.
/////////////////////////////////////////////////////////////////////

public static class NFI
	{
	internal static NumberFormatInfo DecSep;			// number format info decimal separetor is period
	static NFI()
		{
		// number format (decimal separator is period)
		DecSep = new NumberFormatInfo();
		DecSep.NumberDecimalSeparator = ".";
		return;
		}
	}

/////////////////////////////////////////////////////////////////////
// PdfDocument Class
// The main class for controlling the production of the PDF document.
// Step 1: Instantiate one PdfDocument class.
// Step 2: Add resources and pages to the class.
// Step 3: Call CreateFile method to output the document.
/////////////////////////////////////////////////////////////////////

public class PdfDocument
	{
	internal	Double		ScaleFactor;		// from unit of measure to points
	internal	SizeD		PageSize;			// in points
	internal	Int32		PageCount;			// number of pages in the document
	internal	List<PdfObject>	ObjectArray;	// list of all PDF indirect objects for this document
	internal 	PdfObject	PagesObject;		// parent object of all pages
	internal	PdfObject	ProcSetObject;		// standard proc set object for all page resources dictionaries
	internal	List<ResNo> ResNoArray;			// resource number

	public		Boolean		Debug = false;		// Debug flag. Default is false. The program will generate normal PDF file.
												// If true do not compress contents, replace images and font file with text place holder.
												// The generated file can be viewed with a text editor but cannot be loaded to PDF reader.

	public static readonly String	RevisionNumber = "1.2";
	public static readonly String	RevisionDate = "2013/07/21";

	// translation of user units to points
	// must agree with UnitOfMeasure enumeration
	internal static Double[] UnitInPoints = new Double[]
		{
		1.0,			// Point
		72.0,			// Inch
		72.0 / 2.54,	// cm
		72.0 / 25.4,	// mm
		};

	// standard paper sizes (in points)
	// must agree with PaperType enumeration
	internal static SizeD[] PaperTypeSize = new SizeD[]
		{
		new SizeD(8.5 * 72, 11.0 * 72),					// letter
		new SizeD(8.5 * 72, 14.0 * 72),					// legal
		new SizeD(21.0 * 72 / 2.54, 29.7 * 72 / 2.54),	// A4
		};

	////////////////////////////////////////////////////////////////////
	// Default constructor
	// Default page size is letter (height 11”, width 8.5”)
	// Page orientation is portrait
	// Unit of measure is points (scale factor 1.0)
	////////////////////////////////////////////////////////////////////

	public PdfDocument()
		{
		// initialize object array
		ObjectArray = new List<PdfObject>();

		// PDF document root object the Catalog object
		PdfObject Catalog = new PdfObject(this, false, "/Catalog");

		// add viewer preferences
		Catalog.AddToDictionary("/ViewerPreferences", "<</PrintScaling/None>>");

		// Parent object for all pages
		PagesObject = new PdfObject(this, false, "/Pages");

		// add indirect reference to pages within the catalog object
		Catalog.AddToDictionary("/Pages", PagesObject);

		// create standard proc set
		ProcSetObject = new PdfObject(this, false);
		ProcSetObject.ContentsString = new StringBuilder("[/PDF/Text/ImageB/ImageC/ImageI]");

		// save page default size
		PageSize = new SizeD(8.5 * 72.0, 11.0 * 72.0);

		// scale factor
		ScaleFactor = 1.0;

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor
	// Default page size is Width and Height in user unit of measure
	// Default page orientation is portrait if Height > Width landscape if Height < Width
	// Scale factor from user unit of measure to points (i.e. 72.0 for inch)
	////////////////////////////////////////////////////////////////////

	public PdfDocument
			(
			Double		Width,			// page width
			Double		Height,			// page height
			Double		ScaleFactor		// scale factor from user units to points (i.e. 72.0 for inch)
			) : this()
		{
		// set scale factor (user units to points)
		this.ScaleFactor = ScaleFactor;

		// save page default size
		this.PageSize.Width = Width * ScaleFactor;
		this.PageSize.Height = Height * ScaleFactor;

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor
	// Default page size is Width and Height in user unit of measure
	// Default page orientation is portrait if Height > Width landscape if Height < Width
	// Unit of measure is: Point, Inch, cm, mm
	////////////////////////////////////////////////////////////////////

	public PdfDocument
			(
			Double			Width,			// page width
			Double			Height,			// page height
			UnitOfMeasure	UnitOfMeasure	// unit of measure: Point, Inch, cm, mm
			) : this()
		{
		// set scale factor (user units to points)
		ScaleFactor = UnitInPoints[(Int32) UnitOfMeasure];

		// save page default size
		this.PageSize.Width = Width * ScaleFactor;
		this.PageSize.Height = Height * ScaleFactor;

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor
	// Default page is Letter, Legal or A4
	// Default page orientation is portrait or landscape
	// Unit of measure is: Point, Inch, cm, mm
	////////////////////////////////////////////////////////////////////

	public PdfDocument
			(
			PaperType		PaperType,		// Letter, Legal, A4
			Boolean			Landscape,		// if true width > height, if false height >= width
			UnitOfMeasure	UnitOfMeasure	// Point, Inch, Cm, mm
			) : this()
		{
		// set scale factor (user units to points)
		ScaleFactor = UnitInPoints[(Int32) UnitOfMeasure];

		// get standard paper size
		PageSize.Width = PaperTypeSize[(Int32) PaperType].Width;
		PageSize.Height = PaperTypeSize[(Int32) PaperType].Height;

		// for landscape swap width and height
		if(Landscape)
			{
			Double Temp = PageSize.Width;
			PageSize.Width = PageSize.Height;
			PageSize.Height = Temp;
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Create PDF document file.
	// The last step of document creation after all pages were constructed.
	// FileName is the path and name of the output file.
	////////////////////////////////////////////////////////////////////
	
	public void CreateFile
			(
			String	FileName	// PDF document file name
			)
		{
		// convert stream to binary writer
		using (BinaryWriter PdfFile =
			new BinaryWriter(new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None), Encoding.UTF8))
			{		
			// write PDF version number
			PdfFile.Write(Encoding.ASCII.GetBytes("%PDF-1.7\n"));

			// add this comment to tell compression programs that this is a binary file
			PdfFile.Write(Encoding.ASCII.GetBytes("%\u00b5\u00b5\u00b5\u00b5\n"));

			// objects
			for(Int32 Index = 0; Index < ObjectArray.Count; Index++)
				{
				// save file position for this object
				ObjectArray[Index].FilePosition = (Int32) PdfFile.BaseStream.Position;

				// write object to pdf file
				ObjectArray[Index].WriteObjectToPdfFile(PdfFile);
				}

			// save cross reference table position
			Int32 XRefPos = (Int32) PdfFile.BaseStream.Position;

			// cross reference
			PdfFile.Write(Encoding.ASCII.GetBytes(String.Format("xref\n0 {0}\n0000000000 65535 f \n", ObjectArray.Count + 1)));
			foreach(PdfObject PO in ObjectArray)
				{
				PdfFile.Write(Encoding.ASCII.GetBytes(String.Format("{0:0000000000} 00000 n \n", PO.FilePosition)));
				}

			// generate new id
			String FileID = Guid.NewGuid().ToString("N");

			// trailer
			PdfFile.Write(Encoding.ASCII.GetBytes(String.Format("trailer\n<</Size {0}/Root 1 0 R/ID [<{1}><{1}>]>>\nstartxref\n{2}\n",
				ObjectArray.Count + 1, FileID, XRefPos)));

			// write PDF end of file marker
			PdfFile.Write(Encoding.UTF8.GetBytes("%%EOF\n"));

			// close file
			PdfFile.Close();
			}

		// successful exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Generate unique resource number
	////////////////////////////////////////////////////////////////////

	internal String GenerateResourceNumber
			(
			Char	Code		// one letter code for each type of resource
			)
		{
		// create resource number list
		if(ResNoArray == null) ResNoArray = new List<ResNo>();

		// search for code
		ResNo ResourceCode = new ResNo(Code);
		Int32 Index = ResNoArray.BinarySearch(ResourceCode);

		// not found
		if(Index < 0)
			{
			Index = ~Index;
			ResNoArray.Insert(Index, ResourceCode);
			}

		// create resource code
		return(String.Format("/{0}{1}", Code, ++ResNoArray[Index].Number));
		}
	}

/////////////////////////////////////////////////////////////////////
// Resource code number class
// This class used to generate unique resource codes
/////////////////////////////////////////////////////////////////////

internal class ResNo : IComparable<ResNo>
	{
	internal Char	Code;
	internal Int32	Number;

	internal ResNo
			(
			Char	Code
			)
		{
		this.Code = Code;
		return;
		}

	public Int32 CompareTo
			(
			ResNo	Other
			)
		{
		return(this.Code - Other.Code);
		}
	}
}
