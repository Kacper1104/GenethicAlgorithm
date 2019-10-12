using System;
using System.Collections.Generic;
using System.Text;

namespace Genethic_Algorithm
{
    internal class KnapsackProblem : IProblem
    {
        Loader testData;
        double[] values;
        double[] weights;
        int itemsNumber;
        double knapsackCapacity;

        public KnapsackProblem()
        {
        }

        public KnapsackProblem(Loader testData)
        {
            this.testData = testData;
            values = testData.values;
            weights = testData.weights;
            itemsNumber = testData.itemsNumber;
            knapsackCapacity = testData.kapsackCapacity;
        }

        public double Evaluate(int[] itemsTaken)
        {
            throw new NotImplementedException();
        }
    }
}
