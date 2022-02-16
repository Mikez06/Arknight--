public class EffectData : IConfig 
{
      public string Id { get ; set ; }
      public string Prefab;
      public int StartPos;
      public UnityEngine.Vector3 Offset;
      public string BindPoint;
      public bool BoneFollow;
      public int ParentFollow;
      public int ScaleXFollow;
      public int ForwordDirection;
      public bool FaceCamera;
      public float LifeTime;
}
