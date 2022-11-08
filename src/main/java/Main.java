import model.DataSet;
import model.Specimen;
import services.CsvStatisticsHolder;
import services.GA;
import utils.Utils;

import java.io.IOException;

public class Main {
    private static final String TRIVIAL_0 = "src/main/resources/trivial_0.ttp";
    private static final String TRIVIAL_1 = "src/main/resources/trivial_1.ttp";
    private static final String LONG = "src/main/resources/1.ttp";

   // private static final List<CsvTotalResult> results = new List<CsvTotalResult>();
    public static void main(String[] args) throws IOException {
        DataSet dataSet = DataSet.getInstance();
        dataSet.loadDataFromCSV(TRIVIAL_0);
        dataSet.calculateDistanceMatrix();
        //dataSet.writeDistances();
        runGeneticAlgorithm();
    }
    private static void runGeneticAlgorithm()
    {
        CsvStatisticsHolder.getInstance().reset();
        GA ga = new GA();
        Specimen best = ga.geneticCycle();

        System.out.println("FINISHED: " + best.getObjectiveFunction());
        //Utils.savePathSolutionToFile(best);

        double b = best.getObjectiveFunction();
        //double avg = CsvStatisticsHolder.getInstance().getResults(). Average(p => p.averageOfResults);
        //double minavg = (CsvStatisticsHolder.getInstance().getResults(). Average(p => p.worstResult));
        //double maxavg = (CsvStatisticsHolder.getInstance().getResults(). Average(p => p.bestResult));

        //results.add(new CsvTotalResult(b, maxavg, minavg, avg));
    }
}
