using System;
using System.Collections.Generic;
using System.Text;

namespace Genethic_Algorithm
{
    class KNPSpecimen : Specimen
    {
        int score;
        int[] genotype;

        public KNPSpecimen()
        {
        }

        public int[] Genotype { get => genotype; set => genotype = value; }

        public KNPSpecimen[] Crossover(KNPSpecimen partner)
        {
            throw new NotImplementedException();
        }

        public KNPSpecimen Mutate()
        {
            throw new NotImplementedException();
        }

        public double Evaluate(Problem problem){
            if(score = null) 
                score = problem.Evaluate(genotype); 
            return score;
            }
    }
}
