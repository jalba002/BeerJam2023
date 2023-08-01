using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int startingHealth = 5;

    private int currentHealth;

    public bool isAlive = true;

    public Action OnSpawn;
    public Action OnEliminated;

    public Animator EnemyAnimator;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public virtual void Spawn()
    {
        isAlive = true;
        currentHealth = startingHealth;
        OnSpawn?.Invoke();
    }

    public virtual void Eliminate()
    {
        isAlive = false;
        // Disable collisions too? For now it does not receive damage.
        // Do something or trigger OnEgvent
        OnEliminated?.Invoke();
    }

    public virtual void DealDamage(int damage)
    {
        if (!isAlive) return;
        currentHealth -= damage;

        EnemyAnimator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Eliminate();
        }
    }
}
