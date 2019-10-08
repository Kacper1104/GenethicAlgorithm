using System;
using System.Collections.Generic;
using System.Text;

namespace Genethic_Algorithm
{
    class Specimen : ISpecimen
    {
        int[] genotype;

        public int[] Genotype { get => genotype; set => genotype = value; }

        internal static Specimen[] Crossover(Specimen specimen1, Specimen specimen2)
        {
            throw new NotImplementedException();
        }

        internal Specimen Mutate()
        {
            throw new NotImplementedException();
        }
    }
}
