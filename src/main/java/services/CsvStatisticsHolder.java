package services;

import lombok.AccessLevel;
import lombok.Getter;
import lombok.Setter;
import model.CsvResult;
import model.Specimen;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.NoSuchElementException;

public class CsvStatisticsHolder {
    private static CsvStatisticsHolder INSTANCE;
    @Getter
    @Setter(AccessLevel.PRIVATE)
    private List<CsvResult> results;

    private CsvStatisticsHolder() {
        results = new ArrayList<>();
    }

    public static CsvStatisticsHolder getInstance(){
        if(INSTANCE == null) {
            INSTANCE = new CsvStatisticsHolder();
        }
        return INSTANCE;
    }

    public void addNewGenerationStatistics(int generationCounter, List<Specimen> generation) {
        double average = generation.stream().map(Specimen::getObjectiveFunction)
                .reduce(0., Double::sum) / generation.size();
        double bestSolution = generation.stream()
                .min(Comparator.comparing(Specimen::getObjectiveFunction))
                .orElseThrow(NoSuchElementException::new)
                .getObjectiveFunction();
        double worstSolution = generation.stream()
                .max(Comparator.comparing(Specimen::getObjectiveFunction))
                .orElseThrow(NoSuchElementException::new)
                .getObjectiveFunction();

        results.add(new CsvResult(generationCounter, bestSolution, worstSolution, average));
    }

    public void reset()
    {
        results.clear();
    }
}
