public class BuffData : IConfig 
{
      public string Id { get ; set ; }
      public string Type;
      public string Name;
      public float LastTime;
      public System.Collections.Generic.Dictionary<string,object> Data;
}
