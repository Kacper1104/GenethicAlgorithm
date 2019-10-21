using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Genethic_Algorithm
{
    class KNPGenethicAlgorithm
    {
        //PARAMS
        protected int populationSize;
        protected double probM;
        protected double probX;
        protected int tournamentSize;
        protected StreamWriter fileWriter;
        protected Loader testdata;
        protected KNPProblem problem;

        //VARIABLES
        protected KNPSpecimen[] population;
        protected int indexOfCurrentPopulation;
        protected int indexOfBestPopulation;
        protected double bestScore;
        protected sbyte[] bestGenotype;
        protected double populationBestScore;
        protected double populationWorstScore;
        protected double populationAvarageScore;

        //CONSTANTS
        protected const int POPULATIONS_PRINTER_INTERVAL = 100;
        protected const int ALGORITHM_STOP_MARKER = 5000;
        protected const double INITIAL_GENOTYPE_PROBABILITY = 0.3;

        public KNPGenethicAlgorithm(int populationSize, double probM, double probX, int tournamentSize, Loader testData, string outputDataFile)
        {
            this.populationSize = populationSize;
            this.probM = probM;
            this.probX = probX;
            this.tournamentSize = tournamentSize;
            this.testdata = testData;
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

        internal void Run()
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

        internal KNPSpecimen[] Initialize()
        {
            Random rnd = new Random();
            sbyte[] initialGenotype = new sbyte[testdata.ItemCount*testdata.KnapsackCount];
            for (int i = 0; i < testdata.ItemCount*testdata.KnapsackCount; i++)
            {
                if(i < INITIAL_GENOTYPE_PROBABILITY*testdata.ItemCount*testdata.KnapsackCount)
                {
                    initialGenotype[i] = 1;
                }
                else
                {
                    initialGenotype[i] = 0;
                }
            }
            population = new KNPSpecimen[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                sbyte[] shuffledGenotype = initialGenotype.OrderBy(x => rnd.Next()).ToArray(); //shuffle initial genotype
                population[i] = new KNPSpecimen((sbyte[])shuffledGenotype.Clone(), testdata);
            }
            indexOfCurrentPopulation = 0;
            indexOfBestPopulation = 0;
            bestScore = double.MinValue;

            return population;
        }

        internal void Evaluate()
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
                bestGenotype = (sbyte[])population[bestSpecimenIndex].Genotype.Clone();
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
        }//End of Evaluation()

        internal void Crossover()
        {
            for(int i = 0; i < populationSize; i += 2)
            {
                KNPSpecimen[] children = population[i].Crossover(population[i+1], probX);
                population[i] = children[0];
                population[i + 1] = children[1];
            }
        }//End of Crossover()

        internal void Mutation()
        {
            for (int i = 0; i < populationSize; i++)
            {
                population[i] = population[i].Mutate(probM);
            }
        }//End of Mutation()

        internal KNPSpecimen[] Selection()
        {
            KNPSpecimen[] newPopulation = new KNPSpecimen[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                newPopulation[i] = Tournament(population, tournamentSize); //tournament selection method
                //newPopulation[i] = Roulette(population); //Roulette selection method
                //newPopulation[i] = RandomSpecimen(population); //'Random' selection method
            }
            return newPopulation;
        }//End of Selection()

        internal KNPSpecimen Tournament(KNPSpecimen[] population, int tournamentSize)
        {
            Random random = new Random();
            KNPSpecimen fittest = new KNPSpecimen();
            double fittestScore = Double.MinValue;
            KNPSpecimen[] contestants = new KNPSpecimen[tournamentSize];
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
                    fittest = new KNPSpecimen((sbyte[])contestant.Genotype.Clone(), testdata);
                }
            }
            return fittest;
        }//End of Tournament

        internal void PrintScore()
        {
            Console.WriteLine("Population index: " + indexOfCurrentPopulation);
            Console.WriteLine("Population best score: " + populationBestScore);
            Console.WriteLine("Population worst score: " + populationWorstScore);
            Console.WriteLine("Population avarage score: " + populationAvarageScore);
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Best score ever: " + bestScore);
            Console.WriteLine("Best population index: " + indexOfBestPopulation);
            Console.WriteLine("Best population genotype: "+bestGenotype.ToString());
        }
    }
}
