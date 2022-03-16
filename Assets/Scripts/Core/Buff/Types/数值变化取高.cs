using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 数值变化取高:数值变化
    {
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
                var targetValue = GetValue(i);
                if (baseValue < targetValue)
                    field.SetValue(Unit, targetValue);
                UnityEngine.Debug.Log($"{Unit.UnitData.Id}的{names[i]}变成{field.GetValue(Unit)}");
            }
        }
    }
}
