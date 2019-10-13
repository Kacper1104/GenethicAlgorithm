namespace Genethic_Algorithm
{
    internal abstract class Specimen
    {
        internal abstract Specimen[] Crossover(Specimen partner);

        internal abstract Specimen Mutate();

        internal abstract double Evaluate(Problem problem);
    }
}