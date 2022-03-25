using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class UnitModel:MonoBehaviour
{
    public Unit Unit;

    public virtual void Init(Unit unit)
    {
        transform.position = unit.Position;
    }

    public virtual Vector3 GetModelPositon()
    {
        return transform.position;
    }

    public virtual Vector3 GetPoint(string name)
    {
        return transform.position;
    }

    public virtual void BreakAnimation()
    {

    }

    public virtual float GetSkillDelay(string[] animationName, string[] lastState, out float fullDuration, out float beginDuration)
    {
        fullDuration = 0;
        beginDuration = 0;
        return 0;
    }

    public virtual float GetAnimationDuration(string animationName)
    {
        return 0;
    }

    public virtual void SetColor(Color color)
    {

    }
    public void ShowCrit(DamageInfo damage)
    {
        BattleUI.UI_Battle.Instance.ShowDamageText(damage, 0, transform.position.WorldToUI());
    }

    public void ShowHeal(DamageInfo heal)
    {
        BattleUI.UI_Battle.Instance.ShowDamageText(heal, 1, transform.position.WorldToUI());
    }
    public void ShowMiss()
    {
        BattleUI.UI_Battle.Instance.ShowDamageText("", 2, transform.position.WorldToUI());
    }
    public void ShowPower(float count)
    {
        BattleUI.UI_Battle.Instance.ShowDamageText(count.ToString("F0"), 3, transform.position.WorldToUI());
    }
    public virtual void ChangeToEnd()
    {

    }
}

