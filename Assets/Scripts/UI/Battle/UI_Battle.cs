﻿using System;
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

        GObjectPool UIPool;

        Units.干员 selectedUnit;
        partial void Init()
        {
            Instance = this;
            UIPool = new GObjectPool(container.cachedTransform);
            m_state.onChanged.Add(pageChange);
            m_DirectionPanel.draggable = true;
            m_DirectionPanel.onDragStart.Add(dragDirection);
            DragDropManager.inst.dragAgent.onDragMove.Add(dragDirectionMove);
            DragDropManager.inst.dragAgent.onDragEnd.Add(dragDirectionEnd);
            m_DirectonCancal.onClick.Add(stopSetUnit);
            m_SkillUseBack.onClick.Add(StopChooseUnit);
            m_SkillUsePanel.m_Leave.onClick.Add(leaveUnit);
            m_SkillUsePanel.m_mainSkillInfo.onClick.Add(useMainSkill);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Battle != null)
            {
                m_hp.text = Battle.Hp.ToString();
                m_cost.text = Battle.Cost.ToString();
                m_costBar.value = 1 - Battle.CostCounting.value;

                if (m_state.selectedIndex == 4)
                {
                    Vector2 pos = Camera.main.WorldToScreenPoint(BattleCamera.Instance.FocusUnit.UnitModel.SkeletonAnimation.transform.position);
                    pos.y = Screen.height - pos.y;
                    m_SkillUsePanel.position = pos.ScreenToUI();
                }
            }
        }

        public void SetBattle(Battle battle)
        {
            this.Battle = battle;
            updateUnitsLayout();
        }

        public void CreateUIUnit(Unit unit)
        {
            var battleUnit = UIPool.GetObject(UI_BattleUnit.URL) as UI_BattleUnit;
            battleUnit.SetUnit(unit);
            m_Units.AddChild(battleUnit);
        }

        public void ReturnUIUnit(Unit unit)
        {
            unit.uiUnit.Unit = null;
            m_Units.RemoveChild(unit.uiUnit);
            UIPool.ReturnObject(unit.uiUnit);
            unit.uiUnit = null;
        }

        public void ChooseUnit(Unit unit)
        {
            TimeHelper.Instance.SetGameSpeed(0.2f);
            selectedUnit = unit as Units.干员;
            m_state.selectedIndex = 4;
            m_left.SetUnit(unit as Units.干员);
            Debug.Log(unit.Config.Name);
            BattleCamera.Instance.ShowUnitInfo(unit);
        }

        public void StopChooseUnit()
        {
            if (m_state.selectedIndex == 4)
            {
                m_state.selectedIndex = 0;
            }
        }

        void stopSetUnit()
        {
            selectedUnit.UnitModel.gameObject.SetActive(false);
            selectedUnit = null;
            m_state.selectedIndex = 0;
            BattleCamera.Instance.HideUnitAttackArea();
            updateUnitsLayout();
        }

        void updateUnitsLayout()
        {
            for (int i = 0; i < m_Builds.numChildren; i++)
            {
                var head = m_Builds.GetChildAt(i) as UI_BuildSprite;
                head.Unit = null;
                UIPool.ReturnObject(head);
            }
            m_Builds.RemoveChildren();
            var units = Battle.PlayerUnits.Where(x => x.MapIndex == -1).ToList();
            units.Sort((x, y) => x.Config.Cost - y.Config.Cost);
            foreach (var unit in units)
            {
                var head = UIPool.GetObject(UI_BuildSprite.URL) as UI_BuildSprite;
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
            StopChooseUnit();//如果当前正在看干员详细状态，就退出来
            if (selectedUnit == unit)
            {
                m_state.selectedIndex = 0;
            }
            else
            {
                selectedUnit = unit;
                m_state.selectedIndex = 1;
            }
        }

        void dragUnit(EventContext evt)
        {
            evt.PreventDefault();
            var unit = (evt.sender as UI_BuildSprite).Unit;
            if (!unit.CanBuild()) return;//不能造的时候拽不出来
            if (m_state.selectedIndex != 1 && unit != selectedUnit) return;//拽错了也不许出来
            m_state.selectedIndex = 2;
            BattleCamera.Instance.StartBuild();
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


        void leaveUnit()
        {
            selectedUnit.LeaveMap();
            m_state.selectedIndex = 0;
        }


        void useMainSkill()
        {
            var sk = selectedUnit.MainSkill;
            if (sk.CanOpen())
            {
                sk.DoOpen();
                m_state.selectedIndex = 0;
            }
        }

        void pageChange()
        {
            switch (m_state.selectedIndex)
            {
                case 0:
                    selectedUnit = null;
                    BattleCamera.Instance.Rotate = false;
                    BattleCamera.Instance.ShowUnitInfo(null);
                    TimeHelper.Instance.SetGameSpeed(1);
                    BattleCamera.Instance.HideHighLight();
                    updateUnitsLayout();
                    break;
                case 1:
                    TimeHelper.Instance.SetGameSpeed(0.2f);
                    m_left.SetUnit(selectedUnit);
                    BattleCamera.Instance.Rotate = true;
                    BattleCamera.Instance.BuildUnit = selectedUnit;
                    BattleCamera.Instance.ShowHighLight();
                    updateUnitsLayout();
                    break;
                case 3:
                    Vector2 mousePos = Camera.main.WorldToScreenPoint(selectedUnit.UnitModel.transform.position); //Stage.inst.touchPosition.ScreenToUI();
                    mousePos.y = Screen.height - mousePos.y;
                    mousePos = mousePos.ScreenToUI();
                    m_DirectionPanel.position = mousePos;
                    m_DirectionPanel.m_coner.selectedIndex = 0;
                    m_DirectionBack.m_hole.position = mousePos;
                    m_DirectionPanel.m_grip.position = new Vector2(m_DirectionPanel.width / 2, m_DirectionPanel.height / 2);
                    break;
                case 4:
                    m_SkillUsePanel.SetUnit(selectedUnit);
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