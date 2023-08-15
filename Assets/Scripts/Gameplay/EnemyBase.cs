using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    private GameObject deathFXPrefab;
    private BulletManager bulletManager;

    void Awake()
    {
        bulletManager = GameObject.Find("BulletManager").GetComponent<BulletManager>();
    }

    public void OnDeath()
    {
        GameObject g = Instantiate(deathFXPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void FireBullet()
    {
        bulletManager.SpawnBullet(BulletType.TypeA, transform.position, transform.rotation);
    }
}
