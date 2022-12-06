using System;
using System.Collections;
using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] appleSpawners;
    [SerializeField] private GameObject apples;
    private bool canSpawn = true;
    [SerializeField] private float rateSpawn = 20f;

    private void Start()
    {
        appleSpawners = GameObject.FindGameObjectsWithTag("AppleTree");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canSpawn)
        {
            foreach (var trees in appleSpawners)
            {
                Instantiate(apples, trees.transform.position, trees.transform.rotation);
            }
            canSpawn = false;
        }
        else
        {
            StartCoroutine(RestartBool());
        }
    }

  
    private IEnumerator RestartBool()
    {
        yield return new WaitForSeconds(rateSpawn);
        canSpawn = true;
    }
}
