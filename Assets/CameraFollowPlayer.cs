using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] private float rotationSpeed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera.main.transform.position = transform.position + offset;
    }

    void Update()
    {
        Camera.main.transform.position = transform.position + offset;
        RotateCamera();
    }

    private void RotateCamera()
    {
        float horizontalInput = Input.GetAxis("Mouse X");
        float desiredRotationAngle = horizontalInput * rotationSpeed;

        Quaternion rotation = Quaternion.Euler(0, desiredRotationAngle, 0);
        offset = rotation * offset;
        RotatePlayer(rotation);
        Camera.main.transform.LookAt(transform.position);
    }

    private void RotatePlayer(Quaternion rotation)
    {
        transform.rotation *= rotation;
    }
}
