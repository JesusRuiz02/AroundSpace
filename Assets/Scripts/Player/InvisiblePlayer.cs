using System;
using System.Collections;
using UnityEngine;

public class InvisiblePlayer : MonoBehaviour
{
   [SerializeField] private LayerMask _ignore = default;
   [SerializeField] private LayerMask _objective = default;
   private MeshRenderer _meshRenderer;

   private void Start()
   {
       _meshRenderer = GetComponent<MeshRenderer>();
   }

   public IEnumerator Invisible()
    {
        gameObject.layer = _ignore;
        yield return new WaitForSeconds(3f);
        gameObject.layer = _objective;
    }
}
