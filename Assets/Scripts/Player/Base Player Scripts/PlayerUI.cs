using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] PlayerMovement moveScript = null;
    [SerializeField] PlayerLook lookScript = null;
    [SerializeField] PlayerItemsInHand itemScript = null;
    [SerializeField] PlayerInventory inventoryScript = null;

    [Header("UI Components")]
    [SerializeField] GameObject journalPanel = null;

    [Header("Journal Panel")]
    [SerializeField] GameObject[] allJournalPages = null;
    [SerializeField] Button weaponButton = null;
    [SerializeField] Button questsButton = null;
    [SerializeField] GameObject weaponsPanel = null;
    [SerializeField] GameObject questsPanel = null;

    [Header("Keybinds")]
    public KeyCode openJournal;

    private void Start()
    {
        weaponButton.onClick.AddListener(OpenWeaponsPanel);
        questsButton.onClick.AddListener(OpenQuestPanel);
    }

    void Update()
    {
        ToggleJournal(false);
    }

    private void DisableScripts()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        inventoryScript.SetInventory();
    }

    private void EnableScripts()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        inventoryScript.ClearInventory();
    }

    public void ToggleJournal(bool external = false)
    {
        if (Input.GetKeyDown(openJournal) || external)
        {
            journalPanel.SetActive(!journalPanel.activeInHierarchy);
            
            if (journalPanel.activeInHierarchy)
            {
                DisableScripts();
                weaponButton.onClick.Invoke();
                weaponButton.Select();
            }
            else
            {
                EnableScripts();
            }
        }
    }

    #region Public Functions
    #region Journal Public Functions
    public void OpenNewJournalPage(GameObject newPanel)
    {
        foreach (GameObject o in allJournalPages)
        {
            o.SetActive(false);
        }

        newPanel.SetActive(true);
    }

    public void OpenWeaponsPanel()
    {
        OpenNewJournalPage(weaponsPanel);
    }

    public void OpenQuestPanel()
    {
        OpenNewJournalPage(questsPanel);
    }
    #endregion
    #endregion
}
