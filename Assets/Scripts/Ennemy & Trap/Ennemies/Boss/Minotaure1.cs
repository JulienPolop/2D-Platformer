using UnityEngine;
using System.Collections;

public class Minotaure1 : MonoBehaviour
{
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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
