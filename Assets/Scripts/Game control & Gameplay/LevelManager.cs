using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : GameSceneManager
{
    public GameObject Player;
    public Camera Camera;
    
    [Header("Camera Limits")]
    public float Haut;
    public float Bas;
    public float Gauche;
    public float Droite;


    public List<GameObject> LevelEntries;



    // Use this for initialization
    new void Start()
    {
        Player = GameManager.Player.gameObject;
        Camera = FindObjectOfType<CameraFollow>().GetComponent<Camera>();
        Persistence.LevelManager = this;



        PlacePlayerInLevel();

        //Si le dictionnaire de persistence est null
        if (Persistence.GameSave != null && Persistence.GameSave.LevelsSavableObjects == null)
        {
            Persistence.GameSave.LevelsSavableObjects = new Dictionary<string, List<SaveObjectBase>>();
        }

        LoadSavableObject();

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnQuitLevel()
    {
        SaveSavableObject();
    }

    void PlacePlayerInLevel()
    {
        if (GameManager.PlayerisRespawningInSavePoint)
        {
            GameManager.PlayerisRespawningInSavePoint = false;
            SavePoint SavePointToRespawn = FindObjectsOfType<SavePoint>().Where((sp) => sp.GuidString == Persistence.GameSave.GuidStringSavePoint).First();
            Player.transform.position = SavePointToRespawn.transform.position;

            GameManager.MainCamera.transform.position = new Vector3(GameManager.Player.TargetCamera.transform.position.x, GameManager.Player.TargetCamera.transform.position.y, GameManager.MainCamera.transform.position.z);
            GameManager.Player.GetComponent<Player_HealthManager>().PlayerHealth = GameManager.Player.GetComponent<Player_HealthManager>().MaxHealth;
            GameManager.Player.GetComponent<Player_HealthManager>().PlayerLife = GameManager.Player.GetComponent<Player_HealthManager>().BeginLife;
        }
        else
        {
            int? EntrieIndexToPlacePlayer = Persistence.IndexNextLevelEntrie;
            if (EntrieIndexToPlacePlayer != null && LevelEntries.Count >= EntrieIndexToPlacePlayer)
            {
                Player.transform.position = LevelEntries[EntrieIndexToPlacePlayer.Value].transform.position;
                Player.GetComponent<Player_Controller>().MoveDirection = Persistence.PlayerDirection;
                Player.GetComponent<Player_Controller>().IsGrounded = false;
                Player.GetComponent<Player_Controller>().CanNormalJump = LevelEntries[EntrieIndexToPlacePlayer.Value].GetComponent<LevelEntrie>().canJump;
                Player.GetComponent<Player_Controller>().MoveTargetCamera();

                Camera.gameObject.transform.position = new Vector3(Player.GetComponent<Player_Controller>().TargetCamera.transform.position.x, Player.GetComponent<Player_Controller>().TargetCamera.transform.position.y, Camera.gameObject.transform.position.z);

                Player.GetComponent<Rigidbody2D>().velocity = new Vector2(Player.GetComponent<Rigidbody2D>().velocity.x, 0);
            }
        }
    }

    public void SaveSavableObject()
    {
        List<ObjectToSave>  ListObjectToSave = FindObjectsOfType<ObjectToSave>().ToList();
        Debug.Log("ISavableObject Count: " + ListObjectToSave.Count);
        for (int i = 0; i < ListObjectToSave.Count; i++)
        {
            //ListSavabaleObjects[i].IdInScene = i.ToString;
            string name = "";
            if (ListObjectToSave[i] != null)
            {
                name = ListObjectToSave[i].gameObject.name;
            }

            Debug.Log(ListObjectToSave[i].GuidString + "   " + name);
        }

        List<SaveObjectBase> ListSaveObject = new List<SaveObjectBase>();
        foreach (ObjectToSave objectToSave in ListObjectToSave)
            ListSaveObject.Add(objectToSave.GetSaveObject());

        if (Persistence.GameSave.LevelsSavableObjects.ContainsKey(this.gameObject.scene.name))
            Persistence.GameSave.LevelsSavableObjects.Remove(this.gameObject.scene.name);

        Persistence.GameSave.LevelsSavableObjects.Add(this.gameObject.scene.name, ListSaveObject);
    }
    private void LoadSavableObject()
    {
        List<SaveObjectBase> ListSaveObject;

        if (Persistence.GameSave.LevelsSavableObjects.TryGetValue(this.gameObject.scene.name, out ListSaveObject))
            Debug.Log("SceneDéjaSauvegardée, il y a: " + ListSaveObject.Count );

        if (ListSaveObject != null)
        {
            foreach (SaveObjectBase saveObject in ListSaveObject)
            {
                FindObjectsOfType<ObjectToSave>().Where((objectToSave) => objectToSave.GuidString == saveObject.GuidString).First().LoadFromSaveObject(saveObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 posHautGauche = new Vector2(transform.position.x - Gauche, transform.position.y + Haut);
        Vector2 posHautDroit = new Vector2(transform.position.x + Droite, transform.position.y + Haut);
        Vector2 posBasGauche = new Vector2(transform.position.x - Gauche, transform.position.y - Bas);
        Vector2 posBasDroit = new Vector2(transform.position.x + Droite, transform.position.y - Bas);


        Gizmos.DrawLine(new Vector2(posHautGauche.x, posHautGauche.y), new Vector2(posHautDroit.x, posHautDroit.y));
        Gizmos.DrawLine(new Vector2(posBasGauche.x, posBasGauche.y), new Vector2(posBasDroit.x, posBasDroit.y));
        Gizmos.DrawLine(new Vector2(posHautGauche.x, posHautGauche.y), new Vector2(posBasGauche.x, posBasGauche.y));
        Gizmos.DrawLine(new Vector2(posHautDroit.x, posHautDroit.y), new Vector2(posBasDroit.x, posBasDroit.y));
    }
}
