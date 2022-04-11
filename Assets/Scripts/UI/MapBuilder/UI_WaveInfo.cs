using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilderUI
{
    partial class UI_WaveInfo
    {
        public WaveInfo WaveInfo;
        MapInfo MapInfo => UI_MapBuilder.Instance.MapInfo;

        partial void Init()
        {
            m_path.onChanged.Add(() => { WaveInfo.Path = m_path.value; MapManager.Instance.ShowPath(MapInfo.PathInfos.FirstOrDefault(x => x.Name == WaveInfo.Path).Path); });
            m_x.onChanged.Add(() => WaveInfo.OffsetX = float.Parse(m_x.text));
            m_y.onChanged.Add(() => WaveInfo.OffetsetY = float.Parse(m_y.text));
            m_delay.onChanged.Add(() => WaveInfo.Delay = float.Parse(m_delay.text));
            m_count.onChanged.Add(() => WaveInfo.Count = int.Parse(m_count.text));
            m_gap.onChanged.Add(() => WaveInfo.GapTime = float.Parse(m_gap.text));
            m_headback.onClick.Add(async () =>
            {
                var id= await (parent.parent as UI_WavePage).Choose();
                WaveInfo.sUnitId = Database.Instance.Get<UnitData>(id).Id;
                if (WaveInfo.sUnitId == null)
                {
                    m_head.icon = "";
                    m_name.text = "出怪指示线";
                }
                else
                {
                    m_head.icon = "ui://Res/" + Database.Instance.Get<UnitData>(WaveInfo.sUnitId).HeadIcon;
                    m_name.text = Database.Instance.Get<UnitData>(WaveInfo.sUnitId).Name;
                }
            });
        }

        public void SetInfo(WaveInfo waveInfo,string[] dropValue)
        {
            this.WaveInfo = waveInfo;
            m_path.items = dropValue;
            m_path.values = dropValue;
            if (waveInfo.sUnitId == null)
            {
                m_head.icon = "";
                m_name.text = "出怪指示线";
            }
            else
            {               
                m_head.icon = "ui://Res/" + Database.Instance.Get<UnitData>(waveInfo.sUnitId).HeadIcon;
                m_name.text = Database.Instance.Get<UnitData>(waveInfo.sUnitId).Name;
            }
            m_path.value = waveInfo.Path;
            m_x.text = waveInfo.OffsetX.ToString();
            m_y.text = waveInfo.OffetsetY.ToString();
            m_delay.text = waveInfo.Delay.ToString();
            m_count.text = waveInfo.Count.ToString();
            m_gap.text = waveInfo.GapTime.ToString();
        }

    }
}
