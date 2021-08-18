using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class DataHelper
{
    public static int GetInt(this object[] self, int index, int defaultValue = 0)
    {
        if (self == null || self.Length <= index) return defaultValue;
        return Convert.ToInt32(self[index]);
    }

    public static float GetFloat(this object[] self, int index, float defaultValue = 0)
    {
        if (self == null || self.Length <= index) return defaultValue;
        return Convert.ToSingle(self[index]);
    }

    public static int GetInt(this Dictionary<string, object> self, string key, int defaultValue = 0)
    {
        if (self == null) return defaultValue;
        if (self.TryGetValue(key, out object r))
        {
            return Convert.ToInt32(r);
        }
        return defaultValue;
    }

    public static float GetFloat(this Dictionary<string, object> self, string key, float defaultValue = 0)
    {
        if (self == null) return defaultValue;
        if (self.TryGetValue(key, out object r))
        {
            return Convert.ToSingle(r);
        }
        return defaultValue;
    }

    public static bool GetBool(this Dictionary<string, object> self, string key)
    {
        if (self == null) return false;
        if (self.TryGetValue(key, out object r))
        {
            return Convert.ToBoolean(r);
        }
        return false;
    }

    public static string GetStr(this Dictionary<string, object> self, string key, string defaultValue = null)
    {
        if (self == null) return defaultValue;
        if (self.TryGetValue(key, out object r))
        {
            return Convert.ToString(r);
        }
        return defaultValue;
    }

    public static object[] GetArray(this Dictionary<string, object> self, string key)
    {
        if (self == null) return null;
        if (self.TryGetValue(key, out object r))
        {
            return (r as Newtonsoft.Json.Linq.JArray).ToObject<string[]>();
        }
        return null;
    }

    public static bool StringsEqual(this string[] self,string[] target)
    {
        if (self == target) return true;
        if (self == null && target == null) return true;
        if (self == null || target == null) return false;
        if (self.Length != target.Length) return false;
        for(int i = 0; i < self.Length; i++)
        {
            if (self[i] != target[i]) return false;
        }
        return true;
    }
}

