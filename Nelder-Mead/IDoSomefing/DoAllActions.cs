using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nelder_Mead
{
    public class DoAllActions : IDoSomefing
    {
        List<IDoSomefing> Actions = new List<IDoSomefing>();

        public void Do(Simplex nowSimplex)
        {
            foreach (IDoSomefing action in Actions)
                action.Do(nowSimplex);
        }

        public void AddAction(IDoSomefing action)
        {
            Actions.Add(action);
        }
    }
}
