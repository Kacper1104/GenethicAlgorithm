using System;
using System.Collections.Generic;
using System.Text;

namespace Genethic_Algorithm
{
    internal class KNPProblem : Problem
    {
        Loader testData;
        double[] values;
        double[] weights;
        int itemsNumber;
        double knapsackCapacity;

        public KNPProblem()
        {
        }

        public KNPProblem(Loader testData)
        {
            this.testData = testData;
            values = testData.values;
            weights = testData.weights;
            itemsNumber = testData.itemsNumber;
            knapsackCapacity = testData.kapsackCapacity;
        }

        internal override double Evaluate(int[] itemsTaken)
        {
            throw new NotImplementedException();
        }
    }
}
