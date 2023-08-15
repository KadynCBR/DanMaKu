using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CherryTeaGames.Core.Events
{
    public abstract class ParameterGameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T> where E : ParameterGameEvent<T> where UER : UnityEvent<T>
    {
        [SerializeField]
        private E gameEvent;
        private E GameEvent { get { return gameEvent; } set { gameEvent = value; } }
        [SerializeField]
        private UER unityEventResponse;

        private void OnEnable()
        {
            if (gameEvent == null)
            {
                return;
            }
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (gameEvent == null)
            {
                return;
            }
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            if (unityEventResponse != null)
            {
                unityEventResponse.Invoke(item);
            }
        }
    }
}
