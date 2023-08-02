using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public Transform waterSplash;

    private void OnTriggerEnter(Collider other)
    {
        // Splash
        // Instantiate(waterSplash);
        var splash = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        splash.transform.position = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
    }
}
