using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ExcelExportEditor
{
    static string excelPath = "./Excel/";
    static string exportPath = "./Assets/Bundles/Data/";
    static string scriptPath = "./Assets/Scripts/Config/";

    static List<string> eUnit;
    static List<string> eBuff;
    static List<string> eSkill;
    static List<string> eItem;
    static List<string> eEquip;
    static List<string> eJob;


    static Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>()
    {

    };

    [MenuItem("Tools/导出配置")]
    public static void ExportAll()
    {
        ExportClass();
        ExportData();
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

    static void ExportClass()
    {
        dic.Clear();
        //读取所有表的Id，用于转换索引
        foreach (var path in Directory.GetFiles(excelPath, "*.xlsx"))
        {
            if (path.Contains("$")) continue;
            IExcelDataReader reader;
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            reader = ExcelReaderFactory.CreateReader(file);
            foreach (System.Data.DataTable sheet in reader.AsDataSet().Tables)
            {
                if (sheet.TableName.StartsWith("#")) continue;
                List<string> Ids = new List<string>();
                dic.Add(sheet.TableName, Ids);
                for (int i = 3; i < sheet.Rows.Count; i++)
                {
                    var Id = GetCellString(sheet, i, 0);
                    if (string.IsNullOrEmpty(Id) || Id.StartsWith("#")) continue;
                    Ids.Add(Id);
                }
            }
        }

        foreach (var path in Directory.GetFiles(excelPath, "*.xlsx"))
        {
            if (path.Contains("$")) continue;
            IExcelDataReader reader;
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            reader = ExcelReaderFactory.CreateReader(file);
            foreach (System.Data.DataTable sheet in reader.AsDataSet().Tables)
            {
                if (sheet.TableName.StartsWith("#")) continue;
                FileStream txt = new FileStream(scriptPath + sheet.TableName + ".cs", FileMode.Create);
                StreamWriter sw = new StreamWriter(txt);
                sw.Write($"public class {sheet.TableName} : IConfig \n");
                sw.Write("{\n");
                sw.Write("      public string Id { get ; set ; }\n");
                for (int i = 0; i < sheet.Columns.Count; i++)
                {
                    var fieldInfo = GetCellString(sheet, 0, i);
                    if (fieldInfo.StartsWith("#") || GetCellString(sheet, 1, i) == "Id") continue;
                    string fieldType = GetCellString(sheet, 2, i);
                    if (string.IsNullOrEmpty(fieldType)) continue;
                    if (dic.ContainsKey(fieldType)) fieldType = "int?";
                    if (dic.ContainsKey(fieldType.TrimEnd("[]".ToCharArray()))) fieldType = "int[]";
                    sw.Write($"      public {fieldType} {GetCellString(sheet, 1, i)};\n");
                }
                sw.Write("}\n");
                sw.Close();
                txt.Close();
            }
        }
    }

    static void ExportData()
    {
        foreach (var path in Directory.GetFiles(excelPath, "*.xlsx"))
        {
            if (path.Contains("$")) continue;
            Export(path);
        }
    }

    static void Export(string fileName)
    {
        IExcelDataReader reader;
        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        reader = ExcelReaderFactory.CreateReader(file);
        foreach (System.Data.DataTable sheet in reader.AsDataSet().Tables)
        {
            if (sheet.TableName.StartsWith("#")) continue;
            StringBuilder sb = new StringBuilder();
            int cellCount = sheet.Columns.Count;
            for (int i = 3; i < sheet.Rows.Count; i++)
            {
                string Id = GetCellString(sheet, i, 0);
                if (string.IsNullOrEmpty(Id) || Id.StartsWith("#"))
                {
                    continue;
                }
                sb.Append("{");
                for (int j = 0; j < cellCount; j++)
                {
                    string fieldName = GetCellString(sheet, 1, j);
                    string fieldType = GetCellString(sheet, 2, j);
                    if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldType))
                    {
                        continue;
                    }
                    string fieldValue = GetCellString(sheet, i, j);
                    if (string.IsNullOrEmpty(fieldValue))
                    {
                        continue;
                    }
                    sb.Append($"\"{fieldName}\":{Convert(fieldType, fieldValue)},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("}\n");
            }
            if (sb.Length > 1) sb.Remove(sb.Length - 1, 1);

            FileStream txt = new FileStream(exportPath + sheet.TableName + ".txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(txt);
            sw.Write(sb.ToString());
            sw.Close();
            txt.Close();
        }
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

    private static string Convert(string type, string value)
    {
        try
        {
            if (type.EndsWith("Enum[]"))
            {
                Type t = typeof(Init).Assembly.GetType(type.Substring(0, type.Length - 2));
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
                    Type t = typeof(Init).Assembly.GetType(type);
                    return ((int)Enum.Parse(t, value)).ToString();
                }
                catch
                {
                    return value;
                }
            }

            if (dic.ContainsKey(type))
            {
                int index = dic[type].IndexOf(value);
                if (index == -1) throw new Exception(value);
                return index.ToString();
            }

            if (dic.ContainsKey(type.TrimEnd("[]".ToCharArray())))
            {
                sbCache.Clear();
                sbCache.Append("[");
                string[] sp = value.Split(',');
                foreach (var s in sp)
                {
                    int index = dic[type.TrimEnd("[]".ToCharArray())].IndexOf(s);
                    if (index == -1) throw new Exception(value);
                    sbCache.Append($"{index},");
                }
                if (sbCache.Length > 1) sbCache.Remove(sbCache.Length - 1, 1);
                sbCache.Append("]");
                return sbCache.ToString();
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
                case "UnityEngine.Vector2":
                case "UnityEngine.Vector2Int":
                    sp = value.Split(',');
                    return $"{{\"x\":{sp[0]},\"y\":{sp[1]}}}";
                case "UnityEngine.Vector2[]":
                case "UnityEngine.Vector2Int[]":
                    sbCache.Clear();
                    sp = value.Split('#');
                    for (int i = 0; i < sp.Length; i++)
                    {
                        var sp1 = sp[i].Split(',');
                        sbCache.Append( $"{{\"x\":{sp1[0]},\"y\":{sp1[1]}}}");
                        if (i != sp.Length - 1) sbCache.Append(",");
                    }
                    return $"[{sbCache}]";
                case "UnityEngine.Rect":
                    sp = value.Split(',');
                    return $"{{\"x\":{sp[0]},\"y\":{sp[1]},\"width\":{sp[2]},\"height\":{sp[3]}}}";
                default:
                    //Debug.LogWarning($"unexcpeted type:{type}");
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