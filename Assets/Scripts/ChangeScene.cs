using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private GameObject _CanvasPause;
    private void OnTriggerEnter(Collider other)
    { 
        if(other.CompareTag("Planet"))
        {
            Debug.Log("Hola");
            SceneManager.LoadScene(0);
        }
        else if(other.CompareTag("SpaceShip"))
        {
            ChangeSceneToSpace(1);
        }
      
    }
    public void ChangeSceneToSpace(int numberScene)
    {
        SceneManager.LoadScene(numberScene);
    }
    public void RunningState(float currentState)
    {
        Time.timeScale = currentState;
    }

    public void ActiveState(bool activeState)
    {
        float CurrentState = activeState ? 0 : 1;
        RunningState(CurrentState);
        _CanvasPause.SetActive(activeState);
    }

    public void PauseSettings(bool State)
    {
        _CanvasPause.SetActive(State);
    }
    
    private void Start()
    {
        Time.timeScale = 1;
      
    }

    public void ActivateGameOver()
    {
        RunningState(0);
        _gameOverCanvas.SetActive(true);
    }

}
