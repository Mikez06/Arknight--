using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 跳跃 : Skill
    {
        float jumpDist;

        public override void Init()
        {
            base.Init();
            jumpDist = SkillData.Data.GetFloat("JumpDist");
        }

        public override bool Ready()
        {
            if (Unit is Units.敌人 u && u.StopUnit == null) return false;
            return base.Ready();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Cast()
        {
            base.Cast();
            if (Unit is Units.敌人 u)
            {
                u.Jump(jumpDist);
            }
        }

        //protected override float GetSkillDelay(string[] animationName, string[] lastState, out float fullDuration, out float beginDuration)
        //{
        //    var f1 = Unit.UnitModel.GetAnimationDuration(SkillData.ModelAnimation[0]);
        //    var f2 = Unit.UnitModel.GetAnimationDuration(SkillData.ModelAnimation[1]);
        //    fullDuration = f1 + f2;
        //    beginDuration = 0;
        //    return f1;
        //    //return base.GetSkillDelay(animationName, lastState, out fullDuration, out beginDuration);
        //}
    }
}
