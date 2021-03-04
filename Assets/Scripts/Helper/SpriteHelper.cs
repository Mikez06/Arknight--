using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using FairyGUI;

public static class SpriteHelper
{
    public static void SetSprite(this GLoader self,Sprite sprite)
    {
        if (self.texture == null)
        {
            self.texture = new NTexture(sprite);
        }
        else
        {
            self.texture.Reload(sprite.texture, null);
        }
    }

    public static Sprite GetSprite(string atlaName, string spriteName)
    {
        try
        {
            ResourcesManager.Instance.LoadBundle(atlaName);
            return (ResourcesManager.Instance.GetAsset<Sprite>(atlaName, spriteName));

        }
        catch (Exception e)
        {
            return null;
        }
    }

    public static Sprite GetStandPic(string name)
    {
        Debug.Log(name);
        ResourcesManager.Instance.LoadBundle(PathHelper.SpritePath + "StandPic/" + name);
        return ResourcesManager.Instance.GetAsset<Sprite>(PathHelper.SpritePath + "StandPic/" + name, name);
    }
}
