using System;
using System.Collections.Generic;
using System.Text;

namespace Genethic_Algorithm
{
    internal class KNPProblem
    {
        //params
        Loader testData;
        //constructors
        public KNPProblem()
        {
        }
        public KNPProblem(Loader testData)
        {
            this.testData = testData;
        }
        //methods
        internal double Evaluate(sbyte[] genotype)
        {
            throw new NotImplementedException();
        }
    }
}
