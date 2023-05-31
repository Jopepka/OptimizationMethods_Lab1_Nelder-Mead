using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using Vector;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Nelder_Mead
{
    public class Simplex
    {
        public Pair Best { get => pairs[0]; }
        public Pair Good { get => pairs[^2]; }
        public Pair Worst
        {
            get => pairs[^1];
            set
            {
                pairs[^1] = value;
                Initializing();
            }
        }

        public Pair CentreG { get; private set; }
        public Pair Reflected { get; private set; }
        public Pair Extended { get => Expansion(); }

        public double Alfa { get; } = 1;
        public double Betta { get; } = 0.5;
        public double Gamma { get; } = 2;

        public CalculationFunction Calculation { get; }
        public List<Pair> pairs { get; private set; }

        public Simplex(CalculationFunction function, List<VectorM> vectorsSimplex, double alfa = 1, double betta = 0.5, double gamma = 2)
        {
            Calculation = function;
            Alfa = alfa;
            Betta = betta;
            Gamma = gamma;

            CreateSimplex(vectorsSimplex);
        }

        public Simplex(CalculationFunction function, int dimension, double sizeSimplex = 1,
        VectorM? startVector = null, double alfa = 1, double betta = 0.5, double gamma = 2)
        {
            Calculation = function;
            Alfa = alfa;
            Betta = betta;
            Gamma = gamma;

            CreateSimplex(dimension, sizeSimplex, startVector);
        }

        private void CreateSimplex(List<VectorM> VectorsSimplex)
        {
            pairs = new List<Pair>(VectorsSimplex.Count);
            CreatePairs(VectorsSimplex);
        }

        private void CreateSimplex(int dimension, double sizeSimplex = 1,
        VectorM? startVector = null)
        {
            VectorM firstVector = startVector is null ? new VectorM(dimension) : startVector;

            List<VectorM> vectorsSimplex = new List<VectorM>(dimension + 1) { firstVector };
            for (int i = 0; i < dimension; i++)
            {
                double[] doublesForVect = new double[dimension];
                for (int j = 0; j < dimension; j++)
                    doublesForVect[j] = 0;
                doublesForVect[i] = 1 * sizeSimplex;

                vectorsSimplex.Add(firstVector + new VectorM(doublesForVect));
            }

            CreatePairs(vectorsSimplex);
        }

        public Pair CentreGravity()
        {
            VectorM centreG = new VectorM(pairs[0].X.Count);

            for (int i = 0; i < pairs.Count - 1; i++)
                centreG += pairs[i].X;

            centreG /= pairs.Count - 1;

            return new Pair(centreG, Calculation(centreG));
        }

        public Pair Reflection()
        {
            VectorM reflected = (1 + Alfa) * CentreG.X - Alfa * Worst.X;
            return new Pair(reflected, Calculation(reflected));
        }

        public Pair Expansion()
        {
            VectorM expansion = (1 - Gamma) * CentreG.X + Gamma * Reflected.X;
            return new Pair(expansion, Calculation(expansion));
        }

        public Pair Compression(Pair relativeToCompress)
        {
            VectorM compress = Betta * relativeToCompress.X + (1 - Betta) * CentreG.X;
            return new Pair(compress, Calculation(compress));
        }

        public Simplex GlobalCompression()
        {
            List<VectorM> newVectors = new List<VectorM> { pairs[0].X };

            for (int i = 1; i < pairs.Count; i++)
            {
                VectorM newVectX = new VectorM(pairs[0].X + (pairs[i].X - pairs[0].X) / 2);
                newVectors.Add(newVectX);
            }

            return new Simplex(Calculation, newVectors);
        }

        public double Dispersion()
        {
            VectorM averageX = new VectorM(pairs[0].X.Count);
            double dispersion = 0;

            foreach (Pair pair in pairs)
                averageX += pair.X;
            averageX /= pairs.Count;

            foreach (Pair pair in pairs)
                dispersion += (pair.X - averageX) * (pair.X - averageX);
            dispersion /= pairs.Count;

            return dispersion;
        }

        private void CreatePairs(List<VectorM> vectors)
        {
            pairs = new List<Pair>();
            foreach (VectorM vector in vectors)
                pairs.Add(new Pair(vector, Calculation(vector)));

            Initializing();
        }

        private void Initializing()
        {
            Sort();
            CentreG = CentreGravity();
            Reflected = Reflection();
        }

        private void Sort()
        {
            pairs.Sort((left, right) => left.Y.CompareTo(right.Y));
        }
    }
}
