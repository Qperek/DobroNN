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

            /*   while(iterations < 30)
               {
                   digit = parser.ReadNext();
                   input = parser.ByteArrayToDoubleArray(digit);
                   output = new double[] { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 };
                   err = teacher.Run(input, output);

                   if (iterations % 150000 == 0)
                       MessageBox.Show(err.ToString());
                   iterations++;
               }
               MessageBox.Show(err.ToString());*/

            Learn(6);
        }

        public void ReadLabelTest()
        {
            string path = ResourceStorage.testLabelsPath;
            LabelParser lb = new LabelParser(path);
            
            double[] output = lb.GetOutputFromByte(lb.GetLabel(lb.ReadNext()));
        }

        public void ReadDigitTest()
        {
            string path = ResourceStorage.testDigitsPath;

            DigitParser parser = new DigitParser(path);
            double[] result = parser.GetNextInput();
        }

        public void Learn(int epochsCount)
        {
            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(), 784, 15, 10);
            BackPropagationLearning teacher = new BackPropagationLearning(network);

            DigitParser digitParser = new DigitParser(ResourceStorage.testDigitsPath);
            LabelParser labelParser = new LabelParser(ResourceStorage.testLabelsPath);

            MessageBox.Show("Learning rate: " + teacher.LearningRate + "\n" +
                            "Momentum: " + teacher.Momentum + "\n");
            
            double[] input;
            double[] output;
            double err;
            double prevErr = -1;

            int epoch = digitParser.samplesCount;

            for(int i = 0; i < epochsCount * epoch; i++)
            {
                input = digitParser.GetNextInput();
                output = labelParser.GetNextOutput();
                err = teacher.Run(input, output);
                if (err == prevErr)
                    break;
                prevErr = err;
                if (i % epoch == 0)
                {
                    MessageBox.Show(err.ToString());
                    ShowWeights(network.Layers);
                }
            }
        }

        public void ShowWeights(Layer[] layers)
        {
            Neuron[] neurons = layers[1].Neurons;
            string result = "";

            for(int i = 0; i < neurons.Count(); i++)
            {
                double[] weights = neurons[i].Weights;
                for(int j = 0; j < weights.Count(); j++)
                {
                    result += weights[j].ToString("N3") + " ";
                }
                result += "\n";
            }
            MessageBox.Show(result);
        }
    }
}
