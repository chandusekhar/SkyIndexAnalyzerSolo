using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.Structure;
using MathNet.Numerics.LinearAlgebra.Double;
using Emgu.CV;

namespace SkyIndexAnalyzerLibraries
{
    public partial class DataDistributionRepresentingForm : Form
    {
        public DenseMatrix dmValues = null;
        public DenseMatrix dmSpaceValues = null;
        public DenseMatrix dmWeightsValues = null;
        public bool contextHasBeenChanged = false;
        private bool isSpaceEmpty = true;
        private bool isWeightsEmpty = true;
        private Image<Bgr, Byte> representingImage = null;

        public DataDistributionRepresentingForm(DenseMatrix dataValues, DenseMatrix spaceValues = null, DenseMatrix weightValues = null)
        {
            if (dataValues == null) return;
            dmValues = (DenseMatrix)dataValues.Clone();
            dmSpaceValues = (spaceValues == null)? (null):(DenseMatrix)spaceValues.Clone();
            isSpaceEmpty = (spaceValues == null);
            dmWeightsValues = (weightValues == null) ? (null) : (DenseMatrix)weightValues.Clone();
            isWeightsEmpty = (weightValues == null);
            InitializeComponent();


        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {

        }

        private void chbRes1DynamicScale_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void DataDistributionRepresentingForm_Paint(object sender, PaintEventArgs e)
        {
            if (contextHasBeenChanged)
            {
                constructdataRepresentingPicture();
                contextHasBeenChanged = false;
            }

            ThreadSafeOperations.UpdatePictureBox(pbRes, representingImage.Bitmap, true);
        }


        private void constructdataRepresentingPicture()
        {
            representingImage = new Image<Bgr, byte>(pbRes.Width, pbRes.Height);
        }

        private void DataDistributionRepresentingForm_Resize(object sender, EventArgs e)
        {
            contextHasBeenChanged = true;
        }

        
        
        /// <summary>
        /// Handles the KeyPress event of the DataDistributionRepresentingForm control.
        /// Closes the window
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private void DataDistributionRepresentingForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }
    }
}
