using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Genethic_Algorithm
{
    class KNPSpecimen
    {
        //variables
        double score;
        sbyte[] genotype;
        //params
        Loader testdata;
        //constructors
        public KNPSpecimen()
        {
        }
        public KNPSpecimen(sbyte[] genotype, Loader testdata)
        {
            this.testdata = testdata;
            this.genotype = genotype;
        }
        //getters and setters
        public double Score { get => score; set => score = value; }
        public sbyte[] Genotype { get => genotype; set => genotype = value; }
        //methods
        internal KNPSpecimen[] Crossover(KNPSpecimen partner, double probX)
        {
            sbyte[] child1Genotype = new sbyte[testdata.ItemCount*testdata.KnapsackCount];
            sbyte[] child2Genotype = new sbyte[testdata.ItemCount * testdata.KnapsackCount];
            Random rn = new Random();
            int cutover = rn.Next(testdata.ItemCount*testdata.KnapsackCount);
            for(int i = 0; i < testdata.ItemCount*testdata.KnapsackCount; i++)
            {
                if (i < cutover)
                {
                    child1Genotype[i] = this.genotype[i];
                    child2Genotype[i] = partner.Genotype[i];
                }
                else
                {
                    child1Genotype[i] = partner.Genotype[i];
                    child2Genotype[i] = this.genotype[i];
                }
            }
            child1Genotype = FixGenotype(child1Genotype, cutover);
            child2Genotype = FixGenotype(child2Genotype, cutover);
            return new KNPSpecimen[] { new KNPSpecimen(child1Genotype, testdata), new KNPSpecimen(child2Genotype, testdata) };
        }//End of Crossover()
        private sbyte[] FixGenotype(sbyte[] genotype, int startIdx)
        {
            sbyte[] fixedGenotype = (sbyte[])genotype.Clone();
            for(int i = startIdx / testdata.ItemCount; i < testdata.KnapsackCount; i++)//for each knapsack from cutover to n
            {
                for(int j = 0; j < testdata.ItemCount; j++)//for each item
                {
                    for(int k = 0; k < startIdx / testdata.ItemCount; k++)//for each knapsack from 0 to cutover
                    {
                        if(genotype[i*testdata.ItemCount+j] == 1 && genotype[k*testdata.ItemCount+j] == 1)
                        {
                            fixedGenotype[i * testdata.ItemCount + j] = 0;
                        }
                    }
                }
            }
            return fixedGenotype;
        }//End of FixGenotype();
        internal KNPSpecimen Mutate(double probM)
        {
            Random random = new Random();
            sbyte[] newGenotype = (sbyte[])genotype.Clone();
            for(int i = 0; i < testdata.KnapsackCount; i++)//for each knapsack
            {
                for(int j = 0; j < testdata.ItemCount; j++)//for each item
                {
                    if (random.NextDouble() <= probM)
                    {
                        if(testdata.MainKnapsackIdx == i || newGenotype[testdata.MainKnapsackIdx * testdata.ItemCount + j] == 0)
                        {
                            if (newGenotype[i*testdata.ItemCount+j] == 0)
                            {
                                newGenotype[i* testdata.ItemCount + j] = 1;
                            }
                            else
                            {
                                newGenotype[i* testdata.ItemCount + j] = 0;
                            }
                        }
                    }
                }
            }
            return new KNPSpecimen(newGenotype, testdata);
        }//End of Mutate()
        internal double Evaluate(KNPProblem problem){
            if(score == null) 
                score = problem.Evaluate(genotype); 
            return score;
        }//End of Evaluate();

    }
}
