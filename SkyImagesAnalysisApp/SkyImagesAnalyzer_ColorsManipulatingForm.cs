using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using SkyImagesAnalyzerLibraries;
using System.IO;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;
using Emgu.CV;
using Emgu.CV.Structure;




namespace SkyImagesAnalyzer
{
    enum RGB { R, G, B }


    /// <summary>
    /// Class SkyIndexAnalyzing_ColorsManipulatingForm.
    /// Класс, описывающий поведение формы манипуляций с цветовыми составляющими анализируемого изобржаения
    /// </summary>
    public partial class SkyImagesAnalyzer_ColorsManipulatingForm : Form
    {
        public Dictionary<string, object> defaultProperties = null;

        private double minSunAreaPart = 0.0003d;
        private double maxSunAreaPart = 0.0005d;
        private double theStdDevMarginValueDefiningTrueSkyArea = 0.65d;
        private double theStdDevMarginValueDefiningSkyCloudSeparation = 0.75d;



        /// <summary>
        /// The parent form
        /// </summary>
        private MainAnalysisForm parentForm = null;

        /// <summary>
        /// The objects to dispose
        /// </summary>
        private List<object> objectsToDispose = new List<object>();

        /// <summary>
        /// The input bitmap
        /// </summary>
        //private Bitmap inputBitmap;
        private Image<Bgr, Byte> inputImage;

        /// <summary>
        /// The actual color channel color scheme ruler representation bitmap
        /// </summary>
        private Bitmap bmColorSchemeVisChannel;
        //private Bitmap bmColorSchemeVisRes1, bmColorSchemeVisRes2, bmColorSchemeVisRes3;

        /// <summary>
        /// Экземпляр класса ImageProcessing, используемый для всех обсчетов данных изображения
        /// Формируется при открытии формы на основании данных входящего изображения inputBitmap
        /// </summary>
        private ImageProcessing imgP;

        /// <summary>
        /// TextBox для вывода логов
        /// </summary>
        public TextBox tbLog;

        /// <summary>
        /// The current displayed channel mark
        /// </summary>
        private RGB currentDisplayedChannel = RGB.R;

        /// <summary>
        /// The Red channel data
        /// </summary>
        private imageConditionAndData channelDataR;
        /// <summary>
        /// The Green channel data
        /// </summary>
        private imageConditionAndData channelDataG;
        /// <summary>
        /// The Blue channel data
        /// </summary>
        private imageConditionAndData channelDataB;

        /// <summary>
        /// объект, представляющий результирующие данные в окне result 1
        /// </summary>
        private imageConditionAndData result1;
        /// <summary>
        /// объект, представляющий результирующие данные в окне result 2
        /// </summary>
        private imageConditionAndData result2;
        /// <summary>
        /// объект, представляющий результирующие данные в окне result 3
        /// </summary>
        private imageConditionAndData result3;

        /// <summary>
        /// The current sky index data
        /// </summary>
        private imageConditionAndData currentSkyIndexData;

        /// <summary>
        /// Плавающая подсказка рядом с курсором мыши
        /// </summary>
        private ToolTip textToolTip = null;
        /// <summary>
        /// The previous tooltip mouse location
        /// </summary>
        private Point previousTooltipMouseLocation;
        /// <summary>
        /// Left button down arguments var
        /// хранит данные события onMouseDown до тех пор, пока не
        /// - обнулят, если кнопка была отпущена за пределами PictureBox, где была нажата
        /// - используют при формировании данных выделения на PictureBox
        /// </summary>
        private MouseEventArgs meLeftButtonDownArgs = null;

        private MouseEventArgs meResLeftButtonDownArgs = null;

        /// <summary>
        /// Left button down sender
        /// Хранит объект, на котором появилось onMouseDown событие, данные которого записалось в meДуаеButtonDownArgs
        /// хранится до тех пор, пока не будет ненужен
        /// </summary>
        private object meLeftButtonDownSender = null;

        private object meResLeftButtonDownSender = null;

        //private PictureBoxSelection pbSelection = null;

        //private DateTime clickTrackingStartTime;

        private LogWindow theLogWindow = null;





        /// <summary>
        /// Initializes a new instance of the <see cref="SkyImagesAnalyzer_ColorsManipulatingForm" /> class.
        /// </summary>
        /// <param name="bmToAnalyze">The bitmap to analyze and manipulate with.</param>
        /// <param name="analyzerForm">The analyzer form.</param>
        //public SkyIndexAnalyzing_ColorsManipulatingForm(Bitmap bmToAnalyze, SkyIndexAnalyzer_AnalysisForm analyzerForm, Dictionary<string, object> settings)
        public SkyImagesAnalyzer_ColorsManipulatingForm(Image<Bgr, Byte> imgToAnalyze, MainAnalysisForm analyzerForm, Dictionary<string, object> settings)
        {
            if (imgToAnalyze == null)
            {
                return;
            }



            defaultProperties = settings;
            minSunAreaPart = Convert.ToDouble(defaultProperties["GrIxSunDetectionMinimalSunAreaPartial"]);
            maxSunAreaPart = Convert.ToDouble(defaultProperties["GrIxSunDetectionMaximalSunAreaPartial"]);
            //theStdDevMarginValueDefiningTrueSkyArea = 0.65d;
            //theStdDevMarginValueDefiningSkyCloudSeparation = 0.75d;

            parentForm = analyzerForm;

            inputImage = imgToAnalyze.Copy();

            imgP = new ImageProcessing(inputImage, true);
            channelDataR = new imageConditionAndData(imgP, 2);
            channelDataR.setGrayscaleCalculatedColorScheme();
            channelDataG = new imageConditionAndData(imgP, 1);
            channelDataG.setGrayscaleCalculatedColorScheme();
            channelDataB = new imageConditionAndData(imgP, 0);
            channelDataB.setGrayscaleCalculatedColorScheme();





            result1 = new imageConditionAndData(imgP);
            //result1.isColorSchemeMarginsFixed = true;
            //result1.UpdateColorSchemeRuler();
            result2 = new imageConditionAndData(imgP);
            //result2.isColorSchemeMarginsFixed = true;
            //result2.UpdateColorSchemeRuler();
            result3 = new imageConditionAndData(imgP);
            //result3.isColorSchemeMarginsFixed = true;
            //result3.UpdateColorSchemeRuler();

            objectsToDispose.Add(imgP);
            objectsToDispose.Add(channelDataR);
            objectsToDispose.Add(channelDataG);
            objectsToDispose.Add(channelDataB);
            objectsToDispose.Add(inputImage);
            objectsToDispose.Add(result1);
            objectsToDispose.Add(result2);
            objectsToDispose.Add(result3);





            InitializeComponent();



            ThreadSafeOperations.SetTextTB(tbFormula1, "R+0", false);
            ThreadSafeOperations.SetTextTB(tbFormula2, "G+0", false);
            ThreadSafeOperations.SetTextTB(tbFormula3, "B+0", false);
            ThreadSafeOperations.SetTextTB(tbColorSchemePath1, "matlab_jet.rgb", false);
            ThreadSafeOperations.SetTextTB(tbColorSchemePath2, "matlab_jet.rgb", false);
            ThreadSafeOperations.SetTextTB(tbColorSchemePath3, "matlab_jet.rgb", false);
            chbRes1DynamicScale.Checked = true;
            chbRes2DynamicScale.Checked = true;
            chbRes3DynamicScale.Checked = true;

            bgwSkyIndexAnalyzer.RunWorkerAsync();
        }



        /// <summary>
        /// Handles the Paint event of the SkyIndexAnalyzing_ColorsManipulatingForm control.
        /// Пересчитывает и отрисовывает все картинки и линейки
        /// </summary>
        /// <param name="sender">The source of the event. Не используется</param>
        /// <param name="e">The <see cref="PaintEventArgs" /> instance containing the event data. Не используется</param>
        private void SkyIndexAnalyzing_ColorsManipulatingForm_Paint(object sender, PaintEventArgs e)
        {
            //if (this.WindowState != FormWindowState.Normal)
            //{
            //    return;
            //}

            Bitmap bm2Show = null;
            bmColorSchemeVisChannel = null;

            if (currentDisplayedChannel == RGB.R)
            {
                bm2Show = channelDataR.dataRepresentingImageColored();
                bmColorSchemeVisChannel = channelDataR.currentColorSchemeRuler.RulerBitmap(pbScale.Width, pbScale.Height);
            }
            else if (currentDisplayedChannel == RGB.G)
            {
                bm2Show = channelDataG.dataRepresentingImageColored();
                bmColorSchemeVisChannel = channelDataG.currentColorSchemeRuler.RulerBitmap(pbScale.Width, pbScale.Height);
            }
            else if (currentDisplayedChannel == RGB.B)
            {
                bm2Show = channelDataB.dataRepresentingImageColored();
                bmColorSchemeVisChannel = channelDataB.currentColorSchemeRuler.RulerBitmap(pbScale.Width, pbScale.Height);
            }

            ThreadSafeOperations.UpdatePictureBox(pbChannel, bm2Show, true);
            ThreadSafeOperations.UpdatePictureBox(pbRes1, result1.dataRepresentingImageColored(), true);
            ThreadSafeOperations.UpdatePictureBox(pbRes2, result2.dataRepresentingImageColored(), true);
            ThreadSafeOperations.UpdatePictureBox(pbRes3, result3.dataRepresentingImageColored(), true);
            ThreadSafeOperations.UpdatePictureBox(pbRes1Scale, result1.currentColorSchemeRuler.RulerBitmap(pbRes1Scale.Width, pbRes1Scale.Height), false);
            ThreadSafeOperations.UpdatePictureBox(pbRes2Scale, result2.currentColorSchemeRuler.RulerBitmap(pbRes2Scale.Width, pbRes2Scale.Height), false);
            ThreadSafeOperations.UpdatePictureBox(pbRes3Scale, result3.currentColorSchemeRuler.RulerBitmap(pbRes3Scale.Width, pbRes3Scale.Height), false);
            ThreadSafeOperations.UpdatePictureBox(pbScale, bmColorSchemeVisChannel, false);
            ThreadSafeOperations.UpdatePictureBox(pbSourceImage, inputImage.Bitmap, true);

            if (currentSkyIndexData != null)
            {
                ThreadSafeOperations.UpdatePictureBox(pbSkyIndexImage, currentSkyIndexData.dataRepresentingImageColored(), true);
            }

        }



