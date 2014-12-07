using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using SkyImagesAnalyzerLibraries;


namespace SkyImagesAnalyzer
{
    public partial class PropertiesEditor : Form
    {
        public Dictionary<string, object> localEditingDict { get; set; }
        public string fNamePropertiesXMLfile = "";


        public PropertiesEditor(Dictionary<string, object> propertiesObj, string xmlFileName)
        {
            InitializeComponent();
            localEditingDict = propertiesObj;
            fNamePropertiesXMLfile = xmlFileName;
        }



        private void PropertiesEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dictSettings = new Dictionary<string, object>();
            foreach (Pair<string, object> item in (DictionaryBindingList<string,object>)propertyGrid.DataSource)
            {
                dictSettings.Add(item.Key, item.Value);
            }

            //string fname = Directory.GetCurrentDirectory() + "\\settings\\SkyImagesAnalyzerSettings.xml";
            ServiceTools.WriteDictionaryToXml(dictSettings, fNamePropertiesXMLfile, false);

            this.Close();
        }





        private void PropertiesEditor_Load(object sender, EventArgs e)
        {
            Application.EnableVisualStyles();
            propertyGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            var bl = localEditingDict.ToBindingList();
            propertyGrid.DataSource = bl;
        }
    }
}
