using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilderUI
{
    partial class UI_MidUnitInfo
    {
        public UnitData UnitData;
        public void SetInfo(UnitData unitData)
        {
            this.UnitData = unitData;
            m_name.text = unitData.Name;
            m_desc.text = unitData.AblitityInfo;
        }
    }
}
