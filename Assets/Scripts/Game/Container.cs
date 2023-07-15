using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public AreaGroup side;

    public Animator animatorA;
    public Animator animatorB;

    [SerializeField] private SMBox objectiveA;
    [SerializeField] private SMBox objectiveB;

    public bool debugA = false;
    public bool debugB = false;

    private bool doorAStatus = false; // FAlse = close.
    private bool doorBStatus = false;

    private void Awake()
    {
    }

    public void OpenA()
    {
        if (doorAStatus) return;
        animatorA.SetTrigger("Open");
        objectiveA.enabled = true;
        doorAStatus = true;
    }
    public void OpenB()
    {
        if (doorBStatus) return;
        animatorB.SetTrigger("Open");
        objectiveB.enabled = true;
        doorBStatus = true;
    }

    public void CloseA()
    {
        if (!doorAStatus) return;
        animatorA.SetTrigger("Close");
        objectiveA.enabled = false;
        doorAStatus = false;
    }
    public void CloseB()
    {
        if (!doorBStatus) return;
        animatorB.SetTrigger("Close");
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
