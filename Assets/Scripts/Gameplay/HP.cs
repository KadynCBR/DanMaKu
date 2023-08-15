using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CherryTeaGames.Core.Variables;

public class HP : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public FloatVariable HPVariable;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    public void OnTakeDamage(float amt)
    {
        currentHP -= amt;
        // if (HPVariable) HPVariable.SetValue(currentHP);
        if (currentHP <= 0) BroadcastMessage("OnDeath", null, SendMessageOptions.DontRequireReceiver);
    }

    void Update()
    {
        if (HPVariable) HPVariable.SetValue(currentHP);
    }
}
