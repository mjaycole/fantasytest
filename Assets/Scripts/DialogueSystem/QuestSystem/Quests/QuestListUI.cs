using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] QuestItemUI questPrefab = null;
    [SerializeField] QuestItemUI completedQuestPrefab = null;
    [SerializeField] Transform questListParent = null;
    [SerializeField] Transform subQuestParent = null;
    [SerializeField] Transform completedQuestsParent = null;

    [SerializeField] Button questMenuButton = null;

    QuestList questList = null;

    private void OnEnable()
    {
        questList = GameObject.FindObjectOfType<QuestList>();
        questList.onQuestListUpdated += UpdateUI;

        questMenuButton.onClick.AddListener(OpenMainQuests);

        SetupQuests();

        LayoutRebuilder.ForceRebuildLayoutImmediate(questListParent.GetComponent<RectTransform>());
    }

    public void UpdateUI(Quest newQuest)
    {
        SetupQuests();

        LayoutRebuilder.ForceRebuildLayoutImmediate(questListParent.GetComponent<RectTransform>());
    }

    private void OpenMainQuests()
    {
        questListParent.gameObject.SetActive(true);
        subQuestParent.gameObject.SetActive(false);

        LayoutRebuilder.ForceRebuildLayoutImmediate(questListParent.GetComponent<RectTransform>());
    }

    private void SetupQuests()
    {
        foreach (Transform t in questListParent)
        {
            Destroy(t.gameObject);
        }

        foreach (Transform t in subQuestParent)
        {
            Destroy(t.gameObject);
        }

        OpenMainQuests();

        //CreateCompletedQuestUI();
        CreateIncompleteQuestUI();
    }

    private void CreateCompletedQuestUI()
    {
        if (questList.GetStatuses().Count() == 0) { return; }

        foreach (QuestStatus status in questList.GetStatuses())
        {
            if (status.GetCompletedObjectes().Count() == status.GetAllObjectives().Count())
            {
                QuestItemUI newQuest = Instantiate(completedQuestPrefab, questListParent);
                newQuest.Setup(status);
                newQuest.GetComponent<Button>().onClick.AddListener(delegate { OpenQuestLine(status); });
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(questListParent.GetComponent<RectTransform>());
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(questListParent.GetComponent<RectTransform>());
    }

    private void CreateIncompleteQuestUI()
    {
        if (questList.GetStatuses().Count() == 0) { return; }

        foreach (QuestStatus status in questList.GetStatuses())
        {
            if (status.GetIncompleteObjectives().Count == 0)
            {
                //This should be covered in CreateCompletedQuestUI
            }
            else
            {
                QuestItemUI newQuest = Instantiate(questPrefab, questListParent);
                newQuest.Setup(status);
                newQuest.GetComponent<Button>().onClick.AddListener(delegate { OpenQuestLine(status); });
            }    

            LayoutRebuilder.ForceRebuildLayoutImmediate(questListParent.GetComponent<RectTransform>());
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(questListParent.GetComponent<RectTransform>());
    }


    #region Subquests
    private void OpenQuestLine(QuestStatus quest)
    {
        questListParent.gameObject.SetActive(false);
        subQuestParent.gameObject.SetActive(true);

        foreach (Transform t in subQuestParent)
        {
            Destroy(t.gameObject);
        }

        CreateSubQuestUI(quest);

        LayoutRebuilder.ForceRebuildLayoutImmediate(subQuestParent.GetComponent<RectTransform>());
    }

    private void CreateSubQuestUI(QuestStatus quest)
    {
        for (int i = quest.GetAllObjectives().ToArray().Count(); i > 0; i--)
        {
            QuestItemUI subQuestInstance = Instantiate(questPrefab, subQuestParent);
            subQuestInstance.SetupSubQuest(quest, i - 1);
            subQuestInstance.GetComponent<Button>().enabled = false;

            LayoutRebuilder.ForceRebuildLayoutImmediate(subQuestParent.GetComponent<RectTransform>());
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(subQuestParent.GetComponent<RectTransform>());
    }

    #endregion
}
