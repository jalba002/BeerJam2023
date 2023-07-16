using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMBox_Pickable : SMBox
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            Disable();
            Destroy(this.gameObject);
        }
    }
}
