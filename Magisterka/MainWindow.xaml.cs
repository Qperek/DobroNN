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
        private int currentLayersCount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnSubmitLayers_Click(object sender, RoutedEventArgs e)
        {
            int positionToInsert = LeftStackPanel.Children.IndexOf(LayersStackPanel);
            ClearLayers();

            if (!Int32.TryParse(tboxLayers.Text, out currentLayersCount))
                throw new TextBoxParseException("Cannot parse layers textbox");            

            for (int i = 0; i < currentLayersCount; i++)
            {
                StackPanel tmpStackPanel = new StackPanel(){
                    Orientation = Orientation.Horizontal,
                    Name = "LayerStackPanel" + (i + 1)
                };

                FillStackPanel(i, tmpStackPanel);
                LeftStackPanel.Children.Insert(positionToInsert + 1, tmpStackPanel);
                positionToInsert++;
            }
        }

        private void ClearLayers()
        {
            int iterator = currentLayersCount;
            UIElement el = null;

            for(int i = LeftStackPanel.Children.Count - 1; i >= 0; i--)
            {
                if(LeftStackPanel.Children[i].GetType() == typeof(StackPanel))
                {
                    if((LeftStackPanel.Children[i] as StackPanel).Name == "LayerStackPanel" + iterator)
                    {
                        el = LeftStackPanel.Children[i];
                        LeftStackPanel.Children.Remove(el);
                        iterator--;
                    }
                }
            }
        }

        private static void FillStackPanel(int i, StackPanel tmpStackPanel)
        {
            tmpStackPanel.Children.Add(new Label
            {
                Name = "lblLayer" + (i + 1),
                Height = 27,
                Content = "Neurons in layer " + (i + 1),
                Margin = new Thickness(3, 1, 1, 3)
            });
            tmpStackPanel.Children.Add(new TextBox
            {
                Name = "tboxLayer" + (i + 1),
                Height = 27,
                MinWidth = 70,
                Margin = new Thickness(3, 1, 1, 3)
            });
        }

        private void btnStartLearning_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllFieldsCorrect())
            {
                MessageBox.Show("Fill all the fields!");
                return;
            }

            CreateNetworkManager();
          
            nnManager.SetTrainingPaths(ResourceStorage.trainDigitsPath, ResourceStorage.trainLabelsPath);
            SetLearningRate();
            StartTraining();
          /*  nnManager.SetLearingRate(0.1);

            nnManager.TrainNetwork(epochsCount, true);
            double correctAnswersPercentage = nnManager.TestNetwork(true);
            MessageBox.Show(correctAnswersPercentage.ToString());*/

        }

        private async void StartTraining()
        {
            int epochsCount = Int32.Parse(tboxEpochs.Text);
            bool isNormalized = (bool)chboxNormalized.IsChecked;

            Progress<int> progress = new Progress<int>();
            progress.ProgressChanged += ReportProgress;

            double networkError = await Task.Run(() => nnManager.TrainNetwork(epochsCount, progress, isNormalized));
            lblCurrentError.Content = "Network error: " + networkError.ToString("N4");
            MessageBox.Show("Training complete");
        }

        private void ReportProgress(object sender, int e)
        {
            LearningProgressBar.Value = e;
            prgBarPercentage.Content = e.ToString() +"%";
        }

        private void CreateNetworkManager()
        {
            ActivationFunctionParser parser = new ActivationFunctionParser();
            if(!String.IsNullOrWhiteSpace(tboxAlpha.Text))
                parser.SetAlpha(Double.Parse(tboxAlpha.Text));
            
            IActivationFunction activationFunction = parser.StringToActivationFunction(cmbActivationFunction.Text);
            int[] layers = new int[currentLayersCount + 1];

            GetLayersValues(layers);
            layers[currentLayersCount] = 10;
            nnManager = new NeuralNetworkManager(activationFunction, 784, layers);
        }

        private void SetLearningRate()
        {
            if(!String.IsNullOrWhiteSpace(tboxLearningRate.Text))
            { 
                double learningRate = Double.Parse(tboxLearningRate.Text);
                nnManager.SetLearingRate(learningRate);
            }
        }
        private void GetLayersValues(int[] layers)
        {
            int iterator = currentLayersCount;
            StackPanel el = null;

            for (int i = LeftStackPanel.Children.Count - 1; i >= 0; i--)
            {
                if (LeftStackPanel.Children[i].GetType() == typeof(StackPanel))
                {
                    if ((LeftStackPanel.Children[i] as StackPanel).Name == "LayerStackPanel" + iterator)
                    {
                        el = (LeftStackPanel.Children[i] as StackPanel);
                        layers[iterator - 1] = Int32.Parse((el.Children[1] as TextBox).Text);
                        iterator--;
                    }
                }
            }            
        }

        private bool AreAllFieldsCorrect()
        {
            if (!AreTextBoxesFilled())
                return false;
            if (currentLayersCount > 0)
            {
                if (!AreLayersFilled())
                    return false;
            }
            return true;
        }
        private bool AreTextBoxesFilled()
        {
            foreach(var children in LeftStackPanel.Children)
            {
                if(children.GetType() == typeof(TextBox))
                {
                    if (String.IsNullOrWhiteSpace((children as TextBox).Text))
                        return false;
                }                
            }
            return true;
        }

        private bool AreLayersFilled()
        {
            int iterator = currentLayersCount;
            StackPanel el = null;
            TextBox tbox = null;

            for (int i = LeftStackPanel.Children.Count - 1; i >= 0; i--)
            {
                if (LeftStackPanel.Children[i].GetType() == typeof(StackPanel))
                {
                    if ((LeftStackPanel.Children[i] as StackPanel).Name == "LayerStackPanel" + iterator)
                    {
                        el = (LeftStackPanel.Children[i] as StackPanel);
                        tbox = (el.Children[1] as TextBox);
                        iterator--;
                        if (String.IsNullOrWhiteSpace(tbox.Text))
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
