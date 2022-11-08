package services;

import lombok.Builder;
import model.City;
import model.DataSet;
import model.Specimen;
import utils.Utils;

import java.io.Console;
import java.util.*;

import static utils.Utils.Utilities.*;

public class GA {

    private final DataSet dataSet;
    private final CsvStatisticsHolder csvStatisticsHolder;

    private int currentGeneration;
    private int stagnationCounter;

    private List<Specimen> oldPopulation;
    private List<Specimen> newPopulation;

    private Specimen bestSolution;

    private Random random;

    private Utils.Utilities.SELECTION_METHOD selectionMethod;
    private Utils.Utilities.MUTATION_METHOD mutationMethod;
    private Utils.Utilities.CROSSOVER_METHOD crossoverMethod;


    public GA() {
        this.dataSet = DataSet.getInstance();
        this.csvStatisticsHolder = CsvStatisticsHolder.getInstance();
        random = new Random();
        setSelectionMethod();
        setMutationMethod();
        setCrossoverMethod();
        initializePopulation();
    }

    private void setSelectionMethod() {
        this.selectionMethod = CHOSEN_SELECTION_METHOD;
    }

    private void setMutationMethod() {
        this.mutationMethod = CHOSEN_MUTATION_METHOD;
    }

    private void setCrossoverMethod() {
        this.crossoverMethod = CHOSEN_CROSSOVER_METHOD;
    }

    private void initializePopulation() {
        oldPopulation = new ArrayList<Specimen>(POPULATION_SIZE);
        for (int i = 0; i < POPULATION_SIZE; i++)
            oldPopulation.add(new Specimen());
    }

    public Specimen geneticCycle() {
        bestSolution = findBest(oldPopulation).deepClone();
        csvStatisticsHolder.addNewGenerationStatistics(currentGeneration, oldPopulation);

        while (currentGeneration < NUMBER_OF_GENERATIONS && stagnationCounter < STAGNATION_FACTOR * NUMBER_OF_GENERATIONS) {
            currentGeneration++;

            newPopulation = selection();
            crossover(newPopulation);
            mutation(newPopulation);
            evaluate(newPopulation);

            Specimen bestSolutionInNewPopolation = findBest(newPopulation);

            if (bestSolutionInNewPopolation.getObjectiveFunction() < bestSolution.getObjectiveFunction()) {
                bestSolution = bestSolutionInNewPopolation.deepClone();
                stagnationCounter = 0;
            } else
                stagnationCounter++;

            csvStatisticsHolder.addNewGenerationStatistics(currentGeneration, newPopulation);

            oldPopulation = newPopulation;
            System.out.printf("Generation: %s: %s%n", currentGeneration, bestSolutionInNewPopolation.getObjectiveFunction());
        }

        return bestSolution;
    }

    private Specimen findBest(List<Specimen> examinedPopulation) {
        return examinedPopulation.stream()
                .min(Comparator.comparing(Specimen::getObjectiveFunction))
                .orElseThrow(NoSuchElementException::new);
    }

    private List<Specimen> selection() {
        switch (selectionMethod) {
            case TOURNAMENT:
                return tournamentSelectionMethod();
            case ROULETTE:
                //TODO: rouletteSelectionMethod();
                throw new RuntimeException("Roulette method missing");
            case RANK:
                //TODO: rankSelectionMethod();
                throw new RuntimeException("Rank method missing");
            default:
                throw new IllegalArgumentException("Selection method missing");
        }
    }

    private void mutation(List<Specimen> population) {
        if (random.nextDouble() <= PROBABILITY_OF_MUTATION) {
            switch (mutationMethod) {
                case SWAP:
                    population.forEach(specimen -> swapMutation(specimen));
                case INVERSION:
                    //TODO: inversionMutation();
                    throw new RuntimeException("Inversion method missing");
                default:
                    throw new IllegalArgumentException("Mutation method missing");
            }
        }
    }

    private void crossover(List<Specimen> population) {
        for (int i = 0; i < POPULATION_SIZE; i += 2) {
            if (i + 1 < POPULATION_SIZE) {
                Specimen[] children = random.nextDouble() <= PROBABILITY_OF_CROSSOVER
                        ? getCrossChildren(population.get(i), population.get(i + 1))
                        : new Specimen[]{population.get(i), population.get(i + 1)};

                population.set(i, children[0]);
                population.set(i + 1, children[1]);
            }
        }
    }
    private Specimen[] getCrossChildren(Specimen parent1, Specimen parent2) {
        if (random.nextDouble() <= PROBABILITY_OF_CROSSOVER) {
            return switch (CHOSEN_CROSSOVER_METHOD) {
                case RANDOM_CROSSOVER -> randomCrossover(parent1, parent2);
                case ONE_POINT_CROSSOVER ->
                    //TODO: onePointCrossover();
                        throw new RuntimeException("One point crossover method missing");
                case TWO_POINT_CROSSOVER ->
                    //TODO: twoPointCrossover();
                        throw new RuntimeException("Two point crossover method missing");
                default -> throw new IllegalArgumentException("Crossover method missing");
            };
        } else return new Specimen[]{parent1, parent2};
    }

