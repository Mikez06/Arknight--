﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class SkillDataEx
{
    public static float[] GetBuffData(this SkillData self,int index)
    {
        if (index == 0)
            return self.BuffData;
        else
            return self.BuffData2;
    }
}

