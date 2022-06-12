using UnityEngine;
using UnityEditor;

public class MinautorWall : MonoBehaviour
{
    public int Health = 3;
    public GameObject HitParticles;
    public GameObject DestroyParticles;
    public bool canBeDestroyed;
    [Header("Pop Stalagtite?")]
    public BoxCollider2D zonePopStalactites;
    public GameObject StalagtitePrefab;

    public void Hit()
    {
        if ((!canBeDestroyed || Health > 1) && zonePopStalactites != null)
        {
            SpawnStalagtites();
        }
        if (canBeDestroyed)
        {
            GameObject Particle = Instantiate(HitParticles, transform);
            Particle.transform.parent = null;
            Health--;
            CheckDestroy();
        }
    }

    private void CheckDestroy()
    {
        if (Health <= 0)
        {
            Instantiate(DestroyParticles, transform.position, DestroyParticles.transform.rotation);

            Destroy(this.gameObject);
        }
    }

    void SpawnStalagtites()
    {
        Debug.Log("TIMBER!");
        Instantiate(StalagtitePrefab, new Vector3(Random.Range(zonePopStalactites.bounds.min.x, zonePopStalactites.bounds.max.x), Random.Range(zonePopStalactites.bounds.min.y, zonePopStalactites.bounds.max.y), 0), zonePopStalactites.transform.rotation);
        Instantiate(StalagtitePrefab, new Vector3(Random.Range(zonePopStalactites.bounds.min.x, zonePopStalactites.bounds.max.x), Random.Range(zonePopStalactites.bounds.min.y, zonePopStalactites.bounds.max.y), 0), zonePopStalactites.transform.rotation);
        Instantiate(StalagtitePrefab, new Vector3(Random.Range(zonePopStalactites.bounds.min.x, zonePopStalactites.bounds.max.x), Random.Range(zonePopStalactites.bounds.min.y, zonePopStalactites.bounds.max.y), 0), zonePopStalactites.transform.rotation);
        Instantiate(StalagtitePrefab, new Vector3(Random.Range(zonePopStalactites.bounds.min.x, zonePopStalactites.bounds.max.x), Random.Range(zonePopStalactites.bounds.min.y, zonePopStalactites.bounds.max.y), 0), zonePopStalactites.transform.rotation);

    }
}