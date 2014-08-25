/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfXObject
//	PDF X Object resource class.
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

namespace PdfFileWriter
{
public class PdfXObject : PdfContents
	{
	////////////////////////////////////////////////////////////////////
	// Constructor
	////////////////////////////////////////////////////////////////////

	public PdfXObject
			(
			PdfDocument		Document,
			Double			Width,
			Double			Height
			) : base(Document, "/XObject")
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('X');

		// add items to dictionary
		AddToDictionary("/Subtype", "/Form");
		AddToDictionary("/FormType", "1");
		AddToDictionary("/BBox", String.Format(NFI.DecSep, "[0 0 {0} {1}]", ToPt(Width), ToPt(Height)));
		return;
		}
	}
}
