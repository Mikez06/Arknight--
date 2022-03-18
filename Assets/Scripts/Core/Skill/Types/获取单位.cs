using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 获取单位 : Skill
    {
        public int Count;
        public int ChildId;
        public int MaxCount;
        public override void Init()
        {
            base.Init();
            Count = SkillData.Data.GetInt("Count");
            ChildId = Database.Instance.GetIndex<UnitData>(SkillData.Data.GetStr("UnitId"));
            MaxCount = SkillData.Data.GetInt("MaxCount");
        }

        public override bool Useable()
        {
            return base.Useable();
        }

        public override void Cast()
        {
            for (int i = 0; i < Count; i++)
            {
                (Unit as Units.干员).GainChild(ChildId);
            }
            base.Cast();
        }

        public override void DoOpen()
        {
            if (MaxCount != 0 && (Unit as Units.干员).Children.Where(x=>x.InputTime<0).Count() >= MaxCount) return;
            base.DoOpen();
        }
    }
}
