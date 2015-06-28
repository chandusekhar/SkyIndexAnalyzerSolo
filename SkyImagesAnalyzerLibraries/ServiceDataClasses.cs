using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using SolarPositioning;


namespace SkyImagesAnalyzerLibraries
{
    public enum GPSdatasources
    {
        IOFFEvesselDataServer,
        CloudCamArduinoGPS
    }


    public class accelerometerData
    {
        private int accX;
        private int accY;
        private int accZ;
        private double accDoubleX;
        private double accDoubleY;
        private double accDoubleZ;
        private double accMagnitude;
        public int devID = 0;

        public bool validAccData = true;

        public int AccX
        {
            get { return accX; }
            set
            {
                accX = value;
                accDoubleX = (double)accX;
                //calculateMagnitude();
            }
        }

        public int AccY
        {
            get { return accY; }
            set
            {
                accY = value;
                accDoubleY = (double)accY;
                //calculateMagnitude();
            }
        }

        public int AccZ
        {
            get { return accZ; }
            set
            {
                accZ = value;
                accDoubleZ = (double)accZ;
                //calculateMagnitude();
            }
        }

        public double AccDoubleX
        {
            get { return accDoubleX; }
            set
            {
                accDoubleX = value;
                accX = Convert.ToInt32(Math.Round(accDoubleX, 0));
                //calculateMagnitude();
            }
        }

        public double AccDoubleY
        {
            get { return accDoubleY; }
            set
            {
                accDoubleY = value;
                accY = Convert.ToInt32(Math.Round(accDoubleY, 0));
                //calculateMagnitude();
            }
        }

        public double AccDoubleZ
        {
            get { return accDoubleZ; }
            set
            {
                accDoubleZ = value;
                accZ = Convert.ToInt32(Math.Round(accDoubleZ, 0));
                //calculateMagnitude();
            }
        }

        public double AccMagnitude
        {
            get
            {
                calculateMagnitude();
                return accMagnitude;
            }
        }

        public accelerometerData()
        {
            this.AccX = 0;
            this.AccY = 0;
            this.AccZ = 0;
        }
        public accelerometerData(int aX, int aY, int aZ)
        {
            this.AccX = aX;
            this.AccY = aY;
            this.AccZ = aZ;
        }
        public accelerometerData(double aX, double aY, double aZ)
        {
            this.AccDoubleX = aX;
            this.AccDoubleY = aY;
            this.AccDoubleZ = aZ;
        }


        public accelerometerData(IEnumerable<int> source)
        {
            if (source.Count() == 3)
            {
                try
                {
                    this.AccX = source.ElementAt(0);
                    this.AccY = source.ElementAt(1);
                    this.AccZ = source.ElementAt(2);
                }
                catch (Exception)
                {
                    this.AccX = 0;
                    this.AccY = 0;
                    this.AccZ = 0;
                    validAccData = false;
                }
            }
            else if (source.Count() == 4) // devID specified
            {
                try
                {
                    this.AccX = source.ElementAt(0);
                    this.AccY = source.ElementAt(1);
                    this.AccZ = source.ElementAt(2);
                    devID = Convert.ToInt32(source.ElementAt(3));
                }
                catch (Exception)
                {
                    this.AccX = 0;
                    this.AccY = 0;
                    this.AccZ = 0;
                    validAccData = false;
                }
            }
        }



        public accelerometerData(IEnumerable<double> source)
        {
            if (source.Count() == 3)
            {
                try
                {
                    this.AccDoubleX = source.ElementAt(0);
                    this.AccDoubleY = source.ElementAt(1);
                    this.AccDoubleZ = source.ElementAt(2);
                }
                catch (Exception)
                {
                    this.AccDoubleX = 0;
                    this.AccDoubleY = 0;
                    this.AccDoubleZ = 0;
                    validAccData = false;
                }
            }
            else if (source.Count() == 4) // devID specified
            {
                try
                {
                    this.AccDoubleX = source.ElementAt(0);
                    this.AccDoubleY = source.ElementAt(1);
                    this.AccDoubleZ = source.ElementAt(2);
                    devID = Convert.ToInt32(source.ElementAt(3));
                }
                catch (Exception)
                {
                    this.AccDoubleX = 0;
                    this.AccDoubleY = 0;
                    this.AccDoubleZ = 0;
                    validAccData = false;
                }
            }

        }




        public accelerometerData(IEnumerable<string> source)
        {
            if (source.Count() == 3)
            {
                try
                {
                    this.AccDoubleX = Convert.ToDouble(source.ElementAt(0));
                    this.AccDoubleY = Convert.ToDouble(source.ElementAt(1));
                    this.AccDoubleZ = Convert.ToDouble(source.ElementAt(2));
                }
                catch (Exception)
                {
                    this.AccDoubleX = 0.0d;
                    this.AccDoubleY = 0.0d;
                    this.AccDoubleZ = 0.0d;
                    validAccData = false;
                }
            }
            else if (source.Count() == 4) // devID specified
            {
                try
                {
                    this.AccDoubleX = Convert.ToDouble(source.ElementAt(0));
                    this.AccDoubleY = Convert.ToDouble(source.ElementAt(1));
                    this.AccDoubleZ = Convert.ToDouble(source.ElementAt(2));
                    devID = Convert.ToInt32(source.ElementAt(3));
                }
                catch (Exception)
                {
                    this.AccDoubleX = 0.0d;
                    this.AccDoubleY = 0.0d;
                    this.AccDoubleZ = 0.0d;
                    validAccData = false;
                }
            }

        }


        private void calculateMagnitude()
        {
            accMagnitude = Math.Sqrt(accDoubleX * accDoubleX + accDoubleY * accDoubleY + accDoubleZ * accDoubleZ);
        }

        public double xyProjectionMagnitude()
        {
            return (Math.Sqrt(accDoubleX * accDoubleX + accDoubleY * accDoubleY));
        }

        public double xzProjectionMagnitude()
        {
            return (Math.Sqrt(accDoubleX * accDoubleX + accDoubleZ * accDoubleZ));
        }

        public double yzProjectionMagnitude()
        {
            return (Math.Sqrt(accDoubleZ * accDoubleZ + accDoubleY * accDoubleY));
        }


        public static accelerometerData operator +(accelerometerData accVector1, accelerometerData accVector2)
        {
            accelerometerData retAccData = new accelerometerData();
            retAccData.AccDoubleX = accVector1.AccDoubleX + accVector2.AccDoubleX;
            retAccData.AccDoubleY = accVector1.AccDoubleY + accVector2.AccDoubleY;
            retAccData.AccDoubleZ = accVector1.AccDoubleZ + accVector2.AccDoubleZ;


            return retAccData;
        }



        public static accelerometerData operator -(accelerometerData accVector1, accelerometerData accVector2)
        {
            accelerometerData retAccData = new accelerometerData();
            retAccData.AccDoubleX = accVector1.AccDoubleX - accVector2.AccDoubleX;
            retAccData.AccDoubleY = accVector1.AccDoubleY - accVector2.AccDoubleY;
            retAccData.AccDoubleZ = accVector1.AccDoubleZ - accVector2.AccDoubleZ;


            return retAccData;
        }

        public static accelerometerData operator /(accelerometerData accVector1, double dValue)
        {
            accelerometerData retAccData = new accelerometerData();
            retAccData.AccDoubleX = accVector1.AccDoubleX / dValue;
            retAccData.AccDoubleY = accVector1.AccDoubleY / dValue;
            retAccData.AccDoubleZ = accVector1.AccDoubleZ / dValue;


            return retAccData;
        }


