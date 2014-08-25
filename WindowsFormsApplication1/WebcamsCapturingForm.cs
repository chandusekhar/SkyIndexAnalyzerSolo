using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectShowLib;
using System.Runtime.InteropServices;
using SkyIndexAnalyzerLibraries;


namespace SkyIndexAnalyzerSolo
{
    public partial class WebcamsCapturingForm : Form
    {
        private Capture cam;
        IntPtr m_ip = IntPtr.Zero;


        public WebcamsCapturingForm()
        {
            InitializeComponent();
        }

        private void WebcamsCapturingForm_Load(object sender, EventArgs e)
        {

        }


        public void ProbeWebcams()
        {
            string CapDeviceName = "";
            const int VIDEOBITSPERPIXEL = 24; // BitsPerPixel values determined by device

            CapDevicesList.Items.Clear();
            int capDevicesNumber = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).Length;
            Cursor.Current = Cursors.WaitCursor;
            for (int iDeviceNum = 0; iDeviceNum < capDevicesNumber; iDeviceNum++)
            {
                try
                {
                    cam = new Capture(iDeviceNum, 640, 480, VIDEOBITSPERPIXEL, pictureBox1);
                    CapDeviceName = cam.CapDevicename;
                    if (m_ip != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(m_ip);
                        m_ip = IntPtr.Zero;
                    }
                    //m_ip = cam.Click();
                    //Bitmap b = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, m_ip);
                    cam.Dispose();
                    cam = null;
                    pictureBox1.Image = null;
                    //b.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    CapDevicesList.Items.Add(CapDeviceName, false);
                    //pictureBox1.Image = b;
                }
                catch (COMException)
                {
                    ThreadSafeOperations.SetTextTB(textBox1, "Не удалось получить изображение с камеры №" + iDeviceNum.ToString() + Environment.NewLine, true);
                }
                catch
                {
                    ThreadSafeOperations.SetTextTB(textBox1, "Не удалось получить изображение с камеры " + iDeviceNum.ToString() + Environment.NewLine, true);
                }
            }
            Cursor.Current = Cursors.Default;
        }


        public void StartCapturing()
        {

        }

        public void StopCapturing()
        {

        }
    }
}
