using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Log
{
    public static void Debug(object msg)
    {
        UnityEngine.Debug.Log(msg);
    }

    public static void Error(object msg)
    {
        UnityEngine.Debug.LogError(msg);
    }
}

