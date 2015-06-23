using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SkyImagesAnalyzerLibraries
{

    public struct IoffeVesselDualNavData
    {
        public int DateGPS;
        public int curtime;
        public float Lat;
        public float Lon;
        public char SimLat;
        public char SimLon;
        public char fakechar1;
        public char fakechar2;
        public float Course;
        public float trueHead;
        public float Speed;
        public float Gyro;
        public float Depth;
    }



    public class IoffeVesselDualNavDataConverted
    {
        public DateTime dateTime;
        public GPSdata gps;
        public double Course;


        public override string ToString()
        {
            string retStr = "";

            retStr = dateTime.ToString("u").Replace("Z", "") +
                     " : " + gps.ToString();

            return retStr;
        }
    }




    public class IoffeVesselNavDataReader
    {
        public static Tuple<DateTime, DateTime> GetNavFileDateTimeMargins(string InputFilename)
        {
            int bytesCountPerObj = Marshal.SizeOf(typeof(IoffeVesselDualNavData));
            long fileLength = (new FileInfo(InputFilename)).Length;
            if (fileLength < (4 + bytesCountPerObj))
            {
                return null;
            }


            BinaryReader reader = new BinaryReader(File.Open(InputFilename, FileMode.Open, FileAccess.Read, FileShare.Read));
            char[] dualmode = reader.ReadChars(4);
            if (new string(dualmode) != "Dual")
            {
                // неправильный файл навигации - данные не в том формате
                return null;
            }

            byte[] buf1st = reader.ReadBytes(bytesCountPerObj);
            GCHandle handle1st = GCHandle.Alloc(buf1st, GCHandleType.Pinned);
            IoffeVesselDualNavData record1st =
                (IoffeVesselDualNavData)
                    Marshal.PtrToStructure(handle1st.AddrOfPinnedObject(), typeof(IoffeVesselDualNavData));
            DateTime dt1 = ExtractDateTime(record1st);

            reader.BaseStream.Seek(-bytesCountPerObj, SeekOrigin.End);
            byte[] buf2nd = reader.ReadBytes(bytesCountPerObj);
            GCHandle handle2nd = GCHandle.Alloc(buf2nd, GCHandleType.Pinned);
            IoffeVesselDualNavData record2nd =
                (IoffeVesselDualNavData)
                    Marshal.PtrToStructure(handle2nd.AddrOfPinnedObject(), typeof(IoffeVesselDualNavData));
            DateTime dt2 = ExtractDateTime(record2nd);

            reader.Close();

            return new Tuple<DateTime, DateTime>(dt1, dt2);
        }




        private static DateTime ExtractDateTime(IoffeVesselDualNavData navData)
        {
            string dateStr = navData.DateGPS.ToString();
            string timeStr = navData.curtime.ToString("D4");

            int sec = Convert.ToInt32(timeStr.Substring(timeStr.Length - 2));
            timeStr = timeStr.Substring(0, timeStr.Length - 2);
            int min = Convert.ToInt32(timeStr.Substring(timeStr.Length - 2));
            timeStr = timeStr.Substring(0, timeStr.Length - 2);
            int hour = (timeStr.Length > 0) ? (Convert.ToInt32(timeStr)) : (0);

            int year = 2000 + Convert.ToInt32(dateStr.Substring(dateStr.Length - 2, 2));
            dateStr = dateStr.Substring(0, dateStr.Length - 2);
            int month = Convert.ToInt32(dateStr.Substring(dateStr.Length - 2, 2));
            dateStr = dateStr.Substring(0, dateStr.Length - 2);
            int day = Convert.ToInt32(dateStr);

            DateTime dt = new DateTime(year, month, day, hour, min, sec, DateTimeKind.Utc);

            return dt;
        }



        public static List<IoffeVesselDualNavDataConverted> ReadNavFile(string InputFilename) //, bool readOnlyThe1stRecord = false)
        {
            int bytesCountPerObj = Marshal.SizeOf(typeof(IoffeVesselDualNavData));
            if ((new FileInfo(InputFilename)).Length < (4 + bytesCountPerObj))
            {
                return null;
            }
            BinaryReader reader = new BinaryReader(File.Open(InputFilename, FileMode.Open, FileAccess.Read, FileShare.Read));
            char[] dualmode = reader.ReadChars(4);
            if (new string(dualmode) != "Dual")
            {
                // неправильный файл навигации - данные не в том формате
                return null;
            }
            reader.Close();

            byte[] fileData = File.ReadAllBytes(InputFilename);

            reader = new BinaryReader(new MemoryStream(fileData));
            reader.ReadChars(4);

            List<IoffeVesselDualNavData> lFileNavData = new List<IoffeVesselDualNavData>();

            int structCount = Convert.ToInt32((fileData.Count() - 4) / bytesCountPerObj);
            if (structCount == 0)
            {
                return null;
            }
            int readStructs = 0;
            while (true)
            {
                byte[] readBuffer = new byte[bytesCountPerObj];
                readBuffer = reader.ReadBytes(bytesCountPerObj);
                GCHandle handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
                IoffeVesselDualNavData curRecord =
                    (IoffeVesselDualNavData)
                        Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(IoffeVesselDualNavData));
                readStructs++;
                lFileNavData.Add(curRecord);
                //if ((readStructs >= structCount) || (readOnlyThe1stRecord))
                if (readStructs >= structCount)
                {
                    break;
                }
            }



            List<IoffeVesselDualNavDataConverted> lRetList = lFileNavData.ConvertAll(navData =>
            {
                IoffeVesselDualNavDataConverted retNavData = new IoffeVesselDualNavDataConverted();

                #region // -> ExtractDateTime()
                //string dateStr = navData.DateGPS.ToString();
                //string timeStr = navData.curtime.ToString("D4");
                //DateTime dt;
                //if (navData.curtime >= 2400)
                //{
                //    int daysToAdd = 0;
                //    while (navData.curtime >= 2400)
                //    {
                //        navData.curtime -= 2400;
                //        daysToAdd ++;
                //    }
                //    timeStr = (navData.curtime).ToString("D4");
                //    dt = new DateTime(2000 + Convert.ToInt32(dateStr.Substring(dateStr.Length - 2, 2)),
                //        Convert.ToInt32(dateStr.Substring(dateStr.Length - 4, 2)),
                //        Convert.ToInt32(dateStr.Substring(0, dateStr.Length - 4)),
                //        Convert.ToInt32(timeStr.Substring(0, 2)),
                //        Convert.ToInt32(timeStr.Substring(2, 2)),
                //        0);
                //    dt = dt.AddDays(daysToAdd);
                //}
                //else
                //{
                //    dt = new DateTime(2000 + Convert.ToInt32(dateStr.Substring(dateStr.Length - 2, 2)),
                //        Convert.ToInt32(dateStr.Substring(dateStr.Length - 4, 2)),
                //        Convert.ToInt32(dateStr.Substring(0, dateStr.Length - 4)),
                //        Convert.ToInt32(timeStr.Substring(0, 2)),
                //        Convert.ToInt32(timeStr.Substring(2, 2)),
                //        0);
                //}
                #endregion // -> ExtractDateTime()

                retNavData.dateTime = ExtractDateTime(navData);
                GPSdata curGPS = new GPSdata()
                {
                    GPSstring = "",
                    lat = (double)navData.Lat,
                    latHemisphere = "" + navData.SimLat,
                    lon = (double)navData.Lon,
                    lonHemisphere = "" + navData.SimLon,
                    dateTimeUTC = retNavData.dateTime,
                    validGPSdata = true,
                    IOFFEdataHeadingTrue = navData.trueHead,
                    IOFFEdataHeadingGyro = navData.Gyro,
                    IOFFEdataSpeedKnots = navData.Speed,
                    IOFFEdataDepth = navData.Depth,
                    dataSource = GPSdatasources.IOFFEvesselDataServer
                };
                retNavData.gps = curGPS;
                retNavData.Course = navData.Course;
                return retNavData;
            });


            return lRetList;
        }





        public static IoffeVesselDualNavDataConverted GetNavDataByDatetime(string navDataFilesPath, DateTime dt)
        {
            if (navDataFilesPath == "")
            {
                return null;
            }
            else if (!Directory.Exists(navDataFilesPath))
            {
                return null;
            }

            TimeSpan span = new TimeSpan(0, 2, 0);


            List<IoffeVesselDualNavDataConverted> lAllNavData = new List<IoffeVesselDualNavDataConverted>();
            string[] sNavFilenames = Directory.GetFiles(navDataFilesPath, "*.nv2", SearchOption.AllDirectories);
            if (!sNavFilenames.Any())
            {
                return null;
            }

            foreach (string navFilename in sNavFilenames)
            {
                Tuple<DateTime, DateTime> timeSpan =
                    GetNavFileDateTimeMargins(navFilename);
                if (timeSpan == null)
                {
                    continue;
                }

                if ((dt < timeSpan.Item1) || (dt > timeSpan.Item2))
                {
                    continue;
                }

                List<IoffeVesselDualNavDataConverted> dataHasBeenRead = ReadNavFile(navFilename);
                if (dataHasBeenRead == null)
                {
                    continue;
                }
                lAllNavData.AddRange(dataHasBeenRead);
            }

            if (!lAllNavData.Any())
            {
                return null;
            }

            lAllNavData.Sort((gpsRecord1, gpsRecord2) =>
            {
                double dev1 = Math.Abs((gpsRecord1.gps.dateTimeUTC - dt).TotalMilliseconds);
                double dev2 = Math.Abs((gpsRecord2.gps.dateTimeUTC - dt).TotalMilliseconds);
                return (dev1 >= dev2) ? (1) : (-1);
            });
            IoffeVesselDualNavDataConverted retData = new IoffeVesselDualNavDataConverted()
            {
                dateTime = lAllNavData[0].dateTime,
                gps = lAllNavData[0].gps,
            };

            return retData;
        }
    }




    #region // meteo data reader
    //-- Преобразование выбранного файла
    //bool  TMainForm::ConvertMeteoFile(string InF,string OutF)
    //{
    //if(FileExists(OutF )){
    //  string r = "Выходной файл "+ ExtractFileName(OutFile.Text)+" уже существует! Заменить его?";
    //  int k = MediaTypeNames.Application.MessageBox(r.c_str(),"Предупреждение",MB_ICONEXCLAMATION+MB_YESNO);
    //    if(k != mrYes) return false;
    //    }

    //FILE *in,*out;
    //  in = fopen(InF.c_str(),"rb");
    //  if(in == NULL){
    //  string p = "Не удалось открыть файл данных "+InF;
    //   ShowMessage(p);
    //    return false;
    //  }
    //  //-кол-во структур в файле
    //  long filesize = filelength(fileno(in))/sizeof(WRITE_METEO_DATA);

    //  //-- Открыть выходной файл данных (текстовый)
    //   out = fopen(OutF.c_str(),"w+t");
    //   if(in == NULL){
    //   string p = "Не удалось открыть файл для записи!";
    //    ShowMessage(p);
    //     return false;
    //  }
    //  if(FormSet.StringManual.Checked) fprintf(out,"%s\n",ManualMeteo.c_str());
    //  WRITE_METEO_DATA dat;
    //  int s=0;
    //  string Res="",TAir,SWind,DWind,dewpoint,Press,hum,W_chill,Dep;
    //   double pos = 0,step = (double)CGauge.MaxValue/(double)filesize;

    //   //----читаем данные и пишем в выходной файл

    //  while(s = fread(&dat,sizeof(dat),1,in)){
    //    try{

    //    CData = IntToStr(dat.Data);                  //текущая дата
    //    CData = AddChar('0',CData,6);
    //    //--Разделяем пробелами и меняем местами год и месяц
    //    Res = CData.SubString(5,2)+" "+CData.SubString(3,2)+" "+CData.SubString(1,2);
    //    CData = Res;  Res="";
    //    Ctime = IntToStr(dat.curtime);               //текущее время
    //    Ctime = AddChar('0',Ctime,6);
    //    Res = Ctime.SubString(1,2)+" "+Ctime.SubString(3,2)+" "+Ctime.SubString(5,2);
    //    Ctime = Res; Res="";
    //    TAir = FloatToStrF(dat.TAir,ffFixed,5,1);    //температура воздуха  (float)
    //    TAir = AddChar('0',TAir,3);
    //    SWind = FloatToStrF(dat.SWind,ffFixed,5,1);  // скорость ветра       (float)
    //    SWind = AddChar('0',SWind,3);
    //    DWind = IntToStr(dat.DWind);                 // направление ветра    (int)
    //    DWind = AddChar('0',DWind,3);
    //    dewpoint = FloatToStrF(dat.dewpoint,ffFixed,6,1);           // точка росы         (int)
    //    Press = FloatToStrF(dat.Press,ffFixed,6,1);                 // давление           (int)
    //    hum = IntToStr(dat.hum);                     // влажность          (int)
    //    W_chill = FloatToStrF(dat.W_chill,ffFixed,6,1);             //приведенная температура (int)
    //    Dep = IntToStr(dat.Dep);                     // кол-во осадков           (int)
    //   }
    //   catch(...){     continue;   }
    //   Res =CData+", "+Ctime+",    "+TAir+",          "+SWind+",           "
    //   +DWind+",       "+dewpoint+",      "
    //   +Press+",      "+hum+",   "+W_chill+",     "+Dep;
    //    fprintf(out,"%s\n",Res.c_str());
    //     pos += step;
    //     CGauge.Progress = (int)pos;
    //      Res="";
    //   };
    //    fclose(out);
    //     fclose(in);
    //      CGauge.Progress = 0;
    //        MessageBeep(MB_ICONASTERISK);  StartConBut.Enabled = false;
    //         return true;
    //}
    #endregion // meteo data reader





    public struct IoffeVesselMetData
    {
        public int curtime;
        public int Date;
        public float tAir;
        public float windSpeed;
        public int windDirection;
        public float dewPoint;
        public float pressure;
        public int rHumidity;
        public float chillTemp;
        public int percipitation;
    }


    public class IoffeVesselMetDataConverted
    {
        public DateTime dateTime;
        public float tAir;
        public float windSpeed;
        public int windDirection;
        public float dewPoint;
        public float pressure;
        public int rHumidity;
        public float chillTemp;
        public int percipitation;


        public override string ToString()
        {
            string retStr = "";

            retStr += dateTime.ToString("u").Replace("Z", "") +
                      " : Ta=" + tAir.ToString("F2") +
                      " Wspd=" + windSpeed.ToString("F2") +
                      " Wdir=" + windDirection.ToString() +
                      " P=" + pressure.ToString("F1") +
                      " Rhum=" + rHumidity.ToString("F1");

            return retStr;
        }
    }



    public class IoffeVesselMeteoDataReader
    {
        public static Tuple<DateTime, DateTime> GetMetFileDateTimeMargins(string InputFilename)
        {
            int bytesCountPerObj = Marshal.SizeOf(typeof(IoffeVesselMetData));
            long fileLength = (new FileInfo(InputFilename)).Length;
            if (fileLength < bytesCountPerObj)
            {
                return null;
            }


            BinaryReader reader = new BinaryReader(File.Open(InputFilename, FileMode.Open, FileAccess.Read, FileShare.Read));

            byte[] buf1st = reader.ReadBytes(bytesCountPerObj);
            GCHandle handle1st = GCHandle.Alloc(buf1st, GCHandleType.Pinned);
            IoffeVesselMetData record1st =
                (IoffeVesselMetData)
                    Marshal.PtrToStructure(handle1st.AddrOfPinnedObject(), typeof(IoffeVesselMetData));
            DateTime dt1 = ExtractDateTime(record1st);

            reader.BaseStream.Seek(-bytesCountPerObj, SeekOrigin.End);
            byte[] buf2nd = reader.ReadBytes(bytesCountPerObj);
            GCHandle handle2nd = GCHandle.Alloc(buf2nd, GCHandleType.Pinned);
            IoffeVesselMetData record2nd =
                (IoffeVesselMetData)
                    Marshal.PtrToStructure(handle2nd.AddrOfPinnedObject(), typeof(IoffeVesselMetData));
            DateTime dt2 = ExtractDateTime(record2nd);

            reader.Close();

            return new Tuple<DateTime, DateTime>(dt1, dt2);
        }




        private static DateTime ExtractDateTime(IoffeVesselMetData meteoData)
        {
            string dateStr = meteoData.Date.ToString();
            string timeStr = meteoData.curtime.ToString("D6");

            int sec = Convert.ToInt32(timeStr.Substring(timeStr.Length - 2));
            timeStr = timeStr.Substring(0, timeStr.Length - 2);
            int min = Convert.ToInt32(timeStr.Substring(timeStr.Length - 2));
            timeStr = timeStr.Substring(0, timeStr.Length - 2);
            int hour = (timeStr.Length > 0) ? (Convert.ToInt32(timeStr)) : (0);

            int year = 2000 + Convert.ToInt32(dateStr.Substring(dateStr.Length - 2, 2));
            dateStr = dateStr.Substring(0, dateStr.Length - 2);
            int month = Convert.ToInt32(dateStr.Substring(dateStr.Length - 2, 2));
            dateStr = dateStr.Substring(0, dateStr.Length - 2);
            int day = Convert.ToInt32(dateStr);

            DateTime dt = new DateTime(year, month, day, hour, min, sec, DateTimeKind.Utc);

            return dt;
        }



        public static List<IoffeVesselMetDataConverted> ReadMetFile(string InputFilename) //, bool readOnlyThe1stRecord = false)
        {
            int bytesCountPerObj = Marshal.SizeOf(typeof(IoffeVesselMetData));
            if ((new FileInfo(InputFilename)).Length < bytesCountPerObj)
            {
                return null;
            }
            BinaryReader reader = new BinaryReader(File.Open(InputFilename, FileMode.Open, FileAccess.Read, FileShare.Read));

            reader.Close();

            byte[] fileData = File.ReadAllBytes(InputFilename);

            reader = new BinaryReader(new MemoryStream(fileData));

            List<IoffeVesselMetData> lFileMetData = new List<IoffeVesselMetData>();

            int structCount = Convert.ToInt32(fileData.Count() / bytesCountPerObj);
            if (structCount == 0)
            {
                return null;
            }
            int readStructs = 0;
            while (true)
            {
                byte[] readBuffer = new byte[bytesCountPerObj];
                readBuffer = reader.ReadBytes(bytesCountPerObj);
                GCHandle handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
                IoffeVesselMetData curRecord =
                    (IoffeVesselMetData)
                        Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(IoffeVesselMetData));
                readStructs++;
                lFileMetData.Add(curRecord);
                //if ((readStructs >= structCount) || (readOnlyThe1stRecord))
                if (readStructs >= structCount)
                {
                    break;
                }
            }



            List<IoffeVesselMetDataConverted> lRetList = lFileMetData.ConvertAll(meteoData =>
            {
                IoffeVesselMetDataConverted retMeteoData = new IoffeVesselMetDataConverted();

                retMeteoData.dateTime = ExtractDateTime(meteoData);
                retMeteoData.tAir = meteoData.tAir;
                retMeteoData.windSpeed = meteoData.windSpeed;
                retMeteoData.windDirection = meteoData.windDirection;
                retMeteoData.dewPoint = meteoData.dewPoint;
                retMeteoData.pressure = meteoData.pressure;
                retMeteoData.rHumidity = meteoData.rHumidity;
                retMeteoData.chillTemp = meteoData.chillTemp;
                retMeteoData.percipitation = meteoData.percipitation;
                return retMeteoData;
            });


            return lRetList;
        }





        public static IoffeVesselMetDataConverted GetMetDataByDatetime(string metDataFilesPath, DateTime dt)
        {
            if (metDataFilesPath == "")
            {
                return null;
            }
            else if (!Directory.Exists(metDataFilesPath))
            {
                return null;
            }

            TimeSpan span = new TimeSpan(0, 2, 0);


            List<IoffeVesselMetDataConverted> lAllMetData = new List<IoffeVesselMetDataConverted>();
            string[] sMetFilenames = Directory.GetFiles(metDataFilesPath, "*.met", SearchOption.AllDirectories);
            if (!sMetFilenames.Any())
            {
                return null;
            }

            foreach (string metFilename in sMetFilenames)
            {
                Tuple<DateTime, DateTime> timeSpan =
                    GetMetFileDateTimeMargins(metFilename);
                if (timeSpan == null)
                {
                    continue;
                }

                if ((dt < timeSpan.Item1) || (dt > timeSpan.Item2))
                {
                    continue;
                }

                List<IoffeVesselMetDataConverted> dataHasBeenRead = ReadMetFile(metFilename);
                if (dataHasBeenRead == null)
                {
                    continue;
                }
                lAllMetData.AddRange(dataHasBeenRead);
            }

            if (!lAllMetData.Any())
            {
                return null;
            }

            lAllMetData.Sort((gpsRecord1, gpsRecord2) =>
            {
                double dev1 = Math.Abs((gpsRecord1.dateTime - dt).TotalMilliseconds);
                double dev2 = Math.Abs((gpsRecord2.dateTime - dt).TotalMilliseconds);
                return (dev1 >= dev2) ? (1) : (-1);
            });

            return lAllMetData[0];
        }
    }

}
