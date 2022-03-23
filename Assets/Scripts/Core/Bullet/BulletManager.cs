using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BulletManager
{
    public static BulletManager Instance => instance == null ? instance = new BulletManager() : instance;
    private static BulletManager instance;

    Pool<BulletModel> Pool=new Pool<BulletModel>();
    Pool<PullLineModel> Pool1 = new Pool<PullLineModel>();

    public BulletModel Get(string model)
    {
        var result= Pool.Spawn(ResHelper.GetAsset<GameObject>(PathHelper.EffectPath + model).GetComponent<BulletModel>(), Vector3.zero);
        return result;
    }

    public void Return(BulletModel bulletModel)
    {
        Pool.Despawn(bulletModel);
    }

    public PullLineModel GetLine(string model)
    {
        var result = Pool1.Spawn(ResHelper.GetAsset<GameObject>(PathHelper.EffectPath + model).GetComponent<PullLineModel>(), Vector3.zero);
        return result;
    }

    public void Return(PullLineModel bulletModel)
    {
        Pool1.Despawn(bulletModel);
    }
}
