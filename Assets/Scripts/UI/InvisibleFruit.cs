using System;
using UnityEngine;

public class InvisibleFruit : MonoBehaviour
{
   [SerializeField] private PlayerController _playerController;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Combat") || collision.gameObject.CompareTag("Player"))
        {
            _playerController.AddInvisibleCounter();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Combat") || other.gameObject.CompareTag("Player"))
        {
            _playerController.AddInvisibleCounter();
            Destroy(gameObject);
        }
    }
}
