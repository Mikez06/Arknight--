using System.Collections;
using UnityEngine;

public class BulletModel : MonoBehaviour
{
    public Bullet Bullet;

    public void Init(Bullet bullet)
    {
        this.Bullet = bullet;
        transform.position = bullet.Position;

        var vec = Vector3.Cross(Bullet.Position - Camera.main.transform.position, Bullet.Direction);

        transform.rotation = Quaternion.LookRotation(Bullet.Direction, vec);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Bullet.Position;
        var vec = Vector3.Cross(Bullet.Position - Camera.main.transform.position, Bullet.Direction);

        transform.rotation = Quaternion.LookRotation(Bullet.Direction, vec);
    }
}