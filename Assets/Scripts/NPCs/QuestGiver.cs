using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] Quest quest;

    public void GiveQuest()
    {
        QuestList questList = GameObject.FindObjectOfType<QuestList>();
        questList.AddQuest(quest);
    }
}
