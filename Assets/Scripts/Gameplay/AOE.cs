using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// meant to be instantiated?

public enum AOEType
{
    RECTANGLE,
    CIRCLE,
    CONE,
    //etc
}

public class AOE : MonoBehaviour
{
    public AOEType aoeType;
    public float damage;
    public float detonationDelay;
    public GameObject vfxsfxObject;

    private IndicatorSystem indicatorSystem;

    void Start()
    {
        indicatorSystem = GameObject.FindGameObjectWithTag("IndicatorSystem").GetComponent<IndicatorSystem>();
        Invoke("Detonate", detonationDelay);
        CreateIndicator();
    }

    protected virtual void CreateIndicator()
    {
        switch (aoeType)
        {
            case AOEType.RECTANGLE:
                indicatorSystem.SpawnRectangularAOEIndicator(transform.position, new Vector2(transform.localScale.x, transform.localScale.z), transform.rotation.eulerAngles.y, detonationDelay);
                break;
            case AOEType.CIRCLE:
                indicatorSystem.SpawnCircleAOEIndicator(transform.position, new Vector2(transform.localScale.x, transform.localScale.z), detonationDelay);
                break;
        }
    }

    void Detonate()
    {

        if (vfxsfxObject != null)
            Instantiate(vfxsfxObject, transform.position, Quaternion.identity);
        ApplyDamageAndEffect();
        Destroy(gameObject);
    }

    protected virtual void ApplyDamageAndEffect()
    {
        Collider[] hitColliders = new Collider[0];
        switch (aoeType)
        {
            case AOEType.RECTANGLE:
                hitColliders = Physics.OverlapBox(
                    transform.localPosition + transform.forward * transform.localScale.z / 2,
                    transform.localScale / 2,
                    transform.localRotation);
                break;
            case AOEType.CIRCLE:
                hitColliders = Physics.OverlapSphere(transform.position, transform.localScale.x);
                break;
        }
        Debug.Log($"AOE found {hitColliders.Length} colliders in AOE.");
        if (hitColliders.Length > 0)
        {
            foreach (Collider hit in hitColliders)
            {
                if (hit.tag == "Player")
                {
                    hit.GetComponent<Damageable>().TakeDamage(damage);
                }
            }
        }
    }


    void OnDrawGizmos()
    {
        switch (aoeType)
        {
            case AOEType.RECTANGLE:
                Gizmos.matrix = Matrix4x4.TRS(transform.localPosition + transform.forward * transform.localScale.z / 2, transform.localRotation, transform.localScale);
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
                break;
            case AOEType.CIRCLE:
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireSphere(Vector3.zero, .38f);
                break;
        }
    }
}
