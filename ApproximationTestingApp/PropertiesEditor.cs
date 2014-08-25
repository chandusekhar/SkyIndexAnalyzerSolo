using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApproximationTestingApp
{
    public partial class PropertiesEditor : Form
    {
        public PropertiesEditor(object propertiesObj)
        {
            InitializeComponent();
            propertyGrid.SelectedObject = propertiesObj;
        }

        private void PropertiesEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();

            this.Close();
        }
    }
}
