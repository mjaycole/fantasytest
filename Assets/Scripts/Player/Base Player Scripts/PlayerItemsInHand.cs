using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemsInHand : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform leftHandParent = null;
    [SerializeField] Transform rightHandParent = null;
    [SerializeField] PlayerConversant playerConversant = null;

    [Header("Inventory")]
    [SerializeField] HandheldItem leftHandItem = null;
    [SerializeField] HandheldItem rightHandItem = null;
    bool canUseItem = true;
    float rightCooldownTimer;
    float leftCooldownTimer;

    public HandeheldItemScriptableObject testSword;
    public HandeheldItemScriptableObject testShield;

    [Header("Keybinds")]
    public KeyCode equipCode;
    public KeyCode primaryAttack;
    public KeyCode secondaryAttack;

    private void Start()
    {
        playerConversant.onConversationStarted += SetCanUseItem;
        playerConversant.onConversationEnded += SetCanUseItem;
    }

    private void Update()
    {
        #region Cooldown Timers
        rightCooldownTimer -= Time.deltaTime;
        leftCooldownTimer -= Time.deltaTime;
        #endregion

        #region Testing
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestSetItem();
        }
        #endregion

        #region Use Items
        //Right hand items for swords, bows, two-handed weapons, staffs
        if (Input.GetKeyDown(primaryAttack) && canUseItem && rightHandItem != null)
        {
            if (CheckRightCoolDownTimer())
            {
                UseRightItem();

                if (leftHandItem != null)
                {
                    ReleaseLeftItem();
                    StartCoroutine(WaitBeforeUsingItemAgain(rightHandItem.cooldownTime));
                }
            }
        }

        //Left hand items are shields only
        if (Input.GetKey(secondaryAttack) && canUseItem && leftHandItem != null)
        {
            if (CheckLeftCoolDownTimer())
            {
                UseLeftItem();
            }
        }
        if (Input.GetKeyUp(secondaryAttack) && canUseItem && leftHandItem != null)
        {
            ReleaseLeftItem();
        }
        #endregion

        #region Equip Items
        if (Input.GetKeyDown(equipCode) && canUseItem)
        {
            SetItemsActive();
        }
        #endregion
    }

    #region Private Functions

    private void UseRightItem()
    {
        rightHandItem.UseItem();
        rightCooldownTimer = rightHandItem.cooldownTime;
    }

    private void UseLeftItem()
    {
        leftHandItem.UseItem();
    }

    private void ReleaseLeftItem()
    {
        leftHandItem.ReleaseItemUse();
    }

    private bool CheckRightCoolDownTimer()
    {
        if (rightCooldownTimer <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckLeftCoolDownTimer()
    {
        if (leftCooldownTimer <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetItemsActive()
    {
        if (rightHandItem == null && leftHandItem == null) { return; }

        if (rightHandItem != null)
        {
            if (rightHandItem.gameObject.activeInHierarchy)
            {
                StartCoroutine(PutAwayItems());
                return;
            }
            else
            {
                rightHandItem.gameObject.SetActive(true);
            }
        }
        if (leftHandItem != null)
        {
            if (leftHandItem.gameObject.activeInHierarchy)
            {
                StartCoroutine(PutAwayItems());
                return;
            }
            else
            {
                leftHandItem.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator PutAwayItems()
    {
        canUseItem = false;
        if (rightHandItem != null)
            rightHandItem.PutAwayItem();
        if (leftHandItem != null)
            leftHandItem.PutAwayItem();

        yield return new WaitForSeconds(1f);

        if (rightHandItem != null)
            rightHandItem.gameObject.SetActive(!rightHandItem.gameObject.activeInHierarchy);
        if (leftHandItem != null)
            leftHandItem.gameObject.SetActive(!leftHandItem.gameObject.activeInHierarchy);

        canUseItem = true;
    }

    IEnumerator WaitBeforeUsingItemAgain(float amount)
    {
        canUseItem = false;
        yield return new WaitForSeconds(amount);
        canUseItem = true;
    }
    #endregion

    #region Public Functions
    public void TestSetItem()
    {
        SetRightHandItem(testSword);
        SetLeftHandItem(testShield);
    }

    public void SetRightHandItem(HandeheldItemScriptableObject newItem)
    {
        //Don't set shields to right hand
        if (newItem.prefab.GetComponent<HandheldItem>().itemType == HandheldItem.ItemTypes.Shield) { return; }

        if (rightHandItem != null)
        {
            if (rightHandItem.itemName == newItem.name)
            {
                return;
            }
            else
            {
                Destroy(rightHandParent.transform.GetChild(0).gameObject);
            }
        }


        //Instantiate and set object to hand
        GameObject newObject = Instantiate(newItem.prefab, rightHandParent.position, rightHandParent.rotation, rightHandParent);
        rightHandItem = newObject.GetComponent<HandheldItem>();
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void SetLeftHandItem(HandeheldItemScriptableObject newItem)
    {
        //Only set the left hand to shields
        if (newItem.prefab.GetComponent<HandheldItem>().itemType == HandheldItem.ItemTypes.Melee || newItem.prefab.GetComponent<HandheldItem>().itemType == HandheldItem.ItemTypes.Bow || newItem.prefab.GetComponent<HandheldItem>().itemType == HandheldItem.ItemTypes.Staff) { return; }

        if (leftHandItem != null)
        {
            if (leftHandItem.itemName == newItem.name)
            {
                return;
            }
            else
            {
                Destroy(leftHandItem.transform.GetChild(0).gameObject);
            }
        }


        //Instantiate and set object to hand
        GameObject newObject = Instantiate(newItem.prefab, leftHandParent.position, leftHandParent.rotation, leftHandParent);
        leftHandItem = newObject.GetComponent<HandheldItem>();
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localRotation = Quaternion.Euler(Vector3.zero);        
    }
    #endregion

    #region Setters
    public void SetCanUseItem()
    {
        canUseItem = !canUseItem;
    }
    #endregion
}
