using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DungeonCard : ICard
{
    public CardData CardData => Database.Instance.Get<CardData>(CardId);
    public UnitData UnitData => Database.Instance.Get<UnitData>(CardData.units[UpgradeUp]);

    public int UnitId => CardData.units[UpgradeUp];

    public int Upgrade => UnitData.Upgrade;

    public int Level => UnitData.Level;

    public int CardId;
    public int UpgradeUp;
    public int Exp;
    public int UsingSkill;

    public int GetUpgradeExp()
    {
        return 5;
    }

    public void GainExp(int exp)
    {
        Exp += exp;
        while (Exp > GetUpgradeExp() && UpgradeUp < 2)
        {
            Exp -= GetUpgradeExp();
            UpgradeUp++;
        }
    }
}