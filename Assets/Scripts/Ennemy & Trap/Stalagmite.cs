using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stalagmite : MonoBehaviour
{
    float InitialSpeed = 100;
    public enum StateEnum
    {
        Waiting,
        Falling,
    }

    bool TouchgroundAtStart = false;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;

        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if(!collider.isTrigger)
            {
                List<Collider2D> colliderCollideList = new List<Collider2D>();
                collider.OverlapCollider(new ContactFilter2D(), colliderCollideList);

                foreach (Collider2D collider2 in colliderCollideList)
                {
                    if (collider2.gameObject.layer == 10)
                    {
                        TouchgroundAtStart = true;
                    }
                }
            }
        }
    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Player.gameObject)
        {
            Debug.Log("CHUTE");
            GetComponent<Rigidbody2D>().gravityScale = 3;
            //GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, InitialSpeed);

            foreach (Collider2D collider in GetComponents<Collider2D>())
            {
                if (collider.isTrigger)
                {
                    collider.enabled = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if (TouchgroundAtStart)
                TouchgroundAtStart = false;
            else
            {
                Debug.Log("VRAI TOUCH");
                gameObject.layer = 15;
            }
        }
        else if (collision.gameObject == GameManager.Player.gameObject)
        {
            gameObject.layer = 15;
        }
        else if (collision.gameObject.GetComponent<Scr_Moving_Platform>() != null)
        {
            gameObject.layer = 15;
            transform.parent = collision.gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        this.gameObject.transform.parent = null;
    }

}
