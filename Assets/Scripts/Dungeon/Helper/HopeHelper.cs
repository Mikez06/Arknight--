using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class HopeHelper
{
    public static int GetCost(Dungeon dungeon,int unitId,out bool ifUpgrade)
    {
        ifUpgrade = false;
        UnitData unitData = Database.Instance.Get<UnitData>(unitId);
        if (unitData.Rare <= 3) return 0;
        else if (dungeon.AllCards.Any(x => x.CardData.units[0] == unitId))
        {
            ifUpgrade = true;
            return unitData.Rare - 3;
        }
        else
        {
            return (unitData.Rare - 3) * 2;
        }
    }
}
