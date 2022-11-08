import model.DataSet;
import services.GA;

import java.io.IOException;

public class Main {
    private static final String TRIVIAL_0 = "src/main/resources/trivial_0.ttp";
    private static final String TRIVIAL_1 = "src/main/resources/trivial_1.ttp";
    private static final String LONG = "src/main/resources/1.ttp";
    public static void main(String[] args) throws IOException {
        DataSet dataSet = DataSet.getInstance();
        dataSet.loadDataFromCSV(TRIVIAL_0);
        dataSet.calculateDistanceMatrix();
        dataSet.writeDistances();
        GA algorithm = new GA();
    }
}
