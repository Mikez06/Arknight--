using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
public class logdata
{
    public string output = "";
    public string stack = "";
    public static logdata Init(string o, string s)
    {
        logdata log = new logdata();
        log.output = o;
        log.stack = s;
        return log;
    }
    public void Show(/*bool showstack*/)
    {
        GUILayout.Label(output);
        //if (showstack)
        GUILayout.Label(stack);
    }
}
/// <summary>
/// 手机调试脚本
/// 本脚本挂在一个空对象或转换场景时不删除的对象便可
/// 错误和异常输出日记路径 Application.persistentDataPath
/// </summary>
public class ShowDebugInPhone : MonoBehaviour
{

    List<logdata> logDatas = new List<logdata>();//log链表
    List<logdata> errorDatas = new List<logdata>();//错误和异常链表
    List<logdata> warningDatas = new List<logdata>();//警告链表

    static List<string> mWriteTxt = new List<string>();
    Vector2 uiLog;
    Vector2 uiError;
    Vector2 uiWarning;
    bool open = false;
    bool showLog = false;
    bool showError = false;
    bool showWarning = false;
    private string outpath;
    void Start()
    {
        //Application.persistentDataPath Unity中只有这个路径是既能够读也能够写的。
        //Debug.Log(Application.persistentDataPath);
        outpath = Application.persistentDataPath + "/outLog.txt";
        //每次启动客户端删除以前保存的Log
        if (System.IO.File.Exists(outpath))
        {
            File.Delete(outpath);
        }
        //转换场景不删除
        Application.DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {
        //注册log监听
        Application.RegisterLogCallback(HangleLog);
    }

    void OnDisable()
    {
        // Remove callback when object goes out of scope
        //当对象超出范围，删除回调。
        Application.RegisterLogCallback(null);
    }
    void HangleLog(string logString, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Log:
                logDatas.Add(logdata.Init(logString, stackTrace));
                break;
            case LogType.Error:
            case LogType.Exception:
                errorDatas.Add(logdata.Init(logString, stackTrace));
                mWriteTxt.Add(logString);
                mWriteTxt.Add(stackTrace);
                break;
            case LogType.Warning:
                warningDatas.Add(logdata.Init(logString, stackTrace));
                break;
        }
    }
    void Update()
    {
        //由于写入文件的操做必须在主线程中完成，因此在Update中才给你写入文件。
        if (errorDatas.Count > 0)
        {
            string[] temp = mWriteTxt.ToArray();
            foreach (string t in temp)
            {
                using (StreamWriter writer = new StreamWriter(outpath, true, Encoding.UTF8))
                {
                    writer.WriteLine(t);
                }
                mWriteTxt.Remove(t);
            }
        }
    }
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(">>Open", GUILayout.Height(150), GUILayout.Width(150)))
            open = !open;
        if (open)
        {
            if (GUILayout.Button("清理", GUILayout.Height(150), GUILayout.Width(150)))
            {
                logDatas = new List<logdata>();
                errorDatas = new List<logdata>();
                warningDatas = new List<logdata>();
            }
            if (GUILayout.Button("显示log日志:" + showLog, GUILayout.Height(150), GUILayout.Width(200)))
            {
                showLog = !showLog;
                if (open == true)
                    open = !open;
            }
            if (GUILayout.Button("显示error日志:" + showError, GUILayout.Height(150), GUILayout.Width(200)))
            {
                showError = !showError;
                if (open == true)
                    open = !open;
            }
            if (GUILayout.Button("显示warning日志:" + showWarning, GUILayout.Height(150), GUILayout.Width(200)))
            {
                showWarning = !showWarning;
                if (open == true)
                    open = !open;
            }
        }
        GUILayout.EndHorizontal();
        if (showLog)
        {
            GUI.color = Color.white;
            uiLog = GUILayout.BeginScrollView(uiLog);
            foreach (var va in logDatas)
            {
                va.Show();
            }
            GUILayout.EndScrollView();
        }
        if (showError)
        {
            GUI.color = Color.red;
            uiError = GUILayout.BeginScrollView(uiError);
            foreach (var va in errorDatas)
            {
                va.Show();
            }
            GUILayout.EndScrollView();
        }
        if (showWarning)
        {
            GUI.color = Color.yellow;
            uiWarning = GUILayout.BeginScrollView(uiWarning);
            foreach (var va in warningDatas)
            {
                va.Show();
            }
            GUILayout.EndScrollView();
        }
    }
}
