using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class FindSwordScript : ObjectToSave
{
    public TextAsset DialogueFile;
    public string DialogueName;
    public string DialogueGetSwordName;

    [Header("After Activation")]
    public DialogueManager dialoguePanneauToChange;
    public string newDialogue;
    private bool Activated = false;




    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangeStateAferActivation()
    {
        Activated = true;
        dialoguePanneauToChange.DialogueName = newDialogue;
    }

    public void StartScript()
    {
        if (!Activated)
        {
            FindObjectOfType<TextBoxManager>().StartDialogue(Assets.Scripts.Helpers.XMLParser.XMLtoArray(DialogueFile, DialogueName), GetSword);
            ChangeStateAferActivation();
        }
        else
            FindObjectOfType<TextBoxManager>().StartDialogue(Assets.Scripts.Helpers.XMLParser.XMLtoArray(DialogueFile, DialogueName));
    }

    void GetSword()
    {
        FindObjectOfType<TextBoxManager>().StartDialogue(Assets.Scripts.Helpers.XMLParser.XMLtoArray(DialogueFile, DialogueGetSwordName), EndDialogue);
        GameManager.Player.AttackUnlocked = true;
        GameManager.Player.GetComponent<Animator>().SetBool("IsShowingSword", true);
    }

    void EndDialogue()
    {
        GameManager.Player.GetComponent<Animator>().SetBool("IsShowingSword", false);
    }

    public override void LoadFromSaveObject(SaveObjectBase saveObject)
    {
        if(saveObject.Activated)
        {
            ChangeStateAferActivation();
        }
    }

    public override SaveObjectBase GetSaveObject()
    {
        return new SaveObjectBase
        {
            Activated = this.Activated,
            GuidString = this.GuidString
        };
    }
}
