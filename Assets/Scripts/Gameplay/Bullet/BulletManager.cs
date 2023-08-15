using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Instead of individual entities instantiating bullets,a singular bullet manager will handle it.
// if more bullets than are available are required, bullet manager will make it
// and then store it for later use.

// Should probably consider turning this into a singleton to ensure only one bulletmanager and 
// make it easier to use.

public enum BulletType
{
    TypeA, // Basic
    TypeB, // Parryable Basic
    TypeH, // Homing
    TypePlayer,
    TypeC, // missile
    TypeD, // impactforce
    TypeE, // Beam
}

public class BulletManager : MonoBehaviour
{
    public bool isManufacturing;
    // To get rid of these I could do some whacky string match + resources...
    // But i think this is okay for now. 
    public GameObject TypeAPrefab;
    public GameObject TypeBPrefab;
    public GameObject TypePPrefab;
    public GameObject TypeHPrefab;
    public GameObject TypeCPrefab;
    public GameObject TypeDPrefab;

    private List<GameObject> bulletPrefabs;
    private List<Transform> holders;
    private List<Queue<GameObject>> availableBullets;

    void Start()
    {
        availableBullets = new List<Queue<GameObject>>();
        holders = new List<Transform>();
        bulletPrefabs = new List<GameObject>();

        // List indicies are going to be handled by ENUM Value.
        foreach (int i in Enum.GetValues(typeof(BulletType)))
        {
            string name = Enum.GetName(typeof(BulletType), i);
            // Add queue
            availableBullets.Add(new Queue<GameObject>());
            // Create and add holder
            GameObject ge = new GameObject($"{name}Holder");
            ge.transform.parent = this.transform;
            holders.Add(ge.transform);
        }
        // Bullet prefabs MUST be added in order according to the BulletType enum.
        bulletPrefabs.Add(TypeAPrefab);
        bulletPrefabs.Add(TypeBPrefab);
        bulletPrefabs.Add(TypeHPrefab);
        bulletPrefabs.Add(TypePPrefab);
        bulletPrefabs.Add(TypeCPrefab);
        bulletPrefabs.Add(TypeDPrefab);
        isManufacturing = true;
    }

    public void StopManufacture() { isManufacturing = false; }
    public void ResumeManufacture() { isManufacturing = true; }

    public void RemoveAllBullets()
    {
        foreach (Transform holder in holders)
        {
            foreach (Transform bullet in holder)
            {
                BulletBase bulletBase = bullet.gameObject.GetComponent<BulletBase>();
                bulletBase.DisableBullet();
            }
        }
    }

    public void QuickStop()
    {
        StopManufacture();
        RemoveAllBullets();
    }

    public bool SpawnBullet(BulletType bulletType, Vector3 position, Quaternion orientation, Transform target = null)
    {
        if (!isManufacturing)
            return false;
        GameObject bullet = GetBullet(bulletType);
        bullet.transform.position = position;
        bullet.transform.rotation = orientation;
        if (bulletType == BulletType.TypeH)
        {
            // bullet.GetComponent<BulletTypeH>().homingTarget = target;
        }
        bullet.SetActive(true);
        return true;
    }

    public BulletBase SpawnBulletRetval(BulletType bulletType, Vector3 position, Quaternion orientation, Transform target = null)
    {
        if (!isManufacturing)
            return null;
        GameObject bullet = GetBullet(bulletType);
        bullet.transform.position = position;
        bullet.transform.rotation = orientation;
        if (bulletType == BulletType.TypeH)
        {
            // bullet.GetComponent<BulletTypeH>().homingTarget = target;
        }
        bullet.SetActive(true);
        return bullet.GetComponent<BulletBase>();
    }

    GameObject GetBullet(BulletType bulletType)
    {
        // If there's a bullet in queue, it was added there by the bullet itself.
        if (availableBullets[(int)bulletType].Count != 0)
            return availableBullets[(int)bulletType].Dequeue();

        // Else, create a new one 
        // Put it in holder
        // return that (This lets our "magazine" grow automatically.)
        GameObject b = Instantiate(bulletPrefabs[(int)bulletType]);
        b.transform.parent = holders[(int)bulletType];
        BulletBase bb = b.GetComponent<BulletBase>();
        bb.SetBulletManager(this);
        return b;
    }

    public void QueueBullet(GameObject inactiveBullet, BulletType bulletType)
    {
        // allow bullets themselves to line back up.
        availableBullets[(int)bulletType].Enqueue(inactiveBullet);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            QuickStop();
    }
}
