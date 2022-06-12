using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakableWall : MonoBehaviour
{
    public int Life = 2;
    public GameObject HitParticles;
    public GameObject DestroyParticles;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.tag.Equals("Weapon"))
        //{
        //    Instantiate(HitParticles, other.transform.position, HitParticles.transform.rotation);

        //    Life--;
        //    if (Life == 0)
        //    {
        //        Destroy();
        //    }
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Weapon"))
        {
            Instantiate(HitParticles, collision.GetContact(0).point, HitParticles.transform.rotation);
            GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.1f, 0.25f);

            Life--;
            if (Life == 0)
            {
                Instantiate(DestroyParticles, GetComponent<SpriteRenderer>().bounds.center, DestroyParticles.transform.rotation);
                Destroy();
            }
        }
    }

    void Hit()
    {

    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
