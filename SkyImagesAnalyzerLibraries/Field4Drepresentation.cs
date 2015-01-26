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
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using SkyImagesAnalyzerLibraries;

namespace SkyImagesAnalyzerLibraries
{
    public partial class Field4Drepresentation : Form
    {
        private string strOutputDirectory = "";
        private Dictionary<string, object> defaultProperties = null;
        public string strDataDescription = "";

        public DenseMatrix dmDataList = null;
        public SlicingVariable variableSliceBy = SlicingVariable.z;

        public List<PointD> lPtdMarks = new List<PointD>();

        private ILPlotCube currSurfPlotCube = null;




        public Field4Drepresentation()
        {
            InitializeComponent();
        }




        public enum SlicingVariable
        {
            x,
            y,
            z
        }






        public Field4Drepresentation(
            DenseMatrix dmListOfData,
            Dictionary<string, object> properties,
            bool byIsolines = true,
            string description = "")
        {
            InitializeComponent();

            strDataDescription = description;
            ThreadSafeOperations.SetText(lblDescription, strDataDescription, false);

            defaultProperties = properties;
            strOutputDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
            if (!ServiceTools.CheckIfDirectoryExists(strOutputDirectory))
            {
                strOutputDirectory = "";
            }

            dmDataList = dmListOfData.Copy();
            double dataMaxVal = dmDataList.Column(3).Max();
            double dataMinVal = dmDataList.Column(3).Min();

            ILScene scene = new ILScene();
            currSurfPlotCube = new ILPlotCube();
            currSurfPlotCube.TwoDMode = false;

            List<List<DenseMatrix>> llDataMatricesSlicedByZ = ReshapeDataToMatrices(dmDataList, SlicingVariable.z);
            DenseMatrix dmXvalues = llDataMatricesSlicedByZ[0][0].Copy();
            DenseMatrix dmYvalues = llDataMatricesSlicedByZ[0][1].Copy();

            List<List<DataValuesOver3DGrid>> lDataVectorsForSurfices = Group_DVOG_DataByValues(dmDataList);

            foreach (List<DataValuesOver3DGrid> currSurfVectorsList in lDataVectorsForSurfices)
            {
                //ILInArray<float> ilaXvalues =
                //    currSurfVectorsList.ConvertAll<float>(dvog => Convert.ToSingle(dvog.x)).ToArray();
                //ILInArray<float> ilaYvalues =
                //    currSurfVectorsList.ConvertAll<float>(dvog => Convert.ToSingle(dvog.y)).ToArray();
                //ILInArray<float> ilaZvalues =
                //    currSurfVectorsList.ConvertAll<float>(dvog => Convert.ToSingle(dvog.z)).ToArray();

                //ILSurface currSurf = new ILSurface(ilaZvalues, ilaXvalues, ilaYvalues);
                // не катит - надо, чтобы сетка z была m*n, сетка x = m*[1|n], сетка y - [1|m]*n
                // поэтому просто список точек, которые должны составить поверхность, - не катят
                //  => или отрисовывать множества точек, без привязки именно к понятию поверхности. Это пока не получилось
                //  => или переформировать список точек так, чтобы они составили m*n поверхность

                // скомпоновать матрицу значений Z, соответствующих значениям x и y
                DenseMatrix dmZvalues = DenseMatrix.Create(dmXvalues.RowCount, dmXvalues.ColumnCount, (r, c) =>
                {
                    double x = dmXvalues[r, c];
                    double y = dmYvalues[r, c];
                    int idx = currSurfVectorsList.FindIndex(dvog => ((dvog.x == x) && (dvog.y == y)));
                    if (idx == -1) // ничего нужного нет
                    {
                        return double.NaN;
                    }
                    else return currSurfVectorsList[idx].z;
                });
                ILArray<double> arrXvalues = (ILArray<double>)(dmXvalues.ToArray());
                ILArray<double> arrYvalues = (ILArray<double>)(dmYvalues.ToArray());
                ILArray<double> arrZvalues = (ILArray<double>)(dmZvalues.ToArray());

                // сформируем colors array
                ColorScheme newCS = new ColorScheme("");
                DenseMatrix dmValues = DenseMatrix.Create(dmXvalues.RowCount, dmXvalues.ColumnCount, (r, c) =>
                {
                    double x = dmXvalues[r, c];
                    double y = dmYvalues[r, c];
                    int idx = currSurfVectorsList.FindIndex(dvog => ((dvog.x == x) && (dvog.y == y)));
                    if (idx == -1) // ничего нужного нет
                    {
                        return double.NaN;
                    }
                    else return currSurfVectorsList[idx].values[0];
                });

                double[,] dmRvalues = DenseMatrix.Create(dmValues.RowCount, dmValues.ColumnCount,
                    (r, c) =>
                    {
                        Bgr currColor = newCS.GetColorByValueAndRange(dmValues[r, c], dataMinVal, dataMaxVal);
                        return currColor.Red / 255.0d;
                    }).ToArray();
                double[,] dmGvalues = DenseMatrix.Create(dmValues.RowCount, dmValues.ColumnCount,
                    (r, c) =>
                    {
                        Bgr currColor = newCS.GetColorByValueAndRange(dmValues[r, c], dataMinVal, dataMaxVal);
                        return currColor.Green / 255.0d;
                    }).ToArray();
                double[,] dmBvalues = DenseMatrix.Create(dmValues.RowCount, dmValues.ColumnCount,
                    (r, c) =>
                    {
                        Bgr currColor = newCS.GetColorByValueAndRange(dmValues[r, c], dataMinVal, dataMaxVal);
                        return currColor.Blue / 255.0d;
                    }).ToArray();
                float[, ,] rgbaArrData = new float[4, dmRvalues.GetLength(0), dmRvalues.GetLength(1)];
                for (int i = 0; i < dmRvalues.GetLength(0); i++)
                {
                    for (int j = 0; j < dmRvalues.GetLength(1); j++)
                    {
                        rgbaArrData[0, i, j] = Convert.ToSingle(dmRvalues[i, j]);
                        rgbaArrData[1, i, j] = Convert.ToSingle(dmGvalues[i, j]);
                        rgbaArrData[2, i, j] = Convert.ToSingle(dmBvalues[i, j]);
                        rgbaArrData[3, i, j] = 0.3f;
                    }
                }



                ILSurface currSurf = new ILSurface(ILMath.convert<double, float>(arrZvalues),
                    ILMath.convert<double, float>(arrXvalues), ILMath.convert<double, float>(arrYvalues), rgbaArrData);

                
                //currSurf.ColorMode = ILSurface.ColorModes.Solid;
                //double coloredValue = currSurfVectorsList.Min(dvog => dvog.values[0]);
                //currSurf.UpdateSolidColor(
                //    newCS.GetColorByValueAndRange(coloredValue, dataMinVal, dataMaxVal).ToRGBColor());

                
                
                //ColorSchemeRuler newCSR = new ColorSchemeRuler(newCS, dataMinVal, dataMaxVal);
                
                //currSurf.UpdateRGBA(null, rgbaArrData);

                currSurf.UseLighting = true;

                currSurfPlotCube.Children.Add(currSurf);

                //if (currSurfPlotCube.Children.Count() > 4)
                //{
                //    currSurfPlotCube.Children.Add(new ILColorbar());
                //}


                //if (currSurfPlotCube.Children.Count() > 4)
                //{
                //    break;
                //}
            }


            //// surf.Children.Add(new ILColorbar());

            ////currSurfPlotCube.Children.Add(new ILColorbar());

            ////currSurfPlotCube.Rotation = Matrix4.Rotation(new Vector3(1, 0, 0), 1.2f) *
            ////                    Matrix4.Rotation(new Vector3(0, 0, 1), Math.PI);

            currSurfPlotCube.Projection = Projection.Orthographic;

            currSurfPlotCube.Plots.Reset();

            scene.Add(currSurfPlotCube);

            ilPanel1.Scene = scene;


        }






