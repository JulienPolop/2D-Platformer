using UnityEngine;
using System.Collections;

public class OutlineWhenPlayerNear : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GetComponent<SpriteRenderer>().material.shader = Shader.Find("Sprites/Default");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().material.shader = Shader.Find("Custom/Sprite Outline");
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().material.shader = Shader.Find("Sprites/Default");
        }
    }
}