    private void evaluate(List<Specimen> population) {
        for (int i = 0; i < population.size(); i++)
            population.get(i).setObjectiveFunction();
    }

    //selection methods
    private List<Specimen> tournamentSelectionMethod() {
        List<Specimen> nextPopulation = new ArrayList<>(POPULATION_SIZE);
        for (int i = 0; i < POPULATION_SIZE; i++)
            nextPopulation.add(tournamentSelectionMethodForOneSpecimen());

        return nextPopulation;
    }

    private Specimen tournamentSelectionMethodForOneSpecimen() {
        List<Specimen> chosen = new ArrayList<>();
        List<Integer> randomIndexes = new ArrayList<>();
        while (randomIndexes.size() < TOURNAMENT_SIZE) {
            int randomIndex = random.nextInt(0, POPULATION_SIZE);
            while (randomIndexes.contains(randomIndex))
                randomIndex = random.nextInt(0, POPULATION_SIZE);

            randomIndexes.add(randomIndex);
        }

        for (int i = 0; i < TOURNAMENT_SIZE; i++)
            chosen.add(oldPopulation.get(randomIndexes.get(i)));

        return chosen.stream()
                .min(Comparator.comparing(Specimen::getObjectiveFunction))
                .orElseThrow(NoSuchElementException::new);
    }

    //mutation methods
    private Specimen swapMutation(Specimen specimen) {
        int index1 = random.nextInt(0, dataSet.getNumberOfCities());
        int index2 = random.nextInt(0, dataSet.getNumberOfCities());

        while (index2 == index1)
            index2 = random.nextInt(0, dataSet.getNumberOfCities());

        City city1 = specimen.getCitiesVisitedInOrder().get(index1);
        City city2 = specimen.getCitiesVisitedInOrder().get(index2);

        specimen.getCitiesVisitedInOrder().set(index1, city2);
        specimen.getCitiesVisitedInOrder().set(index2, city1);
        return specimen;
    }

    //crossover methods
    private Specimen[] randomCrossover(Specimen parent1, Specimen parent2) {
        int numberOfRandomChromosomes = random.nextInt(1, dataSet.getNumberOfCities() - 1);

        Specimen child1 = randomCrossBasedOnFirstParent(parent1, parent2, numberOfRandomChromosomes);
        Specimen child2 = randomCrossBasedOnFirstParent(parent2, parent1, numberOfRandomChromosomes);

        return new Specimen[] { child1, child2 };
    }

    private Specimen randomCrossBasedOnFirstParent(Specimen parent1, Specimen parent2, int numberOfRandomChromosomes) {
        Specimen child = new Specimen();

        child.getCitiesVisitedInOrder().clear();

        List<Integer> randomIndices = new ArrayList<>();
        List<Integer> allIndices = new ArrayList<>();
        //List<City> restOfGenes = new ArrayList<>();

        for (int i = 0; i < dataSet.getNumberOfCities(); i++){
            //restOfGenes.add(parent1.getCitiesVisitedInOrder().get(i));
            allIndices.add(i);
        }

        for (int i = 0; i < numberOfRandomChromosomes; i++)
        {
            int randomIndex = random.nextInt(0, allIndices.size() - 1);
            randomIndices.add(allIndices.get(randomIndex));
            allIndices.remove(randomIndex);
        }

        for (int i = 0; i < dataSet.getNumberOfCities(); i++) {
            {
                if (randomIndices.contains(i)) {
                    if (child.getCitiesVisitedInOrder().contains(parent2.getCitiesVisitedInOrder().get(i))) {
                        child.getCitiesVisitedInOrder().add(parent2.getCitiesVisitedInOrder().get(i).deepClone());
                    } else {
                        child.getCitiesVisitedInOrder().add(parent1.getCitiesVisitedInOrder().get(i).deepClone());
                    }
                } else {
                    child.getCitiesVisitedInOrder().add(parent1.getCitiesVisitedInOrder().get(i).deepClone());
                }
            }
        }
        return child;
    }
}
