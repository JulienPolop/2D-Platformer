using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Helpers;

public class DialogueManager : MonoBehaviour
{
    [Header("Text Parameters")]
    public TextAsset TextToDisplay;
    public string DialogueName;
    private TextBoxManager theTextBox;

    [Header("Activation Parameters")]
    public bool RequireButtonPress;
    public bool PlayerInZone;
    public bool destroyObjectWhenActivated = false;


    private bool frameButtonIsPressed = false;
    private Player_Controller playerController;


    // Use this for initialization
    void Start()
    {

        //Debug.Log(texts[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (frameButtonIsPressed)
        {
            StartDialogue();
            frameButtonIsPressed = false;
        }
        if (RequireButtonPress && PlayerInZone && Input.GetButtonDown("Activate") && playerController.State == Player_Controller.StateEnum.Normal && playerController.IsGrounded)
        {
            frameButtonIsPressed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player_Controller>() != null)
        {
            playerController = other.gameObject.GetComponent<Player_Controller>();
            PlayerInZone = true;
            if (!RequireButtonPress)
                StartDialogue();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player_Controller>() != null)
        {
            PlayerInZone = false;
            playerController = null;
        }
    }

    private void StartDialogue()
    {
        string[] texts = XMLParser.XMLtoArray(TextToDisplay, DialogueName);

        theTextBox = FindObjectOfType<TextBoxManager>();
        theTextBox.StartDialogue(texts.ToArray());

        if (destroyObjectWhenActivated)
        {
            Destroy(this.gameObject);
        }
    }
}
