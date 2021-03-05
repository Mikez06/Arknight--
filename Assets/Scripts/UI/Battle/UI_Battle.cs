using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace BattleUI
{
    partial class UI_Battle
    {
        public static UI_Battle Instance;
        public Battle Battle;

        GObjectPool HeadPool;

        Units.干员 selectedUnit;
        partial void Init()
        {
            Instance = this;
            HeadPool = new GObjectPool(container.cachedTransform);
            m_state.onChanged.Add(pageChange);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Battle != null)
            {
                m_hp.text = Battle.Hp.ToString();
                m_cost.text = Battle.Cost.ToString();
                m_costBar.value = 1 - Battle.CostCounting.value;
            }
        }

        public void SetBattle(Battle battle)
        {
            this.Battle = battle;
            updateUnitsLayout();
        }

        void updateUnitsLayout()
        {
            for (int i = 0; i < m_Builds.numChildren; i++)
            {
                var head = m_Builds.GetChildAt(i) as UI_BuildSprite;
                head.Unit = null;
                HeadPool.ReturnObject(head);
            }
            m_Builds.RemoveChildren();
            var units = Battle.PlayerUnits.Where(x => !x.InMap).ToList();
            units.Sort((x, y) => x.Config.Cost - y.Config.Cost);
            foreach (var unit in units)
            {
                var head = HeadPool.GetObject(UI_BuildSprite.URL) as UI_BuildSprite;
                head.SetUnit(unit);
                m_Builds.AddChild(head);
                head.xy = new UnityEngine.Vector2(1750 - units.IndexOf(unit) * head.width, unit == selectedUnit ? 830 : 883);
                head.onClick.Set(() => clickUnit(unit));
                head.draggable = true;
                head.onDragStart.Set(dragUnit);
            }
        }

        void clickUnit(Units.干员 unit)
        {
            if (selectedUnit == unit)
            {
                m_state.selectedIndex = 0;
            }
            else
            {
                selectedUnit = unit;
                m_state.selectedIndex = 1;
            }
            updateUnitsLayout();
        }

        void dragUnit(EventContext evt)
        {
            if (m_state.selectedIndex != 1) return;
            var unit = (evt.sender as UI_BuildSprite).Unit;
            evt.PreventDefault();
            BattleCamera.Instance.StartBuild(unit);
        }

        void pageChange()
        {
            switch (m_state.selectedIndex)
            {
                case 0:
                    selectedUnit = null;
                    TimeHelper.Instance.SetGameSpeed(1);
                    break;
                case 1:
                    TimeHelper.Instance.SetGameSpeed(0.1f);
                    m_left.SetUnit(selectedUnit);
                    break;
            }
        }
    }
}
