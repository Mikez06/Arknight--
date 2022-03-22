public class SkillData : IConfig 
{
      public string Id { get ; set ; }
      public string Type;
      public string Name;
      public string Desc;
      public int Upgrade;
      public SkillReadyEnum ReadyType;
      public SkillUseTypeEnum UseType;
      public TriggerEnum Trigger;
      public int MaxUseCount;
      public int[] EnableBuff;
      public int[] DisableBuff;
      public int[] TargetEnableBuff;
      public int[] TargetDisableBuff;
      public bool OpenDisable;
      public float TargetHpLess;
      public float TargetHpMore;
      public bool AutoUse;
      public bool NoTargetAlsoUse;
      public bool RegetTarget;
      public bool StopBreak;
      public float EffectiveRate;
      public bool StopOtherSkill;
      public int TargetTeam;
      public bool AttackFly;
      public bool UseEventUser;
      public bool UseEventTarget;
      public SkillTargetFilterEnum TargetFilter;
      public bool DeadFind;
      public UnitTypeEnum ProfessionLimit;
      public int[] UnitLimit;
      public int RareLimit;
      public int PosLimit;
      public int CostLimit;
      public AttackTargetOrderEnum AttackOrder;
      public AttackTargetOrder2Enum AttackOrder2;
      public UnityEngine.Vector2Int[] AttackPoints;
      public bool AttackPoint;
      public float AttackRange;
      public bool AttackAreaWithMain;
      public DamageTypeEnum DamageType;
      public bool IfHeal;
      public float DamageRate;
      public bool DamageWithFrameRate;
      public int DamageBase;
      public float LifeSteal;
      public int DamageCount;
      public int BurstCount;
      public float BurstDelay;
      public bool BurstFind;
      public float AreaRange;
      public float AreaMainDamage;
      public float AreaDamage;
      public int PushPower;
      public int CostCount;
      public int[] Skills;
      public int[] ExSkills;
      public int[] ExSkillWeight;
      public int? UpgradeSkill;
      public float Cooldown;
      public float OpenTime;
      public float StartPower;
      public float MaxPower;
      public int PowerCount;
      public PowerRecoverTypeEnum PowerType;
      public PowerRecoverTypeEnum PowerUseType;
      public string[] ModelAnimation;
      public float AnimationTime;
      public string[] ModelAnimationDown;
      public string[] OverwriteAnimation;
      public string[] OverwriteAnimationDown;
      public AttackModeEnum AttackMode;
      public int? Bullet;
      public string ShootPoint;
      public int[] Modifys;
      public int[] ModifyDatas;
      public int[] BuffRemoves;
      public int[] Buffs;
      public float[] BuffData;
      public float[] BuffData2;
      public float[] BuffData3;
      public float? BuffLastTime;
      public float[] BuffChance;
      public bool BuffRely;
      public System.Collections.Generic.Dictionary<string,object> Data;
      public int? ReadyEffect;
      public int[] StartEffect;
      public int[] CastEffect;
      public int? HitEffect;
      public int? EffectEffect;
      public int? GatherEffect;
      public int? LoopStartEffect;
      public int? LoopCastEffect;
      public string Icon;
      public bool AntiHide;
      public bool CanDestory;
      public bool NotAttackFlag;
}
