using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SkyIndexAnalyzerSolo
{
    public partial class AgentForm : Form
    {
        Form form1var, datacollectorform;

        public AgentForm()
        {
            InitializeComponent();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void AgentForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(10, "Skyindex Analyzer agent", "Приложение свернуто в трей и доступно по двойному клику мышью на этой иконке или кликом прямо по этому сообщению.", ToolTipIcon.Info);
                this.Hide();
            }
        }

        private void AgentForm_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (form1var == null)
            {
                form1var = new SkyIndexAnalyzer_AnalysisForm();
                form1var.Show();
            }
            else if (!form1var.Visible)
            {
                form1var = new SkyIndexAnalyzer_AnalysisForm();
                form1var.Show();
            }
            else
            {
                form1var.Activate();
                if (form1var.WindowState == FormWindowState.Minimized)
                {
                    form1var.WindowState = FormWindowState.Normal;
                }
            }
            
        }

        
        
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        
        
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                NotifyIconContextMenu.Show(Cursor.Position.X, Cursor.Position.Y);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Show();
                WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = false;
            }
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void NotifyIconContextMenu_MouseLeave(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            NotifyIconContextMenu.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (datacollectorform == null)
            {
                datacollectorform = new SkyIndexAnalyzerDataCollector();
                datacollectorform.Show();
            }
            else if (!datacollectorform.Visible)
            {
                datacollectorform = new SkyIndexAnalyzerDataCollector();
                datacollectorform.Show();
            }
            else
            {
                datacollectorform.Activate();
                if (datacollectorform.WindowState == FormWindowState.Minimized)
                {
                    datacollectorform.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

    }
}