        public Field4Drepresentation(
            DenseMatrix dmListOfData,
            Dictionary<string, object> properties,
            Dictionary<string, int> valuesColumnsIndeces = null,
            SlicingVariable varSliceBy = SlicingVariable.z,
            string description = "")
        {
            InitializeComponent();

            variableSliceBy = varSliceBy;

            if (valuesColumnsIndeces == null)
            {
                valuesColumnsIndeces = new Dictionary<string, int>();
                valuesColumnsIndeces.Add("x", 0);
                valuesColumnsIndeces.Add("y", 1);
                valuesColumnsIndeces.Add("z", 2);
                valuesColumnsIndeces.Add("values", 3);
            }

            strDataDescription = description;
            ThreadSafeOperations.SetText(lblDescription, strDataDescription, false);

            defaultProperties = properties;
            strOutputDirectory = (string)defaultProperties["DefaultDataFilesLocation"];
            if (!ServiceTools.CheckIfDirectoryExists(strOutputDirectory))
            {
                strOutputDirectory = "";
            }

            dmDataList = dmListOfData.Copy();
            double dataMaxVal = dmDataList.Column(3).Max();
            double dataMinVal = dmDataList.Column(3).Min();

            ILScene scene = new ILScene();
            currSurfPlotCube = new ILPlotCube();
            currSurfPlotCube.TwoDMode = false;



            List<List<DenseMatrix>> lDataMatricesForSurfices = ReshapeDataToMatrices(dmDataList, variableSliceBy);



            foreach (List<DenseMatrix> currSurfMatricesList in lDataMatricesForSurfices)
            {
                ILInArray<double> ilaXvalues = currSurfMatricesList[0].ToArray();
                ILInArray<double> ilaYvalues = currSurfMatricesList[1].ToArray();
                ILInArray<double> ilaZvalues = currSurfMatricesList[2].ToArray();

                MathNet.Numerics.LinearAlgebra.Single.DenseMatrix floatDMcolors =
                    MathNet.Numerics.LinearAlgebra.Single.DenseMatrix.Create(currSurfMatricesList[0].RowCount,
                        currSurfMatricesList[0].ColumnCount, 0.0f);
                currSurfMatricesList[3].MapConvert<Single>(dval => Convert.ToSingle(dval), floatDMcolors,
                    MathNet.Numerics.LinearAlgebra.Zeros.Include);
                ILInArray<float> ilaForColorValues = floatDMcolors.ToArray();


                ILSurface currSurf = new ILSurface(ilaZvalues, ilaXvalues, ilaYvalues);
                currSurf.ColorMode = ILSurface.ColorModes.RBGA;

                ColorScheme newCS = new ColorScheme("");
                //ColorSchemeRuler newCSR = new ColorSchemeRuler(newCS, dataMinVal, dataMaxVal);
                double[,] dmRvalues = DenseMatrix.Create(floatDMcolors.RowCount, floatDMcolors.ColumnCount,
                    (r, c) =>
                    {
                        Bgr currColor = newCS.GetColorByValueAndRange(currSurfMatricesList[3][r, c], dataMinVal, dataMaxVal);
                        return currColor.Red / 255.0d;
                    }).ToArray();
                double[,] dmGvalues = DenseMatrix.Create(floatDMcolors.RowCount, floatDMcolors.ColumnCount,
                    (r, c) =>
                    {
                        Bgr currColor = newCS.GetColorByValueAndRange(currSurfMatricesList[3][r, c], dataMinVal, dataMaxVal);
                        return currColor.Green / 255.0d;
                    }).ToArray();
                double[,] dmBvalues = DenseMatrix.Create(floatDMcolors.RowCount, floatDMcolors.ColumnCount,
                    (r, c) =>
                    {
                        Bgr currColor = newCS.GetColorByValueAndRange(currSurfMatricesList[3][r, c], dataMinVal, dataMaxVal);
                        return currColor.Blue / 255.0d;
                    }).ToArray();
                float[, ,] rgbaArrData = new float[4, dmRvalues.GetLength(0), dmRvalues.GetLength(1)];
                for (int i = 0; i < dmRvalues.GetLength(0); i++)
                {
                    for (int j = 0; j < dmRvalues.GetLength(1); j++)
                    {
                        rgbaArrData[0, i, j] = Convert.ToSingle(dmRvalues[i, j]);
                        rgbaArrData[1, i, j] = Convert.ToSingle(dmGvalues[i, j]);
                        rgbaArrData[2, i, j] = Convert.ToSingle(dmBvalues[i, j]);
                        rgbaArrData[3, i, j] = 0.3f;
                    }
                }

                currSurf.UpdateRGBA(null, rgbaArrData);

                currSurf.UseLighting = true;

                currSurfPlotCube.Children.Add(currSurf);

                //if (currSurfPlotCube.Children.Count() > 4)
                //{
                //    currSurfPlotCube.Children.Add(new ILColorbar());
                //}


                //if (currSurfPlotCube.Children.Count() > 4)
                //{
                //    break;
                //}
            }

            // surf.Children.Add(new ILColorbar());

            //currSurfPlotCube.Children.Add(new ILColorbar());

            //currSurfPlotCube.Rotation = Matrix4.Rotation(new Vector3(1, 0, 0), 1.2f) *
            //                    Matrix4.Rotation(new Vector3(0, 0, 1), Math.PI);

            currSurfPlotCube.Projection = Projection.Orthographic;

            currSurfPlotCube.Plots.Reset();

            scene.Add(currSurfPlotCube);

            ilPanel1.Scene = scene;

        }





