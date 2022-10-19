using System.Collections;
using UnityEngine;

public class PlayerIDamage : MonoBehaviour, IDamageable
{
   [SerializeField] private AttackRadius _attackRadius;
   [SerializeField] private Animator _animator;
   private Coroutine LookCoroutine;
   [SerializeField] private int _health = 300;

   private const string ATTACK_TRIGGER = "Attack";

   

   private void Awake()
   {
      _attackRadius.OnAttack += OnAttack;
   }

   private void OnAttack(IDamageable Target)
   {
      _animator.SetTrigger(ATTACK_TRIGGER);
      if (LookCoroutine!= null)
      {
         StopCoroutine(LookCoroutine);
      }
      LookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));
   }

   private IEnumerator LookAt(Transform Target)
   {
      Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
      float time = 0;
      while (time < 1)
      {
         transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

         time += Time.deltaTime * 2;
         yield return null;
      }

      transform.rotation = lookRotation;
   }

   public void TakeDamage(int Damage)
   {
      _health -= Damage;
      if (_health <= 0)
      {
        Destroy(gameObject);
      }
   }

   public Transform GetTransform()
   {
      return transform;
   }
}
