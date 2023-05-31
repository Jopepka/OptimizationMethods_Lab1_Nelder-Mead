using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using Vector;

namespace Nelder_Mead
{
    public class NelderMead
    {
        public int MaxSteps { get; }
        public double? MinDispersion { get; }
        public Simplex StartSimplex { get; private set; }

        public int Steps { get; private set; }
        public double Dispersion { get; private set; }
        public int Dimention { get; }

        public NelderMead(int maxSteps = 100, double? dispersion = null)
        {
            MaxSteps = maxSteps;
            MinDispersion = dispersion;
        }

        public void SetSimplex(Simplex startSimplex)
        {
            StartSimplex = startSimplex;
        }

        public Pair Run(IDoSomefing? doSomefings = null)
        {
            if (StartSimplex is null)
                throw new ArgumentNullException("The simplex must not be null");

            int steps = 0;
            Simplex simplex = StartSimplex;
            while (((Dispersion = simplex.Dispersion()) > MinDispersion || MinDispersion == null) && steps < MaxSteps)
            {
                steps++;
                doSomefings?.Do(simplex);
                simplex = RunIteration(simplex);
            }

            Steps = steps;
            return simplex.Best;
        }

        private Simplex RunIteration(Simplex simplex)
        {
            if (simplex.Reflected < simplex.Best)
            {
                simplex.Worst = simplex.Extended < simplex.Reflected ? simplex.Extended : simplex.Reflected;
            }
            else if (simplex.Reflected < simplex.Good)
                simplex.Worst = simplex.Reflected;
            else
            {
                Pair whoCompress = simplex.Reflected < simplex.Worst ? simplex.Reflected : simplex.Worst;
                Pair compress = simplex.Compression(whoCompress);
                if (compress < whoCompress)
                    simplex.Worst = compress;
                else
                    simplex = simplex.GlobalCompression();
            }

            return simplex;
        }
    }
}

public delegate double CalculationFunction(VectorM X);
