using UnityEngine;

public class CameraMinimapFollowPlayer : MonoBehaviour
{
    private Transform player;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        Camera minimapCamera = GetComponent<Camera>();
        minimapCamera.orthographicSize = offset.y;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        // Update the position of the camera to match the player's position, but maintain the same height.
        transform.position = player.position + offset;
    }
}
