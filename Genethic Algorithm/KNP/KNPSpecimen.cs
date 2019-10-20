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
        Loader testdata;



        public KNPSpecimen()
        {
        }

        public KNPSpecimen(int itemCount, int knapsackCount, bool schuffle)
        {
            this.genotype = new sbyte[itemCount*knapsackCount];
            for(int i = 0; i < itemCount*knapsackCount; i++)
            {
                this.genotype[i] = 0;
            }
        }

        public KNPSpecimen(sbyte[] genotype, Loader testdata)
        {
            this.testdata = testdata;
            this.genotype = (sbyte[])genotype.Clone();
        }

        public double Score { get => score; set => score = value; }
        public sbyte[] Genotype { get => genotype; set => genotype = value; }

        internal override Specimen[] Crossover(Specimen partner)
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

        internal override double Evaluate(Problem problem){
            if(score == null) 
                score = problem.Evaluate(genotype); 
            return score;
        }

    }
}
