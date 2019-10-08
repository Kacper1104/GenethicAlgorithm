using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Genethic_Algorithm
{
    class GenethicAlgorithm : IGenethicAlgorithm
    {
        //PARAMS
        int populationSize;
        double probM;
        double probX;
        int tournamentSize;
        Loader testData;
        StreamWriter fileWriter;
        Object problem;

        //VARIABLES
        Specimen[] population;
        int indexOfCurrentPopulation;
        int indexOfBestPopulation;
        double bestScore;
        int[] bestGenotype;
        double populationBestScore;
        double populationWorstScore;
        double populationAvarageScore;

        //CONSTANTS
        const bool SHUFFLE = true;

        public GenethicAlgorithm(int populationSize, double probM, double probX, int tournamentSize, Loader testData, string outputDataFile)
        {
            this.populationSize = populationSize;
            this.probM = probM;
            this.probX = probX;
            this.tournamentSize = tournamentSize;
            this.testData = testData;
            this.problem = new Problem(testData);

            try
            {
                this.fileWriter = new StreamWriter(outputDataFile);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            //Add CSV printer
        }

        public Specimen[] Initialize()
        {
            int[] initialGenotype = new int[testData.dimensions];
            for (int i = 0; i < testData.dimensions; i++)
            {
                initialGenotype[i] = testData.genes[i];
            }
            population = new Specimen[populationSize];
            Specimen ancestor;
            for(int i = 0; i < populationSize; i++)
            {
                ancestor = new Specimen(initialGenotype, testData.maxSpeed, testData.minSpeed, SHUFFLE, problem);
                population[i] = ancestor;
            }
            indexOfCurrentPopulation = 0;
            indexOfBestPopulation = 0;
            bestScore = double.MinValue;

            return population;
        }

        public void Evaluate()
        {
            populationBestScore = double.MinValue;
            populationWorstScore = double.MaxValue;
            populationAvarageScore = 0;
            int bestSpecimenIndex = 0;
            for(int i = 0; i < population.Length; i++)
            {
                double score = population[i].Evaluate();

                if (populationBestScore < score)
                {
                    populationBestScore = score;
                    bestSpecimenIndex = i;
                }
                if()
            }
            
            throw new NotImplementedException();
        }

        public void run()
        {
            throw new NotImplementedException();
        }

        public void Crossover()
        {
            throw new NotImplementedException();
        }

        public void Mutation()
        {
            throw new NotImplementedException();
        }

        public void PrintScore()
        {
            throw new NotImplementedException();
        }

        public Specimen[] Selection()
        {
            throw new NotImplementedException();
        }

        public Specimen Trournament()
        {
            throw new NotImplementedException();
        }
    }
}
