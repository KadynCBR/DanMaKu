using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuEmitter : MonoBehaviour
{
    private BulletManager bulletManager;
    private int multiplr;
    private int timer;
    public bool isDmode;
    public int firerate;
    public float ao;

    public void Start()
    {
        ao = 0;
        multiplr = 1;
        bulletManager = GameObject.Find("BulletManager").GetComponent<BulletManager>();
    }

    // Theres a lot I can do with just this

    public void SpawnBulletsCircle(int numBullets, float angularOffset = 0)
    {
        multiplr *= -1;
        float angle = 360 / numBullets;
        float rollingAngle = 0;
        for (int i = 0; i < numBullets; i++)
        {
            BulletBase bb = bulletManager.SpawnBulletRetval(BulletType.TypeD, transform.position, Quaternion.Euler(0, rollingAngle + angularOffset, 0));
            bb.forwardVelocity = 3 + 1 * multiplr;
            bb.angularVelocity = .75f * multiplr;
            rollingAngle += angle;
        }
    }

    public void toggle()
    {
        isDmode = !isDmode;
    }

    void FixedUpdate()
    {
        if (!isDmode) return;
        timer++;
        if (timer >= firerate)
        {
            timer = 0;
            SpawnBulletsCircle(10);
            ao += 1f;
        }
    }
}
