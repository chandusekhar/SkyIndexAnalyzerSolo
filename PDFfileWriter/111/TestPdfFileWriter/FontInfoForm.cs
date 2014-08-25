/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	FontInfoForm
//	This class displays all characters available for a given font.
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
using System.Windows.Forms;
using PdfFileWriter;

namespace TestPdfFileWriter
{
////////////////////////////////////////////////////////////////////
// Display metrics for characters range
////////////////////////////////////////////////////////////////////

public partial class FontInfoForm : Form
	{
	private FontFamily			FontFamily;
	private FontStyle			Style;
	private Font				DesignFont;
	private FontApi				FontInfo;
	private Int32				DesignHeight;
	private Int32				FirstChar;
	private Int32				LastChar;
	private Int16[]				GlyphCode;
	private CustomDataGridView	DataGrid;

	// Data grid view columns
	private enum FontInfoColumn
		{
		CharCode,
		GlyphCode,
		Glyph,
		CharWidth,
		GlyphBoxLeft,
		GlyphBoxBottom,
		GlyphBoxRight,
		GlyphBoxTop,
		}

	////////////////////////////////////////////////////////////////////
	// Font info form constructor
	////////////////////////////////////////////////////////////////////

	public FontInfoForm
			(
			FontFamily FontFamily
			)
		{
		this.FontFamily = FontFamily;
		InitializeComponent();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Font info form initialization
	////////////////////////////////////////////////////////////////////

	private void OnLoad
			(
			object sender,
			EventArgs e
			)
		{
		// wait coursor
		Cursor SaveCursor = Cursor;
		Cursor = Cursors.WaitCursor;
		ViewButton.Enabled = false;
		ExitButton.Enabled = false;

		// try regulaar, bold, italic and bold-italic
		Int32 StyleIndex;
		for(StyleIndex = 0; StyleIndex < 4 && !FontFamily.IsStyleAvailable((FontStyle) StyleIndex); StyleIndex++);
		Style = (FontStyle) StyleIndex;

		// design height
		DesignHeight = FontFamily.GetEmHeight(Style);

		// create font
		DesignFont = new Font(FontFamily, DesignHeight, Style, GraphicsUnit.Pixel);

		// create windows sdk font info object
		FontInfo = new FontApi(DesignFont, DesignHeight);

		// get outline text metrics structure
		WinOutlineTextMetric OTM = FontInfo.GetOutlineTextMetricsApi();

		// first and last character available
		FirstChar = OTM.otmTextMetric.tmFirstChar;
		LastChar = OTM.otmTextMetric.tmLastChar;

		// get font and glyph information
		GlyphCode = FontInfo.GetGlyphIndicesApi(FirstChar, LastChar);

		// create and load data grid
		DataGrid = new CustomDataGridView(this, true);
		DataGrid.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(OnMouseDoubleClick);

		// add columns
		DataGrid.Columns.Add("CharCode", "Char\nCode");
		DataGrid.Columns.Add("GlyphCode", "Glyph\nCode");
		DataGrid.Columns.Add("Glyph", "Glyph");
		DataGrid.Columns.Add("AdvanceWidth", "Advance\nWidth");
		DataGrid.Columns.Add("BBoxLeft", "BBox\nLeft");
		DataGrid.Columns.Add("BBoxBottom", "BBox\nBottom");
		DataGrid.Columns.Add("BBoxRight", "BBox\nRight");
		DataGrid.Columns.Add("BBoxTop", "BBox\nTop");

		// change the font for the display of the character to the current font
		DataGridViewCellStyle GlyphCellStyle = new DataGridViewCellStyle(DataGrid.DefaultCellStyle);
		GlyphCellStyle.Font = new Font(FontFamily, 12.0F, Style);
		DataGrid.Columns[(Int32) FontInfoColumn.Glyph].DefaultCellStyle = GlyphCellStyle;

		// load one row at a time to data grid
		for(Int32 CharPtr = 0; CharPtr < GlyphCode.Length; CharPtr++)
			{
			if(GlyphCode[CharPtr] == 0) continue;
			LoadDataGridRow(FirstChar + CharPtr, GlyphCode[CharPtr], FontInfo.GetGlyphMetricsApi((Char) (FirstChar + CharPtr)));
			}

		// select first row
		if(DataGrid.Rows.Count > 0) DataGrid.Rows[0].Selected = true;
		OnResize(null, null);

		// normal cursor
		Cursor = SaveCursor;
		ViewButton.Enabled = true;
		ExitButton.Enabled = true;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Load one row of data grid view
	////////////////////////////////////////////////////////////////////

	private void LoadDataGridRow
			(
			Int32		CharCode,
			Int32		GlyphCode,
			BoundingBox Box
			)
		{
		// data grid row
		Int32 Row = DataGrid.Rows.Add();
		DataGridViewRow ViewRow = DataGrid.Rows[Row];
		ViewRow.Tag = CharCode;

		// set value of each column
		ViewRow.Cells[(Int32) FontInfoColumn.CharCode].Value = CharCode;
		ViewRow.Cells[(Int32) FontInfoColumn.GlyphCode].Value = GlyphCode;
		ViewRow.Cells[(Int32) FontInfoColumn.Glyph].Value = (Char) CharCode;
		ViewRow.Cells[(Int32) FontInfoColumn.CharWidth].Value = (Int32) (1000.0 * Box.Width);
		ViewRow.Cells[(Int32) FontInfoColumn.GlyphBoxLeft].Value = (Int32) (1000.0 * Box.Rect.Left);
		ViewRow.Cells[(Int32) FontInfoColumn.GlyphBoxBottom].Value = (Int32) (1000.0 * Box.Rect.Bottom);
		ViewRow.Cells[(Int32) FontInfoColumn.GlyphBoxRight].Value = (Int32) (1000.0 * Box.Rect.Right);
		ViewRow.Cells[(Int32) FontInfoColumn.GlyphBoxTop].Value = (Int32) (1000.0 * Box.Rect.Top);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// user double click a line to display character
	////////////////////////////////////////////////////////////////////

	private void OnMouseDoubleClick
			(
			object sender,
			DataGridViewCellMouseEventArgs e
			)
		{
		if(e.Button == MouseButtons.Left && e.RowIndex >= 0) OnView(sender, e);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// user pressed view button to display character
	////////////////////////////////////////////////////////////////////

	private void OnView
			(
			object sender,
			EventArgs e
			)
		{
		// get character selected by user
		Object CharCode = DataGrid.GetSelection();
		if(CharCode == null) return;

		// create a dialog to display the character
		DrawGlyphForm Dialog = new DrawGlyphForm(FontFamily, Style, (Int32) CharCode);

		// display the character as series of lines and curves
		Dialog.ShowDialog(this);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// resize form
	////////////////////////////////////////////////////////////////////

	private void OnResize
			(
			object sender,
			EventArgs e
			)
		{
		// protect against minimize button
		if(ClientSize.Width == 0) return;

		// buttons
		ButtonsGroupBox.Left = (ClientSize.Width - ButtonsGroupBox.Width) / 2;
		ButtonsGroupBox.Top = ClientSize.Height - ButtonsGroupBox.Height - 4;

		// position datagrid
		if(DataGrid != null)
			{
			DataGrid.Left = 2;
			DataGrid.Top = 2;
			DataGrid.Width = ClientSize.Width - 4;
			DataGrid.Height = ButtonsGroupBox.Top - 4;
			}

		// exit
		return;
		}
	}
}
