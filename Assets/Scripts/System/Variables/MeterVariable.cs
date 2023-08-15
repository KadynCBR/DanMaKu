using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CherryTeaGames.Core.Variables
{
    public class MeterVariable : ScriptableObject
    {
        [Multiline]
        public string DeveloperDescription = "";
        public float Value;
        public float MaxValue;
        public float MinValue;

        [System.NonSerialized]
        public UnityEvent<float> MeterChangeEvent;
        [System.NonSerialized]
        public UnityEvent MeterExaustedEvent;
        [System.NonSerialized]
        public UnityEvent MeterFullEvent;

        private void OnEnable()
        {
            if (MeterChangeEvent == null) MeterChangeEvent = new UnityEvent<float>();
            if (MeterExaustedEvent == null) MeterExaustedEvent = new UnityEvent();
            if (MeterFullEvent == null) MeterFullEvent = new UnityEvent();
        }

        public void AdjustMeter(float amount)
        {
            Value += amount;
            Debug.Log($"Meter [{DeveloperDescription}]: Adjusted by {amount}");
            MeterChangeEvent.Invoke(Value);
            if (Value <= MinValue) MeterExaustedEvent.Invoke();
            if (Value >= MaxValue)
            {
                Value = MaxValue;
                MeterFullEvent.Invoke();
            }
        }
    }
}
