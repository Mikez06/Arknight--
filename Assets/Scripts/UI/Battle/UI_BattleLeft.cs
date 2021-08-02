using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleUI
{
    partial class UI_BattleLeft
    {
        public void SetUnit(Units.干员 unit)
        {
            m_name.text = unit.UnitData.Name;
        }
    }
}
