using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Genethic_Algorithm
{
    class KNPGenethicAlgorithm : GenethicAlgorithm
    {

        public KNPGenethicAlgorithm(int populationSize, double probM, double probX, int tournamentSize, Loader testData, string outputDataFile)
        {
            this.populationSize = populationSize;
            this.probM = probM;
            this.probX = probX;
            this.tournamentSize = tournamentSize;
            this.testData = testData;
            this.problem = new KNPProblem(testData);

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

        internal override void Run()
        {
            while (true)
            {
                Evaluate();
                if (indexOfCurrentPopulation - indexOfBestPopulation >= ALGORITHM_STOP_MARKER)
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

        internal override Specimen[] Initialize()
        {
            int[] initialGenotype = new int[testData.ItemCount];
            for (int i = 0; i < testData.ItemCount; i++)
            {
                initialGenotype[i] = testData.genes[i];
            }
            population = new KNPSpecimen[populationSize];
            Specimen ancestor;
            for(int i = 0; i < populationSize; i++)
            {
                ancestor = new KNPSpecimen(initialGenotype, testData.maxSpeed, testData.minSpeed, SHUFFLE, problem);
                population[i] = ancestor;
            }
            indexOfCurrentPopulation = 0;
            indexOfBestPopulation = 0;
            bestScore = double.MinValue;

            return population;
        }

        internal override void Evaluate()
        {
            populationBestScore = double.MinValue;
            populationWorstScore = double.MaxValue;
            populationAvarageScore = 0;
            int bestSpecimenIndex = 0;
            for(int i = 0; i < population.Length; i++)
            {
                double score = population[i].Evaluate(problem);

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
                bestGenotype = new int[testData.ItemCount];
                for (int d = 0; d < testData.ItemCount; d++)
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
        internal override void Crossover()
        {
            Random random = new Random();
            for(int i = 0; i < populationSize; i += 2)
            {
                if(random.NextDouble() <= probX)
                {
                    Specimen[] children = population[i].Crossover(population[i + 1]);
                    population[i] = children[0];
                    population[i + 1] = children[1];
                }
            }
        }

        internal override void Mutation()
        {
            for (int i = 0; i < populationSize; i++)
                    population[i] = population[i].Mutate(probM);
            }

        internal override Specimen[] Selection()
        {
            Specimen[] newPopulation = new Specimen[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                newPopulation[i] = Tournament(population, tournamentSize); //tournament selection method
                //newPopulation[i] = Roulette(population); //Roulette selection method
                //newPopulation[i] = RandomSpecimen(population); //'Random' selection method
            }
            return newPopulation;
        }

        internal override Specimen Tournament(Specimen[] population, int tournamentSize)
        {
            Random random = new Random();
            Specimen fittest = new KNPSpecimen();
            double fittestScore = Double.MinValue;
            Specimen[] contestants = new KNPSpecimen[tournamentSize];
            int randomContestant = random.Next(populationSize);
            for (int i = 0; i < tournamentSize; i++)
            {
                while (Array.Find(contestants, c => object.ReferenceEquals(c, population[randomContestant])) != null)
                {
                    randomContestant = random.Next(populationSize);
                }
                contestants[i] = population[randomContestant];
            }
            double contestantScore;
            foreach (KNPSpecimen contestant in contestants)
            {
                contestantScore = contestant.Evaluate(problem);
                if (contestantScore > fittestScore)
                {
                    fittestScore = contestantScore;
                    fittest = new KNPSpecimen(contestant.Genotype, noschuffle);
                }
            }
            return fittest;
        }

        internal override void PrintScore()
        {
            Console.WriteLine("Population index: " + indexOfCurrentPopulation);
            Console.WriteLine("Population best score: " + populationBestScore);
            Console.WriteLine("Population worst score: " + populationWorstScore);
            Console.WriteLine("Population avarage score: " + populationAvarageScore);
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Best score ever: " + bestScore);
            Console.WriteLine("Best population index: " + indexOfBestPopulation);
            Console.WriteLine();
        }
    }
}
