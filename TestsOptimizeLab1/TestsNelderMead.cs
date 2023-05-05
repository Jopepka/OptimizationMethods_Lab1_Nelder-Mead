using Nelder_Mead;
using OptimizationMethods_Lab1_Nelder_Mead;
using Vector;

namespace TestsOptimizeLab1
{
    [TestClass]
    public class TestsNelderMead
    {
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

            NelderMead nm = new NelderMead();

            nm.SetSimplex(new Simplex(funcTest, simplex));

            Pair res = nm.Run();

            double trueResY = -21;
            double delta = 0.001;

            Assert.AreEqual(trueResY, res.Y, delta);
        }

        [TestMethod]
        public void TestRes2()
        {
            simplex = new List<VectorM>()
            {
                new VectorM(new double[]{-2}),
                new VectorM(new double[] {100})
            };

            NelderMead nm = new NelderMead();
            nm.SetSimplex(new Simplex(x => x * x, simplex));

            Pair res = nm.Run();
            int steps1 = nm.Steps;

            double trueResY = 0;
            double delta = 0.001;
            Assert.AreEqual(trueResY, res.Y, delta);
        }

        public class Resultation
        {
            public NelderMead Nm { get; }
            public Simplex _Simplex { get; }
            public Pair _res { get; }

            public Resultation(NelderMead nm, Simplex simplex, Pair res)
            {
                Nm = nm;
                _Simplex = simplex;
                _res = res;
            }
        }
        public Resultation StartTest(string str_func, List<VectorM>? vectorSimplex = null, double? dispersion = null, IDoSomefing? doSomefings = null)
        {
            SomeFunction someFunction = new SomeFunction(str_func, str_func);
            CalculationFunction calculate = someFunction.GetValue;

            Simplex simplex = vectorSimplex is not null ?
                new Simplex(calculate, vectorSimplex) :
                new Simplex(calculate, 2);
            NelderMead nm = new NelderMead(dispersion: dispersion);
            nm.SetSimplex(simplex);

            Pair res = nm.Run(doSomefings);

            return new Resultation(nm, simplex, res);
        }

        [TestMethod]
        public void TestRes3()
        {
            simplex = new List<VectorM>()
                    {
                        new VectorM(new double[]{-2}),
                        new VectorM(new double[]{100})
                    };

            string str_func = "x^2";

            double trueResY = 0;
            double delta = 0.001;

            Resultation resRun = StartTest(str_func, simplex);

            Assert.AreEqual(trueResY, resRun._res.Y, delta);
        }

        [TestMethod]
        public void TestRes4()
        {
            string str_func = "(1 - x)^2 + 100(y - x^2)^2";

            double trueResY = 0;
            double delta = 0.001;

            Resultation resRun = StartTest(str_func);

            Assert.AreEqual(trueResY, resRun._res.Y, delta);
        }

        [TestMethod]
        public void TestRes5()
        {
            string str_func = "(x^2 + y - 11)^2 + (x + y^2 - 7)^2";

            double trueResY = 0;
            double delta = 0.001;

            Resultation resRun = StartTest(str_func);

            Assert.AreEqual(trueResY, resRun._res.Y, delta);
        }

        [TestMethod]
        public void TestRes6()
        {
            string str_func = "(x^2 + y - 11)^2 + (x + y^2 - 7)^2";
            //simplex = NelderMead.CreatureSimplex(2, startVector: new VectorM(new double[] { -2.0, 3 }));
            double trueResY = 0;
            double delta = 0.001;

            Resultation resRun = StartTest(str_func);

            Assert.AreEqual(trueResY, resRun._res.Y, delta);
        }

        [TestMethod]
        public void TestDispersion1()
        {
            string str_func = "(x^2 + y - 11)^2 + (x + y^2 - 7)^2";
            double dispersion = 0.001;

            Resultation resRun = StartTest(str_func, dispersion: dispersion);

            Assert.IsTrue(dispersion >= resRun.Nm.Dispersion);
        }

        [TestMethod]
        public void TestSteps1()
        {
            simplex = new List<VectorM>()
            {
                new VectorM(new double[]{-2}),
                new VectorM(new double[] {100})
            };

            string str_func = "x^2";

            Resultation ResRun = StartTest(str_func, simplex);

            Assert.AreEqual(ResRun.Nm.Steps, ResRun.Nm.MaxSteps);
            Assert.AreEqual(ResRun.Nm.Steps, ResRun.Nm.MaxSteps);
        }

        [TestMethod]
        public void TestConsoleWrite()
        {
            string str_func = "(x^2 + y - 11)^2 + (x + y^2 - 7)^2";
            DoPrintConsole printToConsole = new DoPrintConsole();

            Resultation ResRun = StartTest(str_func, doSomefings: printToConsole);

            Assert.IsNotNull(printToConsole.Log);
            Assert.AreNotEqual("", printToConsole.Log);
        }
    }
}