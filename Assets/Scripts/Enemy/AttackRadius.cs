using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRadius : MonoBehaviour
{
   public SphereCollider collider;
   private List<IDamageable> _damageables = new List<IDamageable>(); 
   public int _damage = 10;
   public float _attackDelay = 0.5f;

   public delegate void AttackEvent(IDamageable Target);

   public AttackEvent OnAttack;
   private Coroutine _attackCoroutine;

   private void Awake()
   {
      collider = GetComponent<SphereCollider>();
   }

   private void OnTriggerEnter(Collider other)
   {
      IDamageable damageable = other.GetComponent<IDamageable>();
      if (damageable != null)
      {
         _damageables.Add(damageable);
         if (_attackCoroutine == null)
         {
            _attackCoroutine = StartCoroutine(Attack());
         }
      }
   }

   private void OnTriggerExit(Collider other)
   {
      IDamageable damageable = other.GetComponent<IDamageable>();
      if (damageable != null)
      {
         _damageables.Remove(damageable);
         if (_damageables.Count == 0)
         {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
         }
      }
   }

   private IEnumerator Attack()
   {
      WaitForSeconds Wait = new WaitForSeconds(_attackDelay);
      
      yield return Wait;

      IDamageable closestDamageable = null;
      float closestDistance = float.MaxValue;
      while (_damageables.Count > 0)
      {
         for (int i = 0; i < _damageables.Count; i++)
         {
            Transform damageableTransform = _damageables[i].GetTransform();
            float distance = Vector3.Distance(transform.position, damageableTransform.position);
            if (distance < closestDistance)
            {
               closestDistance = distance;
               closestDamageable = _damageables[i];
            }
         }

         if (closestDamageable != null)
         {
            OnAttack?.Invoke(closestDamageable);
            closestDamageable.TakeDamage(_damage);
         }

         closestDamageable = null;
         closestDistance = float.MaxValue;

         yield return Wait;
       //  _damageables.RemoveAll(DisableDamageables);
      }
      _attackCoroutine = null;
   }

  /* private bool DisableDamageables(IDamageable damageable)
   {
      return damageable != null && !damageable.GetTransform().gameObject.activeSelf;
   }*/
}
