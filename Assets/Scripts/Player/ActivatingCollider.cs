using UnityEngine;

public class ActivatingCollider : MonoBehaviour
{
  [SerializeField]  private AttackSystem LeftLeg;
  [SerializeField]  private AttackSystem RightLeg;
    public void ActivateLeftScript()
    {
        LeftLeg.enabled = true;
    }
    public void ActivateRightScript()
    {
        LeftLeg.enabled = true;
    }
    
    public void DisableScript()
    {
       RightLeg.enabled = false;
       LeftLeg.enabled = false;
    }
}
