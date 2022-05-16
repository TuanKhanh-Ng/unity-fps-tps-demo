using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform target;

    // Variables for movements on the ground
    public float rotationSpeed = 15.0f;
    public float groundMoveSpeed = 5.0f;

    // Variables for vertical movements
    private float vertMoveSpeed;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFallSpeed = -1.5f;

    private ControllerColliderHit _contact;

    private Vector3 movement;
    private CharacterController _charController;

    void Start()
    {
        vertMoveSpeed = minFallSpeed;
        _charController = GetComponent<CharacterController>();
    }
    void Update()
    {
        movement = Vector3.zero;

        // Movements on the grounds
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        if (horInput != 0 || vertInput != 0)
        {
            movement.x = horInput * groundMoveSpeed;
            movement.z = vertInput * groundMoveSpeed;

            Quaternion initialRotation = target.rotation;

            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);

            target.rotation = initialRotation;

            // Rotate the character to look at the direction of the camera
            if (movement.magnitude > 0)
            {
                Vector3 forward = new Vector3(target.forward.x, 0, target.forward.z);
                Quaternion rotation = Quaternion.LookRotation(forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
        }

        // Vertical movements

        bool hitGround = false;

        RaycastHit hitInfo;
        if (vertMoveSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hitInfo))
        {
            hitGround = hitInfo.distance <= (_charController.height + _charController.radius) / 1.9f;
        }

        if (hitGround)
        {
            bool jumpInput = Input.GetButtonDown("Jump");
            if (jumpInput)
            {
                vertMoveSpeed = jumpSpeed;
            }
            else
            {
                vertMoveSpeed = minFallSpeed;
            }
        }
        else
        {
            vertMoveSpeed += gravity * Time.deltaTime;
            if (vertMoveSpeed < terminalVelocity)
            {
                vertMoveSpeed = terminalVelocity;
            }

            if (_charController.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * groundMoveSpeed;
                }
                else
                {
                    movement += _contact.normal * groundMoveSpeed;
                }
            }
        }
        movement.y = vertMoveSpeed;

        movement *= Time.deltaTime;
        _charController.Move(movement);

    }

    void OnControllerColliderHit(ControllerColliderHit contact)
    {
        _contact = contact;
    }
}
