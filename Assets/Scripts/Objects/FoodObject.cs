using UnityEngine;
using System.Collections;

public class FoodObject : MonoBehaviour
{
    public GameObject HitParticle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<Player_HealthManager>().GetFood())
            {
                Instantiate(HitParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }

        }
    }
}
