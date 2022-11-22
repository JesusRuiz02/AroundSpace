using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Animator _animator;
    
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _currentHealth = 100;
    private bool isInmune = false;
    [SerializeField] private Image _HurtImage = default;
    [SerializeField] private GameObject SceneManager = default;
    [SerializeField] private GameObject _gameoverCanvas = default;
    [SerializeField] private AudioClip _audioHurt = default;
    private ChangeScene _changeScene;
    private PlayerController _playerController;
    private PlayerStats _playerStats;
    [Header("RegenVoid")]
    [SerializeField] private float _regenRate = 1;
    [SerializeField] private bool _startCoolddown = false; 
    [SerializeField] private bool canRegen = false;
    [SerializeField] private float _healCooldown = 3.0f;
    [SerializeField] private float _maxHealthCooldown = 3.0f;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _changeScene = SceneManager.GetComponent<ChangeScene>();
        _playerController = GetComponent<PlayerController>();
        _playerStats = GetComponent<PlayerStats>();
        _maxHealth = _playerStats.Health;
        _currentHealth = _maxHealth;
        UpdateHealth();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        AudioManager.instance.PlaySFX(_audioHurt);
        if (_currentHealth > 0)
        {
            canRegen = false;
            UpdateHealth();
            _healCooldown = _maxHealthCooldown;
            _startCoolddown = true;
        }
        if  (_currentHealth <= 0)
        {
            _animator.SetTrigger("Dead");
            _gameoverCanvas.SetActive(true);
            _playerController.enabled = false;

        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !isInmune)
        {
            TakeDamage(33);
        }
    }

    public IEnumerator InmunityTime(float duration)
    {
        isInmune = true;
        yield return new WaitForSeconds(duration);
        isInmune = false;
    }
    private void UpdateHealth()
    {
        float alph = (1 - (_currentHealth / _maxHealth)) * 255;
        alph = Mathf.Round(alph);
        if (alph > 255)
        {
            alph = 255;
        }
        byte alpha = Convert.ToByte(alph);
        _HurtImage.color = new Color(255, 255, 255, alpha);     
    }
    private void Update()
    {
        if(_startCoolddown)
        {
            _healCooldown -= Time.deltaTime;
            if(_healCooldown <= 0)
            {
                canRegen = true;
                _startCoolddown = false;
            }
        }
        if (canRegen)
        {
            if (_currentHealth <= _maxHealth - 0.01)
            {
                _currentHealth += Time.deltaTime * _regenRate;
                UpdateHealth();
            }
            else
            {
                _currentHealth = _maxHealth;
                _healCooldown = _maxHealthCooldown;
                canRegen = false;
            }
        }
    }
}
