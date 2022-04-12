using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapBuilderUI
{
    partial class UI_EnemyInfo
    {
        public UnitData UnitData;
        partial void Init()
        {
            m_hp.onChanged.Add((x) => { getInfo().Hp = int.Parse(m_hp.text); fresh();});
            m_hp.onClick.Add(x => { x.StopPropagation(); });
            m_def.onChanged.Add((x) => { getInfo().Def = int.Parse(m_def.text); fresh(); });
            m_def.onClick.Add(x => { x.StopPropagation(); });
            m_atk.onChanged.Add((x) => { getInfo().Atk = int.Parse(m_atk.text); fresh(); });
            m_atk.onClick.Add(x => { x.StopPropagation(); });
            m_magDef.onChanged.Add((x) => { getInfo().MagDef = int.Parse(m_magDef.text); fresh(); });
            m_magDef.onClick.Add(x => { x.StopPropagation(); });
            m_speed.onChanged.Add((x) => { getInfo().Speed = int.Parse(m_speed.text); fresh(); });
            m_speed.onClick.Add(x => { x.StopPropagation(); });
        }

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
                m_speed.text = "";
                m_test.selectedIndex = 0;
            }
            else
            {
                fresh();
            }
        }

        OverwriteUnitInfo getInfo()
        {
            var unitovInfo = UI_MapBuilder.Instance.MapInfo.UnitOvDatas.FirstOrDefault(x => x.UnitId == UnitData.Id);
            if (unitovInfo == null)
            {
                unitovInfo = new OverwriteUnitInfo()
                {
                    Atk = UnitData.Attack,
                    Def = UnitData.Defence,
                    Hp = UnitData.Hp,
                    MagDef = UnitData.MagicDefence,
                    UnitId = UnitData.Id,
                };
                UI_MapBuilder.Instance.MapInfo.UnitOvDatas.Add(unitovInfo);
            }
            return unitovInfo;
        }

        void fresh()
        {
            var unitovInfo = UI_MapBuilder.Instance.MapInfo.UnitOvDatas.FirstOrDefault(x => x.UnitId == UnitData.Id);

            m_head.icon = "ui://Res/" + UnitData.HeadIcon;
            m_name.text = UnitData.Name;
            m_atk.text = UnitData.Attack.ToString();
            m_hp.text = UnitData.Hp.ToString();
            m_def.text = UnitData.Defence.ToString();
            m_magDef.text = UnitData.MagicDefence.ToString();
            m_speed.text = UnitData.Speed.ToString();
            m_test.selectedIndex = UnitData.Test ? 1 : 0;
            if (unitovInfo != null)
            {
                if (UnitData.Attack != unitovInfo.Atk)
                {
                    m_atk.text = unitovInfo.Atk.ToString();
                    m_atk.color = Color.red;
                }
                else m_atk.color = Color.black;
                if (UnitData.Defence != unitovInfo.Def)
                {
                    m_def.text = unitovInfo.Def.ToString();
                    m_def.color = Color.red;
                }
                else m_def.color = Color.black;
                if (UnitData.Hp != unitovInfo.Hp)
                {
                    m_hp.text = unitovInfo.Hp.ToString();
                    m_hp.color = Color.red;
                }
                else m_hp.color = Color.black;
                if (UnitData.MagicDefence != unitovInfo.MagDef)
                {
                    m_magDef.text = unitovInfo.MagDef.ToString();
                    m_magDef.color = Color.red;
                }
                else m_magDef.color = Color.black;
                if (UnitData.Speed != unitovInfo.Speed)
                {
                    m_speed.text = unitovInfo.Speed.ToString();
                    m_speed.color = Color.red;
                }
                else m_speed.color = Color.black;
            }
        }
    }
}