        public static double operator *(accelerometerData accVector1, accelerometerData accVector2)
        {
            double product = accVector1.AccDoubleX * accVector2.AccDoubleX +
                             accVector1.AccDoubleY * accVector2.AccDoubleY +
                             accVector1.AccDoubleZ * accVector2.AccDoubleZ;
            return product;
        }


        public static accelerometerData operator *(accelerometerData accVector1, double dValue)
        {
            accelerometerData retAccData = new accelerometerData();
            retAccData.AccDoubleX = accVector1.AccDoubleX * dValue;
            retAccData.AccDoubleY = accVector1.AccDoubleY * dValue;
            retAccData.AccDoubleZ = accVector1.AccDoubleZ * dValue;


            return retAccData;
        }



        public static accelerometerData operator *(double dValue, accelerometerData accVector1)
        {
            accelerometerData retAccData = new accelerometerData();
            retAccData.AccDoubleX = accVector1.AccDoubleX * dValue;
            retAccData.AccDoubleY = accVector1.AccDoubleY * dValue;
            retAccData.AccDoubleZ = accVector1.AccDoubleZ * dValue;


            return retAccData;
        }




        public static accelerometerData operator ^(accelerometerData accVector1, accelerometerData accVector2)
        {
            return VectorProduct(accVector1, accVector2);
        }


        public static accelerometerData VectorProduct(accelerometerData accVector1, accelerometerData accVector2)
        {
            accelerometerData resVect = new accelerometerData();
            resVect.AccDoubleX = accVector1.AccDoubleY * accVector2.AccDoubleZ -
                                 accVector1.AccDoubleZ * accVector2.AccDoubleY;
            resVect.AccDoubleY = -(accVector1.AccDoubleX * accVector2.AccDoubleZ -
                                 accVector1.AccDoubleZ * accVector2.AccDoubleX);
            resVect.AccDoubleZ = accVector1.AccDoubleX * accVector2.AccDoubleY -
                                 accVector1.AccDoubleY * accVector2.AccDoubleX;
            return resVect;
        }



        public DenseVector ToDenseVector()
        {
            DenseVector dvRetVect = DenseVector.Create(3, i =>
            {
                switch (i)
                {
                    case 0:
                        return accDoubleX;
                    case 1:
                        return accDoubleY;
                    case 2:
                        return accDoubleZ;
                    default:
                        return 0.0d;
                }
            });
            dvRetVect = DenseVector.OfEnumerable(dvRetVect.Concat(new double[] { devID }));
            return dvRetVect;
        }



        public static List<accelerometerData> OfDenseMatrix(DenseMatrix dmSource)
        {
            if (dmSource.RowCount == 0)
            {
                return null;
            }

            List<accelerometerData> listRetAccData = new List<accelerometerData>();

            foreach (Tuple<int, Vector<double>> tplRow in dmSource.EnumerateRowsIndexed())
            {
                accelerometerData newAccDatum = new accelerometerData(tplRow.Item2);
                if (newAccDatum != null)
                {
                    listRetAccData.Add(newAccDatum);
                }
            }

            return listRetAccData;
        }


        public accelerometerData Copy()
        {
            accelerometerData newInstance = new accelerometerData(accDoubleX, accDoubleY, accDoubleZ);
            newInstance.devID = devID;
            return newInstance;
        }
    }







    public class GyroData
    {
        private int gyroX;
        private int gyroY;
        private int gyroZ;
        private double gyroDoubleX;
        private double gyroDoubleY;
        private double gyroDoubleZ;
        private double gyroMagnitude;
        public int devID = 0;

        public int GyroX
        {
            get { return gyroX; }
            set
            {
                gyroX = value;
                gyroDoubleX = (double)gyroX;
                //calculateMagnitude();
            }
        }


        public int GyroY
        {
            get { return gyroY; }
            set
            {
                gyroY = value;
                gyroDoubleY = (double)gyroY;
                //calculateMagnitude();
            }
        }


        public int GyroZ
        {
            get { return gyroZ; }
            set
            {
                gyroZ = value;
                gyroDoubleZ = (double)gyroZ;
                //calculateMagnitude();
            }
        }


        public double GyroDoubleX
        {
            get { return gyroDoubleX; }
            set
            {
                gyroDoubleX = value;
                gyroX = Convert.ToInt32(Math.Round(gyroDoubleX, 0));
                //calculateMagnitude();
            }
        }

        public double GyroDoubleY
        {
            get { return gyroDoubleY; }
            set
            {
                gyroDoubleY = value;
                gyroY = Convert.ToInt32(Math.Round(gyroDoubleY, 0));
                //calculateMagnitude();
            }
        }

        public double GyroDoubleZ
        {
            get { return gyroDoubleZ; }
            set
            {
                gyroDoubleZ = value;
                gyroZ = Convert.ToInt32(Math.Round(gyroDoubleZ, 0));
                //calculateMagnitude();
            }
        }

        public double GyroMagnitude
        {
            get
            {
                calculateMagnitude();
                return gyroMagnitude;
            }
        }

        public GyroData()
        {
            this.GyroX = 0;
            this.GyroY = 0;
            this.GyroZ = 0;
        }
        public GyroData(int gyroX, int gyroY, int gyroZ)
        {
            this.GyroX = gyroX;
            this.GyroY = gyroY;
            this.GyroZ = gyroZ;
        }
        public GyroData(double gyroX, double gyroY, double gyroZ)
        {
            this.GyroDoubleX = gyroX;
            this.GyroDoubleY = gyroY;
            this.GyroDoubleZ = gyroZ;
        }

        public GyroData(IEnumerable<int> source)
        {
            try
            {
                this.GyroX = source.ElementAt(0);
                this.GyroY = source.ElementAt(1);
                this.GyroZ = source.ElementAt(2);
            }
            catch (Exception)
            {
                this.GyroX = 0;
                this.GyroY = 0;
                this.GyroZ = 0;
            }
        }

        public GyroData(IEnumerable<double> source)
        {
            try
            {
                this.GyroDoubleX = source.ElementAt(0);
                this.GyroDoubleY = source.ElementAt(1);
                this.GyroDoubleZ = source.ElementAt(2);
            }
            catch (Exception)
            {
                this.GyroDoubleX = 0.0d;
                this.GyroDoubleY = 0.0d;
                this.GyroDoubleZ = 0.0d;
            }

        }

        public GyroData(IEnumerable<string> source)
        {
            try
            {
                this.GyroDoubleX = Convert.ToDouble(source.ElementAt(0));
                this.GyroDoubleY = Convert.ToDouble(source.ElementAt(1));
                this.GyroDoubleZ = Convert.ToDouble(source.ElementAt(2));
            }
            catch (Exception)
            {
                this.GyroDoubleX = 0.0d;
                this.GyroDoubleY = 0.0d;
                this.GyroDoubleZ = 0.0d;
            }

        }



        private void calculateMagnitude()
        {
            gyroMagnitude = Math.Sqrt(gyroDoubleX * gyroDoubleX + gyroDoubleY * gyroDoubleY + gyroDoubleZ * gyroDoubleZ);
        }

        public double xyProjectionMagnitude()
        {
            return (Math.Sqrt(gyroDoubleX * gyroDoubleX + gyroDoubleY * gyroDoubleY));
        }

        public double xzProjectionMagnitude()
        {
            return (Math.Sqrt(gyroDoubleX * gyroDoubleX + gyroDoubleZ * gyroDoubleZ));
        }

        public double yzProjectionMagnitude()
        {
            return (Math.Sqrt(gyroDoubleZ * gyroDoubleZ + gyroDoubleY * gyroDoubleY));
        }



