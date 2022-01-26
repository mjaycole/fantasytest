using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStatus
{
    public Quest quest;
    public List<string> incompleteObjectives = new List<string>();
    public List<string> completedObjectives = new List<string>();

    #region Getters
    public QuestStatus(Quest quest)
    {
        this.quest = quest;

        List<string> statuses = new List<string>();
        statuses = quest.GetObjectives();
        foreach (string objective in statuses)
        {
            incompleteObjectives.Add(objective);
        }
    }

    public Quest GetQuest()
    {
        return quest;
    }

    public List<string> GetAllObjectives()
    {
        return quest.GetObjectives();
    }

    public List<string> GetIncompleteObjectives()
    {
        return incompleteObjectives;
    }

    public List<string> GetCompletedObjectes()
    {
        return completedObjectives;
    }

    public bool IsObjectiveComplete(string objective)
    {
        return completedObjectives.Contains(objective);
    }
    #endregion


    public void CompleteObjective(string objective)
    {
        if (quest.HasObjective(objective))
        {
            completedObjectives.Add(objective);
            incompleteObjectives.Remove(objective);
        }
    }
}
