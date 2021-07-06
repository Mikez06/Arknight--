public class WaveData : IConfig 
{
      public string Id { get ; set ; }
      public string Map;
      public int? UnitId;
      public float Delay;
      public UnityEngine.Vector2Int[] Path;
      public float[] PathWait;
}