        public static GyroData operator +(GyroData gyroVector1, GyroData gyroVector2)
        {
            GyroData retgyroData = new GyroData();
            retgyroData.gyroDoubleX = gyroVector1.gyroDoubleX + gyroVector2.gyroDoubleX;
            retgyroData.gyroDoubleY = gyroVector1.gyroDoubleY + gyroVector2.gyroDoubleY;
            retgyroData.gyroDoubleZ = gyroVector1.gyroDoubleZ + gyroVector2.gyroDoubleZ;


            return retgyroData;
        }



        public static GyroData operator -(GyroData gyroVector1, GyroData gyroVector2)
        {
            GyroData retgyroData = new GyroData();
            retgyroData.gyroDoubleX = gyroVector1.gyroDoubleX - gyroVector2.gyroDoubleX;
            retgyroData.gyroDoubleY = gyroVector1.gyroDoubleY - gyroVector2.gyroDoubleY;
            retgyroData.gyroDoubleZ = gyroVector1.gyroDoubleZ - gyroVector2.gyroDoubleZ;


            return retgyroData;
        }

        public static GyroData operator /(GyroData gyroVector1, double dValue)
        {
            GyroData retGyroData = new GyroData();
            retGyroData.GyroDoubleX = gyroVector1.GyroDoubleX / dValue;
            retGyroData.GyroDoubleY = gyroVector1.GyroDoubleY / dValue;
            retGyroData.GyroDoubleZ = gyroVector1.GyroDoubleZ / dValue;


            return retGyroData;
        }


        public static double operator *(GyroData gyroVector1, GyroData gyroVector2)
        {
            double product = gyroVector1.GyroDoubleX * gyroVector2.GyroDoubleX +
                             gyroVector1.GyroDoubleY * gyroVector2.GyroDoubleY +
                             gyroVector1.GyroDoubleZ * gyroVector2.GyroDoubleZ;
            return product;
        }


        public static GyroData operator *(GyroData gyroVector1, double dValue)
        {
            GyroData retGyroData = new GyroData();
            retGyroData.GyroDoubleX = gyroVector1.GyroDoubleX * dValue;
            retGyroData.GyroDoubleY = gyroVector1.GyroDoubleY * dValue;
            retGyroData.GyroDoubleZ = gyroVector1.GyroDoubleZ * dValue;


            return retGyroData;
        }



        public static GyroData operator *(double dValue, GyroData gyroVector1)
        {
            GyroData retGyroData = new GyroData();
            retGyroData.GyroDoubleX = gyroVector1.GyroDoubleX * dValue;
            retGyroData.GyroDoubleY = gyroVector1.GyroDoubleY * dValue;
            retGyroData.GyroDoubleZ = gyroVector1.GyroDoubleZ * dValue;


            return retGyroData;
        }



        public static GyroData operator ^(GyroData gyroVector1, GyroData gyroVector2)
        {
            return VectorProduct(gyroVector1, gyroVector2);
        }



        public static GyroData VectorProduct(GyroData gyroVector1, GyroData gyroVector2)
        {
            GyroData resVect = new GyroData();
            resVect.GyroDoubleX = gyroVector1.GyroDoubleY * gyroVector2.GyroDoubleZ -
                                 gyroVector1.GyroDoubleZ * gyroVector2.GyroDoubleY;
            resVect.GyroDoubleY = -(gyroVector1.GyroDoubleX * gyroVector2.GyroDoubleZ -
                                 gyroVector1.GyroDoubleZ * gyroVector2.GyroDoubleX);
            resVect.GyroDoubleZ = gyroVector1.GyroDoubleX * gyroVector2.GyroDoubleY -
                                 gyroVector1.GyroDoubleY * gyroVector2.GyroDoubleX;
            return resVect;
        }




        public DenseVector ToDenseVector()
        {
            DenseVector dvRetVect = DenseVector.Create(3, i =>
            {
                switch (i)
                {
                    case 0:
                        return gyroDoubleX;
                    case 1:
                        return gyroDoubleY;
                    case 2:
                        return gyroDoubleZ;
                    default:
                        return 0.0d;
                }
            });
            //dvRetVect.Add(devID);
            dvRetVect = DenseVector.OfEnumerable(dvRetVect.Concat(new double[] {devID}));
            return dvRetVect;
        }


        public DenseMatrix ToOneRowDenseMatrix()
        {
            DenseVector dvRow = ToDenseVector();
            List<DenseVector> listOfRows = new List<DenseVector>();
            listOfRows.Add(dvRow);
            DenseMatrix dmRetMatr = DenseMatrix.OfRows(1, dvRow.Count, listOfRows);
            return dmRetMatr;
        }



        public GyroData Copy()
        {
            GyroData newInstance = new GyroData(gyroDoubleX, gyroDoubleY, gyroDoubleZ);
            newInstance.devID = devID;
            return newInstance;
        }
    }


    public class MagnetometerData
    {
        private int magnX;
        private int magnY;
        private int magnZ;
        private double magnDoubleX;
        private double magnDoubleY;
        private double magnDoubleZ;
        private double magnMagnitude;
        public int devID = 0;

        public int MagnX
        {
            get { return magnX; }
            set
            {
                magnX = value;
                magnDoubleX = (double)magnX;
                //calculateMagnitude();
            }
        }

        public int MagnY
        {
            get { return magnY; }
            set
            {
                magnY = value;
                magnDoubleY = (double)magnY;
                //calculateMagnitude();
            }
        }

        public int MagnZ
        {
            get { return magnZ; }
            set
            {
                magnZ = value;
                magnDoubleZ = (double)magnZ;
                //calculateMagnitude();
            }
        }

        public double MagnDoubleX
        {
            get { return magnDoubleX; }
            set
            {
                magnDoubleX = value;
                magnX = Convert.ToInt32(Math.Round(magnDoubleX, 0));
                //calculateMagnitude();
            }
        }

        public double MagnDoubleY
        {
            get { return magnDoubleY; }
            set
            {
                magnDoubleY = value;
                magnY = Convert.ToInt32(Math.Round(magnDoubleY, 0));
                //calculateMagnitude();
            }
        }

        public double MagnDoubleZ
        {
            get { return magnDoubleZ; }
            set
            {
                magnDoubleZ = value;
                magnZ = Convert.ToInt32(Math.Round(magnDoubleZ, 0));
                //calculateMagnitude();
            }
        }

        public double MagnMagnitude
        {
            get
            {
                calculateMagnitude();
                return magnMagnitude;
            }
        }

        public MagnetometerData()
        {
            this.MagnX = 0;
            this.MagnY = 0;
            this.MagnZ = 0;
        }
        public MagnetometerData(int mX, int mY, int mZ)
        {
            this.MagnX = mX;
            this.MagnY = mY;
            this.MagnZ = mZ;
        }
        public MagnetometerData(double mX, double mY, double mZ)
        {
            this.MagnDoubleX = mX;
            this.MagnDoubleY = mY;
            this.MagnDoubleZ = mZ;
        }

        public MagnetometerData(IEnumerable<int> source)
        {
            try
            {
                this.MagnX = source.ElementAt(0);
                this.MagnY = source.ElementAt(1);
                this.MagnZ = source.ElementAt(2);
            }
            catch (Exception)
            {
                this.MagnX = 0;
                this.MagnY = 0;
                this.MagnZ = 0;
            }

        }

        public MagnetometerData(IEnumerable<double> source)
        {
            try
            {
                this.MagnDoubleX = source.ElementAt(0);
                this.MagnDoubleY = source.ElementAt(1);
                this.MagnDoubleZ = source.ElementAt(2);
            }
            catch (Exception)
            {
                this.MagnDoubleX = 0.0d;
                this.MagnDoubleY = 0.0d;
                this.MagnDoubleZ = 0.0d;
            }

        }

