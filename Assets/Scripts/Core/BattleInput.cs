using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BattleInput
{
    public string MapName;
    public int Seed;
    public List<int> Contracts;

    //两者选一个传入，表示不同模式
    public Team Team;
    public Dungeon Dungeon;
}