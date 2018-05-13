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

        public MainWindow()
        {
            InitializeComponent();

            nnManager = new NeuralNetworkManager(new SigmoidFunction(1), 784, 30, 10);
            
            TestNeuralNetworkManager(80);
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
    }
}
