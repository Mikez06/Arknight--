public class MapData : IConfig 
{
      public string Id { get ; set ; }
      public string MapName;
      public string Description;
      public string Scene;
      public int InitCost;
      public int MaxBuildCount;
      public string MapModel;
      public int[] Contracts;
}
