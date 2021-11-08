public class BuffData : IConfig 
{
      public string Id { get ; set ; }
      public string Type;
      public string Name;
      public bool IfSwitch;
      public int? LastingEffect;
      public float LastTime;
      public int? Upgrade;
      public System.Collections.Generic.Dictionary<string,object> Data;
}
