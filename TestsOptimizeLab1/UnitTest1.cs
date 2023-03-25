using Nelder_Mead;
using OptimizationMethods_Lab1_Nelder_Mead;
using Vector;

namespace TestsOptimizeLab1
{
    [TestClass]
    public class UnitTest1
    {
        //Параметры для алгоритма Nelder-Mead
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

        // Тестирование через ссылку на функцию
        [TestMethod]
        public void TestRes1()
        {
            simplex = new List<VectorM>()
            {
                new VectorM(new double[]{0, 0}),
                new VectorM(new double[]{1, 0}),
                new VectorM(new double[]{1, 1})
            };

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm();
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
        }

        //Тестирование через лямбда выражение
        [TestMethod]
        public void TestRes2()
        {
            simplex = new List<VectorM>()
            {
                new VectorM(new double[]{-2}),
                new VectorM(new double[] {100})
            };

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm();
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
        }

        // Тестирование через класс SomeFunction, использующий 
        // пакет AngouriMath  
        [TestMethod]
        public void TestRes3()
        {
            simplex = new List<VectorM>()
            {
                new VectorM(new double[]{-2}),
                new VectorM(new double[]{100})
            };

            string str_func = "x^2";
            SomeFunction someFunction = new SomeFunction(str_func, str_func);
            CalculationFunction calculate = someFunction.GetValue;

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm();
            nm.Fit(calculate, simplex);
            VectorM res1 = nm.Run();
            double resY1 = calculate(res1);
            int steps1 = nm.Steps;

            VectorM res2 = nm.RunV2();
            double resY2 = calculate(res2);
            int steps2 = nm.Steps;

            trueResY = 0;
            Assert.IsTrue(Math.Abs(resY1 - trueResY) < accuracy);
            Assert.IsTrue(Math.Abs(resY2 - trueResY) < accuracy);
        }

        //Тестирование на функции Химмельблау
        [TestMethod]
        public void TestRes4()
        {
            string str_func = "(1 - x)^2 + 100(y - x^2)^2";
            SomeFunction someFunction = new SomeFunction(str_func, str_func);
            CalculationFunction calculate = someFunction.GetValue;

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm();
            simplex = nm.CreatureSimplex(2);
            nm.Fit(calculate, simplex);

            VectorM res1 = nm.Run();
            double resY1 = calculate(res1);
            int steps1 = nm.Steps;

            VectorM res2 = nm.RunV2();
            double resY2 = calculate(res2);
            int steps2 = nm.Steps;

            trueResY = 0;
            Assert.IsTrue(Math.Abs(resY1 - trueResY) < accuracy);
            Assert.IsTrue(Math.Abs(resY2 - trueResY) < accuracy);
        }

        //Тестировании на функции Розенброка
        [TestMethod]
        public void TestRes5()
        {
            string str_func = "(x^2 + y - 11)^2 + (x + y^2 - 7)^2";
            SomeFunction someFunction = new SomeFunction(str_func, str_func);
            CalculationFunction calculate = someFunction.GetValue;

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm();
            simplex = nm.CreatureSimplex(2);
            nm.Fit(calculate, simplex);

            VectorM res1 = nm.Run();
            double resY1 = calculate(res1);
            int steps1 = nm.Steps;

            VectorM res2 = nm.RunV2();
            double resY2 = calculate(res2);
            int steps2 = nm.Steps;

            trueResY = 0;
            Assert.IsTrue(Math.Abs(resY1 - trueResY) < accuracy);
            Assert.IsTrue(Math.Abs(resY2 - trueResY) < accuracy);
        }

        //Тестировании на функции Розенброка
        [TestMethod]
        public void TestRes6()
        {
            string str_func = "(x^2 + y - 11)^2 + (x + y^2 - 7)^2";
            SomeFunction someFunction = new SomeFunction(str_func, str_func);
            CalculationFunction calculate = someFunction.GetValue;

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm();
            simplex = nm.CreatureSimplex(2, startVector: new VectorM(new double[] { -2.0, 3 }));
            nm.Fit(calculate, simplex);

            VectorM res1 = nm.Run();
            double resY1 = calculate(res1);
            int steps1 = nm.Steps;

            VectorM res2 = nm.RunV2();
            double resY2 = calculate(res2);
            int steps2 = nm.Steps;

            trueResY = 0;
            Assert.IsTrue(Math.Abs(resY1 - trueResY) < accuracy);
            Assert.IsTrue(Math.Abs(resY2 - trueResY) < accuracy);
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
            SomeFunction someFunction = new SomeFunction(str_func, str_func);
            CalculationFunction calculate = someFunction.GetValue;

            Nelder_Mead_algorithm nm = new Nelder_Mead_algorithm();
            nm.Fit(calculate, simplex);
            nm.Run();
            Assert.AreEqual(nm.Steps, nm.MaxSteps);

            nm.RunV2();
            Assert.AreEqual(nm.Steps, nm.MaxSteps);
        }
    }
}