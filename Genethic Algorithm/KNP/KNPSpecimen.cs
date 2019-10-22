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
        public KNPSpecimen() { }
        public KNPSpecimen(sbyte[] genotype, Loader testdata)
        {
            this.testdata = testdata;
            this.genotype = genotype;
            this.score = double.MinValue;
        }
        //getters and setters
        public double Score { get => score; set => score = value; }
        public sbyte[] Genotype { get => genotype; set => genotype = value; }
        //methods
        internal KNPSpecimen[] Crossover(KNPSpecimen partner, double probX)
        {
            Random rn = new Random();
            if (rn.NextDouble() <= probX)
            {
                sbyte[] child1Genotype = new sbyte[testdata.ItemCount * testdata.KnapsackCount];
                sbyte[] child2Genotype = new sbyte[testdata.ItemCount * testdata.KnapsackCount];
                int cutover = rn.Next(testdata.ItemCount * testdata.KnapsackCount);
                for (int i = 0; i < testdata.ItemCount * testdata.KnapsackCount; i++)
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
                child1Genotype = FixGenotype(child1Genotype);
                child2Genotype = FixGenotype(child2Genotype);
                return new KNPSpecimen[] { new KNPSpecimen(child1Genotype, testdata), new KNPSpecimen(child2Genotype, testdata) };
            }
            return new KNPSpecimen[] { new KNPSpecimen((sbyte[])this.genotype.Clone(), testdata), new KNPSpecimen((sbyte[])partner.genotype.Clone(), testdata) };
        }//End of Crossover()
        public sbyte[] FixGenotype(sbyte[] genotypeToCheck)
        {
            bool itemFound;
            sbyte[] fixedGenotype = (sbyte[])genotypeToCheck.Clone();
                for (int i = 0; i < testdata.ItemCount; i++)//for each item
                {
                    itemFound = false;
                    for (int j = 0; j < testdata.KnapsackCount; j++)//for each knapsack
                    {
                        if (genotypeToCheck[j * testdata.ItemCount + i] == 1)//if item is already taken
                        {
                            if (itemFound)
                            {
                                fixedGenotype[j * testdata.ItemCount + i] = 0;//throw it away from every other knapsack
                            }
                            itemFound = true;
                        }
                    }
                }
            return fixedGenotype;
        }//End of FixGenotype()
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
            newGenotype = FixGenotype(newGenotype);
            return new KNPSpecimen(newGenotype, testdata);
        }//End of Mutate()
        internal double Evaluate(KNPProblem problem){
            if(score == double.MinValue) 
                score = problem.Evaluate(genotype); 
            return score;
        }//End of Evaluate();

    }
}
