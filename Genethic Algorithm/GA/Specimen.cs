namespace Genethic_Algorithm
{
    internal abstract class Specimen
    {
        internal abstract Specimen[] Crossover(Specimen partner);

        internal abstract Specimen Mutate(double probM);

        internal abstract double Evaluate(Problem problem);
    }
}