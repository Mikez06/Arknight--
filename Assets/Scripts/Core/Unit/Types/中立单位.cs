using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Units
{
    public class 中立单位 : Unit
    {
        public override void UpdateAction()
        {
            base.UpdateAction();

            //if (MainSkill != null && MainSkill.SkillData.PowerType == PowerRecoverTypeEnum.自动)
            //{
            //    RecoverPower(PowerSpeed * SystemConfig.DeltaTime);
            //}
            if (this.State == StateEnum.Die)
            {
                UpdateDie();
            }
            else
            {
                UpdateSkills();
            }
        }

        public override void Init()
        {
            base.Init();
            if (UnitData.MainSkill != null)
                MainSkill = LearnSkill(UnitData.MainSkill[0], null);
            BattleUI.UI_Battle.Instance.CreateUIUnit(this);
            Agi = 100;
        }

        public override void Finish(bool leaveEvent = true)
        {
            base.Finish(leaveEvent);
            Battle.AllUnits.Remove(this);
            if (Team == 0) Battle.PlayerUnits2.Remove(this);
            BattleUI.UI_Battle.Instance.ReturnUIUnit(this);
            if (Battle.Map.Tiles[GridPos.x, GridPos.y].Unit == this)
                Battle.Map.Tiles[GridPos.x, GridPos.y].Unit = null;
            if (Battle.Map.Tiles[GridPos.x, GridPos.y].MidUnit == this)
                Battle.Map.Tiles[GridPos.x, GridPos.y].MidUnit = null;
            UnityEngine.GameObject.Destroy(UnitModel.gameObject);
            UnitModel = null;
        }
    }
}
