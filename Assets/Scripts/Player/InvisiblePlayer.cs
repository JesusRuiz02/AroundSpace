using System.Collections;
using UnityEngine;

public class InvisiblePlayer : MonoBehaviour
{
     private LayerMask _ignore = default;
     private LayerMask _objective = default;

     private void Start()
  {
      _ignore = LayerMask.NameToLayer("Invisible");
      _objective = LayerMask.NameToLayer("AI");
  }

  public IEnumerator Invisible()
  {
      gameObject.layer = _ignore;
      ChangingLayerChildren(_ignore);
      yield return new WaitForSeconds(5f);
      ChangingLayerChildren(_objective);
      gameObject.layer = _objective;
  }

  private void ChangingLayerChildren(LayerMask layer)
  {
      for (int i = 0; i < gameObject.transform.childCount; i++)
      {
          GameObject mesh = gameObject.transform.GetChild(i).gameObject;
          mesh.layer = layer;
      }
  }
  
}
