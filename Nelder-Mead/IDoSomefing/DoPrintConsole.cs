using System;
using System.Collections.Generic;

namespace Nelder_Mead
{
    public class DoPrintConsole : IDoSomefing
    {
        public void Do(List<FuncValue> funcValues)
        {
            FuncValue best = funcValues[0];
            FuncValue good = funcValues[funcValues.Count - 2];
            FuncValue worst = funcValues[funcValues.Count - 1];

            string ans = "Now Simplex: \n";
            ans += "Best:\nX = " + best.X + "\nY = " + best.Y + "\n";
            ans += "Good:\nX = " + good.X + "\nY = " + good.Y + "\n";
            ans += "Worst:\nX = " + worst.X + "\nY = " + worst.Y + "\n";

            Console.WriteLine(ans);
        }
    }
}
