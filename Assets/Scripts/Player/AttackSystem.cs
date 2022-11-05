using System;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
   [SerializeField] private Rigidbody _rigidbody;
   private Collider _boxcollider;

   private void Start()
   {
      _rigidbody = GetComponent<Rigidbody>();
   }
   private void OnCollisionEnter(Collision collision)
   {
 
      if (collision.gameObject.CompareTag("Enemy"))
      {
         collision.gameObject.GetComponent<NpcHealth>().LostHealth();
      }
   }
}
