using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class EnemyDamageable : MonoBehaviour, IDamageable
{
  private EnemyNavMesh _enemyNavMesh;
  public int Health = 100;
  private EnemyStats _enemyStats;
  private NavMeshAgent _agent;
  private const string ATTACK_TRIGGER = "Attack";
  private AttackRadius _attackRadius;
  private Animator _animator = default;
  private Coroutine LookCoroutine;

  private void OnAttack(IDamageable Target)
  {
    _animator.SetTrigger(ATTACK_TRIGGER);
    if (LookCoroutine!= null)
    {
      StopCoroutine(LookCoroutine);
    }
    LookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));
  }

  private IEnumerator LookAt(Transform Target)
  {
    Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
    float time = 0;
    while (time < 1)
    {
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

      time += Time.deltaTime * 2;
      yield return null;
    }

    transform.rotation = lookRotation;
  }

  private void Awake()
  {
    _attackRadius.OnAttack += OnAttack;
    _enemyStats = GetComponent<EnemyStats>();
  }

  private void Start()
  {
     SetupAgentFromConfiguration();
  }

  private void SetupAgentFromConfiguration()
  {
    _agent.acceleration = _enemyStats.Acceleration;
    _agent.angularSpeed = _enemyStats.AngularSpeed;
    _agent.areaMask = _enemyStats.AreaMask;
    _agent.avoidancePriority = _enemyStats.AvoidancePriority;
    _agent.baseOffset = _enemyStats.BaseOffset;
    _agent.obstacleAvoidanceType = _enemyStats.ObstacleAvoidanceType;
    _agent.speed = _enemyStats.Speed;
    _agent.stoppingDistance = _enemyStats.StoppingDistance;
    _attackRadius.collider.radius = _enemyStats.AttackRadius;
    _attackRadius._attackDelay = _enemyStats.AttackDelay;
    _attackRadius._damage = _enemyStats.Damage;
  }

  public void TakeDamage(int Damage)
  {
    Health -= Damage;
    if (Health >= 0)
    {
      Destroy(gameObject);
    }
  }

  public Transform GetTransform()
  {
    return transform;
  }
}
