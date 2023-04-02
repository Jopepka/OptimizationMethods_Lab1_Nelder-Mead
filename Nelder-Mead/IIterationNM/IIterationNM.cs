using System.Collections.Generic;

namespace Nelder_Mead
{
    public interface IIterationNM
    {
        List<FuncValue> RunIteration(List<FuncValue> funcValues, NelderMead nm);
    }
}
