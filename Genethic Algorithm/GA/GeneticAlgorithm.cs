using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Genethic_Algorithm
{
    abstract class GenethicAlgorithm
    {
        //PARAMS
        protected int populationSize;
        protected double probM;
        protected double probX;
        protected int tournamentSize;
        protected StreamWriter fileWriter;
        protected Loader testData;
        protected Problem problem;

        //VARIABLES
        protected Specimen[] population;
        protected int indexOfCurrentPopulation;
        protected int indexOfBestPopulation;
        protected double bestScore;
        protected int[] bestGenotype;
        protected double populationBestScore;
        protected double populationWorstScore;
        protected double populationAvarageScore;

        //CONSTANTS
        protected const bool SHUFFLE = true;
        protected const int POPULATIONS_PRINTER_INTERVAL = 100;
        protected const int ALGORITHM_STOP_MARKER = 5000;


        internal abstract Specimen[] Initialize();
        internal abstract void Evaluate();
        internal abstract void Run();
        internal abstract Specimen[] Selection();
        internal abstract Specimen Tournament(Specimen[] population, int tournamentSize);
        internal abstract void Crossover();
        internal abstract void Mutation();
        internal abstract void PrintScore();
    }
}
