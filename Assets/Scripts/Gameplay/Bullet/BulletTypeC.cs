using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTypeC : BulletBase
{
    public Vector3 target;
    public float accuracyNoise;
    public float anticipationTime;
    private float _anticipationTime;
    public float scale;
    public float explosionForce;
    public LayerMask playerMask;
    public ParticleSystem vfx;
    private bool placed;
    private bool exploded;

    public override void OnEnable()
    {
        base.OnEnable();
        placed = false;
        exploded = false;
        _anticipationTime = anticipationTime;
        transform.localScale = Vector3.one * scale;
    }

    public override void Movement()
    {
        // If we haven't placed it yet, place it.
        if (!placed)
        {
            float x_pos = target.x + Random.Range(-accuracyNoise, accuracyNoise);
            float y_pos = target.y; // this should typically be ground level.
            float z_pos = target.y + Random.Range(-accuracyNoise, accuracyNoise);
            transform.position = new Vector3(x_pos, y_pos, z_pos);
            placed = true;
            vfx.Play();
        }
    }

    public override void Timers()
    {
        _anticipationTime -= Time.deltaTime;
        if (_anticipationTime <= 0 && !exploded)
        {
            AOEDamageCheck();
            exploded = true;
        }
    }

    public override void CollideEnvironment(Collider other)
    {
        return;
    }

    void AOEDamageCheck()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, transform.lossyScale.x, playerMask);
        if (others.Length > 0)
        {
            foreach (Collider other in others)
            {
                if (other.gameObject.tag != "Player")
                    continue;
                // TwinStickController playcon = other.gameObject.GetComponent<TwinStickController>();
                // ImpactReceiver playconinpactreceiver = other.gameObject.GetComponent<ImpactReceiver>();
                // playcon.GetHit(this);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, transform.lossyScale.x);
    }
}
