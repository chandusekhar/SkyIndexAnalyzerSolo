/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfTilingPattern
//	PDF tiling pattern resource class.
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
public enum TilingType
	{
	Constant = 1,
	NoDistortion,
	ConstantAndFast,
	}

public class PdfTilingPattern : PdfContents
	{
	////////////////////////////////////////////////////////////////////
	// Constructor
	// Note: this program support only color tiling pattern
	// PaintType = 1
	////////////////////////////////////////////////////////////////////

	public PdfTilingPattern
			(
			PdfDocument		Document
			) : base(Document, "/Pattern")
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('T');

		// add items to dictionary
		AddToDictionary("/PatternType", "1");		// Tiling pattern
		AddToDictionary("/PaintType", "1");			// color
		AddToDictionary("/TilingType", "1");		// constant
		AddToDictionary("/BBox", String.Format(NFI.DecSep, "[0 0 {0} {1}]", ToPt(1.0), ToPt(1.0)));
		AddToDictionary("/XStep", String.Format(NFI.DecSep, "{0}", ToPt(1.0)));
		AddToDictionary("/YStep", String.Format(NFI.DecSep, "{0}", ToPt(1.0)));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set tiling type
	////////////////////////////////////////////////////////////////////

	public void SetTilingType
			(
			TilingType	TilingType
			)
		{
		// by default the constructor set tiling type to 1 = constant
		AddToDictionary("/TilingType", ((Int32) TilingType).ToString());
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set square bounding box and equal step
	////////////////////////////////////////////////////////////////////

	public void SetTileBox
			(
			Double			Side
			)
		{
		SetTileBox(Side, Side, Side, Side);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set rectangle bounding box and equal step
	////////////////////////////////////////////////////////////////////

	public void SetTileBox
			(
			Double			Width,
			Double			Height
			)
		{
		SetTileBox(Width, Height, Width, Height);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set bounding box and step
	////////////////////////////////////////////////////////////////////

	public void SetTileBox
			(
			Double			Width,
			Double			Height,
			Double			StepX,
			Double			StepY
			)
		{
		// by default XStep == Width
		AddToDictionary("/BBox", String.Format(NFI.DecSep, "[0 0 {0} {1}]", ToPt(Width), ToPt(Height)));
		AddToDictionary("/XStep", String.Format(NFI.DecSep, "{0}", ToPt(StepX)));
		AddToDictionary("/YStep", String.Format(NFI.DecSep, "{0}", ToPt(StepY)));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set Scale
	// warning: the program replaces the transformation matrix
	// with a new one [ScaleX 0 0 ScaleY 0 0]
	////////////////////////////////////////////////////////////////////

	public void SetScale
			(
			Double			Scale
			)
		{
		// add items to dictionary
		AddToDictionary("/Matrix", String.Format(NFI.DecSep, "[{0} 0 0 {0} 0 0]", Round(Scale)));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set Scale
	// warning: the program replaces the transformation matrix
	// with a new one [ScaleX 0 0 ScaleY 0 0]
	////////////////////////////////////////////////////////////////////

	public void SetScale
			(
			Double			ScaleX,
			Double			ScaleY
			)
		{
		// add items to dictionary
		AddToDictionary("/Matrix", String.Format(NFI.DecSep, "[{0} 0 0 {1} 0 0]", Round(ScaleX), Round(ScaleY)));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set Scale and origin
	// warning: the program replaces the transformation matrix
	// with a new one [ScaleX 0 0 ScaleY OrigX OrigY]
	////////////////////////////////////////////////////////////////////

	public void SetScaleAndOrigin
			(
			Double			OrigX,
			Double			OrigY,
			Double			ScaleX,
			Double			ScaleY
			)
		{
		// add items to dictionary
		AddToDictionary("/Matrix", String.Format(NFI.DecSep, "[{0} 0 0 {1} {2} {3}]", Round(ScaleX), Round(ScaleY), ToPt(OrigX), ToPt(OrigY)));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set pattern transformation matrix
	// Xpage = a * Xuser + c * Yuser + e
	// Ypage = b * Xuser + d * Yuser + f
	////////////////////////////////////////////////////////////////////
	
	public void SetPatternMatrix
			(
			Double		a,
			Double		b,
			Double		c,
			Double		d,
			Double		e,
			Double		f
			)
		{
		// create full pattern transformation matrix
		AddToDictionary("/Matrix", String.Format(NFI.DecSep, "[{0} {1} {2} {3} {4} {5}]",
			Round(a), Round(b), Round(c), Round(d), ToPt(e), ToPt(f)));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Create new PdfTilingPattern class with brick paatern
	// The pattern in a square with one user unit side.
	// The bottom half is one brick. The top half is two half bricks.
	// Arguments:
	// Scale the pattern to your requierments.
	// Stroking color is the mortar color.
	// Nonstrokink color is th brick color.
	////////////////////////////////////////////////////////////////////

	public static PdfTilingPattern SetBrickPattern
			(
			PdfDocument		Document,
			Double			Scale,
			Color			Stroking,
			Color			NonStroking
			)
		{
		PdfTilingPattern Pattern = new PdfTilingPattern(Document);
		Pattern.SetScale(Scale);
		Pattern.SaveGraphicsState();
		Pattern.SetLineWidth(0.05);
		Pattern.SetColorStroking(Stroking);
		Pattern.SetColorNonStroking(NonStroking);
		Pattern.DrawRectangle(0.025, 0.025, 0.95, 0.45, PaintOp.CloseFillStroke);
		Pattern.DrawRectangle(-0.475, 0.525, 0.95, 0.45, PaintOp.CloseFillStroke);
		Pattern.DrawRectangle(0.525, 0.525, 0.95, 0.45, PaintOp.CloseFillStroke);
		Pattern.RestoreGraphicsState();
		return(Pattern);
		}

	////////////////////////////////////////////////////////////////////
	// Create new PdfTilingPattern class with weave paatern
	// The pattern in a square with one user unit side.
	// It is made of horizontal and vertical rectangles.
	// Arguments:
	// Scale the pattern to your requierments.
	// Stroking color is the mortar color.
	// Nonstrokink color is th brick color.
	////////////////////////////////////////////////////////////////////

	public static PdfTilingPattern SetWeavePattern
			(
			PdfDocument		Document,
			Double			Scale,
			Color			Background,
			Color			Horizontal,
			Color			Vertical
			)
		{
		const Double RectSide1 = 4.0 / 6.0;
		const Double RectSide2 = 2.0 / 6.0;
		const Double LineWidth = 0.2 / 6.0;
		const Double HalfWidth = 0.5 * LineWidth;

		PdfTilingPattern Pattern = new PdfTilingPattern(Document);
		Pattern.SetScale(Scale);
		Pattern.SaveGraphicsState();
		Pattern.SetTileBox(1.0);
		Pattern.SetColorNonStroking(Color.DarkSlateBlue);
		Pattern.DrawRectangle(0.0, 0.0, 1.0, 1.0, PaintOp.Fill);
		Pattern.SetLineWidth(LineWidth);
		Pattern.SetColorStroking(Background);

		Pattern.SetColorNonStroking(Horizontal);
		Pattern.DrawRectangle(HalfWidth, 1.0 / 6.0 + HalfWidth, RectSide1 - LineWidth, RectSide2 - LineWidth, PaintOp.CloseFillStroke);
		Pattern.DrawRectangle(-(3.0 / 6.0 - HalfWidth), 4.0 / 6.0 + HalfWidth, RectSide1 - LineWidth, RectSide2 - LineWidth, PaintOp.CloseFillStroke);
		Pattern.DrawRectangle(3.0 / 6.0 + HalfWidth, 4.0 / 6.0 + HalfWidth, RectSide1 - LineWidth, RectSide2 - LineWidth, PaintOp.CloseFillStroke);

		Pattern.SetColorNonStroking(Vertical);
		Pattern.DrawRectangle(4.0 / 6.0 + HalfWidth, HalfWidth, RectSide2 - LineWidth, RectSide1 - LineWidth, PaintOp.CloseFillStroke);
		Pattern.DrawRectangle(1.0 / 6.0 + HalfWidth, -(3.0 / 6.0 - HalfWidth), RectSide2 - LineWidth, RectSide1 - LineWidth, PaintOp.CloseFillStroke);
		Pattern.DrawRectangle(1.0 / 6.0 + HalfWidth, 3.0 / 6.0 + HalfWidth, RectSide2 - LineWidth, RectSide1 - LineWidth, PaintOp.CloseFillStroke);

		Pattern.RestoreGraphicsState();
		return(Pattern);
		}
	}
}
