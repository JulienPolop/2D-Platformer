using UnityEngine;

public class Scr_Minautor2 : MonoBehaviour {

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
        Pause,
        WaitingForCharge,
        Charging,
        goKO,
        isKO,
        KOForGood,
        Dead
    }
    public enum DirectionEnum
    {
        Droite,
        Gauche
    }

    public StateEnum State = StateEnum.Pause;
    public DirectionEnum Direction = DirectionEnum.Gauche;

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



        if (State == StateEnum.Pause)
        {

        }

        if (State == StateEnum.WaitingForCharge)
        {
            if (timer <= 0)
            {
                if (Direction == DirectionEnum.Droite && isCollidingWall)
                {
                    MakeCharging(DirectionEnum.Gauche);
                }
                else if (Direction == DirectionEnum.Gauche && isCollidingWall)
                {
                    MakeCharging(DirectionEnum.Droite);
                }
            }
        }

        if (State == StateEnum.Charging)
        {
            if (Direction == DirectionEnum.Droite)
            {
                myRigidBody.velocity = (new Vector2(1 * Move_speed, myRigidBody.velocity.y));
            }
            else
            {
                myRigidBody.velocity = (new Vector2(-1 * Move_speed, myRigidBody.velocity.y));
            }
        }


        if (State == StateEnum.isKO)
        {
            if (timer <= 0)
            {
                MakeWaitingForCharge(1);
            }
        }

        if (State == StateEnum.KOForGood)
        {

        }
    }

    private void MakeKo()
    {
        myRigidBody.velocity = (new Vector2(0, 0));

        myAnimator.SetInteger("State_Animation", 2);
        myAnimator.SetInteger("State_Animation", 3);

        dammageManager.actionOnContact = DammageManager.DammageManagerEnum.Nothing;

        myRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        State = StateEnum.isKO;
    }
    public void MakeKo(int seconds)
    {
        MakeKo();
        timer = seconds;
    }
    public void MakeKoForGood()
    {
        MakeKo();
        State = StateEnum.KOForGood;
    }

    public void MakeWaitingForCharge(int seconds)
    {
        dammageManager.actionOnContact = DammageManager.DammageManagerEnum.HitPlayer;

        myAnimator.SetInteger("State_Animation", 0);
        myRigidBody.velocity = (new Vector2(0, 0));

        timer = seconds;

        State = StateEnum.WaitingForCharge;
    }

    public void MakeCharging(DirectionEnum direction)
    {
        dammageManager.actionOnContact = DammageManager.DammageManagerEnum.HitPlayer;
        myAnimator.SetInteger("State_Animation", 1);

        if (direction == DirectionEnum.Gauche)
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

    public void MakePause(DirectionEnum direction)
    {
        Direction = direction;
        if (direction == DirectionEnum.Gauche)
        {
            transform.localScale = new Vector3(System.Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-System.Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        MakePause();
    }
    public void MakePause()
    {
        dammageManager.actionOnContact = DammageManager.DammageManagerEnum.Nothing;

        myAnimator.SetInteger("State_Animation", 0);
        myRigidBody.velocity = (new Vector2(0, 0));

        State = StateEnum.Pause;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<MinautorWall>() != false)
        {
            print("Collide Wall");
            isCollidingWall = true;
            GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.2f, 0.3f);
            other.gameObject.GetComponent<MinautorWall>().Hit();
            if (other.gameObject.GetComponent<MinautorWall>().Health >= 1)
                MakeKo(3);
        }
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Don't Climb"))
        {
            isCollidingWall = false;
        }
    }

}
