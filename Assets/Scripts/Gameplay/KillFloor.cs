using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Damageable d = null;
        other.gameObject.TryGetComponent<Damageable>(out d);
        if (d)
        {
            d.TakeDamage(999999999); // let damageable kill
        }
    }
}
