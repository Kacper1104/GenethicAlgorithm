namespace Genethic_Algorithm
{
    internal interface ISpecimen
    {
        Specimen[] Crossover(Specimen specimen1, Specimen specimen2);

        Specimen Mutate();

        double Evaluate();
    }
}