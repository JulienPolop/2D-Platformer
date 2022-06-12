using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using System;

public class MinautorIA_1 : MonoBehaviour
{
    public DammageManager dammageManager;
    private Rigidbody2D myRigidBody;
    public GameObject Player;
    private MinautorController MinautorController;

    public bool isCollidingWall = false;

    public State CurrentState;

    public enum StateType
    {
        Stop, //0
        PrepareCharge, //3

        Charging, // 1

        isKO, //2
        KOForGood,
        Dead
    }

    // Use this for initialization
    void Start()
    {
        Player = GameManager.Player.gameObject;
        myRigidBody = GetComponent<Rigidbody2D>();
        dammageManager = gameObject.AddComponent(typeof(DammageManager)) as DammageManager;
        MinautorController = GetComponent<MinautorController>();

        dammageManager.actionOnContact = DammageManager.DammageManagerEnum.Nothing;
        dammageManager.pushDirectionMode = DammageManager.PushDirectionMode.PushDiretionDependOfPositionOfTheTwoObjects;

        dammageManager.pushForceX = 10;
        dammageManager.pushForceY = 10;
        dammageManager.timerKnockback = 0.5f;

        dammageManager.dammageOnHit = 1;
    }

    void ChangeState(State newState)
    {
        CurrentState.Quit();
        CurrentState = newState;
        CurrentState.Start();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState.Update();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<MinautorWall>() != null)
        {
            print("Collide Wall");
            isCollidingWall = true;
            GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.2f, 0.3f);
            other.gameObject.GetComponent<MinautorWall>().Hit();
            //if (other.gameObject.GetComponent<MinautorWall>().Health >= 1)
                //MakeKo(3);
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<MinautorWall>() != null)
        {
            isCollidingWall = false;
        }
    }

    public class State
    {
        Action OnStartAction;
        Action OnUpdateAction;
        Action OnQuitAction;

        public State(Action onStartAction, Action onUpdateAction, Action onQuitAction)
        {
            OnStartAction = onStartAction;
            OnUpdateAction = onUpdateAction;
            OnQuitAction = onQuitAction;
        }

        public void Start()
        {
            OnStartAction.Invoke();
        }
        public void Update()
        {
            OnUpdateAction.Invoke();
        }
        public void Quit()
        {
            OnQuitAction.Invoke();
        }
    }
}



