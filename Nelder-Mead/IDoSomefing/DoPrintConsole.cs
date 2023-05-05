using System;
using System.Collections.Generic;

namespace Nelder_Mead
{
    public class DoPrintConsole : IDoSomefing
    {
        public string Log { get; private set; } = "";
        public override void Do(Simplex nowSimplex)
        {

            Log += "\n\nNow Simplex:";
            Log += "\nBest:\n" + nowSimplex.Best;
            Log += "\nGood:\n" + nowSimplex.Good;
            Log += "\nWorst:\n" + nowSimplex.Worst;

            Console.WriteLine(Log);
        }

        public void ClearLog() => Log = "";
    }
}
