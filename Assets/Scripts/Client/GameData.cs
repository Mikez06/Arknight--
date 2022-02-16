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
    public Team[] Teams = new Team[4];
    public string Name;
    public int MainPageUnitId;

    public void TestInit()
    {
        foreach (var unitConfig in Database.Instance.GetAll<CardData>())
        {
            var unitdata = Database.Instance.Get<UnitData>(unitConfig.units.Last());
            Card card = new Card()
            {
                UnitId = Database.Instance.GetIndex(unitdata),
                Level = unitdata.Level,
                Upgrade = unitdata.Upgrade,
            };
            Cards.Add(card);
        }

        for (int i = 0; i < Instance.Teams.Length; i++)
        {
            Teams[i] = new Team();
            foreach (var unitId in Database.Instance.GetAll<SystemData>()[0].StartUnits)
            {
                Teams[i].Cards.Add(Cards.Find(x => x.UnitId == unitId));
                Teams[i].UnitSkill.Add(0);
            }
        }
        MainPageUnitId = Cards[0].UnitId;
        Name = "玩家名字";
    }
}
