using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

//TODO 资源池
public class ResHelper
{
    public static T GetAsset<T>(string path)
    {
        var op = Addressables.LoadAssetAsync<T>(path);
        op.WaitForCompletion();
        if (op.IsDone)
        {
            var result = op.Result;
            //Addressables.Release(op);
            return result;
        }
        else
        {
            throw new Exception();
        }
    }

    public static async Task<T> GetAssetAsync<T>(string path)
    {
        var op = Addressables.LoadAssetAsync<T>(path);
        await op.Task;
        return op.Task.Result;
    }

    public static GameObject Instantiate(string path)
    {
        var op = Addressables.InstantiateAsync(path);
        op.WaitForCompletion();
        return op.Result;
    }

    public static async Task<GameObject> InstantiateAsyncImmediate(string path)
    {
        //TaskCompletionSource<GameObject> tcs = new TaskCompletionSource<GameObject>();
        var op = Addressables.LoadAssetAsync<GameObject>(path);
        var g = await op.Task;
        var r = await Addressables.InstantiateAsync(path).Task;
        //Addressables.InstantiateAsync(path).Completed += (x) =>
        //{
        //    tcs.SetResult(x.Result);
        //};
        //var g= await tcs.Task;
        return r;
    }

    public static void Return(GameObject go)
    {
        Addressables.ReleaseInstance(go);
    }
    public static void Return<T>(T go)
    {
        Addressables.Release(go);
    }

    static HashSet<int> units = new HashSet<int>();
    public static async Task Prepare(int unitId,int mainSkillIndex=-1)
    {
#if UNITY_EDITOR
        return;
#endif
        if (units.Contains(unitId)) return;
        units.Add(unitId);
        UnitData unitData = Database.Instance.Get<UnitData>(unitId);
        await Addressables.LoadAssetAsync<GameObject>(PathHelper.UnitPath + unitData.Model).Task;
        if (unitData.Skills != null)
            foreach (var skillId in unitData.Skills)
            {
                await PrepareSkill(skillId);
            }
        if (mainSkillIndex >= 0 && unitData.MainSkill != null && unitData.MainSkill.Length > mainSkillIndex)
        {
            await PrepareSkill(unitData.MainSkill[mainSkillIndex]);
        }
    }   

    static HashSet<int> skills = new HashSet<int>();
    protected static async Task PrepareSkill(int skillId)
    {
        if (skills.Contains(skillId)) return;
        skills.Add(skillId);
        SkillData skillData = Database.Instance.Get<SkillData>(skillId);
        await PrepareEffect(skillData.ReadyEffect);
        await PrepareEffect(skillData.StartEffect);
        await PrepareEffect(skillData.CastEffect);
        await PrepareEffect(skillData.HitEffect);
        await PrepareEffect(skillData.EffectEffect);
        await PrepareEffect(skillData.GatherEffect);
        await PrepareEffect(skillData.LoopStartEffect);
        await PrepareEffect(skillData.LoopCastEffect);
        if (skillData.Buffs != null)
            foreach (var buff in skillData.Buffs)
            {
                await PrepareBuff(buff);
            }
        if (skillData.Bullet != null)
        {
            var b = Database.Instance.Get<BulletData>(skillData.Bullet.Value);
            if (!string.IsNullOrEmpty(b.Line)) await Addressables.LoadAssetAsync<GameObject>(PathHelper.EffectPath + b.Line).Task;
            if (!string.IsNullOrEmpty(b.Model)) await Addressables.LoadAssetAsync<GameObject>(PathHelper.EffectPath + b.Model).Task;
        }
        if (skillData.Skills != null)
        {
            foreach (var sk in skillData.Skills) await PrepareSkill(sk);
        }
        if (skillData.ExSkills != null)
        {
            foreach (var sk in skillData.ExSkills) await PrepareSkill(sk);
        }       
    }

    static async Task PrepareBuff(int buffId)
    {
        var effect = Database.Instance.Get<BuffData>(buffId).LastingEffect;
        await PrepareEffect(effect);
    }

    static async Task PrepareEffect(int[] effectIds)
    {
        if (effectIds == null) return;
        foreach (var effectId in effectIds)
            await Addressables.LoadAssetAsync<GameObject>(PathHelper.EffectPath + Database.Instance.Get<EffectData>(effectId).Prefab).Task;
    }
    static async Task PrepareEffect(int? effectId)
    {
        if (effectId == null) return;
        await Addressables.LoadAssetAsync<GameObject>(PathHelper.EffectPath + Database.Instance.Get<EffectData>(effectId.Value).Prefab).Task;
    }
}

