using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject mainCam = null;
    [SerializeField] TMP_Text interactText = null;
    [SerializeField] PlayerInventory inventory = null;
    [SerializeField] PlayerItemsInHand itemsInHandScript = null;
    [SerializeField] PlayerConversant playerConversant = null;
    Interactable currentInteractable = null;

    [Header("Variables")]
    [SerializeField] float interactDistance;
    [SerializeField] LayerMask interactableObjects;

    [Header("Keybinds")]
    public KeyCode interact;

    private void Update()
    {
        DetectInteractable();
    }

    private void DetectInteractable()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, interactDistance, interactableObjects))
        {
            if (hit.transform.GetComponent<Interactable>() != null)
            {
                interactText.gameObject.SetActive(true);

                currentInteractable = hit.transform.GetComponent<Interactable>();

                HandleInteracting();
            }
            else
            {
                currentInteractable = null;
                interactText.text = "";
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);
            currentInteractable = null;
            interactText.text = "";
        }
    }

    private void HandleInteracting()
    {
        switch(currentInteractable.interactType)
        {
            case Interactable.InteractableType.Pickup:
                interactText.text = "Pick up " + currentInteractable.transform.GetComponent<HandheldItem>().itemName;

                if (DetectInteractKey())
                {
                    OnPickupInteracted(currentInteractable);
                }

                break;

            case Interactable.InteractableType.Conversing:
                if (currentInteractable.transform.GetComponent<AIConversant>().GetOpeningDialogue() == null) 
                { 
                    return; 
                }
                else
                {
                    if (currentInteractable.transform.GetComponent<EnemyAI>() != null)
                    {
                        if (currentInteractable.transform.GetComponent<EnemyAI>().GetIsAggro())
                        {
                            return;
                        }
                    }

                    interactText.text = "Speak to " + currentInteractable.transform.GetComponent<AIConversant>().GetNPCName();

                    if (DetectInteractKey())
                    {
                        OnConversationInteracted(currentInteractable);
                    }
                }
                break;
        }
    }

    private bool DetectInteractKey()
    {
        return Input.GetKeyDown(interact);
    }

    private void OnPickupInteracted(Interactable item)
    {
        inventory.AddInventory(item.transform.GetComponent<HandheldItem>().GetItem());
        Destroy(item.transform.gameObject);
        interactText.gameObject.SetActive(false);        
    }

    private void OnConversationInteracted(Interactable npc)
    {
        Dialogue newDialogue = npc.GetComponent<AIConversant>().GetOpeningDialogue();
        playerConversant.StartDialogue(npc.GetComponent<AIConversant>(), newDialogue);
    }
}
