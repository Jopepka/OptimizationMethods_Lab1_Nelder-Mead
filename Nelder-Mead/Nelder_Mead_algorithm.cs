using System;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;
using Vector;
using System.IO.Compression;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime;

namespace Nelder_Mead
{
    public class Nelder_Mead_algorithm
    {
        public class FuncValue
        {
            public FuncValue(double Y, VectorM X, Func? func = null)
            {
                this.Y = Y;
                this.X = X;

                this.func = func;
            }

            public FuncValue(VectorM X, Func func):this(func(X), X, func) { }

            public double Y {get; }
            public VectorM X { get;}

            Func? func { get; }
        }

        public double Alfa { get;}
        public double Betta { get;}
        public double Gamma { get;}
        public int? Dim { get;}
        public Func Function { get; private set;}

        public int MaxSteps { get; }
        public double? Dispersion { get; }
        public List<VectorM>? StartSimplex { get; private set;}
        public int Steps{get; private set;}


        public Nelder_Mead_algorithm(double alfa, double betta, double gamma, 
            int dimension, Func func, int maxSteps = 1000,
            List<VectorM>? simplex = null, double ? dispersion = null)
        {
            this.Alfa = alfa;
            this.Betta = betta;
            this.Gamma = gamma;
            Dim = dimension;
            Function = func;

            MaxSteps = maxSteps;
            Dispersion = dispersion;
            StartSimplex = simplex;


        }
        public VectorM Run()
        {
            List<VectorM> vectors;
            if (StartSimplex is null)
            {
                vectors = CriateRandomPoints();
                StartSimplex = vectors;
            }
            else 
                vectors = StartSimplex;
            
            List<FuncValue> funcValues = new List<FuncValue>() { };

            foreach (VectorM vector in vectors)
            {
                funcValues.Add(new FuncValue(vector, Function));
            }

            int steps = 0;
            while (steps < MaxSteps)
            {
                steps++;

                funcValues.Sort((left, right) => left.Y.CompareTo(right.Y));
                FuncValue best = funcValues[0],
                    good = funcValues[funcValues.Count-2],
                    worst = funcValues[funcValues.Count - 1];

                VectorM x_centreG = CentreGravity(funcValues.GetRange(0, funcValues.Count - 1).Select(v => v.X).ToList());
                FuncValue centreG = new FuncValue(x_centreG, Function);

                FuncValue reflected = Reflection(worst, centreG);
                /////

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
                {
                    goStep = 6;
                }

                funcValues[funcValues.Count-1] = worst;

                if (goStep == 6)
                {
                    FuncValue s = Compression(funcValues.Last(), centreG);

                    if (s.Y < funcValues.Last().Y)
                    {
                        funcValues[funcValues.Count() - 1] = s;
                    }
                    else if (s.Y > funcValues.Last().Y)
                    {
                        funcValues = GlobalCompression(funcValues);
                    }
                }

                if (ProximityAssessment(funcValues) < Dispersion || steps > MaxSteps)
                {
                    break;
                }
            }
            Steps = steps;
            return funcValues[0].X;
        }
        public VectorM NewRun()
        {
            int steps = 0;
            List<VectorM> vectors;

            //Если начальный симплекс не задан, то создаем
            if (StartSimplex is null)
            {
                vectors = CriateRandomPoints();
                StartSimplex = vectors;
            }
            else
                vectors = StartSimplex;

            List<FuncValue> funcValues = new List<FuncValue>() { };
            foreach (VectorM vector in vectors)
            {
                funcValues.Add(new FuncValue(vector, Function));
            }

            while (true)
            {
                steps++;

                //(1) Сортируем точки по их значениям
                funcValues.Sort((left, right) => left.Y.CompareTo(right.Y));

                //Выбираем точки из симплекса
                FuncValue best = funcValues[0],
                    good = funcValues[funcValues.Count - 2],
                    worst = funcValues[funcValues.Count - 1];

                //(2) Находим центр тяжести
                VectorM centreG_vect = CentreGravity(funcValues.GetRange(0, funcValues.Count - 1).Select(v => v.X).ToList());
                FuncValue centreG = new FuncValue(centreG_vect, Function);

                //(3) Находим отраженную точку
                FuncValue reflected = Reflection(worst, centreG);
                //Если отраженная точка находится между лучшей и хорошей
                if (best.Y <= reflected.Y && reflected.Y < good.Y)
                {
                    funcValues[funcValues.Count-1] = reflected;
                }
                //(4) Если отраженная точка лучшая, то 
                else if (reflected.Y < best.Y)
                {
                    //Попробуем растянуть
                    FuncValue expansion = Expansion(reflected, centreG);
                    if (expansion.Y < reflected.Y)
                    {
                        funcValues[funcValues.Count - 1] = expansion;
                    }
                    else
                    {
                        funcValues[funcValues.Count - 1] = reflected;
                    }
                }
                //(5) Отраженная точка точно хуже хорошей
                else
                {
                    //Если отраженная лучше худшей, то попробуем сжать наружу
                    if (reflected.Y < worst.Y)
                    {
                        FuncValue compress = Compression(reflected, centreG);
                        if (compress.Y < reflected.Y)
                        {
                            funcValues[funcValues.Count - 1] = compress;
                        }
                        //(6) Если не получилось, то глобальное сжатие
                        else
                        {
                            funcValues = GlobalCompression(funcValues);
                        }
                    }
                    // Если отраженная хуже худшей, то попробуем сжать внутрь
                    else if(reflected.Y >= worst.Y)
                    {
                        FuncValue compress = Compression(worst, centreG);
                        if (compress.Y < worst.Y)
                        {
                            funcValues[funcValues.Count-1] = compress;
                        }
                        //(6) Если не получилось, то глобальное сжатие
                        else
                        {
                            funcValues = GlobalCompression(funcValues);
                        }
                    }
                }

                //Проверяем на удовлетворение точности
                if (ProximityAssessment(funcValues) < Dispersion || steps > MaxSteps)
                {
                    break;
                }
            } //конец while(true)
            Steps = steps;
            return funcValues[0].X;
        }

