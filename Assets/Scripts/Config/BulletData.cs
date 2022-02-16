public class BulletData : IConfig 
{
      public string Id { get ; set ; }
      public string Type;
      public string Model;
      public float Speed;
      public int FaceCamera;
      public int ScaleX;
      public System.Collections.Generic.Dictionary<string,object> Data;
}
