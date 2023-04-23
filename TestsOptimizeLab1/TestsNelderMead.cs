using Nelder_Mead;
using OptimizationMethods_Lab1_Nelder_Mead;
using Vector;

namespace TestsOptimizeLab1
{
    [TestClass]
    public class TestsNelderMead
    {
        List<VectorM> simplex;

        IIterationNM iterationRu = new RunWikiRus();
        IIterationNM iterationEng = new RunWikiEng();

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

            nm.Fit(funcTest, simplex);

            VectorM res1 = nm.Run(iterationRu);
            double resY1 = funcTest(res1);

            VectorM res2 = nm.Run(iterationEng);
            double resY2 = funcTest(res2);

            double trueResY = -21;
            double delta = 0.001;

            Assert.AreEqual(trueResY, resY1, delta);
            Assert.AreEqual(trueResY, resY2, delta);
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
            nm.Fit(x => x * x, simplex);

            VectorM res1 = nm.Run(iterationRu);
            double resY1 = res1[0] * res1[0];
            int steps1 = nm.Steps;

            VectorM res2 = nm.Run(iterationEng);
            double resY2 = res2[0] * res2[0];
            int steps2 = nm.Steps;

            double trueResY = 0;
            double delta = 0.001;
            Assert.AreEqual(trueResY, resY1, delta);
            Assert.AreEqual(trueResY, resY2, delta);
        }

        public class ResultationRun
        {
            public NelderMead Nm { get; }
            public List<VectorM> StartSimplex { get; }
            public VectorM X1 { get; }
            public VectorM X2 { get; }
            public double Y1 { get; }
            public double Y2 { get; }

            public ResultationRun(NelderMead nm, List<VectorM> startSimplex, VectorM x1, VectorM x2)
            {
                Nm = nm;
                StartSimplex = startSimplex;
                X1 = x1;
                X2 = x2;

                Y1 = Nm.Function(X1);
                Y2 = Nm.Function(X2);
            }
        }

        public ResultationRun StartTest(string str_func, List<VectorM> simplex, List<IDoSomefing>? doSomefings = null)
        {
            SomeFunction someFunction = new SomeFunction(str_func, str_func);
            CalculationFunction calculate = someFunction.GetValue;

            NelderMead nm = new NelderMead();
            nm.Fit(calculate, simplex);

            VectorM res1 = nm.Run(iterationRu, doSomefings);
            VectorM res2 = nm.Run(iterationEng, doSomefings);

            return new ResultationRun(nm, simplex, res1, res2);
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

            ResultationRun ResRun = StartTest(str_func, simplex);

            Assert.AreEqual(trueResY, ResRun.Y1, delta);
            Assert.AreEqual(trueResY, ResRun.Y2, delta);
        }

        [TestMethod]
        public void TestRes4()
        {
            simplex = NelderMead.CreatureSimplex(2);

            string str_func = "(1 - x)^2 + 100(y - x^2)^2";

            double trueResY = 0;
            double delta = 0.001;

            ResultationRun ResRun = StartTest(str_func, simplex);

            Assert.AreEqual(trueResY, ResRun.Y1, delta);
            Assert.AreEqual(trueResY, ResRun.Y2, delta);
        }

        [TestMethod]
        public void TestRes5()
        {
            simplex = NelderMead.CreatureSimplex(2);

            string str_func = "(x^2 + y - 11)^2 + (x + y^2 - 7)^2";

            double trueResY = 0;
            double delta = 0.001;

            ResultationRun ResRun = StartTest(str_func, simplex);

            Assert.AreEqual(trueResY, ResRun.Y1, delta);
            Assert.AreEqual(trueResY, ResRun.Y2, delta);
        }

        [TestMethod]
        public void TestRes6()
        {
            string str_func = "(x^2 + y - 11)^2 + (x + y^2 - 7)^2";
            simplex = NelderMead.CreatureSimplex(2, startVector: new VectorM(new double[] { -2.0, 3 }));
            double trueResY = 0;
            double delta = 0.001;

            ResultationRun ResRun = StartTest(str_func, simplex);

            Assert.AreEqual(trueResY, ResRun.Y1, delta);
            Assert.AreEqual(trueResY, ResRun.Y2, delta);
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

            ResultationRun ResRun = StartTest(str_func, simplex);

            Assert.AreEqual(ResRun.Nm.Steps, ResRun.Nm.MaxSteps);
            Assert.AreEqual(ResRun.Nm.Steps, ResRun.Nm.MaxSteps);
        }

        [TestMethod]
        public void TestConsoleWrite()
        {
            string str_func = "(x^2 + y - 11)^2 + (x + y^2 - 7)^2";
            simplex = NelderMead.CreatureSimplex(2, startVector: new VectorM(new double[] { -2.0, 3 }));
            IDoSomefing printToConsole = new DoPrintConsole();

            ResultationRun ResRun = StartTest(str_func, simplex, new List<IDoSomefing> { printToConsole });
        }
    }
}