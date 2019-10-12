using System;
using System.Collections.Generic;
using System.Text;

namespace Genethic_Algorithm
{
    class KnapsackProblem : Evaluate
    {
        Loader testData;
        double[] values;
        double[] weights;
        int itemsNumber;
        double knapsackCapacity;

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
