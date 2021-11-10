using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Highlight : MonoBehaviour {
    [SerializeField] MeshRenderer myMeshRenderer, myMeshRenderer2;
    private Material myMaterial,material2;


    void Start () {
        myMaterial = myMeshRenderer.material;
        material2 = myMeshRenderer2.material;
    }

    // Update is called once per frame
    void Update () {
        myMaterial.mainTextureOffset = new Vector2(Time.time * 5, 0);
        material2.mainTextureOffset = new Vector2(Time.time * 5, 0);
    }
}
