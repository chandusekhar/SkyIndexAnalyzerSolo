using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Geometry;
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





    public class AccelerometerData : CsvExportable
    {
        private int accX;
        private int accY;
        private int accZ;
        private double accDoubleX;
        private double accDoubleY;
        private double accDoubleZ;
        private double accMagnitude;
        public int devID = 1;

        public bool validAccData = true;


        #region for CSV export
        public int CloudCamDevID
        {
            get { return devID; }
        }

        #endregion


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

        public AccelerometerData()
        {
            this.AccX = 0;
            this.AccY = 0;
            this.AccZ = 0;
        }
        public AccelerometerData(int aX, int aY, int aZ)
        {
            this.AccX = aX;
            this.AccY = aY;
            this.AccZ = aZ;
        }
        public AccelerometerData(double aX, double aY, double aZ)
        {
            this.AccDoubleX = aX;
            this.AccDoubleY = aY;
            this.AccDoubleZ = aZ;
        }


        public AccelerometerData(IEnumerable<int> source)
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



        public AccelerometerData(IEnumerable<double> source)
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




        public AccelerometerData(IEnumerable<string> source)
        {
            if (source.Count() == 3)
            {
                try
                {
                    this.AccDoubleX = CommonTools.ParseDouble(source.ElementAt(0));
                    this.AccDoubleY = CommonTools.ParseDouble(source.ElementAt(1));
                    this.AccDoubleZ = CommonTools.ParseDouble(source.ElementAt(2));
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
                    this.AccDoubleX = CommonTools.ParseDouble(source.ElementAt(0));
                    this.AccDoubleY = CommonTools.ParseDouble(source.ElementAt(1));
                    this.AccDoubleZ = CommonTools.ParseDouble(source.ElementAt(2));
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


        public static AccelerometerData operator +(AccelerometerData accVector1, AccelerometerData accVector2)
        {
            AccelerometerData retAccData = new AccelerometerData();
            retAccData.AccDoubleX = accVector1.AccDoubleX + accVector2.AccDoubleX;
            retAccData.AccDoubleY = accVector1.AccDoubleY + accVector2.AccDoubleY;
            retAccData.AccDoubleZ = accVector1.AccDoubleZ + accVector2.AccDoubleZ;


            return retAccData;
        }



        public static AccelerometerData operator -(AccelerometerData accVector1, AccelerometerData accVector2)
        {
            AccelerometerData retAccData = new AccelerometerData();
            retAccData.AccDoubleX = accVector1.AccDoubleX - accVector2.AccDoubleX;
            retAccData.AccDoubleY = accVector1.AccDoubleY - accVector2.AccDoubleY;
            retAccData.AccDoubleZ = accVector1.AccDoubleZ - accVector2.AccDoubleZ;


            return retAccData;
        }

        public static AccelerometerData operator /(AccelerometerData accVector1, double dValue)
        {
            AccelerometerData retAccData = new AccelerometerData();
            retAccData.AccDoubleX = accVector1.AccDoubleX / dValue;
            retAccData.AccDoubleY = accVector1.AccDoubleY / dValue;
            retAccData.AccDoubleZ = accVector1.AccDoubleZ / dValue;


            return retAccData;
        }


        public static double operator *(AccelerometerData accVector1, AccelerometerData accVector2)
        {
            double product = accVector1.AccDoubleX * accVector2.AccDoubleX +
                             accVector1.AccDoubleY * accVector2.AccDoubleY +
                             accVector1.AccDoubleZ * accVector2.AccDoubleZ;
            return product;
        }


        public static AccelerometerData operator *(AccelerometerData accVector1, double dValue)
        {
            AccelerometerData retAccData = new AccelerometerData();
            retAccData.AccDoubleX = accVector1.AccDoubleX * dValue;
            retAccData.AccDoubleY = accVector1.AccDoubleY * dValue;
            retAccData.AccDoubleZ = accVector1.AccDoubleZ * dValue;


            return retAccData;
        }



        public static AccelerometerData operator *(double dValue, AccelerometerData accVector1)
        {
            AccelerometerData retAccData = new AccelerometerData();
            retAccData.AccDoubleX = accVector1.AccDoubleX * dValue;
            retAccData.AccDoubleY = accVector1.AccDoubleY * dValue;
            retAccData.AccDoubleZ = accVector1.AccDoubleZ * dValue;


            return retAccData;
        }




        public static AccelerometerData operator ^(AccelerometerData accVector1, AccelerometerData accVector2)
        {
            return VectorProduct(accVector1, accVector2);
        }


        public static AccelerometerData VectorProduct(AccelerometerData accVector1, AccelerometerData accVector2)
        {
            AccelerometerData resVect = new AccelerometerData();
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




        public DenseVector ToDenseVector(Dictionary<string, AccelerometerData> dctCalibrationAccDataByDevID)
        {
            DenseVector dvRetVect = DenseVector.Create(7, i =>
            {
                switch (i)
                {
                    case 0:
                        return accDoubleX;
                    case 1:
                        return accDoubleY;
                    case 2:
                        return accDoubleZ;
                    case 3:
                        return dctCalibrationAccDataByDevID["devID" + devID].accDoubleX;
                    case 4:
                        return dctCalibrationAccDataByDevID["devID" + devID].accDoubleY;
                    case 5:
                        return dctCalibrationAccDataByDevID["devID" + devID].accDoubleZ;
                    case 6:
                        return
                            Math.Acos((this * dctCalibrationAccDataByDevID["devID" + devID]) /
                                      (AccMagnitude * dctCalibrationAccDataByDevID["devID" + devID].AccMagnitude));
                    default:
                        return 0.0d;
                }
            });
            dvRetVect = DenseVector.OfEnumerable(dvRetVect.Concat(new double[] { devID }));
            return dvRetVect;
        }




        public static List<AccelerometerData> OfDenseMatrix(DenseMatrix dmSource)
        {
            if (dmSource.RowCount == 0)
            {
                return null;
            }

            List<AccelerometerData> listRetAccData = new List<AccelerometerData>();

            foreach (Tuple<int, Vector<double>> tplRow in dmSource.EnumerateRowsIndexed())
            {
                AccelerometerData newAccDatum = new AccelerometerData(tplRow.Item2);
                if (newAccDatum != null)
                {
                    listRetAccData.Add(newAccDatum);
                }
            }

            return listRetAccData;
        }


        public AccelerometerData Copy()
        {
            AccelerometerData newInstance = new AccelerometerData(accDoubleX, accDoubleY, accDoubleZ);
            newInstance.devID = devID;
            return newInstance;
        }





        public static DenseMatrix ToDenseMatrix(IEnumerable<AccelerometerData> lAccData)
        {
            IEnumerable<DenseVector> lDVdata =
                new List<AccelerometerData>(lAccData).ConvertAll(accDat => accDat.ToDenseVector());
            DenseMatrix dmRetMatr = DenseMatrix.OfRows(lDVdata.Count(), lDVdata.ElementAt(0).Count, lDVdata);
            return dmRetMatr;
        }




        public static DenseMatrix ToDenseMatrix(IEnumerable<AccelerometerData> lAccData, Dictionary<string, AccelerometerData> dctCalibrationAccDataByDevID)
        {
            if (lAccData.Count() == 0)
            {
                AccelerometerData acc1 = new AccelerometerData();
                DenseVector dvAcc1 = acc1.ToDenseVector(dctCalibrationAccDataByDevID);
                IEnumerable<DenseVector> lDVdata = new List<DenseVector>() { dvAcc1 };
                DenseMatrix dmRetMatr = DenseMatrix.OfRows(lDVdata.Count(), lDVdata.ElementAt(0).Count, lDVdata);
                return dmRetMatr;
            }
            else
            {
                IEnumerable<DenseVector> lDVdata =
                    new List<AccelerometerData>(lAccData).ConvertAll(
                        accDat => accDat.ToDenseVector(dctCalibrationAccDataByDevID));
                DenseMatrix dmRetMatr = DenseMatrix.OfRows(lDVdata.Count(), lDVdata.ElementAt(0).Count, lDVdata);
                return dmRetMatr;
            }
        }
    }







    public class GyroData : CsvExportable
    {
        private int gyroX;
        private int gyroY;
        private int gyroZ;
        private double gyroDoubleX;
        private double gyroDoubleY;
        private double gyroDoubleZ;
        private double gyroMagnitude;
        public int devID = 0;


        #region for CSV export
        public int CloudCamDevID
        {
            get { return devID; }
        }

        #endregion


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
            if (source.Count() != 3)
            {
                this.GyroDoubleX = 0.0d;
                this.GyroDoubleY = 0.0d;
                this.GyroDoubleZ = 0.0d;
            }
            else
            {

                try
                {
                    this.GyroDoubleX = CommonTools.ParseDouble(source.ElementAt(0));
                    this.GyroDoubleY = CommonTools.ParseDouble(source.ElementAt(1));
                    this.GyroDoubleZ = CommonTools.ParseDouble(source.ElementAt(2));
                }
                catch (Exception ex)
                {
                    this.GyroDoubleX = 0.0d;
                    this.GyroDoubleY = 0.0d;
                    this.GyroDoubleZ = 0.0d;
                }
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




        public static DenseMatrix ToDenseMatrix(IEnumerable<GyroData> lGyroData)
        {
            IEnumerable<DenseVector> lDVdata =
                new List<GyroData>(lGyroData).ConvertAll(gyroDat => gyroDat.ToDenseVector());
            DenseMatrix dmRetMatr = DenseMatrix.OfRows(lDVdata.Count(), lDVdata.ElementAt(0).Count, lDVdata);
            return dmRetMatr;
        }



        public GyroData Copy()
        {
            GyroData newInstance = new GyroData(gyroDoubleX, gyroDoubleY, gyroDoubleZ);
            newInstance.devID = devID;
            return newInstance;
        }
    }






    public class MagnetometerData : CsvExportable
    {
        private int magnX;
        private int magnY;
        private int magnZ;
        private double magnDoubleX;
        private double magnDoubleY;
        private double magnDoubleZ;
        private double magnMagnitude;
        public int devID = 0;


        #region for CSV export
        public int CloudCamDevID
        {
            get { return devID; }
        }

        #endregion


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
                this.MagnDoubleX = CommonTools.ParseDouble(source.ElementAt(0));
                this.MagnDoubleY = CommonTools.ParseDouble(source.ElementAt(1));
                this.MagnDoubleZ = CommonTools.ParseDouble(source.ElementAt(2));
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


        public double compassAngle(AccelerometerData accData)
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







    public class GPSdata : CsvExportable
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



        #region for CSV export

        public string GpsString
        {
            get { return GPSstring; }
        }

        public DateTime DateTimeUTC
        {
            get { return dateTimeUTC; }
        }

        public double IoffeHeadingTrue
        {
            get { return IOFFEdataHeadingTrue; }
        }

        public double IoffeHeadingGyro
        {
            get { return IOFFEdataHeadingGyro; }
        }

        public double IoffeSpeedKnots
        {
            get { return IOFFEdataSpeedKnots; }
        }

        public double IoffeDepth
        {
            get { return IOFFEdataDepth; }
        }

        public int CloudCamDevID
        {
            get { return devID; }
        }

        public GPSdatasources DataSource
        {
            get { return dataSource; }
        }

        #endregion



        /// <summary>
        /// Initializes a new instance of the <see cref="GPSdata" /> class.
        /// 
        /// NOTE!!! Use only with the today-strings only!
        /// NMEA GPS data of GPGGA type doesn`t include DATE information!!!
        /// </summary>
        /// <param name="gpsString">The GPS NMEA GPGGA-type string.</param>
        /// <param name="source">The source.</param>
        public GPSdata(string gpsString, GPSdatasources source, DateTime date)
        {
            GPSstring = gpsString;
            dataSource = source;
            parseGPSstring();

            if (source == GPSdatasources.CloudCamArduinoGPS)
            {
                dateTimeUTC = new DateTime(date.Year, date.Month, date.Day, DateTimeUTC.Hour, DateTimeUTC.Minute,
                    DateTimeUTC.Second, DateTimeUTC.Millisecond);
            }
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
                string strLatTrunc = (Convert.ToInt32(latTrunc)).ToString("D5"); // "5950"
                double latMinutes = CommonTools.ParseDouble(strLatTrunc.Substring(strLatTrunc.Length - 2, 2)); //50.0d

                double latGrad1 = (latTrunc - latMinutes)/100.0d; // 59.0d
                double latGrad2 = latMinutes/60.0d; // 50.0d/60.0d
                //double latGrad3 = (latGrad - latTrunc) / 60.0d;     // 0.32d/60.0d
                double latGrad3 = (latGrad - latTrunc)/3.6d; // 0.32d * 100.0d / 3600.0d

                latGrad = latGrad1 + latGrad2 + latGrad3;

                return latGrad*theSign;
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
                string strlonTrunc = (Convert.ToInt32(lonTrunc)).ToString("D5"); // "04050"
                double lonMinutes = CommonTools.ParseDouble(strlonTrunc.Substring(strlonTrunc.Length - 2, 2)); //50.0d

                double lonGrad1 = (lonTrunc - lonMinutes)/100.0d; // 40.0d
                double lonGrad2 = lonMinutes/60.0d; // 50.0d/60.0d
                //double lonGrad3 = (lonGrad - lonTrunc) / 60.0d;     // 0.32d/60.0d
                double lonGrad3 = (lonGrad - lonTrunc)/3.6d; // 0.32d * 100.0d/3600.0d

                lonGrad = lonGrad1 + lonGrad2 + lonGrad3;

                return lonGrad*theSign;
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




        public string HRString(int decimals = int.MaxValue)
        {
            if (decimals == int.MaxValue)
            {
                return "" + Math.Abs(LatDec) + latHemisphere + ", " + Math.Abs(LonDec) + lonHemisphere;
            }
            else
            {
                return "" + Math.Round(Math.Abs(LatDec), decimals) + latHemisphere + ", " +
                       Math.Round(Math.Abs(LonDec), decimals) + lonHemisphere;
            }
            
        }




        /// <summary>
        /// Parses the gps string.
        /// NOTE!!! Use only with the today-strings only!
        /// NMEA GPS data of GPGGA type doesn`t include DATE information!!!
        /// </summary>
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
                    lat = CommonTools.ParseDouble(strValues[2]);
                    latHemisphere = strValues[3];
                }
                catch (Exception)
                {
                    lat = 0.0d;
                    validGPSdata = false;
                }


                try
                {
                    lon = CommonTools.ParseDouble(strValues[4]);
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
                    lat = CommonTools.ParseDouble(strValues[3]);
                    latHemisphere = strValues[4];
                }
                catch (Exception)
                {
                    lat = 0.0d;
                    validGPSdata = false;
                }


                try
                {
                    lon = CommonTools.ParseDouble(strValues[5]);
                    lonHemisphere = strValues[6];
                }
                catch (Exception)
                {
                    lon = 0.0d;
                    validGPSdata = false;
                }



                try
                {
                    IOFFEdataHeadingTrue = CommonTools.ParseDouble(strValues[7]);
                }
                catch (Exception)
                {
                    IOFFEdataHeadingTrue = 0.0d;
                    //validGPSdata = false;
                }

                try
                {
                    IOFFEdataHeadingGyro = CommonTools.ParseDouble(strValues[8]);
                }
                catch (Exception)
                {
                    IOFFEdataHeadingGyro = 0.0d;
                    //validGPSdata = false;
                }


                try
                {
                    IOFFEdataSpeedKnots = CommonTools.ParseDouble(strValues[9]);
                }
                catch (Exception)
                {
                    IOFFEdataSpeedKnots = 0.0d;
                    //validGPSdata = false;
                }


                try
                {
                    IOFFEdataDepth = CommonTools.ParseDouble(strValues[10]);
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
                double[] arr = new double[] {Lat, Lon};
                dvRetVect = DenseVector.OfEnumerable(arr);
            }
            else if (dataSource == GPSdatasources.IOFFEvesselDataServer)
            {
                double[] arr = new double[]
                {Lat, Lon, IOFFEdataHeadingTrue, IOFFEdataHeadingGyro, IOFFEdataSpeedKnots, IOFFEdataDepth};
                dvRetVect = DenseVector.OfEnumerable(arr);
            }

            dvRetVect = DenseVector.OfEnumerable(dvRetVect.Concat(new double[] {devID}));

            return dvRetVect;
        }




        public DenseVector ToDenseVectorIncludingGPSdatetimeUTC()
        {
            DenseVector dvRetVect = null;

            if (dataSource == GPSdatasources.CloudCamArduinoGPS)
            {
                double[] arr = new double[] {Lat, Lon};
                dvRetVect = DenseVector.OfEnumerable(arr);
            }
            else if (dataSource == GPSdatasources.IOFFEvesselDataServer)
            {
                double[] arr = new double[]
                {Lat, Lon, IOFFEdataHeadingTrue, IOFFEdataHeadingGyro, IOFFEdataSpeedKnots, IOFFEdataDepth};
                dvRetVect = DenseVector.OfEnumerable(arr);
            }

            dvRetVect = DenseVector.OfEnumerable(dvRetVect.Concat(new double[] {devID, dateTimeUTC.Ticks}));

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
            List<DenseVector> lDVdata =
                new List<GPSdata>(lGPSData).ConvertAll(gpsDat => gpsDat.ToDenseVector());
            IEnumerable<int> lDVdataVectorsLenth = lDVdata.ConvertAll<int>(dv => dv.Count);
            int maxVectorLength = lDVdataVectorsLenth.Max();
            lDVdata = lDVdata.ConvertAll(dv =>
            {
                if (dv.Count < maxVectorLength)
                {
                    List<double> dvValuesList = new List<double>(dv);
                    DenseVector dvValuesToAppend = DenseVector.Create(maxVectorLength - dv.Count, 0.0d);
                    dvValuesList.AddRange(new List<double>(dvValuesToAppend));
                    return DenseVector.OfEnumerable(dvValuesList);
                }
                else return dv;
            });
            DenseMatrix dmRetMatr = DenseMatrix.OfRows(lDVdata.Count(), maxVectorLength, lDVdata);
            return dmRetMatr;
        }




        public GPSdata(double doubleLat, double doubleLon)
        {
            try
            {
                latHemisphere = (doubleLat < 0) ? ("S") : ("N");
                double doubleLatInternal = Math.Abs(doubleLat);
                lonHemisphere = (doubleLon < 0) ? ("W") : ("E");
                double doubleLonInternal = Math.Abs(doubleLon);

                int intLat = (int) doubleLatInternal; // Truncate the decimals
                double t1 = (doubleLatInternal - intLat)*60;
                int minLat = (int) t1;
                double secLat = (t1 - minLat)*60;
                lat = intLat*100.0d + minLat + secLat/100.0d;

                int intLon = (int) doubleLonInternal; // Truncate the decimals
                double t2 = (doubleLonInternal - intLon)*60;
                int minLon = (int) t2;
                double secLon = (t2 - minLon)*60;
                lon = intLon*100.0d + minLon + secLon/100.0d;
                // CommonTools.ParseDouble("" + intLon.ToString() + minLon.ToString("d02") + "," + secLon.ToString("F02"));
            }
            catch (Exception)
            {
                validGPSdata = false;
                return;
            }
        }




        public GPSdata(IEnumerable<double> source)
        {
            if (source.Count() == 2)
            {
                try
                {
                    lat = source.ElementAt(0);
                    if (Math.Abs(lat) > 9000.0d)
                    {
                        validGPSdata = false;
                        return;
                    }
                    latHemisphere = (lat >= 0.0d) ? ("N") : ("S");
                    lat = Math.Abs(lat);

                    lon = source.ElementAt(1);
                    if (Math.Abs(lon) > 18000.0d)
                    {
                        validGPSdata = false;
                        return;
                    }
                    lonHemisphere = (lon >= 0.0d) ? ("E") : ("W");
                    lon = Math.Abs(lon);
                }
                catch (Exception)
                {
                    validGPSdata = false;
                    return;
                }
            }
            else if (source.Count() == 3) // devID specified
            {
                try
                {
                    lat = source.ElementAt(0);
                    if (Math.Abs(lat) > 9000.0d)
                    {
                        validGPSdata = false;
                        return;
                    }
                    latHemisphere = (lat >= 0.0d) ? ("N") : ("S");
                    lat = Math.Abs(lat);

                    lon = source.ElementAt(1);
                    if (Math.Abs(lon) > 18000.0d)
                    {
                        validGPSdata = false;
                        return;
                    }
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
                    if (Math.Abs(lat) > 9000.0d)
                    {
                        validGPSdata = false;
                        return;
                    }
                    latHemisphere = (lat >= 0.0d) ? ("N") : ("S");
                    lat = Math.Abs(lat);

                    lon = source.ElementAt(1);
                    if (Math.Abs(lon) > 18000.0d)
                    {
                        validGPSdata = false;
                        return;
                    }
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
                    if (Math.Abs(lat) > 9000.0d)
                    {
                        validGPSdata = false;
                        return;
                    }
                    latHemisphere = (lat >= 0.0d) ? ("N") : ("S");
                    lat = Math.Abs(lat);

                    lon = source.ElementAt(1);
                    if (Math.Abs(lon) > 18000.0d)
                    {
                        validGPSdata = false;
                        return;
                    }
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
                    .Where(newgpsDatum => (newgpsDatum != null) && (newgpsDatum.validGPSdata))
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



        public AzimuthZenithAngle SunZenithAzimuth(out SPA spaCalcObject)
        {
            spaCalcObject = new SPA(dateTimeUTC.Year, dateTimeUTC.Month, dateTimeUTC.Day, dateTimeUTC.Hour,
                        dateTimeUTC.Minute, dateTimeUTC.Second, (float)LonDec, (float)LatDec,
                        (float)SPAConst.DeltaT(dateTimeUTC));
            int res = spaCalcObject.spa_calculate();
            AzimuthZenithAngle sunPositionSPAext = new AzimuthZenithAngle(spaCalcObject.spa.azimuth,
                spaCalcObject.spa.zenith);
            return sunPositionSPAext;
        }
    }






    public class MeteoData : CsvExportable
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



        #region for CSV export

        public double Pressure
        {
            get { return pressure; }
        }

        public double RelHumidity
        {
            get { return Rhumidity; }
        }

        public double AirTemperature
        {
            get { return airTemperature; }
        }

        public double WindSpeed
        {
            get { return windSpeed; }
        }

        public double WindDirection
        {
            get { return windDirection; }
        }

        public double WaterTemperature
        {
            get { return waterTemperature; }
        }

        public double WaterSalinity
        {
            get { return waterSalinity; }
        }

        #endregion




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
                        airTemperature = CommonTools.ParseDouble(datumSubstr);
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
                        pressure = CommonTools.ParseDouble(datumSubstr);
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
                        windSpeed = CommonTools.ParseDouble(datumSubstr);
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
                        windDirection = CommonTools.ParseDouble(datumSubstr);
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
                        Rhumidity = CommonTools.ParseDouble(datumSubstr);
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
                        waterTemperature = CommonTools.ParseDouble(strWaterConsistency.Substring(idx1 + 8, idx2 - 8 - idx1));
                        int idx3 = strWaterConsistency.IndexOf("<Field1>");
                        int idx4 = strWaterConsistency.IndexOf("</Field1>");
                        waterSalinity = CommonTools.ParseDouble(strWaterConsistency.Substring(idx3 + 8, idx4 - 8 - idx3));
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

            //    airTemperature = CommonTools.ParseDouble(sunstrings[1].Trim());
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
            //    pressure = CommonTools.ParseDouble(sunstrings[1].Trim());
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
            //    windSpeed = CommonTools.ParseDouble(sunstrings[1].Trim());
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
            //    windDirection = CommonTools.ParseDouble(sunstrings[1].Trim());
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
            //    Rhumidity = CommonTools.ParseDouble(sunstrings[1].Trim());
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
            //    waterTemperature = CommonTools.ParseDouble(strWaterConsistency.Substring(idx1 + 8, idx2 - 8 - idx1));
            //    int idx3 = strWaterConsistency.IndexOf("<Field1>");
            //    int idx4 = strWaterConsistency.IndexOf("</Field1>");
            //    waterSalinity = CommonTools.ParseDouble(strWaterConsistency.Substring(idx3 + 8, idx4 - 8 - idx3));
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
            List<T> lRet = new List<T>();
            int counter = 0;
            bool succeess = false;
            while (!succeess)
            {
                try
                {
                    lRet = queue.ToList();
                    succeess = true;
                }
                catch (Exception ex)
                {
                    succeess = false;
                    counter++;
                }
            }
            lRet.Reverse();
            return lRet;
        }


        public T[] ToArray()
        {
            List<T> lRet = new List<T>();
            int counter = 0;
            bool succeess = false;
            while (!succeess)
            {
                try
                {
                    lRet = queue.ToList();
                    succeess = true;
                }
                catch (Exception ex)
                {
                    succeess = false;
                    counter++;
                }
            }

            lRet.Reverse();
            return lRet.ToArray();
        }
    }





    public class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
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
            try
            {
                var item = base.Dequeue();
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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





    [Serializable]
    public enum SunDiskCondition
    {
        NoSun,
        Sun0,
        Sun1,
        Sun2,
        Defect,
        Undefined
    }



    [Serializable]
    public class SunDiskConditionData : CsvExportable
    {
        public string filename { get; set; }
        public SunDiskCondition sunDiskCondition { get; set; }

        public SunDiskConditionData()
        {
            filename = "";
            sunDiskCondition = SunDiskCondition.NoSun;
        }


        public SunDiskConditionData(string fName, SunDiskCondition sDiskCondition)
        {
            filename = fName;
            sunDiskCondition = sDiskCondition;
        }


        public void ShrinkFileName()
        {
            filename = Path.GetFileName(filename);
        }



        public static int MatlabNumeralSDC(SunDiskCondition sdc)
        {
            switch (sdc)
            {
                case SunDiskCondition.NoSun:
                    return 4;
                    break;
                case SunDiskCondition.Sun0:
                    return 1;
                    break;
                case SunDiskCondition.Sun1:
                    return 2;
                    break;
                case SunDiskCondition.Sun2:
                    return 3;
                    break;
                case SunDiskCondition.Defect:
                    return 0;
                    break;
                default:
                    return 0;
            }
        }




        public static List<SunDiskCondition> MatlabEnumeratedSDCorderedList()
        {
            List<int> sdcValuesNumbers = new List<int>();
            for (int i = 1; i < 5; i++)
            {
                sdcValuesNumbers.Add(i);
            }
            return sdcValuesNumbers.ConvertAll(MatlabSDCenum);
        }





        public static SunDiskCondition MatlabSDCenum(int sdcInt)
        {
            switch (sdcInt)
            {
                case 4:
                    return SunDiskCondition.NoSun;
                    break;
                case 1:
                    return SunDiskCondition.Sun0;
                    break;
                case 2:
                    return SunDiskCondition.Sun1;
                    break;
                case 3:
                    return SunDiskCondition.Sun2;
                    break;
                default:
                    return SunDiskCondition.Defect;
            }
        }




        public static SunDiskCondition NumeralSourceToSDCenum(int sdcInt)
        {
            switch (sdcInt)
            {
                case -1:
                    return SunDiskCondition.NoSun;
                    break;
                case 0:
                    return SunDiskCondition.Sun0;
                    break;
                case 1:
                    return SunDiskCondition.Sun1;
                    break;
                case 2:
                    return SunDiskCondition.Sun2;
                    break;
                default:
                    return SunDiskCondition.Defect;
            }
        }








        #region // obsolete - moved to CsvExportable abstract class
        //public string ToCSV()
        //{
        //    List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
        //    List<string> lStrValues = new List<string>();
        //    string retStr = "";

        //    foreach (string propertyName in lStrPropertiesNames)
        //    {

        //        object propValue = GetType().GetProperty(propertyName).GetValue(this, null);
        //        if ((propValue.GetType() == typeof(double)) || (propValue.GetType() == typeof(float)))
        //        {
        //            lStrValues.Add(propValue.ToString().Replace(",", "."));
        //        }
        //        else
        //        {
        //            lStrValues.Add(propValue.ToString());
        //        }

        //    }

        //    retStr = String.Join(",", lStrValues.ToArray<string>());
        //    return retStr;
        //}




        //public string CSVHeader()
        //{
        //    List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
        //    List<string> lStrHeaders = new List<string>();
        //    string retStr = "";

        //    foreach (string propertyName in lStrPropertiesNames)
        //    {
        //        lStrHeaders.Add(propertyName);
        //    }

        //    retStr = String.Join(",", lStrHeaders.ToArray<string>());
        //    return retStr;
        //}
        #endregion // obsolete - moved to CsvExportable abstract class
    }




    [Serializable]
    public class SkyImageMedianPerc5Data : CsvExportable
    {
        public string FileName { get; set; }
        public double GrIxStatsMedian { get; set; }
        public double GrIxStatsPerc5 { get; set; }


        public SkyImageMedianPerc5Data()
        {
            FileName = "";
            GrIxStatsMedian = 0.0d;
            GrIxStatsPerc5 = 0.0d;
        }


        public SkyImageMedianPerc5Data(string inFName, double median, double perc5)
        {
            FileName = inFName;
            GrIxStatsMedian = median;
            GrIxStatsPerc5 = perc5;
        }


        public void ShrinkFileName()
        {
            FileName = Path.GetFileName(FileName);
        }



        #region // obsolete - moved to CsvExportable abstract class

        //public string ToCSV()
        //{
        //    List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
        //    List<string> lStrValues = new List<string>();
        //    string retStr = "";

        //    foreach (string propertyName in lStrPropertiesNames)
        //    {

        //        object propValue = GetType().GetProperty(propertyName).GetValue(this, null);
        //        if ((propValue.GetType() == typeof(double)) || (propValue.GetType() == typeof(float)))
        //        {
        //            lStrValues.Add(propValue.ToString().Replace(",", "."));
        //        }
        //        else
        //        {
        //            lStrValues.Add(propValue.ToString());
        //        }

        //    }

        //    retStr = String.Join(",", lStrValues.ToArray<string>());
        //    return retStr;
        //}




        //public string CSVHeader()
        //{
        //    List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
        //    List<string> lStrHeaders = new List<string>();
        //    string retStr = "";

        //    foreach (string propertyName in lStrPropertiesNames)
        //    {
        //        lStrHeaders.Add(propertyName);
        //    }

        //    retStr = String.Join(",", lStrHeaders.ToArray<string>());
        //    return retStr;
        //}

        #endregion // obsolete - moved to CsvExportable abstract class
    }









    [Serializable]
    public class SkyImageVariableStatsData : ICsvExportable
    {
        public string varname { get; set; }
        public string FileName { get; set; }
        public List<FieldPercentileValue> lTplFieldPercentiles { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double mean { get; set; }
        public double variance { get; set; }
        public double stdev { get; set; }
        public double skewness { get; set; }
        public double kurtosis { get; set; }
        public double rms { get; set; }


        public SkyImageVariableStatsData()
        {
        }


        public string ToCSV()
        {
            List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
            List<string> lStrValues = new List<string>();
            string retStr = "";

            foreach (string propertyName in lStrPropertiesNames)
            {
                if (propertyName == "varname")
                {
                    continue;
                }
                if (propertyName == "lTplFieldPercentiles")
                {
                    foreach (FieldPercentileValue tplFieldPercentile in lTplFieldPercentiles)
                    {
                        lStrValues.Add(tplFieldPercentile.percValue.ToString().Replace(",", "."));
                    }
                }
                else
                {
                    object propValue = GetType().GetProperty(propertyName).GetValue(this, null);
                    if ((propValue.GetType() == typeof(double)) || (propValue.GetType() == typeof(float)))
                    {
                        lStrValues.Add(propValue.ToString().Replace(",", "."));
                    }
                    else
                    {
                        lStrValues.Add(propValue.ToString());
                    }

                }
            }

            retStr = String.Join(",", lStrValues.ToArray<string>());
            return retStr;
        }




        public IEnumerable<double> ToRawDoubleValuesEnumerable()
        {
            List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
            List<double> lValues = new List<double>();
            string retStr = "";

            foreach (string propertyName in lStrPropertiesNames)
            {
                if (propertyName == "varname")
                {
                    continue;
                }
                if (propertyName == "lTplFieldPercentiles")
                {
                    foreach (FieldPercentileValue tplFieldPercentile in lTplFieldPercentiles)
                    {
                        lValues.Add(tplFieldPercentile.percValue);
                    }
                }
                else
                {
                    object propValue = GetType().GetProperty(propertyName).GetValue(this, null);
                    if ((propValue.GetType() == typeof(double)) || (propValue.GetType() == typeof(float)))
                    {
                        lValues.Add(Convert.ToDouble(propValue));
                    }
                }
            }


            return lValues;
        }




        public string CSVHeader()
        {
            List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
            List<string> lStrHeaders = new List<string>();
            string retStr = "";

            foreach (string propertyName in lStrPropertiesNames)
            {
                if (propertyName == "varname")
                {
                    continue;
                }
                if (propertyName == "lTplFieldPercentiles")
                {
                    foreach (FieldPercentileValue tplFieldPercentile in lTplFieldPercentiles)
                    {
                        lStrHeaders.Add(varname + "_perc_" + tplFieldPercentile.percIndex);
                    }
                }
                else
                {
                    lStrHeaders.Add(varname + "_" + propertyName);
                }
            }

            retStr = String.Join(",", lStrHeaders.ToArray<string>());
            return retStr;
        }



        public IEnumerable<string> DoubleVariablesNames()
        {
            List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
            List<string> lStrHeaders = new List<string>();

            foreach (string propertyName in lStrPropertiesNames)
            {
                if (propertyName == "varname")
                {
                    continue;
                }
                if (propertyName == "lTplFieldPercentiles")
                {
                    foreach (FieldPercentileValue tplFieldPercentile in lTplFieldPercentiles)
                    {
                        lStrHeaders.Add(varname + "_perc_" + tplFieldPercentile.percIndex);
                    }
                }
                else
                {
                    object propValue = GetType().GetProperty(propertyName).GetValue(this, null);
                    if ((propValue.GetType() == typeof(double)) || (propValue.GetType() == typeof(float)))
                    {
                        lStrHeaders.Add(varname + "_" + propertyName);
                    }
                }
            }

            return lStrHeaders;
        }
    }



    public class FieldPercentileValue
    {
        public int percIndex { get; set; }
        public double percValue { get; set; }

        public FieldPercentileValue()
        {
        }

        public FieldPercentileValue(int idx, double val)
        {
            percIndex = idx;
            percValue = val;
        }
    }



    [Serializable]
    public class SkyImageIndexesStatsData : ICsvExportable
    {
        public string FileName { get; set; }
        public SkyImageVariableStatsData grixStatsData { get; set; }
        public SkyImageVariableStatsData brightnessStatsData { get; set; }
        public SkyImageVariableStatsData rStatsData { get; set; }
        public SkyImageVariableStatsData gStatsData { get; set; }
        public SkyImageVariableStatsData bStatsData { get; set; }

        public SkyImageIndexesStatsData()
        {
        }




        public void ShrinkFileName()
        {
            FileName = Path.GetFileName(FileName);
            grixStatsData.FileName = Path.GetFileName(grixStatsData.FileName);
            brightnessStatsData.FileName = Path.GetFileName(brightnessStatsData.FileName);
            rStatsData.FileName = Path.GetFileName(rStatsData.FileName);
            gStatsData.FileName = Path.GetFileName(gStatsData.FileName);
            bStatsData.FileName = Path.GetFileName(bStatsData.FileName);
        }


        public string ToCSV()
        {
            string retStr = "";
            List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
            List<string> lStrValues = new List<string>();

            foreach (string propertyName in lStrPropertiesNames)
            {
                object propValue = GetType().GetProperty(propertyName).GetValue(this, null);
                if (propValue.GetType() == typeof(SkyImageVariableStatsData))
                {

                    lStrValues.Add(((SkyImageVariableStatsData)propValue).ToCSV());
                }
                else
                {
                    lStrValues.Add(propValue.ToString());
                }
            }

            retStr = String.Join(",", lStrValues.ToArray<string>());
            return retStr;
        }




        public IEnumerable<double> ToRawDoubleValuesEnumerable()
        {
            string retStr = "";
            List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
            List<double> lStrValues = new List<double>();

            foreach (string propertyName in lStrPropertiesNames)
            {
                object propValue = GetType().GetProperty(propertyName).GetValue(this, null);
                if (propValue.GetType() == typeof(SkyImageVariableStatsData))
                {

                    lStrValues.AddRange(((SkyImageVariableStatsData)propValue).ToRawDoubleValuesEnumerable());
                }
            }

            return lStrValues;
        }




        public string CSVHeader()
        {
            List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
            List<string> lStrHeaders = new List<string>();
            string retStr = "";

            foreach (string propertyName in lStrPropertiesNames)
            {
                object propValue = GetType().GetProperty(propertyName).GetValue(this, null);
                if (propValue.GetType() == typeof(SkyImageVariableStatsData))
                {
                    lStrHeaders.Add(((SkyImageVariableStatsData)propValue).CSVHeader());
                }
                else
                {
                    lStrHeaders.Add(propertyName);
                }
            }

            retStr = String.Join(",", lStrHeaders.ToArray<string>());
            return retStr;
        }





        public IEnumerable<string> DoubleVariablesNames()
        {
            List<string> lStrPropertiesNames = ServiceTools.GetPropertiesNamesOfClass(this);
            List<string> lStrHeaders = new List<string>();
            string retStr = "";

            foreach (string propertyName in lStrPropertiesNames)
            {
                object propValue = GetType().GetProperty(propertyName).GetValue(this, null);
                if (propValue.GetType() == typeof(SkyImageVariableStatsData))
                {
                    lStrHeaders.AddRange(((SkyImageVariableStatsData)propValue).DoubleVariablesNames());
                }
            }


            return lStrHeaders;
        }
    }





    public class ImageStatsDataCalculationResult
    {
        // currentFullFileName, mp5dt, true, grixyrgbStatsData, stopwatch, procStart, procEnd
        public string imgFilename { get; set; }
        public Stopwatch stopwatch { get; set; }
        public TimeSpan procTotalProcessorTimeStart { get; set; }
        public TimeSpan procTotalProcessorTimeEnd { get; set; }
        public bool calcResult { get; set; }
        public Exception exception { get; set; }
        public SkyImageMedianPerc5Data mp5Result { get; set; }
        public SkyImageIndexesStatsData grixyrgbStatsData { get; set; }


        public ImageStatsDataCalculationResult()
        {
            imgFilename = "";
            stopwatch = null;
            procTotalProcessorTimeStart = new TimeSpan(0);
            procTotalProcessorTimeEnd = new TimeSpan(0);
            calcResult = false;
            exception = null;
            mp5Result = null;
            grixyrgbStatsData = null;
        }
    }






    public class ImagesProcessingData
    {
        public string filename
        {
            get; set;
        }


        public string concurrentDataXMLfile
        {
            get; set;
        }



        public ConcurrentData concurrentData { get; set; }



        public string grixyrgbStatsXMLfile
        {
            get; set;
        }



        public SkyImageIndexesStatsData grixyrgbStats { get; set; }
    }





    public class ObservedClCoverData
    {
        public DateTime dt { get; set; }
        public int CloudCoverTotal { get; set; }
        public int CloudCoverLower { get; set; }

        public ObservedClCoverData() { }

        public ObservedClCoverData(IEnumerable<string> csvData)
        {
            dt = DateTime.Parse(csvData.ElementAt(0), null, System.Globalization.DateTimeStyles.AdjustToUniversal);
            CloudCoverTotal = Convert.ToInt32(csvData.ElementAt(1));
            CloudCoverLower = Convert.ToInt32(csvData.ElementAt(2));
        }
    }




    public class PredictedCloudCoverData
    {
        public DateTime dateTimeUTC { get; set; }
        public int CloudCoverTotal { get; set; }
        public int CloudCoverLower { get; set; }

        public PredictedCloudCoverData() { }

        public PredictedCloudCoverData(IEnumerable<string> csvData)
        {
            dateTimeUTC = DateTime.Parse(csvData.ElementAt(0), null,
                System.Globalization.DateTimeStyles.AdjustToUniversal);
            CloudCoverTotal = Convert.ToInt32(csvData.ElementAt(1));
            CloudCoverLower = Convert.ToInt32(csvData.ElementAt(2));
        }
    }




    public class SkyImagesDataWith_Concurrent_Stats_CloudCover
    {
        public string skyImageFullFileName { get; set; }
        public string skyImageFileName { get; set; }
        public DateTime currImageDateTime { get; set; }
        public ObservedClCoverData observedCloudCoverData { get; set; }
        public string concurrentDataXMLfile{ get; set; }
        public ConcurrentData concurrentData { get; set; }
        public string grixyrgbStatsXMLfile{get; set;}
        public SkyImageIndexesStatsData grixyrgbStats { get; set; }


        public SkyImagesDataWith_Concurrent_Stats_CloudCover() { }
    }




    public class SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC
    {
        public string skyImageFullFileName { get; set; }
        public string skyImageFileName { get; set; }
        public DateTime currImageDateTime { get; set; }
        public ObservedClCoverData observedCloudCoverData { get; set; }
        public string concurrentDataXMLfile { get; set; }
        public ConcurrentData concurrentData { get; set; }
        public string grixyrgbStatsXMLfile { get; set; }
        public SkyImageIndexesStatsData grixyrgbStats { get; set; }
        public SunDiskCondition SDCvalue { get; set; }
        public List<SDCdecisionProbability> SDCprobabilities { get; set; }


        public SkyImagesDataWith_Concurrent_Stats_CloudCover_SDC() { }
    }




    public class SDCdecisionProbability
    {
        public SunDiskCondition sdc { get; set; }
        public double sdcDecisionProbability { get; set; }
    }


    public class SkyImagesProcessedAndPredictedData
    {
        public string skyImageFullFileName { get; set; }
        public string skyImageFileName { get; set; }
        public DateTime imageShootingDateTimeUTC { get; set; }
        public PredictedCloudCoverData PredictedCC { get; set; }
        public string concurrentDataXMLfile { get; set; }
        public ConcurrentData concurrentData { get; set; }
        public string grixyrgbStatsXMLfile { get; set; }
        public SkyImageIndexesStatsData grixyrgbStats { get; set; }
        public SunDiskCondition PredictedSDC { get; set; }
        public List<SDCdecisionProbability> sdcDecisionProbabilities { get; set; }


        public SkyImagesProcessedAndPredictedData() { }
    }





    public class MissionsObservedData
    {
        public DateTime dateTime { get; set; }
        public double latNMEA { get; set; }
        public double lonNMEA { get; set; }
        public double heading { get; set; }
        public double headingGyro { get; set; }
        public double speedKt { get; set; }
        public double pressure { get; set; }
        public double T_air { get; set; }
        public double T_water { get; set; }
        public double windSpeed { get; set; }
        public double windDirection { get; set; }
        public double relHumidity { get; set; }
        public int swell { get; set; }
        public int CloudCoverTotal { get; set; }
        public int CloudCoverLower { get; set; }
        public string CloudTypesObserved { get; set; }
        public int LowerCloudsIndex { get; set; }
        public int MedCloudsIndex { get; set; }
        public int HighCloudsIndex { get; set; }
        public SunDiskCondition SDC { get; set; }
        public string report { get; set; }


        public MissionsObservedData(){}

        public MissionsObservedData(List<string> observationRecord)
        {
            DateTime outDT = DateTime.UtcNow;
            bool dtRes = DateTime.TryParse(observationRecord[0], out outDT);
            if (dtRes)
            {
                dateTime = outDT;
            }
            else
            {
                throw new InvalidCastException("unable to parse date-time string " + observationRecord[0]);
            }

            try
            {
                latNMEA = CommonTools.ParseDouble(observationRecord[1]);
                lonNMEA = CommonTools.ParseDouble(observationRecord[2]);
                heading = CommonTools.ParseDouble(observationRecord[3]);
                headingGyro = CommonTools.ParseDouble(observationRecord[4]);
                speedKt = CommonTools.ParseDouble(observationRecord[5]);
                pressure = CommonTools.ParseDouble(observationRecord[6]);
                T_air = CommonTools.ParseDouble(observationRecord[7]);
                T_water = CommonTools.ParseDouble(observationRecord[8]);
                windSpeed = CommonTools.ParseDouble(observationRecord[9]);
                windDirection = CommonTools.ParseDouble(observationRecord[10]);
                relHumidity = CommonTools.ParseDouble(observationRecord[11]);
                swell = Convert.ToInt32(CommonTools.ParseDouble(observationRecord[12]));
                CloudCoverTotal = Convert.ToInt32(CommonTools.ParseDouble(observationRecord[13]));
                CloudCoverLower = Convert.ToInt32(CommonTools.ParseDouble(observationRecord[14]));
                CloudTypesObserved = observationRecord[15];
                LowerCloudsIndex = Convert.ToInt32(CommonTools.ParseDouble(observationRecord[16]));
                MedCloudsIndex = Convert.ToInt32(CommonTools.ParseDouble(observationRecord[17]));
                HighCloudsIndex = Convert.ToInt32(CommonTools.ParseDouble(observationRecord[18]));
                int SDCint = Convert.ToInt32(CommonTools.ParseDouble(observationRecord[19]));
                switch (SDCint)
                {
                    case 1:
                        SDC = SunDiskCondition.Sun0;
                        break;
                    case 2:
                        SDC = SunDiskCondition.Sun1;
                        break;
                    case 3:
                        SDC = SunDiskCondition.Sun2;
                        break;
                    case 4:
                        SDC = SunDiskCondition.NoSun;
                        break;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
            
        }
    }
}