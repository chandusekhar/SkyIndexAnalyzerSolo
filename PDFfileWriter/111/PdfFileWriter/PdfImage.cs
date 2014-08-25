/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfImage
//	PDF Image resource.
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
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace PdfFileWriter
{
public class PdfImage : PdfObject
	{
	public  Double			WidthPt;
	public  Double			HeightPt;
	public	Int32			WidthPix;
	public	Int32			HeightPix;
	private String			ImageFileName;
	private Boolean			ImageFormatJpeg;

	////////////////////////////////////////////////////////////////////
	// Constructor
	////////////////////////////////////////////////////////////////////

	public PdfImage
			(
			PdfDocument		Document,
			String			ImageFileName
			) : base(Document, true, "/XObject")
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('I');

		// save image file name
		this.ImageFileName = ImageFileName;

		// test exitance
		if(!File.Exists(ImageFileName)) throw new ApplicationException("Image file " + ImageFileName + " does not exist");

		// get file length
		FileInfo FI = new FileInfo(ImageFileName);
		Int64 ImageFileLength = FI.Length;
		if(ImageFileLength >= 0x40000000) throw new ApplicationException("Image file " + ImageFileName + " too long");

		// image width and height in pixels
		WidthPix = 0;
		HeightPix = 0;
		ImageFormatJpeg = false;

		try
			{
			// load the image
			Bitmap Picture = new Bitmap(ImageFileName);

			// get image width and height in pixels
			WidthPix = Picture.Width;
			HeightPix = Picture.Height;

			// image width and height in points
			WidthPt = (Double) WidthPix * 72.0 / Picture.HorizontalResolution;
			HeightPt = (Double) HeightPix * 72.0 / Picture.VerticalResolution;

			// make sure we have a JPEG picture
			if(Picture.RawFormat.Equals(ImageFormat.Jpeg)) ImageFormatJpeg = true;

			// we do not need the picture
			Picture.Dispose();
			}

		catch(ArgumentException)
			{
			throw new ApplicationException("Invalid image file: " + ImageFileName);
			}

		// add items to dictionary
		AddToDictionary("/Subtype", "/Image");
		AddToDictionary("/Width", WidthPix.ToString());
		AddToDictionary("/Height", HeightPix.ToString());
		AddToDictionary("/Filter", "/DCTDecode");
		AddToDictionary("/ColorSpace", "/DeviceRGB");
		AddToDictionary("/BitsPerComponent", "8");
		AddToDictionary("/Length", ImageFileLength.ToString());

		// clear memory
		GC.Collect();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Calculate height from width to preserve aspect ratio
	////////////////////////////////////////////////////////////////////

	private Double HeightFromWidth
			(
			Double Width
			)
		{
		return(Width * HeightPt / WidthPt);
		}

	////////////////////////////////////////////////////////////////////
	// Calculate width from height to preserve aspect ratio
	////////////////////////////////////////////////////////////////////

	private Double WidthFromHeight
			(
			Double Height
			)
		{
		return(Height * WidthPt / HeightPt);
		}

	////////////////////////////////////////////////////////////////////
	// Calculate best fit to preserve aspect ratio
	////////////////////////////////////////////////////////////////////

	public SizeD ImageSize
			(
			SizeD InputSize
			)
		{
		SizeD OutputSize = new SizeD();
		OutputSize.Height = HeightFromWidth(InputSize.Width);
		if(OutputSize.Height <= InputSize.Height)
			{
			OutputSize.Width = InputSize.Width;
			}
		else
			{
			OutputSize.Width = WidthFromHeight(InputSize.Height);
			OutputSize.Height = InputSize.Height;
			}
		return(OutputSize);
		}

	////////////////////////////////////////////////////////////////////
	// Calculate best fit to preserve aspect ratio
	////////////////////////////////////////////////////////////////////

	public SizeD ImageSize
			(
			Double	Width,
			Double	Height
			)
		{
		SizeD OutputSize = new SizeD();
		OutputSize.Height = HeightFromWidth(Width);
		if(OutputSize.Height <= Height)
			{
			OutputSize.Width = Width;
			}
		else
			{
			OutputSize.Width = WidthFromHeight(Height);
			OutputSize.Height = Height;
			}
		return(OutputSize);
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

	internal override void WriteObjectToPdfFile
			(
			BinaryWriter PdfFile
			)
		{
		// write object header
		PdfFile.Write(Encoding.ASCII.GetBytes(String.Format("{0} 0 obj\n", ObjectNumber)));

		// write dictionary
		DictionaryToPdfFile(PdfFile);

		// output stream
		PdfFile.Write(Encoding.ASCII.GetBytes("stream\n"));

		// debug
		if(Document.Debug)
			{
			PdfFile.Write(Encoding.ASCII.GetBytes("*** IMAGE PLACE HOLDER ***"));
			}

		// copy image file to output file
		else
			{
			String TempFileName = ImageFileName;
			if(!ImageFormatJpeg)
				{
				TempFileName = ImageFileName.Substring(0, ImageFileName.Length - 4) + "~temp~.jpg";
				Bitmap Picture = new Bitmap(ImageFileName);
				Picture.Save(TempFileName, ImageFormat.Jpeg);
				Picture.Dispose();
				}

			// convert stream to binary writer
			using(BinaryReader ImageFile =
				new BinaryReader(new FileStream(TempFileName, FileMode.Open, FileAccess.Read, FileShare.None), Encoding.UTF8))
				{
				Int32 FileLen = (Int32) ImageFile.BaseStream.Length;
				Int32 BufSize = 0x10000;
				if(FileLen < BufSize) BufSize = FileLen;
				Byte[] Buffer = new Byte[BufSize];
				while(FileLen > 0)
					{
					Int32 Len = ImageFile.Read(Buffer, 0, Buffer.Length);
					PdfFile.Write(Buffer, 0, Len);
					FileLen -= Len;
					}
				}

			// delete temp file
			if(TempFileName != ImageFileName) File.Delete(TempFileName);
			}

		// output stream
		PdfFile.Write(Encoding.ASCII.GetBytes("\nendstream\n"));

		// output object trailer
		PdfFile.Write(Encoding.ASCII.GetBytes("endobj\n"));
		return;
		}
	}
}
