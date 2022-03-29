public class ContractData : IConfig 
{
      public string Id { get ; set ; }
      public string Icon;
      public string Name;
      public string Description;
      public int Group;
      public int[] Skills;
      public int TeamLimit;
      public UnitTypeEnum[] ProfessionLimit;
      public int MapHp;
      public int Level;
}
