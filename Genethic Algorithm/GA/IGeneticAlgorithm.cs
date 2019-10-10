﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Genethic_Algorithm
{
    interface IGenethicAlgorithm
    {
        Specimen[] Initialize();
        void Evaluate();
        void run();
        Specimen[] Selection();
        Specimen Tournament();
        void Crossover();
        void Mutation();
        void PrintScore();
    }
}
