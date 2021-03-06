using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("EffectManager");
                DontDestroyOnLoad(gameObject);
                instance = gameObject.AddComponent<EffectManager>();
            }
            return instance;
        }
    }
    private static EffectManager instance;
    public Pool<Effect> pool = new Pool<Effect>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Effect GetEffect(int id)
    {
        //ResourcesManager.Instance.LoadBundle(PathHelper.EffectPath + name);
        //var result = pool.Spawn(ResourcesManager.Instance.GetAsset<GameObject>(PathHelper.EffectPath + name, name).GetComponent<Effect>(), Vector3.zero, null);
        var config = Database.Instance.Get<EffectData>(id);
        var result = pool.Spawn(ResHelper.GetAsset<GameObject>(PathHelper.EffectPath + config.Prefab).GetComponent<Effect>(), Vector3.zero, Quaternion.identity);//.GetComponent<Effect>();
        result.Id = id;
        result.SetLifeTime(config.LifeTime);
        return result;
    }

    public void ReturnEffect(Effect ps)
    {
        pool.Despawn(ps);
    }
    public void ReturnAll()
    {
        pool.DespawnAll();
    }
}
