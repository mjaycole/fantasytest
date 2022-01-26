using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform fpsCam = null;
    [SerializeField] Transform itemCam = null;
    [SerializeField] Transform weaponParent = null;

    [Header("Look Variables")]
    [SerializeField] float sensitivity;
    float xRotation;
    float mouseY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        fpsCam.localRotation = Quaternion.Euler(xRotation, 0, 0);
        itemCam.localRotation = Quaternion.Euler(xRotation, 0, 0);
        weaponParent.localRotation = fpsCam.localRotation;
        transform.Rotate(Vector3.up * mouseX);
    }

    public float GetLookRotation()
    {
        return mouseY;
    }

    public void SetLookRotation(float rotation)
    {
        mouseY = rotation;
    }
}
