﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffs
{
    public class 数值变化 : Buff
    {
        string[] names;
        public override void Init()
        {
            base.Init();
            var datas = Config.Data.GetArray("t");
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
                float baseValue = (float)field.GetValue(Unit);
                field.SetValue(Unit, baseValue + Skill.Config.BuffData[i]);
                Log.Debug(field.GetValue(Unit));
            }
        }
    }
}