        private List<VectorM> CriateRandomPoints()
        {
            Random rd = new Random();
            return null;
        }

        private VectorM CentreGravity(List<VectorM> vectors)
        {
            VectorM sumVect = new VectorM(vectors[0].Count);
            foreach (VectorM vector in vectors)
            {
                sumVect += vector;
            }

            return sumVect/vectors.Count;
        }

        private FuncValue Reflection(FuncValue worst, FuncValue centreG)
        {
            VectorM reflectedVector = (1+Alfa)*centreG.X - Alfa*worst.X;
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
            VectorM s = Betta*worst.X + (1-Betta)*centreG.X;
            return new FuncValue(s, Function);
        }
        private List<FuncValue> GlobalCompression(List<FuncValue> funcValues)
        {
            List<FuncValue> newFuncValues = new List<FuncValue>();
            for(int i = 1; i < funcValues.Count; i++)
            {
                VectorM newVectX = new VectorM(funcValues[0].X + (funcValues[i].X - funcValues[0].X)/2);
                newFuncValues.Add(new FuncValue(newVectX, Function));
            }
            return newFuncValues;
        }

        private double ProximityAssessment(List<FuncValue> funcValues)
        {
            double sumY = 0;
            foreach (FuncValue funcValue in funcValues)
            {
                sumY += funcValue.Y;
            }
            double averageY = sumY/funcValues.Count;

            double buf = 0;
            foreach(FuncValue funcValue in funcValues)
            {
                buf = (funcValue.Y - averageY)*(funcValue.Y - averageY);
            }
            return buf/(funcValues.Count);
        }

    }

    public delegate double Func(VectorM X);
}
