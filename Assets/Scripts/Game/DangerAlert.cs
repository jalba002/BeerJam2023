using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerAlert : MonoBehaviour
{
    public float duration = 5f;
    public Vector3 offset = new Vector3(0f, 1f, 0f);
    private Camera attachedCamera;
    private Transform attachedTransform;
    private void Start()
    {
        Destroy(this.gameObject, duration);
        attachedCamera = FindObjectOfType<Camera>();
    }

    public void Attach(Transform transform)
    {
        attachedTransform = transform;
    }
    private void Update()
    {
        Vector2 newPos = attachedCamera.WorldToScreenPoint(attachedTransform.position);
        // Limit resolution?
        newPos.x = Mathf.Clamp(newPos.x, Screen.width * 0.05f, Screen.width *0.95f);
        newPos.y = Mathf.Clamp(newPos.y, Screen.height * 0.05f, Screen.height * 0.95f);
        transform.position = newPos + (Vector2)offset;
    }
}
