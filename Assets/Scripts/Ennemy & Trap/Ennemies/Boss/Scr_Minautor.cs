using Assets.Scripts.Helpers;
using UnityEngine;

public class Scr_Minautor : MonoBehaviour {

    //private int State = 0;
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    public GameObject Player;
    public int Move_speed = 10;

    public bool isCollidingWall = false;

    private float timer = 0;

    public DammageManager dammageManager;

    public enum StateEnum
    {
        Stop, //0
        PrepareCharge, //3

        Charging, // 1

        isKO, //2
        KOForGood,
        Dead
    }

    public StateEnum State = StateEnum.Stop;
    public HorizontalDirection Direction = HorizontalDirection.Left;

    // Use this for initialization
    void Start () {
        Player = GameManager.Player.gameObject;
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        dammageManager = gameObject.AddComponent(typeof(DammageManager)) as DammageManager;

        dammageManager.actionOnContact = DammageManager.DammageManagerEnum.Nothing;
        dammageManager.pushDirectionMode = DammageManager.PushDirectionMode.PushDiretionDependOfPositionOfTheTwoObjects;

        dammageManager.pushForceX = 10;
        dammageManager.pushForceY = 10;
        dammageManager.timerKnockback = 0.5f;

        dammageManager.dammageOnHit = 1;
    }
	
	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 0;
        }


        switch (State)
        {
            case StateEnum.Stop:
                break;
            case StateEnum.PrepareCharge:
                break;
            case StateEnum.Charging:
                HandleCharging();
                break;
            case StateEnum.isKO:
                HandleIsKo();
                break;
            case StateEnum.KOForGood:
                break;
        }
    }
 
    private void HandleIsKo()
    {
        if (timer <= 0)
        {
            ChooseNextAction(StateEnum.isKO);
        }
    }

    public void ChooseNextAction(StateEnum lastState)
    {
        switch (lastState)
        {
            case StateEnum.isKO:
                if (Direction == HorizontalDirection.Right && isCollidingWall)
                {
                    PrepareCharge(HorizontalDirection.Left);
                }
                else if (Direction == HorizontalDirection.Left && isCollidingWall)
                {
                    PrepareCharge(HorizontalDirection.Right);
                }
                break;

            default:
                break;
        }

    }

    public void MakeKoForGood()
    {
        MakeKo();
        State = StateEnum.KOForGood;
    }
    public void MakeKo(int seconds)
    {
        MakeKo();
        timer = seconds;
    }
    private void MakeKo()
    {
        myRigidBody.velocity = (new Vector2(0, 0));

        myAnimator.SetInteger("State_Animation", 2);

        dammageManager.actionOnContact = DammageManager.DammageManagerEnum.Nothing;

        myRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        State = StateEnum.isKO;
    }



    public void PrepareCharge(HorizontalDirection direction)
    {
        Direction = direction;
        if (direction == HorizontalDirection.Left)
        {
            transform.localScale = new Vector3(System.Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-System.Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        dammageManager.actionOnContact = DammageManager.DammageManagerEnum.HitPlayer;

        myAnimator.SetInteger("State_Animation", 3);
        myRigidBody.velocity = (new Vector2(0, 0));

        //timer = seconds;

        State = StateEnum.PrepareCharge;
    }
    public void EndPrepareCharge() //Appelé par l'animator
    {
        Charge(Direction);
    }
    private void Charge(HorizontalDirection direction)
    {
        dammageManager.actionOnContact = DammageManager.DammageManagerEnum.HitPlayer;
        myAnimator.SetInteger("State_Animation", 1);

        if (direction == HorizontalDirection.Left)
        {
            transform.localScale = new Vector3(System.Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-System.Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

        Direction = direction;
        State = StateEnum.Charging;
    }
    private void HandleCharging()
    {
        if (Direction == HorizontalDirection.Right)
        {
            myRigidBody.velocity = (new Vector2(1 * Move_speed, myRigidBody.velocity.y));
        }
        else
        {
            myRigidBody.velocity = (new Vector2(-1 * Move_speed, myRigidBody.velocity.y));
        }
    }

    public void Stop(HorizontalDirection direction)
    {
        Direction = direction;
        if (direction == HorizontalDirection.Left)
        {
            transform.localScale = new Vector3(System.Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-System.Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        Stop();
    }
    public void Stop()
    {
        dammageManager.actionOnContact = DammageManager.DammageManagerEnum.Nothing;

        myAnimator.SetInteger("State_Animation", 0);
        myRigidBody.velocity = (new Vector2(0, 0));

        State = StateEnum.Stop;
    }

    public void Attack1()
    {
    }
    public void EndAttack1()
    {
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<MinautorWall>() != null)
        {
            print("Collide Wall");
            isCollidingWall = true;
            GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.2f, 0.3f);
            other.gameObject.GetComponent<MinautorWall>().Hit();
            if (other.gameObject.GetComponent<MinautorWall>().Health >= 1)
                MakeKo(3);
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<MinautorWall>() != null)
        {
            isCollidingWall = false;
        }
    }

}
