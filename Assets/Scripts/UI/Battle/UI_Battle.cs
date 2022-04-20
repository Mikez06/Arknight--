using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace BattleUI
{
    partial class UI_Battle:IGameUIView
    {
        public static UI_Battle Instance;
        public Battle Battle;

        GObjectPool UIPool;

        public Unit selectedUnit;
        public Units.干员 SelectPlayerUnit => selectedUnit as Units.干员;

        GameObject worldUI;
        UI_DragPanel dragPanel;
        
        partial void Init()
        {
            Instance = this;
            UIPool = new GObjectPool(container.cachedTransform);
            m_state.onChanged.Add(pageChange);
            m_SkillUseBack.onClick.Add(StopChooseUnit);
            m_SkillUsePanel.m_Leave.onClick.Add(leaveUnit);
            m_SkillUsePanel.m_mainSkillInfo.onClick.Add(useMainSkill);
            m_endClick.onClick.Add(ExitBattle);
            m_Setting.onClick.Add(TryGiveup);
            m_CancelGiveUp.onClick.Add(cancelGiveup);
            m_GiveUpBack.onClick.Add(cancelGiveup);
            m_GiveUp.onClick.Add(doGiveUp);
            m_GameSpeed.onClick.Add(() => TimeHelper.Instance.SetFastSpeed(!TimeHelper.Instance.FastSpeed));
            m_Pause.onClick.Add(() => TimeHelper.Instance.SetPause(!TimeHelper.Instance.Pause));

            worldUI = ResHelper.Instantiate("Assets/Bundles/Other/UIPanel");
            GameObject.DontDestroyOnLoad(worldUI);
            dragPanel = worldUI.GetComponent<UIPanel>().ui as UI_DragPanel;
            //dragPanel.AddRelation(GRoot.inst, RelationType.Size);
            dragPanel.SetSize(GRoot.inst.size.x, GRoot.inst.size.y);
            dragPanel.visible = false;
            dragPanel.displayObject.cachedTransform.localPosition = new Vector3(-dragPanel.size.x / 2, dragPanel.size.y / 2, -50);
            dragPanel.Parent = this;
            //m_DirectionPanel
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Battle != null)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !Battle.Finish)
                {
                    m_Pause.onClick.Call();
                }
                m_GameSpeed.m_Speed.selectedIndex = TimeHelper.Instance.FastSpeed ? 1 : 0;
                m_Pause.m_Speed.selectedIndex = TimeHelper.Instance.Pause ? 1 : 0;
                m_enemy.text = Battle.EnemyCount.ToString();
                m_hp.text = Battle.Hp.ToString();
                m_cost.text = Battle.Cost.ToString();
                m_costBar.value = 1 - Battle.CostCounting.value;
                m_number.text = Battle.BuildCount.ToString();

                if (m_state.selectedIndex == 4)
                {
                    Vector2 pos = Camera.main.WorldToScreenPoint(BattleCamera.Instance.FocusUnit.UnitModel.GetModelPositon());
                    pos.y = Screen.height - pos.y;
                    m_SkillUsePanel.position = pos.ScreenToUI();
                }
            }
        }

        public void SetBattle(Battle battle)
        {
            this.Battle = battle;
            m_DamageInfo.RemoveChildren(0, m_DamageInfo.numChildren, true);
            UpdateUnitsLayout();
        }

        public void CreateUIUnit(Unit unit)
        {
            var battleUnit = UIPool.GetObject(UI_BattleUnit.URL) as UI_BattleUnit;
            battleUnit.SetUnit(unit);
            m_Units.AddChild(battleUnit);
        }

        public void ReturnUIUnit(Unit unit)
        {
            if (unit.uiUnit == null) return;
            unit.uiUnit.Unit = null;
            m_Units.RemoveChild(unit.uiUnit);
            UIPool.ReturnObject(unit.uiUnit);
            unit.uiUnit = null;
        }

        public void ChooseUnit(Unit unit)
        {
            TimeHelper.Instance.SetGameSpeed(0.2f);
            selectedUnit = unit;
            m_state.selectedIndex = 4;
            m_left.SetUnit(unit);
            BattleCamera.Instance.ShowUnitInfo(unit);
        }

        public void StopChooseUnit()
        {
            if (m_state.selectedIndex == 4)
            {
                m_state.selectedIndex = 0;
            }
        }

        public void BattleEnd()
        {
            foreach (var uiUnit in m_Units.GetChildren())
            {
                UIPool.ReturnObject(uiUnit);
            }
            m_Units.RemoveChildren();

            m_state.selectedIndex = 5;
            BattleCamera.Instance.Blur = true;
            var unit = Battle.PlayerUnits[UnityEngine.Random.Range(0, Battle.PlayerUnits.Count)];
            string picName = unit.UnitData.StandPic;
            m_endPic.texture = new NTexture(ResHelper.GetAsset<Texture>(PathHelper.StandPicPath + picName));
            if (Battle.Win)
            {
                m_win.selectedIndex = 0;
                if (Battle.Hp >= 10)
                {
                    m_win3.Play();
                }
                else if (Battle.Hp >= 5)
                {
                    m_win2.Play();
                }
                else
                {
                    m_win1.Play();
                }
            }
            else
            {
                m_win.selectedIndex = 1;
            }
        }

        public void ExitBattle()
        {
            BattleManager.Instance.FinishBattle();
        }

        public void UpdateUnitsLayout()
        {
            for (int i = 0; i < m_Builds.numChildren; i++)
            {
                var head = m_Builds.GetChildAt(i) as UI_BuildSprite;
                head.Unit = null;
                UIPool.ReturnObject(head);
            }
            m_Builds.RemoveChildren();
            var units = Battle.PlayerUnits.Where(x => x.InputTime == -1).GroupBy(x => x.Id).ToList();
            units.Sort((x, y) => y.FirstOrDefault().UnitData.Cost - x.FirstOrDefault().UnitData.Cost);
            foreach (var group in units)
            {
                var head = UIPool.GetObject(UI_BuildSprite.URL) as UI_BuildSprite;
                head.SetUnit(group.FirstOrDefault());
                m_Builds.AddChild(head);
                head.xy = new UnityEngine.Vector2(width * 0.9f - units.IndexOf(group) * head.width, group.FirstOrDefault() == selectedUnit ? height - 50f : height);
                head.onClick.Set(() => clickUnit(group.FirstOrDefault()));
                head.draggable = true;
                head.onDragStart.Set(dragUnit);
                if (group.FirstOrDefault().UnitData.NotReturn)
                {
                    head.m_count.visible = true;
                    head.m_count.SetVar("n", group.Count().ToString()).FlushVars();
                }
                else
                {
                    head.m_count.visible = false;
                }
            }
        }

        void clickUnit(Units.干员 unit)
        {
            StopChooseUnit();//如果当前正在看干员详细状态，就退出来
            if (selectedUnit == unit)
            {
                selectedUnit = null;
                BattleCamera.Instance.CancelBuild();
                m_state.selectedIndex = 0;
            }
            else
            {
                BattleCamera.Instance.CancelBuild();
                selectedUnit = unit;
                m_state.selectedIndex = 1;
                inSelectUnit();
            }
        }

        void dragUnit(EventContext evt)
        {
            evt.PreventDefault();
            var unit = (evt.sender as UI_BuildSprite).Unit;
            if (!unit.CanBuild()) return;//不能造的时候拽不出来
            if (unit != selectedUnit) clickUnit(unit);
            //if (m_state.selectedIndex != 1 && unit != selectedUnit) return;//拽错了也不许出来
            m_state.selectedIndex = 2;
            BattleCamera.Instance.BuildUnit = SelectPlayerUnit;
            BattleCamera.Instance.StartBuild();
        }




        void leaveUnit()
        {
            SelectPlayerUnit.LeaveMap(true);
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
            if (m_state.selectedIndex != 3) dragPanel.visible = false;
            switch (m_state.selectedIndex)
            {
                case 0:
                    selectedUnit = null;
                    BattleCamera.Instance.Rotate = false;
                    BattleCamera.Instance.ShowUnitInfo(null);
                    TimeHelper.Instance.SetGameSpeed(1);
                    BattleCamera.Instance.HideHighLight();
                    UpdateUnitsLayout();
                    break;
                case 1:
                    inSelectUnit();
                    break;
                case 3:
                    dragPanel.visible = true;
                    worldUI.transform.position = selectedUnit.UnitModel.transform.position;
                    dragPanel.SetSize(GRoot.inst.size.x, GRoot.inst.size.y);
                    dragPanel.displayObject.cachedTransform.localPosition = new Vector3(-dragPanel.size.x / 2, dragPanel.size.y / 2, -50);
                    //Vector2 mousePos = Camera.main.WorldToScreenPoint(selectedUnit.UnitModel.transform.position); //Stage.inst.touchPosition.ScreenToUI();
                    //mousePos.y = Screen.height - mousePos.y;
                    //mousePos = mousePos.ScreenToUI();
                    //dragPanel.m_DirectionPanel.position = mousePos;
                    dragPanel.m_DirectionPanel.m_coner.selectedIndex = 0;
                    //dragPanel.m_DirectionBack.m_hole.position = mousePos;
                    dragPanel.m_DirectionPanel.m_grip.position = new Vector2(dragPanel.m_DirectionPanel.width / 2, dragPanel.m_DirectionPanel.height / 2);
                    break;
                case 4:
                    m_SkillUsePanel.SetUnit(selectedUnit);
                    break;
            }
        }

        void inSelectUnit()
        {
            TimeHelper.Instance.SetGameSpeed(0.2f);
            m_left.SetUnit(selectedUnit);
            BattleCamera.Instance.Rotate = true;
            BattleCamera.Instance.BuildUnit = SelectPlayerUnit;
            BattleCamera.Instance.ShowHighLight();
            UpdateUnitsLayout();
        }

        void TryGiveup()
        {
            m_state.selectedIndex = 6;
            TimeHelper.Instance.SetPause(true);
            BattleCamera.Instance.BuildMode = false;
            if (selectedUnit != null)
            {
                selectedUnit.UnitModel.gameObject.SetActive(false);
                BattleCamera.Instance.BuildUnit = null;
                BattleCamera.Instance.HideUnitAttackArea();
            }
            BattleCamera.Instance.FocusUnit = null;
        }

        void cancelGiveup()
        {
            TimeHelper.Instance.SetPause(false);
            m_state.selectedIndex = 0;
        }

        void doGiveUp()
        {
            BattleManager.Instance.Battle.GiveUp();
            TimeHelper.Instance.SetGameSpeed(1f);
            TimeHelper.Instance.SetFastSpeed(false);
            TimeHelper.Instance.SetPause(false);
            //BattleManager.Instance.FinishBattle();
        }

        public void Enter()
        {
            m_state.SetSelectedIndex(0);
        }

        Queue<(int, int, Vector2)> textQueue = new Queue<(int, int, Vector2)>();       

        public void ShowDamageText(DamageInfo damage, int type,Vector2 pos)
        {
            int showDamage = Mathf.RoundToInt(Mathf.Abs(damage.FinalDamage));
            if (showDamage == 0) return;
            textQueue.Enqueue((showDamage, type, pos));
            DoShowText();
            //ShowDamageText(showDamage.ToString(), type, pos);
        }

        async void DoShowText()
        {
            if (textQueue.Count > 1) return;
            while (textQueue.Count > 0)
            {
                var target = textQueue.Peek();
                ShowDamageText(target.Item1.ToString(), target.Item2, target.Item3);
                await TimeHelper.Instance.WaitAsync(0.0f);
                textQueue.Dequeue();
            }
        }

        public void ShowDamageText(string text,int type,Vector2 pos)
        {
            var damageInfo = UIPackage.CreateObjectFromURL(UI_DamageInfo.URL) as UI_DamageInfo;
            damageInfo.m_number.SetVar("n", text).FlushVars();
            damageInfo.m_type.selectedIndex = type;
            m_DamageInfo.AddChild(damageInfo);
            damageInfo.position = pos;
            damageInfo.m_show.Play(() =>
            {
                damageInfo.Dispose();
            });
        }
    }
}
