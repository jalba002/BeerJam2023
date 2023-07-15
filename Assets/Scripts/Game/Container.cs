using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Container : MonoBehaviour
{
    public AreaGroup side;
    private Animator animator;

    [SerializeField] private SMBox objectiveA;
    [SerializeField] private SMBox objectiveB;

    public bool debugA = false;
    public bool debugB = false;

    private bool doorAStatus = false; // FAlse = close.
    private bool doorBStatus = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenA()
    {
        if (doorAStatus) return;
        animator.SetTrigger("OpenA");
        objectiveA.enabled = true;
        doorAStatus = true;
    }
    public void OpenB()
    {
        if (doorBStatus) return;
        animator.SetTrigger("OpenB");
        objectiveB.enabled = true;
        doorBStatus = true;
    }

    public void CloseA()
    {
        if (!doorAStatus) return;
        animator.SetTrigger("CloseA");
        objectiveA.enabled = false;
        doorAStatus = false;
    }
    public void CloseB()
    {
        if (!doorBStatus) return;
        animator.SetTrigger("CloseB");
        objectiveB.enabled = false;
        doorBStatus = false;
    }

    private void OnValidate()
    {
        if (debugA)
        {
            if (doorAStatus) CloseA();
            else OpenA();
            debugA = false;
        }
        else if (debugB)
        {
            if (doorBStatus) CloseB();
            else OpenB();
            debugB = false;
        }
    }
}
