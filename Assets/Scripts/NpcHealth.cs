using System;
using UnityEngine;

public class NpcHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100;
    EnemyStats _enemyStats;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        _enemyStats = GetComponent<EnemyStats>();
        _health = _enemyStats.Health;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Conecto con el jugador");
        }
    }

    public void LostHealth()
    {
        _health -= 25;
       if(_health <=0 )
        {
            _enemyStats.TurnOffScripts();
            animator.SetTrigger("Die");
            Destroy(gameObject, 2f);
        }
    }
    
}
