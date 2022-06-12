using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : GameSceneManager
{
    public string levelSelect;

    public Button ContinueButton;

    private void Start()
    {
        if (!GameManager.CheckSaveFile())
        {
            ContinueButton.interactable = false;
        }
        else
        {
            ContinueButton.interactable = true;
        }

        base.Start();
    }

    public void NewGame()
    {
        //SceneManager.LoadScene(startLevel);


        GameManager gm = FindObjectOfType<GameManager>();
        gm.StartNewGame();
    }

    public void Continue()
    {
        FindObjectOfType<GameManager>().ContinueGame();
        GameManager.LoadFromSaveFile();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
