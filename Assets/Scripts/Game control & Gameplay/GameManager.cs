using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using System.IO;
using Newtonsoft.Json;
using Assets.Scripts.Helpers;

class Change
{
    public long coin2 = 0;
    public long bill5 = 0;
    public long bill10 = 0;
}

public class GameManager : MonoBehaviour
{
    [Header("Level")]
    public string mainMenuLevel;
    public string startGameLevel;
    public string ActualLevel;
    private string LevelToLoad;
    static public GameSceneManager GameSceneManager;
    [Header("UI")]
    public FadeUI FadeUI;
    public TextBoxManager TextBoxManager;
    public PauseMenu PauseMenu;
    public GameObject GameUI;
    [Header("Player")]
    static public Player_Controller Player;
    static public bool PlayerisRespawningInSavePoint;
    static public Camera MainCamera;
    [Header("Pause")]
    static public bool IsPaused;
    [Header("Cinematic")]
    static public bool InCinematic;

    static public string defaultGuidStringSavePoint = "30e01815-b96a-49a5-8029-54b30aca5118";

    static public readonly string SaveFileUrl = "saveJson123.json";

    private void Start()
    {
        IsPaused = false;
        PauseMenu.Continue();

        InCinematic = false;

        if (FindObjectOfType<LevelManager>() == null)
        {
            if (Persistence.ActualLevel == null)
            {
                SceneManager.LoadScene(mainMenuLevel, LoadSceneMode.Additive);
                ActualLevel = mainMenuLevel;
            }
            else
            {
                SceneManager.LoadScene(Persistence.ActualLevel, LoadSceneMode.Additive);
                ActualLevel = Persistence.ActualLevel;
            }
        }
        else
        {
            ActualLevel = FindObjectOfType<LevelManager>().gameObject.scene.name;
        }

        Player = FindObjectOfType<Player_Controller>();
        if (Player != null)
            SceneManager.MoveGameObjectToScene(Player.transform.root.gameObject, SceneManager.GetActiveScene());

        if (Persistence.GameSave == null)
        {
            Persistence.GameSave = new GameSaveObject();
            Persistence.GameSave.LevelSavePoint = startGameLevel;
            Persistence.GameSave.GuidStringSavePoint = defaultGuidStringSavePoint;
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //Debug.Log("CANCEL");
            if (!FadeUI.IsFading)
            {
                if (TextBoxManager.isActive)
                {
                    TextBoxManager.DisableTextBox();
                }
                else
                {
                    if (IsPaused)
                    {
                        IsPaused = false;
                        PauseMenu.Continue();
                    }
                    else if (!IsPaused)
                    {
                        IsPaused = true;
                        PauseMenu.Pause();
                    }
                }
            }
        }
    }

    public void StartNewGame()
    {
        Persistence.GameSave = new GameSaveObject();
        Persistence.GameSave.LevelSavePoint = startGameLevel;
        Persistence.GameSave.GuidStringSavePoint = defaultGuidStringSavePoint;

        SaveToSaveFile();

        ChangeLevel(startGameLevel, null, null);
        Persistence.IndexNextLevelEntrie = 0;
    }
    public void ContinueGame()
    {
        MovePlayerTolastSavePoint(null);
    }


    public void OnSceneLoaded()
    {
        //Appelé lors du Start du LevelManager
        if (GameSceneManager is MainMenuManager)
        {
            PauseMenu.Continue();
            IsPaused = false;
            GameUI.SetActive(false);
            if (Player != null)
            {
                Destroy(Player.gameObject);
            }
        }
        else if (GameSceneManager is LevelManager)
        {
            PauseMenu.Continue();
            IsPaused = false;
            GameUI.SetActive(true);

            if (MainCamera.GetComponent<CameraFollow>() != null)
                MainCamera.GetComponent<CameraFollow>().target = Player.TargetCamera;
        }
    }