        ILColormap ConstructILColormap(ColorScheme theScheme, double dataMinValue, double dataMaxValue)
        {
            ColorScheme newCS = new ColorScheme("");
            float[,] colorsArr = new float[5, newCS.colorsArray.Count];
            int i = 0;
            foreach (Bgr bgrColor in newCS.colorsArray)
            {
                colorsArr[0, i] = Convert.ToSingle(dataMinValue + (dataMaxValue - dataMinValue) * (double)i / (double)newCS.colorsArray.Count);
                colorsArr[1, i] = Convert.ToSingle(bgrColor.Red / 255.0d);
                colorsArr[2, i] = Convert.ToSingle(bgrColor.Green / 255.0d);
                colorsArr[3, i] = Convert.ToSingle(bgrColor.Blue / 255.0d);
                colorsArr[4, i] = 1.0f;

                i++;
            }

            ILColormap retColormap = new ILColormap(colorsArr);
            return retColormap;
        }




        List<List<DenseMatrix>> ReshapeDataToMatrices(DenseMatrix dmRawDataByColumns, SlicingVariable varSliceBy)
        {
            List<List<DenseMatrix>> retSurficesDataMatricesList = new List<List<DenseMatrix>>();
            List<DataValuesOver3DGrid> lDataList = new List<DataValuesOver3DGrid>();
            foreach (Vector<double> vec in dmRawDataByColumns.EnumerateRows())
            {
                lDataList.Add(new DataValuesOver3DGrid(vec.ToArray()));
            }

            List<IGrouping<double, DataValuesOver3DGrid>> groupingByLeadingVar =
                new List<IGrouping<double, DataValuesOver3DGrid>>(lDataList.GroupBy(dvog =>
                {
                    switch (varSliceBy)
                    {
                        case SlicingVariable.x:
                            {
                                return dvog.x;
                                break;
                            }
                        case SlicingVariable.y:
                            {
                                return dvog.y;
                                break;
                            }
                        case SlicingVariable.z:
                            {
                                return dvog.z;
                                break;
                            }
                        default:
                            {
                                return dvog.z;
                                break;
                            }
                    }
                }));

            foreach (IGrouping<double, DataValuesOver3DGrid> currGrouping in groupingByLeadingVar)
            {
                // собрать матрицы для значений x, y, отображаемых значений
                // на забыть, что матрица (идентичных) значений z тоже должна быть на выходе - третьей
                // посчитаем, сколько колонок и столбцов должно быть во всех матрицах
                List<DenseMatrix> currSurfDataMatricesList = new List<DenseMatrix>();

                List<DataValuesOver3DGrid> currLeadingValueData = new List<DataValuesOver3DGrid>(currGrouping);
                List<IGrouping<double, DataValuesOver3DGrid>> groupingBy2ndVar = new List<IGrouping<double, DataValuesOver3DGrid>>(currLeadingValueData.GroupBy(dvog =>
                {
                    switch (varSliceBy)
                    {
                        case SlicingVariable.x:
                            {
                                return dvog.y;
                                break;
                            }
                        case SlicingVariable.y:
                            {
                                return dvog.z;
                                break;
                            }
                        case SlicingVariable.z:
                            {
                                return dvog.x;
                                break;
                            }
                        default:
                            {
                                return dvog.x;
                                break;
                            }
                    }
                }));

                //columns count
                int columnCount = groupingBy2ndVar.Count;
                //rows count
                int rowCount = groupingBy2ndVar[0].Count();

                DenseMatrix dmLeadingValues = DenseMatrix.Create(rowCount, columnCount, currGrouping.Key);

                DenseMatrix var2Values = DenseMatrix.Create(rowCount, columnCount, (r, c) => groupingBy2ndVar[c].Key);
                currSurfDataMatricesList.Add(var2Values);
                DenseMatrix var3Values = DenseMatrix.Create(rowCount, columnCount, (r, c) =>
                {
                    switch (varSliceBy)
                    {
                        case SlicingVariable.x:
                            {
                                return groupingBy2ndVar[0].ElementAt(r).z;
                                break;
                            }
                        case SlicingVariable.y:
                            {
                                return groupingBy2ndVar[0].ElementAt(r).x;
                                break;
                            }
                        case SlicingVariable.z:
                            {
                                return groupingBy2ndVar[0].ElementAt(r).y;
                                break;
                            }
                        default:
                            {
                                return groupingBy2ndVar[0].ElementAt(r).y;
                                break;
                            }
                    }

                });
                currSurfDataMatricesList.Add(var3Values);

                currSurfDataMatricesList.Add(dmLeadingValues);

                //ILArray<double> tmpLeadingValuesMatrix = (ILArray<double>) dmLeadingValues.ToArray();
                //ILArray<double> tmpvar2ValuesMatrix = (ILArray<double>)var2Values.ToArray();
                //ILArray<double> tmpvar3ValuesMatrix = (ILArray<double>)var3Values.ToArray();

                //ILArray<double> tmparr = tmpLeadingValuesMatrix + tmpvar2ValuesMatrix + tmpvar3ValuesMatrix;

                int valuesCount = lDataList[0].values.Count();
                for (int i = 0; i < valuesCount; i++)
                {
                    DenseMatrix tmpDM = DenseMatrix.Create(rowCount, columnCount, (r, c) => groupingBy2ndVar[c].ElementAt(r).values[i]);
                    currSurfDataMatricesList.Add(tmpDM);
                }

                retSurficesDataMatricesList.Add(currSurfDataMatricesList);
            }

            return retSurficesDataMatricesList;
        }







