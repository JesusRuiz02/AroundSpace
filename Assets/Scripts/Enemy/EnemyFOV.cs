using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFOV : MonoBehaviour
{
    [SerializeField] private float _radius = default;
    [Range(0,360)]
    [SerializeField] private float _angle = default;
    [SerializeField] private GameObject _targetRef = default;
    [SerializeField] private LayerMask _targetMask = default;
    [SerializeField] private LayerMask _obstructionMask = default;
    [SerializeField] private bool _canSeeTarget = default;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform _firePosition;
    [SerializeField] private float _powerBullet = 250f;
    private float _nextFire = default;
    [SerializeField] private float _fireRate = 3f;
    private Vector3 _directionToTarget;
    private EnemyNavMesh _enemyNavMesh;
    private AnimalAI _animalAI = default;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

   [SerializeField] private AudioClip _fireball;
    private const string RangeAttack = "Cast Spell";
    void Start()
    {
        _animator = GetComponent<Animator>();
        _targetRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        _enemyNavMesh = GetComponent<EnemyNavMesh>();
        _animalAI = GetComponent<AnimalAI>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private IEnumerator Shoot()
    {
        _animator.SetTrigger(RangeAttack);
        yield return new WaitForSeconds(0.25f);
        GameObject bullet = Instantiate(projectile, _firePosition.position, Quaternion.identity);
        yield return new WaitForSeconds(0.25f);
        AudioManager.instance.PlaySFX(_fireball);
        if (bullet != null)
        {
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * _powerBullet, ForceMode.Acceleration);
        }
        Destroy(bullet, 10f);
    }

    private void Update()
    {
        if (_canSeeTarget)
        {
            if (Time.time >= _nextFire)
            {
                _nextFire = Time.time + 1f / _fireRate;
                StartCoroutine(Shoot());
            }
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _targetMask);
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            _directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, _directionToTarget) < _angle / 2) //Verify if the player is in the designed fov
            {
                float distanceToTarge = Vector3.Distance(transform.position, target.position); // Minimum distance to see the target/pl
                _canSeeTarget = !Physics.Raycast(transform.position, _directionToTarget, distanceToTarge, _obstructionMask);
                //Verify if an obstacle is in front of the target
                _enemyNavMesh.enabled = _canSeeTarget;
                ActivatingScripts();
            }
            else
            {
                _canSeeTarget = false;
                ActivatingScripts();
            }
        }
        else if (_canSeeTarget)
        {
            _canSeeTarget = false;
            ActivatingScripts();
        }
    }

    private void ActivatingScripts()
    {
        _enemyNavMesh.enabled = _canSeeTarget;
        _animalAI.enabled = !_canSeeTarget;
        _navMeshAgent.enabled = _canSeeTarget;
    }
}

