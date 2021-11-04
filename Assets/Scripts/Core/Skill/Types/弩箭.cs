using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Skills
{
    public class 弩箭 : Skill
    {
        public override void Update()
        {
            if (SkillData.AutoUse && Power == MaxPower)
            {
                DoOpen();
            }
            if (Ready())
            {
                Start();
            }

            if (Casting.Update(SystemConfig.DeltaTime))
            {
                Cast();
            }

            if (Bursting.Update(SystemConfig.DeltaTime))
            {
                Burst();
            }
        }

        public override void Start()
        {
            if (SkillData.ReadyType == SkillReadyEnum.充能释放)
            {
                Power -= MaxPower;
                Opening.Set(SkillData.OpenTime);
            }
            if (SkillData.ModelAnimation == null)
            {
                //Debug.Log(Unit.UnitData.Id + "的" + SkillData.Id + "没有动画,直接使用");
                ResetCooldown(1);
                Cast();
            }
            else
            {
                var duration = Unit.UnitModel.GetSkillDelay(SkillData.ModelAnimation, Unit.GetAnimation(), out float fullDuration, out float beginDuration);//.SkeletonAnimation.skeleton.data.Animations.Find(x => x.Name == "Attack");
                float attackSpeed = 1f / Unit.Agi * 100;//攻速影响冷却时间
                ResetCooldown(attackSpeed);
                //float aniSpeed = 1;//动画表现上的攻速
                if (fullDuration * attackSpeed != Cooldown.value)
                {
                    //动画时间已经超出攻击间隔了，此时攻速被攻击间隔强制拉快，动画速度也会被强制拉快
                    //动画时间低于攻击间隔时，动画也会被拉长
                    attackSpeed = Cooldown.value / fullDuration;
                    attackSpeed = Mathf.Clamp(attackSpeed, 0.1f, Unit.UnitData.MaxAnimationScale);
                }
                duration = duration * attackSpeed;
                fullDuration = fullDuration * attackSpeed;
                Unit.AttackingAction.Set(fullDuration);
                Unit.State = StateEnum.Attack;
                Unit.AnimationName = SkillData.ModelAnimation;
                Unit.AttackingSkill = this;
                //Debug.Log(SkillData.ModelAnimation);
                Unit.UnitModel?.BreakAnimation();
                Unit.AnimationSpeed = 1 / attackSpeed * (beginDuration + fullDuration) / fullDuration;
                duration = (duration + beginDuration) * fullDuration / (beginDuration + fullDuration);
                Casting.Set(duration);
                Debug.Log(Unit.UnitData.Id + "的" + SkillData.Id + "AttackStart,pointDelay:" + duration + ",fullDuration" + fullDuration + ",beginDuration" + beginDuration + ",Time:" + Time.time);
                if (duration == 0)
                {
                    Cast();
                }
            }
        }

        public override void Cast()
        {
            Effect(null);
            CastExSkill();
            if (SkillData.BurstCount > 0)
            {
                Burst();
            }
        }

        public override void Effect(Unit target)
        {
            if (SkillData.Bullet != null)
            {
                //创建一个子弹
                var startPoint = Unit.UnitModel.GetPoint(SkillData.ShootPoint);
                //Debug.Log($"攻击{target.Config.Name}:{target.Hp} 起点：{startPoint}");
                Battle.CreateBullet(SkillData.Bullet.Value, startPoint, startPoint + new Vector3(Unit.Direction.x, 0, Unit.Direction.y).normalized * 20f, target, this);
            }
        }
    }
}
