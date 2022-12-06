
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField] private float _rotationspeed = 3;
    [SerializeField] private bool _directionZTorotate = true;
    [SerializeField] private bool _directionXTorotate = false;
    [SerializeField] private bool _directionYTorotate = false;
    
    void FixedUpdate()
    {
        if (_directionZTorotate)
        {
            transform.Rotate(0,0,1 * _rotationspeed);  
        }
        if (_directionXTorotate)
        {
            transform.Rotate(1 * _rotationspeed,0, 0);
        }

        if (_directionYTorotate)
        {
            transform.Rotate(0,1 * _rotationspeed, 0);
        }
     
    }
}
