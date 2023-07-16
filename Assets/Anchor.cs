using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : Trap
{
    private Rigidbody rb;
    protected override void TrapBehaviour(Transform activator)
    {
        // Give it a rigidbody and remove static.
        if (rb == null) rb = this.gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.angularDrag = 0f;
        rb.drag = 0f;
        // Duration is 5s or so.
        Invoke("LimitDuration", duration);
    }

    private void LimitDuration()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    protected override void Recharge()
    {
        GetComponentInChildren<MeshRenderer>().enabled = true;
        isActivatable = true;
        transform.position = startingPosition;// Default one
        transform.rotation = startingRotation;// Default one
        Destroy(rb); // Remove RB
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If collision is an enemy that has HealthManager, OOF it.
        collision.gameObject.GetComponent<HealthManager>()?.DealDamage(1978);
    }
}
