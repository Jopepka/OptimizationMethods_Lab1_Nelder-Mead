using Vector;

namespace Nelder_Mead
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
}
