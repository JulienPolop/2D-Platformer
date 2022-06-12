using Assets.Scripts;
using UnityEngine;

public class SavePoint : UniqueGuidObject
{
    public void ChangePlayerSavePoint()
    {
        Persistence.GameSave.LevelSavePoint = this.gameObject.scene.name;
        Persistence.GameSave.GuidStringSavePoint = this.GuidString;

        Debug.Log("SAVE: "+GuidString);

        if (GameManager.GameSceneManager is LevelManager)
        {
            LevelManager levelManager = (LevelManager)GameManager.GameSceneManager;
            levelManager.SaveSavableObject();
        }

        GameManager.SaveToSaveFile();

        FindObjectOfType<Player_HealthManager>().FullHeal();
    }
}
