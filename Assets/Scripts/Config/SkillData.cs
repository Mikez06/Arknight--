public class SkillData : IConfig 
{
      public string Id { get ; set ; }
      public string Type;
      public string Name;
      public string Desc;
      public SkillReadyEnum ReadyType;
      public SkillUseTypeEnum UseType;
      public TriggerEnum Trigger;
      public int MaxUseCount;
      public int[] EnableBuff;
      public int[] DisableBuff;
      public bool AutoUse;
      public bool NoTargetAlsoUse;
      public bool RegetTarget;
      public bool StopBreak;
      public bool StopOtherSkill;
      public int TargetTeam;
      public bool AttackFly;
      public bool UseEventTarget;
      public SkillTargetFilterEnum TargetFilter;
      public UnitTypeEnum ProfessionLimit;
      public AttackTargetOrderEnum AttackOrder;
      public AttackTargetOrder2Enum AttackOrder2;
      public UnityEngine.Vector2Int[] AttackPoints;
      public float AttackRange;
      public bool AttackAreaWithMain;
      public DamageTypeEnum DamageType;
      public bool IfHeal;
      public float DamageRate;
      public int DamageBase;
      public int DamageCount;
      public int BurstCount;
      public float BurstDelay;
      public bool BurstFind;
      public float AreaRange;
      public float AreaDamage;
      public int PushPower;
      public int CostCount;
      public int[] Skills;
      public int[] ExSkills;
      public float Cooldown;
      public float OpenTime;
      public float StartPower;
      public float MaxPower;
      public int PowerCount;
      public PowerRecoverTypeEnum PowerType;
      public string[] ModelAnimation;
      public string[] OverwriteAnimation;
      public AttackModeEnum AttackMode;
      public int? Bullet;
      public string ShootPoint;
      public int[] Modifys;
      public int[] BuffRemoves;
      public int[] Buffs;
      public float[] BuffData;
      public float? BuffLastTime;
      public System.Collections.Generic.Dictionary<string,object> Data;
      public int? ReadyEffect;
      public int? StartEffect;
      public int? HitEffect;
      public int? EffectEffect;
      public string Icon;
      public bool AntiHide;
}
