using System.Collections;
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
    private EnemyNavMesh _enemyNavMesh;
    private AnimalAI _animalAI = default;
    private NavMeshAgent _navMeshAgent;
    void Start()
    {
        _targetRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRountine());
        _enemyNavMesh = GetComponent<EnemyNavMesh>();
        _animalAI = GetComponent<AnimalAI>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private IEnumerator FOVRountine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _angle / 2) //Verify if the player is in the designed fov
            {
                float distanceToTarge = Vector3.Distance(transform.position, target.position); // Minimum distance to see the target/pl
                _canSeeTarget = !Physics.Raycast(transform.position, directionToTarget, distanceToTarge, _obstructionMask);
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

