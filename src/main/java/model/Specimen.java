package model;

import lombok.Builder;
import lombok.Getter;
import lombok.Setter;

import java.util.*;
import java.util.stream.Collectors;

@Builder
public class Specimen {

    private final DataSet dataSet;

    @Getter
    @Setter
    private List<City> citiesVisitedInOrder;
    @Getter
    @Setter
    private Double objectiveFunction;
    @Getter
    @Setter
    private List<Double> currentVelocity;
    @Getter
    @Setter
    private List<Double> bestPosition;
    @Getter
    @Setter
    private List<Double> currentPosition;

    private Random random;

    public Specimen() {
        this.dataSet = DataSet.getInstance();
        this.random = new Random();
        this.citiesVisitedInOrder = new ArrayList<>();

        this.fillTheVisitedCitiesList();
        this.initializeVelocities();
        this.initializePosition();
        this.setObjectiveFunction();
    }

    private void initializeVelocities() {
        currentVelocity = new ArrayList<Double>();
        dataSet.getCitySet().forEach(
                city -> currentVelocity.add(random.nextDouble() * 2. - 1.));
    }

    private void initializePosition() {
        bestPosition = new ArrayList<>();
        currentPosition = new ArrayList<>();
        for (int i = 0; i < dataSet.getNumberOfCities(); i++) {
            bestPosition.add((double) i);
            currentPosition.add((double) i);
        }
    }

    private void fillTheVisitedCitiesList() {
        citiesVisitedInOrder = dataSet.getCitySet().stream()
                .map(City::deepClone)
                .collect(Collectors.toList());
        Collections.shuffle(citiesVisitedInOrder);
    }

    public void setObjectiveFunction() {
        var newObjectiveFunction = getTotalDistanceTraveled();
        if (newObjectiveFunction < objectiveFunction) {
            bestPosition.clear();
            bestPosition.addAll(currentPosition);
        }
        objectiveFunction = newObjectiveFunction;
    }

    private double getTotalDistanceTraveled() {
        Double totalDistance = (double) 0;

        for (int i = 0; i < citiesVisitedInOrder.size() - 1; i++) {
            totalDistance += calculateDistance(citiesVisitedInOrder.get(i), citiesVisitedInOrder.get(i + 1));
        }

        totalDistance += calculateDistance(citiesVisitedInOrder.get(citiesVisitedInOrder.size() - 1), citiesVisitedInOrder.get(0));
        return totalDistance;
    }

    private double calculateDistance(City city, City otherCity)
    {
        return dataSet.getDistanceMatrix().get(city).get(otherCity);
    }

    public Specimen deepClone() {
        Specimen newSpecimen = Specimen.builder()
                .citiesVisitedInOrder(List.of())
                .objectiveFunction(objectiveFunction)
                .bestPosition(bestPosition)
                .currentVelocity(currentVelocity)
                .currentPosition(currentPosition)
                .build();
        for(int i = 0; i < citiesVisitedInOrder.size(); i++) {
            newSpecimen.citiesVisitedInOrder.add(citiesVisitedInOrder.get(i).deepClone());
        }
        return newSpecimen;
    }

    public void setNewVelocityAndPosition(Double w, Double c1, Double r1, Double c2, Double r2, Specimen bestResult)
    {
        for (int i = 0; i < dataLoaded.TotalNumberOfCities; i++)
        {
            double vel = CurrentVelocity[i];
            double pos = CurrentPosition[i];
            double pBestLoc = BestPosition[i];

            double gBestLoc = bestResult.BestPosition[i];

            double newVel = (w * vel) + (r1 * c1) * (pBestLoc -
                    pos) + (r2 * c2) * (gBestLoc - pos);

            CurrentVelocity[i] = newVel;
            double newPos = pos + newVel;
            CurrentPosition[i] = newPos;

            SwapWithLocation((int)Math.Abs(pos - newPos));
        }
    }

}
