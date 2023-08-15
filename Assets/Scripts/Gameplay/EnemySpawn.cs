using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemy_prefab;
    [SerializeField] private GameObject spawnVFX;
    private Transform enemy_holder;
    private List<Transform> spawnPoints;

    void Awake()
    {
        spawnPoints = new List<Transform>();
        enemy_holder = GameObject.Find("Enemies").transform;
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }
    }

    public void Spawn()
    {
        StartCoroutine(spawnCoRoutine());
    }

    IEnumerator spawnCoRoutine()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject.Instantiate(spawnVFX, spawnPoint.position, spawnPoint.rotation, transform);
        }
        yield return new WaitForSeconds(1.5f);
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject.Instantiate(enemy_prefab, spawnPoint.position, spawnPoint.rotation, enemy_holder);
        }
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
