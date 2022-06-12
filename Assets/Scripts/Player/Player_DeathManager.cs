using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_DeathManager : MonoBehaviour {
    [Header("Checkpoint for spikes")]
    public GameObject currentCheckpoint;
    public int pointPenaltyOnDeath = 50;
    [Header("Respawn")]
    public float Respawn_time = 1;
    public bool isDestroyed;
    private float Timer_Respawn;

    [Header("Particles")]
    public GameObject respawnParticle;
    public GameObject DeathParticle;
    public GameObject DrownParticle;


    // Use this for initialization
    void Start () {
        Instantiate(respawnParticle, transform);
        isDestroyed = false;
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void Kill()
    {
        Instantiate(DeathParticle, transform);

        GetComponent<Player_ScoreManager>().PlayerScore -= pointPenaltyOnDeath;
        GetComponent<Player_HealthManager>().PlayerHealth--;
        RespawnPlayer();
    }

    public void Drown()
    {
        Instantiate(DrownParticle, transform);

        GetComponent<Player_ScoreManager>().PlayerScore -= pointPenaltyOnDeath;
        GetComponent<Player_HealthManager>().PlayerHealth--;
        RespawnPlayer();
    }

    public void Hit(int Damage_dealt)
    {
        GetComponent<Player_HealthManager>().PlayerHealth -= Damage_dealt;
    }


    #region Respawn
    public void RespawnPlayer()
    {
        // Appelé au début du respawn, permet de faire disparaitre le joueur et d'attendre un peu avant de Respawn.
        DestroyPlayer();
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);

        // Le véritable Respawn


        if (GetComponent<Player_HealthManager>().PlayerHealth > 0)
        {
            RecoverPlayer();

            transform.position = new Vector3(currentCheckpoint.transform.position.x, currentCheckpoint.transform.position.y, 0);
            GameObject Particles = Instantiate(respawnParticle, transform);
            Particles.layer = 11;
        }

        else
        {
            PlayerLoseLife();
        }
    }

    public void DestroyPlayer()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Player_Controller>().State = Player_Controller.StateEnum.Death;
    }

    public void RecoverPlayer()
    {
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GetComponent<SpriteRenderer>().enabled = true;

        GetComponent<Player_Controller>().State = Player_Controller.StateEnum.Normal;
    }
    #endregion


    public void PlayerLoseLife()
    {
        GetComponent<Player_HealthManager>().PlayerLife--;
        CheckAfterLoseLife();
    }

    public void CheckAfterLoseLife()
    {
        if (GetComponent<Player_HealthManager>().PlayerLife <= 0)
        {
            PlayerLose();
        }
        else
        {
            GetComponent<Player_HealthManager>().PlayerHealth = GetComponent<Player_HealthManager>().MaxHealth;
            FindObjectOfType<GameManager>().ResetActualLevel();
        }
    }

    public void PlayerLose()
    {
        //isRespawningInSavePoint = true;
        //FindObjectOfType<GameManager>().ChangeLevel(Persistence.GameSave.LevelSavePoint, null, this.gameObject);

        FindObjectOfType<GameManager>().MovePlayerTolastSavePoint(this.gameObject);
    }


}
