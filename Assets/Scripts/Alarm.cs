using BEER2023.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : Trap
{
    [Header("Alarm")]
    public BoxCollider coveredArea;
    public float force = 500f;
    public Vector3 direction = Vector3.zero;

    protected override void TrapBehaviour(Transform activator)
    {
        coveredArea.enabled = true;
    }

    protected override void Recharge()
    {
        coveredArea.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Repel for X seconds.
        // Repel on a set directon (WORLD ONE)
        if (coveredArea.enabled && remainingDuration > 0f)
        {
            var enemy = other.gameObject.GetComponent<EnemyController>();
            if (enemy != null) { enemy.Ragdoll(direction * force, false, ForceMode.Impulse); }
        }
    }
}
