public class RelicData : IConfig 
{
      public string Id { get ; set ; }
      public string Icon;
      public string Effect;
      public string Descripiton;
      public string Name;
      public int Rare;
      public UnitTypeEnum[] Profession;
      public int CardCount;
      public int BuildCount;
      public int Hope;
      public int Gold;
      public float GoldDropRate;
      public int[] Skills;
}
