package model;

import lombok.AllArgsConstructor;
import lombok.Getter;

@AllArgsConstructor
public class CsvResult {
    @Getter
    public int generationCounter;
    @Getter
    public double bestResult;
    @Getter
    public double worstResult;
    @Getter
    public double averageOfResults;
}
