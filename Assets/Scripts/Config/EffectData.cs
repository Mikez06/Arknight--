public class EffectData : IConfig 
{
      public string Id { get ; set ; }
      public string Prefab;
      public int StartPos;
      public UnityEngine.Vector3 Offset;
      public string BindPoint;
      public bool BoneFollow;
      public bool ForwardOnly;
      public int ParentFollow;
      public int ScaleXFollow;
      public int ForwordDirection;
      public float FaceCamera;
      public float LifeTime;
}
