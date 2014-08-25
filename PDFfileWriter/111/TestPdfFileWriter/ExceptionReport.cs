/////////////////////////////////////////////////////////////////////
//
//	TestPdfFileWriter
//	Test/demo program for PdfFileWrite C# Class Library.
//
//	ExceptionReport
//	Support class used in conjunction with try/catch operator.
//  The class saves in a trace file the calling stack at the
//  time of program exception.
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

namespace TestPdfFileWriter
{
public static class ExceptionReport
	{
	/////////////////////////////////////////////////////////////////////
	// Get exception message and exception stack
	/////////////////////////////////////////////////////////////////////

	public static String[] GetMessageAndStack
			(
			Exception		Ex
			)
		{
		// get system stack at the time of exception
		String StackTraceStr = Ex.StackTrace;

		// break it into individual lines
		String[] StackTraceLines = StackTraceStr.Split(new Char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

		// create a new array of trace lines
		List<String> StackTrace = new List<String>();

		// exception error message
		StackTrace.Add(Ex.Message);
		Trace.Write(Ex.Message);

		// add trace lines
		foreach(String Line in StackTraceLines) if(Line.Contains("PdfFileWriter"))
			{
			StackTrace.Add(Line);
			Trace.Write(Line);
			}

		// error exit
		return(StackTrace.ToArray());
		}
	}
}
