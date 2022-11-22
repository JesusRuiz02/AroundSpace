using UnityEngine;

public class ActivatingCollider : MonoBehaviour
{
  [SerializeField]  private AttackSystem LeftLeg;
  [SerializeField]  private AttackSystem RightLeg;
  [SerializeField]  private AttackSystem Punch;
    public void ActivateLeftScript()
    {
        LeftLeg.enabled = true;
    }
    public void ActivateRightScript()
    {
        LeftLeg.enabled = true;
    }

    public void ActivatePunch()
    {
        Punch.enabled = true;
    }
    
    public void DisableScript()
    {
       RightLeg.enabled = false;
       LeftLeg.enabled = false;
       Punch.enabled = false;
    }
}
