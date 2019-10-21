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
            KNPGenethicAlgorithm algorithm = new KNPGenethicAlgorithm(100, 0.02, 0.7, 20, testdata, "");
            algorithm.Initialize();
            algorithm.Run();
        }
    }
}
