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
    }

    public void LostHealth()
    {
        _health -= 10;
        Debug.Log("Matalo");
    }
}