        public MagnetometerData(IEnumerable<string> source)
        {
            try
            {
                this.MagnDoubleX = Convert.ToDouble(source.ElementAt(0));
                this.MagnDoubleY = Convert.ToDouble(source.ElementAt(1));
                this.MagnDoubleZ = Convert.ToDouble(source.ElementAt(2));
            }
            catch (Exception)
            {
                this.MagnDoubleX = 0.0d;
                this.MagnDoubleY = 0.0d;
                this.MagnDoubleZ = 0.0d;
            }

        }



        private void calculateMagnitude()
        {
            magnMagnitude = Math.Sqrt(magnDoubleX * magnDoubleX + magnDoubleY * magnDoubleY + magnDoubleZ * magnDoubleZ);
        }

        public double xyProjectionMagnitude()
        {
            return (Math.Sqrt(magnDoubleX * magnDoubleX + magnDoubleY * magnDoubleY));
        }

        public double xzProjectionMagnitude()
        {
            return (Math.Sqrt(magnDoubleX * magnDoubleX + magnDoubleZ * magnDoubleZ));
        }

        public double yzProjectionMagnitude()
        {
            return (Math.Sqrt(magnDoubleZ * magnDoubleZ + magnDoubleY * magnDoubleY));
        }



        public static MagnetometerData operator +(MagnetometerData magnVector1, MagnetometerData magnVector2)
        {
            MagnetometerData retMagnData = new MagnetometerData();
            retMagnData.MagnDoubleX = magnVector1.MagnDoubleX + magnVector2.MagnDoubleX;
            retMagnData.MagnDoubleY = magnVector1.MagnDoubleY + magnVector2.MagnDoubleY;
            retMagnData.MagnDoubleZ = magnVector1.MagnDoubleZ + magnVector2.MagnDoubleZ;


            return retMagnData;
        }


        public static MagnetometerData operator -(MagnetometerData magnVector1, MagnetometerData magnVector2)
        {
            MagnetometerData retMagnData = new MagnetometerData();
            retMagnData.MagnDoubleX = magnVector1.MagnDoubleX - magnVector2.MagnDoubleX;
            retMagnData.MagnDoubleY = magnVector1.MagnDoubleY - magnVector2.MagnDoubleY;
            retMagnData.MagnDoubleZ = magnVector1.MagnDoubleZ - magnVector2.MagnDoubleZ;


            return retMagnData;
        }





        public static MagnetometerData operator /(MagnetometerData magnVector1, double dValue)
        {
            MagnetometerData retMagnData = new MagnetometerData();
            retMagnData.MagnDoubleX = magnVector1.MagnDoubleX / dValue;
            retMagnData.MagnDoubleY = magnVector1.MagnDoubleY / dValue;
            retMagnData.MagnDoubleZ = magnVector1.MagnDoubleZ / dValue;


            return retMagnData;
        }


        public static double operator *(MagnetometerData magnVector1, MagnetometerData magnVector2)
        {
            double product = magnVector1.MagnDoubleX * magnVector2.MagnDoubleX +
                             magnVector1.MagnDoubleY * magnVector2.MagnDoubleY +
                             magnVector1.MagnDoubleZ * magnVector2.MagnDoubleZ;
            return product;
        }


        public static MagnetometerData operator *(MagnetometerData magnVector1, double dValue)
        {
            MagnetometerData retMagnData = new MagnetometerData();
            retMagnData.MagnDoubleX = magnVector1.MagnDoubleX * dValue;
            retMagnData.MagnDoubleY = magnVector1.MagnDoubleY * dValue;
            retMagnData.MagnDoubleZ = magnVector1.MagnDoubleZ * dValue;


            return retMagnData;
        }



        public static MagnetometerData operator *(double dValue, MagnetometerData magnVector1)
        {
            MagnetometerData retMagnData = new MagnetometerData();
            retMagnData.MagnDoubleX = magnVector1.MagnDoubleX * dValue;
            retMagnData.MagnDoubleY = magnVector1.MagnDoubleY * dValue;
            retMagnData.MagnDoubleZ = magnVector1.MagnDoubleZ * dValue;


            return retMagnData;
        }



        public static MagnetometerData operator ^(MagnetometerData magnVector1, MagnetometerData magnVector2)
        {
            return VectorProduct(magnVector1, magnVector2);
        }



        public static MagnetometerData VectorProduct(MagnetometerData magnVector1, MagnetometerData magnVector2)
        {
            MagnetometerData resVect = new MagnetometerData();
            resVect.MagnDoubleX = magnVector1.MagnDoubleY * magnVector2.MagnDoubleZ -
                                 magnVector1.MagnDoubleZ * magnVector2.MagnDoubleY;
            resVect.MagnDoubleY = -(magnVector1.MagnDoubleX * magnVector2.MagnDoubleZ -
                                 magnVector1.MagnDoubleZ * magnVector2.MagnDoubleX);
            resVect.MagnDoubleZ = magnVector1.MagnDoubleX * magnVector2.MagnDoubleY -
                                 magnVector1.MagnDoubleY * magnVector2.MagnDoubleX;
            return resVect;
        }




        public double compassAngle()
        {
            PointPolar ptP = new PointPolar(new PointD(magnDoubleX, magnDoubleY));
            return ptP.PhiDegrees;
        }


        public double compassAngle(accelerometerData accData)
        {
            DenseVector dvAccVector = accData.ToDenseVector();

            DenseVector dvMagnVector = this.ToDenseVector();

            DenseVector dvAccUnitaryVector = DenseVector.Create(3, (i =>
            {
                return dvAccVector[i] / Math.Sqrt(dvAccVector * dvAccVector);
            }));

            DenseVector dvMagnHorizontalProjection = dvMagnVector - dvAccUnitaryVector * (dvMagnVector * dvAccUnitaryVector);

            PointPolar ptP = new PointPolar(new PointD(dvMagnHorizontalProjection[0], dvMagnHorizontalProjection[1]), true);
            return ptP.PhiDegrees;
        }

        public DenseVector ToDenseVector()
        {
            DenseVector dvRetVect = DenseVector.Create(3, i =>
            {
                switch (i)
                {
                    case 0:
                        return magnDoubleX;
                    case 1:
                        return magnDoubleY;
                    case 2:
                        return magnDoubleZ;
                    default:
                        return 0.0d;
                }
            });
            dvRetVect = DenseVector.OfEnumerable(dvRetVect.Concat(new double[] { devID }));
            return dvRetVect;
        }




        public MagnetometerData Copy()
        {
            MagnetometerData newInstance = new MagnetometerData(magnDoubleX, magnDoubleY, magnDoubleZ);
            newInstance.devID = devID;
            return newInstance;
        }
    }


    public class GPSdata
    {
        public string GPSstring;
        public double lat = 0.0d;
        public string latHemisphere = "N";
        public double lon = 0.0d;
        public string lonHemisphere = "E";
        public DateTime dateTimeUTC = DateTime.UtcNow;
        public bool validGPSdata = true;


        public double IOFFEdataHeadingTrue = 0.0d;
        public double IOFFEdataHeadingGyro = 0.0d;
        public double IOFFEdataSpeedKnots = 0.0d;
        public double IOFFEdataDepth = 0.0d;

        public int devID = 0;

        public GPSdatasources dataSource = GPSdatasources.CloudCamArduinoGPS;



        public GPSdata(string gpsString, GPSdatasources source)
        {
            GPSstring = gpsString;
            dataSource = source;
            parseGPSstring();
        }




