using AForge.Neuro;
using AForge.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Magisterka
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(), 784, 15, 10);
            string path = "C:\\Users\\Maciek\\documents\\visual studio 2017\\Projects\\Magisterka\\Magisterka\\Data\\train-images.idx3-ubyte";

            DigitParser parser = new DigitParser(path);
            parser.ReadDigitsHeader();

            MessageBox.Show("Magic number: "+parser.GetMagicNumber().ToString()  + "\n" +
                            "Samples count: "+parser.GetSampleCount().ToString() + "\n" +
                            "Sample rows: " + parser.GetHeight().ToString()      + "\n" +
                            "Sample collumns: " + parser.GetWidth().ToString()   + "\n" +
                            "Offset: " + parser.GetOffSet().ToString());
                                    
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            byte[,] digit;
            byte[] oneDimensionDigit;
            double[] input;
            double[] output;

            double err = 9999.0d;
            double[] result;
            long iterations = 0;

            while(err > 2.0d)
            {
                digit = parser.ReadNextDigit();
                oneDimensionDigit = parser.MatrixToArray(digit);
                input = parser.ByteArrayToDoubleArray(oneDimensionDigit);
                output = new double[] { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 };
                err = teacher.Run(input, output);

                if (iterations % 150000 == 0)
                    MessageBox.Show(err.ToString());
                iterations++;
            }
            MessageBox.Show(err.ToString());
        }
    }
}
