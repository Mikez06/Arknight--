using UnityEngine;


public static class PathHelper
{     /// <summary>
      ///应用程序外部资源路径存放路径(热更新资源路径)
      /// </summary>
    public static string AppHotfixResPath
    {
        get
        {
            string game = Application.productName;
            string path = AppResPath;
            if (Application.isMobilePlatform)
            {
                path = $"{Application.persistentDataPath}/{game}/";
            }
            return path;
        }

    }

    /// <summary>
    /// 应用程序内部资源路径存放路径
    /// </summary>
    public static string AppResPath
    {
        get
        {
            return Application.streamingAssetsPath;
        }
    }

    /// <summary>
    /// 应用程序内部资源路径存放路径(www/webrequest专用)
    /// </summary>
    public static string AppResPath4Web
    {
        get
        {
#if UNITY_IOS || UNITY_STANDALONE_OSX
                return $"file://{Application.streamingAssetsPath}";
#else
            return Application.streamingAssetsPath;
#endif

        }
    }

    public static string DataPath = "Assets/Bundles/Data/";
    public static string UIPath = "Assets/Bundles/UI/";
    public static string SpritePath = "Assets/Bundles/Image/StandPic/";
    public static string OtherPath = "Assets/Bundles/Other/";
    public static string UnitPath = "Assets/Bundles/Units/";
    public static string BulletPath = "Assets/Bundles/Bullet/";
    public static string EffectPath = "Assets/Bundles/Effect/";
    public static string DungeonGridPath = "Assets/Bundles/DungeonTile/";
    //public static string TimelinePath = "Bundles/Effect/Timeline/";
}
