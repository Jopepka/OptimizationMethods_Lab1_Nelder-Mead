using System;
using System.Collections.Generic;
using System.Linq;
using Vector;

namespace Nelder_Mead
{
    public class Nelder_Mead_algorithm
    {
        public class FuncValue
        {
            public FuncValue(double Y, VectorM X, CalculationFunction? func = null)
            {
                this.Y = Y;
                this.X = X;

                this.func = func;
            }

            public FuncValue(VectorM X, CalculationFunction func) : this(func(X), X, func) { }

            public double Y { get; }
            public VectorM X { get; }

            CalculationFunction? func { get; }
        }
        //ѕараметры непосредственно вли€ющие на алгоритм
        public double Alfa { get; }
        public double Betta { get; }
        public double Gamma { get; }
        public int MaxSteps { get; }
        public double? MinDispersion { get; }
        public List<VectorM>? StartSimplex { get; private set; }

        //‘ункци€ рассчета критери€
        public CalculationFunction Function { get; private set; }

        //¬нутренние параметры
        public int Steps { get; private set; }
        public double Dispersion { get; private set; }
        public int Dimention { get; }

        public Nelder_Mead_algorithm(double alfa = 1, double betta = 0.5, double gamma = 2,
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

        public List<VectorM> CreatureSimplex(int dimension, double sizeSimplex = 1, VectorM? startVector = null)
        {
            VectorM? firstVectorSimplex = null;
            if (startVector is null)
            {
                firstVectorSimplex = new VectorM(dimension);
            }
            else
            {
                if (startVector.Size == dimension)
                {
                    firstVectorSimplex = startVector;
                }
                else
                {
                    throw new ArgumentException($"dimension {dimension} and size startVector {startVector.Size} must be equal");
                }
            }
            List<VectorM> baseVectors = new List<VectorM>();
            for (int i = 0; i < dimension; i++)
            {
                VectorM baseVector = new VectorM(dimension);
                baseVector[i] += 1;
                baseVectors.Add(baseVector);
            }

            List<VectorM> simplex = new List<VectorM> { firstVectorSimplex };
            for (int i = 0; i < dimension; i++)
            {
                VectorM newVector = firstVectorSimplex + baseVectors[i] * sizeSimplex;
                simplex.Add(newVector);
            }
            return simplex;
        }

        public VectorM? Run()
        {
            if (StartSimplex is null)
            {
                return null;
            }

            List<FuncValue> funcValues = new List<FuncValue>() { };

            foreach (VectorM vector in StartSimplex)
            {
                funcValues.Add(new FuncValue(vector, Function));
            }

            int steps = 0;
            while (steps < MaxSteps)
            {
                steps++;

                funcValues.Sort((left, right) => left.Y.CompareTo(right.Y));

                FuncValue
                    best = funcValues[0],
                    good = funcValues[funcValues.Count - 2],
                    worst = funcValues[funcValues.Count - 1];

                FuncValue centreG = CentreGravity(funcValues.GetRange(0, funcValues.Count - 1).Select(v => v.X).ToList());

                FuncValue reflected = Reflection(worst, centreG);

                int goStep = 6;
                VectorM eVect;
                if (reflected.Y < best.Y)
                {
                    eVect = (1 - Gamma) * centreG.X + Gamma * reflected.X;
                    FuncValue e = new FuncValue(eVect, Function);

                    if (e.Y < reflected.Y)
                    {
                        worst = e;
                        goStep = 9;
                    }
                    if (reflected.Y < e.Y)
                    {
                        worst = reflected;
                        goStep = 9;
                    }
                }
                else if (good.Y < reflected.Y && reflected.Y < worst.Y)
                {
                    worst = reflected;
                    goStep = 9;
                }
                else if (good.Y < reflected.Y && reflected.Y < worst.Y)
                {
                    FuncValue bufToSwap = reflected;
                    reflected = worst;
                    worst = bufToSwap;
                    goStep = 6;
                }
                else if (worst.Y < reflected.Y)
                    goStep = 6;

                funcValues[funcValues.Count - 1] = worst;

                if (goStep == 6)
                {
                    FuncValue s = Compression(funcValues.Last(), centreG);
                    if (s.Y < funcValues.Last().Y)
                        funcValues[funcValues.Count() - 1] = s;
                    else if (s.Y > funcValues.Last().Y)
                        funcValues = GlobalCompression(funcValues);
                }

                if ((Dispersion = DispersionVectors(funcValues)) < MinDispersion || steps >= MaxSteps)
                    break;
            }

            Steps = steps;
            return funcValues[0].X;
        }

        public VectorM? RunV2()
        {
            if (StartSimplex is null)
                return null;

            List<FuncValue> funcValues = new List<FuncValue>() { };

            foreach (VectorM vector in StartSimplex)
            {
                funcValues.Add(new FuncValue(vector, Function));
            }

            int steps = 0;
            while (true)
            {
                steps++;

                funcValues.Sort((left, right) => left.Y.CompareTo(right.Y));

                FuncValue best = funcValues[0],
                    good = funcValues[funcValues.Count - 2],
                    worst = funcValues[funcValues.Count - 1];

                FuncValue centreG = CentreGravity(funcValues.GetRange(0, funcValues.Count - 1).Select(v => v.X).ToList());

                FuncValue reflected = Reflection(worst, centreG);
                if (best.Y <= reflected.Y && reflected.Y < good.Y)
                    funcValues[funcValues.Count - 1] = reflected;
                else if (reflected.Y < best.Y)
                {
                    FuncValue expansion = Expansion(reflected, centreG);
                    if (expansion.Y < reflected.Y)
                        funcValues[funcValues.Count - 1] = expansion;
                    else
                        funcValues[funcValues.Count - 1] = reflected;
                }
                else
                {
                    if (reflected.Y < worst.Y)
                    {
                        FuncValue compress = Compression(reflected, centreG);
                        if (compress.Y < reflected.Y)
                            funcValues[funcValues.Count - 1] = compress;
                        else
                            funcValues = GlobalCompression(funcValues);
                    }
                    else if (reflected.Y >= worst.Y)
                    {
                        FuncValue compress = Compression(worst, centreG);
                        if (compress.Y < worst.Y)
                            funcValues[funcValues.Count - 1] = compress;
                        else
                            funcValues = GlobalCompression(funcValues);
                    }
                }

                if ((Dispersion = DispersionVectors(funcValues)) < MinDispersion || steps >= MaxSteps)
                    break;
            } // while(true) end
            Steps = steps;
            return funcValues[0].X;
        }

        private FuncValue CentreGravity(List<VectorM> vectors)
        {
            VectorM sumVect = new VectorM(vectors[0].Count);
            foreach (VectorM vector in vectors)
                sumVect += vector;
            return new FuncValue(sumVect / vectors.Count, Function);
        }

        private FuncValue Reflection(FuncValue worst, FuncValue centreG)
        {
            VectorM reflectedVector = (1 + Alfa) * centreG.X - Alfa * worst.X;
            FuncValue reflected = new FuncValue(reflectedVector, Function);
            return reflected;
        }

        private FuncValue Expansion(FuncValue reflected, FuncValue centreG)
        {
            VectorM expansion_vect = (1 - Gamma) * centreG.X + Gamma * reflected.X;
            FuncValue expansion = new FuncValue(expansion_vect, Function);
            return expansion;
        }

        private FuncValue Compression(FuncValue worst, FuncValue centreG)
        {
            VectorM s = Betta * worst.X + (1 - Betta) * centreG.X;
            return new FuncValue(s, Function);
        }

        private List<FuncValue> GlobalCompression(List<FuncValue> funcValues)
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

    }
}

public delegate double CalculationFunction(VectorM X);
