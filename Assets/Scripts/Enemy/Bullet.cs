using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
