using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public string levelToLoad;
    public int indexEntrie;
    public GameObject GameObjectLevelToLoad;

    private bool PlayerInZone;
    private bool used = false;
    public bool needInterraction;

    private GameObject Player;

	// Use this for initialization
	void Start () {
        PlayerInZone = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerInZone && !used)
        {
            if (needInterraction && Input.GetButtonDown("Up"))
            {
                ChangeLevel();
            }
            else if (!needInterraction)
            {
                ChangeLevel();
            }
        }

	}
    private void ChangeLevel()
    {
        FindObjectOfType<GameManager>().ChangeLevel(levelToLoad, indexEntrie, Player);
        used = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInZone = true;
            Player = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInZone = false;
            Player = null;
        }
    }
}
