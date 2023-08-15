using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTypeH : BulletBase
{
    public Transform homingTarget;
    public float speed;
    private float _speed;

    public override void OnEnable()
    {
        base.OnEnable();
        _speed = speed;
    }

    public override void Movement()
    {
        if (_lifetime > lifetime - lifetime * .25f)
        {
            transform.position += transform.forward * _speed * 1.5f * Time.deltaTime;
        }
        else
        {
            // Because this only searches once, this can get kinda funky going 
            // where you're not expecting.
            // Eventually should evaluate how this would perform on a constant looking basis.
            // Optionally implement return-to-sender and have all bullets "signed" with a transform
            // from the shooter and carry that down the chain to this bullet.
            // would kinda break the current vibe tho so we'll see.
            if (!homingTarget)
                homingTarget = FindClosestEnemy();
            Vector3 direction = homingTarget.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            // Look smoothly
            // float rotAngle = Mathf.Lerp(5, 1, Time.deltaTime);
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotAngle);
            transform.rotation = toRotation;
            transform.position += transform.forward * _speed * Time.deltaTime;
            _speed += 100 * Time.deltaTime;
        }
    }

    private Transform FindClosestEnemy()
    {
        // this might be problematic with a lot of enemies, maybe limit it by range? 
        // though not too many enemies on screen planned.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest_enemy_pos = null;
        if (enemies.Length > 0)
        {
            GameObject closest_enemy = enemies[0];
            float distance = Vector3.Distance(transform.position, closest_enemy.transform.position);
            foreach (GameObject enemy in enemies)
            {
                float chkdistance = Vector3.Distance(transform.position, enemy.transform.position);
                if (chkdistance < distance)
                {
                    distance = chkdistance;
                    closest_enemy = enemy;
                }
            }
            closest_enemy_pos = closest_enemy.transform;
        }
        return closest_enemy_pos;
    }

}
