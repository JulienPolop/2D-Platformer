using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class FoxyController : MonoBehaviour
{
    public float MoveSpeed;
    public HorizontalDirection LookDirection;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void March(HorizontalDirection moveDirection)
    {
        GetComponent<Animator>().SetBool("IsMarching", true);
        ChangeLookDirection(moveDirection);
        switch (moveDirection)
        {
            case HorizontalDirection.Right:
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                break;
            case HorizontalDirection.Left:
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                break;
        }
    }

    public void ChangeLookDirection(HorizontalDirection lookDirection)
    {
        switch (lookDirection)
        {
            case HorizontalDirection.Right:
                GetComponent<SpriteRenderer>().flipX = false;
                break;
            case HorizontalDirection.Left:
                GetComponent<SpriteRenderer>().flipX = true;
                break;
        }
    }

    public void Stop()
    {
        GetComponent<Animator>().SetBool("IsMarching", false);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
    }



    private void OnValidate()
    {
        ChangeLookDirection(LookDirection);
    }
}
