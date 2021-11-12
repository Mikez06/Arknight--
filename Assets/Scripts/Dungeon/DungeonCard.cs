using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DungeonCard
{
    public CardData CardData => Database.Instance.Get<CardData>(CardId);
    public UnitData UnitData => Database.Instance.Get<UnitData>(CardData.units[Upgrade]);
    public int CardId;
    public int Upgrade;
    public int Exp;
}