public class ModifyData : IConfig 
{
      public string Id { get ; set ; }
      public string Type;
      public int? Buff;
      public System.Collections.Generic.Dictionary<string,object> Data;
}
