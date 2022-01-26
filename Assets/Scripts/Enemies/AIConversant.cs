using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversant : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] string npcName;

    [Header("Dialogue Trees")]
    [SerializeField] Dialogue beginningDialogue = null;
    public List<Dialogue> dialogueSequences = new List<Dialogue>();

    #region Getters
    public string GetNPCName()
    {
        return npcName;
    }

    public Dialogue GetOpeningDialogue()
    {
        return beginningDialogue;
    }

    public IEnumerable<Dialogue> GetDialogueSequences()
    {
        return dialogueSequences;
    }
    #endregion

    #region Setters
    public void UnlockDialogueSequence(Dialogue newDialogue)
    {
        dialogueSequences.Add(newDialogue);
    }
    #endregion
}
