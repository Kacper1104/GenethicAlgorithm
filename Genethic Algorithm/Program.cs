using System;
using System.IO;

namespace Genethic_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Loader testdata;
            KNPGenethicAlgorithm algorithm;
            //testdata = new Loader("resources\\single\\" + "p01" + ".csv", false);
            //testdata.readFile();
            //algorithm = new KNPGenethicAlgorithm(100, 0.05, 0.7, 5000, 20, testdata, "test.csv");
            //algorithm.Initialize();
            //algorithm.Run();

            //multi
            testdata = new Loader("resources\\multi\\" + "p01" + ".csv", true);
            testdata.readFile();
            algorithm = new KNPGenethicAlgorithm(100, 0.05, 0.7, 5000, 20, testdata, "test.csv");
            algorithm.Initialize();
            algorithm.Run();
        }
    }
}
