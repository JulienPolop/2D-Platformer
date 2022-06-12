using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;
using System;

public class TextBoxManager : MonoBehaviour
{

    public bool isActive;
    [Header("GameObjectsToModify")]
    public GameObject textBox;
    public Text theText;
    [Space]
    public float SpeedTyping;

    private bool isTyping = false;
    private bool cancelTyping = false;
    private bool cancelDialogue = false;


    Action OnEndDialogueCallback;
    IEnumerator OnEndDialogueIEnumerator;
    Player_Controller Player;

    // Use this for initialization
    void Start()
    {
        Player = GameManager.Player;
        OnEndDialogueCallback = null;
        OnEndDialogueIEnumerator = null;

        if (isActive)
        {
            EnableTextBox();
        }
        else
        {
            DisableTextBox();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Activate"))
            cancelTyping = true;
    }

    public IEnumerator StartDialogueEnumerator(string[] textLines)
    {
        OnEndDialogueCallback = null;
        OnEndDialogueIEnumerator = null;

        Player = GameManager.Player;
        textBox.SetActive(true);
        Player.State = Player_Controller.StateEnum.Speaking;
        isActive = true;
        cancelDialogue = false;
        yield return DialogueScroll(textLines);
    }
    public void StartDialogue(string[] TextLines, IEnumerator onEndDialogueIEnumerator)
    {
        OnEndDialogueIEnumerator = onEndDialogueIEnumerator;
        OnEndDialogueCallback = null;

        EnableTextBox(TextLines);
    }
    public void StartDialogue(string[] TextLines, Action onEndDialogue)
    {
        OnEndDialogueIEnumerator = null;
        OnEndDialogueCallback = onEndDialogue;

        EnableTextBox(TextLines);
    }
    public void StartDialogue(string[]  TextLines)
    {
        OnEndDialogueCallback = null;
        OnEndDialogueIEnumerator = null;

        EnableTextBox(TextLines);
    }

    private void OnEndDialogue()
    {
        OnEndDialogueCallback?.Invoke();
        if (OnEndDialogueIEnumerator != null)
            StartCoroutine(OnEndDialogueIEnumerator);
    }   



    public void EnableTextBox()
    {
        cancelDialogue = false;
        textBox.SetActive(true);
        Player.State = Player_Controller.StateEnum.Speaking;
        isActive = true;
    }
    public void EnableTextBox(string[] textLines)
    {
        cancelDialogue = false;
        Player = GameManager.Player;
        textBox.SetActive(true);
        Player.State = Player_Controller.StateEnum.Speaking;
        isActive = true;
        StartCoroutine(DialogueScroll(textLines));
    }
    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;
        if (Player != null)
            Player.State = Player_Controller.StateEnum.Normal;

        CancelDialogue();
        OnEndDialogue();
    }

    private void CancelDialogue()
    {
        cancelDialogue = true;
    }

    private IEnumerator DialogueScroll(string[] TextLines)
    {
        theText.text = "";
        yield return WaitForFrames(1);
        foreach (string line in TextLines)
        {
            if (!cancelDialogue)
            {
                isTyping = true;
                yield return TextScroll(line);
                yield return new WaitUntil(() => Input.GetButtonDown("Submit") || Input.GetButtonDown("Activate") || cancelDialogue);
            }
        }
        DisableTextBox();
    }

    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;

        yield return WaitForFrames(1);

        while (isTyping && (letter < lineOfText.Length) && !cancelDialogue)
        {
            if (cancelTyping)
            {
                theText.text = lineOfText;
                yield return CheckTextOverflow(lineOfText);
                break;
            }
            else
            {
                theText.text += lineOfText[letter];
                yield return CheckTextOverflow(lineOfText);
                letter += 1;
                yield return new WaitForSeconds(SpeedTyping);
            }
        }
        isTyping = false;
        //print("END COROUTINE");
    }

    public static IEnumerator WaitForFrames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
        //print("END CO 2");
    }

    public IEnumerator CheckTextOverflow(string lineOftext)
    {
        // do some actions here  
        yield return StartCoroutine(WaitForFrames(1)); // wait for 5 frames
                                                       // do some actions after 5 frames
                                                       //print(" CountVisible: " + theText.cachedTextGenerator.characterCountVisible + " LastCountVisible: " + textlines[currentLine].Length);
        if (theText.cachedTextGenerator.characterCountVisible < theText.text.Length && theText.cachedTextGenerator.characterCountVisible != -1)
        {
            //print("TEXT TROP LONG");
            string endOfPreviousLine = lineOftext.Substring(theText.cachedTextGenerator.characterCountVisible, lineOftext.Length - theText.cachedTextGenerator.characterCountVisible);
            //textlines[currentLine] = textlines[currentLine].Substring(0, theText.cachedTextGenerator.characterCountVisible);

            //endOfPreviousLine = endOfPreviousLine.Substring(0, endOfPreviousLine.Length-1);

            yield return new WaitUntil(() => Input.GetButtonDown("Submit") || Input.GetButtonDown("Activate"));
            yield return TextScroll(endOfPreviousLine);
        }
    }
}
