using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Genethic_Algorithm
{
    class KNPSpecimen : Specimen
    {
        double score;
        sbyte[] genotype;
        int[] knapsacksLengths;
        double[] knapsacksWeights;
        int biggestKnapsackStart;



        public KNPSpecimen()
        {
        }

        public KNPSpecimen(sbyte[] genotype, int[] knapsacksLengths, double[] knapsacksWeights)
        {
            this.genotype = (sbyte[])genotype.Clone();
            this.knapsacksLengths = (int[])knapsacksLengths.Clone();
            this.knapsacksWeights = (double[])knapsacksWeights.Clone();
            
            biggestKnapsackStart = 0;
            int maxValue = knapsacksLengths.Max();
            int maxIndex = knapsacksLengths.ToList().IndexOf(maxValue);
            for(int i = 0; i < maxIndex; i++)
            {
                biggestKnapsackStart += knapsacksLengths[i];
            }
        }

        public double Score { get => score; set => score = value; }
        public sbyte[] Genotype { get => genotype; set => genotype = value; }
        public int[] KnapsackLengths { get => knapsacksLengths; set => knapsacksLengths = value; }
        public double[] KnapsackWeights { get => knapsacksWeights; set => knapsacksWeights = value; }

        public KNPSpecimen[] Crossover(KNPSpecimen partner)
        {
            throw new NotImplementedException();
        }

        internal override Specimen Mutate(double probM)
        {
            Random random = new Random();
            for(int i = 0; i < genotype.Length; i++)
            {
                if (random.NextDouble() <= probM)
                {
                    if()
                    if (genotype[i] == 0) genotype[i] = 1;
                    else genotype[i] = 0;
                    
                }
            }
        }

        public double Evaluate(Problem problem){
            if(score == null) 
                score = problem.Evaluate(genotype); 
            return score;
            }
    }
}
