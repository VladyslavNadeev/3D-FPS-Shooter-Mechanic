using UnityEngine;

public interface IDamageable
{
    int CurrentHealth { get; }
    int MaxHealth { get; }
    
    bool IsDead { get; set; }

    delegate void TakeDamageEvent(int damage);

    event TakeDamageEvent OnTakeDamage;

    delegate void DeathEvent(Vector3 position);

    event DeathEvent OnDeath;

    void TakeDamage(int damage);
}