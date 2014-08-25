/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	Geometry
//	Double precision drawing support functions.
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

namespace PdfFileWriter
{
/////////////////////////////////////////////////////////////////////
// point in double precision
/////////////////////////////////////////////////////////////////////

public class PointD
	{
	public Double	X;
	public Double	Y;

	public PointD
			(
			Double	X,
			Double	Y
			)
		{
		this.X = X;
		this.Y = Y;
		return;
		}

	public PointD
			(
			PointD	Center,
			Double	Radius,
			Double	Alpha
			)
		{
		this.X = Center.X + Radius * Math.Cos(Alpha);
		this.Y = Center.Y + Radius * Math.Sin(Alpha);
		return;
		}

	public PointD
			(
			Double	CenterX,
			Double	CenterY,
			Double	Radius,
			Double	Alpha
			)
		{
		this.X = CenterX + Radius * Math.Cos(Alpha);
		this.Y = CenterY + Radius * Math.Sin(Alpha);
		return;
		}

	public PointD
			(
			LineD	L1,
			LineD	L2
			)
		{
		Double L1DX = L1.DX;
		Double L1DY = L1.DY;
		Double L2DX = L2.DX;
		Double L2DY = L2.DY;

		Double Denom = L1DX * L2DY - L1DY * L2DX;
		if(Denom == 0.0)
			{
			X = Double.NaN;
			Y = Double.NaN;
			return;
			}

		X = (L1.DXY * L2.DX - L2.DXY * L1.DX) / Denom;
		Y = (L1.DXY * L2.DY - L2.DXY * L1.DY) / Denom;
		return;
		}

	public Double Distance 
			(
			PointD	Other
			)
		{
		return((new LineD(this, Other)).Length);
		}
	}

/////////////////////////////////////////////////////////////////////
// size in double precision
/////////////////////////////////////////////////////////////////////

public class SizeD
	{
	public Double	Width;
	public Double	Height;

	public SizeD() {}

	public SizeD
			(
			Double	Width,
			Double	Height
			)
		{
		this.Width = Width;
		this.Height = Height;
		return;
		}
	}

/////////////////////////////////////////////////////////////////////
// line in double precision
/////////////////////////////////////////////////////////////////////

public class LineD
	{
	public PointD	P1;
	public PointD	P2;

	public LineD
			(
			PointD	P1,
			PointD	P2
			)
		{
		this.P1 = P1;
		this.P2 = P2;
		return;
		}

	public LineD
			(
			Double		X1,
			Double		Y1,
			Double		X2,
			Double		Y2
			)
		{
		this.P1 = new PointD(X1, Y1);
		this.P2 = new PointD(X2, Y2);
		return;
		}

	public Double DX {get {return(P2.X - P1.X);}}
	public Double DY {get {return(P2.Y - P1.Y);}}
	public Double DXY {get {return(P2.X * P1.Y - P2.Y * P1.X);}}
	public Double Length {get {return(Math.Sqrt(DX * DX + DY * DY));}}
	public Double Alpha {get {return(Math.Atan2(DY, DX));}}
	public Double AlphaDeg {get {return(Alpha * 180.0 / Math.PI);}}
	}

/////////////////////////////////////////////////////////////////////
// Bezier in double precision
/////////////////////////////////////////////////////////////////////

public class BezierD
	{
	public PointD	P1;
	public PointD	P2;
	public PointD	P3;
	public PointD	P4;

	private static Double	CircleFactor = (Math.Sqrt(2.0) - 1) / 0.75;

	public BezierD
			(
			PointD	P1,
			PointD	P2,
			PointD	P3,
			PointD	P4
			)
		{
		this.P1 = P1;
		this.P2 = P2;
		this.P3 = P3;
		this.P4 = P4;
		return;
		}

	public BezierD
			(
			Double		X1,
			Double		Y1,
			Double		X2,
			Double		Y2,
			Double		X3,
			Double		Y3,
			Double		X4,
			Double		Y4
			)
		{
		this.P1 = new PointD(X1, Y1);
		this.P2 = new PointD(X2, Y2);
		this.P3 = new PointD(X3, Y3);
		this.P4 = new PointD(X4, Y4);
		return;
		}

	public BezierD
			(
			PointD	P1,
			Double	Factor2,
			Double	Alpha2,
			Double	Factor3,
			Double	Alpha3,
			PointD	P4
			)
		{
		// save two end points
		this.P1 = P1;
		this.P4 = P4;

		// distance between end points
		LineD Line = new LineD(P1, P4);
		Double Length = Line.Length;
		if(Length == 0)
			{
			P2 = P1;
			P3 = P4;
			return;
			}

		// angle of line between end points
		Double Alpha = Line.Alpha;

		this.P2 = new PointD(P1, Factor2 * Length, Alpha + Alpha2);
		this.P3 = new PointD(P4, Factor3 * Length, Alpha + Alpha3);
		return;
		}

	public static BezierD CircleFirstQuarter
			(
			Double		X,
			Double		Y,
			Double		Radius
			)
		{
		return(new BezierD(X + Radius, Y, X + Radius, Y + CircleFactor * Radius, X + CircleFactor * Radius, Y + Radius, X, Y + Radius));
		}

	public static BezierD CircleSecondQuarter
			(
			Double		X,
			Double		Y,
			Double		Radius
			)
		{
		return(new BezierD(X, Y + Radius, X - CircleFactor * Radius, Y + Radius, X - Radius, Y + CircleFactor * Radius, X - Radius, Y));
		}

	public static BezierD CircleThirdQuarter
			(
			Double		X,
			Double		Y,
			Double		Radius
			)
		{
		return(new BezierD(X - Radius, Y, X - Radius, Y - CircleFactor * Radius, X - CircleFactor * Radius, Y - Radius, X, Y - Radius));
		}

	public static BezierD CircleFourthQuarter
			(
			Double		X,
			Double		Y,
			Double		Radius
			)
		{
		return(new BezierD(X, Y - Radius, X +CircleFactor * Radius, Y - Radius, X + Radius, Y - CircleFactor * Radius, X + Radius, Y));
		}

	public static BezierD OvalFirstQuarter
			(
			Double		X,
			Double		Y,
			Double		Width,
			Double		Height
			)
		{
		return(new BezierD(X + Width, Y, X + Width, Y + CircleFactor * Height, X + CircleFactor * Width, Y + Height, X, Y + Height));
		}

	public static BezierD OvalSecondQuarter
			(
			Double		X,
			Double		Y,
			Double		Width,
			Double		Height
			)
		{
		return(new BezierD(X, Y + Height, X - CircleFactor * Width, Y + Height, X - Width, Y + CircleFactor * Height, X - Width, Y));
		}

	public static BezierD OvalThirdQuarter
			(
			Double		X,
			Double		Y,
			Double		Width,
			Double		Height
			)
		{
		return(new BezierD(X - Width, Y, X - Width, Y - CircleFactor * Height, X - CircleFactor * Width, Y - Height, X, Y - Height));
		}

	public static BezierD OvalFourthQuarter
			(
			Double		X,
			Double		Y,
			Double		Width,
			Double		Height
			)
		{
		return(new BezierD(X, Y - Height, X +CircleFactor * Width, Y - Height, X + Width, Y - CircleFactor * Height, X + Width, Y));
		}
	}
}
