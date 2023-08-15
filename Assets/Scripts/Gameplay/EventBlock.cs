using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CherryTeaGames.Core.Events;

public class EventBlock : MonoBehaviour
{
    public UnityEvent _event;
    public GameEvent _CTG_Event;
    public bool single_use = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Enable UI for event init
            _event?.Invoke();
            _CTG_Event?.Raise();
            if (single_use)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Disable UI for event init
        }
    }
}
