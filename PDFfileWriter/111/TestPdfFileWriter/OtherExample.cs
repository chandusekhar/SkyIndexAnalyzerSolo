/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	OtherExample
//	Produce PDF document when Other Example button is clicked.
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
using System.Diagnostics;
using System.Drawing;
using PdfFileWriter;

namespace TestPdfFileWriter
{
public class OtherExample
	{
	private const Double DegToRad = Math.PI / 180.0;
	private const Double PageWidth = 8.5;
	private const Double PageHeight = 11.0;
	private const Double Margin = 0.75;
	private const Double HeadingHeight = 0.5;
	private const Double DispWidth = PageWidth - 2 * Margin;
	private const Double DispHeight = PageHeight - 2 * Margin - HeadingHeight;
	private const Double AreaWidth = DispWidth / 2;
	private const Double AreaHeight = DispHeight / 3;
	private const Double AreaX1 = Margin;
	private const Double AreaX2 = Margin + AreaWidth;
	private const Double AreaX3 = Margin + 2 * AreaWidth;
	private const Double AreaY1 = Margin;
	private const Double AreaY2 = Margin + AreaHeight;
	private const Double AreaY3 = Margin + 2 * AreaHeight;
	private const Double AreaY4 = Margin + 3 * AreaHeight;
	private const Double NoteX = 0.1;
	private const Double NoteY0 = 0.1;
	private       Double NoteY1;
	private       Double NoteY2;
	private const Double NoteSize = 10;

	private PdfDocument	Document;
	private PdfContents BaseContents;
	private PdfFont		ArialNormal;
	private PdfFont		ArialBold;
	private PdfFont		ArialItalic;
	private PdfFont		ArialBoldItalic;
	private PdfFont		TimesNormal;
	private PdfFont		TimesBold;
	private PdfFont		TimesItalic;
	private PdfFont		TimesBoldItalic;
	private PdfFont		LucidaNormal;
	private PdfFont		Comic;
	private PdfFont		Symbol;

	////////////////////////////////////////////////////////////////////
	// create test PDF document
	////////////////////////////////////////////////////////////////////
	
	public void Test
			(
			Boolean Debug,
			String	FileName
			)
		{
		// create document (letter size, portrait, inches)
		Document = new PdfDocument(PageWidth, PageHeight, UnitOfMeasure.Inch);
		Document.Debug = Debug;

		// define font resource
		ArialNormal = new PdfFont(Document, "Arial", FontStyle.Regular, true);
		ArialBold = new PdfFont(Document, "Arial", FontStyle.Bold, true);
		ArialItalic = new PdfFont(Document, "Arial", FontStyle.Italic, true);
		ArialBoldItalic = new PdfFont(Document, "Arial", FontStyle.Bold | FontStyle.Italic, true);
		TimesNormal = new PdfFont(Document, "Times New Roman", FontStyle.Regular, true);
		TimesBold = new PdfFont(Document, "Times New Roman", FontStyle.Bold, true);
		TimesItalic = new PdfFont(Document, "Times New Roman", FontStyle.Italic, true);
		TimesBoldItalic = new PdfFont(Document, "Times New Roman", FontStyle.Bold | FontStyle.Italic, true);
		LucidaNormal = new PdfFont(Document, "Lucida Console", FontStyle.Regular, true);
		Comic = new PdfFont(Document, "Comic Sans MS", FontStyle.Regular, true);
		Symbol = new PdfFont(Document, "Wingdings", FontStyle.Regular, true);

		NoteY1 = NoteY0 + ArialNormal.LineSpacing(NoteSize);
		NoteY2 = NoteY1 + ArialNormal.LineSpacing(NoteSize);

		// print euro sign instead of cent sign
		ArialNormal.CharSubstitution(0x20ac, 0x20ac, 0xa2);

		// create page base contents
		BaseContents = CreateBaseContents(Document);

		// pages
		CreatePage1Contents();
		CreatePage2Contents();
		CreatePage3Contents();
		CreatePage4Contents();
		CreatePage5Contents();

		// create pdf file
		Document.CreateFile(FileName);

		// start default PDF reader and display the file
		Process Proc = new Process();
	    Proc.StartInfo = new ProcessStartInfo(FileName);
	    Proc.Start();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// create base contents for all pages
	////////////////////////////////////////////////////////////////////

	public PdfContents CreateBaseContents
			(
			PdfDocument Document
			)
		{
		// create unattached contents
		PdfContents Contents = new PdfContents(Document);

		// save graphics state
		Contents.SaveGraphicsState();

		// frame
		Contents.SetLineWidth(0.02);
		Contents.SetColorStroking(Color.Black);
		Contents.DrawRectangle(AreaX1, AreaY1, DispWidth, DispHeight, PaintOp.CloseStroke);
		Contents.DrawLine(AreaX2, AreaY1, AreaX2, AreaY4);
		Contents.DrawLine(AreaX1, AreaY2, AreaX3, AreaY2);
		Contents.DrawLine(AreaX1, AreaY3, AreaX3, AreaY3);

		// heading
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialBold, 24, 0.5 * PageWidth, PageHeight - Margin - (HeadingHeight - ArialBold.CapHeight(24)) / 2,
			TextJustify.Center, "PDF File Writer Example");

		// restore graphics state
		Contents.RestoreGraphicsState();

		// exit
		return(Contents);
		}

