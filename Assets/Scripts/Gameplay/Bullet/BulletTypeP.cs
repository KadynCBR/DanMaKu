using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTypeP : BulletBase
{
    public float speed;

    public override void Movement()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }


}
