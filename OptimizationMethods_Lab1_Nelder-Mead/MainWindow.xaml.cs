using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using Nelder_Mead;
using Vector;

namespace OptimizationMethods_Lab1_Nelder_Mead
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NelderMead NM;
        IIterationNM IterationNMRu;
        IIterationNM IterationNMEng;
        double alfa = 1;
        double betta = 0.5;
        double gamma = 2;
        int maxSteps = 100;
        double? dispersion = null;
        CalculationFunction calculation;
        List<VectorM> simplex;

        SomeFunction function;

        DoPrintConsole doPrint = new DoPrintConsole();

        VectorM resultationVector;
        public MainWindow()
        {
            InitializeComponent();

            IterationNMRu = new RunWikiRus();
            IterationNMEng = new RunWikiEng();

            simplex = NelderMead.CreatureSimplex(2);

            function_TextBox.Text = "(1 - x)^2 + 100(y - x^2)^2";

            CustomSimplex_TextBox.Text = "0 0\n2 2";

            Auto_RadioButton.IsChecked = true;

        }

        private void StartAlgorithm(IIterationNM iteration)
        {
            try
            {
                ApplyFunction();
                ApplySettingsAlgorith();

                if (Auto_RadioButton.IsChecked == true)
                    ApplyAuto();
                else if (Custom_RadioButton.IsChecked == true)
                    ApplyCustom();
                else
                    ApplyGenerate();
            }
            catch (Exception e)
            {
                ShowExeptionWindow("Error in algorithm settings:\n" + e.Message);
            }

            try
            {
                doPrint.ClearLog();

                NM = new NelderMead(alfa, betta, gamma, maxSteps, dispersion);
                NM.Fit(calculation, simplex);
                resultationVector = NM.Run(iteration, new List<IDoSomefing> { doPrint });

                ChangeWindow();
            }
            catch (Exception e)
            {
                string exeptionString;

                exeptionString = "An error occurred when the algorithm was running:\n";
                exeptionString += e.Message;
                exeptionString += "\nPerhaps the wrong dimension of the vectors for the function";

                ShowExeptionWindow(exeptionString);
            }
        }

        private void ChangeWindow()
        {
            Log_TextBlock.Text = doPrint.Log;
            Answer_TextBlock.Text = CreateResStr();
            CreateImage();
        }
        private string CreateResStr()
        {
            string ansResStr = "Algorithm Settings:";
            ansResStr += "\nalfa = " + alfa.ToString();
            ansResStr += "\nbetta = " + betta.ToString();
            ansResStr += "\ngamma = " + gamma.ToString();
            ansResStr += "\nMaxSteps = " + maxSteps.ToString();
            ansResStr += "\nDispersion = " + dispersion.ToString();

            ansResStr += "\n\nStartSimplex:";
            for (int i = 0; i < NM.StartSimplex.Count; i++)
                ansResStr += $"\nV{i} = " + NM.StartSimplex[i];

            ansResStr += "\n\nSteps taken:";
            ansResStr += "\n" + NM.Steps.ToString();

            ansResStr += "\n\nVariance of the simplex:";
            ansResStr += "\n" + NM.Dispersion.ToString();

            ansResStr += "\n\nResultation vector: ";
            ansResStr += "\n" + resultationVector;

            ansResStr += "\n\nThe value of the criterion:";
            ansResStr += "\n" + calculation(resultationVector).ToString();

            return ansResStr;
        }

        private void ShowExeptionWindow(string text)
        {
            MessageBox.Show("Error. " + text);
        }

        private void CreateImage()
        {
            WpfPlot1.Plot.Clear();

            if (function.Dimention == 2)
            {
                int sizeMap = 5;
                double step = 0.5;

                WpfPlot1.Plot.Title("Function: " + function.Name);

                double[,] data2D = new double[Convert.ToInt32(2 * sizeMap / step), Convert.ToInt32(2 * sizeMap / step)];

                int xI = 0;
                for (double x = -sizeMap; x < sizeMap; x += step, xI++)
                {
                    int yI = 0;
                    for (double y = -sizeMap; y < sizeMap; y += step, yI++)
                        data2D[xI, yI] = calculation(new VectorM(new double[] { x, y }));
                }

                var hm = WpfPlot1.Plot.AddHeatmap(data2D);
                hm.XMin = -sizeMap;
                hm.XMax = sizeMap;
                hm.YMin = -sizeMap;
                hm.YMax = sizeMap;
                hm.Smooth = true;

                WpfPlot1.Plot.AddPoint(resultationVector[0], resultationVector[1]);
            }
            else
                WpfPlot1.Plot.Title("The graph can only be drawn for a two-dimensional function");

            WpfPlot1.Refresh();
        }

        private void ApplyFunction()
        {
            try
            {
                function = new SomeFunction(function_TextBox.Text, function_TextBox.Text);
                calculation = function.GetValue;
            }
            catch
            {
                throw new Exception("Something is wrong with the function");
            }
        }

        private void ApplySettingsAlgorith()
        {
            try
            {
                alfa = Convert.ToDouble(alfa_TextBox.Text);
                betta = Convert.ToDouble(betta_TextBox.Text);
                gamma = Convert.ToDouble(gamma_TextBox.Text);
                maxSteps = Convert.ToInt32(maxSteps_TextBox.Text);

                if (dispersion_TextBox.Text != "null" && dispersion_TextBox.Text != "")
                    dispersion = Convert.ToDouble(dispersion_TextBox.Text);
                else
                    dispersion = null;
            }
            catch
            {
                throw new Exception("Incorrect alpha, beta, gamma, max steps or dispertion");
            }
        }

        private void StartAlgorithmRu_Button_Click(object sender, RoutedEventArgs e)
        {
            StartAlgorithm(IterationNMRu);
        }

        private void StartAlgorithmEng_Button_Click(object sender, RoutedEventArgs e)
        {
            StartAlgorithm(IterationNMEng);
        }

        private void Auto_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Auto_TabItem.IsSelected = true;
        }

        private void GenerateSimplex_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Generate_TabItem.IsSelected = true;
        }

        private void Custom_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Custom_TabItem.IsSelected = true;
        }

        private void ApplyAuto()
        {
            try
            {
                simplex = NelderMead.CreatureSimplex(function.Dimention);
            }
            catch
            {
                throw new Exception("Can't create simplex");
            }
        }
        private void ApplyGenerate()
        {
            try
            {
                VectorM startVector = new VectorM(GenerateStartVector_TextBox.Text.Split('\n')[0].Split(' ').Select(Convert.ToDouble).ToArray());
                int dim = startVector.Size;
                int sizeSimplex = Convert.ToInt32(GenerateSizeSimplex_TextBox.Text);

                simplex = NelderMead.CreatureSimplex(dim, sizeSimplex, startVector);
            }
            catch
            {
                throw new Exception("Incorrect initial vector, dimension, or simplex size");
            }
        }

        private void ApplyCustom()
        {
            try
            {
                List<VectorM> simplex = CustomSimplex_TextBox.Text.Split('\n').Select(s => new VectorM(s.Split(' ').Select(Convert.ToDouble).ToArray())).ToList();

                this.simplex = simplex;
            }
            catch
            {
                throw new Exception("Incorrect simplex");
            }
        }
    }
}
