using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    CharacterController controller = null;
    [SerializeField] Transform itemParent = null;

    [Header("Physics")]
    [SerializeField] float gravity;
    [SerializeField] Transform groundCheck = null;
    [SerializeField] LayerMask groundLayer;
    Vector3 velocity;
    bool isGrounded;

    [Header("Movement Variables")]
    bool canMove = true;
    [SerializeField] float baseMoveSpeed;
    [SerializeField] float jumpHeight;
    float currentMoveSpeed;
    Vector3 moveDirection;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        MovementInputs();
        Gravity();
        Jump();
        MovePlayer();
    }

    private void MovementInputs()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDirection = transform.right * x + transform.forward * z;
    }

    private void Gravity()
    {
        if (!canMove) { return; }

        isGrounded = Physics.CheckSphere(groundCheck.position, .15f, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        Physics.SyncTransforms();

        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (!canMove) { return; }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    private void MovePlayer()
    {
        if (!canMove) { return; }

        Physics.SyncTransforms();

        controller.Move(moveDirection * currentMoveSpeed * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            foreach (Transform t in itemParent)
            {
                if (t.childCount > 0)
                {
                    t.GetChild(0).GetComponent<Animator>().SetFloat("Walk", 1f);
                }
            }
        }
        else
        {
            foreach (Transform t in itemParent)
            {
                if (t.childCount > 0)
                {
                    t.GetChild(0).GetComponent<Animator>().SetFloat("Walk", 0f);
                }
            }
        }
    }
}
