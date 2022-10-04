using System;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
   [SerializeField] private Rigidbody _rigidbody;

   private void Start()
   {
      _rigidbody = GetComponent<Rigidbody>();
   }

   /*private void OnCollisionEnter(Collision collision)
   {
      Debug.Log("Hola");
      if (collision.gameObject.CompareTag("Enemy"))
      {
         collision.gameObject.GetComponent<NpcHealth>().LostHealth();
      }
   }*/
}
