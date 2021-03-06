﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;

namespace SkyImagesAnalyzerLibraries
{
    public partial class SectionRequestForm : Form
    {
        public PointD retPt1;
        public PointD retPt2;
        public bool fromMarginToMargin;

        public SectionRequestForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            retPt1 = new PointD(Convert.ToDouble(textBox1.Text.Replace(".", ",")), Convert.ToDouble(textBox2.Text.Replace(".", ",")));
            retPt2 = new PointD(Convert.ToDouble(textBox3.Text.Replace(".", ",")), Convert.ToDouble(textBox4.Text.Replace(".", ",")));
            fromMarginToMargin = checkBox1.Checked;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void SectionRequestForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
        }



    }
}
