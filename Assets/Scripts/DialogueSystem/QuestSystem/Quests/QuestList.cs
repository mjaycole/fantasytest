using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestList : MonoBehaviour
{
    [SerializeField] List<QuestStatus> statuses = new List<QuestStatus>();

    public event Action<Quest> onQuestListUpdated;

    #region Getters
    public List<QuestStatus> GetStatuses()
    {
        return statuses;
    }

    public bool HasQuest(Quest quest)
    {
        return GetQuestStatus(quest) != null;
    }

    public bool HasObjective(string objective)
    {
        foreach (QuestStatus status in statuses)
        {
            if (status.GetQuest().GetObjectives().Contains(objective))
            {
                return true;
            }
        }
        
        return false;
        
    }
    #endregion


    #region Setters
    public void AddQuest(Quest quest)
    {
        if (HasQuest(quest)) { return; }

        QuestStatus newStatus = new QuestStatus(quest);
        statuses.Add(newStatus);

        if (onQuestListUpdated != null)
        {
            onQuestListUpdated(quest);
        }
    }

    public void CompleteObjective(Quest quest, string objective)
    {
        if (!HasObjective(objective)) { return; }

        QuestStatus status = GetQuestStatus(quest);
        status.CompleteObjective(objective);
    }

    private QuestStatus GetQuestStatus(Quest quest)
    {
        foreach (QuestStatus status in statuses)
        {
            if (quest == status.GetQuest())
            {
                return status;
            }
        }

        return null;
    }


    public void SaveQuests()
    {
        //SaveSystem.SaveQuests();
    }
    #endregion
}
