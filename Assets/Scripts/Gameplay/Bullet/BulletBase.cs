using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletInstructionType
{
    FORWARDVELOCITY,
    ANGULARVELOCITY
}

[System.Serializable]
public class BulletInstruction
{
    public BulletInstructionType instructionType;
    public int frameStart;
    public int frameEnd;
    public float value;

    public void PerformInstruction(BulletBase bb, bool toremove = false)
    {
        int addremovemultiplier = 1;
        if (toremove) addremovemultiplier = -1;
        switch (instructionType)
        {
            case BulletInstructionType.FORWARDVELOCITY:
                bb.forwardVelocity += value * addremovemultiplier;
                break;
            case BulletInstructionType.ANGULARVELOCITY:
                bb.angularVelocity += value * addremovemultiplier;
                break;
        }
    }

}

public class BulletBase : MonoBehaviour
{
    public BulletType bulletType;
    public float lifetime;
    protected float _lifetime;
    public float damage;
    public bool isFriendly;
    public bool piercing;
    private BulletManager bulletManager;
    public bool enemyBullet { get; private set; }
    private int currentFrame;

    [Header("Danmaku Settings")]
    public float forwardVelocity;
    public float angularVelocity;
    public Vector3 offset;
    public List<BulletInstruction> bulletInstructions;

    public virtual void OnEnable()
    {
        _lifetime = lifetime;
        currentFrame = 0;
    }

    public void FixedUpdate()
    {
        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0)
        {
            DisableBullet();
        }
        Instructions();
        Movement();
        Timers();
    }

    public virtual void Instructions()
    {
        foreach (BulletInstruction b in bulletInstructions)
        {
            if (currentFrame == b.frameStart)
            {
                b.PerformInstruction(this);
            }
            else if (currentFrame == b.frameEnd)
            {
                b.PerformInstruction(this, true);
            }
        }
    }

    public virtual void Movement()
    {
        transform.position += transform.forward * forwardVelocity * Time.deltaTime;
        transform.rotation = transform.rotation * Quaternion.Euler(0, angularVelocity, 0);
    }

    public virtual void Timers()
    {
        currentFrame++;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        // Other collisions are handled on trigger enter in their respective damageables.
        if (other.gameObject.tag == "environment" || other.gameObject.tag == "ground")
            CollideEnvironment(other);
    }

    public virtual void CollideEnvironment(Collider other)
    {
        //  // DisableBullet on enviornment collision
        //     if (bulletType == BulletType.TypeC)
        //     {
        //         SpawnExplosion();
        //         AOEDamageCheck();
        //     }
        DisableBullet();
    }

    public virtual void DisableBullet()
    {
        this.gameObject.SetActive(false);
        bulletManager.QueueBullet(this.gameObject, bulletType);
    }

    public void EnableBullet()
    {
        this.enabled = true;
    }

    public void SetBulletManager(BulletManager bm)
    {
        bulletManager = bm;
    }

}
