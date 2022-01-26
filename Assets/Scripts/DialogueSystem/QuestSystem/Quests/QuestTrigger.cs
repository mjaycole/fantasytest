using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTrigger;
    [SerializeField] string questObjective;

    public void Trigger()
    {
        onTrigger.Invoke();        
    }
}
