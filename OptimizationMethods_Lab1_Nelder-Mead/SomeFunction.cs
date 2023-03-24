using System.Linq;
using System.Numerics;
using AngouriMath;
using AngouriMath.Core;
using Vector;

namespace OptimizationMethods_Lab1_Nelder_Mead
{
    public class SomeFunction
    {
        public string Function { get; }
        public string Name { get; }
        public Entity Expr { get; }
        public Entity.Variable[] Variables { get; }
        public FastExpression compl { get; }

        public SomeFunction(string function, string name)
        {
            Function = function;
            Name = name;

            Expr = function;
            Variables = Expr.Vars.ToArray();
            compl = Expr.Compile(Variables);
        }

        public double GetValue(VectorM vector_x)
        {
            return compl.Call(Complexes_x(vector_x)).Real;
        }

        private Complex[] Complexes_x(VectorM vector_x)
        {
            Complex[] complexes_x = new Complex[vector_x.Count];
            for (int i = 0; i < vector_x.Count; i++)
                complexes_x[i] = new Complex(vector_x[i], 0);

            return complexes_x;
        }
    }
}
