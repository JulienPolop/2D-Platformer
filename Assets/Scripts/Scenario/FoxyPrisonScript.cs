using UnityEngine;
using System.Collections;

public class FoxyPrisonScript : MonoBehaviour
{
   public enum StateEnum
    {
        Interraction1,
        Interraction2,
        Finish,
    }

    [Header("Fox To Control")]
    public FoxyController FoxController;

    [Header("Actual State")]
    StateEnum State = StateEnum.Interraction1;

    [Header("Texts to Display")]
    public TextAsset DialogueFile;



   public void Interraction()
    {
        switch (State)
        {
            case StateEnum.Interraction1:
                Interraction1();
                break;
            case StateEnum.Interraction2:
                Interraction2();
                break;
        }
    }






    void Interraction1()
    {
        StartCoroutine(Cutscene());
    }
    IEnumerator Cutscene()
    {
        FoxController.ChangeLookDirection(Assets.Scripts.Helpers.HorizontalDirection.Left);
        yield return FindObjectOfType<TextBoxManager>().StartDialogueEnumerator(Assets.Scripts.Helpers.XMLParser.XMLtoArray(DialogueFile, "DialogueRenard0"));
        GameManager.Player.State = Player_Controller.StateEnum.Pause;
        FoxController.March(Assets.Scripts.Helpers.HorizontalDirection.Left);
        yield return new WaitForSeconds(0.3f);
        FoxController.Stop();
        yield return FindObjectOfType<TextBoxManager>().StartDialogueEnumerator(Assets.Scripts.Helpers.XMLParser.XMLtoArray(DialogueFile, "DialogueRenard1"));
        GameManager.Player.State = Player_Controller.StateEnum.Pause;
        FoxController.March(Assets.Scripts.Helpers.HorizontalDirection.Right);
        yield return new WaitForSeconds(0.3f);
        FoxController.Stop();
        GameManager.Player.State = Player_Controller.StateEnum.Normal;

        State = StateEnum.Interraction2;
    }



    void Interraction2() //Check si le joueur possède la clée
    {
        StartCoroutine(Cutscene2());
    }
    IEnumerator Cutscene2()
    {
        FoxController.ChangeLookDirection(Assets.Scripts.Helpers.HorizontalDirection.Left);
        yield return FindObjectOfType<TextBoxManager>().StartDialogueEnumerator(Assets.Scripts.Helpers.XMLParser.XMLtoArray(DialogueFile, "DialogueRenard2"));
        FoxController.ChangeLookDirection(Assets.Scripts.Helpers.HorizontalDirection.Right);
    }


}
