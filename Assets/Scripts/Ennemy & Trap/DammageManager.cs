using UnityEngine;
using UnityEditor;

public class DammageManager : MonoBehaviour
{
    [Header("Dammages")]
    public int dammageOnHit;
    public bool onTrigger = true;

    [Header("Knockback")]
    public int pushForceX;
    public int pushForceY;
    [Tooltip("Put -1 or negative value to use the standard player Knockback Time")]
    public float timerKnockback;

    public enum PushDirectionMode
    {
        PushDiretionDependOfPositionOfTheTwoObjects,
        PushDirectionDependOnTheVelocity,
    }
    public PushDirectionMode pushDirectionMode;

    public enum DammageManagerEnum
    {
        KillPlayer,
        DrownPlayer,
        HitPlayer,
        Nothing,
    }
    public DammageManagerEnum actionOnContact;


    public void KillPlayer(GameObject playerGameObject)
    {
        playerGameObject.GetComponent<Player_DeathManager>().Kill();
    }
    public void HitPlayer(GameObject playerGameObject, Vector3 objectPosition)
    {
        playerGameObject.GetComponent<Player_DeathManager>().Hit(dammageOnHit);
        Player_Controller.DirectionEnum directionPush;


        if (playerGameObject.transform.position.x < objectPosition.x)
            directionPush = Player_Controller.DirectionEnum.Left;
        else
            directionPush = Player_Controller.DirectionEnum.Right;


        if (pushDirectionMode == PushDirectionMode.PushDirectionDependOnTheVelocity)
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
                if (GetComponent<Rigidbody2D>().velocity.x < 0)
                    directionPush = Player_Controller.DirectionEnum.Left;
                else if (GetComponent<Rigidbody2D>().velocity.x > 0)
                    directionPush = Player_Controller.DirectionEnum.Right;
            }
        }


        if (timerKnockback >= 0)
            playerGameObject.GetComponent<Player_Controller>().BeginKnockback(pushForceX, pushForceY, directionPush, timerKnockback);
        else
            playerGameObject.GetComponent<Player_Controller>().BeginKnockback(pushForceX, pushForceY, directionPush);
    }
    public void DrownPlayer(GameObject playerGameObject)
    {
        playerGameObject.GetComponent<Player_DeathManager>().Drown();
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (actionOnContact == DammageManagerEnum.KillPlayer)
                KillPlayer(other.gameObject);
            if (actionOnContact == DammageManagerEnum.DrownPlayer )
                DrownPlayer(other.gameObject);
            if (actionOnContact == DammageManagerEnum.HitPlayer && !other.gameObject.GetComponent<Player_Controller>().isInvincible)
                HitPlayer(other.gameObject, transform.position);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (actionOnContact == DammageManagerEnum.KillPlayer)
                KillPlayer(other.gameObject);
            if (actionOnContact == DammageManagerEnum.DrownPlayer)
                DrownPlayer(other.gameObject);
            if (actionOnContact == DammageManagerEnum.HitPlayer && !other.gameObject.GetComponent<Player_Controller>().isInvincible)
                HitPlayer(other.gameObject, transform.position);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (onTrigger && other.gameObject.tag.Equals("Player"))
        {
            if (actionOnContact == DammageManagerEnum.KillPlayer)
                KillPlayer(other.gameObject);
            if (actionOnContact == DammageManagerEnum.DrownPlayer)
                DrownPlayer(other.gameObject);
            if (actionOnContact == DammageManagerEnum.HitPlayer && !other.gameObject.GetComponent<Player_Controller>().isInvincible)
                HitPlayer(other.gameObject, transform.position);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (onTrigger && other.gameObject.tag.Equals("Player"))
        {
            if (actionOnContact == DammageManagerEnum.KillPlayer)
                KillPlayer(other.gameObject);
            if (actionOnContact == DammageManagerEnum.DrownPlayer)
                DrownPlayer(other.gameObject);
            if (actionOnContact == DammageManagerEnum.HitPlayer && !other.gameObject.GetComponent<Player_Controller>().isInvincible)
                HitPlayer(other.gameObject, transform.position);
        }
    }
}