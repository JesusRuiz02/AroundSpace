using System;
using System.Collections;
using UnityEngine;

public class InvisiblePlayer : MonoBehaviour
{
    [SerializeField] private float transparency = 0f;
     private LayerMask _ignore = default;
     private LayerMask _objective = default;
     [SerializeField] private Renderer _renderer;

  private void Start()
  {
      _ignore = LayerMask.NameToLayer("OBstacles");
      _objective = LayerMask.NameToLayer("AI");
  }

  public IEnumerator Invisible()
   {
      
       _renderer.material.color = new Color(1, 1, 1, transparency);
       gameObject.layer = _ignore;
        Debug.Log(gameObject.layer);
        yield return new WaitForSeconds(3f);
        gameObject.layer = _objective;
        Debug.Log(gameObject.layer);
        _renderer.material.color = new Color(1, 1, 1, 1);
   }
}
