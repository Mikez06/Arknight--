public class EventData : IConfig 
{
      public string Id { get ; set ; }
      public string Group;
      public string EventId;
      public string Description;
      public string[] Choices;
      public string[] ChoicesTitle;
      public string[] Next;
      public int[] Branch;
      public int[] Rewards;
      public string Background;
      public string Tile;
}
