using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterractionInfoTextBoxManager : MonoBehaviour
{

    public Image textBox;
    public Text theText;

    public enum fadeChoice
    {
        FadeIn,
        FadeOut,
    }
    public fadeChoice FadeChoice;

    private bool StopFadeIn;

    // Start is called before the first frame update
    void Start()
    {
        textBox.color = new Color(1, 1, 1, 0);
        theText.color = new Color(1, 1, 1, 0);

        StopFadeIn = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator FadeInTextBox()
    {
        // loop over 1 second
        for (float i = 0; i <= 0.75 && !StopFadeIn; i += Time.deltaTime*4)
        {
            // set color with i as alpha
            textBox.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }
    IEnumerator FadeInText()
    {
        // loop over 1 second
        for (float i = 0; i <= 1 && !StopFadeIn; i += Time.deltaTime*4)
        {
            // set color with i as alpha
            theText.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }


    IEnumerator FadeOutTextBox()
    {
        // loop over 1 second
        for (float i = textBox.color.a; i >= 0; i -= Time.deltaTime*4)
        {
            // set color with i as alpha
            textBox.color = new Color(1, 1, 1, i);
            yield return null;
        }
        textBox.color = new Color(1, 1, 1, 0);
    }
    IEnumerator FadeOutText()
    {
        // loop over 1 second
        for (float i = theText.color.a; i >= 0; i -= Time.deltaTime*4)
        {
            // set color with i as alpha
            theText.color = new Color(1, 1, 1, i);
            yield return null;
        }
        theText.color = new Color(1, 1, 1, 0);
    }

    public void Show()
    {
        StopFadeIn = false;
        StartCoroutine(FadeInTextBox());
        StartCoroutine(FadeInText());
    }
    public void Hide()
    {
        StopFadeIn = true;
        StopCoroutine(FadeInTextBox());
        StopCoroutine(FadeInText());
        StartCoroutine(FadeOutTextBox());
        StartCoroutine(FadeOutText());
    }
}
