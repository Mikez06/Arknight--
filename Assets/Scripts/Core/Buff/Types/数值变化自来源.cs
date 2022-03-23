using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 鼓舞 : Buff
    {
        protected string[] names;
        public override void Init()
        {
            base.Init();
            var datas = BuffData.Data.GetArray("t");
            names = new string[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                names[i] = Convert.ToString(datas[i]);
            }
        }

        public override void Apply()
        {
            for (int i = 0; i < names.Length; i++)
            {
                string fieldName = (string)names[i];
                var field = Unit.GetType().GetField(fieldName);
                if (field == null)
                {
                    Log.Debug($"{Unit.UnitData.Id} 没有 属性 {fieldName}");
                    continue;
                }
                float baseValue = (float)field.GetValue(Unit);
                field.SetValue(Unit, baseValue + GetValue(i));
                UnityEngine.Debug.Log($"{Unit.UnitData.Id}的{names[i]}变成{field.GetValue(Unit)}");
            }
        }

        protected virtual float GetValue(int i)
        {
            return Skill.SkillData.GetBuffData(Index)[i] * Skill.Unit.Attack;
        }
    }
}
