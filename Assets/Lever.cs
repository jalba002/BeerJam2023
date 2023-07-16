using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Trap
{
    public Trap referencedTrap;

    public override bool Activate(Transform activator)
    {
        if (base.Activate(activator))
        {
            animator.SetTrigger("Activate");

        }
        return false;
    }

    protected override void Recharge()
    {
        // Play the recharge animation
        animator.SetTrigger("Recharge");
    }
}
