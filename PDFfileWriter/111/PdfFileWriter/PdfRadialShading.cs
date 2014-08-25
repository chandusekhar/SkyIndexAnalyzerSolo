/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfRadialShading
//	PDF radial shading resource class.
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
using System.Linq;
using System.Text;
using System.IO;

namespace PdfFileWriter
{
public class PdfRadialShading : PdfObject
	{
	////////////////////////////////////////////////////////////////////
	// Constructor
	////////////////////////////////////////////////////////////////////

	public PdfRadialShading
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
		ResourceCode = Document.GenerateResourceNumber('R');

		// color space red, green and blue
		AddToDictionary("/ColorSpace", "/DeviceRGB");

		// shading type radial
		AddToDictionary("/ShadingType", "3");

		// bounding box
		AddToDictionary("/BBox", String.Format(NFI.DecSep, "[{0} {1} {2} {3}]", ToPt(PosX), ToPt(PosY), ToPt(PosX + Width), ToPt(PosY + Height)));

		// set center to bounding box center and radius to half the diagonal
		AddToDictionary("/Coords", String.Format(NFI.DecSep, "[{0} {1} {2} {0} {1} 0]",
			ToPt(PosX + Width / 2), ToPt(PosY + Height / 2), ToPt(Math.Sqrt(Width * Width + Height * Height) / 2)));

		// add shading function to shading dictionary
		AddToDictionary("/Function", ShadingFunction);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set circles
	////////////////////////////////////////////////////////////////////

	public void SetCircle
			(
			Double	PosX0,
			Double	PosY0,
			Double	Rad0
			)
		{
		AddToDictionary("/Coords", String.Format(NFI.DecSep, "[{0} {1} {2} {0} {1} 0]", ToPt(PosX0), ToPt(PosY0), ToPt(Rad0)));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set circles
	////////////////////////////////////////////////////////////////////

	public void SetCircles
			(
			Double	PosX0,
			Double	PosY0,
			Double	Rad0,
			Double	PosX1,
			Double	PosY1,
			Double	Rad1
			)
		{
		AddToDictionary("/Coords", String.Format(NFI.DecSep, "[{0} {1} {2} {3} {4} {5}]",
			ToPt(PosX0), ToPt(PosY0), ToPt(Rad0), ToPt(PosX1), ToPt(PosY1), ToPt(Rad1)));
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
