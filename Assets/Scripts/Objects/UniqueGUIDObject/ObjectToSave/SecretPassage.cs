using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.Tilemaps;

public class SecretPassage : ObjectToSave
{
    public bool OnlyPlayer;
    private bool isDiscovered = false;
    private FadeChoice fadeChoice = FadeChoice.FadeOut;
    private FadeState fadeState = FadeState.Ready;

    private enum FadeChoice
    {
        FadeIn,
        FadeOut,
    }
    private enum FadeState
    {
        Ready,
        Fading,
        Finish,
    }

    public override SaveObjectBase GetSaveObject()
    {
        return new SaveObjectBase()
        {
            Activated = this.isDiscovered,
            GuidString = this.GuidString,
        };
    }

    public override void LoadFromSaveObject(SaveObjectBase saveObject)
    {
        Tilemap Tilemap = GetComponent<Tilemap>();
        if (saveObject.Activated)
        {
            Tilemap.color = new Color(Tilemap.color.r, Tilemap.color.g, Tilemap.color.b, 0);
            isDiscovered = true;
        }
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeSprite());
        fadeChoice = FadeChoice.FadeOut;
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeSprite());
        fadeChoice = FadeChoice.FadeIn;
    }

    IEnumerator FadeSprite()
    {
        Tilemap Tilemap = GetComponent<Tilemap>();

        if (fadeState == FadeState.Ready)
        {
            fadeState = FadeState.Fading;
            // fade from opaque to transparent
            if (fadeChoice == FadeChoice.FadeOut)
            {
                isDiscovered = true;
                // loop over 1 second backwards
                for (float i = 1; i >= 0; i -= Time.deltaTime)
                {
                    // set color with i as alpha
                    Tilemap.color = new Color(Tilemap.color.r, Tilemap.color.g, Tilemap.color.b, i);
                    yield return null;
                }
            }
            // fade from transparent to opaque
            else
            {
                // loop over 1 second
                for (float i = 0; i <= 1; i += Time.deltaTime)
                {
                    // set color with i as alpha
                    Tilemap.color = new Color(Tilemap.color.r, Tilemap.color.g, Tilemap.color.b, i);
                    yield return null;
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (OnlyPlayer && collision.gameObject == GameManager.Player.gameObject && !isDiscovered)
            StartFadeOut();
        else if (!OnlyPlayer && !isDiscovered)
            StartFadeOut();
    }
}
