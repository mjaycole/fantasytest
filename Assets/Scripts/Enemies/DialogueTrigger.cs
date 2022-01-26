using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] string triggeringAction;
    [SerializeField] UnityEvent onTrigger;

    public void Trigger(string actionToTrigger)
    {
        if (actionToTrigger == triggeringAction)
        {
            onTrigger.Invoke();
        }
    }
}
