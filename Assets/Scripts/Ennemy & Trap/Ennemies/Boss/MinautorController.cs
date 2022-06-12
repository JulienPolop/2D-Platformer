using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class MinautorController : MonoBehaviour
{

    public int Life;
    public int Dammage;

    public float MarchSpeed;
    public float ChargeSpeed;
    public HorizontalDirection LookDirection;
    public HorizontalDirection MoveDirection;

    private Rigidbody2D myRigidBody;
    private Animator myAnimator;

    private bool animationEnd { get; set; }

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }


    public void March(HorizontalDirection moveDirection)
    {
        myAnimator.SetInteger("State_Animation", 1);
        ChangeLookDirection(moveDirection);
        switch (moveDirection)
        {
            case HorizontalDirection.Right:
                GetComponent<Rigidbody2D>().velocity = new Vector2(MarchSpeed, GetComponent<Rigidbody2D>().velocity.y);
                break;
            case HorizontalDirection.Left:
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MarchSpeed, GetComponent<Rigidbody2D>().velocity.y);
                break;
        }
    }

    public void PrepareCharge(HorizontalDirection direction)
    {
        myAnimator.SetInteger("State_Animation", 3);
        ChangeLookDirection(direction);
        MoveDirection = direction;

        myRigidBody.velocity = (new Vector2(0, 0));

        //dammageManager.actionOnContact = DammageManager.DammageManagerEnum.HitPlayer;


        //myAnimator.

        //timer = seconds;

        //State = StateEnum.PrepareCharge;
    }
    public void Charge(HorizontalDirection moveDirection)
    {
        myAnimator.SetInteger("State_Animation", 1);
        ChangeLookDirection(moveDirection);
        switch (moveDirection)
        {
            case HorizontalDirection.Right:
                GetComponent<Rigidbody2D>().velocity = new Vector2(ChargeSpeed, GetComponent<Rigidbody2D>().velocity.y);
                break;
            case HorizontalDirection.Left:
                GetComponent<Rigidbody2D>().velocity = new Vector2(-ChargeSpeed, GetComponent<Rigidbody2D>().velocity.y);
                break;
        }
    }

    private void FallKo()
    {
        myRigidBody.velocity = (new Vector2(0, 0));

        myAnimator.SetInteger("State_Animation", 2);

        //dammageManager.actionOnContact = DammageManager.DammageManagerEnum.Nothing;

        myRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        //State = StateEnum.isKO;
    }

    public void ChangeLookDirection(HorizontalDirection lookDirection)
    {
        switch (lookDirection)
        {
            case HorizontalDirection.Right:
                transform.localScale = new Vector3(System.Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case HorizontalDirection.Left:
                transform.localScale = new Vector3(-System.Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
        }
        LookDirection = lookDirection;
    }

    public void Stop(HorizontalDirection direction)
    {
        ChangeLookDirection(direction);
        Stop();
    }
    public void Stop()
    {
        myAnimator.SetInteger("State_Animation", 0);
        myRigidBody.velocity = (new Vector2(0, 0));
    }

    public void Attack1(HorizontalDirection direction)
    {

    }
    public void Attack2(HorizontalDirection direction)
    {

    }
    public void Attack3(HorizontalDirection direction)
    {

    }
    public void Attack4(HorizontalDirection direction)
    {

    }

    public IEnumerator WaitEndOfAnimation() //Bloque la coroutine jusqu'a ce que l'animation se termine, faire une boucle si l'on veux que l'anim se joue plusieurs fois
    {
        animationEnd = false;
        yield return new WaitUntil( () => animationEnd);
        animationEnd = false;
    }

    public void OnEndAnimation() //Appelé par l'animator à chaque fin d'animation
    {
        animationEnd = true;
    }

    private void OnValidate()
    {
        ChangeLookDirection(LookDirection);
    }
}
