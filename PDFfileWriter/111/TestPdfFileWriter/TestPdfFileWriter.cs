/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	TestPdfFileWriter main program
//	This is the test program of the PDF file writer C# Class Library
//  project. It is a windows form class. It allows the operator to
//  create two PDF files and to list all Truetype fonts available.
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
using System.Windows.Forms;
using PdfFileWriter;

namespace TestPdfFileWriter
{
public partial class TestPdfFileWriter : Form
	{
    public TestPdfFileWriter()
        {
        InitializeComponent();
        }

    private void OnLoad
			(
			object sender,
			EventArgs e
			)
		{
		// open trace file
		Trace.Open("PdfFileWriterTrace.txt");

		// program title
		Text = "TestPdfFileWriter - Revision " + PdfDocument.RevisionNumber + " " + PdfDocument.RevisionDate + " - \u00a9 2013 Granotech Limited";
		Trace.Write(Text);

		// C# class library revision
		Trace.Write(String.Format("PdfFileWriter C# class library - Revision {0} {1} - \u00a9 2013 Granotech Limited",
			PdfDocument.RevisionNumber, PdfDocument.RevisionDate));

		// copyright box
		CopyrightTextBox.Rtf =
			"{\\rtf1\\ansi\\deff0\\deftab720{\\fonttbl{\\f0\\fswiss\\fprq2 Verdana;}}" +
			"\\par\\plain\\fs24\\b PdfFileWriter\\plain \\fs20 \\par\\par \n" +
			"PDF File Writer C# class library.\\par \n" +
			"Create PDF files directly from your .net application.\\par\\par \n" +
			"Revision Number: " + PdfDocument.RevisionNumber + "\\par \n" +
			"Revision Date: " + PdfDocument.RevisionDate + "\\par \n" +
			"Author: Uzi Granot\\par\\par \n" +
			"Copyright \u00a9 2013 Granotech Limited. All rights reserved.\\par\\par \n" +
			"Free software distributed under the Code Project Open License (CPOL) 1.02.\\par \n" +
			"As per PdfFileWriterReadmeAndLicense.pdf file attached to this distribution.\\par \n" +
			"You must read and agree with the terms specified to use this program.}";

		// exit
		return;
		}


	private void OnArticleExample
			(
			object sender,
			EventArgs e
			)
		{
		try
			{
			ArticleExample AE = new ArticleExample();
			AE.Test(DebugCheckBox.Checked, "ArticleExample.pdf");
			return;
			}

		catch (Exception Ex)
			{
			// error exit
			String[] ExceptionStack = ExceptionReport.GetMessageAndStack(Ex);
			MessageBox.Show(this, "PDF Document creation falied\n" + ExceptionStack[0] + "\n" + ExceptionStack[1],
				"PDFDocument Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
			}
		}

	private void OnOtherExample
			(
			object sender,
			EventArgs e
			)
		{
		try
			{
			OtherExample GE = new OtherExample();
			GE.Test(DebugCheckBox.Checked, "OtherExample.pdf");
			return;
			}

		catch (Exception Ex)
			{
			// error exit
			String[] ExceptionStack = ExceptionReport.GetMessageAndStack(Ex);
			MessageBox.Show(this, "PDF Document creation falied\n" + ExceptionStack[0] + "\n" + ExceptionStack[1],
				"PDFDocument Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
			}
		}

	private void OnFontFamilies
			(
			object sender,
			EventArgs e
			)
		{
		EnumFontFamilies Dialog = new EnumFontFamilies();
		Dialog.ShowDialog();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Program is closing
	////////////////////////////////////////////////////////////////////

	private void OnClosing
			(
			object sender,
			FormClosingEventArgs e
			)
		{
		Trace.Write("PDF file writer is closing");
		return;
		}
	}
}
