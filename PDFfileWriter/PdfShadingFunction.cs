/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfShadingFunction
//	Support class for both axial and radial shading resources.
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
using System.Drawing;

namespace PdfFileWriter
{
////////////////////////////////////////////////////////////////////
// PDF function to convert a number between 0 and 1 into a
// color red green and blue based on the sample color array
////////////////////////////////////////////////////////////////////

public class PdfShadingFunction : PdfObject
	{
	////////////////////////////////////////////////////////////////////
	// Constructor
	////////////////////////////////////////////////////////////////////

	public PdfShadingFunction
			(
			PdfDocument		Document,		// PDF document object
			Color[]			ColorArray		// Array of colors. Minimum 2.
			) : base(Document, true)
		{
		// test for error
		if(ColorArray.Length < 2) throw new ApplicationException("Shading function color array must have two or more items");

		// the shading function is a sampled function
		AddToDictionary("/FunctionType", "0");

		// input variable is between 0 and 1
		AddToDictionary("/Domain", "[0 1]");

		// output variables are red, green and blue color components between 0 and 1
		AddToDictionary("/Range", "[0 1 0 1 0 1]");

		// each color components in the stream is 8 bits
		AddToDictionary("/BitsPerSample", "8");

		// number of colors in the stream must be two or more
		AddToDictionary("/Size", String.Format("[{0}]", ColorArray.Length.ToString()));

		// add color array to contents stream
		foreach(Color Color in ColorArray)
			{
			ContentsString.Append((Char) Color.R);	// red
			ContentsString.Append((Char) Color.G);	// green
			ContentsString.Append((Char) Color.B);	// blue
			}
		return;
		}
	}
}
