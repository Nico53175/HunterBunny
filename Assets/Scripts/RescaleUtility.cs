#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class RescaleUtility : MonoBehaviour
{
    [ContextMenu("Set Current Scale as Default")]
    void SetScaleAsDefault()
    {
        GameObject newRoot = new GameObject(gameObject.name + "_root");
        newRoot.transform.SetPositionAndRotation(transform.position, transform.rotation);
        newRoot.transform.localScale = transform.localScale;

        gameObject.transform.SetParent(newRoot.transform);
        gameObject.name = gameObject.name.Replace("_root", "");

        transform.localScale = Vector3.one;
    }
}
#endif