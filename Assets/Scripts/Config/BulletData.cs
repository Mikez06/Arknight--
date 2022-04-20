public class BulletData : IConfig 
{
      public string Id { get ; set ; }
      public string Type;
      public string Model;
      public string Line;
      public float Speed;
      public int FaceCamera;
      public int ScaleX;
      public int EffectBase;
      public System.Collections.Generic.Dictionary<string,object> Data;
}
