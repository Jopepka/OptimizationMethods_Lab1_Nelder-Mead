using System.Collections.Generic;

namespace Nelder_Mead
{
    public abstract class IDoSomefing
    {
        List<IDoSomefing> Actions = new List<IDoSomefing>();

        public IDoSomefing()
        {
            Actions.Add(this);
        }

        public abstract void Do(Simplex nowSimplex);

        public void Run(Simplex nowSimplex)
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
