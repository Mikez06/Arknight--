using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.Linq;
using MongoDB.Bson.IO;

public class ExcelExportEditor
{
    static string excelPath = "./Excel/";
    static string exportPath = "./Assets/Bundles/Data/";

    static List<string> eUnits;
    static List<string> eCards;
    static List<string> eBuffs;
    static List<string> eBullets;
    static List<string> eTeam;
    static List<string> eMaps;
    static List<string> eItems;
    static List<string> eShops;
    static List<string> eSkills;
    static List<string> eWaves;

    static Dictionary<string, Func<string, string>> dic = new Dictionary<string, Func<string, string>>()
        {
            { "Unit",(x)=>n(x,eUnits) },
            { "Unit[]",(x)=>nArray(x,eUnits) },
             { "Card",(x)=>n(x,eCards) },
            { "Card[]",(x)=>nArray(x,eCards) },

             { "Bullet",(x)=>n(x,eBullets) },
            { "Bullet[]",(x)=>nArray(x,eBullets) },
            { "Buff",(x)=>n(x,eBuffs) },
            { "Buff[]",(x)=>nArray(x,eBuffs) },
            { "Team",(x)=>n(x,eTeam) },
            { "Team[]",(x)=>nArray(x,eTeam) },
            { "Map",(x)=>n(x,eMaps) },
            { "Map[]",(x)=>nArray(x,eMaps) },
            { "Item",(x)=>n(x,eItems) },
            { "Item[]",(x)=>nArray(x,eItems) },
            { "Shop",(x)=>n(x,eShops) },
            { "Shop[]",(x)=>nArray(x,eShops) },
            { "Skill",(x)=>n(x,eSkills) },
            { "Skill[]",(x)=>nArray(x,eSkills) },
            { "Wave",(x)=>n(x,eWaves) },
            { "Wave[]",(x)=>nArray(x,eWaves) },
    };

    [MenuItem("Tools/导出配置")]
    private static void ExportAll()
    {
        eUnits = Export("battle.xlsx", "unit", "_Id");
        //eCards = Export("battle.xlsx", "card", "_Id");
        //eBuffs = Export("battle.xlsx", "buff", "_Id");
        eBullets = Export("battle.xlsx", "bullet", "_Id");
        //eTeam = Export("battle.xlsx", "team", "_Id");
        eMaps = Export("battle.xlsx", "map", "_Id");
        //eItems = Export("battle.xlsx", "item", "_Id");
        //eShops = Export("battle.xlsx", "shop", "_Id");
        eSkills = Export("battle.xlsx", "skill", "_Id");
        eWaves = Export("battle.xlsx", "wave", "_Id");

        var unitConfigs = Export<UnitConfig>("battle.xlsx", "unit", "UnitConfig.txt");
        var skillConfigs = Export<SkillConfig>("battle.xlsx", "skill", "SkillConfig.txt");
        var bulletConfigs = Export<BulletConfig>("battle.xlsx", "bullet", "BulletConfig.txt");
        var mapConfigs = Export<BulletConfig>("battle.xlsx", "map", "MapConfig.txt");
        var waveConfigs = Export<WaveConfig>("battle.xlsx", "wave", "WaveConfig.txt");

        AssetDatabase.Refresh();
        Debug.Log("导出结束");
    }

    public static void writeData(string fileName, string obj)
    {
        FileStream txt = new FileStream(exportPath + fileName, FileMode.Create);
        StreamWriter sw = new StreamWriter(txt);
        sw.Write(obj);
        sw.Close();
        txt.Close();
    }

