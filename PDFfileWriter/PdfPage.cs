/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfPage
//	PDF page class. An indirect PDF object.
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
public class PdfPage : PdfObject
	{
	internal List<PdfContents> ContentsArray;

	////////////////////////////////////////////////////////////////////
	// Default constructor
	// Page size is taken from PdfDocument
	////////////////////////////////////////////////////////////////////

	public PdfPage
			(
			PdfDocument Document
			) : base(Document, false, "/Page")
		{
		PdfPageConstructor(Document.PageSize.Width, Document.PageSize.Height);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor
	// PageSize override the default page size
	////////////////////////////////////////////////////////////////////

	public PdfPage
			(
			PdfDocument		Document,
			SizeD			PageSize
			) : base(Document, false, "/Page")
		{
		PdfPageConstructor(ToPt(PageSize.Width), ToPt(PageSize.Height));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor
	// PaperType and orientation override the default page size
	////////////////////////////////////////////////////////////////////

	public PdfPage
			(
			PdfDocument		Document,
			PaperType		PaperType,
			Boolean			Landscape
			) : base(Document, false, "/Page")
		{
		// get standard paper size
		Double Width = PdfDocument.PaperTypeSize[(Int32) PaperType].Width;
		Double Height = PdfDocument.PaperTypeSize[(Int32) PaperType].Height;

		// for landscape swap width and height
		if(Landscape)
			{
			Double Temp = Width;
			Width = Height;
			Height = Temp;
			}

		// exit
		PdfPageConstructor(Width, Height);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor
	// Width and Height override the default page size
	////////////////////////////////////////////////////////////////////

	public PdfPage
			(
			PdfDocument		Document,
			Double			Width,
			Double			Height
			) : base(Document, false, "/Page")
		{
		PdfPageConstructor(ToPt(Width), ToPt(Height));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Add existing contents to page
	////////////////////////////////////////////////////////////////////

	public void AddContents
			(
			PdfContents	Contents
			)
		{
		// set page contents flag
		Contents.PageContents = true;
 
		// add content to content array
		if(ContentsArray == null) ContentsArray = new List<PdfContents>();
		ContentsArray.Add(Contents);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor common method
	////////////////////////////////////////////////////////////////////

	private void PdfPageConstructor
			(
			Double	Width,
			Double	Height
			)
		{
		// add page to parent array of pages
		if(Document.PageCount == 0)
			{
			Document.PagesObject.AddToDictionary("/Kids", String.Format("[{0} 0 R]", ObjectNumber));
			}
		else
			{
			String Kids = Document.PagesObject.GetDictionaryValue("/Kids");
			Document.PagesObject.AddToDictionary("/Kids", Kids.Replace("]", String.Format(" {0} 0 R]", ObjectNumber)));			
			}

		// update page count
		Document.PagesObject.AddToDictionary("/Count", (++Document.PageCount).ToString());

		// link page to parent
		AddToDictionary("/Parent", Document.PagesObject);

		// add page size in points
		AddToDictionary("/MediaBox", String.Format(NFI.DecSep, "[0 0 {0} {1}]", Round(Width), Round(Height)));

		// exit
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
		// we have at least one contents object
		if(ContentsArray != null)
			{
			// contents dictionary entry
			StringBuilder ContentsStr = new StringBuilder("[");

			// resources array of all contents objects
			List<PdfObject> ResObjects = new List<PdfObject>();

			// loop for all contents objects
			foreach(PdfContents Contents in ContentsArray)
				{
				// build contents dictionary entry
				ContentsStr.AppendFormat("{0} 0 R ", Contents.ObjectNumber);

				// make sure we have resources
				if(Contents.ResObjects != null)
					{
					// loop for resources within this contents object
					foreach(PdfObject ResObject in Contents.ResObjects)
						{
						// check if we have it already
						Int32 Ptr = ResObjects.BinarySearch(ResObject);
						if(Ptr < 0) ResObjects.Insert(~Ptr, ResObject);
						}
					}
				}

			// add terminating bracket
			if(ContentsStr.Length > 1)
				{
				ContentsStr.Length--;
				ContentsStr.Append(']');
				AddToDictionary("/Contents", ContentsStr.ToString());
				}

			// save to dictionary
			AddToDictionary("/Resources", BuildResourcesDictionary(ResObjects, true));
			}

		// call PdfObject routine
		base.WriteObjectToPdfFile(PdfFile);

		// exit
		return;
		}
	}
}
