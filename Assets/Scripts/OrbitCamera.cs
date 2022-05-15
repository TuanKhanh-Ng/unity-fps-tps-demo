using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotationSpeed = 10.0f;

    private float rotX;
    private float rotY;
    private Vector3 initialOffset;
    void Start()
    {
        rotX = 0;
        rotY = 0;
        initialOffset = target.position - transform.position;
    }

    void LateUpdate()
    {
        rotX += Input.GetAxis("Mouse Y") * rotationSpeed;
        rotY += Input.GetAxis("Mouse X") * rotationSpeed;

        Quaternion rotation = Quaternion.Euler(rotX, rotY, 0);
        transform.position = target.position - (rotation * initialOffset);
        transform.LookAt(target.transform.position);
    }
}
