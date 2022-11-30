using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InvisiblePlayer : MonoBehaviour
{
     private LayerMask _ignore = default;
     private LayerMask _objective = default;
     [SerializeField] private GameObject invisibleCanvas;
     private float _timer = 0.9f;
    

     private void Start()
  {
      _ignore = LayerMask.NameToLayer("Invisible");
      _objective = LayerMask.NameToLayer("AI");
  }

  public IEnumerator Invisible()
  {
      float timer = _timer;
      invisibleCanvas.SetActive(true);
      gameObject.layer = _ignore;
      ChangingLayerChildren(_ignore);
      yield return new WaitForSeconds(2f);
      while (_timer >= 0.1f)
      {
          invisibleCanvas.SetActive(false);
          yield return new WaitForSeconds(0.1f);
          invisibleCanvas.SetActive(true);
          yield return new WaitForSeconds(timer);
          timer -= 0.1f;
      }
      invisibleCanvas.SetActive(false);
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
