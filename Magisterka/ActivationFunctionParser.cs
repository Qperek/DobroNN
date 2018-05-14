using AForge.Neuro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magisterka
{
    class ActivationFunctionParser
    {
        double _alpha = 1;

        public void SetAlpha(double alpha)
        {
            _alpha = alpha;
        }

        public IActivationFunction StringToActivationFunction(string str)
        {
            switch(str)
            {
                case "BipolarSigmoid":
                    return new BipolarSigmoidFunction(_alpha);
                case "Sigmoid":
                    return new SigmoidFunction(_alpha);
                case "Threshold":
                    return new ThresholdFunction();
                default:
                    return new SigmoidFunction(_alpha);
            }
        }
    }
}
