using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapBuilderUI
{
    partial class UI_MidInfo
    {
        public UnitInfo UnitInfo;
        static Vector2[] Directions = new Vector2[]
        {
            new Vector2(0,1),
            new Vector2(0,-1),
            new Vector2(-1,0),
            new Vector2(1,0),
        };

        partial void Init()
        {
            m_tag.onChanged.Add(() => { UnitInfo.Tag = m_tag.text; });
            m_x.onChanged.Add(() => { UnitInfo.X = int.Parse(m_x.text); });
            m_y.onChanged.Add(() => { UnitInfo.Y = int.Parse(m_y.text); });
            m_delay.onChanged.Add(() => { UnitInfo.ActiveTime = float.Parse(m_delay.text); });
            m_direction.onChanged.Add(() => { UnitInfo.Direction = Directions[m_direction.selectedIndex]; });
            m_headback.onClick.Add(changeUnit);
        }

        public void SetInfo(UnitInfo unitInfo)
        {
            this.UnitInfo = unitInfo;
            if (!string.IsNullOrEmpty(unitInfo.UnitId))
                m_name.text = Database.Instance.Get<UnitData>(UnitInfo.UnitId).Name;
            else
                m_name.text = "";
            m_tag.text = UnitInfo.Tag;
            m_x.text = UnitInfo.X.ToString();
            m_y.text = UnitInfo.Y.ToString();
            m_delay.text = UnitInfo.ActiveTime.ToString();
            m_direction.selectedIndex = Array.IndexOf(Directions, UnitInfo.Direction);
        }

        async void changeUnit()
        {
            try
            {
                var result = await (parent.parent as UI_MidPage).Choose();
                UnitInfo.UnitId = result;
                m_name.text = Database.Instance.Get<UnitData>(UnitInfo.UnitId).Name;

                (parent.parent as UI_MidPage).UpdatePoints();
            }
            catch (Exception e)
            {
                if (e is TaskCanceledException) return;
                throw e;
            }
        }
    }
}