        private string getChecksum(string sentence)
        {
            //Start with first Item
            int checksum = Convert.ToByte(sentence[sentence.IndexOf('$') + 1]);
            // Loop through all chars to get a checksum
            for (int i = sentence.IndexOf('$') + 2; i < sentence.IndexOf('*'); i++)
            {
                // No. XOR the checksum with this character's value
                checksum ^= Convert.ToByte(sentence[i]);
            }
            // Return the checksum formatted as a two-character hexadecimal
            return checksum.ToString("X2");
        }





        public GPSdata()
        {
            lat = 0.0d;
            lon = 0.0d;
            lonHemisphere = "E";
            latHemisphere = "N";
            dateTimeUTC = DateTime.UtcNow;
            validGPSdata = false;
        }


        public GPSdata(GPSdata objectToClone)
        {
            GPSstring = objectToClone.GPSstring;
            lat = objectToClone.lat;
            latHemisphere = objectToClone.latHemisphere;
            lon = objectToClone.lon;
            lonHemisphere = objectToClone.lonHemisphere;
            dateTimeUTC = objectToClone.dateTimeUTC;
            validGPSdata = objectToClone.validGPSdata;


            IOFFEdataHeadingTrue = objectToClone.IOFFEdataHeadingTrue;
            IOFFEdataHeadingGyro = objectToClone.IOFFEdataHeadingGyro;
            IOFFEdataSpeedKnots = objectToClone.IOFFEdataSpeedKnots;
            IOFFEdataDepth = objectToClone.IOFFEdataDepth;
        }


        public GPSdata Clone()
        {
            GPSdata newInstance = new GPSdata(this);
            newInstance.devID = devID;
            return newInstance;
        }


        public double Lat
        {
            get
            {
                if (latHemisphere == "N") return lat;
                else return -lat;
            }
        }


        public double LatDec
        {
            get
            {
                double latGrad = lat;
                double theSign = 1.0d;
                if (latHemisphere == "S") theSign = -1.0d;
                else theSign = 1.0d;

                //5950.32
                double latTrunc = Math.Truncate(latGrad); //5950.0d
                string strLatTrunc = (Convert.ToInt32(latTrunc)).ToString("D5");// "5950"
                double latMinutes = Convert.ToDouble(strLatTrunc.Substring(strLatTrunc.Length - 2, 2)); //50.0d

                double latGrad1 = (latTrunc - latMinutes) / 100.0d; // 59.0d
                double latGrad2 = latMinutes / 60.0d;               // 50.0d/60.0d
                double latGrad3 = (latGrad - latTrunc) / 60.0d;     // 0.32d/60.0d

                latGrad = latGrad1 + latGrad2 + latGrad3;

                return latGrad * theSign;
            }
        }


        public double Lon
        {
            get
            {
                if (lonHemisphere == "E") return lon;
                else return -lon;
            }
        }



        public double LonDec
        {
            get
            {
                double lonGrad = lon;
                double theSign = 1.0d;
                if (lonHemisphere == "W") theSign = -1.0d;
                else theSign = 1.0d;

                //4050.32
                double lonTrunc = Math.Truncate(lonGrad); //4050.0d
                string strlonTrunc = (Convert.ToInt32(lonTrunc)).ToString("D5");// "04050"
                double lonMinutes = Convert.ToDouble(strlonTrunc.Substring(strlonTrunc.Length - 2, 2)); //50.0d

                double lonGrad1 = (lonTrunc - lonMinutes) / 100.0d; // 40.0d
                double lonGrad2 = lonMinutes / 60.0d;               // 50.0d/60.0d
                double lonGrad3 = (lonGrad - lonTrunc) / 60.0d;     // 0.32d/60.0d

                lonGrad = lonGrad1 + lonGrad2 + lonGrad3;

                return lonGrad * theSign;
            }
        }



        public double SpeedKmHour
        {
            get { return 0.0d; }
        }



        public double SpeedMetresSec
        {
            get { return 0.0d; }
        }


        public override string ToString()
        {
            return "" + LatDec + ", " + LonDec;
        }




        public string HRString()
        {
            return "" + Math.Abs(Lat) + latHemisphere + ", " + Math.Abs(Lon) + lonHemisphere;
        }




        private void parseGPSstring()
        {
            string[] strValues = GPSstring.Split(',');

            if (dataSource == GPSdatasources.CloudCamArduinoGPS)
            {
                if (strValues.Count() != 15)
                {
                    //некорректная строка. вернем без результатов
                    validGPSdata = false;
                    return;
                }


                try
                {
                    string timeUTC = strValues[1];
                    dateTimeUTC = DateTime.UtcNow.Date;
                    dateTimeUTC = dateTimeUTC.AddHours(Convert.ToInt32(timeUTC.Substring(0, 2)));
                    dateTimeUTC = dateTimeUTC.AddMinutes(Convert.ToInt32(timeUTC.Substring(2, 2)));
                    dateTimeUTC = dateTimeUTC.AddSeconds(Convert.ToInt32(timeUTC.Substring(4, 2)));
                }
                catch (Exception)
                {
                    dateTimeUTC = DateTime.UtcNow;
                    validGPSdata = false;
                }


                try
                {
                    lat = Convert.ToDouble(strValues[2].Replace(".", ","));
                    latHemisphere = strValues[3];
                }
                catch (Exception)
                {
                    lat = 0.0d;
                    validGPSdata = false;
                }


                try
                {
                    lon = Convert.ToDouble(strValues[4].Replace(".", ","));
                    lonHemisphere = strValues[5];
                }
                catch (Exception)
                {
                    lon = 0.0d;
                    validGPSdata = false;
                }
            }
            else if (dataSource == GPSdatasources.IOFFEvesselDataServer)
            {
                //Coord,$240914,235707,5732.39755766,N,00000.21852030,W,146.774,144.8,11.531,-114.39,(E),147.034#

                if (strValues.Count() != 13)
                {
                    //некорректная строка. вернем без результатов
                    validGPSdata = false;
                    return;
                }


                try
                {
                    string dateUTC = strValues[1];
                    string timeUTC = strValues[2];
                    dateTimeUTC = new DateTime(
                        2000 + Convert.ToInt32(dateUTC.Substring(4, 2)),
                        Convert.ToInt32(dateUTC.Substring(2, 2)),
                        Convert.ToInt32(dateUTC.Substring(0, 2)),
                        Convert.ToInt32(timeUTC.Substring(0, 2)),
                        Convert.ToInt32(timeUTC.Substring(2, 2)),
                        Convert.ToInt32(timeUTC.Substring(4, 2)));
                }
                catch (Exception)
                {
                    dateTimeUTC = DateTime.UtcNow;
                    validGPSdata = false;
                }


                try
                {
                    lat = Convert.ToDouble(strValues[3].Replace(".", ","));
                    latHemisphere = strValues[4];
                }
                catch (Exception)
                {
                    lat = 0.0d;
                    validGPSdata = false;
                }


                try
                {
                    lon = Convert.ToDouble(strValues[5].Replace(".", ","));
                    lonHemisphere = strValues[6];
                }
                catch (Exception)
                {
                    lon = 0.0d;
                    validGPSdata = false;
                }



                try
                {
                    IOFFEdataHeadingTrue = Convert.ToDouble(strValues[7].Replace(".", ","));
                }
                catch (Exception)
                {
                    IOFFEdataHeadingTrue = 0.0d;
                    //validGPSdata = false;
                }

                try
                {
                    IOFFEdataHeadingGyro = Convert.ToDouble(strValues[8].Replace(".", ","));
                }
                catch (Exception)
                {
                    IOFFEdataHeadingGyro = 0.0d;
                    //validGPSdata = false;
                }


                try
                {
                    IOFFEdataSpeedKnots = Convert.ToDouble(strValues[9].Replace(".", ","));
                }
                catch (Exception)
                {
                    IOFFEdataSpeedKnots = 0.0d;
                    //validGPSdata = false;
                }


                try
                {
                    IOFFEdataDepth = Convert.ToDouble(strValues[10].Replace(".", ","));
                }
                catch (Exception)
                {
                    IOFFEdataDepth = 0.0d;
                    //validGPSdata = false;
                }
            }
        }



