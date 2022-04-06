using System.Collections;
using UnityEngine;

public class BulletModel : MonoBehaviour
{
    public Bullet Bullet;

    public void Init(Bullet bullet)
    {
        this.Bullet = bullet;
        transform.position = bullet.Position;

        var dir = Vector3.ProjectOnPlane(Bullet.Direction, Camera.main.transform.forward);
        var angle = Vector3.SignedAngle(dir, Vector3.right, Vector3.up);
        transform.eulerAngles = new Vector3(60, 0, angle);

        GetComponent<Effect>()?.Play();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Bullet.Position;
        //var vec = Vector3.Cross(Bullet.Position - Camera.main.transform.position, Bullet.Direction);

        //transform.rotation = Quaternion.LookRotation(Bullet.Direction, vec);

        var dir = Vector3.ProjectOnPlane(Bullet.Direction, Camera.main.transform.forward);
        var angle = Vector3.SignedAngle(dir, Vector3.right, Vector3.up);
        transform.eulerAngles = new Vector3(60, 0, angle);
    }
}