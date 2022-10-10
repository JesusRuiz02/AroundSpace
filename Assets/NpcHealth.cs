using System;
using UnityEngine;

public class NpcHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Conecto con el jugador");
        }
        if (collision.gameObject.CompareTag("Combat"))
        {
            _health -= 30;
        }
    }

    public void LostHealth()
    {
        _health -= 30;
        Debug.Log("Matalo");
    }
}
