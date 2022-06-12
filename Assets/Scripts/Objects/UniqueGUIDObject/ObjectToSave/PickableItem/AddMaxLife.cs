using UnityEngine;
using Assets.Scripts;

public class AddMaxLife : PickableObject
{
    // Use this for initialization
    void Start()
    {
        Debug.Log("I'm ADD MAX LIFE And my GUID is: " + GuidString.ToString());
    }


    protected override void Effect()
    {
        GameManager.Player.GetComponent<Player_HealthManager>().MaxHealth++;
        GameManager.Player.GetComponent<Player_HealthManager>().PlayerHealth++;
    }

    protected override void Destroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        //foreach (Transform children in gameObject.GetComponentsInChildren<Transform>())
        //{
        //    children.gameObject.SetActive(false);
        //}

    }
}
