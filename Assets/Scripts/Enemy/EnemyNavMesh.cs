using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    [SerializeField] private GameObject _targetPosition;
    private NavMeshAgent _navMeshAgent;
    private bool _isActive;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _targetPosition = GameObject.FindGameObjectWithTag("Player");
        _animator.SetBool("Run Forward",true);
    }

    private void OnDisable()
    {
        _animator.SetBool("Run Forward",false);
    }

    private void Update()
    {
        _navMeshAgent.destination = _targetPosition.transform.position;
    }
    
  
}
