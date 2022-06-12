using UnityEngine;
using System.Collections;

public class RedBlobProjectile : MonoBehaviour
{
    public GameObject DestroyParticles;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("OneWay Platform") && other.gameObject.layer != LayerMask.NameToLayer("Ennemy") && !other.isTrigger)
        {
            Instantiate(DestroyParticles, transform.position, DestroyParticles.transform.rotation);
            Destroy(gameObject);
        }
    }

}
