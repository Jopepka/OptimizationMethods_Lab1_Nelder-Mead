using System.Collections.Generic;
using System.Linq;

namespace Nelder_Mead
{
    public class RunWikiEng : IIterationNM
    {
        public List<FuncValue> RunIteration(List<FuncValue> funcValues, NelderMead nm)
        {
            funcValues.Sort((left, right) => left.Y.CompareTo(right.Y));

            FuncValue best = funcValues[0];
            FuncValue good = funcValues[^2];
            FuncValue worst = funcValues[^1];
            FuncValue centreG = nm.CentreGravity(funcValues.GetRange(0, funcValues.Count - 1).Select(v => v.X).ToList());
            FuncValue reflected = nm.Reflection(worst, centreG);

            if (reflected.Y < best.Y)
            {
                FuncValue expansion = nm.Expansion(reflected, centreG);
                funcValues[^1] = expansion.Y < reflected.Y ? expansion : reflected;
            }
            else if (reflected.Y < good.Y)
                funcValues[^1] = reflected;
            else
            {
                FuncValue whoCompress = reflected.Y < worst.Y ? reflected : worst;
                FuncValue compress = nm.Compression(whoCompress, centreG);
                if (compress.Y < whoCompress.Y)
                    funcValues[^1] = compress;
                else
                    funcValues = nm.GlobalCompression(funcValues);
            }

            return funcValues;
        }
    }
}
