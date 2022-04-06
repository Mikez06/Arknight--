using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainUI
{
    partial class UI_LevelInfo
    {
        public void SetInfo(string mapId)
        {
            var mapData = Database.Instance.GetMap(mapId);
            m_id.text = mapId;
            m_name.text = mapData.MapName;
            m_Desc.text = mapData.Description;
        }
    }
}
