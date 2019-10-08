﻿using System;
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
        const int POPULATIONS_PRINTER_INTERVAL = 100;
        const int ALGORITHM_STOP_MARKER = 5000;

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
            int[] initialGenotype = new int[testData.Dimensions];
            for (int i = 0; i < testData.Dimensions; i++)
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
                if(populationWorstScore > score)
                {
                    populationWorstScore = score;
                }
                populationAvarageScore += score;
            }
            if (populationBestScore > bestScore)
            {
                bestScore = populationBestScore;
                indexOfBestPopulation = indexOfCurrentPopulation;
                bestGenotype = new int[testData.Dimensions];
                for (int d = 0; d < testData.Dimensions; d++)
                {
                    bestGenotype[d] = population[bestSpecimenIndex].Genotype[d];
                }
            }
            try
            {
                //write to CSV
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            if(indexOfBestPopulation%POPULATIONS_PRINTER_INTERVAL == 0)
            {
                PrintScore();
            }
        }

        public void run()
        {
            while (true)
            {
                Evaluate();
                if(indexOfCurrentPopulation - indexOfBestPopulation >= ALGORITHM_STOP_MARKER)
                {
                    break;
                }

                population = Selection();
                Crossover();
                Mutation();
                indexOfCurrentPopulation++;
            }
            Evaluate();
            Console.WriteLine("Algorithm has finished.");
        }

        public void Crossover()
        {
            Random random = new Random();
            for(int i = 0; i < populationSize; i += 2)
            {
                if(random.NextDouble() <= probX)
                {
                    Specimen[] children = Specimen.Crossover(population[i], population[i + 1]);
                    population[i] = children[0];
                    population[i + 1] = children[1];
                }
            }
        }

        public void Mutation()
        {
            Random random = new Random();
            for (int i = 0; i < populationSize; i ++)
            {
                if (random.NextDouble() <= probX)
                {
                    population[i] = population[i].Mutate();
                }
            }
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