using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleUntilPlayerTouch : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            print("Collide Wall");
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
