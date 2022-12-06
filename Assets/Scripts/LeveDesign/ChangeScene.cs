using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private GameObject _CanvasPause;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private AudioClip _startMusic;
    private void OnTriggerEnter(Collider other)
    { 
        if(other.CompareTag("Planet"))
        {
            Debug.Log("Hola");
            SceneManager.LoadScene(0);
        }
        /*  else if(other.CompareTag("SpaceShip"))
        {
            ChangeSceneToSpace(1);
        }*/
      
    }
    public void ChangeSceneToSpace(int numberScene)
    {
        SceneManager.LoadScene(numberScene);
    }
    public void RunningState(float currentState)
    {
        Time.timeScale = currentState;
    }

    public void ActiveState(bool activeState,GameObject canvas)
    {
        float currentState = activeState ? 0 : 1;
        RunningState(currentState);
        canvas.SetActive(activeState);
    }
    
    public void PauseSettings(bool state)
    {
        ActiveState(state, _CanvasPause);
    }

    public void VolumeSettings(bool state)
    { 
        ActiveState(state, _settingsCanvas);
    }
    
    private void Start()
    {
        Time.timeScale = 1;
        AudioManager.instance.PlayMusic(_startMusic);
    }

    public void ActivateGameOver()
    {
        RunningState(0);
        _gameOverCanvas.SetActive(true);
    }

}
