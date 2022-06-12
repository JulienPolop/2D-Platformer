using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public abstract class GameSceneManager : MonoBehaviour
{
    public GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Persistence.ActualLevel = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("GameManager");
        }
        else
        {
            if (GameManager.Player == null)
            {
                GameManager.Player = FindObjectOfType<Player_Controller>();
                if (GameManager.Player != null)
                {
                    if (GameManager.Player.gameObject.transform.parent != null)
                        GameManager.Player.gameObject.transform.parent = null;
                    SceneManager.MoveGameObjectToScene(GameManager.Player.gameObject, gameManager.gameObject.scene);
                }
            }
            else
            {
                gameManager.DeleteOtherClassGameObject(GameManager.Player);
            }
        }

        GameManager.MainCamera = FindObjectOfType<Camera>();

    }

    protected void Start()
    {
        GameManager.GameSceneManager = this;
        gameManager.OnSceneLoaded();
    }
}
