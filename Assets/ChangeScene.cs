using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
 
    private void OnTriggerEnter(Collider other)
    { 
        if(other.CompareTag("Planet"))
        {
            SceneManager.LoadScene(0);
        }
        else if(other.CompareTag("SpaceShip"))
        {
            ChangeSceneToSpace();
        }
      
    }

    public void ChangeSceneToSpace()
    {
        SceneManager.LoadScene(1);
    }

}
