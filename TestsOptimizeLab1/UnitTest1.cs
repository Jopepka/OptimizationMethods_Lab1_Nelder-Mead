using Nelder_Mead;
using OptimizationMethods_Lab1_Nelder_Mead;
using Vector;

namespace TestsOptimizeLab1
{
    [TestClass]
    public class UnitTest1
    {
        //Параметры для алгоритма Nelder-Mead
        double alfa = 1;
        double betta = 0.5;
        double gamma = 2;
        int dim = 2;
        int maxStep = 100;
        double dispersion = 0.001;
        List<VectorM> simplex;


        //Параметры для проверки:
        //accuracy: Отклонение от верного ответа
        //trueResY: Верный ответ
        double accuracy = 0.001;
        double trueResY;

        //Первая тестируема функция
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

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm(alfa, betta, gamma, maxSteps: maxStep);
            nm.Fit(funcTest, simplex);

            VectorM res1 = nm.Run();
            double resY1 = funcTest(res1);
            int steps1 = nm.Steps;

            VectorM res2 = nm.RunV2();
            double resY2 = funcTest(res2);
            int steps2 = nm.Steps;

            trueResY = -21;
            Assert.IsTrue(Math.Abs(resY1 - trueResY) < accuracy);
            Assert.IsTrue(Math.Abs(resY2 - trueResY) < accuracy);
            Assert.IsTrue(steps1 == maxStep);
            Assert.IsTrue(steps2 == maxStep);
        }

        [TestMethod]
        public void TestRes2()
        {
            simplex = new List<VectorM>()
            {
                new VectorM(new double[]{-2}),
                new VectorM(new double[] {100})
            };

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm(alfa, betta, gamma, maxStep);
            nm.Fit(x => x * x, simplex);

            VectorM res1 = nm.Run();
            double resY1 = res1[0] * res1[0];
            int steps1 = nm.Steps;

            VectorM res2 = nm.RunV2();
            double resY2 = res2[0] * res2[0];
            int steps2 = nm.Steps;

            trueResY = 0;
            Assert.IsTrue(Math.Abs(resY1 - trueResY) < accuracy);
            Assert.IsTrue(Math.Abs(resY2 - trueResY) < accuracy);
            Assert.IsTrue(steps1 == maxStep);
            Assert.IsTrue(steps2 == maxStep);
        }

        [TestMethod]
        public void TestRes3()
        {
            simplex = new List<VectorM>()
            {
                new VectorM(new double[]{-2}),
                new VectorM(new double[] {100})
            };

            string str_func = "x^2";
            SomeFunction someFunction = new SomeFunction(str_func, str_func);
            CalculationFunction del = someFunction.GetValue;

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm(alfa, betta, gamma, maxStep);
            nm.Fit(del, simplex);
            VectorM res1 = nm.Run();
            double resY1 = del(res1);
            int steps1 = nm.Steps;

            VectorM res2 = nm.RunV2();
            double resY2 = del(res2);
            int steps2 = nm.Steps;

            trueResY = 0;
            Assert.IsTrue(Math.Abs(resY1 - trueResY) < accuracy);
            Assert.IsTrue(Math.Abs(resY2 - trueResY) < accuracy);
            Assert.IsTrue(steps1 == maxStep);
            Assert.IsTrue(steps2 == maxStep);
        }
    }
}