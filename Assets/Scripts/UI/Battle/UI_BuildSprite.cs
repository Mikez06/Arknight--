using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleUI
{
    partial class UI_BuildSprite
    {
        public Units.干员 Unit;
        public void SetUnit(Units.干员 unit)
        {
            this.Unit = unit;
            m_typeControl.selectedIndex = (int)unit.Config.UnitType;
            m_cost.text = unit.GetCost().ToString();
            m_bar.max = unit.ResetTime;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Unit == null) return;
            m_cooldown.selectedIndex = Unit.Reseting.Finished() ? 0 : 1;
            m_bar.value = Unit.Reseting.value;
            m_resetTime.text = Unit.Reseting.value.ToString("F1");
            m_canUse.selectedIndex = Unit.Useable() ? 0 : 1;
        }
    }
}
