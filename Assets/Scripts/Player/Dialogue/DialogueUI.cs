using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class DialogueUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerConversant playerConversant;

    [Header("Dialogue Elements")]
    [SerializeField] TMP_Text speakerName = null;
    [SerializeField] TMP_Text npcDialogueField = null;
    [SerializeField] Button nextButton = null;
    [SerializeField] Button quiteButton = null;
    [SerializeField] Transform choiceParent = null;
    [SerializeField] Transform npcDialogueParent = null;
    [SerializeField] GameObject playerChoicePrefab = null;

    private void Start()
    {
        playerConversant = GameObject.FindObjectOfType<PlayerConversant>().GetComponent<PlayerConversant>();


        nextButton.onClick.AddListener(() => playerConversant.Next());
        quiteButton.onClick.AddListener(() => playerConversant.Quit());
        playerConversant.onConversationUpdated += UpdateUI;

        UpdateUI();
    }

    private void Next()
    {
        playerConversant.Next();
    }

    private void UpdateUI()
    {
        gameObject.SetActive(playerConversant.IsActive());
        if (!playerConversant.IsActive())
        {
            return;
        }

        speakerName.text = playerConversant.GetCurrentConversantName();

        npcDialogueParent.gameObject.SetActive(!playerConversant.IsChoosing());
        choiceParent.gameObject.SetActive(playerConversant.IsChoosing());

        if (playerConversant.IsChoosing())
        {
            BuildChoiceList();
        }
        else
        {
            npcDialogueField.text = playerConversant.GetText();
            
            nextButton.gameObject.SetActive(playerConversant.HasNext());
        }
    }

    private void BuildChoiceList()
    {
        foreach (Transform existingOptions in choiceParent)
        {
            Destroy(existingOptions.gameObject);
        }

        foreach (DialogueNode choice in playerConversant.GetChoices())
        {
            GameObject newChoice = Instantiate(playerChoicePrefab, choiceParent);
            var textComp = newChoice.GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = choice.GetText();

            Button button = newChoice.GetComponentInChildren<Button>();
            button.onClick.AddListener(() =>
            {
                playerConversant.SelectChoice(choice);
            });
        }        
    }
}
