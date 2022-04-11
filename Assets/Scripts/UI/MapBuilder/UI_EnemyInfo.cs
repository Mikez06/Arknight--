using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilderUI
{
    partial class UI_EnemyInfo
    {
        public UnitData UnitData;
        public void SetInfo(UnitData unitData)
        {
            this.UnitData = unitData;
            if (unitData == null)
            {
                m_head.icon = "";
                m_name.text = "出怪指示线";
                m_atk.text = "";
                m_hp.text = "";
                m_def.text = "";
            }
            else
            {
                m_head.icon = "ui://Res/" + unitData.HeadIcon;
                m_name.text = unitData.Name;
                m_atk.text = unitData.Attack.ToString();
                m_hp.text = unitData.Hp.ToString();
                m_def.text = unitData.Defence.ToString();
            }
        }
    }
}
