using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private float _maxHealth = default;
    [SerializeField] private float _currentHealth = default; 
    private bool isInmune = false;
    [SerializeField] private GameObject SceneManager = default;
    private ChangeScene _changeScene;
    private PlayerStats _playerStats;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _changeScene = SceneManager.GetComponent<ChangeScene>();
        _playerStats = GetComponent<PlayerStats>();
        _maxHealth = _playerStats.Health;
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if  (_currentHealth <= 0)
        {
            _animator.SetTrigger("Dead");
            Destroy(gameObject,4f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInmune)
        {
            TakeDamage(100);
        }
    }

    public IEnumerator InmunityTime(float duration)
    {
        isInmune = true;
        yield return new WaitForSeconds(duration);
        isInmune = false;
    }
}
