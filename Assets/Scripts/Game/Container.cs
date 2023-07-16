using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Collections;

public class Container : MonoBehaviour
{
    public AreaGroup side;

    public Animator animator;

    [SerializeField] private SMBox objective;
    [SerializeField] private MeshRenderer roof;

    public bool debug = false;

    private bool doorStatus = false; // FAlse = close.

    [ReadOnly] [SerializeField] private int itemID = 0;
    public int ID => itemID;

    public void Open()
    {
        if (doorStatus) return;
        animator.SetTrigger("Open");
        objective.Enable();
        doorStatus = true;
        roof.enabled = false;
    }

    public void Close()
    {
        if (!doorStatus) return;
        animator.SetTrigger("Close");
        objective.Disable();
        doorStatus = false;
        roof.enabled = true;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (debug)
        {
            if (doorStatus) Close();
            else Open();
            debug = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.white;
        UnityEditor.Handles.Label(transform.position + Vector3.up * 3f, itemID.ToString());
#endif
    }
}
