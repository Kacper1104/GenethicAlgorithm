namespace Genethic_Algorithm
{
    internal interface ISpecimen
    {
        Specimen[] Crossover(Specimen partner);

        Specimen Mutate();

        double Evaluate(IProblem problem);
    }
}