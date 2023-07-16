using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Trap
{
    public List<Trap> referencedTraps = new List<Trap>();

    public override bool Activate(Transform activator)
    {
        if (base.Activate(activator))
        {
            animator.SetTrigger("Activate");
            foreach (Trap trap in referencedTraps)
            {
                trap.Activate(activator);
            }
        }
        return false;
    }

    protected override void Recharge()
    {
        // Play the recharge animation
        animator.SetTrigger("Recharge");
    }
}
