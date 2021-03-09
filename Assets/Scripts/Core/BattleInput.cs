using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BattleInput
{
    public string MapName;
    public UnitInput[] UnitInputs;
    public int Seed;
    public int StartCost;
}

public class UnitInput
{
    public int Id;
}