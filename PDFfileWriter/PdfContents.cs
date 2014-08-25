/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfContents
//	PDF contents indirect object. Support for page contents,
//  X Objects and Tilling Patterns.
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
using System.IO;
using System.Globalization;

namespace PdfFileWriter
{
/////////////////////////////////////////////////////////////////////
// PDF font style flags
/////////////////////////////////////////////////////////////////////

public enum DrawStyle
	{
	Normal = 0,
	Underline = 4,
	Strikeout = 8,
	Subscript = 16,
	Superscript = 32
	}

// path painting and clipping operators
public enum PaintOp
	{
	NoOperator,
	NoPaint,			// n
	Stroke,				// S
	CloseStroke,		// s
	Fill,				// f
	FillEor,			// f*
	FillStroke,			// B
	FillStrokeEorr,		// B*
	CloseFillStroke,	// b
	CloseFillStrokeEor,	// b*
	ClipPathWnr,		// h W n
	ClipPathEor,		// h W* n
	}

// line ends cap
public enum PdfLineCap
	{
	Butt,
	Round,
	Square,
	}

// line join type
public enum PdfLineJoin
	{
	Miter,
	Round,
	Bevel,
	}

// text rendering
public enum TextRendering
	{
	Fill,
	Stroke,
	FillStroke,
	Invisible,
	FillClip,
	StrokeClip,
	FillStrokeClip,
	Clip	
	}

// text justify
public enum TextJustify
	{
	Left,
	Center,
	Right
	}

// Bezier draw control
public enum BezierPointOne
	{
	Ignore,
	MoveTo,
	LineTo
	}

////////////////////////////////////////////////////////////////////
// PDF contents object
////////////////////////////////////////////////////////////////////

public class PdfContents : PdfObject
	{
	internal Boolean			PageContents;	// true for page contents, false for X objects and pattern
	internal List<PdfObject>	ResObjects;

	private static String[] PaintStr = new String[] {"", "n", "S", "s", "f", "f*", "B", "B*", "b", "b*", "h W n", "h W* n"};

	////////////////////////////////////////////////////////////////////
	// Constructor for page contents
	////////////////////////////////////////////////////////////////////

