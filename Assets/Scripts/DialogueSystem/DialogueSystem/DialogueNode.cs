using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class DialogueNode : ScriptableObject
{
    [SerializeField] bool isPlayerSpeaking = false;
    [SerializeField] string text;
    [SerializeField] List<string> children = new List<string>();
    [SerializeField] Rect nodeRect = new Rect(0, 0, 200, 100);
    [SerializeField] string onEnterAction;
    [SerializeField] string onExitAction;

    #region Getters
    public Rect GetRect()
    {
        return nodeRect;
    }

    public string GetText()
    {
        return text;
    }

    public List<string> GetChildren()
    {
        return children;
    }

    public bool IsPlayerSpeaking()
    {
        return isPlayerSpeaking;
    }

    public string GetOnEnterAction()
    {
        return onEnterAction;
    }

    public string GetOnExitAction()
    {
        return onExitAction;
    }
    #endregion

#if UNITY_EDITOR

    public void SetSpeaker(bool newIsPlayerSpeaking)
    {
        Undo.RecordObject(this, "Change Dialogue Speaker");

        isPlayerSpeaking = newIsPlayerSpeaking;

        EditorUtility.SetDirty(this);
    }
    public void SetPosition(Vector2 newRect)
    {
        Undo.RecordObject(this, "Move Dialogue Node");

        nodeRect.position = newRect;

        EditorUtility.SetDirty(this);
    }

    public void SetText(string newText)
    {
        if (newText != text)
        {
            Undo.RecordObject(this, "Update Dialogue Text");

            text = newText;

            EditorUtility.SetDirty(this);
        }
    }

    public void AddChild(string childID)
    {
        Undo.RecordObject(this, "Add Dialogue Link");

        children.Add(childID);

        EditorUtility.SetDirty(this);
    }

    public void RemoveChild(string childID)
    {
        Undo.RecordObject(this, "Unlinked Dialogue Child");

        children.Remove(childID);

        EditorUtility.SetDirty(this);
    }

#endif
}