        /// <summary>
        /// Groups the data intems by "values" values. Using the index of the "values" item to group by
        /// </summary>
        /// <param name="dmRawDataByColumns">The dm raw data by columns.</param>
        /// <param name="controlVariableIndex">Index of the control variable.
        /// With the value of this variable data will be separated to surfaces</param>
        /// <param name="levelsCount">The levels count.
        /// Control variable values scale will be splitted by this count of classes,
        /// XYZ points will be discriminated by this classes.</param>
        /// <returns>List&lt;List&lt;DenseMatrix&gt;&gt;.</returns>
        List<List<DenseVector>> GroupDataByValues(DenseMatrix dmRawDataByColumns, int controlVariableIndex = 0, int levelsCount = 10)
        {
            List<List<DenseVector>> retSurficesDataVectorsList = new List<List<DenseVector>>();

            List<DataValuesOver3DGrid> lDataList = new List<DataValuesOver3DGrid>();
            foreach (Vector<double> vec in dmRawDataByColumns.EnumerateRows())
            {
                lDataList.Add(new DataValuesOver3DGrid(vec.ToArray()));
            }

            List<double> valuesList = lDataList.ConvertAll<double>(dvog => dvog.values[controlVariableIndex]);
            double minVarValue = valuesList.Min();
            double maxVarValue = valuesList.Max();
            List<Tuple<double, double>> classesRangeList = new List<Tuple<double, double>>();
            for (int i = 0; i < levelsCount; i++)
            {
                classesRangeList.Add(
                    new Tuple<double, double>(minVarValue + (double)i * (maxVarValue - minVarValue) / (double)(levelsCount),
                        minVarValue + (double)(i + 1) * (maxVarValue - minVarValue) / (double)(levelsCount)));
            }




            List<IGrouping<int, DataValuesOver3DGrid>> groupingByLeadingVar =
                new List<IGrouping<int, DataValuesOver3DGrid>>(
                    lDataList.GroupBy(dvog =>
                    {
                        return
                            classesRangeList.FindIndex(
                                tpl =>
                                    ((dvog.values[controlVariableIndex] >= tpl.Item1) &&
                                     (dvog.values[controlVariableIndex] <= tpl.Item2)));
                    }));

            foreach (IGrouping<int, DataValuesOver3DGrid> currGrouping in groupingByLeadingVar)
            {
                List<DataValuesOver3DGrid> currLeadingValueData = new List<DataValuesOver3DGrid>(currGrouping);
                DenseVector dvX = new DenseVector(currLeadingValueData.ConvertAll<double>(dvog => dvog.x).ToArray());
                DenseVector dvY = new DenseVector(currLeadingValueData.ConvertAll<double>(dvog => dvog.y).ToArray());
                DenseVector dvZ = new DenseVector(currLeadingValueData.ConvertAll<double>(dvog => dvog.z).ToArray());
                DenseVector dvValues = new DenseVector(currLeadingValueData.ConvertAll<double>(dvog => dvog.values[controlVariableIndex]).ToArray());
                List<DenseVector> currSurfDataMatricesList = new List<DenseVector>();
                currSurfDataMatricesList.Add(dvX);
                currSurfDataMatricesList.Add(dvY);
                currSurfDataMatricesList.Add(dvZ);
                currSurfDataMatricesList.Add(dvValues);

                retSurficesDataVectorsList.Add(currSurfDataMatricesList);
            }

            return retSurficesDataVectorsList;
        }








