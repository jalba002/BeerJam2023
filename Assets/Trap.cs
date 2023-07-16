using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trap : MonoBehaviour, IActivatable
{
    public float rechargeTime = 10f;

    public bool isActivatable = true;

    public Animator animator;

    private float nextRecharge = 0f;

    private IEnumerator cor;

    public bool forceRecharge = false;

    protected Vector3 startingPosition;
    protected Quaternion startingRotation;

    // Is ignored here
    private void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    public virtual bool Activate(Transform activator)
    {
        if (!isActivatable || nextRecharge > 0f) return false;
        // Otherwise, DO!
        // play animation
        // play sound.
        Debug.Log("Activate Trap");
        //animator.SetTrigger("Play");
        nextRecharge = rechargeTime;
        if (cor != null) StopCoroutine(cor);
        cor = RechargeEnum();
        StartCoroutine(cor);
        // If everything went alright, return true so we can apply some feedback!
        TrapBehaviour(activator);
        return true;
    }

    protected virtual void TrapBehaviour(Transform activator)
    {
        // nothing at will.
    }

    private IEnumerator RechargeEnum()
    {
        while (nextRecharge > 0f)
        {
            yield return new WaitForSecondsRealtime(1f);
            nextRecharge -= 1f;  
        }
        //animator.SetTrigger("Recharge");
        Recharge();
    }

    protected virtual void Recharge()
    {
        // Here it will recharge usage. 
        // For barrel, it will appear again.
        // For the anchor, it will appear again.
        // The object is never destroyed nor pooled. It does the action, it remains there.
        // As barrel is an element of the game, so it must stay deactivated, and recharge means it reappears
        // Anchor doesn't deactivate, the lever doesn't either, it just recharges.
    }

    private void OnValidate()
    {
        if (forceRecharge)
        {
            Recharge();
            forceRecharge = false;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
