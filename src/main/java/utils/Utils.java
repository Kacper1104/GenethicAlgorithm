package utils;

import model.Specimen;

public class Utils {
    public static class Utilities
    {

        public static final int POPULATION_SIZE = 500;
        public static final int NUMBER_OF_GENERATIONS = 5000;
        public static final int TOURNAMENT_SIZE = 10;

        public static final double STAGNATION_FACTOR = 1; //ile pokoleń był zastój (w procentach 1 == 100 %)
        public static final double PROBABILITY_OF_CROSSOVER = 0.5;
        public static final double PROBABILITY_OF_MUTATION = 0.2;

        public static final CROSSOVER_METHOD CHOSEN_CROSSOVER_METHOD = CROSSOVER_METHOD.RANDOM_CROSSOVER;
        public static final SELECTION_METHOD CHOSEN_SELECTION_METHOD = SELECTION_METHOD.TOURNAMENT;
        public static final MUTATION_METHOD CHOSEN_MUTATION_METHOD = MUTATION_METHOD.SWAP;

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