        /// <summary>
        /// Handles the KeyPress event of the SkyIndexAnalyzing_ColorsManipulatingForm control.
        /// closes the form
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private void SkyIndexAnalyzing_ColorsManipulatingForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)//escape key
            {
                this.Close();
            }
        }



        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            calculateResultBitmap((Button)sender);
        }



        /// <summary>
        /// Calculates the result bitmap.
        /// В зависимости от кнопки theOKbutton модифицируются данные result1
        /// </summary>
        /// <param name="theOKbutton">The o kbutton.</param>
        private void calculateResultBitmap(Button theOKbutton)
        {
            TextBox currentFormulaTextboxElement = tbFormula1;
            PictureBox currentResultPictureBox = pbRes1;
            String formulaString = "";
            TextBox currentColorSchemePathTextbox = tbColorSchemePath1;
            TextBox currentStatsTextbox = tbStats1;

            if (theOKbutton == btnOK1)
            {
                currentFormulaTextboxElement = tbFormula1;
                currentResultPictureBox = pbRes1;
                currentColorSchemePathTextbox = tbColorSchemePath1;
                currentStatsTextbox = tbStats1;
            }
            else if (theOKbutton == btnOK2)
            {
                currentFormulaTextboxElement = tbFormula2;
                currentResultPictureBox = pbRes2;
                currentColorSchemePathTextbox = tbColorSchemePath2;
                currentStatsTextbox = tbStats2;
            }
            else if (theOKbutton == btnOK3)
            {
                currentFormulaTextboxElement = tbFormula3;
                currentResultPictureBox = pbRes3;
                currentColorSchemePathTextbox = tbColorSchemePath3;
                currentStatsTextbox = tbStats3;
            }

            formulaString = currentFormulaTextboxElement.Text;

            if (formulaString == "")
            {
                return;
            }

            ThreadSafeOperations.UpdatePictureBox(currentResultPictureBox, null, false);


            String currentColorSchemeFileName = currentColorSchemePathTextbox.Text;// = Directory.GetCurrentDirectory() + "\\" + tbColorSchemePath1.Text;
            if (File.Exists(currentColorSchemeFileName)) imgP.evaluatingColorScheme = new ColorScheme(currentColorSchemeFileName);
            else
            {
                currentColorSchemeFileName = Directory.GetCurrentDirectory() + "\\" + currentColorSchemePathTextbox.Text;
                imgP.evaluatingColorScheme = new ColorScheme(currentColorSchemeFileName);
            }


            imageConditionAndData theResultvar = null;
            if (theOKbutton == btnOK1)
            {
                //bmRes1 = imgP.tmpImageBitmap();
                theResultvar = result1;
                //result1.DmSourceData = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)imgP.eval(formulaString, channelDataR.DmSourceData, channelDataG.DmSourceData, channelDataB.DmSourceData, currentStatsTextbox).Clone();
            }
            else if (theOKbutton == btnOK2)
            {
                //bmRes2 = imgP.tmpImageBitmap();
                theResultvar = result2;
                //result2.DmSourceData = (DenseMatrix)imgP.eval(formulaString, channelDataR.DmSourceData, channelDataG.DmSourceData, channelDataB.DmSourceData, currentStatsTextbox).Clone();
            }
            else if (theOKbutton == btnOK3)
            {
                //bmRes3 = imgP.tmpImageBitmap();
                theResultvar = result3;
                //result3.DmSourceData = (DenseMatrix)imgP.eval(formulaString, channelDataR.DmSourceData, channelDataG.DmSourceData, channelDataB.DmSourceData, currentStatsTextbox).Clone();
            }
            //imgP.Dispose();
            DenseMatrix dmres =
                (DenseMatrix)
                    imgP.eval(formulaString, channelDataR.DmSourceData, channelDataG.DmSourceData,
                        channelDataB.DmSourceData, currentStatsTextbox).Clone();
            dmres = (DenseMatrix)dmres.PointwiseMultiply(imgP.dmSignificantMaskCircled(95.0d));
            DenseMatrix dmEdgesCut = DenseMatrix.Create(dmres.RowCount, dmres.ColumnCount, new Func<int, int, double>(
                (row, col) =>
                {
                    if (row < 2) return 0.0d;
                    if (col < 2) return 0.0d;
                    if (row > dmres.RowCount - 3) return 0.0d;
                    if (col > dmres.ColumnCount - 3) return 0.0d;
                    return 1.0d;
                }));
            dmres = (DenseMatrix)dmres.PointwiseMultiply(dmEdgesCut);


            theResultvar.DmSourceData = dmres;
            theResultvar.UpdateColorSchemeRuler();


            RaisePaintEvent(null, null);
            ServiceTools.FlushMemory(tbLog, "");
        }





        /// <summary>
        /// Handles the Click event of the btnBrowseColorScheme1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnBrowseColorScheme1_Click(object sender, EventArgs e)
        {
            TextBox currFilenameTextbox = tbColorSchemePath1;
            PictureBox pbCurrentColorScheme = pbRes1Scale;

            if (sender == btnBrowseColorScheme1)
            {
                currFilenameTextbox = tbColorSchemePath1;
            }
            else if (sender == btnBrowseColorScheme2)
            {
                currFilenameTextbox = tbColorSchemePath2;
            }
            else if (sender == btnBrowseColorScheme3)
            {
                currFilenameTextbox = tbColorSchemePath3;
            }

            OpenFileDialog opFD = new OpenFileDialog();
            opFD.Filter = "RGB files (*.rgb)|*.rgb|All files (*.*)|*.*";
            opFD.InitialDirectory = Directory.GetCurrentDirectory();
            opFD.Multiselect = false;
            DialogResult dialogRes = opFD.ShowDialog();
            if (dialogRes == DialogResult.OK)
            {
                String filename = opFD.FileName;
                FileInfo fInfo = new FileInfo(filename);
                if (fInfo.DirectoryName == Directory.GetCurrentDirectory()) ThreadSafeOperations.SetTextTB(currFilenameTextbox, fInfo.Name, false);
                else ThreadSafeOperations.SetTextTB(currFilenameTextbox, fInfo.FullName, false);

                ColorScheme tmpColorScheme = new ColorScheme(fInfo.FullName);


                if (sender == btnBrowseColorScheme1)
                {
                    result1.currentColorScheme = tmpColorScheme;
                    result1.UpdateColorSchemeRuler();
                    //constructScaleRulers(pbRes1, result1);
                }
                else if (sender == btnBrowseColorScheme2)
                {
                    result2.currentColorScheme = tmpColorScheme;
                    result2.UpdateColorSchemeRuler();
                    //constructScaleRulers(pbRes2, result2);
                }
                else if (sender == btnBrowseColorScheme3)
                {
                    result3.currentColorScheme = tmpColorScheme;
                    result3.UpdateColorSchemeRuler();
                    //constructScaleRulers(pbRes3, result3);
                }

                RaisePaintEvent(null, null);
                //ThreadSafeOperations.UpdatePictureBox(pbCurrentColorScheme, currentColorSchemeBitmap, false);

            }
        }


        //private void constructScaleRulers(PictureBox pbRefPictureBox, imageConditionAndData currPuctureData = null)
        //{
        //    if (pbRefPictureBox == pbRes1)
        //    {
        //        if (currPuctureData == null)
        //        {
        //            ColorScheme tmpColorScheme = new ColorScheme(tbColorSchemePath1.Text);
        //            //bmColorSchemeVisRes1 = tmpColorScheme.scaleVisulizationBitmap(pbRes1Scale.Width, pbRes1Scale.Height);
        //        }
        //        else
        //        {
        //            ColorScheme tmpColorScheme = currPuctureData.currentColorScheme;
        //            //bmColorSchemeVisRes1 = tmpColorScheme.scaleVisulizationRuler(currPuctureData.dataMinValue(), currPuctureData.dataMaxValue()).RulerBitmap(pbRes1Scale.Width, pbRes1Scale.Height);
        //        }
        //    }
        //    else if (pbRefPictureBox == pbRes2)
        //    {
        //        if (currPuctureData == null)
        //        {
        //            ColorScheme tmpColorScheme = new ColorScheme(tbColorSchemePath2.Text);
        //            //bmColorSchemeVisRes2 = tmpColorScheme.scaleVisulizationBitmap(pbRes2Scale.Width, pbRes2Scale.Height);
        //        }
        //        else
        //        {
        //            ColorScheme tmpColorScheme = currPuctureData.currentColorScheme;
        //            //bmColorSchemeVisRes2 = tmpColorScheme.scaleVisulizationRuler(currPuctureData.dataMinValue(), currPuctureData.dataMaxValue()).RulerBitmap(pbRes2Scale.Width, pbRes2Scale.Height);
        //        }
        //    }
        //    else if (pbRefPictureBox == pbRes3)
        //    {
        //        if (currPuctureData == null)
        //        {
        //            ColorScheme tmpColorScheme = new ColorScheme(tbColorSchemePath3.Text);
        //            //bmColorSchemeVisRes3 = tmpColorScheme.scaleVisulizationBitmap(pbRes3Scale.Width, pbRes3Scale.Height);
        //        }
        //        else
        //        {
        //            ColorScheme tmpColorScheme = currPuctureData.currentColorScheme;
        //            //bmColorSchemeVisRes3 = tmpColorScheme.scaleVisulizationRuler(currPuctureData.dataMinValue(), currPuctureData.dataMaxValue()).RulerBitmap(pbRes3Scale.Width, pbRes3Scale.Height);
        //        }
        //
        //    }
        //}

        /// <summary>
        /// Handles the Load event of the SkyIndexAnalyzing_ColorsManipulatingForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SkyIndexAnalyzing_ColorsManipulatingForm_Load(object sender, EventArgs e)
        {
            //constructScaleRulers(pbRes1);
            //constructScaleRulers(pbRes2);
            //constructScaleRulers(pbRes3);

            btnOK_Click(btnOK1, null);
            btnOK_Click(btnOK2, null);
            btnOK_Click(btnOK3, null);

            RaisePaintEvent(null, null);
        }

        /// <summary>
        /// Handles the Resize event of the SkyIndexAnalyzing_ColorsManipulatingForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SkyIndexAnalyzing_ColorsManipulatingForm_Resize(object sender, EventArgs e)
        {

            RaisePaintEvent(null, null);
        }

        /// <summary>
        /// Handles the KeyPress event of the tbRedThresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private void tbRedThresh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                //return key
                if (sender == tbChannelThresh)
                {
                    ColorChannelImageUpdate(sender, e);
                }
            }
        }

        //private void trbResThresh_Scroll(object sender, EventArgs e)
        //{
        //    TextBox currentTextBoxElement = tbRes1Thresh;
        //    Button currentOKbuttonElement = btnOK1;
        //
        //    if (sender == trbRes1Thresh)
        //    {
        //        currentTextBoxElement = tbRes1Thresh;
        //        currentOKbuttonElement = btnOK1;
        //    }
        //    else if (sender == trbRes2Thresh)
        //    {
        //        currentTextBoxElement = tbRes2Thresh;
        //        currentOKbuttonElement = btnOK2;
        //    }
        //    else if (sender == trbRes3Thresh)
        //    {
        //        currentTextBoxElement = tbRes3Thresh;
        //        currentOKbuttonElement = btnOK3;
        //    }
        //
        //    ThreadSafeOperations.SetTextTB(currentTextBoxElement, ((TrackBar)sender).Value.ToString(), false);
        //
        //    //if (rbtnThresholdingRedTop.Checked) bmRes1 = imgP.getMaskedImageChannelBitmapThresholdedTop(2, (Byte)trbRedThresh.Value);
        //    //else bmRed = imgP.getMaskedImageChannelBitmapThresholdedBottom(2, (Byte)trbRedThresh.Value);
        //
        //    this.RaisePaintEvent(null, null);
        //    ServiceTools.FlushMemory(tbLog, "");
        //}




        /// <summary>
        /// Handles the Click event of the pbScale control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void pbScale_Click(object sender, EventArgs e)
        {
            //if (((MouseEventArgs)e).Button == System.Windows.Forms.MouseButtons.Left)
            //{
            //    ColorChannelImageUpdate(sender, e);
            //}
        }







        /// <summary>
        /// Handles the CheckedChanged event of the rbtnSwitchChannel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void rbtnSwitchChannel_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnSwitchChannelR.Checked) currentDisplayedChannel = RGB.R;
            else if (rbtnSwitchChannelG.Checked) currentDisplayedChannel = RGB.G;
            else if (rbtnSwitchChannelB.Checked) currentDisplayedChannel = RGB.B;

            ColorChannelImageUpdate(sender, e);
        }



        /// <summary>
        /// Colors the channel image update.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ColorChannelImageUpdate(object sender, EventArgs e)
        {
            imageConditionAndData currentConditionData = null;
            if (currentDisplayedChannel == RGB.R) currentConditionData = channelDataR;
            else if (currentDisplayedChannel == RGB.G) currentConditionData = channelDataG;
            else if (currentDisplayedChannel == RGB.B) currentConditionData = channelDataB;

            //ColorSchemeRuler tmpRuler = currentConditionData.currentColorSchemeRuler;


            if (sender == pbScale)
            {
                double dValueClicked = currentConditionData.currentColorSchemeRuler.GetValueByClickEvent((PictureBox)sender, (MouseEventArgs)e);
                currentConditionData.currentColorSchemeRuler.markerPositionValue = dValueClicked;
                currentConditionData.ThresholdingValue = dValueClicked;
                ThreadSafeOperations.SetTextTB(tbChannelThresh, (Math.Round(dValueClicked, 2)).ToString(), false);
            }
            else if (sender == tbChannelThresh)
            {
                int num = Convert.ToInt32(((TextBox)sender).Text);
                double dValueEntered = (double)num; //currentConditionData.currentColorSchemeRuler.getValueByClickEvent((PictureBox)sender, (MouseEventArgs)e);
                currentConditionData.currentColorSchemeRuler.markerPositionValue = dValueEntered;
                currentConditionData.ThresholdingValue = dValueEntered;
            }
            //else if ((sender == rbtnThresholdingTop) || (sender == rbtnThresholdingBtm))
            //{
            //    currentConditionData.ThresholdingUsageTop = rbtnThresholdingTop.Checked;
            //    currentConditionData.ThresholdingUsageBtm = rbtnThresholdingBtm.Checked;
            //}

            RaisePaintEvent(null, null);
        }



        /// <summary>
        /// Handles the MouseClick event of the pbResScale control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void pbResScale_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                return;
            }
            //then Right button
            ColorSchemeRuler currentRuler = null;
            if ((PictureBox)sender == pbScale)
            {
                if (currentDisplayedChannel == RGB.R)
                {
                    currentRuler = channelDataR.currentColorSchemeRuler;
                }
                else if (currentDisplayedChannel == RGB.G)
                {
                    currentRuler = channelDataG.currentColorSchemeRuler;
                }
                else if (currentDisplayedChannel == RGB.B)
                {
                    currentRuler = channelDataB.currentColorSchemeRuler;
                }
            }
            else if ((PictureBox)sender == pbRes1Scale)
            {
                currentRuler = result1.currentColorSchemeRuler;
            }
            else if ((PictureBox)sender == pbRes2Scale)
            {
                currentRuler = result2.currentColorSchemeRuler;
            }
            else if ((PictureBox)sender == pbRes3Scale)
            {
                currentRuler = result3.currentColorSchemeRuler;
            }




        }



        /// <summary>
        /// Handles the KeyPress event of the tbResultThresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private void tbResultThresh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') ResultImageUpdate(sender, e);
        }


        /// <summary>
        /// Handles the CheckedChanged event of the rbtnResThresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void rbtnResThresh_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtnSender = (RadioButton)sender;
            if (!rbtnSender.Checked)
            {//was true
                return;
            }
            else ResultImageUpdate(sender, e);
        }



        /// <summary>
        /// Results the image update.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ResultImageUpdate(object sender, EventArgs e)
        {
            imageConditionAndData currentConditionData = null;
            TextBox currentTextBox = null;


            //if ((sender == pbRes1Scale) || (sender == tbRes1Thresh) || (sender == rbtnRes1ThreshTop) || (sender == rbtnRes1ThreshBtm))
            if ((sender == pbRes1Scale) || (sender == tbRes1Thresh))
            {
                currentConditionData = result1;
                currentTextBox = tbRes1Thresh;
            }
            //else if ((sender == pbRes2Scale) || (sender == tbRes2Thresh) || (sender == rbtnRes2ThreshTop) || (sender == rbtnRes2ThreshBtm))
            else if ((sender == pbRes2Scale) || (sender == tbRes2Thresh))
            {
                currentConditionData = result2;
                currentTextBox = tbRes2Thresh;
            }
            //else if ((sender == pbRes3Scale) || (sender == tbRes3Thresh) || (sender == rbtnRes3ThreshTop) || (sender == rbtnRes3ThreshBtm))
            else if ((sender == pbRes3Scale) || (sender == tbRes3Thresh))
            {
                currentConditionData = result3;
                currentTextBox = tbRes3Thresh;
            }



            if (sender.GetType().Equals(typeof(PictureBox)))
            {
                double dValueClicked = currentConditionData.currentColorSchemeRuler.GetValueByClickEvent((PictureBox)sender, (MouseEventArgs)e);
                currentConditionData.currentColorSchemeRuler.markerPositionValue = dValueClicked;
                currentConditionData.ThresholdingValue = dValueClicked;
                ThreadSafeOperations.SetTextTB(currentTextBox, (Math.Round(dValueClicked, 2)).ToString(), false);
            }
            else if (sender.GetType().Equals(typeof(TextBox)))
            {
                int num = Convert.ToInt32(((TextBox)sender).Text);
                double dValueEntered = (double)num; //currentConditionData.currentColorSchemeRuler.getValueByClickEvent((PictureBox)sender, (MouseEventArgs)e);
                currentConditionData.currentColorSchemeRuler.markerPositionValue = dValueEntered;
                currentConditionData.ThresholdingValue = dValueEntered;
            }
            else if (sender.GetType().Equals(typeof(RadioButton)))
            {
                if (((RadioButton)sender).Name.Contains("Top"))
                {
                    //сюда приходим только для элемента, на котором теперь флажок - см. функцию экшна
                    currentConditionData.ThresholdingUsageTop = true;
                    currentConditionData.ThresholdingUsageBtm = false;
                }
            }

            RaisePaintEvent(null, null);
        }

        
        /// <summary>
        /// Handles the MouseMove event of the pbScale control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void pbScale_MouseMove(object sender, MouseEventArgs e)
        {
            ColorSchemeRuler currentRuler = null;
            if (sender == pbScale)
            {
                if (currentDisplayedChannel == RGB.R) currentRuler = channelDataR.currentColorSchemeRuler;
                else if (currentDisplayedChannel == RGB.G) currentRuler = channelDataG.currentColorSchemeRuler;
                else if (currentDisplayedChannel == RGB.B) currentRuler = channelDataB.currentColorSchemeRuler;
            }
            else if (sender == pbRes1Scale) currentRuler = result1.currentColorSchemeRuler;
            else if (sender == pbRes2Scale) currentRuler = result2.currentColorSchemeRuler;
            else if (sender == pbRes3Scale) currentRuler = result3.currentColorSchemeRuler;


            double clickedValue = currentRuler.GetValueByClickEvent((PictureBox)sender, e);
            clickedValue = Math.Round(clickedValue, 2);

            ShowToolTip(e, clickedValue.ToString(), sender);
        }



        /// <summary>
        /// Shows the tool tip.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <param name="textToShow">The text to show.</param>
        /// <param name="theElement">The element.</param>
        private void ShowToolTip(MouseEventArgs e, String textToShow, object theElement)
        {

            if (textToolTip == null) textToolTip = new ToolTip();
            else
            {
                if (previousTooltipMouseLocation == e.Location)
                {
                    return;
                }
                textToolTip.Dispose();
                textToolTip = new ToolTip();
            }
            Point loc = (Point)ServiceTools.getPropValue(theElement, "Location");
            textToolTip.Show(textToShow, this, e.X + loc.X + 40, e.Y + loc.Y + 40);
            previousTooltipMouseLocation = e.Location;
        }



        /// <summary>
        /// Handles the MouseLeave event of the pbScale control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void pbScale_MouseLeave(object sender, EventArgs e)
        {
            if (textToolTip != null)
            {
                textToolTip.Dispose();
                textToolTip = null;
            }

            meLeftButtonDownArgs = null;
            meLeftButtonDownSender = null;
        }




        /// <summary>
        /// Handles the MouseDown event of the control containing imageConditionAndData or ColorSchemeRuler representing picture.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void pb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                meLeftButtonDownArgs = e;
                meLeftButtonDownSender = sender;
            }
        }



        /// <summary>
        /// Handles the MouseUp event of the control containing imageConditionAndData or ColorSchemeRuler representing picture.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void pb_MouseUp(object sender, MouseEventArgs e)
        {
            imageConditionAndData currentICD = null;
            Type theSenderDataType = typeof(imageConditionAndData);
            PictureBoxSelection pbSelection = null;


            if (e.Button == MouseButtons.Left && sender.Equals(meLeftButtonDownSender))
            {
                if (sender == pbChannel)
                {
                    if (currentDisplayedChannel == RGB.R)
                    {
                        //pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location, channelDataR);
                        //channelDataR.Selection = pbSelection;
                        currentICD = channelDataR;
                        theSenderDataType = channelDataR.GetType();
                    }
                    else if (currentDisplayedChannel == RGB.G)
                    {
                        //pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location, channelDataG);
                        //channelDataG.Selection = pbSelection;
                        currentICD = channelDataG;
                        theSenderDataType = channelDataG.GetType();
                    }
                    else if (currentDisplayedChannel == RGB.B)
                    {
                        //pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location, channelDataB);
                        //channelDataB.Selection = pbSelection;
                        currentICD = channelDataB;
                        theSenderDataType = channelDataB.GetType();
                    }
                }
                else if (sender == pbScale)
                {
                    if (currentDisplayedChannel == RGB.R)
                    {
                        //pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location, channelDataR.currentColorSchemeRuler);
                        //channelDataR.currentColorSchemeRuler.Selection = pbSelection;
                        currentICD = channelDataR;
                        theSenderDataType = channelDataR.currentColorSchemeRuler.GetType();
                    }
                    else if (currentDisplayedChannel == RGB.G)
                    {
                        //pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location, channelDataG.currentColorSchemeRuler);
                        //channelDataG.currentColorSchemeRuler.Selection = pbSelection;
                        currentICD = channelDataG;
                        theSenderDataType = channelDataG.currentColorSchemeRuler.GetType();
                    }
                    else if (currentDisplayedChannel == RGB.B)
                    {
                        //pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location, channelDataB.currentColorSchemeRuler);
                        //channelDataB.currentColorSchemeRuler.Selection = pbSelection;
                        currentICD = channelDataB;
                        theSenderDataType = channelDataB.currentColorSchemeRuler.GetType();
                    }
                }
                else if (sender == pbRes1Scale)
                {
                    //pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location, result1.currentColorSchemeRuler);
                    //result1.currentColorSchemeRuler.Selection = pbSelection;
                    currentICD = result1;
                    theSenderDataType = result1.currentColorSchemeRuler.GetType();
                }
                else if (sender == pbRes2Scale)
                {
                    pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location,
                        result2.currentColorSchemeRuler);
                    //result2.currentColorSchemeRuler.Selection = pbSelection;
                    currentICD = result2;
                    theSenderDataType = result2.currentColorSchemeRuler.GetType();
                }
                else if (sender == pbRes3Scale)
                {
                    //pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location, result3.currentColorSchemeRuler);
                    //result3.currentColorSchemeRuler.Selection = pbSelection;
                    currentICD = result3;
                    theSenderDataType = result3.currentColorSchemeRuler.GetType();
                }


                //если уже есть selection у этого объекта, а это выделение пусто - проверить, было ли оно внутри
                //если было внутри - значит, был клик или даблклик внутри выделения - не обрабатывать здесь
                if (theSenderDataType == typeof(imageConditionAndData))
                {
                    pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location, currentICD);

                    if ((pbSelection.IsEmptySelection) && (currentICD.Selection != null))
                    {
                        if (currentICD.Selection.CheckIfDoubleclickedinsideSelection(sender, e, currentICD))
                        {
                            return;
                        }
                    }
                    currentICD.Selection = pbSelection;
                }
                else if (theSenderDataType == typeof(ColorSchemeRuler))
                {
                    pbSelection = new PictureBoxSelection(sender, meLeftButtonDownArgs.Location, e.Location,
                        currentICD.currentColorSchemeRuler);
                    currentICD.currentColorSchemeRuler.Selection = pbSelection;
                }


                HighlightLinkedSelection(pbSelection);
                RaisePaintEvent(null, null);

            }
        }




        /// <summary>
        /// Highlights the linked object selection.
        /// if the selection object is ruler then highlight regions on the image
        /// if the selection object is the data image then highlight the ruler segments
        /// </summary>
        /// <param name="theSelection">The selection.</param>
        private void HighlightLinkedSelection(PictureBoxSelection theSelection)
        {
            if (theSelection.usedImageData.GetType().Equals(typeof(imageConditionAndData)))
            {
                ((imageConditionAndData)(theSelection.usedImageData)).currentColorSchemeRuler.makeHihghlightMask();
            }
            else if (theSelection.usedImageData.GetType().Equals(typeof(ColorSchemeRuler)))
            {
                ((ColorSchemeRuler)(theSelection.usedImageData)).imgToRule.makeHihghlightMask();
            }
        }




        /// <summary>
        /// Handles the MouseDoubleClick event of the pbScale control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void pbScale_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                ColorChannelImageUpdate(sender, e);
            }
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the pbRes1Scale control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void pbRes1Scale_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ResultImageUpdate(sender, e);
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the pbChannel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void pbChannel_MouseMove(object sender, MouseEventArgs e)
        {
            imageConditionAndData currentImgData = null;
            if (sender == pbChannel)
            {
                if (currentDisplayedChannel == RGB.R) currentImgData = channelDataR;
                else if (currentDisplayedChannel == RGB.G) currentImgData = channelDataG;
                else if (currentDisplayedChannel == RGB.B) currentImgData = channelDataB;
            }
            else if (sender == pbRes1) currentImgData = result1;
            else if (sender == pbRes2) currentImgData = result2;
            else if (sender == pbRes3) currentImgData = result3;


            PointD origDataPointPosition = new PointD();
            double clickedValue = currentImgData.GetValueByClickEvent((PictureBox)sender, e, out origDataPointPosition);
            clickedValue = Math.Round(clickedValue, 2);

            string strToolTipMessage = origDataPointPosition.ToString() + Environment.NewLine;
            strToolTipMessage += clickedValue.ToString();

            ShowToolTip(e, strToolTipMessage, sender);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chbResDynamicScale control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void chbResDynamicScale_CheckedChanged(object sender, EventArgs e)
        {
            ColorSchemeRuler currentResultRuler = null;
            if (sender == chbRes1DynamicScale)
            {
                currentResultRuler = result1.currentColorSchemeRuler;
            }
            else if (sender == chbRes2DynamicScale)
            {
                currentResultRuler = result2.currentColorSchemeRuler;
            }
            else if (sender == chbRes3DynamicScale)
            {
                currentResultRuler = result3.currentColorSchemeRuler;
            }

            currentResultRuler.IsMarginsFixed = !((CheckBox)sender).Checked;

            RaisePaintEvent(null, null);
        }

        /// <summary>
        /// Handles the DoWork event of the bgwSkyIndexAnalyzer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void bgwSkyIndexAnalyzer_DoWork(object sender, DoWorkEventArgs e)
        {
            SkyCloudClassification classificator = new SkyCloudClassification(inputImage, defaultProperties);
            classificator.cloudSkySeparationValue = parentForm.tunedSIMargin;
            if (parentForm.rbtnClassMethodJapan.Checked)
            {
                classificator.ClassificationMethod = ClassificationMethods.Japan;
            }
            else if (parentForm.rbtnClassMethodUS.Checked)
            {
                classificator.ClassificationMethod = ClassificationMethods.Greek;
            }
            else if (parentForm.rbtnClassMethodGrIx.Checked)
            {
                classificator.ClassificationMethod = ClassificationMethods.GrIx;
                classificator.theStdDevMarginValueDefiningSkyCloudSeparation = parentForm.tunedSIMargin;
            }
            classificator.Classify();


            currentSkyIndexData = new imageConditionAndData(classificator.dmSkyIndexData, classificator.maskImage);
            currentSkyIndexData.currentColorScheme = ColorScheme.BinaryCloudSkyColorScheme(classificator.cloudSkySeparationValue, classificator.dmSkyIndexData.Values.Min(), classificator.dmSkyIndexData.Values.Max());
            currentSkyIndexData.currentColorSchemeRuler = new ColorSchemeRuler(currentSkyIndexData);
            //currentSkyIndexData = new imageConditionAndData(ImageProcessing.DenseMatrixFromImage(new Image<Gray, Byte>(classificator.PreviewBitmap)) , classificator.maskImage/255);
            //Bitmap bitmaoSI = new Bitmap(classificator.PreviewBitmap);
            ServiceTools.FlushMemory(null, null);
            //ThreadSafeOperations.UpdatePictureBox(pbSkyIndexImage, currentSkyIndexData.dataRepresentingImageColored(), true);
        }

        private void bgwSkyIndexAnalyzer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            objectsToDispose.Add(currentSkyIndexData);
            RaisePaintEvent(null, null);
        }



        /// <summary>
        /// Handles the MouseDoubleClick event of the PictureBox controls representing result and other
        /// imageConditionAndData variables
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void pbImageData_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;


            imageConditionAndData currentImageData = null;
            if (sender == pbChannel)
            {
                if (currentDisplayedChannel == RGB.R) currentImageData = channelDataR;
                else if (currentDisplayedChannel == RGB.G) currentImageData = channelDataG;
                else if (currentDisplayedChannel == RGB.B) currentImageData = channelDataB;
            }
            else if (sender == pbRes1) currentImageData = result1;
            else if (sender == pbRes2) currentImageData = result2;
            else if (sender == pbRes3) currentImageData = result3;


            if (currentImageData.Selection != null)
            {
                if (currentImageData.Selection.CheckIfDoubleclickedinsideSelection(sender, e, currentImageData))
                {
                    imageConditionAndData tmpImageData = currentImageData.SelectedImageData();
                    ImageConditionAndDataRepresentingForm tmpAnalysisForm = new ImageConditionAndDataRepresentingForm(tmpImageData);
                    tmpAnalysisForm.Show();
                }
            }
        }

        private void btnSaveDataRes_Click(object sender, EventArgs e)
        {
            imageConditionAndData currentData = result1;
            TextBox tbLogForSaver = tbStats1;
            if (sender == btnSaveDataRes1)
            {
                currentData = result1;
                tbLogForSaver = tbStats1;
            }
            else if (sender == btnSaveDataRes2)
            {
                currentData = result2;
                tbLogForSaver = tbStats2;
            }
            else if (sender == btnSaveDataRes3)
            {
                currentData = result3;
                tbLogForSaver = tbStats3;
            }
            else if (sender == btnSaveDataSkyIndexData)
            {
                currentData = currentSkyIndexData;
                tbLogForSaver = null;
            }


            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.InitialDirectory = Directory.GetCurrentDirectory();
            //sfd.FileName = "ResultData.dat";
            //if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    string theFileName = sfd.FileName;
            //    FileStream dataFileStream = new FileStream(theFileName, FileMode.OpenOrCreate, FileAccess.Write);
            //    string strtowrite = "";
            //    //byte[] info2write = new UTF8Encoding(true).GetBytes(strtowrite);
            //
            //    for (int row = 0; row < currentData.DmSourceData.RowCount; row++)
            //    {
            //        for (int column = 0; column < currentData.DmSourceData.ColumnCount; column++)
            //        {
            //            strtowrite += currentData.DmSourceData[row, column].ToString() + ";";
            //        }
            //        strtowrite += "\n";
            //        byte[] info2write = new UTF8Encoding(true).GetBytes(strtowrite);
            //        dataFileStream.Write(info2write, 0, info2write.Length);
            //    }
            //    //byte[] info2write = new UTF8Encoding(true).GetBytes(strtowrite);
            //    //dataFileStream.Write(info2write, 0, info2write.Length);
            //
            //
            //    dataFileStream.Close();
            //}


            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Directory.GetCurrentDirectory();
            sfd.FileName = "ResultData.nc";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string theFileName = sfd.FileName;
                NetCDFoperations.SaveDataToFile(currentData.DmSourceData, theFileName, tbLogForSaver, true);
            }


        }

        private void btnTestProcess_Click(object sender, EventArgs e)
        {
            string formulaString = "1 - sqrt((R*R+G*G+B*B)/3 - (R+G+B)*(R+G+B)/9) / Y";
            DenseMatrix dmTestData = (DenseMatrix)imgP.eval(formulaString, channelDataR.DmSourceData, channelDataG.DmSourceData, channelDataB.DmSourceData, null).Clone();

            dmTestData.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
            {
                if ((row + column) % 2 == 1) return -val;
                else return val;
            }));

            FFT2d transformator = new FFT2d(dmTestData);
            transformator.FFT2dForward();
            DenseMatrix dmAfterFFT2dReal = transformator.dmOutputReal;
            DenseMatrix dmAfterFFT2dImag = transformator.dmOutputImaginary;

            ArithmeticsOnImages aoiTmp = new ArithmeticsOnImages();
            aoiTmp.dmY = dmAfterFFT2dReal;
            aoiTmp.ExprString = "Y % 4";
            aoiTmp.RPNeval(true);
            dmAfterFFT2dReal = (DenseMatrix)aoiTmp.dmRes.Clone();

            aoiTmp.dmY = dmAfterFFT2dImag;
            aoiTmp.RPNeval(true);
            dmAfterFFT2dImag = (DenseMatrix)aoiTmp.dmRes.Clone();

            aoiTmp.Dispose();
            aoiTmp = null;


            ServiceTools.RepresentDataFromDenseMatrix(dmAfterFFT2dReal, "data after FFT2D, real part", true, true);
            ServiceTools.RepresentDataFromDenseMatrix(dmAfterFFT2dImag, "data after FFT2D, imaginary part", true, true);

            FFT2d transformatorInv = new FFT2d(dmAfterFFT2dReal, dmAfterFFT2dImag);
            transformatorInv.FFT2dInverse();
            DenseMatrix dmAfterFFTi2dReal = transformatorInv.dmOutputReal;
            dmAfterFFTi2dReal.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
            {
                if ((row + column) % 2 == 1) return -val;
                else return val;
            }));
            DenseMatrix dmAfterFFTi2dImag = transformatorInv.dmOutputImaginary;
            dmAfterFFTi2dImag.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
            {
                if ((row + column) % 2 == 1) return -val;
                else return val;
            }));
            //aoiTmp = new ArithmeticsOnImages();
            //aoiTmp.dmY = dmAfterFFTi2d;
            //aoiTmp.exprString = "Y % 3";
            //aoiTmp.RPNeval(true);
            //dmAfterFFT2d = (DenseMatrix)aoiTmp.dmRes.Clone();
            //aoiTmp.Dispose();
            //aoiTmp = null;

            ServiceTools.RepresentDataFromDenseMatrix(dmAfterFFTi2dReal, "restored data after FFT2D fwd+inv, real part");
            ServiceTools.RepresentDataFromDenseMatrix(dmAfterFFTi2dImag, "restored data after FFT2D fwd+inv, imaginary part");
        }






        private void button1_Click(object sender, EventArgs e)
        {
            #region Начальное тестирование алгоритмов по использование GrIx
            //string randomFileName = System.IO.Path.GetRandomFileName().Replace(".", "");
            ////string currentDirectory = Directory.GetCurrentDirectory() + "\\";
            //string currentDirectory = Properties.Settings.Default.DefaultDataFilesLocation;

            //string formulaString = "1 - sqrt((R*R+G*G+B*B)/3 - (R+G+B)*(R+G+B)/9) / Y";
            //DenseMatrix dmTestData = (DenseMatrix)imgP.eval(formulaString, channelDataR.DmSourceData, channelDataG.DmSourceData, channelDataB.DmSourceData, null).Clone();
            //DenseMatrix dmMask = ImageProcessing.DenseMatrixFromImage(imgP.significantMaskImageBinary);
            //dmTestData = (DenseMatrix)dmTestData.PointwiseMultiply(dmMask);
            //ImageConditionAndDataRepresentingForm originalDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmTestData, "original 1-sigma/Y data");


            //formulaString = "Y / 255";
            //DenseMatrix dmTotalLuminance = (DenseMatrix)imgP.eval(formulaString, channelDataR.DmSourceData, channelDataG.DmSourceData, channelDataB.DmSourceData, null).Clone();
            ////ServiceTools.RepresentDataFromDenseMatrix(dmTotalLuminance, "luminance matrix");
            //double totalLuminance = dmTotalLuminance.Values.Sum()/dmMask.Values.Sum();
            //theLogWindow = ServiceTools.LogAText(theLogWindow, "total luminance: "+totalLuminance.ToString());


            //#region оценки градиентов
            ////ArithmeticsOnImages aoiTmp = new ArithmeticsOnImages();
            ////aoiTmp.dmY = dmTestData;
            ////aoiTmp.exprString = "ddx(Y)";
            ////aoiTmp.RPNeval(true);
            ////DenseMatrix dmDDXofTestData = (DenseMatrix)aoiTmp.dmRes.Clone();
            ////aoiTmp.exprString = "ddy(Y)";
            ////aoiTmp.RPNeval(true);
            ////DenseMatrix dmDDYofTestData = (DenseMatrix)aoiTmp.dmRes.Clone();


            ////aoiTmp.dmY = dmDDXofTestData;
            ////aoiTmp.exprString = "mean(abs(Y) % 3)";
            ////aoiTmp.RPNeval(true);
            ////double ddxAbsMeanWithoutTails = aoiTmp.resValue;
            ////aoiTmp.exprString = "sigm(abs(Y) % 3)";
            ////aoiTmp.RPNeval(true);
            ////double ddxAbsSigmWithoutTails = aoiTmp.resValue;


            //#region для оценки распределения градиента ст.откл.
            ////aoiTmp.dmY = dmTestData;
            ////aoiTmp.exprString = "grad(Y)";
            ////aoiTmp.RPNeval(true);
            ////DenseMatrix dmGrad = (DenseMatrix)aoiTmp.dmRes.Clone();
            //#endregion для оценки распределения градиента ст.откл.

            ////aoiTmp.Dispose();
            ////aoiTmp = null;
            //#endregion  оценки градиентов

            //ServiceTools.FlushMemory();


            ////найдем центр солнца
            ////найдем границы маски по солнцу
            ////получим маску по гарантированному небу - пусть будет меньше 0,75
            ////степень отнесения точки к небу p = 1-S+min(S) - от 0.25+min(s) до 1
            ////посчитаем с весом (1-S) среднее расстояние от круга "неба" до центра солнца
            //////на этом расстоянии посчитаем среднее значение S
            //////из этого S и этого расстояния посчитаем опорное значение градиента
            ////Поднимем все данные согласно дальности от границы солнца и опорному значению градиента
            ////
            ////вытянуть данные: чем ближе к солнцу - тем больше выдавить/вытянуть
            ////а вот совсем рядом с солнцем не вытягивать - слишком сильно вылезают шумы
            ////НЕ ИСПОЛЬЗУЕТСЯ  отнормировали данные для распределения между 0.0 и 1.0
            ////


            //#region поиск солнца по засветке
            //DenseMatrix dmSunMask = DenseMatrix.Create(dmTestData.RowCount, dmTestData.ColumnCount,
            //    new Func<int, int, double>(
            //        (row, column) =>
            //        {
            //            if (dmTestData[row, column] == 1.0d) return 1.0d;
            //            else return 0.0d;
            //        }));
            //ImageConditionAndDataRepresentingForm sunMaskDataForm = ServiceTools.RepresentDataFromDenseMatrix(dmSunMask, "Sun mask");
            //sunMaskDataForm.SaveData(randomFileName + "_SunMask.nc");


            //PointD sunCenterPoint = new PointD(0.0d, 0.0d);
            //double weightsSum = dmSunMask.Values.Sum();
            //DenseMatrix dmSunCenterResearchingWeights = (DenseMatrix)dmSunMask.Clone();
            //dmSunCenterResearchingWeights.MapInplace(new Func<double, double>(val => val / weightsSum));
            //DenseMatrix dmSunCenterResearching = DenseMatrix.Create(dmSunCenterResearchingWeights.RowCount,
            //    dmSunCenterResearchingWeights.ColumnCount, new Func<int, int, double>((row, column) => (double)column));
            //dmSunCenterResearching = (DenseMatrix)dmSunCenterResearching.PointwiseMultiply(dmSunCenterResearchingWeights);
            //sunCenterPoint.X = dmSunCenterResearching.Values.Sum();
            //dmSunCenterResearching = DenseMatrix.Create(dmSunCenterResearchingWeights.RowCount,
            //    dmSunCenterResearchingWeights.ColumnCount, new Func<int, int, double>((row, column) => (double)row));
            //dmSunCenterResearching = (DenseMatrix)dmSunCenterResearching.PointwiseMultiply(dmSunCenterResearchingWeights);
            //sunCenterPoint.Y = dmSunCenterResearching.Values.Sum();
            //double sunRadius = Math.Sqrt(weightsSum / Math.PI);
            //#endregion поиск солнца по засветке



            //#region оценка применимости алгоритма подавления солнечной засветки
            //// оценить общую площадь засветки 255 - должно быть прилично ---СКОЛЬКО?
            //// найдем все куски площадью больше - ЧЕГО?
            //// оценить разброс кластеров засветки - должно быть единое круглое солнце, возможно, с лучами
            //// если засветки слишком мало - неприменимо, использовать без подавления
            //// если засветки слишком много - неприменимо, использовать без подавления

            //bool theSunSuppressionSchemeApplicable = true;
            //Image<Gray, Byte> imgSunMask = ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(
            //    dmTestData, 0.0d, 1.0d, false);
            //imgSunMask = imgSunMask.ThresholdBinary(new Gray(254), new Gray(255));
            //double overallSunArea = imgSunMask.CountNonzero()[0];
            //double overallimageMaskArea = imgP.maskSignificantArea;
            //double overallSunAreaPart = overallSunArea/overallimageMaskArea;
            ////if ((overallSunAreaPart < minSunAreaPart) || (overallSunAreaPart > 3 * maxSunAreaPart))
            ////{
            ////    theSunSuppressionSchemeApplicable = false;
            ////}



            //#endregion оценка применимости алгоритма подавления солнечной засветки

            //if (theSunSuppressionSchemeApplicable)
            //{
            //    DenseMatrix theSkyWeightAbs = DenseMatrix.Create(dmTestData.RowCount, dmTestData.ColumnCount,
            //        new Func<int, int, double>(
            //            (row, column) =>
            //            {
            //                if (dmTestData[row, column] < theStdDevMarginValueDefiningTrueSkyArea)
            //                    return 1 - dmTestData[row, column];
            //                else return 0.0d;
            //            }));
            //    theSkyWeightAbs = (DenseMatrix) theSkyWeightAbs.PointwiseMultiply(dmMask);

            //    double theSkyWeightSum = theSkyWeightAbs.Values.Sum();
            //    DenseMatrix theSkyWeight = (DenseMatrix) theSkyWeightAbs.Clone();
            //    theSkyWeight.MapInplace(new Func<double, double>(val => val/theSkyWeightSum));



            //    DenseMatrix dmDistanceToSunMargin = DenseMatrix.Create(theSkyWeight.RowCount, theSkyWeight.ColumnCount,
            //        new Func<int, int, double>(
            //            (row, column) =>
            //            {
            //                double dx = (double) column - sunCenterPoint.X;
            //                double dy = (double) row - sunCenterPoint.Y;
            //                double r = Math.Sqrt(dx*dx + dy*dy) - sunRadius;
            //                r = (r < 0.0d) ? (0.0d) : (r);
            //                return r;
            //            }));
            //    DenseMatrix dmDistanceToSunCenterWeighted =
            //        (DenseMatrix) dmDistanceToSunMargin.PointwiseMultiply(theSkyWeight);
            //    double averageSkyDistance = dmDistanceToSunCenterWeighted.Values.Sum();

            //    DenseMatrix dmDistanceToSunCenterDeviationWeighted = (DenseMatrix) dmDistanceToSunMargin.Clone();
            //    dmDistanceToSunCenterDeviationWeighted.MapInplace(
            //        new Func<double, double>(val => Math.Pow((val - averageSkyDistance), 2.0)));

            //    dmDistanceToSunCenterDeviationWeighted =
            //        (DenseMatrix) dmDistanceToSunCenterDeviationWeighted.PointwiseMultiply(theSkyWeight);
            //    double weightedStdDevSkyDistance = Math.Sqrt(dmDistanceToSunCenterDeviationWeighted.Values.Sum());

            //    DenseMatrix dmRaisedAccuracySkyWeights = (DenseMatrix) theSkyWeightAbs.Clone();
            //    ;
            //    dmRaisedAccuracySkyWeights.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
            //    {
            //        double r = dmDistanceToSunMargin[row, column];
            //        if (Math.Abs(r - averageSkyDistance) < weightedStdDevSkyDistance) return val;
            //        else return 0.0d;
            //    }));
            //    theSkyWeightSum = dmRaisedAccuracySkyWeights.Values.Sum();
            //    dmRaisedAccuracySkyWeights.MapInplace(new Func<double, double>(val => val/theSkyWeightSum));
            //    DenseMatrix dmSkyDataValues = (DenseMatrix) dmTestData.PointwiseMultiply(dmRaisedAccuracySkyWeights);
            //    double averageSkyDataValue = dmSkyDataValues.Values.Sum();
            //    double theGradIndicativeValue = (averageSkyDataValue - 1.0d)/averageSkyDistance;
            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "theGradIndicativeValue = " + theGradIndicativeValue.ToString("e"));

            //    #region проверка данных по определению центра и радиуса окружности, по которой определяем среднее расстояние для градиента

            //    //DenseMatrix dmDistanceToSunTheCircle = (DenseMatrix)dmDistanceToSunCenter.Clone();
            //    //dmDistanceToSunTheCircle.MapInplace(new Func<double, double>(
            //    //    (val) =>
            //    //    {
            //    //        if (val <= averageSkyDistance) return 255.0d;
            //    //        else return 100.0d;
            //    //    }));
            //    //
            //    //ImageConditionAndDataRepresentingForm testForm = ServiceTools.RepresentDataFromDenseMatrix(dmTestData);
            //    //Image<Gray, double> maskSunDistanceImage =
            //    //    ImageProcessing.grayscaleImageFromDenseMatrixWithFixedValuesBounds(dmDistanceToSunTheCircle, 0.0d,
            //    //        255.0d, false).Convert<Gray, double>();
            //    //maskSunDistanceImage = maskSunDistanceImage/255.0;
            //    //testForm.imgData.HighlightMask = maskSunDistanceImage;
            //    //testForm.ImageConditionAndDataRepresentingForm_Paint(null, null);

            //    #endregion проверка данных по определению центра и радиуса окружности, по которой определяем среднее расстояние для градиента

            //    #region посмотрим распределение градиентов в области внутри выделенного круга

            //    //dmGrad.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
            //    //{
            //    //    if (dmDistanceToSunCenter[row, column] >= averageSkyDistance) return 0.0d;
            //    //    else return val;
            //    //}));
            //    //
            //    //aoiTmp = new ArithmeticsOnImages();
            //    //aoiTmp.dmY = dmGrad;
            //    //aoiTmp.exprString = "Y % 3";
            //    //aoiTmp.RPNeval(true);
            //    //dmGrad = (DenseMatrix)aoiTmp.dmRes.Clone();
            //    //aoiTmp.dmY = dmGrad;

            //    #endregion посмотрим распределение градиентов в области внутри выделенного круга

            //    #region вариация использования значений градиента прямо из поля - не работает

            //    ////градиенты посчитали - довольно узкое распределение, надо им воспользоваться
            //    ////есть среднее значение градиента, есть стандартное отклонение. Используем.
            //    //aoiTmp.exprString = "mean(Y)";
            //    //aoiTmp.RPNeval(true);
            //    //double gradFilterMeanVal = aoiTmp.resValue;
            //    //aoiTmp.exprString = "sigm(Y)";
            //    //aoiTmp.RPNeval(true);
            //    //double gradFilterStDev = aoiTmp.resValue;
            //    //aoiTmp.Dispose();
            //    //aoiTmp = null;
            //    //ServiceTools.FlushMemory();
            //    //
            //    //theLogWindow = ServiceTools.LogAText(theLogWindow, "average grad value = " + gradFilterMeanVal.ToString("e"));
            //    //theLogWindow = ServiceTools.LogAText(theLogWindow, "grad std dev = " + gradFilterStDev.ToString("e"));
            //    //
            //    //theLogWindow = ServiceTools.LogAText(theLogWindow, "abs(ddx) MEAN = " + ddxAbsMeanWithoutTails.ToString());
            //    //theLogWindow = ServiceTools.LogAText(theLogWindow, "abs(ddx) sigm = " + ddxAbsSigmWithoutTails.ToString());
            //    //
            //    //dmDDXofTestData.MapInplace(new Func<double, double>((val) =>
            //    //{
            //    //    if (Math.Abs(val - gradFilterMeanVal) < gradFilterStDev) return 0.0d;
            //    //    else return val;
            //    //}));
            //    //
            //    //
            //    ////ServiceTools.RepresentDataFromDenseMatrix(dmDDXofTestData);
            //    //
            //    //
            //    //DenseMatrix dmTestReversed = DenseMatrix.Create(dmTestData.RowCount, dmTestData.ColumnCount,
            //    //    new Func<int, int, double>((row, column) => 0.0d));
            //    //for (int i = 1; i < dmTestReversed.ColumnCount; i++)
            //    //{
            //    //    DenseVector prevVector = (DenseVector)dmTestReversed.Column(i - 1);
            //    //    DenseVector ddxVector = (DenseVector)dmDDXofTestData.Column(i - 1);
            //    //    DenseVector currVector = prevVector + ddxVector;
            //    //    dmTestReversed.SetColumn(i, currVector);
            //    //}

            //    #endregion вариация использования значений градиента прямо из поля - не работает


            //    DenseMatrix dmTestReversed = (DenseMatrix) dmTestData.Clone();
            //    dmTestReversed.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
            //    {
            //        double currDist = dmDistanceToSunMargin[row, column];
            //        if (currDist == 0.0d)
            //        {
            //            return val;
            //        }
            //        else if (dmMask[row, column] == 0.0d)
            //        {
            //            return 1.0d;
            //        }
            //        else if (currDist <= averageSkyDistance)
            //        {
            //            return val - (currDist*theGradIndicativeValue);
            //        }
            //        else return val - (averageSkyDistance*theGradIndicativeValue);
            //    }));


            //    #region данные и Matlab-скрипт для вывода данных в горизонтальном разрезе через центр солнца

            //    int leftSunMargin = Convert.ToInt32(sunCenterPoint.X - sunRadius);
            //    int rightSunMargin = Convert.ToInt32(sunCenterPoint.X + sunRadius);
            //    int topSunMargin = Convert.ToInt32(sunCenterPoint.Y - sunRadius);
            //    int bottomSunMargin = Convert.ToInt32(sunCenterPoint.Y + sunRadius);
            //    int leftAverageSkyMargin = Convert.ToInt32(leftSunMargin - averageSkyDistance);
            //    int rightAverageSkyMargin = Convert.ToInt32(rightSunMargin + averageSkyDistance);
            //    int topAverageSkyMargin = Convert.ToInt32(topSunMargin - averageSkyDistance);
            //    int bottomAverageSkyMargin = Convert.ToInt32(bottomSunMargin + averageSkyDistance);
            //    double leftA = 1.0 - Math.Abs(theGradIndicativeValue)*leftSunMargin;
            //    double rightA = 1.0 + Math.Abs(theGradIndicativeValue)*rightSunMargin;
            //    double topA = 1.0 - Math.Abs(theGradIndicativeValue) * topSunMargin;
            //    double bottomA = 1.0 + Math.Abs(theGradIndicativeValue) * bottomSunMargin;

            //    string matlabScript = "origDataMatrix = ncread('" + currentDirectory + randomFileName +
            //                    "_orig.nc','dataMatrix');" + Environment.NewLine;
            //    matlabScript += "resultDataMatrix = ncread('" + currentDirectory + randomFileName +
            //                    "_res.nc','dataMatrix');" + Environment.NewLine;
            //    matlabScript += "origDataVect = origDataMatrix(1:end," +
            //                    Convert.ToInt32(Math.Round(sunCenterPoint.Y, 0)) + ");" + Environment.NewLine;
            //    matlabScript += "resultDataVect = resultDataMatrix(1:end," +
            //                    Convert.ToInt32(Math.Round(sunCenterPoint.Y, 0)) + ");" + Environment.NewLine;
            //    matlabScript += "xSpace = 1:1:" + dmTestReversed.ColumnCount + ";" + Environment.NewLine;
            //    matlabScript += "markersValues(xSpace) = " + averageSkyDataValue.ToString("e").Replace(",", ".") + ";" +
            //                    Environment.NewLine;
            //    matlabScript += "markersValues(xSpace <= " + leftAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
            //    matlabScript += "markersValues(xSpace >= " + rightAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
            //    matlabScript += "approxFunc1(xSpace) = " + leftA.ToString("e").Replace(",", ".") + " + " +
            //                    Math.Abs(theGradIndicativeValue).ToString("e").Replace(",", ".") + " * xSpace;" +
            //                    Environment.NewLine;
            //    matlabScript += "approxFunc1(xSpace >= " + leftSunMargin + ") = 1.0;" + Environment.NewLine;
            //    matlabScript += "approxFunc2(xSpace) = " + rightA.ToString("e").Replace(",", ".") + " - " +
            //                    Math.Abs(theGradIndicativeValue).ToString("e").Replace(",", ".") + " * xSpace;" +
            //                    Environment.NewLine;
            //    matlabScript += "approxFunc2(xSpace <= " + rightSunMargin + ") = 1.0;" + Environment.NewLine;
            //    matlabScript +=
            //        "plot(xSpace, origDataVect, xSpace, resultDataVect, xSpace, markersValues, xSpace, approxFunc1, xSpace, approxFunc2);" +
            //        Environment.NewLine;

            //    matlabScript += "origDataVectOrtho = origDataMatrix(" +
            //                    Convert.ToInt32(Math.Round(sunCenterPoint.X, 0)) + ", 1:end);" + Environment.NewLine;
            //    matlabScript += "resultDataVectOrtho = resultDataMatrix(" + Convert.ToInt32(Math.Round(sunCenterPoint.X, 0)) + ", 1:end);" + Environment.NewLine;
            //    matlabScript += "xSpaceOrtho = 1:1:" + dmTestReversed.RowCount + ";" + Environment.NewLine;
            //    matlabScript += "markersValuesOrtho(xSpaceOrtho) = " + averageSkyDataValue.ToString("e").Replace(",", ".") + ";" +
            //                    Environment.NewLine;
            //    matlabScript += "markersValuesOrtho(xSpaceOrtho <= " + topAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
            //    matlabScript += "markersValuesOrtho(xSpaceOrtho >= " + bottomAverageSkyMargin + ") = 0.0;" + Environment.NewLine;
            //    matlabScript += "approxFunc3(xSpaceOrtho) = " + topA.ToString("e").Replace(",", ".") + " + " +
            //                    Math.Abs(theGradIndicativeValue).ToString("e").Replace(",", ".") + " * xSpaceOrtho;" +
            //                    Environment.NewLine;
            //    matlabScript += "approxFunc3(xSpaceOrtho >= " + topSunMargin + ") = 1.0;" + Environment.NewLine;
            //    matlabScript += "approxFunc4(xSpaceOrtho) = " + bottomA.ToString("e").Replace(",", ".") + " - " +
            //                    Math.Abs(theGradIndicativeValue).ToString("e").Replace(",", ".") + " * xSpaceOrtho;" +
            //                    Environment.NewLine;
            //    matlabScript += "approxFunc4(xSpaceOrtho <= " + bottomSunMargin + ") = 1.0;" + Environment.NewLine;
            //    matlabScript += "figure;";
            //    matlabScript +=
            //        "plot(xSpaceOrtho, origDataVectOrtho, xSpaceOrtho, resultDataVectOrtho, xSpaceOrtho, markersValuesOrtho, xSpaceOrtho, approxFunc3, xSpaceOrtho, approxFunc4);" +
            //        Environment.NewLine;



            //    theLogWindow = ServiceTools.LogAText(theLogWindow, Environment.NewLine + Environment.NewLine + "files saved:" + Environment.NewLine);
            //    theLogWindow = ServiceTools.LogAText(theLogWindow, currentDirectory + randomFileName + "_orig.nc" + Environment.NewLine);
            //    theLogWindow = ServiceTools.LogAText(theLogWindow, currentDirectory + randomFileName + "_res.nc" + Environment.NewLine);
            //    theLogWindow = ServiceTools.LogAText(theLogWindow, currentDirectory + randomFileName + "_MatlabScript.m" + Environment.NewLine);

            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "===============================MATLAB SCRIPT===============================" + Environment.NewLine);
            //    theLogWindow = ServiceTools.LogAText(theLogWindow, matlabScript);
            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "===============================MATLAB SCRIPT===============================" + Environment.NewLine);

            //    #endregion данные и Matlab-скрипт для вывода данных в горизонтальном разрезе через центр солнца


            //    DenseMatrix dmTestReversedInsideSkyCircle = (DenseMatrix) dmTestReversed.Clone();
            //    dmTestReversedInsideSkyCircle.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
            //    {
            //        double currDist = dmDistanceToSunMargin[row, column];
            //        if ((currDist >= averageSkyDistance) || (currDist == 0.0d) || (dmMask[row, column] == 0.0d))
            //            return 0.0d;
            //        else return val;
            //    }));
            //    double dataMinValueInsideSkyCircle = dmTestReversedInsideSkyCircle.Values.Min();
            //    double dataMaxValueInsideSkyCircle = dmTestReversedInsideSkyCircle.Values.Max();


            //    double dataMinValue = dmTestReversed.Values.Min();
            //    double dataMaxValue = dmTestReversed.Values.Max();
            //    double dataValuesRange = dataMaxValue - dataMinValue;

            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "the image circled mask radius: " + imgP.imageRD.DRadius.ToString());
            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "the image circled mask center: " + imgP.imageRD.pointfCircleCenter().ToString());
            //    theLogWindow = ServiceTools.LogAText(theLogWindow, "the sun center: " + sunCenterPoint.ToString());
            //    theLogWindow = ServiceTools.LogAText(theLogWindow, "the sun radius: " + sunRadius.ToString());
            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "average sky distance to sun margin: " + averageSkyDistance);
            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "average sky data value at average sky distance: " + averageSkyDataValue);

            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "resulting data min value = " + dataMinValue.ToString("e"));
            //    theLogWindow = ServiceTools.LogAText(theLogWindow,
            //        "resulting data max value = " + dataMaxValue.ToString("e"));

            //    dmTestReversed.MapInplace(new Func<double, double>(val => val - dataMinValueInsideSkyCircle - 1.0d));
            //    dmTestReversed = (DenseMatrix) dmTestReversed.PointwiseMultiply(dmMask);
            //    //dmTestReversed.MapInplace(new Func<double, double>(val => val / dataValuesRange));

            //    double H = 1.0d - averageSkyDataValue;
            //    double k = Math.Abs(theGradIndicativeValue);
            //    double f = 1.0d/3.0d;
            //    double p = (H/(k*f*averageSkyDistance) - 1)/(f*averageSkyDistance);


            //    dmTestReversed.MapIndexedInplace(new Func<int, int, double, double>((row, column, val) =>
            //    {
            //        double currDist = dmDistanceToSunMargin[row, column];
            //        if ((currDist >= averageSkyDistance) || (currDist == 0.0d) || (dmMask[row, column] == 0.0d))
            //            return val;
            //        else if (val <= 0.0d) return val;
            //        else if (currDist <= averageSkyDistance*f) return val*(1.0 + p*currDist);
            //        else return val*H/(k*currDist);
            //    }));
            //    ImageConditionAndDataRepresentingForm restoredDataForm =
            //        ServiceTools.RepresentDataFromDenseMatrix(dmTestReversed, "finally restored 1-sigma/Y data");

            //    ServiceTools.logToTextFile(currentDirectory + randomFileName + "_MatlabScript.m", matlabScript, false);
            //    restoredDataForm.SaveData(randomFileName + "_res.nc");
            //    originalDataForm.SaveData(randomFileName + "_orig.nc");
            //}
            //else
            //{

            //}
            #endregion Начальное тестирование алгоритмов по использование GrIx
        }



        private void btnRes1Hist_Click(object sender, EventArgs e)
        {
            imageConditionAndData currImagData = null;
            string desc = "";
            if (sender == btnRes1Hist)
            {
                currImagData = result1;
                desc = tbFormula1.Text;
            }
            else if (sender == btnRes2Hist)
            {
                currImagData = result2;
                desc = tbFormula2.Text;
            }
            else if (sender == btnRes3Hist)
            {
                currImagData = result3;
                desc = tbFormula3.Text;
            }

            DenseMatrix dmProcessingData = (DenseMatrix)currImagData.DmSourceData.Clone();

            DenseVector dvDataToHist = DataAnalysis.DataVectorizedExcludingValues(dmProcessingData, 0.0d);
            HistogramCalcAndShowForm histForm = new HistogramCalcAndShowForm("histogram: " + desc, defaultProperties);
            HistogramDataAndProperties theHist = new HistogramDataAndProperties(dvDataToHist, 100);
            theHist.color = Color.Red;
            theHist.description = desc;
            histForm.HistToRepresent = theHist;
            histForm.Show();
            histForm.Represent();

        }

        /// <summary>
        /// Handles the Click event of the buttonSectionProfile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void buttonSectionProfile_Click(object sender, EventArgs e)
        {
            imageConditionAndData currImagData = null;
            if (sender == btnSectionProfile1)
            {
                currImagData = result1;
            }
            else if (sender == btnSectionProfile2)
            {
                currImagData = result2;
            }
            else if (sender == btnSectionProfile3)
            {
                currImagData = result3;
            }

            AdditionalFieldAnalysisForm anForm = new AdditionalFieldAnalysisForm(currImagData.DmSourceData);
            anForm.Show();

            #region // вынесено в отдельную форму
            //PointD p1 = new PointD();
            //PointD p2 = new PointD();
            //bool fromMarginToMargin = false;
            //SectionRequestForm sectionRequestForm = new SectionRequestForm();
            //DialogResult res = sectionRequestForm.ShowDialog();
            //if (res == System.Windows.Forms.DialogResult.Cancel)
            //{
            //    return;
            //}

            //p1 = sectionRequestForm.retPt1;
            //p2 = sectionRequestForm.retPt2;
            //fromMarginToMargin = sectionRequestForm.fromMarginToMargin;
            //SectionDescription currSection = new SectionDescription(p1, p2, true);
            //currSection = currSection.TransformTillMargins(new Rectangle(0, 0, currImagData.Width, currImagData.Height));
            //LineDescription l1 = currSection.SectionLine;

            //DenseMatrix dmValues = (DenseMatrix) currImagData.DmSourceData.Clone();
            //DenseMatrix dmDistanceToLine = (DenseMatrix)currImagData.DmSourceData.Clone();
            //dmDistanceToLine.MapIndexedInplace((row, col, dVal) =>
            //{
            //    PointD currPt = new PointD(col, row);

            //    double dist = currPt.DistanceToLine(l1);
            //    return dist;
            //});

            //List<Tuple<PointD, double>> dataArray = new List<Tuple<PointD, double>>();
            //for (int row = 0; row < dmValues.RowCount; row++)
            //{
            //    for (int col = 0; col < dmValues.ColumnCount; col++)
            //    {
            //        if (dmDistanceToLine[row, col] <= 1.0d)
            //            dataArray.Add(new Tuple<PointD, double>(new PointD(col, row), dmValues[row, col]));
            //    }
            //}

            //List<Tuple<double, double>> dataArrayRotated = dataArray.ConvertAll((tpl) =>
            //{
            //    PointPolar ptp = new PointPolar(tpl.Item1 - new SizeD(l1.p0));
            //    double angleToSubtract = (new PointPolar(new PointD(l1.directionVector[0], l1.directionVector[1]))).Phi;
            //    ptp.Phi -= angleToSubtract;
            //    //ptp.CropAngle(true);
            //    if (ptp.Phi >= 0.0d) return new Tuple<double, double>(ptp.R, tpl.Item2);
            //    else return new Tuple<double, double>(-ptp.R, tpl.Item2);
            //});

            //double arrayMinPosition = dataArrayRotated.Min<Tuple<double, double>>(tpl1 => tpl1.Item1);
            //dataArrayRotated =
            //    dataArrayRotated.ConvertAll<Tuple<double, double>>(
            //        tpl => new Tuple<double, double>(tpl.Item1 - arrayMinPosition, tpl.Item2));

            //dataArrayRotated.Sort((tpl1, tpl2) => tpl1.Item1.CompareTo(tpl2.Item1));

            //FunctionRepresentationForm form1 = new FunctionRepresentationForm();
            //form1.dvScatterXSpace = DenseVector.OfEnumerable(dataArrayRotated.ConvertAll<double>(tpl => tpl.Item1));
            //form1.dvScatterFuncValues = DenseVector.OfEnumerable(dataArrayRotated.ConvertAll<double>(tpl => tpl.Item2));
            //form1.Show();
            //form1.Represent();
            ////form1.SaveToImage("D:\\MMAEs-2014MSU\\output\\img-2014-09-20T12-46-55devID1-res002.jpg");
            #endregion // вынесено в отдельную форму
        }







        #region Обработка действий мышкой на картинках с результатами

        private void pbRes_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                meResLeftButtonDownArgs = e;
                meResLeftButtonDownSender = sender;
            }
        }

        private void pbRes_MouseUp(object sender, MouseEventArgs e)
        {
            imageConditionAndData currentICD = null;
            Type theSenderDataType = typeof(imageConditionAndData);
            PictureBoxSelection pbSelection = null;


            if (e.Button == MouseButtons.Left && sender.Equals(meResLeftButtonDownSender))
            {
                if (sender == pbRes1)
                {
                    currentICD = result1;
                    theSenderDataType = result1.GetType();
                }
                else if (sender == pbRes2)
                {
                    currentICD = result2;
                    theSenderDataType = result2.GetType();
                }
                else if (sender == pbRes3)
                {
                    currentICD = result3;
                    theSenderDataType = result3.GetType();
                }
                


                //если уже есть selection у этого объекта, а это выделение пусто - проверить, было ли оно внутри
                //если было внутри - значит, был клик или даблклик внутри выделения - не обрабатывать здесь
                if (theSenderDataType == typeof(imageConditionAndData))
                {
                    pbSelection = new PictureBoxSelection(sender, meResLeftButtonDownArgs.Location, e.Location, currentICD);

                    if ((pbSelection.IsEmptySelection) && (currentICD.Selection != null))
                    {
                        if (currentICD.Selection.CheckIfDoubleclickedinsideSelection(sender, e, currentICD))
                        {
                            return;
                        }
                    }
                    currentICD.Selection = pbSelection;
                }


                HighlightLinkedSelection(pbSelection);
                RaisePaintEvent(null, null);

            }
        }

        #endregion Обработка действий мышкой на картинках с результатами


    }
}
