using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    private System.Action CallbackIn;
    private System.Action CallbackOut;

    public bool IsFading;  //Modifié seulement par les animations

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn(System.Action callback)
    {
        CallbackIn = callback;
        GetComponent<Animator>().SetTrigger("FadeIn");
    }
    public void FadeOut(System.Action callback)
    {
        CallbackOut = callback;
        GetComponent<Animator>().SetTrigger("FadeOut");
    }
    public void OnFadeInComplete()
    {
        if (CallbackIn != null)
            CallbackIn();
    }
    public void OnFadeOutComplete()
    {
        if (CallbackOut != null)
            CallbackOut();
    }

}
