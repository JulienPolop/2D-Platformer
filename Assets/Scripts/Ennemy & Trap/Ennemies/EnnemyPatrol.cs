using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyPatrol : MonoBehaviour {

    public float moveSpeed;
    public bool moveRight;

    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;
    private bool hittingWall;

    private bool notAtEdge;
    public Transform edge_check;

    private Vector3 StartScale;

    public GameObject KillParticles;
    public GameObject WalkParticles;

    // Use this for initialization
    void Start () {
        StartScale = transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {

        hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        notAtEdge = Physics2D.OverlapCircle(edge_check.position, wallCheckRadius, whatIsWall);
        if (hittingWall || !notAtEdge)
        {
            moveRight = !moveRight;
        }


		if (moveRight)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
            transform.localScale = new Vector3(-StartScale.x, StartScale.y, StartScale.z);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
            transform.localScale = new Vector3(StartScale.x, StartScale.y, StartScale.z);
        }

        if (GetComponent<Animator>().GetBool("IsDying") )
        {
            moveSpeed = 0;
            //GetComponent<Collider2D>().enabled = false;
            gameObject.layer = 15;
        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Weapon"))
        {
            GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.1f, 0.25f);
            GetComponent<Animator>().SetBool("IsDying", true);
            Instantiate(KillParticles, new Vector2(transform.position.x, transform.position.y + 0.52f), KillParticles.transform.rotation);
            WalkParticles.GetComponent<ParticleSystem>().Stop();
            WalkParticles.transform.parent = null;
            WalkParticles.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