        List<List<DataValuesOver3DGrid>> Group_DVOG_DataByValues(DenseMatrix dmRawDataByColumns, int controlVariableIndex = 0, int levelsCount = 10)
        {
            List<List<DataValuesOver3DGrid>> retSurficesDataVectorsList = new List<List<DataValuesOver3DGrid>>();

            List<DataValuesOver3DGrid> lDataList = new List<DataValuesOver3DGrid>();
            foreach (Vector<double> vec in dmRawDataByColumns.EnumerateRows())
            {
                lDataList.Add(new DataValuesOver3DGrid(vec.ToArray()));
            }

            List<double> valuesList = lDataList.ConvertAll<double>(dvog => dvog.values[controlVariableIndex]);
            double minVarValue = valuesList.Min();
            double maxVarValue = valuesList.Max();
            List<Tuple<double, double>> classesRangeList = new List<Tuple<double, double>>();
            for (int i = 0; i < levelsCount; i++)
            {
                classesRangeList.Add(
                    new Tuple<double, double>(minVarValue + (double)i * (maxVarValue - minVarValue) / (double)(levelsCount),
                        minVarValue + (double)(i + 1) * (maxVarValue - minVarValue) / (double)(levelsCount)));
            }




            List<IGrouping<int, DataValuesOver3DGrid>> groupingByLeadingVar =
                new List<IGrouping<int, DataValuesOver3DGrid>>(
                    lDataList.GroupBy(dvog =>
                    {
                        return
                            classesRangeList.FindIndex(
                                tpl =>
                                    ((dvog.values[controlVariableIndex] >= tpl.Item1) &&
                                     (dvog.values[controlVariableIndex] <= tpl.Item2)));
                    }));

            foreach (IGrouping<int, DataValuesOver3DGrid> currGrouping in groupingByLeadingVar)
            {
                List<DataValuesOver3DGrid> currLeadingValueData = new List<DataValuesOver3DGrid>(currGrouping);

                retSurficesDataVectorsList.Add(currLeadingValueData);
            }

            return retSurficesDataVectorsList;
        }








