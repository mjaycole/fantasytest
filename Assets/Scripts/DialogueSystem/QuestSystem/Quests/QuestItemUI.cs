using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] TMP_Text questName = null;
    [SerializeField] TMP_Text questDescription = null;

    QuestStatus status;

    public void Setup(QuestStatus status)
    {
        this.status = status;
        questName.text = status.GetQuest().GetQuestName();
        //questDescription.text = status.GetQuest().GetObjectives()[0];
        List<string> incompletedObjectives = new List<string>();
        incompletedObjectives = status.GetIncompleteObjectives();
        incompletedObjectives.Reverse();
        questDescription.text = incompletedObjectives[0];
    }

    public void SetupSubQuest(QuestStatus status, int index)
    {
        this.status = status;
        questName.text = status.GetQuest().GetQuestName();
        questDescription.text = status.GetQuest().GetObjectives()[index];
    }

    public QuestStatus GetQuestStatus()
    {
        return status;
    }
}
