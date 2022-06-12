using UnityEngine;
using System.Collections;

public class Staglatites_Minotaure : MonoBehaviour
{
    public GameObject destructionParticles;

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -5);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy();
    }

    void Destroy()
    {
        Instantiate(destructionParticles, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
