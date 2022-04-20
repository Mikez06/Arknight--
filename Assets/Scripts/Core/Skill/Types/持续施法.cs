using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 持续施法 : Skill
    {
        List<Unit> LastTarget = new List<Unit>();
        public override void Update()
        {
            base.Update();
            for (int i = LastTarget.Count - 1; i >= 0; i--)
            {
                if (!CanUseTo(LastTargets[i])) LastTarget.RemoveAt(i);
            }
            if (LastTarget.Count == 0 || Unit.IfStun)
            {
                Opening.Finish();
                finsihCast();
            }
        }

        public override void UpdateOpening()
        {
            bool finish = Opening.Finished();
            base.UpdateOpening();
            if (!finish && Opening.Finished())
            {
                finsihCast();
            }
        }

        void finsihCast()
        {
            LastTarget.Clear();
        }
        public override void FindTarget()
        {
            Targets.Clear();
            if (LastTarget.Count > 0)
            {
                Targets.AddRange(LastTarget);
                return;
            }
            base.FindTarget();
            LastTarget.AddRange(Targets);
        }
    }
}
