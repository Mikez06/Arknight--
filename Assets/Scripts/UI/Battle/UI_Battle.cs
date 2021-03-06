using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

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
            m_DirectionPanel.draggable = true;
            m_DirectionPanel.onDragStart.Add(dragDirection);
            DragDropManager.inst.dragAgent.onDragMove.Add(dragDirectionMove);
            DragDropManager.inst.dragAgent.onDragEnd.Add(dragDirectionEnd);
            m_DirectionBack.onClick.Add(() =>
            {
                selectedUnit.UnitModel.gameObject.SetActive(false);
                selectedUnit = null;
                m_state.selectedIndex = 0;
                updateUnitsLayout();
            });
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
            var units = Battle.PlayerUnits.Where(x => x.MapIndex == -1).ToList();
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
            evt.PreventDefault();
            if (m_state.selectedIndex != 1) return;
            m_state.selectedIndex = 2;
            var unit = (evt.sender as UI_BuildSprite).Unit;
            BattleCamera.Instance.StartBuild(unit);
        }

        Vector2 dragPos;
        void dragDirection(EventContext evt)
        {
            evt.PreventDefault();
            dragPos = Stage.inst.touchPosition.ScreenToUI();
            BattleCamera.Instance.HideUnitAttackArea();
            DragDropManager.inst.StartDrag(evt.sender as GObject, null, null, (int)evt.data);
        }

        void dragDirectionMove()
        {
            var delta = Stage.inst.touchPosition.ScreenToUI() - dragPos;
            if (delta.magnitude < 100)
            {
                //拽的不够远
                m_DirectionPanel.m_coner.selectedIndex = 0;
                BattleCamera.Instance.HideUnitAttackArea();
            }
            else
            {
                float angle = Vector2.SignedAngle(Vector2.right,delta);
                if (angle < 0) angle += 360;
                if (angle>=45 && angle < 135)
                {
                    selectedUnit.Direction_E = DirectionEnum.Up;
                    selectedUnit.ResetAttackPoint();
                    m_DirectionPanel.m_coner.selectedIndex = 3;
                }
                else if (angle >= 135 && angle < 225)
                {
                    selectedUnit.Direction_E = DirectionEnum.Left;
                    m_DirectionPanel.m_coner.selectedIndex = 4;
                    selectedUnit.ResetAttackPoint();
                }
                else if (angle >= 225 && angle < 315)
                {
                    selectedUnit.Direction_E = DirectionEnum.Down;
                    m_DirectionPanel.m_coner.selectedIndex = 1;
                    selectedUnit.ResetAttackPoint();
                }
                else
                {
                    m_DirectionPanel.m_coner.selectedIndex = 2;
                    selectedUnit.Direction_E = DirectionEnum.Right;
                    selectedUnit.ResetAttackPoint();
                }
                BattleCamera.Instance.ShowUnitAttackArea();
            }
            m_DirectionPanel.m_grip.visible = true;
            m_DirectionPanel.m_grip.position = delta + new Vector2(m_DirectionPanel.width / 2, m_DirectionPanel.height / 2);
        }

        void dragDirectionEnd()
        {
            var delta = Stage.inst.touchPosition.ScreenToUI() - dragPos;
            if (delta.magnitude < 100)
            {
                //复位
                m_DirectionPanel.m_grip.position = new Vector2(m_DirectionPanel.width / 2, m_DirectionPanel.height / 2);
            }
            else
            {
                selectedUnit.JoinMap();
                selectedUnit = null;
                m_state.selectedIndex = 0;
                updateUnitsLayout();
                BattleCamera.Instance.HideUnitAttackArea();
            }
        }

        void pageChange()
        {
            switch (m_state.selectedIndex)
            {
                case 0:
                    selectedUnit = null;
                    TimeHelper.Instance.SetGameSpeed(1);
                    BattleCamera.Instance.EndBuild();
                    break;
                case 1:
                    TimeHelper.Instance.SetGameSpeed(0.1f);
                    m_left.SetUnit(selectedUnit);
                    break;
                case 3:
                    Vector2 mousePos = Camera.main.WorldToScreenPoint(selectedUnit.UnitModel.transform.position); //Stage.inst.touchPosition.ScreenToUI();
                    mousePos.y = Screen.height - mousePos.y;
                    mousePos = mousePos.ScreenToUI();
                    m_DirectionPanel.position = mousePos;
                    m_DirectionBack.m_hole.position = mousePos;
                    break;
            }
        }
    }

    public static class TouchHelper
    {
        public static Vector2 ScreenToUI(this Vector2 self)
        {
            return new Vector2(self.x * GRoot.inst.width / Screen.width, self.y * GRoot.inst.height / Screen.height);
        }
    }

}
