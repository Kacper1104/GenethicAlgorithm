using System;
using System.Collections.Generic;
using System.Text;

namespace Genethic_Algorithm
{
    class Specimen : ISpecimen
    {
        int[] genotype;

        public Specimen()
        {
        }

        public int[] Genotype { get => genotype; set => genotype = value; }

        public Specimen[] Crossover(Specimen partner)
        {
            throw new NotImplementedException();
        }

        public Specimen Mutate()
        {
            throw new NotImplementedException();
        }

        public double Evaluate(IProblem problem) => problem.Evaluate(genotype);
    }
}
