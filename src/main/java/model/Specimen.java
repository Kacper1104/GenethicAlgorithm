package model;

import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Random;
import java.util.stream.Collectors;


public class Specimen {

    private final DataSet dataSet;

    @Getter
    @Setter
    private List<City> citiesVisitedInOrder;
    @Getter
    @Setter
    private Double objectiveFunction = Double.MAX_VALUE;
    @Getter
    @Setter
    private List<Double> currentVelocity;
    @Getter
    @Setter
    private List<Double> bestPosition;
    @Getter
    @Setter
    private List<Double> currentPosition;

    private final Random random;

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

    private double calculateDistance(City city, City otherCity) {
        //var cityInMatrix = dataSet.getDistanceMatrix().get(city);
        //var distance = cityInMatrix.get(otherCity);
        //return distance;
        return dataSet.getDistanceMatrix().get(city).get(otherCity);
    }

    public Specimen deepClone() {
        Specimen newSpecimen = new Specimen();
        newSpecimen.setCitiesVisitedInOrder(new ArrayList<>());
        newSpecimen.setObjectiveFunction(objectiveFunction);
        newSpecimen.setBestPosition(new ArrayList<>(bestPosition));
        newSpecimen.setCurrentVelocity(new ArrayList<>(currentVelocity));
        newSpecimen.setCurrentPosition(new ArrayList<>(currentPosition));

        for (City city : citiesVisitedInOrder) {
            newSpecimen.getCitiesVisitedInOrder().add(city.deepClone());
        }
        return newSpecimen;
    }

    //todo: remove unused
    public void setNewVelocityAndPosition(Double w, Double c1, Double r1, Double c2, Double r2, Specimen bestResult) {
        for (int i = 0; i < dataSet.getNumberOfCities(); i++) {
            Double vel = currentVelocity.get(i);
            Double pos = currentPosition.get(i);
            Double pBestLoc = bestPosition.get(i);

            Double gBestLoc = bestResult.bestPosition.get(i);

            double newVel = (w * vel) + (r1 * c1) * (pBestLoc -
                    pos) + (r2 * c2) * (gBestLoc - pos);

            currentVelocity.set(i, newVel);
            double newPos = pos + newVel;
            currentPosition.set(i, newPos);

            swapWithLocation((int) Math.abs(pos - newPos));
        }
    }

    public void swapWithLocation(int coeff) {
        if (coeff > 10) {
            coeff = 10;
        }
        for (int i = 0; i < coeff; i++) {
            int random1 = 0;
            int random2 = 0;

            while (random1 == random2)
                random2 = random.nextInt(0, dataSet.getNumberOfCities());

            City city_1 = citiesVisitedInOrder.get(random1);
            City city_2 = citiesVisitedInOrder.get(random2);

            citiesVisitedInOrder.set(random2, city_1);
            citiesVisitedInOrder.set(random1, city_2);

            if (getTotalDistanceTraveled() > objectiveFunction) {
                citiesVisitedInOrder.set(random2, city_2);
                citiesVisitedInOrder.set(random1, city_1);
            }
        }
    }

}
