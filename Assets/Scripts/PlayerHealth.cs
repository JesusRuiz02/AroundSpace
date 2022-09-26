using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth = default;
    [SerializeField] private float _currentHealth = default;
    [SerializeField] private bool isInmune = false;
    private PlayerStats _playerStats;
    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
        _maxHealth = _playerStats.Health;
        _currentHealth = _maxHealth;
    }

    private void TakeDamage(float damage)
    {
        
    }
}
