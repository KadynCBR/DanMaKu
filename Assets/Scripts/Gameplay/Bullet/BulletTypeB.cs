using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTypeB : BulletBase
{
    public float speed;

    public override void Movement()
    {
        transform.position += transform.forward * -speed * Time.deltaTime;
    }

}
