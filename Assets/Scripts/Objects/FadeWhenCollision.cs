using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class FadeWhenCollision : MonoBehaviour
{
    public enum FadeChoice
    {
        FadeIn,
        FadeOut,
    }
    public FadeChoice fadeChoice;

    private enum FadeState
    {
        Ready,
        Fading,
        Finish,
    }
    private FadeState fadeState = FadeState.Ready;
    public bool OnlyPlayer;

    IEnumerator FadeImage()
    {
        Tilemap Tilemap = GetComponent<Tilemap>();

        if (fadeState == FadeState.Ready)
        {
            fadeState = FadeState.Fading;
            // fade from opaque to transparent
            if (fadeChoice == FadeChoice.FadeOut)
            {
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

        fadeState = FadeState.Finish;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (OnlyPlayer && collision.gameObject == GameManager.Player.gameObject)
            StartCoroutine(FadeImage());
        else if (!OnlyPlayer)
            StartCoroutine(FadeImage());
    }
}
