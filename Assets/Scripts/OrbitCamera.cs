using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private GameObject target;

    public float rotationSpeed = 10.0f;

    private float rotY;
    private Vector3 initialOffset;
    void Start()
    {
        rotY = 0;
        initialOffset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        rotY += Input.GetAxis("Mouse X") * rotationSpeed;

        Quaternion rotation = Quaternion.Euler(0, rotY, 0);
        transform.position = target.transform.position - (rotation * initialOffset);
        transform.LookAt(target.transform.position);
    }
}
