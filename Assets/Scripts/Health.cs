using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private AudioClip _sfx;
    [SerializeField] private AudioClip _deathsfx;
    [SerializeField] private float _volumeSFX = 1;
    [SerializeField] private float _currentHealth = 5;
    [SerializeField] private float _maxHealth = 5;
    [SerializeField] private bool _isEnemy = false;

    public event Action<float, float> OnHealthChanged = delegate { };
    public event Action OnDeath = delegate { };
    public UnityEvent OnKilled;

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Kill();
        }
        else
        {
            if (_sfx != null)
            {
                AudioHelper.PlayClip2D(_sfx, _volumeSFX);
            }
            
        }
        
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    private void Kill()
    {
        if (_deathsfx != null) {
            AudioHelper.PlayClip2D(_deathsfx, .4f);
            
        }
        if (_isEnemy)
        {
            SaveManager.Instance.ActiveSaveData.Score += 1;
            Destroy(gameObject);
        }
        OnDeath?.Invoke();
        OnKilled.Invoke();
        
    }
}
