using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesExportTool : MonoBehaviour
{
    public bool 场景模式;
    public string Path;
    public string SceneName;
    // Start is called before the first frame update
    void Start()
    {
        ABExportTool.Init();
        if (场景模式)
        {
            ABExportTool.CopyFile(Path);
        }
        else
        {
            if (string.IsNullOrEmpty(SceneName))
            {
                ABExportTool.SaveAssets(Path);
            }
            else
            {
                ABExportTool.LoadScene(Path, SceneName);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
