using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CherryTeaGames.Core.Events;

public class ShipActivation : MonoBehaviour
{
    bool canActivate;
    [SerializeField]
    GameEvent shipActivationEvent;
    [SerializeField]
    AudioSource activationsound;

    public void SetActivation() { canActivate = true; }
    public void Activate()
    {
        if (!canActivate) return;
        shipActivationEvent.Raise();
        activationsound.Play();
        Destroy(gameObject, 2f);
        canActivate = false;
    }
}
