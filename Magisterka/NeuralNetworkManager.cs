using AForge.Neuro;
using AForge.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magisterka
{
    class NeuralNetworkManager
    {
        public ActivationNetwork network;
        ResilientBackpropagationLearning teacher;
        DigitParser trainDigitParser;
        LabelParser trainLabelParser;

        private bool isInitialized = false;
        private string errorFilePath = ResourceStorage.errorFilePath;
        private double[][] input;
        private double[][] output;
        
        public NeuralNetworkManager(IActivationFunction function, int inputsCount, params int[] neuronsCount)
        {
            network = new ActivationNetwork(function, inputsCount, neuronsCount);
            teacher = new ResilientBackpropagationLearning(network);
        }
        public void SetTrainingPaths(string inputPath, string outputPath)
        {
            trainDigitParser = new DigitParser(inputPath);
            trainLabelParser = new LabelParser(outputPath);
            isInitialized = true;
        }

        public void SetLearingRate(double value)
        {
            if (!isInitialized)
                return;

            if (value >= 0 && value <= 1)
                teacher.LearningRate = value;
            else
                throw new InRangeExeption("Learing rate value out of range");
        }

        public double TrainNetwork(int epochsCount, IProgress<int> progress ,bool isInputNormalized = false)
        {
            if (!isInitialized) 
                return -1;

            input = CreateEpochInput();
            output = CreateEpochOutput();

            if(isInputNormalized)
                NormalizeInput(input);

            double err = -1;
            PrepareFile();

            for(int i = 0; i < epochsCount; i++)
            {
                err = teacher.RunEpoch(input, output);
                SaveErrorToFile(i, err);
                progress.Report(((i + 1) * 100) / epochsCount);
            }
            return err;
        }

        private double[][] CutInput()
        {
            double[][] smallInput = new double[1000][];
            for(int i = 0; i < 1000; i++)
            {
                smallInput[i] = input[i];
            }
            return smallInput;
        }

        private double[][] CutOutput()
        {
            double[][] smallOutput = new double[1000][];
            for (int i = 0; i < 1000; i++)
            {
                smallOutput[i] = output[i];
            }
            return smallOutput;
        }

        public void TrainNetworkIterations(int epochsCount)
        {
            input = CreateEpochInput();
            output = CreateEpochOutput();
            double[] error = new double[60000];
            PrepareFile();
            int[] seq = Enumerable.Range(0, 60000).ToArray();
            Random rnd = new Random();
            int[] randomSequence = seq.OrderBy(x => rnd.Next()).ToArray();
            int epoch = 0;

            for (int i = 0; i < epochsCount * 60000; i++)
            {
                error[i % 60000] = teacher.Run(input[randomSequence[i%60000]], output[randomSequence[i%60000]]);
                if(i%60000 == 0)
                {
                    SaveErrorToFile(epoch, error.Sum() / 60000);
                    epoch++;
                }
            }

        }

        public double TestNetwork(bool isInputNormalized = false)
        {
            if (!isInitialized)
                return -1;

            DigitParser testDigitParser = new DigitParser(ResourceStorage.testDigitsPath);
            LabelParser testLabelParser = new LabelParser(ResourceStorage.testLabelsPath);

            double[] singleInput;
            double[] singleOutput;
            double[] networkOutput;
            int corectOutputs = 0;
            int totalOutputs = testLabelParser.GetSampleCount();

            for (int i = 0; i < testDigitParser.GetSampleCount(); i++)
            {
                singleInput = testDigitParser.GetNextInput();
                singleOutput = testLabelParser.GetNextOutput();

                if(isInputNormalized)
                    NormalizeSingleInput(singleInput);

                networkOutput = network.Compute(singleInput);
                if (singleOutput.SequenceEqual(ConvertToSingleOutput(networkOutput)))
                    corectOutputs++;
            }
            return (corectOutputs / (double)totalOutputs) * 100.0d;
        }

        private double[][] CreateEpochInput()
        {
            double[][] result = new double[trainDigitParser.samplesCount][];
            for(int i = 0; i < trainDigitParser.samplesCount; i++)
            {                                
                result[i] = trainDigitParser.GetNextInput();                
            }
            return result;
        }

        private double[][] CreateEpochOutput()
        {
            double[][] result = new double[trainLabelParser.samplesCount][];
            for (int i = 0; i < trainLabelParser.samplesCount; i++)
            {
                result[i] = trainLabelParser.GetNextOutput();
            }
            return result;
        }
        public void SetErrorFile(string path)
        {
            errorFilePath = path;
        }
        private void SaveErrorToFile(int epoch, double err)
        {
            string appendText = epoch.ToString() + "       " + err.ToString("N4") + Environment.NewLine;
            File.AppendAllText(errorFilePath, appendText);
        }
        private void PrepareFile()
        {
            if (File.Exists(errorFilePath))
                File.Delete(errorFilePath);
            else
            {
                string headerText = "Epochs   Errors" + Environment.NewLine;
                File.WriteAllText(errorFilePath,headerText);
            }
        }

        private double[] ConvertToSingleOutput(double[] networkOutput)
        {
            double maxValue = networkOutput.Max();
            int indexOfMaxValue = networkOutput.ToList().IndexOf(maxValue);

            double[] result = new double[networkOutput.Length];
            result[indexOfMaxValue] = 1;
            return result;
        }

        private void NormalizeInput(double[][] arr)
        {
            for(int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr[1].Length; j++)
                    arr[i][j] = (arr[i][j] / 255.0d);
            }
        }

        private void NormalizeOutput(double[][] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr[1].Length; j++)
                {
                    if (arr[i][j] == 0)
                        arr[i][j] = -1;                    
                }
            }
        }

        private void NormalizeSingleInput(double[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (arr[i] / 255.0d);
            }
        }
    }
}