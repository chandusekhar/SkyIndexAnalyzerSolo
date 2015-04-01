using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using SkyImagesAnalyzerLibraries;

namespace IoffeVesselDataReader
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



    public struct IoffeVesselDualNavDataConverted
    {
        public DateTime dateTime;
        public SkyImagesAnalyzerLibraries.GPSdata gps;
        public double Course;
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
            
            BinaryReader reader = new BinaryReader(File.Open(InputFilename, FileMode.Open));
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
            DateTime dt;
            if (navData.curtime >= 2400)
            {
                int daysToAdd = 0;
                while (navData.curtime >= 2400)
                {
                    navData.curtime -= 2400;
                    daysToAdd++;
                }
                timeStr = (navData.curtime).ToString("D4");
                dt = new DateTime(2000 + Convert.ToInt32(dateStr.Substring(dateStr.Length - 2, 2)),
                    Convert.ToInt32(dateStr.Substring(dateStr.Length - 4, 2)),
                    Convert.ToInt32(dateStr.Substring(0, dateStr.Length - 4)),
                    Convert.ToInt32(timeStr.Substring(0, 2)),
                    Convert.ToInt32(timeStr.Substring(2, 2)),
                    0);
                dt = dt.AddDays(daysToAdd);
            }
            else
            {
                dt = new DateTime(2000 + Convert.ToInt32(dateStr.Substring(dateStr.Length - 2, 2)),
                    Convert.ToInt32(dateStr.Substring(dateStr.Length - 4, 2)),
                    Convert.ToInt32(dateStr.Substring(0, dateStr.Length - 4)),
                    Convert.ToInt32(timeStr.Substring(0, 2)),
                    Convert.ToInt32(timeStr.Substring(2, 2)),
                    0);
            }
            return dt;
        }



        public static List<IoffeVesselDualNavDataConverted> ReadNavFile(string InputFilename) //, bool readOnlyThe1stRecord = false)
        {
            int bytesCountPerObj = Marshal.SizeOf(typeof(IoffeVesselDualNavData));
            if ((new FileInfo(InputFilename)).Length < (4 + bytesCountPerObj))
            {
                return null;
            }
            BinaryReader reader = new BinaryReader(File.Open(InputFilename, FileMode.Open));
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

            int structCount = Convert.ToInt32((fileData.Count() - 4)/bytesCountPerObj);
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
                        Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof (IoffeVesselDualNavData));
                readStructs++;
                lFileNavData.Add(curRecord);
                //if ((readStructs >= structCount) || (readOnlyThe1stRecord))
                if (readStructs >= structCount)
                {
                    break;
                }
            }


            #region // obsolete - too slooooowwwwww
            //FieldInfo[] fInfoList = typeof(IoffeVesselDualNavData).GetFields();

            //while (true)
            //{
            //    IoffeVesselDualNavData curRecord = new IoffeVesselDualNavData();
            //    foreach (FieldInfo fldInfo in fInfoList)
            //    {
            //        if (fldInfo.FieldType == typeof(int))
            //        {
            //            try
            //            {
            //                int value = reader.ReadInt32();
            //                fldInfo.SetValue(curRecord, value);
            //            }
            //            catch (Exception ex)
            //            {
            //                break;
            //            }

            //        }
            //        if (fldInfo.FieldType == typeof(float))
            //        {
            //            try
            //            {
            //                float value = reader.ReadSingle();
            //                fldInfo.SetValue(curRecord, value);
            //            }
            //            catch (Exception ex)
            //            {
            //                break;
            //            }
            //        }
            //        if (fldInfo.FieldType == typeof(char))
            //        {
            //            try
            //            {
            //                char value = reader.ReadChar();
            //                fldInfo.SetValue(curRecord, value);
            //            }
            //            catch (Exception ex)
            //            {
            //                break;
            //            }
            //        }

            //        if (fldInfo.Name == "SimLon")
            //        {
            //            // пропустить два байта
            //            reader.Read();
            //            reader.Read();
            //        }
            //    }
            //    lFileNavData.Add(curRecord);
            //}
            #endregion // obsolete - too slooooowwwwww


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
                    SkyImagesAnalyzerLibraries.GPSdata curGPS = new SkyImagesAnalyzerLibraries.GPSdata()
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
                        dataSource = SkyImagesAnalyzerLibraries.GPSdatasources.IOFFEvesselDataServer
                    };
                    retNavData.gps = curGPS;
                    retNavData.Course = navData.Course;
                    return retNavData;
                });


            return lRetList;
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


    }
}
