using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameData
{
    public static GameData Instance => instance != null ? instance : instance = new GameData();
    private static GameData instance;

    public List<Card> Cards = new List<Card>();
    public string Name;
    public int MainPageUnitId;

    public void TestInit()
    {
        foreach (var unitConfig in Database.Instance.GetAll<UnitConfig>())
        {
            if (unitConfig.Type == "干员")
            {
                Card card = new Card()
                {
                    UnitId = Database.Instance.GetIndex(unitConfig),
                    Level = UnityEngine.Random.Range(1, 51),                    
                };
                Cards.Add(card);
            }
        }
        MainPageUnitId = Cards[0].UnitId;
        Name = "玩家名字";
    }
}
