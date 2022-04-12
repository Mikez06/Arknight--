using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EnemyInfoExcelTool : MonoBehaviour
{
    public TextAsset DataJson;
    // Start is called before the first frame update
    void Start()
    {
        StringBuilder sb = new StringBuilder();
        //string s = "1\t2\t3\t\r\n4\t5\t";
        //UnityEngine.GUIUtility.systemCopyBuffer = s;
       var a = JsonHelper.FromJson<Data>(DataJson.text);
        foreach (var ei in a.enemies)
        {
            sb.Append(ei.Key);
            sb.Append('\t');
            sb.Append(ei.Value[0].enemyData.name.m_value);
            sb.Append('\t');
            int index0 = ei.Key.IndexOf("_");
            int index1 = ei.Key.IndexOf('_', index0 + 1);
            sb.Append(ei.Key.Substring(index0 + 1, index1 - index0 - 1));
            sb.Append('\t');
            sb.Append(ei.Key.Substring(index1 + 1, ei.Key.Length - index1 - 1));
            sb.Append('\t');
            sb.Append(ei.Value[0].enemyData.description.m_value);
            sb.Append('\t');
            sb.Append(ei.Value[0].enemyData.attributes["maxHp"].m_value);
            sb.Append('\t');
            sb.Append(ei.Value[0].enemyData.attributes["atk"].m_value);
            sb.Append('\t');
            sb.Append(ei.Value[0].enemyData.attributes["def"].m_value);
            sb.Append('\t');
            sb.Append(ei.Value[0].enemyData.attributes["magicResistance"].m_value);
            sb.Append('\t');
            sb.Append(ei.Value[0].enemyData.attributes["blockCnt"].m_value);
            sb.Append('\t');
            sb.Append(ei.Value[0].enemyData.attributes["moveSpeed"].m_value);
            sb.Append('\t');
            sb.Append(ei.Value[0].enemyData.attributes["baseAttackTime"].m_value);
            sb.Append('\t');
            sb.Append(ei.Value[0].enemyData.attributes["hpRecoveryPerSec"].m_value);
            sb.Append('\t');
            sb.Append(ei.Value[0].enemyData.attributes["tauntLevel"].m_value);
            sb.Append("\r\n");
        }
        UnityEngine.GUIUtility.systemCopyBuffer = sb.ToString();
       // {
       //     Debug.Log($"输出 {ei.Value[0].enemyData.name.m_value} 基本数据");
       //     foreach (var kv in ei.Value[0].enemyData.attributes)
       //     {
       //         Debug.Log($"{kv.Key},{kv.Value.m_value}");
       //     }
       // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class Data
    {
        public EnemyInfo[] enemies;
    }
    public class EnemyInfo
    {
        public string Key;
        public EnemyValue[] Value;
    }
    public class EnemyValue
    {
        public int level;
        public EnemyData enemyData;
    }
    public class EnemyData
    {
        public kv name;
        public kv description;
        public Dictionary<string, kv> attributes;
    }

    public class kv
    {
        //public bool m_defined;
        public object m_value;
    }
}
