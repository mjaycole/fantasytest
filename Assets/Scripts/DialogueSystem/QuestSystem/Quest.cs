using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = ("New Quest"))]
public class Quest : ScriptableObject
{
    [SerializeField] string questName;
    [TextArea(20, 20)]
    [SerializeField] List<string> objectives = new List<string>();

    #region Getters
    public string GetQuestName()
    {
        return questName;
    }
    public List<string> GetObjectives()
    {
        return objectives;
    }

    public bool HasObjective(string objective)
    {
        return objectives.Contains(objective);
    }

    #endregion
}
