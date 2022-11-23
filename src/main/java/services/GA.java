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
        return switch (selectionMethod) {
            case TOURNAMENT -> tournamentSelectionMethod();
            case ROULETTE -> rouletteSelectionMethod();
            case RANK -> rankSelectionMethod();
            default -> throw new IllegalArgumentException("Selection method missing");
        };
    }

    private void mutation(List<Specimen> population) {
        if (random.nextDouble() <= PROBABILITY_OF_MUTATION) {
            switch (mutationMethod) {
                case SWAP -> population.forEach(this::swapMutation);
                case INVERSION -> population.forEach(this::inversionMutation);
                default -> throw new IllegalArgumentException("Mutation method missing");
            }
        }
    }

    private void crossover(List<Specimen> population) {
        for (int i = 0; i < POPULATION_SIZE; i += 2) {
            Specimen[] children = random.nextDouble() <= PROBABILITY_OF_CROSSOVER
                    ? getCrossChildren(population.get(i), population.get(i + 1))
                    : new Specimen[]{population.get(i), population.get(i + 1)};

            population.set(i, children[0]);
            population.set(i + 1, children[1]);
        }
    }
    private Specimen[] getCrossChildren(Specimen parent1, Specimen parent2) {
        if (random.nextDouble() <= PROBABILITY_OF_CROSSOVER) {
            return switch (crossoverMethod) {
                case RANDOM_CROSSOVER -> randomCrossover(parent1, parent2);
                case ONE_POINT_CROSSOVER -> onePointCrossover(parent1, parent2);
                case TWO_POINT_CROSSOVER -> twoPointCrossover(parent1, parent2);
                default -> throw new IllegalArgumentException("Crossover method missing");
            };
        } else return new Specimen[]{parent1, parent2};
    }

    private void evaluate(List<Specimen> population) {
        for (Specimen specimen : population) specimen.setObjectiveFunction();
    }

    //selection methods
    private List<Specimen> tournamentSelectionMethod() {
        List<Specimen> nextPopulation = new ArrayList<>(POPULATION_SIZE);
        for (int i = 0; i < POPULATION_SIZE; i++)
            nextPopulation.add(tournamentSelectionMethodForOneSpecimen());

        return nextPopulation;
    }

    private List<Specimen> rouletteSelectionMethod() {
        double totalFitnessSum = oldPopulation.stream().map(Specimen::getObjectiveFunction)
                .reduce(0., Double::sum);

        List<Specimen> nextPopulation = new ArrayList<>(POPULATION_SIZE);
        for (int i = 0; i < POPULATION_SIZE; i++)
            nextPopulation.add(rouletteSelectionMethodForOneSpecimen(totalFitnessSum));

        return nextPopulation;
    }

    private Specimen rouletteSelectionMethodForOneSpecimen(double totalFitnessSum) {
        double randomNumber = random.nextDouble() * totalFitnessSum;
        double partialSum = 0;
        for (int i = 0; i < POPULATION_SIZE; i++)
        {
            partialSum += (oldPopulation.get(i).getObjectiveFunction());
            if (partialSum >= randomNumber)
                return oldPopulation.get(i);
        }
        return null;
    }

    private List<Specimen> rankSelectionMethod() {
        List<Specimen> populationRank = new ArrayList<>(List.copyOf(oldPopulation));
        populationRank.sort(Comparator.comparing(Specimen::getObjectiveFunction));
        double totalFitnessSum = 0;

        for (int i = 0; i < populationRank.size(); i++)
        {
            totalFitnessSum += i;
        }

        List<Specimen> nextPopulation = new ArrayList<>(POPULATION_SIZE);
        for (int i = 0; i < POPULATION_SIZE; i++)
            nextPopulation.add(rankSelectionMethodForOneSpecimen(totalFitnessSum));

        return nextPopulation;
    }

    private Specimen rankSelectionMethodForOneSpecimen(Double totalFitnessSum) {
        double randomNumber = random.nextDouble() * totalFitnessSum;
        double partialSum = 0;
        for (int i = 0; i < POPULATION_SIZE; i++)
        {
            partialSum += i;
            if (partialSum >= randomNumber)
                return oldPopulation.get(i);
        }
        return null;
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
    private void swapMutation(Specimen specimen) {
        int index1 = random.nextInt(0, dataSet.getNumberOfCities());
        int index2 = random.nextInt(0, dataSet.getNumberOfCities());

        while (index2 == index1)
            index2 = random.nextInt(0, dataSet.getNumberOfCities());

        City city1 = specimen.getCitiesVisitedInOrder().get(index1);
        City city2 = specimen.getCitiesVisitedInOrder().get(index2);

        specimen.getCitiesVisitedInOrder().set(index1, city2);
        specimen.getCitiesVisitedInOrder().set(index2, city1);
    }

    private void inversionMutation(Specimen specimen) {
        Collections.reverse(specimen.getCitiesVisitedInOrder());
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

    private Specimen[] onePointCrossover(Specimen parent1, Specimen parent2) {
        int crossPoint = random.nextInt(1, dataSet.getNumberOfCities() - 1);

        Specimen child1 = onePointCrossoverBasedOnFirstParent(parent1, parent2, crossPoint);
        Specimen child2 = onePointCrossoverBasedOnFirstParent(parent2, parent1, crossPoint);

        return new Specimen[] { child1, child2 };
    }

    private Specimen onePointCrossoverBasedOnFirstParent(Specimen parent1, Specimen parent2, int crossPoint) {
        Specimen child = new Specimen();

        List<City> childCities = new ArrayList<>();
        List<City> restOfGenes = new ArrayList<>();

        for (int i = 0; i < crossPoint; i++)
            childCities.add(parent1.getCitiesVisitedInOrder().get(i).deepClone());

        for (int i = crossPoint; i < dataSet.getNumberOfCities(); i++)
            restOfGenes.add(parent1.getCitiesVisitedInOrder().get(i));

        for (int i = crossPoint; i < dataSet.getNumberOfCities(); i++)
        {
            if (restOfGenes.contains(parent2.getCitiesVisitedInOrder().get(i)))
            {
                restOfGenes.remove(parent2.getCitiesVisitedInOrder().get(i));
                childCities.add(parent2.getCitiesVisitedInOrder().get(i).deepClone());
            }
            else
            {
                var item = restOfGenes.get(0);
                restOfGenes.remove(0);
                childCities.add(item.deepClone());
            }
        }

        child.setCitiesVisitedInOrder(childCities);

        return child;
    }

    private Specimen[] twoPointCrossover(Specimen parent1, Specimen parent2) {
        int crossPoint1 = random.nextInt(1, dataSet.getNumberOfCities() - 1);
        int crossPoint2 = random.nextInt(1, dataSet.getNumberOfCities() - 1);

        while (crossPoint1 == crossPoint2)
            crossPoint2 = random.nextInt(1, dataSet.getNumberOfCities() - 1);

        if (crossPoint2 < crossPoint1)
        {
            int temp = crossPoint2;
            crossPoint2 = crossPoint1;
            crossPoint1 = temp;
        }

        Specimen child1 = twoPointBasedOnFirstParent(parent1, parent2, crossPoint1, crossPoint2);
        Specimen child2 = twoPointBasedOnFirstParent(parent2, parent1, crossPoint1, crossPoint2);

        return new Specimen[] { child1, child2 };
    }

    private Specimen twoPointBasedOnFirstParent(Specimen parent1, Specimen parent2, int crossPoint1, int crossPoint2) {
        Specimen child = new Specimen();

        List<City> childCities = new ArrayList<>();
        List<City> restOfGenes = new ArrayList<>();

        for (int i = 0; i < crossPoint1; i++)
            childCities.add(parent1.getCitiesVisitedInOrder().get(i).deepClone());

        for (int i = crossPoint1; i < crossPoint2; i++)
            restOfGenes.add(parent1.getCitiesVisitedInOrder().get(i));

        for (int i = crossPoint2; i < dataSet.getNumberOfCities(); i++)
            restOfGenes.add(parent1.getCitiesVisitedInOrder().get(i));

        for (int i = crossPoint1; i < crossPoint2; i++)
        {
            if (restOfGenes.contains(parent2.getCitiesVisitedInOrder().get(i)))
            {
                restOfGenes.remove(parent2.getCitiesVisitedInOrder().get(i));
                childCities.add(parent2.getCitiesVisitedInOrder().get(i).deepClone());
            }
            else
            {
                var item = restOfGenes.get(0);
                restOfGenes.remove(0);
                childCities.add(item.deepClone());
            }
        }

        for (int i = crossPoint2; i < dataSet.getNumberOfCities(); i++)
        {
            var item = restOfGenes.get(0);
            restOfGenes.remove(0);
            childCities.add(item.deepClone());
        }

        child.setCitiesVisitedInOrder(childCities);
        return child;
    }
}
