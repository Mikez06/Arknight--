public class UnitData : IConfig 
{
      public string Id { get ; set ; }
      public string Type;
      public string Model;
      public string Name;
      public int Hp;
      public int Attack;
      public int Defence;
      public int MagicDefence;
      public int Weight;
      public int Cost;
      public int ResetTime;
      public int Hatred;
      public float HitPointX;
      public float HitPointY;
      public float AttackPointX;
      public float AttackPointY;
      public int[] Skills;
      public int[] MainSkill;
      public float Height;
      public bool CanSetHigh;
      public bool CanSetGround;
      public int StopCount;
      public int Damage;
      public float Speed;
      public float Radius;
      public bool CanStop;
      public UnitTypeEnum Profession;
      public string HeadIcon;
      public string StandPic;
      public int Rare;
      public string[] Tags;
}