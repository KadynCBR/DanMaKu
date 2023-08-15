using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CherryTeaGames.Core.Events
{
    public class EventRaiser : MonoBehaviour
    {
        public GameEvent eventToRaise;

        void Start()
        {
            eventToRaise.Raise();
        }
    }
}
