using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    [SerializeField] Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.transform.position = transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = transform.position + offset;
    }
}
