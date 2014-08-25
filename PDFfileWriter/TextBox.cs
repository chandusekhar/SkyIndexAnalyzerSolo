/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	TextBox
//  Support class for PdfContents class. Format text to fit column.
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace PdfFileWriter
{
public class TextLine
	{
	public Double		Ascent;
	public Double		Descent;
	public Boolean		EndOfParagraph;
	public TextSeg[]	SegArray;

	public Double		LineHeight
		{
		get
			{
			return(Ascent + Descent);
			}
		}

	public TextLine
			(
			Double			Ascent,
			Double			Descent,
			Boolean			EndOfParagraph,
			TextSeg[]		SegArray
			)
		{
		this.Ascent = Ascent;
		this.Descent = Descent;
		this.EndOfParagraph = EndOfParagraph;
		this.SegArray = SegArray;
		return;
		}
	}

public class TextSeg
	{
	public PdfFont		Font;
	public Double		FontSize;
	public DrawStyle	DrawStyle;
	public Color		FontColor;
	public Double		SegWidth;
	public Int32		SpaceCount;
	public String		Text;

	public TextSeg() {}

	public TextSeg
			(
			PdfFont		Font,
			Double		FontSize
			)
		{
		this.Font = Font;
		this.FontSize = FontSize;
		DrawStyle = DrawStyle.Normal;
		FontColor = Color.Empty;
		Text = String.Empty;
		return;
		}

	public TextSeg
			(
			PdfFont		Font,
			Double		FontSize,
			DrawStyle	DrawStyle
			)
		{
		this.Font = Font;
		this.FontSize = FontSize;
		this.DrawStyle = DrawStyle;
		FontColor = Color.Empty;
		Text = String.Empty;
		return;
		}

	public TextSeg
			(
			PdfFont		Font,
			Double		FontSize,
			Color		FontColor
			)
		{
		this.Font = Font;
		this.FontSize = FontSize;
		this.FontColor = FontColor;
		DrawStyle = DrawStyle.Normal;
		Text = String.Empty;
		return;
		}

	public TextSeg
			(
			PdfFont		Font,
			Double		FontSize,
			DrawStyle	DrawStyle,
			Color		FontColor
			)
		{
		this.Font = Font;
		this.FontSize = FontSize;
		this.DrawStyle = DrawStyle;
		this.FontColor = FontColor;
		Text = String.Empty;
		return;
		}

	public TextSeg
			(
			TextSeg		CopySeg
			)
		{
		this.Font = CopySeg.Font;
		this.FontSize = CopySeg.FontSize;
		this.DrawStyle = CopySeg.DrawStyle;
		this.FontColor = CopySeg.FontColor;
		Text = String.Empty;
		return;
		}

	public Boolean IsEqual
			(
			TextSeg		Other
			)
		{
		return(this.Font == Other.Font && this.FontSize == Other.FontSize && this.DrawStyle == Other.DrawStyle && this.FontColor == Other.FontColor);
		}

	public Boolean IsEqual
			(
			PdfFont		Font,
			Double		FontSize,
			DrawStyle	DrawStyle,
			Color		FontColor
			)
		{
		return(this.Font == Font && this.FontSize == FontSize && this.DrawStyle == DrawStyle && this.FontColor == FontColor);
		}
	}

public class TextBox : IEnumerable<TextLine>, IEnumerator<TextLine>
	{
	private Double			_BoxWidth;
	private Double			_BoxHeight;
	private Int32			_ParagraphCount;
	private Double			_FirstLineIndent;	
	private Double			LineBreakFactor;	 // should be >= 0.1 and <= 0.9
	private Char			PrevChar;
	private Double			LineWidth;
	private Double			LineBreakWidth;
	private Int32			BreakSegIndex;
	private Int32			BreakPtr;
	private Double			BreakWidth;
	private Int32			EnumIndex;
	private List<TextSeg>	SegArray;
	private List<TextLine>	LineArray;

	public Double BoxWidth {get{return(_BoxWidth);}}
	public Double BoxHeight {get{return(_BoxHeight);}}
	public Int32  ParagraphCount {get{return(_ParagraphCount);}}
	public Double FirstLineIndent {get{return(_FirstLineIndent);}}

	public TextBox
			(
			Double		TextWidth
			)
		{
		this._BoxWidth = TextWidth;
		_FirstLineIndent = 0.0;
		LineBreakFactor = 0.5;
		SegArray = new List<TextSeg>();
		LineArray = new List<TextLine>();
		Clear();
		return;
		}

	public TextBox
			(
			Double		TextWidth,
			Double		FirstLineIndent
			)
		{
		this._BoxWidth = TextWidth;
		this._FirstLineIndent = FirstLineIndent;
		LineBreakFactor = 0.5;
		SegArray = new List<TextSeg>();
		LineArray = new List<TextLine>();
		Clear();
		return;
		}

	public TextBox
			(
			Double		TextWidth,
			Double		FirstLineIndent,
			Double		LineBreakFactor
			)
		{
		this._BoxWidth = TextWidth;
		this._FirstLineIndent = FirstLineIndent;
		if(LineBreakFactor < 0.1 || LineBreakFactor > 0.9) throw new ApplicationException("LineBreakFactor must be between 0.1 and 0.9");
		this.LineBreakFactor = LineBreakFactor;
		SegArray = new List<TextSeg>();
		LineArray = new List<TextLine>();
		Clear();
		return;
		}

	public void Clear()
		{
		_BoxHeight = 0.0;
		_ParagraphCount = 0;
		PrevChar = ' ';
		LineWidth = 0.0;
		LineBreakWidth = 0.0;
		BreakSegIndex = 0;
		BreakPtr = 0;
		BreakWidth = 0;
		SegArray.Clear();
		LineArray.Clear();
		return;
		}

	public Int32 LineCount
		{
		get
			{
			return(LineArray.Count);
			}
		}

	public TextLine this[Int32 Index]
		{
		get
			{
			return(LineArray[Index]);
			}
		}

	public IEnumerator<TextLine> GetEnumerator()
		{
		return(this);
		}

	IEnumerator IEnumerable.GetEnumerator()
		{
		return((IEnumerator) GetEnumerator());
		}

	public void Reset()
		{
		EnumIndex = -1;
		return;
		}

	public Boolean MoveNext()
		{
		return(++EnumIndex < LineArray.Count);
		}

	public TextLine Current
		{
		get
			{
			return(LineArray[EnumIndex]);
			}
		}

	object IEnumerator.Current
		{
		get
			{
			return(Current);
			}
		}

	public void Dispose() {}

	public void Terminate()
		{
		// terminate last line
		if(SegArray.Count != 0) AddLine(true);

		// remove trailing empty paragraphs
		for(Int32 Index = LineArray.Count - 1; Index >= 0; Index--)
			{
			TextLine Line = LineArray[Index];
			if(!Line.EndOfParagraph || Line.SegArray.Length > 1 || Line.SegArray[0].SegWidth != 0) break;
			_BoxHeight -= Line.Ascent + Line.Descent;
			_ParagraphCount--;
			LineArray.RemoveAt(Index);
			}

		// exit
		return;
		}

	public void AddText
			(
			PdfFont		Font,
			Double		FontSize,
			String		Text
			)
		{
		AddText(Font, FontSize, DrawStyle.Normal, Color.Empty, Text);
		return;
		}

	public void AddText
			(
			PdfFont		Font,
			Double		FontSize,
			DrawStyle	DrawStyle,
			String		Text
			)
		{
		AddText(Font, FontSize, DrawStyle, Color.Empty, Text);
		return;
		}

	public void AddText
			(
			PdfFont		Font,
			Double		FontSize,
			Color		FontColor,
			String		Text
			)
		{
		AddText(Font, FontSize, DrawStyle.Normal, FontColor, Text);
		return;
		}

	public void AddText
			(
			PdfFont		Font,
			Double		FontSize,
			DrawStyle	DrawStyle,
			Color		FontColor,
			String		Text
			)
		{
		// text is null or empty
		if(String.IsNullOrEmpty(Text)) return;

		// create new text segment
		TextSeg Seg = new TextSeg(Font, FontSize, DrawStyle, FontColor);

		// segment array is empty or new segment is different than last one
		if(SegArray.Count == 0 || !SegArray[SegArray.Count - 1].IsEqual(Seg))
			{
			SegArray.Add(Seg);
			}

		// add new text to most recent text segment
		else
			{
			Seg = SegArray[SegArray.Count - 1];
			}

		// save text start pointer
		Int32 TextStart = 0;

		// loop for characters
		for(Int32 TextPtr = 0; TextPtr < Text.Length; TextPtr++)
			{
			// shortcut to current character
			Char CurChar = Text[TextPtr];

			// end of paragraph
			if(CurChar == '\n' || CurChar == '\r')
				{
				// append text to current segemnt
				Seg.Text += Text.Substring(TextStart, TextPtr - TextStart);

				// test for new line after carriage return
				if(CurChar == '\r' && TextPtr + 1 < Text.Length && Text[TextPtr + 1] == '\n') TextPtr++;

				// move pointer to one after the eol
				TextStart = TextPtr + 1;

				// add line
				AddLine(true);

				// update last character
				PrevChar = ' ';

				// end of text
				if(TextPtr + 1 == Text.Length) return;

				// add new empty segment
				Seg = new TextSeg(Font, FontSize, DrawStyle);
				SegArray.Add(Seg);
				continue;
				}

			// validate character
			Font.ValidateChar(CurChar);

			// character width
			Double CharWidth = Font.CharWidth(FontSize, Seg.DrawStyle, CurChar);

			// space
			if(CurChar == ' ')
				{
				// test for transition from non space to space
				// this is a potential line break point
				if(PrevChar != ' ')
					{
					// save potential line break information
					LineBreakWidth = LineWidth;
					BreakSegIndex = SegArray.Count - 1;
					BreakPtr = Seg.Text.Length + TextPtr - TextStart;
					BreakWidth = Seg.SegWidth;
					}

				// add to line width
				LineWidth += CharWidth;
				Seg.SegWidth += CharWidth;

				// update last character
				PrevChar = CurChar;
				continue;
				}

			// add current segment width and to overall line width
			Seg.SegWidth += CharWidth;
			LineWidth += CharWidth;

			// for next loop set last character
			PrevChar = CurChar;

			Double Width = _BoxWidth;
			if(_FirstLineIndent != 0 && (LineArray.Count == 0 || LineArray[LineArray.Count - 1].EndOfParagraph)) Width -= _FirstLineIndent;

			// current line width is less than or equal box width
			if(LineWidth <= Width) continue;

			// append text to current segemnt
			Seg.Text += Text.Substring(TextStart, TextPtr - TextStart + 1);
			TextStart = TextPtr + 1;

			// there are no breaks in this line or last segment is too long
			if(LineBreakWidth < LineBreakFactor * Width)
				{
				BreakSegIndex = SegArray.Count - 1;
				BreakPtr = Seg.Text.Length - 1;
				BreakWidth = Seg.SegWidth - CharWidth;
				}

			// break line
			BreakLine();

			// add line up to break point
			AddLine(false);
			}

		// save text
		Seg.Text += Text.Substring(TextStart);

		// exit
		return;
		}

	private void BreakLine()
		{
		// break segment at line break seg index into two segments
		TextSeg BreakSeg = SegArray[BreakSegIndex];

		// add extra segment to segment array
		if(BreakPtr != 0)
			{
			TextSeg ExtraSeg = new TextSeg(BreakSeg);
			ExtraSeg.SegWidth = BreakWidth;
			ExtraSeg.Text = BreakSeg.Text.Substring(0, BreakPtr);
			SegArray.Insert(BreakSegIndex, ExtraSeg);
			BreakSegIndex++;
			}

		// remove blanks from the area between the two sides of the segment
		for(; BreakPtr < BreakSeg.Text.Length && BreakSeg.Text[BreakPtr] == ' '; BreakPtr++);

		// save the area after the first line
		if(BreakPtr < BreakSeg.Text.Length)
			{
			BreakSeg.Text = BreakSeg.Text.Substring(BreakPtr);
			BreakSeg.SegWidth = BreakSeg.Font.TextWidth(BreakSeg.FontSize, BreakSeg.Text);
			}
		else
			{
			BreakSeg.Text = String.Empty;
			BreakSeg.SegWidth = 0.0;
			}
		BreakPtr = 0;
		BreakWidth = 0.0;
		return;
		}

	private void AddLine
			(
			Boolean		EndOfParagraph
			)
		{
		// end of paragraph
		if(EndOfParagraph) BreakSegIndex = SegArray.Count;

		// test for possible trailing blanks
		if(SegArray[BreakSegIndex - 1].Text.EndsWith(" "))
			{
			// remove trailing blanks
			while(BreakSegIndex > 0)
				{
				TextSeg TempSeg = SegArray[BreakSegIndex - 1];
				TempSeg.Text = TempSeg.Text.TrimEnd(new Char[] {' '});
				TempSeg.SegWidth = TempSeg.Font.TextWidth(TempSeg.FontSize, TempSeg.Text);
				if(TempSeg.Text.Length != 0 || BreakSegIndex == 1 && EndOfParagraph) break;
				BreakSegIndex--;
				SegArray.RemoveAt(BreakSegIndex);
				}
			}

		// test for abnormal case of a blank line and not end of paragraph
		if(BreakSegIndex > 0)
			{
			// allocate segment array
			TextSeg[] LineSegArray = new TextSeg[BreakSegIndex];

			// copy segments
			SegArray.CopyTo(0, LineSegArray, 0, BreakSegIndex);

			// line ascent and descent
			Double LineAscent = 0;
			Double LineDescent = 0;

			// loop for segments until line break segment index
			foreach(TextSeg Seg in LineSegArray)
				{
				Double Ascent = Seg.Font.AscentPlusLeading(Seg.FontSize);
				if(Ascent > LineAscent) LineAscent = Ascent;
				Double Descent = Seg.Font.DescentPlusLeading(Seg.FontSize);
				if(Descent > LineDescent) LineDescent = Descent;

				Int32 SpaceCount = 0;
				foreach(Char Chr in Seg.Text) if(Chr == ' ') SpaceCount++;
				Seg.SpaceCount = SpaceCount;
				}

			// add line
			LineArray.Add(new TextLine(LineAscent, LineDescent, EndOfParagraph, LineSegArray));

			// update column height
			_BoxHeight += LineAscent + LineDescent;

			// update paragraph count
			if(EndOfParagraph) _ParagraphCount++;

			// remove segments
			SegArray.RemoveRange(0, BreakSegIndex);
			}

		// switch to next line
		LineBreakWidth = 0.0;
		BreakSegIndex = 0;

		// new line width
		LineWidth = 0.0;
		foreach(TextSeg Seg in SegArray) LineWidth += Seg.SegWidth;
		return;
		}
	}
}
