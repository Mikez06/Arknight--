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
        public override void Init()
        {
            base.Init();
            Count = SkillData.Data.GetInt("Count");
            ChildId = Database.Instance.GetIndex<UnitData>(SkillData.Data.GetStr("UnitId"));
        }
        public override void Cast()
        {
            for (int i = 0; i < Count; i++)
            {
                (Unit as Units.干员).GainChild(ChildId);
            }
            base.Cast();
        }
    }
}
