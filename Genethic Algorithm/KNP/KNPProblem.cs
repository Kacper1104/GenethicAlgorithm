using System;
using System.Collections.Generic;
using System.Text;

namespace Genethic_Algorithm
{
    internal class KNPProblem
    {
        //params
        Loader testdata;
        int penalty;
        //constructors
        public KNPProblem()
        {
        }
        public KNPProblem(Loader testcata, int penalty)
        {
            this.testdata = testcata;
        }
        //methods
        internal double Evaluate(sbyte[] genotype)
        {
            //wynik = suma wartosci
            //jesli suma wag jest wieksza niz pojemnosc, to kara
            double value = 0;
            double weight = 0;
            bool tooHeavy = false;
            for(int i = 0; i < testdata.KnapsackCount; i++)//for each knapsack
            {
                for(int j = 0; j < testdata.ItemCount; j++)//for each item
                {
                    if(genotype[i*testdata.ItemCount + j] == 1)
                    {
                        value += testdata.ItemValues[j];
                        weight += testdata.ItemWeights[j];
                    }
                }
            }
            if (weight > testdata.KnapsackCapacitiesSum)
            {
                value -= penalty;
            }
            return value;
        }
    }
}
