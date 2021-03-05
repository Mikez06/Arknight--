using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public Battle Battle;
    public UnitConfig Config => Database.Instance.Get<UnitConfig>(Id);
    public int Id;

    public UnitModel UnitModel;

    public Vector3 Position;
    public Vector2 Position2 => new Vector2(Position.x, Position.z);

    public Vector2Int GridPos => new Vector2Int(Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.z));

    public Vector2 Direction = new Vector2(1, 0);

    public MapGrid NowGrid => Battle.Map.Grids[Mathf.RoundToInt(Position.x), Mathf.RoundToInt(Position.z)];

    public StateEnum State = StateEnum.Default;
    public float Hp;
    public float MaxHp;
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

    public string AnimationName = "Default";
    public float AnimationSpeed = 1;

    public virtual void Init()
    {
        if (Config.Skills != null)
            foreach (var skillId in Config.Skills)
            {
                var skillConfig = Database.Instance.Get<SkillConfig>(skillId);
                var skill = typeof(Unit).Assembly.CreateInstance(nameof(Skills) + "." + skillConfig.Type) as Skill;
                skill.Unit = this;
                skill.Init();
                Skills.Add(skill);
            }
        CreateModel();
        Refresh();
        Hp = MaxHp;
    }

    public virtual void Refresh()
    {
        Speed = Config.Speed;
        MaxHp = Config.Hp;
        Agi = 1;
    }

    public void UpdateBuffs()
    {

    }
    public virtual void UpdateAction()
    {
        
    }

    protected void UpdateDie()
    {

    }

    public virtual void DoDie()
    {
        State = StateEnum.Die;
        AnimationName = "Die";
        Dying.Set(UnitModel.SkeletonAnimation.skeleton.data.Animations.Find(x => x.Name == "Die").Duration);
    }

    public virtual void Finish()
    {
        Battle.Enemys.Remove(this);
        GameObject.Destroy(UnitModel.gameObject);
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
            SetStatus(StateEnum.Idle);
        }
    }

    protected virtual void UpdateMove()
    {

    }

    public void SetStatus(StateEnum state)
    {
        this.State = state;
        AnimationName = state.ToString();
        AnimationSpeed = 1;
        if (state != StateEnum.Attack && !Attacking.Finished())
        {
            Attacking.Finish();
        }
    }

    public void CreateModel()
    {
        ResourcesManager.Instance.LoadBundle(PathHelper.UnitPath + Config.Model);
        GameObject go = GameObject.Instantiate(ResourcesManager.Instance.GetAsset<GameObject>(PathHelper.UnitPath + Config.Model, Config.Model));
        UnitModel = go.GetComponent<UnitModel>();
        UnitModel.Unit = this;
        UnitModel.Init();
    }
}
