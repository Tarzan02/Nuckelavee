using System.Collections;
using System.Collections.Generic;
using Unity.Profiling.Editor;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Example : MonoBehaviour
{   
    CharacterController controller;
    public Camera playerCamera;
    public float walkSpeed = 7f;
    public float sprintSpeed = 12f;
    public float jumpPwr = 7f;
    public float gravity = 9.81f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public bool canMove = true;
    public bool groundedPlayer = true;

    Vector3 playerVelocity = Vector3.zero;
    float rotationX = 0;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        #region Movement

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //sprint
        bool isSprinting = false;
        float curSpeedX = canMove ? (isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float playerVelocityY = playerVelocity.y;
        playerVelocity = (forward * curSpeedX) + (right * curSpeedY);

        //jump
        if(Input.GetButton("Jump") && canMove && controller.isGrounded)
            playerVelocity.y = jumpPwr;
        else
            playerVelocity.y = playerVelocityY;

        if(!controller.isGrounded)
            playerVelocity.y = gravity * Time.deltaTime;
        //rotation
        controller.Move(playerVelocity * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion
    }
}