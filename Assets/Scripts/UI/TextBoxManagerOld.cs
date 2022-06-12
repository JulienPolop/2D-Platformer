using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;
using System;

public class TextBoxManagerOld : MonoBehaviour
{
    [Header("GameObjectsToModify")]
    public GameObject textBox;
    public Text theText;
    public string textToPrint;

    private string[] textlines;
    private string endOfPreviousLine = null;

    public int currentLine;
    public int endAtLine;

    public bool isActive;

    private bool isTyping = false;
    private bool cancelTyping = false;

    private bool Disable_next_Frame = false;

    public float SpeedTyping;

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
        if(isActive)
        {
            if (Disable_next_Frame)
            {
                Disable_next_Frame = false;
                DisableTextBox();
            }


            if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Activate"))
            {
                if (!isTyping)
                {
                    if (endOfPreviousLine == null)
                    {
                        currentLine += 1;

                        if (currentLine > endAtLine)
                        {
                            Disable_next_Frame = true;
                            //Debug.Log("DISABLE");
                            //DisableTextBox();
                        }
                        else
                        {
                            StartCoroutine(TextScroll(textlines[currentLine]));
                        }
                    }
                    else
                    {
                        textlines[currentLine] = endOfPreviousLine;
                        StartCoroutine(TextScroll(textlines[currentLine]));
                        endOfPreviousLine = null;
                    }
                }
                else if (isTyping && !cancelTyping)
                {
                    //print("CANCEL TYPING");
                    cancelTyping = true;
                    theText.text = textlines[currentLine];

                    StartCoroutine(CheckTextOverflow());

                }
            }
        }
    }

    public void StartDialogue(string[] TextLines, IEnumerator onEndDialogueIEnumerator)
    {
        OnEndDialogueIEnumerator = onEndDialogueIEnumerator;
        OnEndDialogueCallback = null;

        textlines = TextLines;
        StartDialogue();
    }
    public void StartDialogue(string[] TextLines, Action onEndDialogue)
    {
        OnEndDialogueIEnumerator = null;
        OnEndDialogueCallback = onEndDialogue;

        textlines = TextLines;
        StartDialogue();
    }
    public void StartDialogue(string[]  TextLines)
    {
        OnEndDialogueCallback = null;
        OnEndDialogueIEnumerator = null;
        textlines = TextLines;
        StartDialogue();
    }

    private void StartDialogue()
    {
        Player = GameManager.Player;
        currentLine = 0;
        endAtLine = textlines.Length - 1;
        EnableTextBox();
    }
    private void OnEndDialogue()
    {
        OnEndDialogueCallback?.Invoke();
        if (OnEndDialogueIEnumerator != null)
            StartCoroutine(OnEndDialogueIEnumerator);
    }   



    public void EnableTextBox()
    {
        textBox.SetActive(true);
        Player.State = Player_Controller.StateEnum.Speaking;
        isActive = true;
        StartCoroutine(TextScroll(textlines[currentLine]));
    }
    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;
        if (Player != null)
            Player.State = Player_Controller.StateEnum.Normal;

        OnEndDialogue();
    }

    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;

        while (isTyping && !cancelTyping && (letter < lineOfText.Length))
        {
            theText.text += lineOfText[letter];

            if (theText.cachedTextGenerator.characterCountVisible < letter && theText.cachedTextGenerator.characterCountVisible != -1)
            {
                //print("TEXT TROP LONG COROUTINE");
                endOfPreviousLine = lineOfText.Substring(theText.cachedTextGenerator.characterCountVisible, lineOfText.Length - theText.cachedTextGenerator.characterCountVisible);
                lineOfText = lineOfText.Substring(0, theText.cachedTextGenerator.characterCountVisible);
            }

            letter += 1;
            yield return new WaitForSeconds(SpeedTyping);
        }
        isTyping = false;
        cancelTyping = false;
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

    public IEnumerator CheckTextOverflow()
    {
        // do some actions here  
        yield return StartCoroutine(WaitForFrames(5)); // wait for 5 frames
                                                       // do some actions after 5 frames
        //print(" CountVisible: " + theText.cachedTextGenerator.characterCountVisible + " LastCountVisible: " + textlines[currentLine].Length);
        if (theText.cachedTextGenerator.characterCountVisible < textlines[currentLine].Length && theText.cachedTextGenerator.characterCountVisible != -1)
        {
            //print("TEXT TROP LONG");
            endOfPreviousLine = textlines[currentLine].Substring(theText.cachedTextGenerator.characterCountVisible, textlines[currentLine].Length - theText.cachedTextGenerator.characterCountVisible);
            textlines[currentLine] = textlines[currentLine].Substring(0, theText.cachedTextGenerator.characterCountVisible);


            endOfPreviousLine = endOfPreviousLine.Substring(3, endOfPreviousLine.Length);

        }
    }
}
