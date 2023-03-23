using System.Numerics;
using System;
using Vector;
using Nelder_Mead;

namespace TestsOptimizeLab1
{
    [TestClass]
    public class UnitTest1
    {
        double alfa = 1;
        double betta = 0.5;
        double gamma = 2;
        int dim = 2;
        int step = 2000;
        double dispersion = 0.001;
        List<VectorM> simplex;

        public double funcTest(VectorM vector)
        {
            double x = vector[0];
            double y = vector[1];
            double res = x * x + x * y + y * y - 6 * x - 9 * y;
            return res;
        }

        [TestMethod]
        public void TestRes1()
        {
            simplex = new List<VectorM>()
            { 
                new VectorM(new double[]{0, 0}),
                new VectorM(new double[]{1, 0}),
                new VectorM(new double[]{1, 1})
            };

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm(alfa, betta, gamma,
                dim, funcTest, maxSteps: step, simplex: simplex, dispersion: dispersion);
            VectorM res1 = nm.Run();
            double resY1 = funcTest(res1);
            int steps1 = nm.Steps;

            VectorM res2 = nm.NewRun();
            double resY2 = funcTest(res2);
            int steps2 = nm.Steps;
        }

        [TestMethod]
        public void TestRes2()
        {
            simplex = new List<VectorM>()
            {
                new VectorM(new double[]{-2}),
                new VectorM(new double[] {100})
            };

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm(alfa, betta, gamma,
                dim, x => x*x, 30, simplex: simplex, dispersion: dispersion);
            VectorM res1 = nm.Run();
            double resY1 = res1[0]*res1[0];
            int steps1 = nm.Steps;

            VectorM res2 = nm.NewRun();
            double resY2 = res2[0] * res2[0];
            int steps2 = nm.Steps;
        }

    }
}