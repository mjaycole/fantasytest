using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType {
        Pickup,
        Conversing
    }

    public InteractableType interactType;
}