	////////////////////////////////////////////////////////////////////
	// create page1 contents
	////////////////////////////////////////////////////////////////////

	public void CreatePage1Contents()
		{
		// add contents to page
		PdfContents Contents = AddPageToDocument(1);

		// draw examples
		Example1a(Contents, AreaX1, AreaY3);
		Example1b(Contents, AreaX2, AreaY3);
		Example1c(Contents, AreaX1, AreaY2);
		Example1d(Contents, AreaX2, AreaY2);
		Example1e(Contents, AreaX1, AreaY1);
		Example1f(Contents, AreaX2, AreaY1);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// create page2 contents
	////////////////////////////////////////////////////////////////////

	public void CreatePage2Contents()
		{
		// add contents to page
		PdfContents Contents = AddPageToDocument(2);

		// draw examples
		Example2a(Contents, AreaX1, AreaY3);
		Example2b(Contents, AreaX2, AreaY3);
		Example2c(Contents, AreaX1, AreaY2);
		Example2d(Contents, AreaX2, AreaY2);
		Example2e(Contents, AreaX1, AreaY1);
		Example2f(Contents, AreaX2, AreaY1);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// create page3 contents
	////////////////////////////////////////////////////////////////////

	public void CreatePage3Contents()
		{
		// add contents to page
		PdfContents Contents = AddPageToDocument(3);

		// draw examples
		Example3a(Contents, AreaX1, AreaY3, 0.25, 0.5, 0.75, 2.2,  2.75, 1.9, 3.25, 0.9, "Example 3a: Cubic Bezier with both contol points", "on the same side of the curve");
		Example3a(Contents, AreaX2, AreaY3, 0.25, 0.5, 0.75, 2.2,  2.75, .75, 3.25, 2.75, "Example 3b: Cubic Bezier with contol points", "on the two sides of the curve");
		Example3a(Contents, AreaX1, AreaY2, 0.25, 0.5, 0, 0,  3.25, 2.9, 2.75, 0.9, "Example 3c: Cubic Bezier with end point P0", "equals control point P1");
		Example3a(Contents, AreaX2, AreaY2, 0.25, 0.5, 0.75, 2.2,  0, 0, 3.25, 2.75, "Example 3d: Cubic Bezier with control point P2", "equals end point P3");
		Example3e(Contents, AreaX1, AreaY1);
		Example3f(Contents, AreaX2, AreaY1);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// create page4 contents
	////////////////////////////////////////////////////////////////////

	public void CreatePage4Contents()
		{
		// add contents to page
		PdfContents Contents = AddPageToDocument(4);

		// draw examples
		Example4a(Contents, AreaX1, AreaY3);
		Example4b(Contents, AreaX2, AreaY3);
		Example4c(Contents, AreaX1, AreaY2);
		Example4d(Contents, AreaX2, AreaY2);
		Example4e(Contents, AreaX1, AreaY1);
		Example4f(Contents, AreaX2, AreaY1);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// create page5 contents
	////////////////////////////////////////////////////////////////////

	public void CreatePage5Contents()
		{
		// add contents to page
		PdfContents Contents = AddPageToDocument(5);

		// draw examples
		Example5a(Contents, AreaX1, AreaY3);
		Example5b(Contents, AreaX2, AreaY3);
		Example5c(Contents, AreaX1, AreaY2);
		Example5d(Contents, AreaX2, AreaY2);
		Example5e(Contents, AreaX1, AreaY1);
		Example5f(Contents, AreaX2, AreaY1);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Add page to document and draw page number
	////////////////////////////////////////////////////////////////////

	public PdfContents AddPageToDocument
			(
			Int32			PageNo
			)
		{
		// add new page with two contents objects
		PdfPage Page = new PdfPage(Document);
		Page.AddContents(BaseContents);
		PdfContents Contents = new PdfContents(Page);

		// draw page number right justified
		Contents.SaveGraphicsState();
		Contents.DrawText(ArialNormal, 10, PageWidth - Margin, PageHeight - 0.75 - ArialNormal.CapHeight(10), TextJustify.Right, String.Format("Page {0}", PageNo));
		Contents.RestoreGraphicsState();
		return(Contents);
		}

	////////////////////////////////////////////////////////////////////
	// Example 1a Draw three solid lines
	////////////////////////////////////////////////////////////////////

	public void Example1a
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// draw three solid lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetLineWidth(0.05);
		Contents.SetColorStroking(Color.Red);
		Contents.DrawLine(0.25, 1.0, AreaWidth - 0.25, 1.0);
		Contents.SetLineWidth(0.1);
		Contents.SetColorStroking(Color.Green);
		Contents.DrawLine(1.5, 0.5, 1.5, AreaHeight - 0.25);
		Contents.SetLineWidth(0.15);
		Contents.SetColorStroking(Color.DarkBlue);
		Contents.DrawLine(0.5, AreaHeight - 0.5, AreaWidth - 0.25, 0.25);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1a: Draw solid lines");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1b Three dashed lines
	////////////////////////////////////////////////////////////////////

	public void Example1b
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// draw three lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetLineWidth(0.05);
		Contents.SetDashLine(new Double[] {0.05, 0.05, 0.1, 0.05}, 0);
		Contents.SetColorStroking(Color.Red);
		Contents.DrawLine(0.25, 1.0, AreaWidth - 0.25, 1.0);
		Contents.SetLineWidth(0.1);
		Contents.SetDashLine(new Double[] {0.05, 0.05}, 0);
		Contents.SetColorStroking(Color.Green);
		Contents.DrawLine(1.5, 0.5, 1.5, AreaHeight - 0.25);
		Contents.SetLineWidth(0.15);
		Contents.SetDashLine(new Double[] {0.15, 0.02, 0.3, 0.02}, 0);
		Contents.SetColorStroking(Color.DarkBlue);
		Contents.DrawLine(0.5, AreaHeight - 0.5, AreaWidth - 0.25, 0.25);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1b: Draw dashed lines");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1c Three rectangles: stroke, fill, fill and stroke
	////////////////////////////////////////////////////////////////////

	public void Example1c
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// draw three rectangles
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetLineWidth(0.1);
		Contents.SetColorStroking(Color.DarkBlue);
		Contents.SetColorNonStroking(Color.Turquoise);
		Contents.DrawRectangle(0.2, 2.0, 1.0, 0.7, PaintOp.CloseStroke);
		Contents.DrawRectangle(0.8, 1.2, 1.2, 0.7, PaintOp.Fill);
		Contents.DrawRectangle(1.4, 0.4, 1.4, 0.7, PaintOp.CloseFillStroke);
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1c: Rectangles: stroke, fill, fill+stroke");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1d Rounded rectangle
	////////////////////////////////////////////////////////////////////

	public void Example1d
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// draw rounded rectangle
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetLineWidth(0.06);
		Contents.SetColorNonStroking(Color.Cyan);
		Contents.SetColorStroking(Color.Purple);
		Contents.DrawRoundedRectangle(0.2, 1.7, 2.5, 1.0, 0.25, PaintOp.CloseFillStroke);
		Contents.DrawInwardCornerRectangle(0.8, 0.4, 2.5, 1.0, 0.25, PaintOp.CloseFillStroke);
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1d: Draw Rounded Rectangle (fill+stroke)");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1e Ellipse and circle
	////////////////////////////////////////////////////////////////////

	public void Example1e
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// draw ellipse
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Double X = 0.5 * AreaWidth;
		Double Y = 0.5 * AreaHeight;

		Contents.SetColorNonStroking(Color.Yellow);
		Contents.DrawOval(X - 1.5, Y - 1.0, 3.0, 2.0, PaintOp.Fill);

		Contents.SetLineWidth(0.2);
		Contents.SetColorNonStroking(Color.White);
		Contents.SetColorStroking(Color.Black);
		Contents.DrawOval(X - 0.75, Y, 0.5, 0.5, PaintOp.CloseFillStroke);
		Contents.DrawOval(X + 0.25, Y, 0.5, 0.5, PaintOp.CloseFillStroke);

		Contents.SetColorNonStroking(Color.Black);
		Contents.MoveTo(X - 0.6, Y - 0.4);
		Contents.LineTo(X + 0.6, Y - 0.4);
		Contents.DrawBezier(X, Y - 0.8, X, Y - 0.8, X - 0.6, Y - 0.4);
		Contents.SetPaintOp(PaintOp.Fill);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1e: Ellipse, two circles, line and Bezier");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 1f polygon
	////////////////////////////////////////////////////////////////////

	public void Example1f
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		PointF[] Polygon = new PointF[]
			{
			new PointF(0.4F, 1.5F),
			new PointF(1.5F, 2.5F),
			new PointF(2.8F, 2.1F),
			new PointF(3.2F, 0.8F),
			new PointF(1.8F, 1.7F),
			new PointF(1.3F, 0.7F),
			};

		// draw polygon
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetLineWidth(0.06);
		Contents.SetColorNonStroking(Color.HotPink);
		Contents.SetColorStroking(Color.MidnightBlue);
		Contents.DrawPolygon(Polygon, PaintOp.CloseFillStroke);
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 1f: polygon");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2a polygon
	////////////////////////////////////////////////////////////////////

	public void Example2a
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		Double CenterX = 0.5 * AreaWidth;
		Double CenterY = NoteY2 + 0.5 * (AreaHeight - NoteY2);

		Color Fill = Color.LightCyan;
		for(Int32 Index = 0; Index < 4; Index++)
			{
			Contents.SetColorNonStroking(Fill);
			Contents.DrawRegularPolygon(CenterX, CenterY, (0.4 - 0.1 * Index) * AreaHeight, 90.0 * DegToRad, 8 - Index, PaintOp.CloseFillStroke);
			Fill = Color.FromArgb((Fill.R * 8)/ 10, Fill.G, Fill.B);
			}

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 2a: Regular polygons.");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "From 8 sides to 5 sides");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2b polygon
	////////////////////////////////////////////////////////////////////

	public void Example2b
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		PointD Center = new PointD(0.5 * AreaWidth, NoteY2 + 0.5 * (AreaHeight - NoteY2));

		Color Fill = Color.HotPink;
		for(Int32 Index = 0; Index < 4; Index++)
			{
			Contents.SetColorNonStroking(Fill);
			Contents.DrawStar(Center, (0.4 - 0.1 * Index) * AreaHeight, 90, 8 - Index, PaintOp.CloseFillStroke);
			Fill = Color.FromArgb((Fill.R * 8)/ 10, Fill.G, Fill.B);
			}

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 2b: Star shape polygons.");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "From 8 sides to 5 sides");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2c polygon
	////////////////////////////////////////////////////////////////////

	public void Example2c
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		Double CenterX = 0.5 * AreaWidth;
		Double CenterY = NoteY2 + 0.5 * (AreaHeight - NoteY2);

		Color Fill = Color.MediumSeaGreen;
		Contents.SetColorNonStroking(Fill);
		Contents.DrawStar(CenterX, CenterY, 0.4 * AreaHeight, 0.1 * AreaHeight, 90, 7, PaintOp.CloseFillStroke);

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 2c: Seven sided star shape polygon");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "with small inner circle");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2d polygon
	////////////////////////////////////////////////////////////////////

	public void Example2d
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		Contents.SetColorNonStroking(Color.HotPink);
		Contents.DrawHeart(0.5 * AreaWidth, NoteY2, 0.5 * AreaWidth, AreaHeight - 0.6, PaintOp.CloseFillStroke);

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 2d: Heart shape made of two");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Symmetric Bezier curves");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2e polygon
	////////////////////////////////////////////////////////////////////

	public void Example2e
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		Int32 Sides = 18;
		Double Radius1 = 0.45 * (AreaHeight - NoteY2);
		Double Radius2 = 0.75 * Radius1;
		Double Radius3 = 0.75 * Radius2;
		Double Radius4 = 0.85 * Radius3;
		PointD Center = new PointD(0.5 * AreaWidth, NoteY2 + 0.5 * (AreaHeight - NoteY2));
		Double Alpha = 0;
		Double DeltaAlpha = 2.0 * Math.PI / Sides;

		Contents.SetColorNonStroking(Color.CornflowerBlue);
		for(Int32 Index = 0; Index < Sides; Index++)
			{
			Contents.DrawHeart(new LineD(new PointD(Center, Radius2, Alpha), new PointD(Center, Radius1, Alpha)), PaintOp.Fill);
			Contents.DrawDoubleBezierPath(new LineD(new PointD(Center, Radius3, Alpha), new PointD(Center, Radius2, Alpha)), 0.5, 0.5 * Math.PI, 0.5, 1.5 * Math.PI, PaintOp.Fill);
			Contents.DrawStar(new PointD(Center, Radius4, Alpha), Radius3 - Radius4, Alpha, 5, PaintOp.Fill);
			Alpha += DeltaAlpha;
			}

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 2e: 18 spokes with heart");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "double Bezier path and a star");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 2f Image
	////////////////////////////////////////////////////////////////////

	public void Example2f
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		// image resources
		PdfImage Image1 = new PdfImage(Document, "Flower.jpg");
		Contents.SaveGraphicsState();
		SizeD ImageSize = Image1.ImageSize(0.8 * AreaWidth, 0.8 * AreaHeight);
		Double ImagePosX = (AreaWidth - ImageSize.Width) / 2;
		Double ImagePosY = (AreaHeight - ImageSize.Height) / 2;
		
		Contents.DrawOval(ImagePosX, ImagePosY, ImageSize.Width, ImageSize.Height, PaintOp.ClipPathEor);
		Contents.DrawImage(Image1, ImagePosX, ImagePosY, ImageSize.Width, ImageSize.Height);
		Contents.RestoreGraphicsState();

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 2f: Image");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "\"JPG\" image cropped by an oval");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 3a to 3d Bezier curves
	////////////////////////////////////////////////////////////////////

	public void Example3a
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY,
			Double			X0,
			Double			Y0,
			Double			X1,
			Double			Y1,
			Double			X2,
			Double			Y2,
			Double			X3,
			Double			Y3,
			String			Notes1,
			String			Notes2
			)
		{
		// draw Bezier
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetLineWidth(0.01);
		Contents.SetColorStroking(Color.Red);
		DrawCross(Contents, X0, Y0);
		DrawCross(Contents, X3, Y3);
		if(X1 != 0) DrawCross(Contents, X1, Y1);
		if(X2 != 0) DrawCross(Contents, X2, Y2);
		Contents.SetDashLine(new Double[] {0.05, 0.05}, 0);
		if(X1 != 0) Contents.DrawLine(X0, Y0, X1, Y1);
		if(X2 != 0) Contents.DrawLine(X2, Y2, X3, Y3);
		Contents.SetDashLine(null, 0);
		Contents.SetLineWidth(0.03);
		Contents.SetColorStroking(Color.SteelBlue);
		Contents.MoveTo(X0, Y0);
		if(X1 == 0) Contents.DrawBezierNoP1(X2, Y2, X3, Y3);
		else if(X2 == 0) Contents.DrawBezierNoP2(X1, Y1, X3, Y3);
		else Contents.DrawBezier(X1, Y1, X2, Y2, X3, Y3);
		Contents.SetPaintOp(PaintOp.Stroke);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, Notes1);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, Notes2);
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 3e Tiled colored pattern
	////////////////////////////////////////////////////////////////////

	public void Example3e
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// define patterns
		PdfTilingPattern BrickPattern = new PdfTilingPattern(Document);
		BrickPattern.SetScale(0.25);

		// draw rounded rectangle
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		Double Y = 2.8;
		Double Dy = 0.3;

		Contents.SetLineWidth(0.2);
		Contents.SetColorStroking(Color.DarkBlue);
		Contents.SetLineCap(PdfLineCap.Butt);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line cap: butt");
		Contents.DrawLine(1.4, Y, 2.8, Y);
		Y -= Dy;
		Contents.SetLineCap(PdfLineCap.Square);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line cap: square");
		Contents.DrawLine(1.4, Y, 2.8, Y);
		Y -= Dy;
		Contents.SetLineCap(PdfLineCap.Round);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line cap: round");
		Contents.DrawLine(1.4, Y, 2.8, Y);

		Contents.SetLineWidth(0.01);
		Contents.SetColorStroking(Color.Red);
		Contents.SetDashLine(new Double[] {0.05, 0.05}, 0);
		Contents.DrawLine(1.4, 2.9, 1.4, Y - 0.1);
		Contents.DrawLine(2.8, 2.9, 2.8, Y - 0.1);
		Contents.SetDashLine(null, 0);

		Dy = 0.6;
		Y -= Dy;
		Contents.SetLineWidth(0.12);
		Contents.SetColorStroking(Color.DarkBlue);
		Contents.SetLineCap(PdfLineCap.Square);
		Contents.SetLineJoin(PdfLineJoin.Miter);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line join: miter");
		Contents.DrawPolygon(new Single[] {1.4F, (Single) Y, 2.8F, (Single) Y, 2.1F, (Single) Y + 0.35F}, PaintOp.Stroke);
		Y -= Dy;
		Contents.SetLineJoin(PdfLineJoin.Bevel);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line join: bevel");
		Contents.DrawPolygon(new Single[] {1.4F, (Single) Y, 2.8F, (Single) Y, 2.1F, (Single) Y + 0.35F}, PaintOp.Stroke);
		Y -= Dy;
		Contents.SetLineJoin(PdfLineJoin.Round);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Line join: round");
		Contents.DrawPolygon(new Single[] {1.4F, (Single) Y, 2.8F, (Single) Y, 2.1F, (Single) Y + 0.35F}, PaintOp.Stroke);
		Y -= Dy;

		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 3e: Line cap and line join");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 3f Line cap line join
	////////////////////////////////////////////////////////////////////

	public void Example3f
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// draw three lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Double Y = 2.0;
		Double Dy = 0.8;
		Contents.SetLineWidth(0.12);
		Contents.SetColorStroking(Color.DarkBlue);
		Contents.SetLineCap(PdfLineCap.Square);
		Contents.SetLineJoin(PdfLineJoin.Miter);
		Contents.SetMiterLimit(5.8);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Miter limit 5.8");
		Contents.DrawPolygon(new Single[] {1.4F, (Single) Y, 2.8F, (Single) Y, 2.1F, (Single) Y + 0.25478F}, PaintOp.Stroke);
		Y -= Dy;
		Contents.SetMiterLimit(5.7);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, Y, "Miter limit 5.7");
		Contents.DrawPolygon(new Single[] {1.4F, (Single) Y, 2.8F, (Single) Y, 2.1F, (Single) Y + 0.25478F}, PaintOp.Stroke);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 3f: Miter limit for 20\u00b0. If miter limit is");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "less than 5.759 it is bevel join, otherwise it is miter join.");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 4a axial shading operator
	////////////////////////////////////////////////////////////////////

