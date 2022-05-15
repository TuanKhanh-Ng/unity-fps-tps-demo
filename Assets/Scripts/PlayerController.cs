using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotationSpeed = 15.0f;
    public float groundMoveSpeed = 5.0f;

    private Vector3 movement;
    private CharacterController _charController;

    void Start()
    {
        _charController = GetComponent<CharacterController>();
    }
    void Update()
    {
        movement = Vector3.zero;

        // Horizontal movement
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
        movement *= Time.deltaTime;
        _charController.Move(movement);

    }
}