        public PointD ToPointD()
        {
            return new PointD(LonDec, LatDec);
        }



        //IOFFEdataHeadingTrue = 0.0d;
        //public double IOFFEdataHeadingGyro = 0.0d;
        //public double IOFFEdataSpeedKnots = 0.0d;
        //public double IOFFEdataDepth = 0.0d;

        public DenseVector ToDenseVector()
        {
            DenseVector dvRetVect = null;

            if (dataSource == GPSdatasources.CloudCamArduinoGPS)
            {
                double[] arr = new double[] { Lat, Lon };
                dvRetVect = DenseVector.OfEnumerable(arr);
            }
            else if (dataSource == GPSdatasources.IOFFEvesselDataServer)
            {
                double[] arr = new double[] { Lat, Lon, IOFFEdataHeadingTrue, IOFFEdataHeadingGyro, IOFFEdataSpeedKnots, IOFFEdataDepth };
                dvRetVect = DenseVector.OfEnumerable(arr);
            }

            dvRetVect = DenseVector.OfEnumerable(dvRetVect.Concat(new double[] { devID }));

            return dvRetVect;
        }



        public DenseMatrix ToOneRowDenseMatrix()
        {
            DenseVector dvRow = ToDenseVector();
            List<DenseVector> listOfRows = new List<DenseVector>();
            listOfRows.Add(dvRow);
            DenseMatrix dmRetMatr = DenseMatrix.OfRows(1, dvRow.Count, listOfRows);
            return dmRetMatr;
        }



        public static DenseMatrix ToDenseMatrix(IEnumerable<GPSdata> lGPSData)
        {
            IEnumerable<DenseVector> lDVdata =
                new List<GPSdata>(lGPSData).ConvertAll(gpsDat => gpsDat.ToDenseVector());
            DenseMatrix dmRetMatr = DenseMatrix.OfRows(lDVdata.Count(), lDVdata.ElementAt(0).Count, lDVdata);
            return dmRetMatr;
        }




        public GPSdata(IEnumerable<double> source)
        {
            if (source.Count() == 2)
            {
                try
                {
                    lat = source.ElementAt(0);
                    latHemisphere = (lat >= 0.0d) ? ("N") : ("S");
                    lat = Math.Abs(lat);

                    lon = source.ElementAt(1);
                    lonHemisphere = (lon >= 0.0d) ? ("E") : ("W");
                    lon = Math.Abs(lon);
                }
                catch (Exception)
                {
                    validGPSdata = false;
                    return;
                }
            }
            if (source.Count() == 3) // devID specified
            {
                try
                {
                    lat = source.ElementAt(0);
                    latHemisphere = (lat >= 0.0d) ? ("N") : ("S");
                    lat = Math.Abs(lat);

                    lon = source.ElementAt(1);
                    lonHemisphere = (lon >= 0.0d) ? ("E") : ("W");
                    lon = Math.Abs(lon);

                    if ((source.ElementAt(2) == 1.0) || (source.ElementAt(2) == 2.0))
                    {
                        devID = Convert.ToInt32(source.ElementAt(2));
                    }
                }
                catch (Exception)
                {
                    validGPSdata = false;
                    return;
                }
            }
            else if (source.Count() == 6)
            {
                // { Lat, Lon, IOFFEdataHeadingTrue, IOFFEdataHeadingGyro, IOFFEdataSpeedKnots, IOFFEdataDepth }
                try
                {
                    lat = source.ElementAt(0);
                    latHemisphere = (lat >= 0.0d) ? ("N") : ("S");
                    lat = Math.Abs(lat);

                    lon = source.ElementAt(1);
                    lonHemisphere = (lon >= 0.0d) ? ("E") : ("W");
                    lon = Math.Abs(lon);

                    IOFFEdataHeadingTrue = source.ElementAt(2);

                    IOFFEdataHeadingGyro = source.ElementAt(3);

                    IOFFEdataSpeedKnots = source.ElementAt(4);

                    IOFFEdataDepth = source.ElementAt(5);
                }
                catch (Exception)
                {
                    validGPSdata = false;
                    return;
                }
            }
            else if (source.Count() == 7) // devID specified
            {
                // { Lat, Lon, IOFFEdataHeadingTrue, IOFFEdataHeadingGyro, IOFFEdataSpeedKnots, IOFFEdataDepth }
                try
                {
                    lat = source.ElementAt(0);
                    latHemisphere = (lat >= 0.0d) ? ("N") : ("S");
                    lat = Math.Abs(lat);

                    lon = source.ElementAt(1);
                    lonHemisphere = (lon >= 0.0d) ? ("E") : ("W");
                    lon = Math.Abs(lon);

                    IOFFEdataHeadingTrue = source.ElementAt(2);

                    IOFFEdataHeadingGyro = source.ElementAt(3);

                    IOFFEdataSpeedKnots = source.ElementAt(4);

                    IOFFEdataDepth = source.ElementAt(5);

                    if ((source.ElementAt(6) == 1.0) || (source.ElementAt(6) == 2.0))
                    {
                        devID = Convert.ToInt32(source.ElementAt(6));
                    }
                }
                catch (Exception)
                {
                    validGPSdata = false;
                    return;
                }
            }
            else
            {
                validGPSdata = false;
                return;
            }
        }



        public static List<GPSdata> OfDenseMatrix(DenseMatrix dmSource)
        {
            if (dmSource.RowCount == 0)
            {
                return null;
            }

            return
                dmSource.EnumerateRowsIndexed()
                    .Select(tplRow => new GPSdata(tplRow.Item2))
                    .Where(newgpsDatum => newgpsDatum != null)
                    .ToList();
        }




