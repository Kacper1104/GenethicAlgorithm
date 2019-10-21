using System;
using System.IO;

namespace Genethic_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Loader testdata = new Loader("resources\\multi\\" + "p01" + ".csv");
            testdata.readFile();
            KNPGenethicAlgorithm algorithm = new KNPGenethicAlgorithm(100, 0.05, 0.7, 5, 20, testdata, "test.csv");
            algorithm.Initialize();
            algorithm.Run();
        }
    }
}
