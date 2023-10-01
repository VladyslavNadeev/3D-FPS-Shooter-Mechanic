using UnityEngine;

public class VitalityBehaviour : MonoBehaviour, IDamageable
{
    [Header("Health System")] 
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _health;

    public int CurrentHealth
    {
        get => _health;
        private set => _health = value;
    }

    public int MaxHealth
    {
        get => _maxHealth;
        private set => _maxHealth = value;
    }

    public bool IsDead { get; set; }

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    private void OnEnable()
    {
        CurrentHealth = MaxHealth;
        IsDead = false;
    }

    public void TakeDamage(int damage)
    {
        int damageTaken = Mathf.Clamp(damage, 0, CurrentHealth);

        CurrentHealth -= damageTaken;
        
        if (damageTaken != 0)
        {
            OnTakeDamage?.Invoke(damageTaken);
        }

        if (CurrentHealth == 0 && damageTaken != 0)
        {
            IsDead = true;
            OnDeath?.Invoke(transform.position);
        }
    }
}