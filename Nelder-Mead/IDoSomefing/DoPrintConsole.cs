using System;
using System.Collections.Generic;

namespace Nelder_Mead
{
    public class DoPrintConsole : IDoSomefing
    {
        public string Log { get; private set; } = "";
        public void Do(List<FuncValue> funcValues)
        {
            FuncValue best = funcValues[0];
            FuncValue good = funcValues[funcValues.Count - 2];
            FuncValue worst = funcValues[funcValues.Count - 1];

            Log += "\nNow Simplex: \n";
            Log += "Best:\nX = " + best.X + "\nY = " + best.Y + "\n";
            Log += "Good:\nX = " + good.X + "\nY = " + good.Y + "\n";
            Log += "Worst:\nX = " + worst.X + "\nY = " + worst.Y + "\n";

            Console.WriteLine(Log);
        }

        public void ClearLog() => Log = "";
    }
}