    static public bool CheckSaveFile()
    {
        return File.Exists(SaveFileUrl);
    }

    static public void LoadFromSaveFile()
    {
        try
        {
            //Persistence.GameSave = null;
            Persistence.GameSave = JsonConvert.DeserializeObject<GameSaveObject>(File.ReadAllText(SaveFileUrl));
        }
        catch
        {
            Debug.LogError("Erreur lors de la récupération de la sauvegarde");
        }

    }

    static public void SaveToSaveFile()
    {
        string jsonString = JsonConvert.SerializeObject(Persistence.GameSave, Formatting.Indented);
        File.WriteAllText(SaveFileUrl, jsonString);
    }

    public void ResetActualLevel()
    {
        if (Persistence.IndexNextLevelEntrie == null)
            Persistence.IndexNextLevelEntrie = 0;
        ChangeLevel(ActualLevel, Persistence.IndexNextLevelEntrie.Value, Player.gameObject);
    }


    #region ChangeLevel
    public void ChangeLevel(string levelToLoad, int? indexEntrie, GameObject Player)
    {
        if (FadeUI.IsFading == false)
        {
            PrepareChangeLevel(levelToLoad, indexEntrie, Player);

            FadeUI.FadeOut(FadeOutChangeLevelAndSaveActualCallBack);
        }
    }
    public void ChangeLevelWithoutSavingActual(string levelToLoad, int? indexEntrie, GameObject Player)
    {
        PrepareChangeLevel(levelToLoad, indexEntrie, Player);

        FadeUI.FadeOut(FadeOutChangeLevelCallBack);
    }
    private void PrepareChangeLevel(string levelToLoad, int? indexEntrie, GameObject Player)
    {
        if (Player != null) //pour placer le joueur s'il existe dans la scene gameManager
        {
            if (Player.gameObject.transform.parent == null)
                SceneManager.MoveGameObjectToScene(Player.gameObject, this.gameObject.scene);
            else
                SceneManager.MoveGameObjectToScene(Player.transform.root.gameObject, this.gameObject.scene);
        }

        LevelToLoad = levelToLoad;

        //if (Player != null)
        //    Persistence.PlayerDirection = Player.GetComponent<Player_Controller>().MoveDirection;
        if (indexEntrie != null)
            Persistence.IndexNextLevelEntrie = indexEntrie.Value;
        Persistence.NextLevel = levelToLoad;
    }

    private void FadeOutChangeLevelAndSaveActualCallBack()
    {
        if (GameSceneManager is LevelManager)
        {
            (GameSceneManager as LevelManager).OnQuitLevel();
        }

        FadeOutChangeLevelCallBack();
    }
    private void FadeOutChangeLevelCallBack()
    {
        if (Player != null)
            if (Player.GetComponent<Player_Controller>().State == Player_Controller.StateEnum.Death)
                Player.GetComponent<Player_DeathManager>().RecoverPlayer();

        SceneManager.UnloadSceneAsync(ActualLevel);

        SceneManager.LoadScene(LevelToLoad, LoadSceneMode.Additive);
        ActualLevel = LevelToLoad;

        FadeUI.FadeIn(FadeInLevelCallBack);
    }
       
    private void FadeInLevelCallBack()
    {

    }

    public void MovePlayerTolastSavePoint(GameObject playerGameObject)
    {
        LoadFromSaveFile();
        PlayerisRespawningInSavePoint = true;
        ChangeLevelWithoutSavingActual(Persistence.GameSave.LevelSavePoint, null, playerGameObject);
    }
    #endregion


    public void DeleteOtherClassGameObject<T>(T ObjectToKeep) where T : UnityEngine.Behaviour
    {
        T[] tab = FindObjectsOfType<T>();
        foreach (T t in tab)
        {
            if (t.gameObject != null && t.gameObject != ObjectToKeep.gameObject)
                Destroy(t.gameObject);
        }
    }
}