        public AzimuthZenithAngle SunZenithAzimuth()
        {
            SPA spaCalc = new SPA(dateTimeUTC.Year, dateTimeUTC.Month, dateTimeUTC.Day, dateTimeUTC.Hour,
                        dateTimeUTC.Minute, dateTimeUTC.Second, (float)LonDec, (float)LatDec,
                        (float)SPAConst.DeltaT(dateTimeUTC));
            int res = spaCalc.spa_calculate();
            AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalc.spa.azimuth,
                spaCalc.spa.zenith);
            return sunPositionSPAext;
        }
    }



    public class MeteoData
    {
        public double pressure = 1000.0d; //mB, 100Pa, hPa
        public double Rhumidity = 90.0d;
        public double airTemperature = 10.0d;
        public double windSpeed = 3.0d;
        public double windDirection = 0.0d;
        public double waterTemperature = 0.0d;
        public double waterSalinity = 0.0d;
        //public Dictionary<string, double> additionalUnknownData = new Dictionary<string, double>();
        public bool validMeteoData = true;


        public MeteoData()
        {
        }



        //0     1                 2                3                4                  5               6                                                                               78            9
        //Meteo,Temperature: 11.1,Pressure: 1001.6,Wind speed: 0.70,Wind Dir. : 297.96,R. humidity: 94,Water(t): index='41600'><Field0>11.5097</Field0><Field1>35.0939</Field1></Scan>,,M_Speed: 5.6,M_Dir: 341.5

        public MeteoData(string strIOFFEvesselMeteoInput)
        {
            if ((strIOFFEvesselMeteoInput == "") || (strIOFFEvesselMeteoInput == null))
            {
                validMeteoData = false;
                return;
            }

            string[] strValues;

            try
            {
                strValues = strIOFFEvesselMeteoInput.Split(',');
            }
            catch (Exception)
            {
                validMeteoData = false;
                return;
            }

            //Со включенным термосолинографом SeaSave
            //0     1                2                3                4                  5               6                                                                              78            9
            //Meteo,Temperature: 1.7,Pressure: 1001.1,Wind speed: 0.11,Wind Dir. : 359.19,R. humidity: 96,Water(t): index='38067'><Field0>2.9014</Field0><Field1>30.9782</Field1></Scan>,,M_Speed: 0.9,M_Dir: 333.4
            //
            // С выключенным термосолинографом
            //0     1                2                3                4                 5               6                   78            9
            //Meteo,Temperature: 1.4,Pressure: 1001.0,Wind speed: 4.54,Wind Dir. : 46.82,R. humidity: 96,Water(t): Practical,,M_Speed: 3.8,M_Dir: 73.2

            //if (strValues.Count() < 9)
            //{
            //    //некорректная строка. вернем без результатов
            //    validMeteoData = false;
            //    return;
            //}

            // Meteo,Temperature: 10.8,Pressure: 1015.1,Wind speed: 2.28,Wind Dir. : 323.40,,M_Speed: 5.5,M_Dir: 358.2
            List<string> meteoDataFields = new List<string>(strValues);
            foreach (string strMeteoDatumField in meteoDataFields)
            {
                if (strMeteoDatumField.Contains("Temperature:"))
                {
                    string datumSubstr = strMeteoDatumField.Replace("Temperature:", "").Trim().Replace(".", ",");
                    try
                    {
                        airTemperature = Convert.ToDouble(datumSubstr);
                    }
                    catch (Exception ex)
                    {
                        airTemperature = 0.0d;
                    }
                }

                if (strMeteoDatumField.Contains("Pressure:"))
                {
                    string datumSubstr = strMeteoDatumField.Replace("Pressure:", "").Trim().Replace(".", ",");
                    try
                    {
                        pressure = Convert.ToDouble(datumSubstr);
                    }
                    catch (Exception ex)
                    {
                        pressure = 0.0d;
                    }
                }

                if (strMeteoDatumField.Contains("Wind speed:"))
                {
                    string datumSubstr = strMeteoDatumField.Replace("Wind speed:", "").Trim().Replace(".", ",");
                    try
                    {
                        windSpeed = Convert.ToDouble(datumSubstr);
                    }
                    catch (Exception ex)
                    {
                        windSpeed = 0.0d;
                    }
                }

                if (strMeteoDatumField.Contains("Wind Dir. :"))
                {
                    string datumSubstr = strMeteoDatumField.Replace("Wind Dir. :", "").Trim().Replace(".", ",");
                    try
                    {
                        windDirection = Convert.ToDouble(datumSubstr);
                    }
                    catch (Exception ex)
                    {
                        windDirection = 0.0d;
                    }
                }

                if (strMeteoDatumField.Contains("R. humidity:"))
                {
                    string datumSubstr = strMeteoDatumField.Replace("R. humidity:", "").Trim().Replace(".", ",");
                    try
                    {
                        Rhumidity = Convert.ToDouble(datumSubstr);
                    }
                    catch (Exception ex)
                    {
                        Rhumidity = 0.0d;
                    }
                }


                if (strMeteoDatumField.Contains("Water(t):"))
                {
                    string datumSubstr = strMeteoDatumField.Replace("Water(t):", "").Trim();
                    try
                    {
                        string strWaterConsistency = datumSubstr;
                        int idx1 = strWaterConsistency.IndexOf("<Field0>");
                        int idx2 = strWaterConsistency.IndexOf("</Field0>");
                        waterTemperature = Convert.ToDouble(strWaterConsistency.Substring(idx1 + 8, idx2 - 8 - idx1).Replace(".", ","));
                        int idx3 = strWaterConsistency.IndexOf("<Field1>");
                        int idx4 = strWaterConsistency.IndexOf("</Field1>");
                        waterSalinity = Convert.ToDouble(strWaterConsistency.Substring(idx3 + 8, idx4 - 8 - idx3).Replace(".", ","));
                    }
                    catch (Exception ex)
                    {
                        waterSalinity = 0.0d;
                        waterTemperature = 0.0d;
                    }
                }
            }


            #region obsolete
            //Temperature: 11.1
            //try
            //{
            //    string datumString = strValues[1];
            //    string[] sunstrings = datumString.Split(':');

            //    airTemperature = Convert.ToDouble(sunstrings[1].Trim().Replace(".", ","));
            //}
            //catch (Exception)
            //{
            //    airTemperature = 0.0d;
            //    validMeteoData = false;
            //    return;
            //}


            //Pressure: 1001.6
            //try
            //{
            //    string datumString = strValues[2];
            //    string[] sunstrings = datumString.Split(':');
            //
            //    pressure = Convert.ToDouble(sunstrings[1].Trim().Replace(".", ","));
            //}
            //catch (Exception)
            //{
            //    pressure = 0.0d;
            //    validMeteoData = false;
            //    return;
            //}

            //Wind speed: 0.70
            //try
            //{
            //    string datumString = strValues[3];
            //    string[] sunstrings = datumString.Split(':');
            //
            //    windSpeed = Convert.ToDouble(sunstrings[1].Trim().Replace(".", ","));
            //}
            //catch (Exception)
            //{
            //    windSpeed = 0.0d;
            //    validMeteoData = false;
            //    return;
            //}
            


            //Wind Dir. : 297.96
            //try
            //{
            //    string datumString = strValues[4];
            //    string[] sunstrings = datumString.Split(':');
            //
            //    windDirection = Convert.ToDouble(sunstrings[1].Trim().Replace(".", ","));
            //}
            //catch (Exception)
            //{
            //    windDirection = 0.0d;
            //    validMeteoData = false;
            //    return;
            //}


            //R. humidity: 94
            //try
            //{
            //    string datumString = strValues[5];
            //    string[] sunstrings = datumString.Split(':');
            //
            //    Rhumidity = Convert.ToDouble(sunstrings[1].Trim().Replace(".", ","));
            //}
            //catch (Exception)
            //{
            //    Rhumidity = 0.0d;
            //    validMeteoData = false;
            //    return;
            //}
            


            //try
            //{
            //    //Water(t): index='41600'><Field0>11.5097</Field0><Field1>35.0939</Field1></Scan>
            //    string strWaterConsistency = strValues[6];
            //    int idx1 = strWaterConsistency.IndexOf("<Field0>");
            //    int idx2 = strWaterConsistency.IndexOf("</Field0>");
            //    waterTemperature = Convert.ToDouble(strWaterConsistency.Substring(idx1 + 8, idx2 - 8 - idx1).Replace(".", ","));
            //    int idx3 = strWaterConsistency.IndexOf("<Field1>");
            //    int idx4 = strWaterConsistency.IndexOf("</Field1>");
            //    waterSalinity = Convert.ToDouble(strWaterConsistency.Substring(idx3 + 8, idx4 - 8 - idx3).Replace(".", ","));
            //}
            //catch (Exception)
            //{
            //    waterSalinity = 0.0d;
            //    waterTemperature = 0.0d;
            //}
            #endregion obsolete


        }



        public DenseVector ToDenseVector()
        {
            double[] arr = new double[] { pressure, Rhumidity, airTemperature, windSpeed, windDirection, waterTemperature, waterSalinity };
            return DenseVector.OfEnumerable(arr);
        }



        public DenseMatrix ToOneRowDenseMatrix()
        {
            DenseVector dvRow = ToDenseVector();
            List<DenseVector> listOfRows = new List<DenseVector>();
            listOfRows.Add(dvRow);
            DenseMatrix dmRetMatr = DenseMatrix.OfRows(1, dvRow.Count, listOfRows);
            return dmRetMatr;
        }




        public static DenseMatrix ToDenseMatrix(IEnumerable<MeteoData> lMeteoData)
        {
            IEnumerable<DenseVector> lDVdata =
                new List<MeteoData>(lMeteoData).ConvertAll(metDat => metDat.ToDenseVector());
            DenseMatrix dmRetMatr = DenseMatrix.OfRows(lDVdata.Count(), lDVdata.ElementAt(0).Count, lDVdata);
            return dmRetMatr;
        }




        public MeteoData(IEnumerable<double> source)
        {
            if (source.Count() == 7)
            {
                // { pressure, Rhumidity, airTemperature, windSpeed, windDirection, waterTemperature, waterSalinity }
                try
                {
                    pressure = source.ElementAt(0);
                    Rhumidity = source.ElementAt(1);
                    airTemperature = source.ElementAt(2);
                    windSpeed = source.ElementAt(3);
                    windDirection = source.ElementAt(4);
                    waterTemperature = source.ElementAt(5);
                    waterSalinity = source.ElementAt(6);
                }
                catch (Exception)
                {
                    validMeteoData = false;
                    return;
                }
            }
            else
            {
                validMeteoData = false;
                return;
            }
        }



        public static List<MeteoData> OfDenseMatrix(DenseMatrix dmSource)
        {
            if (dmSource.RowCount == 0)
            {
                return null;
            }

            List<MeteoData> listRetMeteodata = new List<MeteoData>();

            foreach (Tuple<int, Vector<double>> tplRow in dmSource.EnumerateRowsIndexed())
            {
                MeteoData newMeteoDatum = new MeteoData(tplRow.Item2);
                if (newMeteoDatum != null)
                {
                    listRetMeteodata.Add(newMeteoDatum);
                }
            }

            return listRetMeteodata;
        }

    }





    public class FixedQueue<T>
    {
        private Queue<T> queue;

        public FixedQueue(int capacity)
        {
            Capacity = capacity;
            queue = new Queue<T>(capacity);
        }



        public FixedQueue(IEnumerable<T> inputEnum)
        {
            Capacity = inputEnum.Count();
            queue = new Queue<T>(Capacity);
            foreach (T item in inputEnum.Reverse())
            {
                queue.Enqueue(item);
            }
        }




        public int Capacity { get; private set; }

        public int Count { get { return queue.Count; } }

        public T Enqueue(T item)
        {
            queue.Enqueue(item);
            if (queue.Count > Capacity)
            {
                return queue.Dequeue();
            }
            else
            {
                //if you want this to do something else, such as return the `peek` value
                //modify as desired.
                return default(T);
            }
        }




        public void Enqueue(IEnumerable<T> items)
        {
            foreach (T item in items.Reverse())
            {
                Enqueue(item);
            }
        }




        public T Peek()
        {
            return queue.Peek();
        }


        public T First()
        {
            return queue.First();
        }


        public T Last()
        {
            return queue.Last();
        }


        public List<T> ToList()
        {
            List<T> lRet = queue.ToList();
            lRet.Reverse();
            return lRet;
        }


        public T[] ToArray()
        {
            List<T> lRet = queue.ToList();
            lRet.Reverse();
            return lRet.ToArray();
        }
    }





    public class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region // obsolete
        //private List<IObserver<T>> observersList = new List<IObserver<T>>();

        //public IDisposable Subscribe(IObserver<T> observer)
        //{
        //    if (!observersList.Contains(observer))
        //    {
        //        observersList.Add(observer);
        //    }
        //    return new Unsubscriber(observersList, observer);
        //}




        //public void TrackLocation(Nullable<T> loc)
        //{
        //    foreach (var observer in observers)
        //    {
        //        if (!loc.HasValue)
        //            observer.OnError(new LocationUnknownException());
        //        else
        //            observer.OnNext(loc.Value);
        //    }
        //}




        //private class Unsubscriber : IDisposable
        //{
        //    private List<IObserver<T>> _observers;
        //    private IObserver<T> _observer;

        //    public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        //    {
        //        this._observers = observers;
        //        this._observer = observer;
        //    }

        //    public void Dispose()
        //    {
        //        if (_observer != null && _observers.Contains(_observer))
        //            _observers.Remove(_observer);
        //    }
        //}
        #endregion // obsolete

        public ObservableQueue()
        {
        }

        public ObservableQueue(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                base.Enqueue(item);
        }

        public ObservableQueue(List<T> list)
        {
            foreach (var item in list)
                base.Enqueue(item);
        }


        public new virtual void Clear()
        {
            base.Clear();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public new virtual T Dequeue()
        {
            var item = base.Dequeue();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return item;
        }

        public new virtual void Enqueue(T item)
        {
            base.Enqueue(item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }


        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;


        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.RaiseCollectionChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(e);
        }


        protected virtual event PropertyChangedEventHandler PropertyChanged;


        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, e);
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, e);
        }


        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += value; }
            remove { this.PropertyChanged -= value; }
        }
    }






    public class ObservableConcurrentQueue<T> : ConcurrentQueue<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region // obsolete
        //private List<IObserver<T>> observersList = new List<IObserver<T>>();

        //public IDisposable Subscribe(IObserver<T> observer)
        //{
        //    if (!observersList.Contains(observer))
        //    {
        //        observersList.Add(observer);
        //    }
        //    return new Unsubscriber(observersList, observer);
        //}




        //public void TrackLocation(Nullable<T> loc)
        //{
        //    foreach (var observer in observers)
        //    {
        //        if (!loc.HasValue)
        //            observer.OnError(new LocationUnknownException());
        //        else
        //            observer.OnNext(loc.Value);
        //    }
        //}




        //private class Unsubscriber : IDisposable
        //{
        //    private List<IObserver<T>> _observers;
        //    private IObserver<T> _observer;

        //    public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        //    {
        //        this._observers = observers;
        //        this._observer = observer;
        //    }

        //    public void Dispose()
        //    {
        //        if (_observer != null && _observers.Contains(_observer))
        //            _observers.Remove(_observer);
        //    }
        //}
        #endregion // obsolete


        public ObservableConcurrentQueue()
        {
        }

        public ObservableConcurrentQueue(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                base.Enqueue(item);
        }

        public ObservableConcurrentQueue(List<T> list)
        {
            foreach (var item in list)
                base.Enqueue(item);
        }


        
        public new virtual bool TryDequeue(out T item)
        {
            bool res = base.TryDequeue(out item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return res;
        }

        public new virtual void Enqueue(T item)
        {
            base.Enqueue(item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }


        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;


        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.RaiseCollectionChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(e);
        }


        protected virtual event PropertyChangedEventHandler PropertyChanged;


        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, e);
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, e);
        }


        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += value; }
            remove { this.PropertyChanged -= value; }
        }
    }





    public class ObservableStack<T> : Stack<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public ObservableStack()
        {
        }

        public ObservableStack(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                base.Push(item);
        }

        public ObservableStack(List<T> list)
        {
            foreach (var item in list)
                base.Push(item);
        }


        public new virtual void Clear()
        {
            base.Clear();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public new virtual T Pop()
        {
            var item = base.Pop();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return item;
        }

        public new virtual void Push(T item)
        {
            base.Push(item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }


        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;


        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.RaiseCollectionChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(e);
        }


        protected virtual event PropertyChangedEventHandler PropertyChanged;


        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, e);
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, e);
        }


        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += value; }
            remove { this.PropertyChanged -= value; }
        }
    }





}