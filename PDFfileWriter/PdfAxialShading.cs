/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfAxialShading
//	PDF Axial shading indirect object.
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
////////////////////////////////////////////////////////////////////
// PDF Axial Shading object.
// Derived class from PdfObject
////////////////////////////////////////////////////////////////////

public class PdfAxialShading : PdfObject
	{
	////////////////////////////////////////////////////////////////////
	// Constructor
	////////////////////////////////////////////////////////////////////

	public PdfAxialShading
			(
			PdfDocument		Document,
			Double			PosX,
			Double			PosY,
			Double			Width,
			Double			Height,
			PdfShadingFunction	ShadingFunction
			) : base(Document, false)
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('A');

		// color space red, green and blue
		AddToDictionary("/ColorSpace", "/DeviceRGB");

		// shading type axial
		AddToDictionary("/ShadingType", "2");

		// bounding box
		AddToDictionary("/BBox", String.Format(NFI.DecSep, "[{0} {1} {2} {3}]", ToPt(PosX), ToPt(PosY), ToPt(PosX + Width), ToPt(PosY + Height)));

		// assume the direction of color change is along x axis
		AddToDictionary("/Coords", String.Format(NFI.DecSep, "[{0} {1} {2} {1}]", ToPt(PosX), ToPt(PosY), ToPt(PosX + Width)));

		// add shading function to shading dictionary
		AddToDictionary("/Function", ShadingFunction);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set Axis direction
	////////////////////////////////////////////////////////////////////

	public void SetAxis
			(
			Double	PosX,
			Double	PosY,
			Double	Width,
			Double	Height
			)
		{
		AddToDictionary("/Coords", String.Format(NFI.DecSep, "[{0} {1} {2} {3}]", ToPt(PosX), ToPt(PosY), ToPt(PosX + Width), ToPt(PosY + Height)));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// AntiAlias
	////////////////////////////////////////////////////////////////////

	public void AntiAlias
			(
			Boolean		Value
			)
		{
		AddToDictionary("/AntiAlias", Value ? "true" : "false"); 
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Extend shading beyond axis
	////////////////////////////////////////////////////////////////////

	public void ExtendShading
			(
			Boolean		Before,
			Boolean		After
			)
		{
		AddToDictionary("/Extend", String.Format("[{0} {1}]", Before ? "true" : "false", After ? "true" : "false")); 
		return;
		}
	}
}
