using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerConversant : MonoBehaviour
{
    Dialogue currentDialogue;
    DialogueNode currentNode = null;
    AIConversant currentConversant = null;
    bool isChoosing = false;

    public event Action onConversationUpdated;
    public event Action onConversationStarted;
    public event Action onConversationEnded;


    public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
    {
        currentConversant = newConversant;
        currentDialogue = newDialogue;
        currentNode = currentDialogue.GetRootNode();

        onConversationUpdated();
        onConversationStarted();

        SetConversationMode();
        TriggerEnterAction();        
    }

    public void Quit()
    {
        currentDialogue = null;
        currentNode = null;
        isChoosing = false;

        onConversationUpdated();
        onConversationEnded();

        SetPlayMode();
        TriggerExitAction();
        currentConversant = null;
    }

    public void SelectChoice(DialogueNode choice)
    {
        currentNode = choice;
        isChoosing = false;

        TriggerEnterAction();
        Next();
    }

    public void SelectConversation(Dialogue conversation)
    {
        currentDialogue = conversation;
        currentNode = conversation.GetRootNode();
        isChoosing = false;

        TriggerEnterAction();
        Next();
    }
    public void Next()
    {
        int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();

        if (numPlayerResponses > 0)
        {
            isChoosing = true;

            TriggerExitAction();

            onConversationUpdated();
            return;
        }
        else if (currentDialogue.GetAIChildren(currentNode).Count() > 0)
        {
            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());

            TriggerExitAction();

            currentNode = children[randomIndex];

            TriggerEnterAction();

            onConversationUpdated();
        }
        else
        {
            Quit();
        }
    }

    private void SetConversationMode()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    private void SetPlayMode()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void TriggerEnterAction()
    {
        if (currentNode != null)
        {
            TriggerAction(currentNode.GetOnEnterAction());
        }
    }

    private void TriggerExitAction()
    {
        if (currentNode != null)
        {
            TriggerAction(currentNode.GetOnExitAction());
        }
    }

    private void TriggerAction(string actionToTrigger)
    {
        if (actionToTrigger == "") { return; }

        foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
        {
            trigger.Trigger(actionToTrigger);
        }
    }

    #region Getters
    public bool IsActive()
    {
        return currentDialogue != null;
    }

    public bool IsChoosing()
    {
        return isChoosing;
    }

    public string GetText()
    {
        if (currentNode == null)
        {
            return "";
        }
        else
        {
            return currentNode.GetText();
        }
    }

    public bool HasNext()
    {
        return currentDialogue.GetAllChildren(currentNode).Count() > 0;
    }

    public bool HasPlayerOptions()
    {
        return currentConversant.GetDialogueSequences().Count() > 0;
    }

    public string GetCurrentConversantName()
    {
        return currentConversant.GetNPCName();
    }

    public IEnumerable<DialogueNode> GetChoices()
    {
        return currentDialogue.GetPlayerChildren(currentNode);
    }
    #endregion

}
