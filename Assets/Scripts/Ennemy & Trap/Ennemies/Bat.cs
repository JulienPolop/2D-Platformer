using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class Bat : MonoBehaviour
{
    [Header("Movements")]
    float moveSpeed;
    Vector2 targetDirection;

    public float maxMoveSpeed;
    public float accelerationToMoveSpeed;

    public int rangeDetection;

    public bool LookRight;
    public bool isFollowing;

    [Header("Stats")]
    public int life;


    [Header("Particles")]
    public GameObject hitParticles;
    public GameObject deathParticles;

    GameObject Player;
    Timer TimerKnockback;

    public enum StateEnum
    {
        Normal,
        Knockback,
    }
    StateEnum State = StateEnum.Normal;

    // Use this for initialization
    void Start()
    {
        isFollowing = false;
        Player = GameManager.Player.gameObject;
        moveSpeed = 0;

        TimerKnockback = gameObject.AddComponent<Timer>();
        TimerKnockback.SetUpTimer(0.3f);

    }

    #region OnUpdate
    // Update is called once per frame
    void Update()
    {
        if (State == StateEnum.Normal)
        {
            CheckDistance();

            if (isFollowing)
            {
                MoveTowardPlayer();
            }
            else
            {
                Stop();
            }
            CheckDirection();
        }

    }

    void MoveTowardPlayer()
    {
        moveSpeed += accelerationToMoveSpeed;
        if (moveSpeed > maxMoveSpeed)
        {
            moveSpeed = maxMoveSpeed;
        }

        targetDirection = (Player.transform.position - transform.position);
        GetComponent<Rigidbody2D>().velocity = targetDirection.normalized * moveSpeed;

        //targetDirection = (Player.transform.position - transform.position).normalized;
        //float targetAngle = AngleHelper.AngleFromDir(targetDirection);
        //float angleVelocity = AngleHelper.AngleFromDir(GetComponent<Rigidbody2D>().velocity.normalized);

        //Debug.Log(targetDirection +" "+ AngleHelper.DirFromAngle(AngleHelper.AngleFromDir(targetDirection)));


    }
    void Stop()
    {
        moveSpeed -= accelerationToMoveSpeed;
        if (moveSpeed < 0)
        {
            moveSpeed = 0;
        }
        GetComponent<Rigidbody2D>().velocity = targetDirection.normalized * moveSpeed;
    }

    void CheckDistance()
    {
        float distanceWithPlayer = Vector2.Distance(Player.transform.position, transform.position);
        if (distanceWithPlayer <= rangeDetection && !isFollowing && Player.GetComponent<Player_Controller>().State != Player_Controller.StateEnum.Death)
        {
            isFollowing = true;
        }
        if ((distanceWithPlayer > rangeDetection || Player.GetComponent<Player_Controller>().State == Player_Controller.StateEnum.Death) && isFollowing)
        {
            isFollowing = false;
        }
    }
    void CheckDirection()
    {
        if (GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            LookRight = true;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            LookRight = false;
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    #endregion OnUpdate

    #region OnEditor
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rangeDetection);
    }
    private void OnValidate()
    {
        if (LookRight)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;
    }
    #endregion OnEditor

    #region Collision
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Weapon"))
        {
            GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.1f, 0.25f);
            Instantiate(hitParticles, transform.position, hitParticles.transform.rotation);


            life--;
            if (life <= 0)
                TimerKnockback.OnEndTimerCallback = Die;
            else
                TimerKnockback.OnEndTimerCallback = EndKnockBack;

            StartKnockback();
        }
    }

    void StartKnockback()
    {
        State = StateEnum.Knockback;
        if (GameManager.Player.LookDirection == Player_Controller.DirectionEnum.Right)
            GetComponent<Rigidbody2D>().velocity = new Vector2(10, -GetComponent<Rigidbody2D>().velocity.y);
        else if (GameManager.Player.LookDirection == Player_Controller.DirectionEnum.Left)
            GetComponent<Rigidbody2D>().velocity = new Vector2(-10, -GetComponent<Rigidbody2D>().velocity.y);


        TimerKnockback.StartCounter();
        Material Material = GetComponent<SpriteRenderer>().material;

        Material.color = Color.white;

        Material.SetColor("_AdditionalColor", new Color(0.5f, 0.5f, 0.5f, 0));


    }
    void EndKnockBack()
    {
        GetComponent<SpriteRenderer>().material.SetColor("_AdditionalColor", new Color(0f, 0f, 0f, 0));
        State = StateEnum.Normal;
        TimerKnockback.StopCounter();
        moveSpeed = 0;
    }

    void Die()
    {
        Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);
        Destroy(this.gameObject);
    }
    #endregion
}
