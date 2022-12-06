using UnityEngine;

public class NpcHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _knockBack = 300;
    [SerializeField] private Rigidbody _rigidbody;
    EnemyStats _enemyStats;
    Animator animator;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _enemyStats = GetComponent<EnemyStats>();
        _knockBack = _enemyStats.KnockBackForce;
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
            Destroy(gameObject, 1.0f);
        }
    }
    public void KnockBack(GameObject player)
    { 
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        _rigidbody.AddForce(direction * _knockBack); 
    }
}
