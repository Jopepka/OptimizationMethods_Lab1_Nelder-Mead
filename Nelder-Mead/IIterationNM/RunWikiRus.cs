using System.Collections.Generic;
using System.Linq;
using Vector;

namespace Nelder_Mead
{
    public class RunWikiRus : IIterationNM
    {
        public List<FuncValue> RunIteration(List<FuncValue> funcValues, NelderMead nm)
        {
            funcValues.Sort((left, right) => left.Y.CompareTo(right.Y));

            FuncValue best = funcValues[0];
            FuncValue good = funcValues[funcValues.Count - 2];
            FuncValue worst = funcValues[funcValues.Count - 1];
            FuncValue centreG = nm.CentreGravity(funcValues.GetRange(0, funcValues.Count - 1).Select(v => v.X).ToList());
            FuncValue reflected = nm.Reflection(worst, centreG);

            if (reflected.Y < best.Y)
            {
                VectorM eVect = (1 - nm.Gamma) * centreG.X + nm.Gamma * reflected.X;
                FuncValue e = new FuncValue(eVect, nm.Function);

                worst = e.Y < reflected.Y ? e : reflected;
                funcValues[funcValues.Count - 1] = worst;
            }
            else if (good.Y < reflected.Y && reflected.Y < worst.Y)
            {
                worst = reflected;
                funcValues[funcValues.Count - 1] = worst;
            }
            else
            {
                if (good.Y < reflected.Y && reflected.Y < worst.Y)
                {
                    FuncValue bufToSwap = reflected;
                    reflected = worst;
                    worst = bufToSwap;
                }
                FuncValue s = nm.Compression(funcValues.Last(), centreG);
                if (s.Y < funcValues[^1].Y)
                    funcValues[^1] = s;
                else if (s.Y > funcValues[^1].Y)
                    funcValues = nm.GlobalCompression(funcValues);
            }

            return funcValues;
        }
    }
}
