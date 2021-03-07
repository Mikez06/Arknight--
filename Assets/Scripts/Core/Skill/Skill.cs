using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Skill
{
    public Unit Unit;

    public Unit Target;
    protected Battle Battle => Unit.Battle;

    public SkillConfig Config => Database.Instance.Get<SkillConfig>(Id);

    public int Id;

    public List<Vector2Int> AttackPoints;

    public virtual void Init()
    {
        if (Config.AttackPoints != null)
        {
            AttackPoints = new List<Vector2Int>();
            UpdateAttackPoints();
        }
    }

    public virtual void Update()
    {

    }

    public virtual bool CanUse()
    {
        return true;
    }

    public virtual void Cast()
    {

    }

    public virtual void Hit(Unit target)
    {

    }

    public virtual void FindTarget()
    {

    }

    public void UpdateAttackPoints()
    {
        AttackPoints.Clear();
        foreach (var p in Config.AttackPoints)
        {
            var point = (Unit as Units.干员).PointWithDirection(p);
            if (point.x < 0 || point.x >= Battle.Map.Grids.GetLength(0) || point.y < 0 || point.y >= Battle.Map.Grids.GetLength(1)) continue;
            AttackPoints.Add(point);
        }
    }
}

