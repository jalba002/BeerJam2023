using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRail : MonoBehaviour
{
    public Transform target;
    public float movementSpeed = 10f;
    public Vector2 clampDistances;

    private void LateUpdate()
    {
        // Move camera towards player
        var newPosition = target.position;
        float newX = Mathf.Lerp(transform.position.x, newPosition.x, movementSpeed * Time.deltaTime);
        newX = Mathf.Clamp(newX, clampDistances.x, clampDistances.y);
        newPosition = transform.position;
        newPosition.x = newX;
        this.transform.position = newPosition;
    }
}
