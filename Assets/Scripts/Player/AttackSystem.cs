using System;
using System.Collections;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
   [SerializeField] private Rigidbody _rigidbody;
   private Collider _boxcollider;
   private void Start()
   {
      _rigidbody = GetComponent<Rigidbody>();
   }
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Enemy"))
      {
         other.GetComponent<NpcHealth>().KnockBack(gameObject);
        other.gameObject.GetComponent<NpcHealth>().LostHealth();
      }
   }

   
}
