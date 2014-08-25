/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	ArticleExample
//	Produce PDF file when the Artice Example is clicked.
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
public class ArticleExample
	{
	private PdfFont ArialNormal;
	private PdfFont ArialBold;
	private PdfFont ArialItalic;
	private PdfFont ArialBoldItalic;
	private PdfFont TimesNormal;
	private PdfFont Comic;
	private PdfTilingPattern WaterMark;

	////////////////////////////////////////////////////////////////////
	// Create article's example test PDF document
	////////////////////////////////////////////////////////////////////
	
	public void Test
			(
			Boolean Debug,
			String	FileName
			)
		{
		// Step 1: Create empty document
		// Arguments: page width: 8.5”, page height: 11”, Unit of measure: inches
		// Return value: PdfDocument main class
		PdfDocument Document = new PdfDocument(8.5, 11.0, UnitOfMeasure.Inch);

		// Debug property
		// By default it is set to false. Use it for debugging only.
		// If this flag is set, PDF objects will not be compressed, font and images will be replaced
		// by text place holder. You can view the file with a text editor but you cannot open it with PDF reader.
		Document.Debug = Debug;

		// Step 2: create resources
		// define font resources
		DefineFontResources(Document);

		// define tiling pattern resources
		DefineTilingPatternResource(Document);

		// Step 3: Add new page
		PdfPage Page = new PdfPage(Document);

		// Step 4:Add contents to page
		PdfContents Contents = new PdfContents(Page);

		// Step 5: add graphices and text contents to the contents object
		DrawFrameAndBackgroundWaterMark(Contents);
		DrawTwoLinesOfHeading(Contents);
		DrawHappyFace(Contents);
		DrawBrickPattern(Document, Contents);
		DrawFlower(Document, Contents);
		DrawHeart(Contents);
		DrawStar(Contents);
		DrawTextBox(Contents);
		DrawBookOrderForm(Contents);

		// Step 6: create pdf file
		// argument: PDF file name
		Document.CreateFile(FileName);

		// start default PDF reader and display the file
		Process Proc = new Process();
	    Proc.StartInfo = new ProcessStartInfo(FileName);
	    Proc.Start();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Define Font Resources
	////////////////////////////////////////////////////////////////////

	private void DefineFontResources
			(
			PdfDocument Document
			)
		{
		// Define font resources
		// Arguments: PdfDocument class, font family name, font style, embed flag
		// Font style (must be: Regular, Bold, Italic or Bold | Italic) All other styles are invalid.
		// Embed font. If true, the font file will be embedded in the PDF file.
		// If false, the font will not be embedded
		ArialNormal = new PdfFont(Document, "Arial", FontStyle.Regular, true);
		ArialBold = new PdfFont(Document, "Arial", FontStyle.Bold, true);
		ArialItalic = new PdfFont(Document, "Arial", FontStyle.Italic, true);
		ArialBoldItalic = new PdfFont(Document, "Arial", FontStyle.Bold | FontStyle.Italic, true);
		TimesNormal = new PdfFont(Document, "Times New Roman", FontStyle.Regular, true);
		Comic = new PdfFont(Document, "Comic Sans MS", FontStyle.Bold, true);

		// substitute one character for another
		// this program supports characters 32 to 126 and 160 to 255
		// if a font has a character outside these ranges that is required by the application,
		// you can replace an unused character with this character
		ArialNormal.CharSubstitution(9679, 9679, 164);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Define Tiling Pattern Resource
	////////////////////////////////////////////////////////////////////

	private void DefineTilingPatternResource
			(
			PdfDocument Document
			)
		{
		// create empty tiling pattern
		WaterMark = new PdfTilingPattern(Document);

		// the pattern will be PdfFileWriter laied out in brick pattern
		String Mark = "PdfFileWriter";

		// text width and height for Arial bold size 18 points
		Double FontSize = 18.0;
		Double TextWidth = ArialBold.TextWidth(FontSize, Mark);
		Double TextHeight = ArialBold.LineSpacing(FontSize);

		// text base line
		Double BaseLine = ArialBold.DescentPlusLeading(FontSize);

		// the overall pattern box (we add text height value as left and right text margin)
		Double BoxWidth = TextWidth + 2 * TextHeight;
		Double BoxHeight = 4 * TextHeight;
		WaterMark.SetTileBox(BoxWidth, BoxHeight);

		// save graphics state
		WaterMark.SaveGraphicsState();

		// fill the pattern box with background light blue color
		WaterMark.SetColorNonStroking(Color.FromArgb(230, 244, 255));
		WaterMark.DrawRectangle(0, 0, BoxWidth, BoxHeight, PaintOp.Fill);

		// set fill color for water mark text to white
		WaterMark.SetColorNonStroking(Color.White);

		// draw PdfFileWriter at the bottom center of the box
		WaterMark.DrawText(ArialBold, FontSize, BoxWidth / 2, BaseLine, TextJustify.Center, Mark);

		// adjust base line upward by half height
		BaseLine += BoxHeight / 2;

		// draw the right half of PdfFileWriter shifted left by half width
		WaterMark.DrawText(ArialBold, FontSize, 0.0, BaseLine, TextJustify.Center, Mark);

		// draw the left half of PdfFileWriter shifted right by half width
		WaterMark.DrawText(ArialBold, FontSize, BoxWidth, BaseLine, TextJustify.Center, Mark);

		// restore graphics state
		WaterMark.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw frame around example area
	////////////////////////////////////////////////////////////////////

	private void DrawFrameAndBackgroundWaterMark
			(
			PdfContents Contents
			)
		{
		// save graphics state
		Contents.SaveGraphicsState();

		// Draw frame around the page
		// Set line width to 0.02"
		Contents.SetLineWidth(0.02);

		// set frame color dark blue
		Contents.SetColorStroking(Color.DarkBlue);

		// use water mark tiling pattern to fill the frame
		Contents.SetPatternNonStroking(WaterMark);

		// rectangle position: x=1.0", y=1.0", width=6.5", height=9.0"
		Contents.DrawRectangle(1.0, 1.0, 6.5, 9.0, PaintOp.CloseFillStroke);

		// restore graphics sate
		Contents.RestoreGraphicsState();

		// draw article name under the frame
		// Note: the \u00a4 is character 164 that was substituted during Font resource definition
		// this character is a solid circle it is normally unicode 9679 or \u25cf in the Arial family
		Contents.DrawText(ArialNormal, 9.0, 1.1, 0.85, "PdfFileWriter \u00a4 PDF File Writer C# Class Library \u00a4 Author: Uzi Granot");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw heading
	////////////////////////////////////////////////////////////////////

	private void DrawTwoLinesOfHeading
			(
			PdfContents Contents
			)
		{
		// page heading
		// Arguments: Font: ArialBold, size: 36 points, Position: X = 4.25", Y = 9.5"
		// Text Justify: Center (text center will be at X position)
		// Stoking color: R=128, G=0, B=255 (text outline)
		// Nonstroking color: R=255, G=0, B=128 (text body)
		Contents.DrawText(Comic, 40.0, 4.25, 9.25, TextJustify.Center, 0.02, Color.FromArgb(128, 0, 255), Color.FromArgb(255, 0, 128), "PDF FILE WRITER");

		// save graphics state
		Contents.SaveGraphicsState();

		// change nonstroking (fill) color to purple
		Contents.SetColorNonStroking(Color.Purple);

		// Draw second line of heading text
		// arguments: Handwriting font, Font size 30 point, Position X=4.25", Y=9.0"
		// Text Justify: Center (text center will be at X position)
		Contents.DrawText(Comic, 30.0, 4.25, 8.75, TextJustify.Center, "Example");
//		Contents.TranslateScaleRotate(4.25, 8.75, 1.5, Math.PI / 6);
//		Contents.DrawText(Comic, 30.0, 0, 0, TextJustify.Center, "Example");

		// restore graphics sate (non stroking color will be restored to default)
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw Happy Face
	////////////////////////////////////////////////////////////////////

	private void DrawHappyFace
			(
			PdfContents Contents
			)
		{
		// save graphics state
		Contents.SaveGraphicsState();

		// translate coordinate origin to the center of the happy face
		Contents.Translate(4.25, 7.5);

		// change nonstroking (fill) color to yellow
		Contents.SetColorNonStroking(Color.Yellow);

		// draw happy face yellow oval
		Contents.DrawOval(-1.5, -1.0, 3.0, 2.0, PaintOp.Fill);

		// set line width to 0.2" this is the black circle around the eye
		Contents.SetLineWidth(0.2);

		// eye color is white with black outline circle
		Contents.SetColorNonStroking(Color.White);
		Contents.SetColorStroking(Color.Black);

		// draw eyes
		Contents.DrawOval(-0.75, 0.0, 0.5, 0.5, PaintOp.CloseFillStroke);
		Contents.DrawOval(0.25, 0.0, 0.5, 0.5, PaintOp.CloseFillStroke);

		// mouth color is black
		Contents.SetColorNonStroking(Color.Black);

		// draw mouth by creating a path made of one line and one Bezier curve 
		Contents.MoveTo(-0.6, -0.4);
		Contents.LineTo(0.6, -0.4);
		Contents.DrawBezier(0.0, -0.8, 0, -0.8, -0.6, -0.4);

		// fill the path with black color
		Contents.SetPaintOp(PaintOp.Fill);

		// restore graphics sate
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw rectangle with rounded corners and filled with brick pattern
	////////////////////////////////////////////////////////////////////

	private void DrawBrickPattern
			(
			PdfDocument Document,
			PdfContents Contents
			)
		{
		// Define brick tilling pattern resource
		// Arguments: PdfDocument class, Scale factor (0.25), Stroking color (lines between bricks), Nonstroking color (brick)
		// Return value: tilling pattern resource
		PdfTilingPattern BrickPattern = PdfTilingPattern.SetBrickPattern(Document, 0.25, Color.LightYellow, Color.SandyBrown);

		// save graphics state
		Contents.SaveGraphicsState();

		// set outline width 0.04"
		Contents.SetLineWidth(0.04);

		// set outline color to purple
		Contents.SetColorStroking(Color.Purple);

		// set fill pattern to brick
		Contents.SetPatternNonStroking(BrickPattern);

		// draw rounded rectangle filled with brick pattern
		Contents.DrawRoundedRectangle(1.13, 5.0, 1.4, 1.4, 0.2, PaintOp.CloseFillStroke);

		// restore graphics sate
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw image of a flower and clip it
	////////////////////////////////////////////////////////////////////

	private void DrawFlower
			(
			PdfDocument Document,
			PdfContents Contents
			)
		{
		// define local image resources
		PdfImage Image1 = new PdfImage(Document, "flower.jpg");

		// image size will be limited to 1.4" by 1.4"
		SizeD ImageSize = Image1.ImageSize(1.4, 1.4);

		// save graphics state
		Contents.SaveGraphicsState();

		// translate coordinate origin to the center of the picture
		Contents.Translate(3.36, 5.7);

		// clipping path
		Contents.DrawOval(-ImageSize.Width / 2, -ImageSize.Height / 2, ImageSize.Width, ImageSize.Height, PaintOp.ClipPathEor);

		// draw image
		Contents.DrawImage(Image1, -ImageSize.Width / 2, -ImageSize.Height / 2, ImageSize.Width, ImageSize.Height);

		// restore graphics state
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw heart
	////////////////////////////////////////////////////////////////////

	private void DrawHeart
			(
			PdfContents Contents
			)
		{
		// save graphics state
		Contents.SaveGraphicsState();

		// draw heart
		// The first argument are the coordinates of the centerline of the heart shape.
		// The DrawHeart is a special case of DrawDoubleBezierPath method.
		Contents.SetColorNonStroking(Color.HotPink);
		Contents.DrawHeart(new LineD(new PointD(5.14, 5.1), new PointD(5.14, 6.0)), PaintOp.CloseFillStroke);

		// restore graphics state
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw six sided regular polygon
	////////////////////////////////////////////////////////////////////

	private void DrawStar
			(
			PdfContents Contents
			)
		{
		// save graphics state
		Contents.SaveGraphicsState();

		// Regular polygon
		// Coordinate of center point is PosX=6.67" PosY=5.7"
		// Radius of end points is 0.7"
		// First drawing point is at 30 degrees (in radians PI / 6).
		// Number of sides is 6 
		Contents.SetColorNonStroking(Color.Turquoise);
		Contents.DrawStar(6.67, 5.7, 0.7, Math.PI / 6, 6, PaintOp.CloseFillStroke);

		// restore graphics state
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw example of a text box
	////////////////////////////////////////////////////////////////////

	private void DrawTextBox
			(
			PdfContents Contents
			)
		{
		// save graphics state
		Contents.SaveGraphicsState();

		// translate origin to PosX=1.1" and PosY=1.1" this is the bottom left corner of the text box example
		Contents.Translate(1.1, 1.1);
//		Contents.TranslateScaleRotate(7.4, 1.1, 1.0, Math.PI / 2);

		// Define constants
		// Box width 3.25"
		// Box height is 3.65"
		// Normal font size is 9.0 points.
		const Double Width = 3.15;
		const Double Height = 3.65;
		const Double FontSize = 9.0;

		// Create text box object width 3.25"
		// First line indent of 0.25"
		TextBox Box = new TextBox(Width, 0.25);

		// add text to the text box
		Box.AddText(ArialNormal, FontSize,
			"This area is an example of displaying text that is too long to fit within a fixed width " +
			"area. The text is displayed justified to right edge. You define a text box with the required " +
			"width and first line indent. You add text to this box. The box will divide the text into " + 
			"lines. Each line is made of segments of text. For each segment, you define font, font " +
			"size, drawing style and color. After loading all the text, the program will draw the formatted text.\n");
		Box.AddText(TimesNormal, FontSize + 1.0, "Example of multiple fonts: Times New Roman, ");
		Box.AddText(Comic, FontSize, "Comic Sans MS, ");
		Box.AddText(ArialNormal, FontSize, "Example of regular, ");
		Box.AddText(ArialBold, FontSize, "bold, ");
		Box.AddText(ArialItalic, FontSize, "italic, ");
		Box.AddText(ArialBoldItalic, FontSize, "bold plus italic. ");
		Box.AddText(ArialNormal, FontSize - 2.0, "Arial size 7, ");
		Box.AddText(ArialNormal, FontSize - 1.0, "size 8, ");
		Box.AddText(ArialNormal, FontSize, "size 9, ");
		Box.AddText(ArialNormal, FontSize + 1.0, "size 10. ");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Underline, "Underline, ");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Strikeout, "Strikeout. ");
		Box.AddText(ArialNormal, FontSize, "Subscript H");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Subscript, "2");
		Box.AddText(ArialNormal, FontSize, "O. Superscript A");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Superscript, "2");
		Box.AddText(ArialNormal, FontSize, "+B");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Superscript, "2");
		Box.AddText(ArialNormal, FontSize, "=C");
		Box.AddText(ArialNormal, FontSize, DrawStyle.Superscript, "2");
		Box.AddText(ArialNormal, FontSize, "\n");
		Box.AddText(Comic, FontSize, Color.Red, "Lets add some color, ");
		Box.AddText(Comic, FontSize, Color.Green, "green, ");
		Box.AddText(Comic, FontSize, Color.Blue, "blue, ");
		Box.AddText(Comic, FontSize, Color.Orange, "orange, ");
		Box.AddText(Comic, FontSize, DrawStyle.Underline, Color.Purple, "and purple.\n");

		// Draw the text box
		// Text left edge is at zero (note: origin was translated to 1.1") 
		// The top text base line is at Height less first line ascent.
		// Text drawing is limited to vertical coordinate of zero.
		// First line to be drawn is line zero.
		// After each line add extra 0.015".
		// After each paragraph add extra 0.05"
		// Stretch all lines to make smooth right edge at box width of 3.15"
		// After all lines are drawn, PosY will be set to the next text line after the box's last paragraph
		Double PosY = Height;
		Contents.DrawText(0.0, ref PosY, 0.0, 0, 0.015, 0.05, true, Box);

		// Create text box object width 3.25"
		// No first line indent
		Box = new TextBox(Width);

		// Add text as before.
		// No extra line spacing.
		// No right edge adjustment
		Box.AddText(ArialNormal, FontSize,
			"In the examples above this area the text box was set for first line indent of " +
			"0.25 inches. This paragraph has zero first line indent and no right justify.");
		Contents.DrawText(0.0, ref PosY, 0.0, 0, 0.01, 0.05, false, Box);

		// Create text box object width 2.75
		// First line hanging indent of 0.5"
		Box = new TextBox(Width - 0.5, -0.5);

		// Add text
		Box.AddText(ArialNormal, FontSize,
			"This paragraph is set to first line hanging indent of 0.5 inches. " +
			"The left margin of this paragraph is 0.5 inches.");

		// Draw the text
		// left edge at 0.5"
		Contents.DrawText(0.5, ref PosY, 0.0, 0, 0.01, 0.05, false, Box);

		// restore graphics state
		Contents.RestoreGraphicsState();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Draw example of order form
	////////////////////////////////////////////////////////////////////

	private void DrawBookOrderForm
			(
			PdfContents Contents
			)
		{
		// Order form simulation
		// Define constants to make the code readable
		// Define constants
		const Double Width = 3.05;
		const Double Height = 3.65;
		const Double Margin = 0.04;
		const Double FontSize = 9.0;
		Double LineSpacing = ArialNormal.LineSpacing(FontSize);
		Double Descent = ArialNormal.Descent(FontSize);
		Double ColWidth1 = ArialNormal.TextWidth(FontSize, "9999.99") + 2 * Margin;
		Double ColWidth2 = ArialNormal.TextWidth(FontSize, "Qty") + 2 * Margin;
		Double Col4LinePosX = Width - ColWidth1;
		Double Col3LinePosX = Col4LinePosX - ColWidth2;
		Double Col2LinePosX = Col3LinePosX - ColWidth1;
		Double Col1TextPosX = Margin;
		Double Col2TextPosX = Col3LinePosX - Margin;
		Double Col3TextPosX = Col4LinePosX - Margin;
		Double Col4TextPosX = Width - Margin;
 
		// save graphics state
		Contents.SaveGraphicsState();

		// form line width 0.01"
		Contents.SetLineWidth(0.01);

		// Initial vertical position for contents
		Double PosY1 = Height - LineSpacing - 2 * Margin;

		// bottom of the contents area of the form
		Double PosY2 = 2 * Margin + 3 * LineSpacing;

		// shift origin, bottom left of the form to X=4.35" and Y=1.1"
		Contents.Translate(4.35, 1.1);

		// draw outline rectangle
		Contents.DrawRectangle(0.0, 0.0, Width, Height, PaintOp.CloseStroke);

		// draw two horizontal lines. under table heading and above total
		Contents.DrawLine(0, PosY1, Width, PosY1);
		Contents.DrawLine(0, PosY2, Width, PosY2);

		// draw three vertical lines separating the columns
		Contents.DrawLine(Col2LinePosX, Height, Col2LinePosX, PosY2);
		Contents.DrawLine(Col3LinePosX, Height, Col3LinePosX, PosY2);
		Contents.DrawLine(Col4LinePosX, Height, Col4LinePosX, 0);

		// draw table heading
		Double PosY = PosY1 + Margin + Descent;
		Contents.DrawText(ArialNormal, FontSize, Col1TextPosX, PosY, "Description");
		Contents.DrawText(ArialNormal, FontSize, Col2TextPosX, PosY, TextJustify.Right, "Price");
		Contents.DrawText(ArialNormal, FontSize, Col3TextPosX, PosY, TextJustify.Right, "Qty");
		Contents.DrawText(ArialNormal, FontSize, Col4TextPosX, PosY, TextJustify.Right, "Total");
 
		// reset order total
		Double Total = 0;

		// define text box for book title and author
		TextBox Box = new TextBox(Col2LinePosX - 2 * Margin);

		// initial vertical position
		PosY = PosY1 - Margin;

		// loop for all items in the order
		// Order class is a atabase simulation for this example
		foreach(Order Book in Order.OrderList)
			{
			// clear the text box
			Box.Clear();

			// add book title and authors to the box
			Box.AddText(ArialNormal, FontSize, Book.Title);
			Box.AddText(ArialNormal, FontSize, ". By: ");
			Box.AddText(ArialNormal, FontSize, Book.Authors);

			// draw the title and authors.
			// on exit, PosY will be for next line
			Contents.DrawText(Col1TextPosX, ref PosY, PosY2, 0, Box);

			// move PosY up to allow drawing cost on the same line as the last text line of the box
			PosY += Descent;

			// draw price quantity and item's total
			Contents.DrawText(ArialNormal, FontSize, Col2TextPosX, PosY, TextJustify.Right, Book.Price.ToString("#.00"));
			Contents.DrawText(ArialNormal, FontSize, Col3TextPosX, PosY, TextJustify.Right, Book.Qty.ToString());
			Contents.DrawText(ArialNormal, FontSize, Col4TextPosX, PosY, TextJustify.Right, Book.Total.ToString("#.00"));

			// update PosY for next item
			PosY -= Descent + 0.5 * LineSpacing;

			// accumulate total
			Total += Book.Total;
			}

		// draw total before tax
		PosY = PosY2 - Margin - ArialNormal.Ascent(FontSize);
		Contents.DrawText(ArialNormal, FontSize, Col3TextPosX, PosY, TextJustify.Right, "Total before tax");
		Contents.DrawText(ArialNormal, FontSize, Col4TextPosX, PosY, TextJustify.Right, Total.ToString("#.00"));

		// draw tax (Ontario Canada HST)
		PosY -= LineSpacing;
		Contents.DrawText(ArialNormal, FontSize, Col3TextPosX, PosY, TextJustify.Right, "Tax (13%)");
		Double Tax = Math.Round(0.13 * Total, 2, MidpointRounding.AwayFromZero);
		Contents.DrawText(ArialNormal, FontSize, Col4TextPosX, PosY, TextJustify.Right, Tax.ToString("#.00"));

		// draw final total
		PosY -= LineSpacing;
		Contents.DrawText(ArialNormal, FontSize, Col3TextPosX, PosY, TextJustify.Right, "Total payable");
		Total += Tax;
		Contents.DrawText(ArialNormal, FontSize, Col4TextPosX, PosY, TextJustify.Right, Total.ToString("#.00"));

		// restore graphics state
		Contents.RestoreGraphicsState();
		return;
		}
	}

// Simulation of book order database
public class Order
	{
	public String	Title;
	public String	Authors;
	public Double	Price;
	public Int32	Qty;

	public Double	Total
		{
		get
			{
			return(Price * Qty);
			}
		}

	public Order
			(
			String	Title,
			String	Authors,
			Double	Price,
			Int32	Qty
			)
		{
		this.Title = Title;
		this.Authors = Authors;
		this.Price = Price;
		this.Qty = Qty;
		return;
		}

	public static Order[] OrderList = new Order[]
		{
		new Order("Current Trends in Theoritical Computer Science: Algorithms and Complexity", "Paun", 123.5, 2),
		new Order("Theoretical Computer Science: Introduction to Automata, Computability, Complexity, Algorithmics, Randomization", "Juraj Hromkovic", 76.81, 1),
		new Order("Discrete Mathematics for Computer Science (with Student Solutions Manual CD-ROM)", "Gary Haggard, John Schlipf and Sue Whitesides", 229.88, 1),
		};
	}
}
