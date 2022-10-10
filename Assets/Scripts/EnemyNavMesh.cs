using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    [SerializeField] private GameObject _targetPosition;
    private NavMeshAgent _navMeshAgent;

    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _targetPosition = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        _navMeshAgent.destination = _targetPosition.transform.position;
    }
}
