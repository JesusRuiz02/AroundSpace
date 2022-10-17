using UnityEngine;

public class InvisibleFruit : MonoBehaviour
{
    private PlayerController _playerController;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Combat"))
        {
            _playerController.AddInvisibleCounter();
            Destroy(gameObject);
        }
    }
    
}
