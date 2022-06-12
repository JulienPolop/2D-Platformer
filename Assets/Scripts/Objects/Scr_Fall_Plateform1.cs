using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Fall_Plateform1 : MonoBehaviour
{
    public float fallDelay = 2.0f;
    public GameObject Fall_With;
    public bool Tombe = true;
    public bool SeDetruit = false;
    private Vector3 startposition;
    private Quaternion startrotation;

    public bool isRespawning = false;
    public float Respawn_time = 4;
    private float Timer_Respawn;

    void Start()
    {
        startposition = transform.position;
        startrotation = transform.rotation;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (isRespawning)
        {
            Timer_Respawn -= Time.deltaTime;
            if (Timer_Respawn < 0)
            {
                isRespawning = false;
                Respawn();
            }
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Fall_With || other.gameObject.tag == "Player")
        {
            if (Tombe)
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
            if (SeDetruit)
            {
                Destruction();
            }
        }
        if (other.gameObject.tag == "Trigger Bot")
        {
            Destruction();
        }
    }

    void Destruction()
    {
        Timer_Respawn = Respawn_time;
        isRespawning = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

    }
    void Respawn()
    {
        isRespawning = false;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        transform.position = startposition;
        transform.rotation = startrotation;
    }



}
