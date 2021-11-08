﻿using System.Collections;
using UnityEngine;

public class BulletModel : MonoBehaviour
{
    public Bullet Bullet;

    public void Init(Bullet bullet)
    {
        this.Bullet = bullet;
        transform.position = bullet.Position;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Bullet.Position;
        //transform.eulerAngles = Bullet.Direction;
        transform.rotation = Quaternion.LookRotation(Bullet.Direction, Vector3.right);
    }
}