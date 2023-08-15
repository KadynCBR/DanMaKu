using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Beam bullet, 
// Meant to be turned on and off, so not necessary to talk with bullet manager.
// Really a boss only type of bullet so it shouldn't be spawning this.
public class BulletTypeE : BulletBase
{
    public float hitCoolDown;
    private float hitTimer;
    public float impactForce;

    public override void Timers()
    {
        hitTimer += Time.deltaTime;
    }
    // This isn't meant to be disabled.
    public override void DisableBullet() { }


    public override void CollideEnvironment(Collider other) { }

    public override void OnTriggerEnter(Collider other) { }

    public void OnTriggerStay(Collider other)
    {
        if (hitTimer > hitCoolDown && other.gameObject.tag == "Player")
        {
            Debug.Log("IMPLEMENT HARM PLAYER");
        }
    }
}
