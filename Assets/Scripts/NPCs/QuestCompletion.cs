using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCompletion : MonoBehaviour
{
    [SerializeField] Quest quest;
    [SerializeField] string objective;


    public void CompleteObjective()
    {
        QuestList questList = GameObject.FindObjectOfType<QuestList>();
        questList.CompleteObjective(quest, objective);
    }
}
