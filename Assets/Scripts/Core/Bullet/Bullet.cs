using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bullet
{
    public BulletConfig Config => Database.Instance.Get<BulletConfig>(Id);
    public int Id;

    public BulletModel BulletModel;

    public Vector3 Postion;

    protected Battle Battle => Skill.Unit.Battle;
    public Skill Skill;
    public Unit Target;
    public Vector3 TargetPos;

    public virtual void Init()
    {
        CreateModel();
    }

    public virtual void CreateModel()
    {
        ResourcesManager.Instance.LoadBundle(PathHelper.BulletPath + Config.Model);
        BulletModel = GameObject.Instantiate(ResourcesManager.Instance.GetAsset<GameObject>(PathHelper.BulletPath + Config.Model, Config.Model)).GetComponent<BulletModel>();
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

