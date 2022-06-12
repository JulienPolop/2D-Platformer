using UnityEngine;
using System.Collections;

public class MinotaurFightManager : MonoBehaviour
{
    public Scr_Minautor Minotaure;
    private Player_Controller Player;
    [Header("StartDialogue")]
    public TextAsset DialogueFile;
    public string DialogueName;
    public string Dialogue2Name;
    public string Dialogue3Name;
    [Header("Triggers")]
    public GameObject triggerEnterStartFight;
    public GameObject triggerDoorDestroyed;
    public GameObject triggerPauseMinotaure;
    public GameObject triggerEnterStartSecondFight;
    public GameObject triggerEndFight;
    public GameObject triggerEnterSecondFight;
    public GameObject triggerMinotaureKO;
    [Header("CameraPlacement")]
    public Transform TargetCameraPosition1;
    public Transform TargetCameraPosition2;

    private bool FightPassed = false;

    // Use this for initialization
    void Start()
    {
        Player = GameManager.Player;

        if (triggerEnterStartFight == null)
            Debug.Log("NULL");
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerEnterStartFight != null && triggerEnterStartFight.GetComponent<Collider2D>().IsTouching(Player.GetComponent<Collider2D>()))
            StartFight();

        if (triggerDoorDestroyed != null && triggerDoorDestroyed.GetComponent<Collider2D>().IsTouching(Minotaure.GetComponent<Collider2D>()))
            OnTriggerDoorDestroyed();

        if (triggerPauseMinotaure != null && triggerPauseMinotaure.GetComponent<Collider2D>().IsTouching(Minotaure.GetComponent<Collider2D>()))
            OnTriggerPauseMinotaure();

        if (triggerEnterStartSecondFight != null && triggerEnterStartSecondFight.GetComponent<Collider2D>().IsTouching(Player.GetComponent<Collider2D>()))
            OnTriggerStartSecondFight();

        if (triggerEnterSecondFight != null && triggerEnterSecondFight.GetComponent<Collider2D>().IsTouching(Player.GetComponent<Collider2D>()))
            OnTriggerEnterSecondFight();

        if (triggerEndFight != null && triggerEndFight.GetComponent<Collider2D>().IsTouching(Player.GetComponent<Collider2D>()) /*&& triggerEndFight.GetComponent<Collider2D>().IsTouching(Minotaure.GetComponent<Collider2D>()) && Minotaure.State == Scr_Minautor.StateEnum.isKO*/)
            OnTriggerEndFight();

        if (triggerMinotaureKO != null && triggerMinotaureKO.GetComponent<Collider2D>().IsTouching(Minotaure.GetComponent<Collider2D>()) && Minotaure.State == Scr_Minautor.StateEnum.isKO)
            OnTriggerMinotaureKO();
    }

    public void StartFight()
    {
        Destroy(triggerEnterStartFight.gameObject);
        triggerEnterStartFight = null;

        Minotaure.Stop();
        FindObjectOfType<CameraFollow>().target = Minotaure.gameObject;
        FindObjectOfType<TextBoxManager>().StartDialogue(Assets.Scripts.Helpers.XMLParser.XMLtoArray(DialogueFile, DialogueName), EndDialogue);
    }

    public void EndDialogue()
    {
        FindObjectOfType<CameraFollow>().target = TargetCameraPosition1.gameObject;
        Minotaure.PrepareCharge(Assets.Scripts.Helpers.HorizontalDirection.Left);
    }

    public void OnTriggerDoorDestroyed()
    {
        Destroy(triggerDoorDestroyed.gameObject);
        triggerDoorDestroyed = null;

        Player.State = Player_Controller.StateEnum.Speaking;
    }

    public void OnTriggerPauseMinotaure()
    {
        Destroy(triggerPauseMinotaure.gameObject);
        triggerPauseMinotaure = null;

        Minotaure.Stop(Assets.Scripts.Helpers.HorizontalDirection.Left);
        Player.State = Player_Controller.StateEnum.Normal;

        FindObjectOfType<CameraFollow>().target = Player.TargetCamera;
    }

    public void OnTriggerStartSecondFight()
    {
        Destroy(triggerEnterStartSecondFight.gameObject);
        triggerEnterStartSecondFight = null;

        FindObjectOfType<TextBoxManager>().StartDialogue(Assets.Scripts.Helpers.XMLParser.XMLtoArray(DialogueFile, Dialogue2Name), EndDialogue2);
        FindObjectOfType<CameraFollow>().target = Minotaure.gameObject;
    }
    public void EndDialogue2()
    {
        Minotaure.PrepareCharge(Assets.Scripts.Helpers.HorizontalDirection.Left);
        FindObjectOfType<CameraFollow>().target = TargetCameraPosition2.gameObject;
    }
    public void OnTriggerEnterSecondFight()
    {
        //FindObjectOfType<CameraFollow>().target = TargetCameraPosition2.gameObject;
    }

    public void OnTriggerEndFight()
    {
        //FindObjectOfType<CameraFollow>().target = Player.TargetCamera;
        //FightPassed = true;
    }

    public void OnTriggerMinotaureKO()
    {
        Minotaure.MakeKoForGood();
        FindObjectOfType<CameraFollow>().target = Minotaure.gameObject;
        FindObjectOfType<TextBoxManager>().StartDialogue(Assets.Scripts.Helpers.XMLParser.XMLtoArray(DialogueFile, Dialogue3Name), EndDialogue3);
    }

    public void EndDialogue3()
    {
        FindObjectOfType<CameraFollow>().target = Player.TargetCamera;
    }
}
