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
            m_path.onChanged.Add(() => { WaveInfo.Path = m_path.value; var p = MapInfo.PathInfos.FirstOrDefault(x => x.Name == WaveInfo.Path); MapManager.Instance.ShowPath(p.Path, p.FlyPath); });
            m_x.onChanged.Add(() => WaveInfo.OffsetX = float.Parse(m_x.text));
            m_y.onChanged.Add(() => WaveInfo.OffetsetY = float.Parse(m_y.text));
            m_delay.onChanged.Add(() => WaveInfo.Delay = float.Parse(m_delay.text));
            m_count.onChanged.Add(() => WaveInfo.Count = int.Parse(m_count.text));
            m_gap.onChanged.Add(() => WaveInfo.GapTime = float.Parse(m_gap.text));
            m_checkPoint.onChanged.Add(() => { WaveInfo.CheckPoint = int.Parse(m_checkPoint.text); });
            m_tag.onChanged.Add(() => { WaveInfo.Tag = m_tag.text; });
            m_headback.onClick.Add(async () =>
            {
                try
                {
                    var id = await (parent.parent as UI_WavePage).Choose();
                    if (id == -1)
                    {
                        WaveInfo.UnitId = null;
                        WaveInfo.sUnitId = null;
                        m_head.icon = "";
                        m_name.text = "出怪指示线";
                    }
                    else
                    {
                        WaveInfo.sUnitId = Database.Instance.Get<UnitData>(id).Id;
                        m_head.icon = "ui://Res/" + Database.Instance.Get<UnitData>(WaveInfo.sUnitId).HeadIcon;
                        m_name.text = Database.Instance.Get<UnitData>(WaveInfo.sUnitId).Name;
                    }
                }
                catch (Exception e)
                {
                    if (e is TaskCanceledException) return;
                    throw e;
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
            m_checkPoint.text = waveInfo.CheckPoint.ToString();
            m_x.text = waveInfo.OffsetX.ToString();
            m_y.text = waveInfo.OffetsetY.ToString();
            m_delay.text = waveInfo.Delay.ToString();
            m_count.text = waveInfo.Count.ToString();
            m_gap.text = waveInfo.GapTime.ToString();
            m_tag.text = waveInfo.Tag == null ? "" : waveInfo.Tag.ToString();
        }

    }
}
