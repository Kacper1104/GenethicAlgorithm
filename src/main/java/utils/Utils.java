package utils;

import model.Specimen;

public class Utils {
    public static class Utilities
    {

        //GENETIC ALGORITHM

        public static final String PROBLEM_NAME = "1";
        public static final String FILE_NAME = "Data\\"+PROBLEM_NAME+".ttp";
        //public static final String CSV_SAVE_LOCATION_SOLUTION = @"C:\Users\marar\Desktop\results\";
        public static final String CSV_FILE_EXTENSION = ".csv";
        public static final String FILE_ANNOTATION_STATISTICS = "statystyka";
        public static final String FILE_ANNOTATION_PATH = "PATH";

        public static final int POPULATION_SIZE = 500;
        public static final int NUMBER_OF_GENERATIONS = 5000;
        public static final int TOURNAMENT_SIZE = 10;

        public static final double STAGNATION_FACTOR = 1; //ile pokoleń był zastój (w procentach 1 == 100 %)
        public static final double PROBABILITY_OF_CROSSOVER = 0.5;
        public static final double PROBABILITY_OF_MUTATION = 0.2;

        public static final CROSSOVER_METHOD CHOSEN_CROSSOVER_METHOD = CROSSOVER_METHOD.RANDOM_CROSSOVER;
        public static final SELECTION_METHOD CHOSEN_SELECTION_METHOD = SELECTION_METHOD.TOURNAMENT;
        public static final MUTATION_METHOD CHOSEN_MUTATION_METHOD = MUTATION_METHOD.SWAP;

        //PSO ALGORITHM

        public static final int PSO_POPULATION_SIZE = 25;
        public static final int PSO_ITERATIONS = 300;
        public static final double W = 1; //waga inercji
        public static final double c1 = 0.1; //współczynnik uczenia 1 - kognitywny
        public static final double c2 = 1; //współczynnik uczenia 2 - socjalny

        /*public static void SavePathSolutionToFile(Specimen result)
        {
            using (var writer = new StreamWriter($"{CSV_SAVE_LOCATION_SOLUTION + PROBLEM_NAME + DateTime.Now.ToFileTime()}{FILE_ANNOTATION_PATH}{CSV_FILE_EXTENSION}", true))
            using (var csv = new CsvWriter(writer))
            {
                List<CityElement> cities = new List<CityElement>();
                cities.AddRange(result.CitiesVisitedInOrder);
                cities.Add(cities[0]);

                csv.WriteComment("MIASTA");
                csv.NextRecord();

                csv.WriteHeader<CityElement>();
                csv.NextRecord();
                csv.WriteRecords(cities);
//            }
        }*/

        /*public static void SaveTotalsToFile(List<CsvTotalResult> totals)
        {
            using (var writer = new StreamWriter($"{CSV_SAVE_LOCATION_SOLUTION + PROBLEM_NAME + DateTime.Now.ToFileTime()}{FILE_ANNOTATION_STATISTICS}{CSV_FILE_EXTENSION}", true))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteComment("STATYSTYKI");
                csv.NextRecord();
                csv.WriteHeader<CsvResult>();
                csv.NextRecord();
                csv.WriteRecords(totals);
            }
        }*/

        public enum SELECTION_METHOD {
            TOURNAMENT,
            RANK,
            ROULETTE
        }

        public enum CROSSOVER_METHOD
        {
            ONE_POINT_CROSSOVER,
            TWO_POINT_CROSSOVER,
            RANDOM_CROSSOVER,
        }

        public enum MUTATION_METHOD
        {
            INVERSION,
            SWAP
        }

    }
}
