public class MapData : IConfig 
{
      public string Id { get ; set ; }
      public string MapName;
      public string Description;
      public string Scene;
      public int InitHp;
      public int InitCost;
      public int MaxBuildCount;
      public int MaxCost;
      public string MapModel;
      public int[] Contracts;
}
