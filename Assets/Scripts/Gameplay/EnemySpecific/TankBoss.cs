using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CherryTeaGames.Core.Variables;
using CherryTeaGames.Core.Events;

/*

Design:
    - Phase 1 
        - Boss will just wander a bit, not noticing the player,
        - After the bosses shield is broken
            - it will go to a corner, rev up, and do a dash attack across the field.
        - After the dash attack it will stall out and allow the player to hit it.
        - At 50 % HP, phase transition to phase 2.
    - Phase transition
        - boss gets in middle, starts doing doughnuts, causes area wide knockback unless knocking back into wall.
    - Phase 2
        - Boss will not wander, just do the corner attack
        - 



**/

public class TankBoss : MonoBehaviour
{
    [Header("Stats")]
    public float maxShield;
    public float maxHP;
    public float currentShield;
    public float currentHP;

    [Header("VariableSystem")]
    public FloatVariable HPVar;
    public FloatVariable ShieldVar;

    [Header("Events")]
    public GameEvent levelWinEvent;

    [Header("DependentPrefabs")]
    public GameObject rectAOE;

    public bool isVulnerable;

    [SerializeField]
    private GameObject groundexplosionvfx;
    [SerializeField]
    private GameObject deathFXPrefab;
    private IndicatorSystem indicatorSystem;

    void Start()
    {
        indicatorSystem = GameObject.Find("GroundIndicatorCanvas").GetComponent<IndicatorSystem>();
        currentShield = maxShield;
        currentHP = maxHP;
        HPVar.Value = currentHP / maxHP;
        ShieldVar.Value = currentShield / maxShield;
    }

    public void DashAttackAOE()
    {
        // Spawn AOE indicator
        GameObject go = Instantiate(rectAOE, transform.position, transform.rotation);
        go.transform.localScale = new Vector3(10, 1, 42.5f);
        // indicatorSystem.SpawnRectangularAOEIndicator(transform.position, new Vector2(10, 42.5f), transform.rotation.eulerAngles.y, 2f);
    }

    public void OnTakeDamage(float dmgAmount)
    {
        if (isVulnerable) currentHP -= dmgAmount;
        else currentShield -= dmgAmount;
        if (currentShield <= 0) isVulnerable = true;
        HPVar.Value = currentHP / maxHP;
        ShieldVar.Value = currentShield / maxShield;
        if (currentHP <= 0) OnWin();
    }

    public void OnWin()
    {
        GameObject g = Instantiate(deathFXPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        levelWinEvent.Raise();
    }

    public void RestoreShield()
    {
        currentShield = maxShield;
        isVulnerable = false;
        ShieldVar.Value = currentShield / maxShield;
    }

    public void SpawnExplosion(float duration)
    {
        StartCoroutine(continuousSpawnExplosion(duration));
    }

    private IEnumerator continuousSpawnExplosion(float duration)
    {
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            yield return new WaitForSeconds(.1f);
            Instantiate(groundexplosionvfx, transform.position, Quaternion.identity);
        }
    }


}
