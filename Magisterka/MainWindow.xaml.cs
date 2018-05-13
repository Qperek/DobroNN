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
        private NeuralNetworkManager nnManager;
        private List<TextBox> layersTextBoxes;

        public MainWindow()
        {
            InitializeComponent();

            nnManager = new NeuralNetworkManager(new SigmoidFunction(1), 784, 30, 10);
            
            //TestNeuralNetworkManager(80);
        }

        public void TestNeuralNetworkManager(int epochsCount)
        {
            nnManager.SetTrainingPaths(ResourceStorage.trainDigitsPath, ResourceStorage.trainLabelsPath);
            nnManager.SetLearingRate(0.1);

            nnManager.TrainNetwork(epochsCount, true);
            double correctAnswersPercentage = nnManager.TestNetwork(true);
            MessageBox.Show(correctAnswersPercentage.ToString());         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnSubmitLayers_Click(object sender, RoutedEventArgs e)
        {
            int positionToInsert = LeftStackPanel.Children.IndexOf(LayersStackPanel);
            int layersCount;

            if (!Int32.TryParse(tboxLayers.Text, out layersCount))
                throw new TextBoxParseException("Cannot parse layers textbox");
            layersTextBoxes = new List<TextBox>();

            for (int i = 0; i < layersCount; i++)
            {
                StackPanel tmpStackPanel = new StackPanel();
                tmpStackPanel.Orientation = Orientation.Horizontal;

                tmpStackPanel.Children.Add(new TextBox
                {
                    Name = "layer" + (i + 1),
                    Height = 20,
                    Margin = new Thickness(3, 1, 1, 3)
                });
                layersTextBoxes.Add(new TextBox
                {
                    Name = "layer" + (i + 1),
                    Height = 20
                });
                LeftStackPanel.Children.Insert(positionToInsert + 1, layersTextBoxes[i]);
            }

        }
    }
}
