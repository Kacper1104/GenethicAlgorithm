package services;

import lombok.AllArgsConstructor;
import lombok.Builder;
import model.DataSet;
import model.Specimen;

import java.util.List;

@Builder
public class GA {

    private DataSet dataSet;

    private int currentGeneration;
    private int stagnationCounter;

    private List<Specimen> oldPopulation;
    private List<Specimen> newPopulation;



    public GA() {
        this.dataSet = DataSet.getInstance();
    }
}
