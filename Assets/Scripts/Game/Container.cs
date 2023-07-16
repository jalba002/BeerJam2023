using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Collections;

public class Container : MonoBehaviour
{
    public AreaGroup side;

    public Animator animatorA;
    public Animator animatorB;

    [SerializeField] private SMBox objectiveA;
    [SerializeField] private SMBox objectiveB;
    [SerializeField] private MeshRenderer roof;

    public bool debugA = false;
    public bool debugB = false;

    private bool doorAStatus = false; // FAlse = close.
    private bool doorBStatus = false;

    [ReadOnly] [SerializeField] private int itemID = 0;

    private void Awake()
    {

    }

    public void OpenA()
    {
        if (doorAStatus) return;
        animatorA.SetTrigger("Open");
        objectiveA.enabled = true;
        doorAStatus = true;
        if (roof.enabled) roof.enabled = false;
    }
    public void OpenB()
    {
        if (doorBStatus) return;
        animatorB.SetTrigger("Open");
        objectiveB.enabled = true;
        doorBStatus = true;
        if (roof.enabled) roof.enabled = false;
    }

    public void CloseA()
    {
        if (!doorAStatus) return;
        animatorA.SetTrigger("Close");
        objectiveA.enabled = false;
        doorAStatus = false;
        if (doorBStatus) roof.enabled = true;
    }
    public void CloseB()
    {
        if (!doorBStatus) return;
        animatorB.SetTrigger("Close");
        objectiveB.enabled = false;
        doorBStatus = false;
        if (doorAStatus) roof.enabled = true;   
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

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.Label(transform.position + Vector3.up * 3f, itemID.ToString());
#endif
    }
}
