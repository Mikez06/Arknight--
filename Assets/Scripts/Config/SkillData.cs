public class SkillData : IConfig 
{
      public string Id { get ; set ; }
      public string Type;
      public string Desc;
      public SkillReadyEnum ReadyType;
      public SkillUseTypeEnum UseType;
      public string Name;
      public AttackTargetEnum AttackTarget;
      public bool AttackStop;
      public int TargetTeam;
      public bool AttackFly;
      public AttackTargetOrderEnum AttackOrder;
      public UnityEngine.Vector2Int[] AttackPoints;
      public DamageTypeEnum DamageType;
      public float DamageRate;
      public int HitCount;
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
      public int? Bullet;
      public string ModelAnimation;
      public int[] Buffs;
      public float[] BuffData;
      public string StartEffect;
      public string HitEffect;
      public string EffectEffect;
      public float AttackRange;
      public string Icon;
}