    static List<string> Export(string fileName, string tableName, string fieldName)
    {
        List<string> result = new List<string>();
        IExcelDataReader reader;
        FileStream file = new FileStream(excelPath + fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        reader = ExcelReaderFactory.CreateReader(file);
        foreach (System.Data.DataTable sheet in reader.AsDataSet().Tables)
        {
            if (sheet.TableName != tableName) continue;
            int cellCount = sheet.Columns.Count;

            for (int j = 0; j < cellCount; j++)
            {
                if (fieldName != GetCellString(sheet, 0, j)) continue;
                for (int i = 2; i < sheet.Rows.Count; i++)
                {
                    string Id = GetCellString(sheet, i, 0);
                    if (string.IsNullOrEmpty(Id) || Id.StartsWith("#"))
                    {
                        continue;
                    }
                    result.Add(GetCellString(sheet, i, j));
                }
            }
        }
        return result;
    }

    static List<T> Export<T>(string fileName, string tableName,string writeName)
    {
        List<T> result = new List<T>();
        IExcelDataReader reader;
        FileStream file = new FileStream(excelPath + fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        reader = ExcelReaderFactory.CreateReader(file);
        foreach (System.Data.DataTable sheet in reader.AsDataSet().Tables)
        {
            StringBuilder w = new StringBuilder();
            w.Append("[");
            if (sheet.TableName != tableName) continue;
            int cellCount = sheet.Columns.Count;
            for (int i = 2; i < sheet.Rows.Count; i++)
            {
                string Id = GetCellString(sheet, i, 0);
                try
                {
                    if (string.IsNullOrEmpty(Id) || Id.StartsWith("#"))
                    {
                        continue;
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append("{");
                    for (int j = 0; j < cellCount; j++)
                    {
                        string fieldName = GetCellString(sheet, 0, j);
                        string fieldType = GetCellString(sheet, 1, j);
                        if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldType))
                        {
                            continue;
                        }
                        string fieldValue = GetCellString(sheet, i, j);
                        if (string.IsNullOrEmpty(fieldValue))
                        {
                            continue;
                        }
                        if (fieldName == "Id" || fieldName == "_id")
                        {
                            fieldName = "_id";
                        }
                        sb.Append($"\"{fieldName}\":{Convert(fieldType, fieldValue)},");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("}");
                    //Debug.Log(sb);
                    T t = MongoHelper.FromJson<T>(sb.ToString());
                    w.Append($"{sb},");
                    //Debug.Log(MongoHelper.ToJson(t));
                    result.Add(t);
                }
                catch (Exception e)
                {
                    throw new Exception(fileName + " " + tableName + "," + Id + "\n" + e.ToString());
                }
            }
            if (w.Length > 1)
                w.Remove(w.Length - 1, 1);
            w.Append("]");
            writeData(writeName, w.ToString());
        }


        //reader.Close();
        //reader.Dispose();
        file.Close();
        file.Dispose();
        return result;
    }
    private static string GetCellString(System.Data.DataTable sheet, int i, int j)
    {
        try
        {
            return sheet.Rows[i][j].ToString();
        }
        catch
        {
            return "";
        }
    }
    static StringBuilder sbCache = new StringBuilder();
    static Func<string, List<string>, string> n = (x, l) =>
    {
        int index = l.IndexOf(x);
        if (index == -1) throw new Exception($"{x} ");
        return index.ToString();
    };

    static Func<string, List<string>, string> nArray = (x, l) =>
    {
        sbCache.Clear();
        string[] sp = x.Split(',');
        foreach (var s in sp)
        {
            int index = l.IndexOf(s);
            if (index == -1) throw new Exception($"{x} {s} {MongoHelper.ToJson(l)}");
            sbCache.Append($"\"{index}\",");
        }
        sbCache.Remove(sbCache.Length - 1, 1);
        return $"[{sbCache.ToString()}]";
    };

    private static string Convert(string type, string value)
    {
        try
        {
            if (type.EndsWith("Enum[]"))
            {
                Type t = typeof(Database).Assembly.GetType(type.Substring(0, type.Length - 2));
                sbCache.Clear();
                string[] sp = value.Split(',');
                foreach (string s in sp) sbCache.Append((int)Enum.Parse(t, s) + ",");
                sbCache.Remove(sbCache.Length - 1, 1);
                return $"[{sbCache.ToString()}]";
            }
            if (type.EndsWith("Enum"))
            {
                try
                {
                    Type t = typeof(Database).Assembly.GetType(type);
                    return ((int)Enum.Parse(t, value)).ToString();
                }
                catch
                {
                    return value;
                }
            }
            int index;

            if (dic.ContainsKey(type))
            {
                return dic[type].Invoke(value);
            }

            switch (type)
            {
                case "int[]":
                case "int32[]":
                case "long[]":
                case "object[]":
                    return $"[{value}]";
                case "string[]":
                    sbCache.Clear();
                    string[] sp = value.Split(',');
                    if (sp.Length == 1) sp = value.Split('\n');//如果用,分隔失败，尝试用回车分隔
                    foreach (string s in sp) sbCache.Append($"\"{s}\",");
                    sbCache.Remove(sbCache.Length - 1, 1);
                    return $"[{sbCache.ToString()}]";
                case "int":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                    return value;
                case "string":
                    return $"\"{value}\"";
                case "bool":
                    return value;
                case "Data":
                    return $"{{{value}}}";
                case "Vector2":
                case "Vector2Int":
                    sp = value.Split(',');
                    return $"{{\"x\":{sp[0] },\"y\":{sp[1]}}}";
                case "Vector2Int[]":
                case "Vector2[]":
                    sbCache.Clear();
                    string[] s1 = value.Split('#');
                    foreach (var s in s1)
                    {
                        sp = s.Split(',');
                        sbCache.Append($"{{\"x\":{sp[0] },\"y\":{sp[1]}}},");
                    }
                    sbCache.Remove(sbCache.Length - 1, 1);
                    return $"[{sbCache.ToString()}]";
                default:
                    Debug.LogWarning($"unexcpeted type:{type}");
                    if (type.EndsWith("[]"))
                    {
                        return $"[{value}]";
                    }
                    return value;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw e;
        }
    }
}