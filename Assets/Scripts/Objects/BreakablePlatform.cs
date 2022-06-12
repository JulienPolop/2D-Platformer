using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

public class BreakablePlatform : MonoBehaviour, ISpecialGround
{

    public GameObject DestroyParticles;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ISpecialGround.GroundInterraction(Player_Controller Player)
    {
        if (Player.velocityBeforePhysicsUpdate.y < - 10f)
        {
            Instantiate(DestroyParticles, GetComponent<SpriteRenderer>().bounds.center, DestroyParticles.transform.rotation);
            GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.1f, 0.25f);
            Destroy(this.gameObject);
        }
        else
        {
            Player.ResetJump();
        }
    }
}
