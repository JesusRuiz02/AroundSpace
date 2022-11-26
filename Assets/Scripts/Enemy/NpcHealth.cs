using System;
using UnityEngine;

public class NpcHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _knocback = 300;
    EnemyStats _enemyStats;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        _enemyStats = GetComponent<EnemyStats>();
        _knocback = _enemyStats.KnockBackForce;
        _health = _enemyStats.Health;
    }
    public void LostHealth()
    {
        _health -= 25;
       if(_health <=0 )
        {
            _enemyStats.StateScripts(); //disable scripts
            animator.SetTrigger("Die");
            GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject, 1.8f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Combat"))
        {
            Debug.Log("jala");
          GetComponent<Rigidbody>().AddForce(Vector3.back * _knocback);
        }
    }
}
