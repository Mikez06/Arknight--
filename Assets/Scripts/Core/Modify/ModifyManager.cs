using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ModifyManager
{
    public static ModifyManager Instance => instance == null ? instance = new ModifyManager() : instance;
    private static ModifyManager instance;

    //Dictionary<int, Modify> modifies = new Dictionary<int, Modify>();

    //public void Init()
    //{
    //    ModifyData[] array = Database.Instance.GetAll<ModifyData>();
    //    for (int i = 0; i < array.Length; i++)
    //    {
    //        ModifyData data = array[i];
    //        Modify modify = typeof(Unit).Assembly.CreateInstance(nameof(Modifys) + "." + data.Type) as Modify;
    //        modify.Id = i;
    //        modify.Init();
    //        modifies.Add(i, modify);
    //    }
    //}

    public Modify Get(int id,Skill skill)
    {
        ModifyData data = Database.Instance.Get<ModifyData>(id);
        Modify modify = typeof(Unit).Assembly.CreateInstance(nameof(Modifys) + "." + data.Type) as Modify;
        modify.Id = id;
        modify.Skill = skill;
        modify.Init();
        return modify;
    }
}

