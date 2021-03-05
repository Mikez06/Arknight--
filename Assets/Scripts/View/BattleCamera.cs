using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    public static BattleCamera Instance;
    public bool BuildMode;
    public GameObject g;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BuildMode)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                g.transform.position = hit.point - new Vector3(0, 0.5f, 0);
            }
        }
    }

    public void StartBuild(Unit unit)
    {
        BuildMode = true;
        g = unit.UnitModel.gameObject;
        transform.Translate(Vector3.left);
    }

    public void SetUnit()
    {
        BuildMode = false;
    }

    public void EndBuild()
    {
        transform.Translate(Vector3.left);
    }
}
