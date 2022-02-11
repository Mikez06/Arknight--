using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bullet
{
    public BulletData BulletData => Database.Instance.Get<BulletData>(Id);
    public int Id;

    public BulletModel BulletModel;

    public Vector3 StartPosition;
    public Vector3 Position;
    public Vector3 Direction;

    protected Battle Battle => Skill.Unit.Battle;
    public Skill Skill;
    public Unit Target;
    public Vector3 TargetPos;

    public virtual void Init()
    {
        StartPosition = Position;
        CreateModel();
    }

    public virtual void CreateModel()
    {
        BulletModel = ResHelper.Instantiate(PathHelper.EffectPath + BulletData.Model).GetComponent<BulletModel>();
        BulletModel.GetComponent<Effect>().SetLifeTime(float.PositiveInfinity);
        BulletModel.Init(this);
    }

    public virtual void Update()
    {

    }

    public virtual void Finish()
    {
        Battle.Bullets.Remove(this);
        GameObject.Destroy(BulletModel.gameObject);
    }
}

