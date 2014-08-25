using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace SkyIndexAnalyzerSolo
{
    //класс, представляющий веб камеру
    public class WebCamDevice
    {
        //имя устройства
        public string Name { get; set; }
        //версия
        private string Version;
        //номер
        private short Index;
        //Handle
        private int mCapHwnd = 0;

        [DllImport("avicap32.dll", EntryPoint = "capCreateCaptureWindowA")]
        //получает handle окна
        public static extern int capCreateCaptureWindowA(
            string lpszWindowName, //Нуль-терминальная строка, содержащая имя окна захвата.
            int dwStyle, //стиль окна
            int X, //координата X
            int Y, //координата Y
            int nWidth, //ширина окна
            int nHeight, //высота окна
            int hwndParent, //handle родительского окна
            int nID //идентификатор окна
            );

        [DllImport("avicap32.dll")]
        //получить список установленных устройств видео захвата
        protected static extern bool capGetDriverDescriptionA(
            short wDriverIndex, //индекс драйвера видео захвата. Значение индекса может варьироваться от 0 до 9.
            [MarshalAs(UnmanagedType.VBByRefStr)] ref String lpszName, //указатель на буфер, содержащий соответствующее имя драйвера
            int cbName, //размер (в байтах) буфера lpszName
            [MarshalAs(UnmanagedType.VBByRefStr)] ref String lpszVer, //указатель на буфер, содержащий описание определенного драйвера.
            int cbVer //размер буфера (в байтах), в котором хранится описание драйвера.
            );

        [DllImport("user32", EntryPoint = "SendMessage")]
        //отправляет сообщения
        public static extern int SendMessage(
            int hWnd, //Дескриптор окна, оконная процедура которого примет сообщение
            uint Msg, //Определяет сообщение, которое будет отправлено.
            int wParam, //Определяет дополнительную конкретизирующую сообщение информацию.
            int lParam //Определяет дополнительную конкретизирующую сообщение информацию.
            );

        [DllImport("user32")]
        protected static extern bool SetWindowPos(
            int hWnd,
            int hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags
            );

        //Пользовательское сообщение
        private const int WM_CAP = 0x400;
        //соединение с драйвером устройства видеозахвата
        private const int WM_CAP_DRIVER_CONNECT = 0x40a;
        //разрыв связи с драйвером видеозахвата
        private const int WM_CAP_DRIVER_DISCONNECT = 0x40b;
        //копирование кадра в буффер обмена
        private const int WM_CAP_EDIT_COPY = 0x41e;
        //включение/отключение режима предпросмотра
        private const int WM_CAP_SET_PREVIEW = 0x432;
        //включение/отключение режима оверлей
        private const int WM_CAP_SET_OVERLAY = 0x433;
        //Скорость previewrate
        private const int WM_CAP_SET_PREVIEWRATE = 0x434;
        //Включение/отключение масштабирования
        private const int WM_CAP_SET_SCALE = 0x435;
        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;
        //Установка callback функции  для preview
        private const int WM_CAP_SET_CALLBACK_FRAME = 0x405;
        //Получение одиночного фрейма с драйвера видеозахвата
        private const int WM_CAP_GET_FRAME = 0x43c;
        //Сохранение кадра с камеры в файл.
        private const int WM_CAP_SAVEDIB = 0x419;
        //максимальное количество попыток соединения
        private const int MAX_TRY_CONNECT = 100;

        //запускаем захват видео с данной камеры
        public void Connect(Control picBox)
        {
            //узнаём хэндл
            mCapHwnd = capCreateCaptureWindowA(Index.ToString(), WS_VISIBLE | WS_CHILD, 0, 0, picBox.Width, picBox.Height, picBox.Handle.ToInt32(), 0);
            bool isConnected = false;
            //пробуем запустить устройство
            for (int i = 0; i < MAX_TRY_CONNECT; i++)
                if (SendMessage(mCapHwnd, WM_CAP_DRIVER_CONNECT, Index, 0) > 0)
                {
                    SendMessage(mCapHwnd, WM_CAP_SET_SCALE, -1, 0);
                    SendMessage(mCapHwnd, WM_CAP_SET_PREVIEWRATE, 0x42, 0);
                    SendMessage(mCapHwnd, WM_CAP_SET_PREVIEW, -1, 0);
                    SetWindowPos(mCapHwnd, 1, 0, 0, picBox.Width, picBox.Height, 6);
                    isConnected = true;
                    break;
                }
            if (!isConnected)
                //в случае неудачи сообщаем пользователю
                MessageBox.Show("No device found!");
        }

        //взятие скриншота
        public void TakePicture(Form form)
        {

            SendMessage(mCapHwnd, WM_CAP_GET_FRAME, 0, 0);
            SendMessage(mCapHwnd, WM_CAP_EDIT_COPY, 0, 0);
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "JPG Images|*.jpg";
                dlg.OverwritePrompt = true;
                dlg.ValidateNames = true;
                dlg.DefaultExt = "JPG";
                if (dlg.ShowDialog(form) == DialogResult.OK)
                {
                    Clipboard.GetImage().Save(dlg.FileName, ImageFormat.Jpeg);
                }
            }
        }

        //отключаем камеру
        public void Disconnect()
        {
            SendMessage(mCapHwnd, WM_CAP_DRIVER_DISCONNECT, mCapHwnd, 0);
        }

        //Возвращает массив всех доступных веб-камер
        public static WebCamDevice[] GetAllWebCams()
        {
            //имя
            String dName = "".PadRight(100);
            //версия
            String dVersion = "".PadRight(100);
            //список камер
            List<WebCamDevice> WebCams = new List<WebCamDevice>();

            for (short i = 0; i < 10; i++)
            {
                //проверяем камеру с индексом i
                if (capGetDriverDescriptionA(i, ref dName, 100, ref dVersion, 100))
                    //если есть добавляем её к списку
                    WebCams.Add(new WebCamDevice(i, dName.Trim(), dVersion.Trim()));
            }
            //преобразуем список в массив
            return WebCams.ToArray();
        }

        //конструктор 
        private WebCamDevice(short index, string name, string version)
        {
            Index = index;
            Name = name;
            Version = version;
        }
    }
}