        private void btnSaveAsImage_Click(object sender, EventArgs e)
        {
            string filename = strOutputDirectory + DateTime.UtcNow.ToString("s").Replace(":", "-") + ".png";
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = strOutputDirectory;
            dlg.FileName = filename;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dlg.FileName;
            }


            ILScene currScene = ilPanel1.Scene;
            var drv = new ILGDIDriver(ilPanel1.Width, ilPanel1.Height, currScene);
            drv.Render();
            drv.BackBuffer.Bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
        }
    }






    struct DataValuesOver3DGrid
    {
        public double x;
        public double y;
        public double z;
        public List<double> values;


        public DataValuesOver3DGrid(double xVal, double yVal, double zVal, double[] inValues)
        {
            x = xVal;
            y = yVal;
            z = zVal;
            values = new List<double>(inValues);
        }


        public DataValuesOver3DGrid(double[] dataString)
        {
            List<double> tmplist = new List<double>(dataString);
            x = tmplist[0];
            y = tmplist[1];
            z = tmplist[2];
            values = tmplist.GetRange(3, tmplist.Count - 3);
        }
    }




    struct DataValuesOver2DGrid
    {
        public double x;
        public double y;
        public List<double> values;


        public DataValuesOver2DGrid(double xVal, double yVal, double[] inValues)
        {
            x = xVal;
            y = yVal;
            values = new List<double>(inValues);
        }


        public DataValuesOver2DGrid(double[] dataString)
        {
            List<double> tmplist = new List<double>(dataString);
            x = tmplist[0];
            y = tmplist[1];
            values = tmplist.GetRange(2, tmplist.Count - 2);
        }
    }
}
