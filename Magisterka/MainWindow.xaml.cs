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
            string path = ResourceStorage.testDigitsPath;

            DigitParser parser = new DigitParser(path);
            parser.ReadHeader();

            MessageBox.Show("Magic number: "+parser.GetMagicNumber().ToString()  + "\n" +
                            "Samples count: "+parser.GetSampleCount().ToString() + "\n" +
                            "Sample rows: " + parser.GetHeight().ToString()      + "\n" +
                            "Sample collumns: " + parser.GetWidth().ToString()   + "\n" +
                            "Offset: " + parser.GetOffSet().ToString());
                                    
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            byte[] digit;
            double[] input;
            double[] output;

            double err = 9999.0d;
            double[] result;
            long iterations = 0;
            

            while(iterations < 30)
            {
                digit = parser.ReadNext();
                input = parser.ByteArrayToDoubleArray(digit);
                output = new double[] { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 };
                err = teacher.Run(input, output);

                if (iterations % 150000 == 0)
                    MessageBox.Show(err.ToString());
                iterations++;
            }
            MessageBox.Show(err.ToString());

            ReadLabelTest();
        }

        public void ReadLabelTest()
        {
            string path = ResourceStorage.testLabelsPath;
            LabelParser lb = new LabelParser(path);
            
            lb.ReadHeader();
            MessageBox.Show("Magic number: " + lb.GetMagicNumber().ToString() + "\n" +
                "Samples count: " + lb.GetSampleCount().ToString() + "\n" +
                "Offset: " + lb.GetOffSet().ToString());
            double[] output = lb.GetOutputFromByte(lb.GetLabel(lb.ReadNext()));
        }

        public void Learn(int iterations)
        {
            DigitParser digitParser = new DigitParser(ResourceStorage.testDigitsPath);
            LabelParser labelParser = new LabelParser(ResourceStorage.testLabelsPath);

            digitParser.ReadHeader();
            labelParser.ReadHeader();
            
            double[] input;
            double[] output;

            int epoch = digitParser.samplesCount;
            for(int i = 0; i < 4 * epoch; i++)
            {
                input = digitParser.ByteArrayToDoubleArray(digitParser.ReadNext());
                output = labelParser.GetOutputFromByte(labelParser.GetLabel(labelParser.ReadNext()));
            }
        }
    }
}
