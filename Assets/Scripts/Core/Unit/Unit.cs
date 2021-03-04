using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Unit
{
    public Battle Battle;
    public UnitConfig Config => Database.Instance.Get<UnitConfig>(Id);
    public int Id;

    public UnitModel UnitModel;

    public Vector3 Position;
    public Vector2 Position2 => new Vector2(Position.x, Position.z);

    public Vector2Int GridPos => new Vector2Int(Mathf.CeilToInt(Position.x), Mathf.CeilToInt(Position.z));

    public MapGrid NowGrid => Battle.Map.Grids[Mathf.CeilToInt(Position.x), Mathf.CeilToInt(Position.z)];

    public UnitStateEnum State;
    public float Hp;
    public List<Skill> Skills = new List<Skill>();
    /// <summary>
    /// 攻速
    /// </summary>
    public float Agi;
    /// <summary>
    /// 移速
    /// </summary>
    public float Speed;

    /// <summary>
    /// 攻击动画
    /// </summary>
    public CountDown Attacking = new CountDown();
    /// <summary>
    /// 死亡动画
    /// </summary>
    public CountDown Dying = new CountDown();
    /// <summary>
    /// 硬直
    /// </summary>
    public CountDown Recover = new CountDown();

    public string AnimationName;
    public float AnimationSpeed;

    public virtual void Init()
    {
        foreach (var skillId in Config.Skills)
        {
            var skillConfig = Database.Instance.Get<SkillConfig>(skillId);
            var skill = typeof(Unit).Assembly.CreateInstance(nameof(Skills) + "." + skillConfig.Type) as Skill;
            skill.Unit = this;
            skill.Init();
            Skills.Add(skill);
        }
        CreateModel();
    }

    public void Refresh()
    {

    }

    public void UpdateBuffs()
    {

    }
    public virtual void UpdateAction()
    {
        if (this.State == UnitStateEnum.Die)
        {
            UpdateDie();
        }
        else
        {
            UpdateSkills();
            if (State == UnitStateEnum.move || State == UnitStateEnum.stand)
            {
                UpdateMove();
            }
        }
        Recover.Update(SystemConfig.DeltaTime);

        //TODO：Buff刷新动画状态
    }

    protected void UpdateDie()
    {

    }

    public virtual void DoDie()
    {
        SetStatus(UnitStateEnum.Die);
        Dying.Set(UnitModel.SkeletonAnimation.skeleton.data.Animations.Find(x => x.Name == "Die").Duration);
    }

    protected void UpdateSkills()
    {
        for (int i = Skills.Count - 1; i >= 0; i--)
        {
            if (i >= Skills.Count) continue;
            var sk = Skills[i];
            if (sk != null)
                sk.Update();
        }
        if (Attacking.Update(SystemConfig.DeltaTime))
        {
            SetStatus(UnitStateEnum.stand);
        }
    }

    protected virtual void UpdateMove()
    {

    }

    public void SetStatus(UnitStateEnum state)
    {
        this.State = state;
        AnimationName = state.ToString();
        AnimationSpeed = 1;
        if (state != UnitStateEnum.attack && !Attacking.Finished())
        {
            Attacking.Finish();
        }
    }

    public void CreateModel()
    {
        ResourcesManager.Instance.LoadBundle(PathHelper.UnitPath + Config.Model);
        GameObject go = ResourcesManager.Instance.GetAsset<GameObject>(PathHelper.UnitPath + Config.Model, Config.Model);
        UnitModel = go.GetComponent<UnitModel>();
        UnitModel.Unit = this;
        UnitModel.Init();
    }

    public void Destroy()
    {
        GameObject.Destroy(UnitModel.gameObject);
    }
}
