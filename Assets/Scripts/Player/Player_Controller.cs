using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Unlocked")]
    public bool AttackUnlocked;
    public bool DoubleJumpUnlocked;
    public bool DashUnlocked;

    [Header("Movments")]
    bool isRunning;
    public float normalMoveSpeed;
    public float Move_speed;
    public float Force_Jump;
    public DirectionEnum MoveDirection;
    public DirectionEnum LookDirection;
    Vector2 Inertie;
    Vector2 ForceOnPlatform;
    float Input_Mouvement_horizontal
    {
        get
        {
            return Input.GetAxisRaw("Horizontal");
        }
    }
    float originalGravityScale = 0;
    public Vector2 velocityBeforePhysicsUpdate;
    float MaxFallSpeed = 20;

    [Header("Dash")]
    public bool isDashing;
    public bool canDash = true;
    DirectionEnum DirectionDash;
    public float dashSpeed;
    public float dashDuration = 1f;
    float dashDurationTime = 0f;
    public float dashCooldown = 2f;
    float dashCooldownTime = 0f;

    [Header("States")]
    public StateEnum State = StateEnum.Normal;

    [Header("Jumps")]
    public bool CanNormalJump = true;
    public bool CanDoublelJump = true;

    [Header("Attack")]
    public bool isAttacking;
    public bool canAttack = true;
    public float attackCooldown = 2f;
    float attackCooldownTime = 0f;

    [Header("Grounded")]
    public bool IsGrounded = false;

    [Header("Pass One Way Platform")]
    public bool PassthrougOneWayPlat = false;
    public float NoCollisionToOneWayPlat_time = 1;
    private float NoCollisionToOneWayPlat_Timer;

    [Header("Invicibility")]
    public bool isInvincible = false;
    public float TimeInvincible = 1f;
    Timer TimerInvicible;
    //private float TimerInvincible;

    [Header("Knockback")]
    private float TimeKnockback = 0.5f;
    private float TimerKnockback;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    [Header("Particles")]
    public GameObject FallParticles;
    public GameObject WalkParticles;
    public GameObject JumpParticles;

    [Header("Camera")]
    public GameObject TargetCamera;
    public float TargetCameraOffsetX;
    public float TargetCameraOffsetY;

    public enum StateEnum
    {
        Normal,
        Knockback,
        Pause,
        Death,
        Speaking,
    }

    public enum DirectionEnum
    {
        Left,
        Right,
    }
    public struct SideColisionInfoStruct
    {
        public bool Haut { get; set; }
        public bool Bas { get; set; }
        public bool Gauche { get; set; }
        public bool Droite { get; set; }
    }

    //=====================================================================================================================//
    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        Move_speed = normalMoveSpeed;

        ChangeLookDirection(MoveDirection);

        Inertie = new Vector3(0, 0, 0);

        var mouvement_horizontal = Input.GetAxisRaw("Horizontal");

        isAttacking = false;

        TimerInvicible = gameObject.AddComponent<Timer>();


        Debug.Log("START!!! "+UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " " + Input.GetAxisRaw("Horizontal"));

        originalGravityScale = myRigidbody.gravityScale;
    }

    void FixedUpdate()
    {
        if (GameManager.IsPaused == false)
        {
            if (State == StateEnum.Normal)
            {
                MoveTargetCamera();
                RenderSprite();
            }
        }
        velocityBeforePhysicsUpdate = myRigidbody.velocity;
    }


    private void Update()
    {

        //Debug.Log(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " " + Input.GetAxisRaw("Horizontal"));
        if (GameManager.IsPaused == false)
        {
            if (State == StateEnum.Normal)
            {
                Move();
                Check_Direction();
                CheckJump();
                Check_Down();
                CheckAttack();
                CheckDash();
                CheckHeal();

                CoolDownAttack();
                DurationDash();
                CooldownDash();

                //if (isInvincible)
                //CheckInvincible();

            }
            if (State == StateEnum.Pause)
            {

            }
            if (State == StateEnum.Death)
            {
                MoveTargetCamera();
            }
            if (State == StateEnum.Speaking)
            {
                myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
                RenderSprite();
                myAnimator.SetBool("IsRunning", false);
                MoveTargetCamera();
            }
            if (State == StateEnum.Knockback)
            {
                RenderSprite();
                MoveTargetCamera();

                CoolDownAttack();
                CheckKnockback();
            }
        }

        if (myRigidbody.velocity.y <= -MaxFallSpeed)
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, -MaxFallSpeed);
    }
    //=====================================================================================================================//s
    void Check_Direction()
    {
        //Change MoveDirection pour changer l'emplacement de la caméra
        if (myRigidbody.velocity.x > 0 && MoveDirection != DirectionEnum.Right)
        {
            MoveDirection = DirectionEnum.Right;
        }
        if (myRigidbody.velocity.x < 0 && MoveDirection != DirectionEnum.Left)
        {
            MoveDirection = DirectionEnum.Left;
        }

        //Change MoveX de l'animator pour changer le sprite
        if (!myAnimator.GetBool("IsAttacking") && MoveDirection != LookDirection && !isDashing)
        {
            ChangeLookDirection(MoveDirection);
        }
    }

    void ChangeLookDirection(DirectionEnum newLookDirection)
    {
        if (newLookDirection == DirectionEnum.Right)
        {
            myAnimator.SetFloat("MoveX", 1);
            LookDirection = DirectionEnum.Right;
        }
        else if (newLookDirection == DirectionEnum.Left)
        {
            myAnimator.SetFloat("MoveX", -1);
            LookDirection = DirectionEnum.Left;
        }
    }

    //=====================================================================================================================//
    void Move()
    {
        //Ecoute les Inputs


        //Bouge le player
        float inertieX = Inertie.x;
        float ForceOnPlatformX = ForceOnPlatform.x;

        if (Input_Mouvement_horizontal == 0)
        {
            ForceOnPlatformX = 0;
        }
        else
        {
            inertieX /= 2;
            ForceOnPlatformX /= 2;
            if ( (inertieX > 0 && Input_Mouvement_horizontal < 0) || (inertieX < 0 && Input_Mouvement_horizontal > 0))
            {
                Inertie.x = 0;
            }
        }

        if (isDashing)
        {
            if (DirectionDash == DirectionEnum.Left)
                myRigidbody.velocity = new Vector2((-dashSpeed), 0);
            if (DirectionDash == DirectionEnum.Right)
                myRigidbody.velocity = new Vector2((dashSpeed), 0);
        }
        else
        {
            myRigidbody.velocity = new Vector2((Input_Mouvement_horizontal * Move_speed) + inertieX + ForceOnPlatformX, myRigidbody.velocity.y);
        }
    }
    //=====================================================================================================================//
    void CheckJump()
    {
        if (Input.GetButtonDown("Jump") && !isDashing)
        {
            if (CanNormalJump)
            {
                CanNormalJump = false;
                Jump();
            }
            else if (DoubleJumpUnlocked && CanDoublelJump)
            {
                CanDoublelJump = false;
                Jump();
            }

        }

        if (Input.GetButtonUp("Jump") && myRigidbody.velocity.y > 0)
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y * 0.6f);
    }
    
    void Jump()
    {
        PassthrougOneWayPlat = false;
        Physics2D.IgnoreLayerCollision(12, 13, false);
        //myAnimator.SetBool("IsJumping", true);
        Instantiate(JumpParticles, transform);
        Instantiate(FallParticles, new Vector2(transform.position.x, GetComponent<BoxCollider2D>().bounds.min.y), FallParticles.transform.rotation);
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Force_Jump);
        if (myRigidbody.velocity.y == 0)
        {
            Debug.Log("COMPREND PAS");
        }
    }
    //=====================================================================================================================//
    void DurationDash()
    {
        if (isDashing)
        {
            if (dashDurationTime <= 0)
                EndDash();
            if (dashDurationTime > 0)
                dashDurationTime -= Time.deltaTime;
        }
    }
    void CooldownDash()
    {
        if (!canDash)
        {
            if (dashCooldownTime <= 0)
                canDash = true;
            if (dashCooldownTime > 0)
                dashCooldownTime -= Time.deltaTime;
        }
    }



    void CheckDash()
    {
        if (Input.GetButtonDown("Dash") && canDash)
        {
            Dash();
        }
    }

    void Dash()
    {
        GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.2f, 0.1f);
        dashCooldownTime = dashCooldown;
        dashDurationTime = dashDuration;
        DirectionDash = MoveDirection;
        isDashing = true;
        canDash = false;

        if (isAttacking)
            EndAttack();

        Instantiate(JumpParticles, transform);
    }
    void EndDash()
    {
        isDashing = false;
    }
    //=====================================================================================================================//
    void Check_Down()
    {
        //Modifier tout ca!

        if (Input.GetButtonDown("Down") && !PassthrougOneWayPlat && IsGrounded)
        {
            Physics2D.IgnoreLayerCollision(12, 13, true);
            PassthrougOneWayPlat = true;
            NoCollisionToOneWayPlat_Timer = NoCollisionToOneWayPlat_time;
        }
        if (PassthrougOneWayPlat)
        {
            NoCollisionToOneWayPlat_Timer -= Time.deltaTime;
            if (NoCollisionToOneWayPlat_Timer < 0)
            {
                PassthrougOneWayPlat = false;
                Physics2D.IgnoreLayerCollision(12, 13, false);
            }
        }
    }
    //=====================================================================================================================//
    void CoolDownAttack()
    {
        if(!canAttack)
        {
            if (attackCooldownTime <= 0)
                canAttack = true; ;
            if (attackCooldownTime > 0)
                attackCooldownTime -= Time.deltaTime;
        }
    }

    void CheckAttack()
    {
        if (Input.GetButton("Fire1") && AttackUnlocked && canAttack && !isDashing)
        {
            myAnimator.SetBool("IsAttacking", true);
            isAttacking = true;
            canAttack = false;
            attackCooldownTime = 1000f;
        }
    }

    //Est appelée durant l'animation d'attack
    void EndAttack()
    {
        attackCooldownTime = attackCooldown;
        myAnimator.SetBool("IsAttacking", false);
        isAttacking = false;
    }

    //=====================================================================================================================//
    void CheckHeal()
    {
        if (Input.GetButtonDown("Heal") && Input_Mouvement_horizontal == 0)
        {
            if (IsGrounded && !isAttacking && !isDashing && !isInvincible)
                GetComponent<Player_HealthManager>().HealFromFood();
        }
    }
    //=====================================================================================================================//

    void RenderSprite()
    {
        if (State == StateEnum.Normal && IsGrounded)
        {
           //myAnimator.SetBool("IsJumping", false);
            //Si On est arreté
            if (Input_Mouvement_horizontal == 0)
            {
                myAnimator.SetBool("IsRunning", false);
            }
            else
            {
                myAnimator.SetBool("IsRunning", true);
            }
        }

        if (isDashing)
        {
            myAnimator.SetBool("IsDashing", true);
        }
        else
        {
            myAnimator.SetBool("IsDashing", false);
            //Pour les sprites de saut et de chute
            if (IsGrounded)
            {
                myAnimator.SetBool("IsJumping", false);
                myAnimator.SetBool("IsFalling", false);
            }
            else
            {
                if (myRigidbody.velocity.y > 0)
                {
                    myAnimator.SetBool("IsJumping", true);
                    myAnimator.SetBool("IsFalling", false);
                }
                if (myRigidbody.velocity.y <= 0)
                {
                    myAnimator.SetBool("IsJumping", false);
                    myAnimator.SetBool("IsFalling", true);
                }
            }
        }



        if (myAnimator.GetBool("IsRunning") && !myAnimator.GetBool("IsFalling") && !myAnimator.GetBool("IsJumping"))
        {
            WalkParticles.GetComponent<ParticleSystem>().Play();
        }
        else if ((!myAnimator.GetBool("IsRunning") || myAnimator.GetBool("IsFalling") || myAnimator.GetBool("IsJumping")) && WalkParticles.activeInHierarchy)
        {
            WalkParticles.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void MoveTargetCamera()
    {
        if (MoveDirection == DirectionEnum.Right)
        {
            TargetCamera.transform.position = new Vector3(transform.position.x + TargetCameraOffsetX, transform.position.y + TargetCameraOffsetY, TargetCamera.transform.position.y);
        }
        else if (MoveDirection == DirectionEnum.Left)
        {
            TargetCamera.transform.position = new Vector3(transform.position.x - TargetCameraOffsetX, transform.position.y + TargetCameraOffsetY, TargetCamera.transform.position.y);
        }
    }
    //=====================================================================================================================//
    //=====================================================================================================================//
    //=====================================================================================================================//



    void ChangeState(StateEnum newState)
    {
        if (GameManager.IsPaused == false)
        {
            if (newState == StateEnum.Normal)
            {

            }
            if (newState == StateEnum.Pause)
            {

            }
            if (newState == StateEnum.Death)
            {

            }
            if (newState == StateEnum.Speaking)
            {

            }
        }
    }
    //=============================================================================================================
    public void BeginInvicible()
    {
        isInvincible = true;

        //TimerInvincible = TimeInvincible;
        TimerInvicible.SetUpTimer(TimeInvincible);
        TimerInvicible.TimerEndEvent += new System.EventHandler(this.EndInvincible);
        TimerInvicible.StartCounter();

        
        Physics2D.IgnoreLayerCollision(12, 17, true);
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.5f);
    }
    //void CheckInvincible()
    //{
    //    TimerInvincible -= Time.deltaTime;
    //    if (TimerInvincible <= 0)
    //    {
    //        EndInvincible();
    //    }
    //}
    public void EndInvincible(object sender, System.EventArgs e)
    {
        EndInvincible();
    }
    public void EndInvincible()
    {
        isInvincible = false;
        TimerInvicible.TimerEndEvent -= new System.EventHandler(this.EndInvincible);

        Physics2D.IgnoreLayerCollision(12, 17, false);
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1);
    }

    //=============================================================================================================
    public void BeginKnockback(float PushForceX, float PushForceY, DirectionEnum Direction)
    {
        BeginKnockback(PushForceX, PushForceY, Direction, TimeKnockback);
    }

    public void BeginKnockback(float PushForceX, float PushForceY, DirectionEnum Direction, float timeKnockBack)
    {
        GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.2f, 0.3f);
        myAnimator.SetBool("IsKnockBack", true);

        State = StateEnum.Knockback;
        TimerKnockback = timeKnockBack;

        BeginInvicible();

        Vector2 Direction_to_Push = new Vector2(0, 0);
        GetComponent<Player_Controller>().State = Player_Controller.StateEnum.Knockback;
        if (Direction == DirectionEnum.Left)
            Direction_to_Push.x = -PushForceX;
        if (Direction == DirectionEnum.Right)
            Direction_to_Push.x = PushForceX;

        Direction_to_Push.y = PushForceY;


        //GetComponent<Rigidbody2D>().AddForce(Direction_to_Push * Push_Force);
        //GetComponent<Rigidbody2D>().velocity = (Direction_to_Push * Push_Force);
        GetComponent<Rigidbody2D>().velocity = (Direction_to_Push);

        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);

        if (isAttacking)
            EndAttack();
        if (isDashing)
            EndDash();

    }
    void CheckKnockback()
    {
        TimerKnockback -= Time.deltaTime;
        if (TimerKnockback <= 0)
        {
            EndKnockback();
        }
    }
    public void EndKnockback()
    {
        State = StateEnum.Normal;
        myAnimator.SetBool("IsKnockBack", false);

        if (GetComponent<Player_HealthManager>().PlayerHealth <= 0)
        {
            GetComponent<Player_Controller>().EndInvincible();
            GetComponent<Player_DeathManager>().Kill();
        }
        else
        {
            GetComponent<Player_Controller>().State = Player_Controller.StateEnum.Normal;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, GetComponent<SpriteRenderer>().color.a);
            GetComponent<Player_Controller>().BeginInvicible();
        }

    }


    //=====================================================================================================================//
    //=====================================================================================================================//
    //=====================================================================================================================//

    SideColisionInfoStruct FindSideCollision(ContactPoint2D[] contacts)
    {

        bool Collide_droit = false;
        bool Collide_gauche = false;
        bool Collide_haut = false;
        bool Collide_bas = false;

        if (contacts.Length > 1)
        {
            for (int x = 0; x < contacts.Length; x++)
            {
                for (int y = x + 1; y < contacts.Length; y++)
                {

                    //A plat
                    if (contacts[x].point.x == contacts[y].point.x)
                    {
                        // A droite
                        Collide_droit = (contacts[x].point.x >= GetComponent<BoxCollider2D>().bounds.max.x);
                        // A Gauche
                        Collide_gauche = (contacts[x].point.x <= GetComponent<BoxCollider2D>().bounds.min.x);
                    }
                    //Sur un côté
                    if (Math.Round(contacts[x].point.y, 2) == Math.Round(contacts[y].point.y, 2) && (Math.Round(contacts[x].point.x, 2) != Math.Round(contacts[y].point.x, 2)))
                    {
                        // Haut
                        Collide_haut = (contacts[x].point.y >= GetComponent<BoxCollider2D>().bounds.max.y);
                        //Bas
                        Collide_bas = (contacts[x].point.y <= GetComponent<BoxCollider2D>().bounds.min.y);
                    }

                }
            }

        }

        if (contacts.Length == 1)
        {
            //Quand on a 1 Contact Point, Donc on collide un truc en Diagonal ou autre chose 
            Vector2 Contact = contacts[0].point;
            if (Contact.x < GetComponent<BoxCollider2D>().bounds.max.x && Contact.x > GetComponent<BoxCollider2D>().bounds.min.x && Contact.y < GetComponent<BoxCollider2D>().bounds.center.y)
            {
                // Haut
                Collide_haut = (contacts[0].point.y > transform.position.y);
                //Bas
                Collide_bas = (contacts[0].point.y < transform.position.y);
            }
            if (contacts[0].point.y < GetComponent<BoxCollider2D>().bounds.max.y && contacts[0].point.y > GetComponent<BoxCollider2D>().bounds.min.y)
            {
                // A droite
                Collide_droit = (contacts[0].point.x > transform.position.x);
                // A Gauche
                Collide_gauche = (contacts[0].point.x < transform.position.x);
            }
        }

        SideColisionInfoStruct sideColisionInformation = new SideColisionInfoStruct();
        sideColisionInformation.Haut = Collide_haut;
        sideColisionInformation.Bas = Collide_bas;
        sideColisionInformation.Gauche = Collide_gauche;
        sideColisionInformation.Droite = Collide_droit;

        return sideColisionInformation;
    }

    //When Collide
    void OnCollisionStay2D(Collision2D other)
    {
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        GetComponent<BoxCollider2D>().GetContacts(contacts);
        SideColisionInfoStruct sideCollisionInfo = FindSideCollision(contacts.ToArray());

        //SideColisionInfoStruct sideCollisionInfo = FindSideCollision(other.contacts);

        CheckGrounded(sideCollisionInfo, other);
        CheckSide(sideCollisionInfo, other);

        if (sideCollisionInfo.Bas && IsGrounded)
        {
            if (other.gameObject.GetComponent<Scr_Moving_Platform>() != null)
            {
                ForceOnPlatform = -other.gameObject.GetComponent<Scr_Moving_Platform>().velocity;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        GetComponent<BoxCollider2D>().GetContacts(contacts);
        SideColisionInfoStruct sideCollisionInfo = FindSideCollision(contacts.ToArray());
        //SideColisionInfoStruct sideCollisionInfo = FindSideCollision(other.contacts);

        CheckGrounded(sideCollisionInfo, other);
        CheckSide(sideCollisionInfo, other);
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        GetComponent<BoxCollider2D>().GetContacts(contacts);
        SideColisionInfoStruct sideCollisionInfo = FindSideCollision(contacts.ToArray());


        if (!sideCollisionInfo.Bas && IsGrounded)
            EndGrounded(collision.gameObject);

        if (collision.gameObject.GetComponent<Scr_Moving_Platform>() != null)
        {
            transform.parent = null;
        }
    }

    //=====================================================================================================================//

    void CheckGrounded(SideColisionInfoStruct sideCollisionInfo, Collision2D other)
    {
        if (sideCollisionInfo.Bas && IsGrounded == false)
            BeginGrounded(other.gameObject);
        else if (!sideCollisionInfo.Bas && IsGrounded == true)
            EndGrounded(other.gameObject);
    }
    void CheckSide(SideColisionInfoStruct sideCollisionInfo, Collision2D other)
    {
        if (sideCollisionInfo.Gauche && Inertie.x < 0)
            Inertie.x = 0;
        else if (sideCollisionInfo.Droite && Inertie.x > 0)
            Inertie.x = 0;
    }

    void BeginGrounded(GameObject groundObject)
    {
        if (velocityBeforePhysicsUpdate.y == -MaxFallSpeed)
        {
            //GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.3f, 0.2f);
        }
        if (myRigidbody.velocity.y <= 0.001f)
        {
            IsGrounded = true;
            Inertie = Vector2.zero;

            ISpecialGround specialGround = groundObject.GetComponent<ISpecialGround>();
            if (groundObject.tag != "Ennemy" && specialGround == null)
            {
                ResetJump();
            }
            else if (specialGround != null)
            {
                specialGround.GroundInterraction(this);
            }
        }
    }

    void EndGrounded(GameObject groundObject)
    {
        IsGrounded = false;

        if (groundObject.GetComponent<Scr_Moving_Platform>() != null)
        {
            transform.parent = null;
            Inertie = groundObject.GetComponent<Scr_Moving_Platform>().velocity;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y + Inertie.y / 2);
            ForceOnPlatform = Vector2.zero;
        }
    }

    public void ResetJump()
    {
        CanNormalJump = true;
        CanDoublelJump = true;
        Instantiate(FallParticles, new Vector2(transform.position.x, GetComponent<BoxCollider2D>().bounds.min.y), FallParticles.transform.rotation);
    }

    //=====================================================================================================================//
    //=====================================================================================================================//
    //=====================================================================================================================//

}
