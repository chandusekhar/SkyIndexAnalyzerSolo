using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IoffeVesselDataReader
{
    internal class NavData
    {
        public int DateGPS;
        public int curtime;
        public float Lat;
        public float Lon;
        public char SimLat;
        public char SimLon;
        //тут в бинарнике почему-то два пустых байта
        public float Course;
        public float trueHead;
        public float Speed;
        public float Gyro;
        public float Depth;
    }

    public class IoffeVesselNavDataReader
    {

        public static bool ReadNavFile(string InputFilename)
        {
            BinaryReader reader = new BinaryReader(File.Open(InputFilename, FileMode.Open));
            char[] dualmode = reader.ReadChars(4);
            if (new string(dualmode) != "Dual")
            {
                // неправильный файл навигации - данные не в том формате
                return false;
            }


            List<NavData> lFileNavData = new List<NavData>();
            FieldInfo[] fInfoList = typeof(NavData).GetFields();

            while (true)
            {
                NavData curRecord = new NavData();
                foreach (FieldInfo fldInfo in fInfoList)
                {
                    if (fldInfo.FieldType == typeof(int))
                    {
                        try
                        {
                            int value = reader.ReadInt32();
                            fldInfo.SetValue(curRecord, value);
                        }
                        catch (Exception ex)
                        {
                            break;
                        }

                    }
                    if (fldInfo.FieldType == typeof(float))
                    {
                        try
                        {
                            float value = reader.ReadSingle();
                            fldInfo.SetValue(curRecord, value);
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                    if (fldInfo.FieldType == typeof(char))
                    {
                        try
                        {
                            char value = reader.ReadChar();
                            fldInfo.SetValue(curRecord, value);
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }

                    if (fldInfo.Name == "SimLon")
                    {
                        // пропустить два байта
                        reader.Read();
                        reader.Read();
                    }
                }
                lFileNavData.Add(curRecord);
            }

            return true;
        }


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



    }
}
