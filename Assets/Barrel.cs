using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Trap
{
    public float thrustForce = 25f;
    Rigidbody rb;

    protected override void TrapBehaviour(Transform caster)
    {
        //Debug.Log("Trap Behaviour! " + gameObject.name);
        // Gather the direction of the player or forward
        // Give the barrel force instant!

        // Add a rigidbody.
        // Gather the direction
        // 
        // Set the Y to zero to just adapt the direction? It will be crazy
        rb = this.gameObject.AddComponent<Rigidbody>();
        rb.angularDrag = 0.01f;
        rb.drag = 0.05f;
        rb.useGravity = true;
        rb.isKinematic = false;

        Vector3 direction = caster.forward;

        rb.AddForce(direction * thrustForce);
        //rb.AddExplosionForce(thrustForce, caster.transform.position + transform.up * .5f, 3f);
        isActivatable = false;
    }

    protected override void Recharge()
    {
        //base.Recharge();
        isActivatable = true;
        transform.position = startingPosition;// Default one
        transform.rotation = startingRotation;// Default one
        Destroy(rb); // Remove RB
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If it has enough speed, then collide against enemies!
        // Maybe it should play an animation before starting rolling?
    }
}