using UnityEngine;
using System.Collections;

public class RedBlob : MonoBehaviour
{
    public int life = 1;
    public enum StateEnum
    {
        Normal,
        Knockback,
    }
    StateEnum State = StateEnum.Normal;

    [Header("Movements")]
    public float moveSpeed;
    public bool LookRight;

    public float AttackCooldown = 2f;
    float AttackCooldownTime = 0f;
    private bool AttackInCD = false;
    private bool IsAttacking = false;

    [Header("Projectiles")]
    public GameObject Projectile;
    public int range;
    public float SpeedProjectile = 5f;
    public float TimeTillProjectileHit = 1f;
    public Transform SpitPosition;

    private Vector3 StartScale;
    public GameObject Player;

    [Header("Particles")]
    public GameObject DeathParticles;

    private float DistanceWithPlayer;
    Timer TimerKnockback;

    // Use this for initialization
    void Start()
    {
        StartScale = transform.localScale;
        //Player = FindObjectOfType<Player_Controller>().gameObject;
        Player = GameManager.Player.gameObject;

        TimerKnockback = gameObject.AddComponent<Timer>();
        TimerKnockback.SetUpTimer(0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (State == StateEnum.Normal)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);

            if (Player != null)
            {
                if ((Player.transform.position.x - transform.position.x) > 0)
                {
                    LookRight = true;
                    transform.localScale = new Vector3(-StartScale.x, StartScale.y, StartScale.z);
                }
                if ((Player.transform.position.x - transform.position.x) < 0)
                {
                    LookRight = false;
                    transform.localScale = new Vector3(StartScale.x, StartScale.y, StartScale.z);
                }

                CheckAttack();
            }

        }


        if (AttackInCD)
        {
            if (AttackCooldownTime <= 0)
                AttackInCD = false;
            if (AttackCooldownTime > 0)
                AttackCooldownTime -= Time.deltaTime;
        }

    }

    void CheckAttack()
    {
        DistanceWithPlayer = Vector2.Distance(new Vector2(Player.transform.position.x, Player.transform.position.y), new Vector2(transform.position.x, transform.position.y));
        if (DistanceWithPlayer <= range && !AttackInCD && !IsAttacking)
        {
            StartAttack();
        }
        else if (DistanceWithPlayer > range || AttackInCD)
        {
            StopAttack();
        }
    }
    void StartAttack()
    {
        IsAttacking = true;
        GetComponent<Animator>().SetBool("IsAttacking", true);
    }
    void SpitObject()  //Appelé dans l'animator
    {
       
        TimeTillProjectileHit = DistanceWithPlayer/ SpeedProjectile;

        // HOW TO ALWAYS TOUCH A TARGET WITH A BELL TRAJECTORY
        float xdistance = Player.transform.position.x - transform.position.x;
        float ydistance = Player.transform.position.y - transform.position.y;
        float throwAngle = Mathf.Atan((ydistance + 4.905f*(TimeTillProjectileHit * TimeTillProjectileHit)) / xdistance);

        float totalVelo = xdistance / (Mathf.Cos(throwAngle)* TimeTillProjectileHit);
        float xVelo, yVelo;
        xVelo = totalVelo * Mathf.Cos(throwAngle);
        yVelo = totalVelo * Mathf.Sin(throwAngle);


        GameObject ProjectileObject = Instantiate(Projectile, SpitPosition.position, SpitPosition.rotation);
        ProjectileObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelo, yVelo);
    }
    void OnAttackAnimationEnd() //Appelé dans l'animator
    {
        StopAttack();

        AttackCooldownTime = AttackCooldown;
        AttackInCD = true;
        IsAttacking = false;
    }
    void StopAttack()
    {
        GetComponent<Animator>().SetBool("IsAttacking", false);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Weapon"))
        {
            GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.1f, 0.25f);

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
            GetComponent<Rigidbody2D>().velocity = new Vector2(5, 1.7f);
        else if (GameManager.Player.LookDirection == Player_Controller.DirectionEnum.Left)
            GetComponent<Rigidbody2D>().velocity = new Vector2(-5, 1.7f);


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


    public void Die()
    {
        GetComponent<SpriteRenderer>().material.SetColor("_AdditionalColor", new Color(0f, 0f, 0f, 0));
        Instantiate(DeathParticles, new Vector2(transform.position.x, transform.position.y + 0.52f), DeathParticles.transform.rotation);
        Destroy(gameObject);

        //GetComponent<Animator>().SetBool("IsDying", true);
        //GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        //gameObject.layer = 15;
    }
    void DestroyObject()
    {
        Destroy(gameObject);
        Instantiate(DeathParticles, new Vector2(transform.position.x, transform.position.y + 0.52f), DeathParticles.transform.rotation);
    }













    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
    private void OnValidate()
    {
        if (LookRight && transform.localScale.x > 0)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
        else if (!LookRight && transform.localScale.x < 0)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
    }
}
