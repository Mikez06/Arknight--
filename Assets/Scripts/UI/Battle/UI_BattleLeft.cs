using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleUI
{
    partial class UI_BattleLeft
    {
        public void SetUnit(Units.干员 unit)
        {
            m_name.text = unit.UnitData.Name;
            m_atk.text = unit.Attack.ToString();
            m_def.text = unit.Defence.ToString();
            m_magDef.text = unit.MagicDefence.ToString();
            m_block.text = unit.StopCount.ToString();
            m_Hp.max = unit.MaxHp;
            m_Hp.value = unit.Hp;
            m_standPic.texture = new FairyGUI.NTexture(ResHelper.GetAsset<Texture>(PathHelper.StandPicPath + unit.UnitData.StandPic));
        }
    }
}
