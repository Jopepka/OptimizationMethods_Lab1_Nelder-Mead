using Vector;

namespace Nelder_Mead
{
    public class FuncValue
    {
        public FuncValue(VectorM x, CalculationFunction func)
        {
            X = x;
            Func = func;
            Y = Func(X);
        }


        public double Y { get; }
        public VectorM X { get; }

        CalculationFunction? Func { get; }
    }
}
