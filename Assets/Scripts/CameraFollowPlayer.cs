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
        Vector3 lookPosition = transform.position + Vector3.up * CalculateVerticalOffset(15f, offset.magnitude);
        Camera.main.transform.LookAt(lookPosition);
    }

    private float CalculateVerticalOffset(float angleInDegrees, float distanceToPlayer)
    {
        // This calculates the vertical offset based on the tangent of the angle and the distance to the player
        return Mathf.Tan(angleInDegrees * Mathf.Deg2Rad) * distanceToPlayer;
    }

    private void RotatePlayer(Quaternion rotation)
    {
        transform.rotation *= rotation;
    }
}
