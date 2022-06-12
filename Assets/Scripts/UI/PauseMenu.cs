using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public string MainMenu;
    public string LevelSelect;
    private bool IsPaused = false;
    public GameObject pauseMenuCanvas;

    public void Start()
    {

    }

    private void Update()
    {

    }

    public void Pause()
    {
        IsPaused = true;
        pauseMenuCanvas.SetActive(true);
        GameManager.IsPaused = true;
        Time.timeScale = 0;
    }
    public void Continue()
    {
        IsPaused = false;
        GameManager.IsPaused = false;
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1;
    }
    public void ChangeState()
    {
        if (!IsPaused)
        {
            Pause();
        }
        else if (IsPaused)
        {
            Continue();
        }
    }


    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        FindObjectOfType<GameManager>().ResetActualLevel();
    }

    public void SelectLevel()
    {
        SceneManager.LoadScene(LevelSelect);
    }

    public void QuitToMainMenu()
    {
        //SceneManager.LoadScene(MainMenu);
        FindObjectOfType<GameManager>().ChangeLevel(MainMenu, null, null);
    }
}
