using FairyGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleUI
{
    partial class UI_DragPanel
    {
        public UI_Battle Parent;
        partial void Init()
        {
            m_DirectionPanel.draggable = true;
            m_DirectionPanel.onDragStart.Add(dragDirection);
            m_DirectonCancal.onClick.Add(stopSetUnit);
            DragDropManager.inst.dragAgent.onDragMove.Add(dragDirectionMove);
            DragDropManager.inst.dragAgent.onDragEnd.Add(dragDirectionEnd);
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
                float angle = Vector2.SignedAngle(Vector2.right, delta);
                if (angle < 0) angle += 360;
                if (angle >= 45 && angle < 135)
                {
                    Parent.SelectPlayerUnit.Direction_E = DirectionEnum.Up;
                    Parent.SelectPlayerUnit.ResetAttackPoint();
                    m_DirectionPanel.m_coner.selectedIndex = 3;
                }
                else if (angle >= 135 && angle < 225)
                {
                    Parent.SelectPlayerUnit.Direction_E = DirectionEnum.Left;
                    m_DirectionPanel.m_coner.selectedIndex = 4;
                    Parent.SelectPlayerUnit.ResetAttackPoint();
                }
                else if (angle >= 225 && angle < 315)
                {
                    Parent.SelectPlayerUnit.Direction_E = DirectionEnum.Down;
                    m_DirectionPanel.m_coner.selectedIndex = 1;
                    Parent.SelectPlayerUnit.ResetAttackPoint();
                }
                else
                {
                    m_DirectionPanel.m_coner.selectedIndex = 2;
                    Parent.SelectPlayerUnit.Direction_E = DirectionEnum.Right;
                    Parent.SelectPlayerUnit.ResetAttackPoint();
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
                Parent.SelectPlayerUnit.JoinMap();
                Parent.selectedUnit = null;
                //BattleCamera.Instance.FocusUnit = null;
                Parent.m_state.selectedIndex = 0;
                Parent.UpdateUnitsLayout();
                BattleCamera.Instance.HideUnitAttackArea();
            }
        }
        void stopSetUnit()
        {
            Parent.selectedUnit.UnitModel.gameObject.SetActive(false);
            Parent.selectedUnit = null;
            Parent.m_state.selectedIndex = 0;
            BattleCamera.Instance.HideUnitAttackArea();
            Parent.UpdateUnitsLayout();
        }
    }
}
