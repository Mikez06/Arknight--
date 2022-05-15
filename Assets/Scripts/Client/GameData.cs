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
    //public string MainPageUnitId;
    public float Bgm = 1;

    public void Init()
    {
        var str = SaveHelper.LoadFile("/data.sav");
        if (!string.IsNullOrEmpty(str))
        {
            try
            {
                instance = JsonHelper.FromJson<GameData>(str);
                foreach (var t in instance.Teams)
                {
                    var a = t.Cards.ToArray();
                    t.Cards.Clear();
                    foreach (var c in a)
                    {
                        t.Cards.Add(instance.Cards.LastOrDefault(x => x.UnitId == c.UnitId));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"读取存档失败，错误信息:\n{e}");
            }
        }

        List<int> ids = new List<int>();
        for (int i = instance.Cards.Count - 1; i >= 0; i--)
        {
            if (ids.Contains(instance.Cards[i].Id))
            {
                instance.Cards.RemoveAt(i);
            }
            else
                ids.Add(instance.Cards[i].Id);
        }

        foreach (var unitConfig in Database.Instance.GetAll<CardData>())
        {
            if (instance.Cards.Any(x => unitConfig.units.Contains(x.Id))) continue;
            var unitdata = Database.Instance.Get<UnitData>(unitConfig.units.Last());
            Card card = new Card()
            {
                UnitId = unitdata.Id,
                Level = unitdata.Level,
                Upgrade = unitdata.Upgrade,
            };
            if (card.UnitData.MainSkill != null) card.DefaultUsingSkill = card.UnitData.MainSkill.Length - 1;
            instance.Cards.Add(card);
        }
        if (instance.Teams[0] == null)
        {
            for (int i = 0; i < Instance.Teams.Length; i++)
            {
                instance.Teams[i] = new Team();
                foreach (var unitId in Database.Instance.GetAll<SystemData>()[0].StartUnits)
                {
                    var card = Cards.Find(x => x.Id == unitId);
                    instance.Teams[i].Cards.Add(card);
                    if (card.UnitData.MainSkill == null)
                        instance.Teams[i].UnitSkill.Add(0);
                    else
                        instance.Teams[i].UnitSkill.Add(card.UnitData.MainSkill.Length - 1);
                }
            }
            //MainPageUnitId = Cards[0].UnitId;
            Name = "玩家名字";
            SaveHelper.SaveData();
        }
    }
}