	public PdfContents
			(
			PdfPage		Page
			) : base(Page.Document, true)
		{
		// set page contents flag
		PageContents = true;

		// add contents to page's list of contents
		Page.AddContents(this);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor for page contents
	// It must be explicitly attached to a page object
	////////////////////////////////////////////////////////////////////

	public PdfContents
			(
			PdfDocument Document
			) : base(Document, true) {}

	////////////////////////////////////////////////////////////////////
	// Constructor for XObject or Pattern
	////////////////////////////////////////////////////////////////////

	internal PdfContents
			(
			PdfDocument	Document,
			String		ObjectType
			) : base(Document, true, ObjectType) {}

	////////////////////////////////////////////////////////////////////
	// Save graphics state
	////////////////////////////////////////////////////////////////////
	
	public void SaveGraphicsState()
		{
		ContentsString.Append("q\n");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Restore graphics state
	////////////////////////////////////////////////////////////////////
	
	public void RestoreGraphicsState()
		{
		ContentsString.Append("Q\n");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Paint operator string
	////////////////////////////////////////////////////////////////////
	
	public String PaintOpStr
			(
			PaintOp		PP
			)
		{
		// apply paint operator
		return(PaintStr[(Int32) PP]);
		}

	////////////////////////////////////////////////////////////////////
	// Paint path
	////////////////////////////////////////////////////////////////////
	
	public void SetPaintOp
			(
			PaintOp		PP
			)
		{
		// apply paint operator
		if(PP != PaintOp.NoOperator) ContentsString.AppendFormat("{0}\n", PaintStr[(Int32) PP]);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set line width for future path operations
	////////////////////////////////////////////////////////////////////
	
	public void SetLineWidth
			(
			Double		Width
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} w\n", ToPt(Width));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set line cap
	////////////////////////////////////////////////////////////////////
	
	public void SetLineCap
			(
			PdfLineCap		LineCap
			)
		{
		ContentsString.AppendFormat("{0} J\n", (Int32) LineCap);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set line join
	////////////////////////////////////////////////////////////////////
	
	public void SetLineJoin
			(
			PdfLineJoin	LineJoin
			)
		{
		ContentsString.AppendFormat("{0} j\n", (Int32) LineJoin);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set miter limit
	////////////////////////////////////////////////////////////////////
	
	public void SetMiterLimit
			(
			Double		MiterLimit		// default 10.0
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} M\n", Round(MiterLimit));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set line dash pattern
	////////////////////////////////////////////////////////////////////
	
	public void SetDashLine
			(
			Double[]	DashArray,		// default []
			Double		DashPhase		// default 0
			)
		{
		// restore default condition of solid line
		if(DashArray == null || DashArray.Length == 0)
			{
			ContentsString.Append("[] 0 d\n");
			}
		// dash line
		else
			{
			if((DashArray.Length % 2) == 1) throw new ApplicationException("Dash array odd numer of dashes");
			ContentsString.Append("[");
			foreach(Double Value in DashArray) ContentsString.AppendFormat(NFI.DecSep, "{0} ", ToPt(Value));
			ContentsString.AppendFormat(NFI.DecSep, "] {0} d\n", ToPt(DashPhase));
			}
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set gray level for non stroking (fill) operations
	////////////////////////////////////////////////////////////////////
	
	public void GrayLevelNonStroking
			(
			Double GrayLevel
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} g\n", Round(GrayLevel));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set gray level for stroking (outline) operations
	////////////////////////////////////////////////////////////////////
	
	public void GrayLevelStroking
			(
			Double GrayLevel
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} G\n", Round(GrayLevel));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set color for non stroking (fill) operations
	////////////////////////////////////////////////////////////////////
	
	public void SetColorNonStroking
			(
			Color	Paint
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} {2} rg\n", Round((Double) Paint.R / 255.0), Round((Double) Paint.G / 255.0), Round((Double) Paint.B / 255.0));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set color for stroking (outline) operations
	////////////////////////////////////////////////////////////////////
	
	public void SetColorStroking
			(
			Color	Paint
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} {2} RG\n", Round((Double) Paint.R / 255.0), Round((Double) Paint.G / 255.0), Round((Double) Paint.B / 255.0));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set pattern for non stroking (fill) operations
	////////////////////////////////////////////////////////////////////
	
	public void SetPatternNonStroking
			(
			PdfTilingPattern Pattern
			)
		{
		AddToUsedResources(Pattern);
		ContentsString.AppendFormat("/Pattern cs {0} scn\n", Pattern.ResourceCode);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set pattern for stroking (outline) operations
	////////////////////////////////////////////////////////////////////
	
	public void SetPatternStroking
			(
			PdfContents Pattern
			)
		{
		AddToUsedResources(Pattern);
		ContentsString.AppendFormat("/Pattern CS {0} SCN\n", Pattern.ResourceCode);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw axial shading pattern
	////////////////////////////////////////////////////////////////////
	
	public void DrawShading
			(
			PdfAxialShading Shading
			)
		{
		AddToUsedResources(Shading);
		ContentsString.AppendFormat("{0} sh\n", Shading.ResourceCode);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw radial shading pattern
	////////////////////////////////////////////////////////////////////
	
	public void DrawShading
			(
			PdfRadialShading Shading
			)
		{
		AddToUsedResources(Shading);
		ContentsString.AppendFormat("{0} sh\n", Shading.ResourceCode);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set current transformation matrix
	// Xpage = a * Xuser + c * Yuser + e
	// Ypage = b * Xuser + d * Yuser + f
	////////////////////////////////////////////////////////////////////
	
	public void SetTransMatrix
			(
			Double		a,	// ScaleX * Cos(Rotate)
			Double		b,	// ScaleX * Sin(Rotate)
			Double		c,	// ScaleY * (-Sin(Rotate))
			Double		d,	// ScaleY * Cos(Rotate)
			Double		e,
			Double		f
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} {2} {3} {4} {5} cm\n", Round(a), Round(b), Round(c), Round(d), ToPt(e), ToPt(f));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Translate origin
	////////////////////////////////////////////////////////////////////
	
	public void Translate
			(
			PointD	Orig
			)
		{
		Translate(Orig.X, Orig.Y);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Translate origin
	////////////////////////////////////////////////////////////////////
	
	public void Translate
			(
			Double		OrigX,
			Double		OrigY
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "1 0 0 1 {0} {1} cm\n", ToPt(OrigX), ToPt(OrigY));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Scale
	////////////////////////////////////////////////////////////////////
	
	public void Scale
			(
			Double		Scale
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} 0 0 {0} 0 0 cm\n", Round(Scale));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Translate and scale
	////////////////////////////////////////////////////////////////////
	
	public void TranslateScale
			(
			PointD		Orig,
			Double		Scale
			)
		{
		TranslateScale(Orig.X, Orig.Y, Scale);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Translate and scale
	////////////////////////////////////////////////////////////////////
	
	public void TranslateScale
			(
			Double		OrigX,
			Double		OrigY,
			Double		Scale
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{2} 0 0 {2} {0} {1} cm\n", ToPt(OrigX), ToPt(OrigY), Round(Scale));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Translate and scale
	////////////////////////////////////////////////////////////////////
	
	public void TranslateScale
			(
			PointD		Orig,
			Double		ScaleX,
			Double		ScaleY
			)
		{
		TranslateScale(Orig.X, Orig.Y, ScaleX, ScaleY);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Translate and scale
	////////////////////////////////////////////////////////////////////
	
	public void TranslateScale
			(
			Double		OrigX,
			Double		OrigY,
			Double		ScaleX,
			Double		ScaleY
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{2} 0 0 {3} {0} {1} cm\n", ToPt(OrigX), ToPt(OrigY), Round(ScaleX), Round(ScaleY));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Translate, scale and rotate
	////////////////////////////////////////////////////////////////////
	
	public void TranslateScaleRotate
			(
			PointD		Orig,
			Double		Scale,
			Double		Rotate		// degrees
			)
		{
		TranslateScaleRotate(Orig.X, Orig.Y, Scale, Rotate);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Translate, scale and rotate
	////////////////////////////////////////////////////////////////////
	
	public void TranslateScaleRotate
			(
			Double		OrigX,
			Double		OrigY,
			Double		Scale,
			Double		Rotate
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{2} {3} {4} {2} {0} {1} cm\n",
			ToPt(OrigX), ToPt(OrigY), Round(Scale * Math.Cos(Rotate)), Round(Scale * Math.Sin(Rotate)), Round(Scale * Math.Sin(-Rotate)));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Move current pointer to new position
	////////////////////////////////////////////////////////////////////
	
	public void MoveTo
			(
			PointD		Point
			)
		{
		MoveTo(Point.X, Point.Y);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Move current pointer to new position
	////////////////////////////////////////////////////////////////////
	
	public void MoveTo
			(
			Double		X,
			Double		Y
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} m\n", ToPt(X), ToPt(Y));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw line from last position to new position
	////////////////////////////////////////////////////////////////////
	
	public void LineTo
			(
			PointD		Point
			)
		{
		LineTo(Point.X, Point.Y);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw line from last position to new position
	////////////////////////////////////////////////////////////////////
	
	public void LineTo
			(
			Double		X,
			Double		Y
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} l\n", ToPt(X), ToPt(Y));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Bezier cubic path
	////////////////////////////////////////////////////////////////////
	
	public void DrawBezier
			(
			BezierD			B,
			BezierPointOne	Point1
			)
		{
		switch(Point1)
			{
			case BezierPointOne.MoveTo:
				MoveTo(B.P1.X, B.P1.Y);
				break;

			case BezierPointOne.LineTo:
				LineTo(B.P1.X, B.P1.Y);
				break;
			}

		DrawBezier(B.P2.X, B.P2.Y, B.P3.X, B.P3.Y, B.P4.X, B.P4.Y);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Bezier cubic path
	////////////////////////////////////////////////////////////////////
	
	public void DrawBezier
			(
			PointD		P1,
			PointD		P2,
			PointD		P3
			)
		{
		DrawBezier(P1.X, P1.Y, P2.X, P2.Y, P3.X, P3.Y);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Bezier cubic path
	////////////////////////////////////////////////////////////////////
	
	public void DrawBezier
			(
			Double		X1,
			Double		Y1,
			Double		X2,
			Double		Y2,
			Double		X3,
			Double		Y3
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} {2} {3} {4} {5} c\n",
			ToPt(X1), ToPt(Y1), ToPt(X2), ToPt(Y2), ToPt(X3), ToPt(Y3));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Bezier cubic path (P1 is the same as current point)
	////////////////////////////////////////////////////////////////////
	
	public void DrawBezierNoP1
			(
			PointD		P2,
			PointD		P3
			)
		{
		DrawBezierNoP1(P2.X, P2.Y, P3.X, P3.Y);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Bezier cubic path (P1 is the same as current point)
	////////////////////////////////////////////////////////////////////
	
	public void DrawBezierNoP1
			(
			Double		X2,
			Double		Y2,
			Double		X3,
			Double		Y3
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} {2} {3} v\n", ToPt(X2), ToPt(Y2), ToPt(X3), ToPt(Y3));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Bezier cubic path (P2 is the same as P3)
	////////////////////////////////////////////////////////////////////
	
	public void DrawBezierNoP2
			(
			PointD		P1,
			PointD		P3
			)
		{
		DrawBezierNoP1(P1.X, P1.Y, P3.X, P3.Y);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Bezier cubic path (P2 is the same as P3)
	////////////////////////////////////////////////////////////////////
	
	public void DrawBezierNoP2
			(
			Double		X1,
			Double		Y1,
			Double		X3,
			Double		Y3
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} {2} {3} y\n", ToPt(X1), ToPt(Y1), ToPt(X3), ToPt(Y3));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Line
	////////////////////////////////////////////////////////////////////
	
	public void DrawLine
			(
			LineD		Line
			)
		{
		DrawLine(Line.P1.X, Line.P1.Y, Line.P2.X, Line.P2.Y);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Line
	////////////////////////////////////////////////////////////////////
	
	public void DrawLine
			(
			PointD		P1,
			PointD		P2
			)
		{
		DrawLine(P1.X, P1.Y, P2.X, P2.Y);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Line
	////////////////////////////////////////////////////////////////////
	
	public void DrawLine
			(
			Double		X1,
			Double		Y1,
			Double		X2,
			Double		Y2
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} m {2} {3} l S\n", ToPt(X1), ToPt(Y1), ToPt(X2), ToPt(Y2));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Line
	////////////////////////////////////////////////////////////////////
	
	public void DrawLine
			(
			LineD		Line,
			Double		LineWidth
			)
		{
		DrawLine(Line.P1.X, Line.P1.Y, Line.P2.X, Line.P2.Y, LineWidth);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Line
	////////////////////////////////////////////////////////////////////
	
	public void DrawLine
			(
			PointD		P1,
			PointD		P2,
			Double		LineWidth
			)
		{
		DrawLine(P1.X, P1.Y, P2.X, P2.Y, LineWidth);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Line
	////////////////////////////////////////////////////////////////////
	
	public void DrawLine
			(
			Double		X1,
			Double		Y1,
			Double		X2,
			Double		Y2,
			Double		LineWidth
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "q {0} w {1} {2} m {3} {4} l S Q\n", ToPt(LineWidth), ToPt(X1), ToPt(Y1), ToPt(X2), ToPt(Y2));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Rectangle
	////////////////////////////////////////////////////////////////////
	
	public void DrawRectangle
			(
			PointD		Point,
			SizeD		Size,
			PaintOp		PP
			)
		{
		DrawRectangle(Point.X, Point.Y, Size.Width, Size.Height, PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Rectangle
	////////////////////////////////////////////////////////////////////
	
	public void DrawRectangle
			(
			Double		X,
			Double		Y,
			Double		Width,
			Double		Height,
			PaintOp		PP
			)
		{
		// draw rectangle
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} {2} {3} re {4}\n", ToPt(X), ToPt(Y), ToPt(Width), ToPt(Height), PaintOpStr(PP));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw oval
	////////////////////////////////////////////////////////////////////
	
	public void DrawOval
			(
			PointD		Point,
			SizeD		Size,
			PaintOp		PP
			)
		{
		DrawOval(Point.X, Point.Y, Size.Width, Size.Height, PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw oval
	////////////////////////////////////////////////////////////////////
	
	public void DrawOval
			(
			Double		X,
			Double		Y,
			Double		Width,
			Double		Height,
			PaintOp		PP
			)
		{
		Width /= 2;
		Height /= 2;
		X += Width;
		Y += Height;
		DrawBezier(BezierD.OvalFirstQuarter(X, Y, Width, Height), BezierPointOne.MoveTo);
		DrawBezier(BezierD.OvalSecondQuarter(X, Y, Width, Height), BezierPointOne.Ignore);
		DrawBezier(BezierD.OvalThirdQuarter(X, Y, Width, Height), BezierPointOne.Ignore);
		DrawBezier(BezierD.OvalFourthQuarter(X, Y, Width, Height), BezierPointOne.Ignore);
		SetPaintOp(PP);
 		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Heart
	////////////////////////////////////////////////////////////////////
	
	public void DrawHeart
			(
			LineD		CenterLine,
			PaintOp		PP
			)
		{
		// 120 deg and 90 deg
		DrawDoubleBezierPath(CenterLine, 1.0, Math.PI / 1.5, 1.0, 0.5 * Math.PI, PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Heart
	////////////////////////////////////////////////////////////////////
	
	public void DrawHeart
			(
			Double		X1,
			Double		Y1,
			Double		X2,
			Double		Y2,
			PaintOp		PP
			)
		{
		DrawHeart(new LineD(X1, Y1, X2, Y2), PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw double Bezier path
	////////////////////////////////////////////////////////////////////
	
	public void DrawDoubleBezierPath
			(
			LineD		CenterLine,
			Double		Factor1,
			Double		Alpha1,
			Double		Factor2,
			Double		Alpha2,
			PaintOp		PP
			)
		{
		// two symmetric Bezier curves
		DrawBezier(new BezierD(CenterLine.P1, Factor1, -0.5 * Alpha1, Factor2, -0.5 * Alpha2, CenterLine.P2), BezierPointOne.MoveTo);
		DrawBezier(new BezierD(CenterLine.P2, Factor2, Math.PI + 0.5 * Alpha2, Factor1, Math.PI + 0.5 * Alpha1, CenterLine.P1), BezierPointOne.Ignore);

		// set paint operator
		SetPaintOp(PP);
 		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Rounded Rectangle
	////////////////////////////////////////////////////////////////////
	
	public void DrawRoundedRectangle
			(
			PointD		Orig,
			SizeD		Size,
			Double		Rad,
			PaintOp		PP
			)
		{
		DrawRoundedRectangle(Orig.X, Orig.Y, Size.Width, Size.Height, Rad, PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Rounded Rectangle
	////////////////////////////////////////////////////////////////////
	
	public void DrawRoundedRectangle
			(
			Double		OrigX,
			Double		OrigY,
			Double		Width,
			Double		Height,
			Double		Rad,
			PaintOp		PP
			)
		{
		// make sure radius is not too big
		if(Rad > 0.5 * Width) Rad = 0.5 * Width;
		if(Rad > 0.5 * Height) Rad = 0.5 * Height;

		// draw path
		MoveTo(OrigX + Rad, OrigY);
		DrawBezier(BezierD.CircleFourthQuarter(OrigX + Width - Rad, OrigY + Rad, Rad), BezierPointOne.LineTo);
		DrawBezier(BezierD.CircleFirstQuarter(OrigX + Width - Rad, OrigY + Height - Rad, Rad), BezierPointOne.LineTo);
		DrawBezier(BezierD.CircleSecondQuarter(OrigX + Rad, OrigY + Height - Rad, Rad), BezierPointOne.LineTo);
		DrawBezier(BezierD.CircleThirdQuarter(OrigX + Rad, OrigY + Rad, Rad), BezierPointOne.LineTo);

		// set paint operator
		SetPaintOp(PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Rectangle with Inward Corners
	////////////////////////////////////////////////////////////////////
	
	public void DrawInwardCornerRectangle
			(
			Double		OrigX,
			Double		OrigY,
			Double		Width,
			Double		Height,
			Double		Rad,
			PaintOp		PP
			)
		{
		// make sure radius is not too big
		if(Rad > 0.5 * Width) Rad = 0.5 * Width;
		if(Rad > 0.5 * Height) Rad = 0.5 * Height;

		// draw path
		MoveTo(OrigX, OrigY + Rad);
		DrawBezier(BezierD.CircleFourthQuarter(OrigX, OrigY + Height, Rad), BezierPointOne.LineTo);
		DrawBezier(BezierD.CircleThirdQuarter(OrigX + Width, OrigY + Height, Rad), BezierPointOne.LineTo);
		DrawBezier(BezierD.CircleSecondQuarter(OrigX + Width, OrigY, Rad), BezierPointOne.LineTo);
		DrawBezier(BezierD.CircleFirstQuarter(OrigX, OrigY, Rad), BezierPointOne.LineTo);

		// set paint operator
		SetPaintOp(PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw polygon
	////////////////////////////////////////////////////////////////////
	
	public void DrawPolygon
			(
			PointF[]	PathArray,
			PaintOp		PP
			)
		{
		// program error
		if(PathArray.Length < 2) throw new ApplicationException("Draw polygon error: path array must have at least two points");

		// move to first point
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} m\n", ToPt(PathArray[0].X), ToPt(PathArray[0].Y));

		// draw lines		
		for(Int32 Index = 1; Index < PathArray.Length; Index++)
			{
			ContentsString.AppendFormat(NFI.DecSep, "{0} {1} l\n", ToPt(PathArray[Index].X), ToPt(PathArray[Index].Y));
			}

		// set paint operator
		SetPaintOp(PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw polygon
	////////////////////////////////////////////////////////////////////
	
	public void DrawPolygon
			(
			Single[]	PathArray,	// pairs of x and y values
			PaintOp		PP
			)
		{
		// program error
		if(PathArray.Length < 4) throw new ApplicationException("Draw polygon error: path array must have at least 4 items");

		// program error
		if((PathArray.Length & 1) != 0) throw new ApplicationException("Draw polygon error: path array must have even number of items");

		// move to first point
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} m\n", ToPt(PathArray[0]), ToPt(PathArray[1]));

		// draw lines		
		for(Int32 Index = 2; Index < PathArray.Length; Index += 2)
			{
			ContentsString.AppendFormat(NFI.DecSep, "{0} {1} l\n", ToPt(PathArray[Index]), ToPt(PathArray[Index + 1]));
			}

		// set paint operator
		SetPaintOp(PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw regular polygon
	////////////////////////////////////////////////////////////////////
	
	public void DrawRegularPolygon
			(
			Double		CenterX,
			Double		CenterY,
			Double		Radius,
			Double		Alpha,
			Int32		Sides,
			PaintOp		PP
			)
		{
		DrawRegularPolygon(new PointD(CenterX, CenterY), Radius, Alpha, Sides, PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw regular polygon
	////////////////////////////////////////////////////////////////////
	
	public void DrawRegularPolygon
			(
			PointD		Center,
			Double		Radius,
			Double		Alpha,
			Int32		Sides,
			PaintOp		PP
			)
		{
		// validate sides
		if(Sides < 3) throw new ApplicationException("Draw regular polygon. Number of sides must be 3 or more");

		// polygon angle
		Double DeltaAlpha = 2.0 * Math.PI / Sides;

		// first corner coordinates
		MoveTo(new PointD(Center, Radius, Alpha));

		for(Int32 Side = 1; Side < Sides; Side++)
			{
			Alpha += DeltaAlpha;
			LineTo(new PointD(Center, Radius, Alpha));
			}

		// set paint operator
		SetPaintOp(PP);
 		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw star
	////////////////////////////////////////////////////////////////////
	
	public void DrawStar
			(
			Double		CenterX,
			Double		CenterY,
			Double		Radius,
			Double		Alpha,
			Int32		Sides,
			PaintOp		PP
			)
		{
		DrawStar(new PointD(CenterX, CenterY), Radius, Alpha, Sides, PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw star
	////////////////////////////////////////////////////////////////////
	
	public void DrawStar
			(
			PointD		Center,
			Double		Radius,
			Double		Alpha,
			Int32		Sides,
			PaintOp		PP
			)
		{
		// inner radius
		Double Radius1 = 0;

		// for polygon with less than 5, set inner radius to half the main radius
		if(Sides < 5)
			{
			Radius1 = 0.5 * Radius;
			}

		// for polygons with 5 sides, calculate inner radius
		else
			{
			// polygon angle
			Double DeltaAlpha = 2.0 * Math.PI / Sides;

			// first line
			LineD L1 = new LineD(new PointD(Center, Radius, Alpha), new PointD(Center, Radius, Alpha + 2.0 * DeltaAlpha));

			// second line
			LineD L2 = new LineD(new PointD(Center, Radius, Alpha - DeltaAlpha), new PointD(Center, Radius, Alpha + DeltaAlpha));

			// inner radius
			Radius1 = (new PointD(L1, L2)).Distance(Center);
			}

		// draw star
		DrawStar(Center, Radius, Radius1, Alpha, Sides, PP);
 		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw star
	////////////////////////////////////////////////////////////////////
	
	public void DrawStar
			(
			Double		CenterX,
			Double		CenterY,
			Double		Radius1,
			Double		Radius2,
			Double		Alpha,
			Int32		Sides,
			PaintOp		PP
			)
		{
		DrawStar(new PointD(CenterX, CenterY), Radius1, Radius2, Alpha, Sides, PP);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw star
	////////////////////////////////////////////////////////////////////
	
	public void DrawStar
			(
			PointD		Center,
			Double		Radius1,
			Double		Radius2,
			Double		Alpha,
			Int32		Sides,
			PaintOp		PP
			)
		{
		// validate sides
		if(Sides < 3) throw new ApplicationException("Draw star. Number of sides must be 3 or more");

		// move to first point
		MoveTo(new PointD(Center, Radius1, Alpha));

		// increment angle
		Double DeltaAlpha = Math.PI / Sides;

		// double number of sides
		Sides *= 2;

		// line to the rest of the points
		for(Int32 Side = 1; Side < Sides; Side++)
			{
			Alpha += DeltaAlpha;
			LineTo(new PointD(Center, (Side & 1) != 0 ? Radius2 : Radius1, Alpha));
			}

		// set paint operator
		SetPaintOp(PP);
 		return;
		}

	////////////////////////////////////////////////////////////////////
	// Begin text mode
	////////////////////////////////////////////////////////////////////
	
	public void BeginTextMode()
		{
		ContentsString.Append("BT\n");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// End text mode
	////////////////////////////////////////////////////////////////////
	
	public void EndTextMode()
		{
		ContentsString.Append("ET\n");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set text position
	////////////////////////////////////////////////////////////////////

	public void SetTextPosition
			(
			Double		PosX,
			Double		PosY
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} Td\n", ToPt(PosX), ToPt(PosY));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set text rendering mode
	////////////////////////////////////////////////////////////////////

	public void SetTextRenderingMode
			(
			TextRendering	TR
			)
		{
		ContentsString.AppendFormat("{0} Tr\n", (Int32) TR);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set character spacing
	////////////////////////////////////////////////////////////////////

	public void SetCharacterSpacing
			(
			Double	Spacing
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} Tc\n", ToPt(Spacing));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set word spacing
	////////////////////////////////////////////////////////////////////

	public void SetWordSpacing
			(
			Double	Spacing
			)
		{
		ContentsString.AppendFormat(NFI.DecSep, "{0} Tw\n", ToPt(Spacing));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw text
	////////////////////////////////////////////////////////////////////

	public Double DrawText
			(
			PdfFont		Font,
			Double		FontSize,		// in points
			String		Text
			)
		{
		// text is null or empty
		if(String.IsNullOrEmpty(Text)) return(0);

		// add font code to current list of font codes
		AddToUsedResources(Font);

		// draw text
		ContentsString.AppendFormat(NFI.DecSep, "{0} {1} Tf{2} Tj\n", Font.ResourceCode, Round(FontSize), Font.PdfText(Text));
		return(Font.TextWidth(FontSize, Text));
		}

	////////////////////////////////////////////////////////////////////
	// Draw Text
	////////////////////////////////////////////////////////////////////

	public Double DrawText
			(
			PdfFont		Font,
			Double		FontSize,		// in points
			Double		PosX,
			Double		PosY,
			String		Text
			)
		{
		return(DrawText(Font, FontSize, PosX, PosY, TextJustify.Left, Text));
		}

	////////////////////////////////////////////////////////////////////
	// Draw text width draw style
	////////////////////////////////////////////////////////////////////

	public Double DrawText
			(
			PdfFont		Font,
			Double		FontSize,		// in points
			Double		PosX,
			Double		PosY,
			DrawStyle	DrawStyle,
			String		Text
			)
		{
		return(DrawText(Font, FontSize, PosX, PosY, TextJustify.Left, DrawStyle, Color.Empty, Text));
		}

	////////////////////////////////////////////////////////////////////
	// Draw text with color
	////////////////////////////////////////////////////////////////////

	public Double DrawText
			(
			PdfFont		Font,
			Double		FontSize,		// in points
			Double		PosX,
			Double		PosY,
			Color		FontColor,
			String		Text
			)
		{
		return(DrawText(Font, FontSize, PosX, PosY, TextJustify.Left, DrawStyle.Normal, FontColor, Text));
		}

	////////////////////////////////////////////////////////////////////
	// Draw Text
	////////////////////////////////////////////////////////////////////

	public Double DrawText
			(
			PdfFont		Font,
			Double		FontSize,		// in points
			Double		PosX,
			Double		PosY,
			TextJustify	Justify,
			String		Text
			)
		{
		// text is null or empty
		if(String.IsNullOrEmpty(Text)) return(0);

		// add font code to current list of font codes
		AddToUsedResources(Font);

		// text width
		Double Width = Font.TextWidth(FontSize, Text);

		// adjust position
		switch(Justify)
			{
			// right
			case TextJustify.Right:
				PosX -= Width;
				break;

			// center
			case TextJustify.Center:
				PosX -= 0.5 * Width;
				break;
			}

		// draw text
		ContentsString.AppendFormat(NFI.DecSep, "BT{0} {1} Tf {2} {3} Td{4}Tj ET\n",
			Font.ResourceCode, Round(FontSize), ToPt(PosX), ToPt(PosY), Font.PdfText(Text));

		// return text width
		return(Width);
		}

	////////////////////////////////////////////////////////////////////
	// Draw text width draw style
	////////////////////////////////////////////////////////////////////

	public Double DrawText
			(
			PdfFont		Font,
			Double		FontSize,		// in points
			Double		PosX,
			Double		PosY,
			TextJustify	Justify,
			DrawStyle	DrawStyle,
			Color		TextColor,
			String		Text
			)
		{
		// text is null or empty
		if(String.IsNullOrEmpty(Text)) return(0);

		// text width
		Double TextWidth = 0;

		// we have color
		if(TextColor != Color.Empty)
			{
			// save graphics state
			SaveGraphicsState();

			// change non stroking color
			SetColorNonStroking(TextColor);
			}

		// not subscript or superscript
		if((DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == 0)
			{
			// draw text string
			TextWidth = DrawText(Font, FontSize, PosX, PosY, Justify, Text);

			// not regular style
			if(DrawStyle != DrawStyle.Normal)
				{
				// change stroking color
				if(TextColor != Color.Empty) SetColorStroking(TextColor);

				// underline
				if((DrawStyle & DrawStyle.Underline) != 0)
					{
					Double UnderlinePos = PosY + Font.UnderlinePosition(FontSize);
					DrawLine(PosX, UnderlinePos, PosX + TextWidth, UnderlinePos, Font.UnderlineWidth(FontSize));
					}

				// strikeout
				if((DrawStyle & DrawStyle.Strikeout) != 0)
					{
					Double StrikeoutPos = PosY + Font.StrikeoutPosition(FontSize);
					DrawLine(PosX, StrikeoutPos, PosX + TextWidth, StrikeoutPos, Font.StrikeoutWidth(FontSize));
					}
				}
			}

		// subscript or superscript
		else
			{
			// subscript
			if((DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == DrawStyle.Subscript)
				{
				// subscript font size and location
				PosY -= Font.SubscriptPosition(FontSize);
				FontSize = Font.SubscriptSize(FontSize);

				// draw text string
				TextWidth = DrawText(Font, FontSize, PosX, PosY, Justify, Text);
				}

			// superscript
			if((DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == DrawStyle.Superscript)
				{
				// superscript font size and location
				PosY += Font.SuperscriptPosition(FontSize);
				FontSize = Font.SuperscriptSize(FontSize);

				// draw text string
				TextWidth = DrawText(Font, FontSize, PosX, PosY, Justify, Text);
				}
			}

		// we have color
		if(TextColor != Color.Empty)
			{
			// save graphics state
			RestoreGraphicsState();
			}

		// return text width
		return(TextWidth);
		}

	////////////////////////////////////////////////////////////////////
	// Draw Text
	////////////////////////////////////////////////////////////////////

	public Double DrawText
			(
			PdfFont			Font,
			Double			FontSize,		// in points
			Double			PosX,
			Double			PosY,
			KerningAdjust[]	KerningArray
			)
		{
		// text is null or empty
		if(KerningArray == null || KerningArray.Length == 0) return(0);

		// add font code to current list of font codes
		AddToUsedResources(Font);

		// total width
		Double Width = 0;

		// draw text
		ContentsString.AppendFormat(NFI.DecSep, "BT{0} {1} Tf {2} {3} Td[", Font.ResourceCode, Round(FontSize), ToPt(PosX), ToPt(PosY));
		Int32 LastStr = KerningArray.Length - 1;
		for(Int32 Index = 0; Index < LastStr; Index++)
			{
			KerningAdjust KA = KerningArray[Index];
			ContentsString.AppendFormat(NFI.DecSep, "{0}{1}", Font.PdfText(KA.Text), (Single) (1000.0 * (-KA.Adjust)));
			Width += Font.TextWidth(FontSize, KA.Text) + Font.FontUnitsToUserUnits(FontSize, KA.Adjust);
			}

		// last string
		ContentsString.AppendFormat("{0}]TJ ET\n", Font.PdfText(KerningArray[LastStr].Text));
		Width += Font.TextWidth(FontSize, KerningArray[LastStr].Text);
		return(Width);
		}

	////////////////////////////////////////////////////////////////////
	// Draw text with kerning
	////////////////////////////////////////////////////////////////////

	public Double DrawTextWithKerning
			(
			PdfFont		Font,
			Double		FontSize,		// in points
			Double		PosX,
			Double		PosY,
			String		Text
			)
		{
		// text is null or empty
		if(String.IsNullOrEmpty(Text)) return(0);

		// create text position adjustment array based on kerning information
		KerningAdjust[] KernArray = Font.TextKerning(Text);

		// no kerning
		if(KernArray == null) return(DrawText(Font, FontSize, PosX, PosY, Text));

		// draw text with adjustment
		return(DrawText(Font, FontSize, PosX, PosY, KernArray));
		}

	////////////////////////////////////////////////////////////////////
	// Draw special effects Text
	////////////////////////////////////////////////////////////////////

	public Double DrawText
			(
			PdfFont		Font,
			Double		FontSize,		// in points
			Double		PosX,
			Double		PosY,
			TextJustify	Justify,
			Double		OutlineWidth,	// in user units
			Color		StrokingColor,
			Color		NonStokingColor,
			String		Text
			)
		{
		// text is null or empty
		if(String.IsNullOrEmpty(Text)) return(0);

		// add font code to current list of font codes
		AddToUsedResources(Font);

		// save graphics state
		SaveGraphicsState();

		// create text position adjustment array based on kerning information
		KerningAdjust[] KernArray = Font.TextKerning(Text);

		// text width
		Double Width = KernArray == null ? Font.TextWidth(FontSize, Text) : Font.TextKerningWidth(FontSize, KernArray);

		// adjust position
		switch(Justify)
			{
			// right
			case TextJustify.Right:
				PosX -= Width;
				break;

			// center
			case TextJustify.Center:
				PosX -= 0.5 * Width;
				break;
			}

		// special effects
		TextRendering TR = TextRendering.Fill;
		if(!StrokingColor.IsEmpty)
			{
			SetLineWidth(OutlineWidth);
			SetColorStroking(StrokingColor);
			TR = TextRendering.Stroke;
			}
		if(!NonStokingColor.IsEmpty)
			{
			SetColorNonStroking(NonStokingColor);
			TR = StrokingColor.IsEmpty ? TextRendering.Fill : TextRendering.FillStroke;
			}

		// draw text initialization
		ContentsString.AppendFormat(NFI.DecSep, "BT {0} {1} Td{2} {3} Tf {4} Tr ", ToPt(PosX), ToPt(PosY), Font.ResourceCode, Round(FontSize), (Int32) TR);

		// draw text
		if(KernArray == null)
			{
			ContentsString.AppendFormat("{0} Tj ET\n", Font.PdfText(Text));
			}

		// draw text with kerning
		else
			{
			ContentsString.Append("[");
			Int32 LastStr = KernArray.Length - 1;
			for(Int32 Index = 0; Index < LastStr; Index++)
				{
				KerningAdjust KA = KernArray[Index];
				ContentsString.AppendFormat(NFI.DecSep, "{0}{1}", Font.PdfText(KA.Text), (Single) (1000.0 * (-KA.Adjust)));
				}

			// last string
			ContentsString.AppendFormat("{0}]TJ ET\n", Font.PdfText(KernArray[LastStr].Text));
			}

		// restore graphics state
		RestoreGraphicsState();

		// exit
		return(Width);
		}

	////////////////////////////////////////////////////////////////////
	// Draw Text
	////////////////////////////////////////////////////////////////////

	public Int32 DrawText
			(
			Double			PosX,
			ref Double		PosYTop,
			Double			PosYBottom,
			Int32			LineNo,
			TextBox			Box
			)
		{
		return(DrawText(PosX, ref PosYTop, PosYBottom, LineNo, 0.0, 0.0, false, Box));
		}

	////////////////////////////////////////////////////////////////////
	// Draw Text
	////////////////////////////////////////////////////////////////////

	public Int32 DrawText
			(
			Double			PosX,
			ref Double		PosYTop,
			Double			PosYBottom,
			Int32			LineNo,
			Double			LineExtraSpace,
			Double			ParagraphExtraSpace,
			Boolean			FitTextToWidth,
			TextBox			Box
			)
		{
		Box.Terminate();
		for(; LineNo < Box.LineCount; LineNo++)
			{
			// short cut
			TextLine Line = Box[LineNo];

			// break out of the loop if printing below bottom line
			if(PosYTop - Line.LineHeight < PosYBottom) break;

			// adjust PosY to font base line
			PosYTop -= Line.Ascent;

			// text horizontal position
			Double X = PosX;
			Double W = Box.BoxWidth;

			// if we have first line indent, adjust text x position for first line of a paragraph
			if(Box.FirstLineIndent != 0 && (LineNo == 0 || Box[LineNo - 1].EndOfParagraph))
				{
				X += Box.FirstLineIndent;
				W -= Box.FirstLineIndent;
				}

			// draw text normal
			if(!FitTextToWidth || Line.EndOfParagraph)
				{
				DrawText(X, PosYTop, Line);
				}

			// draw text to fit box width
			else
				{
				DrawText(X, PosYTop, W, Line);
				}

			// advance position y to next line
			PosYTop -= Line.Descent + LineExtraSpace;
			if(Line.EndOfParagraph) PosYTop -= ParagraphExtraSpace;
			}
		return(LineNo);
		}

	////////////////////////////////////////////////////////////////////
	// Draw text width draw style
	////////////////////////////////////////////////////////////////////

	public Double DrawText
			(
			Double		PosX,
			Double		PosY,
			TextLine	Line
			)
		{
		Double SegPosX = PosX;
		foreach(TextSeg Seg in Line.SegArray) SegPosX += DrawText(Seg.Font, Seg.FontSize, SegPosX, PosY, TextJustify.Left, Seg.DrawStyle, Seg.FontColor, Seg.Text);
		return(SegPosX - PosX);
		}

	////////////////////////////////////////////////////////////////////
	// Draw text width draw style
	////////////////////////////////////////////////////////////////////

	public Double DrawText
			(
			Double		PosX,
			Double		PosY,
			Double		Width,
			TextLine	Line
			)
		{
		Double WordSpacing;
		Double CharSpacing;
		if(!TextFitToWidth(Width, out WordSpacing, out CharSpacing, Line)) return(DrawText(PosX, PosY, Line));
		SaveGraphicsState();
		SetWordSpacing(WordSpacing);
		SetCharacterSpacing(CharSpacing);

		Double SegPosX = PosX;
		foreach(TextSeg Seg in Line.SegArray)
			{
			SegPosX += DrawText(Seg.Font, Seg.FontSize, SegPosX, PosY, TextJustify.Left, Seg.DrawStyle, Seg.FontColor, Seg.Text) + Seg.SpaceCount * WordSpacing + Seg.Text.Length * CharSpacing;
			}
		RestoreGraphicsState();
		return(SegPosX - PosX);
		}

	////////////////////////////////////////////////////////////////////
	// Stretch text to given width
	////////////////////////////////////////////////////////////////////

	public Boolean TextFitToWidth
			(
			Double		ReqWidth,
			out Double	WordSpacing,
			out Double	CharSpacing,
			TextLine	Line
			)
		{
		WordSpacing = 0;
		CharSpacing = 0;

		Int32 CharCount = 0;
		Double Width = 0;
		Int32 SpaceCount = 0;
		Double SpaceWidth = 0;
		foreach(TextSeg Seg in Line.SegArray)
			{
			// accumulate line width
			CharCount += Seg.Text.Length;
			Width += Seg.SegWidth;

			// count spaces
			SpaceCount += Seg.SpaceCount;

			// accumulate space width
			SpaceWidth += Seg.SpaceCount * Seg.Font.CharWidth(Seg.FontSize, ' ');
			}

		// reduce character count by one
		CharCount--;
		if(CharCount <= 0) return(false);

		// extra spacing required
		Double ExtraSpace = ReqWidth - Width;

		// highest possible output device resolution (12000 dots per inch)
		Double MaxRes = 0.006 / ScaleFactor;

		// string is too wide
		if(ExtraSpace < (-MaxRes)) return(false);

		// string is just right
		if(ExtraSpace < MaxRes) return(true);

		// String does not have any blank characters
		if(SpaceCount == 0)
			{
			CharSpacing = ExtraSpace / CharCount;
			return(true);
			}

		// extra space per word
		WordSpacing = ExtraSpace / SpaceCount;

		// extra space is equal or less than one blank
		if(WordSpacing <= SpaceWidth / SpaceCount) return(true);

		// extra space is larger that one blank
		// increase character and word spacing
		CharSpacing = ExtraSpace / (10 * SpaceCount + CharCount);
		WordSpacing = 10 * CharSpacing;
		return(true);
		}

	////////////////////////////////////////////////////////////////////
	// Clip Text
	////////////////////////////////////////////////////////////////////

	public void ClipText
			(
			PdfFont		Font,
			Double		FontSize,		// in points
			Double		PosX,
			Double		PosY,
			String		Text
			)
		{
		// text is null or empty
		if(String.IsNullOrEmpty(Text)) return;

		// add font code to current list of font codes
		AddToUsedResources(Font);

		// draw text
		ContentsString.AppendFormat(NFI.DecSep, "BT{0} {1} Tf {2} {3} Td 7 Tr{4}Tj ET\n",
			Font.ResourceCode, Round(FontSize), ToPt(PosX), ToPt(PosY), Font.PdfText(Text));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Image
	////////////////////////////////////////////////////////////////////
	
	public void DrawImage
			(
			PdfImage	Image,
			Double		OrigX,
			Double		OrigY,
			Double		Width,
			Double		Height
			)
		{
		// add image code to current list of resources
		AddToUsedResources(Image);

		// draw text
		ContentsString.AppendFormat(NFI.DecSep, "q {0} 0 0 {1} {2} {3} cm {4} Do Q\n",
			ToPt(Width), ToPt(Height), ToPt(OrigX), ToPt(OrigY), Image.ResourceCode);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw XObject
	////////////////////////////////////////////////////////////////////
	
	public void DrawXObject
			(
			PdfXObject	XObject
			)
		{
		// add image code to current list of resources
		AddToUsedResources(XObject);

		// draw object
		ContentsString.AppendFormat("{0} Do\n", XObject.ResourceCode);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw XObject
	////////////////////////////////////////////////////////////////////
	
	public void DrawXObject
			(
			Double		OrigX,
			Double		OrigY,
			PdfXObject	XObject
			)
		{
		SaveGraphicsState();
		Translate(OrigX, OrigY);
		DrawXObject(XObject);
		RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw XObject
	////////////////////////////////////////////////////////////////////
	
	public void DrawXObject
			(
			PdfXObject	XObject,
			Double		OrigX,
			Double		OrigY,
			Double		ScaleX,
			Double		ScaleY
			)
		{
		// add image code to current list of resources
		AddToUsedResources(XObject);

		// draw object
		ContentsString.AppendFormat(NFI.DecSep, "q {0} 0 0 {1} {2} {3} cm {4} Do Q",
			Round(ScaleX), Round(ScaleY), ToPt(OrigX), ToPt(OrigY), XObject.ResourceCode);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw XObject
	////////////////////////////////////////////////////////////////////
	
	public void DrawXObject
			(
			PdfXObject	XObject,
			Double		OrigX,
			Double		OrigY,
			Double		ScaleX,
			Double		ScaleY,
			Double		Alpha
			)
		{
		// add image code to current list of resources
		AddToUsedResources(XObject);

		// draw object
		Double Sin = Math.Sin(Alpha);
		Double Cos = Math.Cos(Alpha);
		ContentsString.AppendFormat(NFI.DecSep, "q {0} {1} {2} {3} {4} {5} cm {6} Do Q",
			Round(ScaleX), Round(ScaleY), ToPt(OrigX), ToPt(OrigY), XObject.ResourceCode);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Add resource to list of used resources
	////////////////////////////////////////////////////////////////////

	private void AddToUsedResources
			(
			PdfObject	ResObject
			)
		{
		if(ResObjects == null) ResObjects = new List<PdfObject>();
		Int32 Index = ResObjects.BinarySearch(ResObject);
		if(Index < 0) ResObjects.Insert(~Index, ResObject);
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
		// build resource dictionary for non page contents
		if(!PageContents) AddToDictionary("/Resources", BuildResourcesDictionary(ResObjects, false));

		// call PdfObject routine
		base.WriteObjectToPdfFile(PdfFile);

		// exit
		return;
		}
	}
}
