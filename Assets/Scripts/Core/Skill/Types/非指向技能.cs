using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Skills
{
    //与普通技能不同的是，就算攻击范围里没有有效模板，这类技能也能被释放
    public class 非指向技能 : Skill
    {
        public override void Update()
        {
            if (SkillData.PowerType == PowerRecoverTypeEnum.自动)
            {
                RecoverPower(Unit.PowerSpeed * SystemConfig.DeltaTime);
            }

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
            if (!Useable()) return;
            if (SkillData.MaxUseCount != 0 && UseCount >= SkillData.MaxUseCount) return;//使用次数不在ready里判断，因为被动技能不会走ready的逻辑

            //走到这里技能就真的用出来了
            UseCount++;
            //Debug.Log(Unit.UnitData.Id + "的" + SkillData.Id + "使用次数:" + UseCount);
            if (SkillData.ReadyType == SkillReadyEnum.充能释放)
            {
                Power -= MaxPower;
                Opening.Set(SkillData.OpenTime);
            }

            if (SkillData.ModelAnimation == null)
            {
                //Debug.Log(Unit.UnitData.Id + "的" + SkillData.Id + "没有动画,直接使用");
                ResetCooldown(1);
                if (SkillData.AnimationTime == null)
                    Cast();
                else
                    Casting.Set(SkillData.AnimationTime.Value);
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

            if (SkillData.StartEffect != null)
            {
                foreach (var id in SkillData.StartEffect)
                {
                    var ps = EffectManager.Instance.GetEffect(id);
                    ps.Init(Unit, Unit, Unit.Position, Unit.Direction);
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

        protected override void Burst()
        {
            if (BurstCount == -1)
            {
                BurstCount = SkillData.BurstCount;
                LastTargets.Clear();
                LastTargets.AddRange(Targets);
            }
            else
            {
                if (SkillData.BurstFind || SkillData.RegetTarget) //当目标为随机时
                {
                    LastTargets.Clear();
                    LastTargets.AddRange(GetAttackTarget());
                }
                Effect(null);
            }
            BurstCount--;
            if (BurstCount != -1)
                if (SkillData.BurstDelay > 0)
                    Bursting.Set(SkillData.BurstDelay);
                else
                    Burst();
        }
    }
}
