public class SkillData : IConfig 
{
      public string Id { get ; set ; }
      public string Type;
      public string Desc;
      public SkillReadyEnum ReadyType;
      public SkillUseTypeEnum UseType;
      public bool AutoUse;
      public string Name;
      public bool AttackStop;
      public bool RegetTarget;
      public bool StopBreak;
      public bool StopOtherSkill;
      public int TargetTeam;
      public bool AttackFly;
      public AttackTargetOrderEnum AttackOrder;
      public AttackTargetOrder2Enum AttackOrder2;
      public UnityEngine.Vector2Int[] AttackPoints;
      public DamageTypeEnum DamageType;
      public float DamageRate;
      public int DamageCount;
      public int BurstCount;
      public float BurstDelay;
      public bool BurstFind;
      public float AreaRange;
      public float AreaDamage;
      public int[] Skills;
      public int[] ExSkills;
      public float Cooldown;
      public float OpenTime;
      public int StartPower;
      public int MaxPower;
      public int PowerCount;
      public PowerRecoverTypeEnum PowerType;
      public bool IfHeal;
      public string ModelAnimation;
      public string OverwriteAnimation;
      public AttackModeEnum AttackMode;
      public int? Bullet;
      public string ShootPoint;
      public int[] Buffs;
      public float[] BuffData;
      public string StartEffect;
      public string HitEffect;
      public string EffectEffect;
      public float AttackRange;
      public string Icon;
}
