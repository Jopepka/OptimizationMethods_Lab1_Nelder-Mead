using System;
using System.Collections.Generic;
using Vector;

namespace Nelder_Mead
{
    public class NelderMead
    {
        public double Alfa { get; }
        public double Betta { get; }
        public double Gamma { get; }
        public int MaxSteps { get; }
        public double? MinDispersion { get; }
        public List<VectorM>? StartSimplex { get; private set; }
        public List<VectorM> NowSimplex { get; private set; }

        public CalculationFunction Function { get; private set; }

        public int Steps { get; private set; }
        public double Dispersion { get; private set; }
        public int Dimention { get; }

        public NelderMead(double alfa = 1, double betta = 0.5, double gamma = 2,
            int maxSteps = 100, double? dispersion = null)
        {
            Alfa = alfa;
            Betta = betta;
            Gamma = gamma;

            MaxSteps = maxSteps;
            MinDispersion = dispersion;
        }

        public void Fit(CalculationFunction calculationFunction, List<VectorM> startSimplex)
        {
            Function = calculationFunction;
            StartSimplex = startSimplex;
        }

        public VectorM? Run(IIterationNM algorithm)
        {
            if (StartSimplex is null)
                return null;

            List<FuncValue> funcValues = new List<FuncValue>() { };
            foreach (VectorM vector in StartSimplex)
                funcValues.Add(new FuncValue(vector, Function));

            int steps = 0;
            while ((Dispersion = DispersionVectors(funcValues)) > MinDispersion || steps < MaxSteps)
            {
                steps++;
                funcValues = algorithm.RunIteration(funcValues, this);
            }
            Steps = steps;
            return funcValues[0].X;
        }

        internal FuncValue CentreGravity(List<VectorM> vectors)
        {
            VectorM sumVect = new VectorM(vectors[0].Count);
            foreach (VectorM vector in vectors)
                sumVect += vector;
            return new FuncValue(sumVect / vectors.Count, Function);
        }

        internal FuncValue Reflection(FuncValue worst, FuncValue centreG)
        {
            VectorM reflectedVector = (1 + Alfa) * centreG.X - Alfa * worst.X;
            FuncValue reflected = new FuncValue(reflectedVector, Function);
            return reflected;
        }

        internal FuncValue Expansion(FuncValue reflected, FuncValue centreG)
        {
            VectorM expansion_vect = (1 - Gamma) * centreG.X + Gamma * reflected.X;
            FuncValue expansion = new FuncValue(expansion_vect, Function);
            return expansion;
        }

        internal FuncValue Compression(FuncValue worst, FuncValue centreG)
        {
            VectorM s = Betta * worst.X + (1 - Betta) * centreG.X;
            return new FuncValue(s, Function);
        }

        internal List<FuncValue> GlobalCompression(List<FuncValue> funcValues)
        {
            List<FuncValue> newFuncValues = new List<FuncValue>();
            newFuncValues.Add(funcValues[0]);
            for (int i = 1; i < funcValues.Count; i++)
            {
                VectorM newVectX = new VectorM(funcValues[0].X + (funcValues[i].X - funcValues[0].X) / 2);
                newFuncValues.Add(new FuncValue(newVectX, Function));
            }
            return newFuncValues;
        }

        private double DispersionVectors(List<FuncValue> funcValues)
        {
            VectorM averageX = new VectorM(funcValues[0].X.Count);
            foreach (FuncValue funcValue in funcValues)
                averageX += funcValue.X;
            averageX /= funcValues.Count;

            double dispersion = 0;
            foreach (FuncValue funcValue in funcValues)
            {
                dispersion += (funcValue.X - averageX) * (funcValue.X - averageX);
            }
            dispersion /= funcValues.Count;
            return dispersion;
        }

        public static List<VectorM> CreatureSimplex(int dimension, double sizeSimplex = 1,
    VectorM? startVector = null)
        {
            VectorM? firstVectorSimplex = null;
            if (startVector is null)
                firstVectorSimplex = new VectorM(dimension);
            else
            {
                if (startVector.Size != dimension)
                    throw new ArgumentException($"dimension {dimension} and size startVector {startVector.Size} must be equal");
                firstVectorSimplex = startVector;
            }
            List<VectorM> baseVectors = new List<VectorM>();
            for (int i = 0; i < dimension; i++)
            {
                double[] doublesForVect = new double[dimension];
                for (int j = 0; j < dimension; j++)
                    doublesForVect[j] = 0;
                doublesForVect[i] = 1;

                baseVectors.Add(new VectorM(doublesForVect));
            }

            List<VectorM> simplex = new List<VectorM> { firstVectorSimplex };
            for (int i = 0; i < dimension; i++)
            {
                VectorM newVector = firstVectorSimplex + baseVectors[i] * sizeSimplex;
                simplex.Add(newVector);
            }

            return simplex;
        }
    }
}

public delegate double CalculationFunction(VectorM X);