	public void Example4a
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// define shading function with 5 samples
		Color[] ColorArray = new Color[] {Color.Red, Color.DarkOrange, Color.Green, Color.Turquoise, Color.Blue};
		PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);

		// define axial shading object with default horizontal shading axis
		PdfAxialShading AxialShading = new PdfAxialShading(Document, 0.25, 0.4, 3, 2.35, ShadingFunction);

		// draw the shading object
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.DrawShading(AxialShading);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 4a: Horizontal Axial shading");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 4b axial shading operator
	////////////////////////////////////////////////////////////////////

	public void Example4b
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// define shading function with 2 samples
		Color[] ColorArray = new Color[] {Color.Red, Color.HotPink};
		PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);

		// create axial shading object
		PdfAxialShading AxialShading = new PdfAxialShading(Document, 0.25, 0.3, 3, 0.9 * AreaHeight, ShadingFunction);

		// set shading axial direction to vertical
		AxialShading.SetAxis(0.25, 0.3, 0, 2.45);

		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		// heart shape clip
		Contents.SaveGraphicsState();
		Contents.DrawHeart(0.5 * AreaWidth, 0.2 * AreaHeight, 0.5 * AreaWidth, 0.75 * AreaHeight, PaintOp.ClipPathEor);

		// draw shading
		Contents.DrawShading(AxialShading);
		Contents.RestoreGraphicsState();

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 4b: Vertical Axial shading clipped by two");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "symmetric Bezier curves.");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 15 axial shading operator
	////////////////////////////////////////////////////////////////////

	public void Example4c
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		Color[] ColorArray = new Color[]
			{Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
			Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
			Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
			Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
			Color.FromArgb(255, 200, 255), Color.FromArgb(200, 255, 255),
			};
		PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);
		PdfAxialShading AxialShading = new PdfAxialShading(Document, 0.25, 0.5, 3, 2.35, ShadingFunction);
		AxialShading.SetAxis(0.25, 2.75, 3, -3 * Math.Tan(30.0 * Math.PI / 180.0));

		// translate origin
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);

		// clip path with regular polygon
		Contents.SaveGraphicsState();
		Contents.DrawRegularPolygon(0.25 + 3.0 / 2, 0.5 + 2.35 / 2, 1.2, 0, 6, PaintOp.NoOperator);
		Contents.DrawRegularPolygon(0.25 + 3.0 / 2, 0.5 + 2.35 / 2, 0.4, 30.0 * DegToRad, 6, PaintOp.ClipPathEor);

		// draw shading
		Contents.DrawShading(AxialShading);
		Contents.RestoreGraphicsState();

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 4c: Diagonal Axial shading");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Shading axis is from top-left to bottom-right");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 4d axial shading operator
	////////////////////////////////////////////////////////////////////

	public void Example4d
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		Color[] ColorArray = new Color[] {Color.Red, Color.DarkOrange, Color.Green, Color.Turquoise, Color.Blue};
		PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);
		PdfRadialShading RadialShading = new PdfRadialShading(Document, 0.25, 0.5, 3, 2.35, ShadingFunction);

		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.DrawShading(RadialShading);
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 4d: Radial shading. One large circle and");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "a second concentric circle with zero radius.");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 4e axial shading operator
	////////////////////////////////////////////////////////////////////

	public void Example4e
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// define patterns
		PdfTilingPattern BrickPattern = PdfTilingPattern.SetBrickPattern(Document, 0.25, Color.LightYellow, Color.SandyBrown);

		// draw rounded rectangle
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetPatternNonStroking(BrickPattern);
		Contents.SetLineWidth(0.04);
		Contents.SetColorStroking(Color.Purple);
		Contents.DrawRoundedRectangle(0.25, 0.5, 3.0, 2.25, 0.3, PaintOp.CloseFillStroke);
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 4e: Brick pattern");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 4f Tiled colored pattern
	////////////////////////////////////////////////////////////////////

	public void Example4f
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// define patterns
		PdfTilingPattern WeavePattern = PdfTilingPattern.SetWeavePattern(Document, 0.3, Color.Black, Color.Beige, Color.Yellow);

		// draw rounded rectangle
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetPatternNonStroking(WeavePattern);
		Contents.SetLineWidth(0.04);
		Contents.SetColorStroking(Color.Purple);
		Contents.DrawRoundedRectangle(0.25, 0.5, 3.0, 2.25, 0.3, PaintOp.CloseFillStroke);
		Contents.SetColorNonStroking(Color.Black);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 4f: Weave pattern");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5a Text
	////////////////////////////////////////////////////////////////////

	public void Example5a
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Double FontSize = 14.0;
		Double BaseLine = AreaHeight - 0.4;
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, "Arial normal ABCD abcd 1234");
		BaseLine -= ArialBold.LineSpacing(FontSize);
		Contents.DrawText(ArialBold, FontSize, 0.1, BaseLine, "Arial bold ABCD abcd 1234");
		BaseLine -= ArialItalic.LineSpacing(FontSize);
		Contents.DrawText(ArialItalic, FontSize, 0.1, BaseLine, "Arial Italic ABCD abcd 1234");
		BaseLine -= ArialBoldItalic.LineSpacing(FontSize);
		Contents.DrawText(ArialBoldItalic, FontSize, 0.1, BaseLine, "Arial bold-italic ABCD abcd 1234");

		FontSize = 12.0;
		BaseLine -= 0.2 + TimesNormal.LineSpacing(FontSize);
		Contents.DrawText(TimesNormal, FontSize, 0.1, BaseLine, "Times New Roman normal ABCD abcd 1234");
		BaseLine -= TimesBold.LineSpacing(FontSize);
		Contents.DrawText(TimesBold, FontSize, 0.1, BaseLine, "Times New Roman bold ABCD abcd 1234");
		BaseLine -= TimesItalic.LineSpacing(FontSize);
		Contents.DrawText(TimesItalic, FontSize, 0.1, BaseLine, "Times New Roman Italic ABCD abcd 1234");
		BaseLine -= TimesBoldItalic.LineSpacing(FontSize);
		Contents.DrawText(TimesBoldItalic, FontSize, 0.1, BaseLine, "Times New Roman bold-italic ABCD abcd 1234");

		String Str = "Lucida Consol normal ABCD abcd 1234";
		FontSize = 10.0;
		BaseLine -= 0.2 + LucidaNormal.LineSpacing(FontSize);
		Contents.DrawText(LucidaNormal, FontSize, 0.1, BaseLine, Str);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY1, "Example 5a: Draw text using Arial,");
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Times New Roman and Lucida Consol fonts.");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5b Text
	////////////////////////////////////////////////////////////////////

	public void Example5b
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Double FontSize = 14.0;
		Double LineSpacing = ArialNormal.LineSpacing(FontSize);
		Double BaseLine = AreaHeight - 0.4;
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, DrawStyle.Underline, "Underline text example");
		BaseLine -= LineSpacing;

		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, DrawStyle.Strikeout, "Strikeout text eacmple");
		BaseLine -= LineSpacing;

		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, DrawStyle.Strikeout | DrawStyle.Underline, "Underline and strikeout combination");
		BaseLine -= LineSpacing;

		Double TextPos = 0.1;
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "Subscript example: H");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, DrawStyle.Subscript, "2");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "O");
		BaseLine -= LineSpacing;

		TextPos = 0.1;
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "Superscript example: A");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, DrawStyle.Superscript, "2");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "+B");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, DrawStyle.Superscript, "2");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "=C");
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, DrawStyle.Superscript, "2");
		BaseLine -= LineSpacing;

		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, "Substitution Euro over Cent: \u00a2");
		BaseLine -= 2 * LineSpacing;

		Contents.DrawText(Comic, 24, 0.1, BaseLine, "Comic Sans MS");
		BaseLine -= 2 * LineSpacing;

		TextPos = 0.1;
		TextPos += Contents.DrawText(ArialNormal, FontSize, TextPos, BaseLine, "Wingdings");
		TextPos += Contents.DrawText(Symbol, 24, TextPos, BaseLine, "\u0022\u0024\u002a\u003a");

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 5b: DrawStyle examples");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5c Text
	////////////////////////////////////////////////////////////////////

	public void Example5c
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.DrawText(ArialBold, 36.0, 0.1, 2.2, TextJustify.Left, 0.02, Color.RoyalBlue, Color.Empty, "Stroke Only");
		Contents.DrawText(ArialBold, 36.0, 0.1, 1.5, TextJustify.Left, 0.02, Color.DarkRed, Color.LightBlue, "Fill & Stroke");

		Contents.SaveGraphicsState();
		Contents.ClipText(ArialBold, 36.0, 0.1, 0.8, "Clip&Shading");
		Color[] ColorArray = new Color[] {Color.Red, Color.DarkOrange, Color.Green, Color.Turquoise, Color.Blue};
		PdfShadingFunction ShadingFunction = new PdfShadingFunction(Document, ColorArray);
		PdfAxialShading AxialShading = new PdfAxialShading(Document, 0.09, 0.5, 3.3, 1.0, ShadingFunction);
		Contents.DrawShading(AxialShading);
		Contents.RestoreGraphicsState();

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 5c: Draw text with special effects");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5d Text
	////////////////////////////////////////////////////////////////////

	public void Example5d
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Double FontSize = 36.0;
		Double BaseLine = AreaHeight / 2;
		String Text = "ATAWLTPAV";
		BoundingBox Box = TimesNormal.TextBoundingBox(FontSize, Text);
		Contents.DrawRectangle(0.2 + Box.Rect.Left, BaseLine + Box.Rect.Bottom, Box.Rect.Right - Box.Rect.Left, Box.Rect.Top - Box.Rect.Bottom, PaintOp.CloseStroke);
		Contents.DrawText(TimesNormal, FontSize, 0.2, BaseLine, Text);

		BaseLine -= 0.75;
		Contents.DrawRectangle(0.2 + Box.Rect.Left, BaseLine + Box.Rect.Bottom, Box.Rect.Right - Box.Rect.Left, Box.Rect.Top - Box.Rect.Bottom, PaintOp.CloseStroke);
		Double Width = Contents.DrawTextWithKerning(TimesNormal, FontSize, 0.2, BaseLine, Text);
		Contents.DrawLine(0.2 + Width, BaseLine + Box.Rect.Bottom, 0.2 + Width, BaseLine + Box.Rect.Top);

		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 5d: Draw text with and without kerning");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5e text box
	////////////////////////////////////////////////////////////////////

	public void Example5e
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.SetColorStroking(Color.Gray);
		Contents.DrawRectangle(0.25, 0.5, AreaWidth - 0.5, AreaHeight - 0.75, PaintOp.CloseStroke);
		TextBox Box = new TextBox(AreaWidth - 0.5);
		// add text to the text box
		Box.AddText(ArialNormal, 11,
			"This area is an example of displaying text that is too long to fit within a fixed width " +
			"area. The text is displayed justified to right edge. You define a text box with the required " +
			"width and first line indent. You add text to this box. The box will divide the text into " + 
			"lines. Each line is made of segments of text. For each segment, you define font, font " +
			"size, drawing style and color. After loading all the text, the program will draw the formatted text.\n");
		Double PosYText = AreaHeight - 0.25;
		Contents.DrawText(0.25, ref PosYText, 0.5, 0, 0.015, 0.06, true, Box);
		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 5e: TextBox class example");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Example 5f Text
	////////////////////////////////////////////////////////////////////

	public void Example5f
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		// draw three lines
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Double FontSize = 14.0;
		Double BaseLine = AreaHeight - 0.4;
		Double LineSpacing = ArialBold.LineSpacing(FontSize);
		String Str = "Test word and char spacing";

		Contents.DrawText(ArialNormal, NoteSize, 0.1, BaseLine, "Draw text normal");
		BaseLine -= LineSpacing;
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, TextJustify.Left, Str);
		BaseLine -= LineSpacing + 0.1;

		Contents.DrawText(ArialNormal, NoteSize, 0.1, BaseLine, "Draw text with character spacing 0.02\"");
		BaseLine -= LineSpacing;
		Contents.SaveGraphicsState();
		Contents.SetCharacterSpacing(0.02);
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, TextJustify.Left, Str);
		Contents.RestoreGraphicsState();
		BaseLine -= LineSpacing + 0.1;

		Contents.DrawText(ArialNormal, NoteSize, 0.1, BaseLine, "Draw text with word spacing 0.1\"");
		BaseLine -= LineSpacing;
		Contents.SaveGraphicsState();
		Contents.SetWordSpacing(0.1);
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, TextJustify.Left, Str);
		Contents.RestoreGraphicsState();
		BaseLine -= LineSpacing + 0.1;

		Contents.DrawText(ArialNormal, NoteSize, 0.1, BaseLine, "Draw text with word and charater spacing");
		BaseLine -= LineSpacing;
		Contents.SaveGraphicsState();
		Double WordSpacing;
		Double CharSpacing;
		ArialNormal.TextFitToWidth(FontSize, AreaWidth - 0.2, out WordSpacing, out CharSpacing, Str);
		Contents.SetWordSpacing(WordSpacing);
		Contents.SetCharacterSpacing(CharSpacing);
		Contents.DrawText(ArialNormal, FontSize, 0.1, BaseLine, TextJustify.Left, Str);
		Contents.RestoreGraphicsState();


		Contents.DrawText(ArialNormal, NoteSize, NoteX, NoteY0, "Example 5f: Word and character extra spacing");
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw small cross
	////////////////////////////////////////////////////////////////////

	public void DrawCross
			(
			PdfContents		Contents,
			Double			PosX,
			Double			PosY
			)
		{
		Contents.SaveGraphicsState();
		Contents.Translate(PosX, PosY);
		Contents.DrawLine(-0.1, 0, 0.1, 0);
		Contents.DrawLine(0, -0.1, 0, 0.1);
		Contents.RestoreGraphicsState();
		}
	}
}
