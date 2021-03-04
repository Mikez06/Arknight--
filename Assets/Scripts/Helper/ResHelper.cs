using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//TODO 资源池
public class ResHelper
{
    public static GameObject GetUnit(string unitName)
    {
        ResourcesManager.Instance.LoadBundle(PathHelper.UnitPath + unitName);
        return GameObject.Instantiate(ResourcesManager.Instance.GetAsset<GameObject>(PathHelper.UnitPath + unitName, unitName));
    }
}

