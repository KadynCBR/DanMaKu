using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CherryTeaGames.Core.Events;

public class Damageable : MonoBehaviour
{
    public GameEvent OnPlayerHit;
    private List<Material> mats;

    [Header("Visual")]
    [GradientUsageAttribute(true)]
    public Gradient onHitBodyGradient;
    public float flashOnHitDuration = 0.5f;

    public bool isFriendly = false;

    void Start()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        Debug.Log($"{rends.Length} rend lengths in {gameObject.name}");
        mats = new List<Material>();
        foreach (Renderer rend in rends)
        {
            mats.AddRange(rend.materials);
        }
        Debug.Log($"{mats.Count} mats lengths in {gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            BulletBase bb = other.GetComponent<BulletBase>();
            if (bb.isFriendly == isFriendly) { return; }
            if (!bb.piercing)
            {
                bb.DisableBullet();
            }
            TakeDamage(bb.damage);
        }
    }

    public void TakeDamage(float dmgAmount)
    {
        Debug.Log("Taking damage!");
        StartCoroutine(DamageBlink(1));
        if (OnPlayerHit != null) OnPlayerHit.Raise();
        BroadcastMessage("OnTakeDamage", dmgAmount, SendMessageOptions.DontRequireReceiver);
    }

    IEnumerator DamageBlink(int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            float blink_start = Time.time;
            while (Time.time - blink_start < flashOnHitDuration)
            {
                foreach (Material mat in mats)
                {
                    Color currentColor = onHitBodyGradient.Evaluate((Time.time - blink_start) / flashOnHitDuration);
                    mat.SetColor("_EmissionColor", currentColor);
                }
                yield return null;
            }
        }
        foreach (Material mat in mats)
            mat.SetColor("_EmissionColor", Color.black);
    }
}
