using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

public class SaveHelper
{
    public const string SingleSaveKey = "SingleData";

    public static string LoadFile(string fileName)
    {
        string filePath = PathHelper.AppHotfixResPath + fileName;
        if (!File.Exists(filePath))
        {
            return string.Empty;
        }

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                stream.Close();
                return Encoding.UTF8.GetString(bytes);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        return string.Empty;
    }

    public static void DeleteFile(string fileName)
    {
        string filePath = PathHelper.AppHotfixResPath + fileName;
        if (File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }

    public static string Save(string str,string fileName)
    {
        //var fileName = (errorSave ? "error_" : "") + System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".txt";
        var dir = System.IO.Path.GetDirectoryName(PathHelper.AppHotfixResPath + fileName);
        Debug.Log(dir);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        FileStream txt = new FileStream(PathHelper.AppHotfixResPath + fileName, FileMode.Create);
        StreamWriter sw = new StreamWriter(txt);
        sw.Write(str);
        sw.Close();
        txt.Close();
        return dir + fileName;
    }
}