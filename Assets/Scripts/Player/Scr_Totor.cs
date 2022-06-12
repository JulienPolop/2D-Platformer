using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Totor : MonoBehaviour {
	public float Move_speed;
    public float Force_Jump;
    public float Velocityy;
    private string State = "Normal";

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    public int Begin_Health = 5;
    public int Player_Health;

    public bool Collide_gauche;
    public bool Collide_droit;
    public bool Collide_haut;
    public bool Collide_bas;



    // Use this for initialization
    void Start () {
        Player_Health = Begin_Health;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (State.Equals("Normal"))
        {
            Move();
            CheckJump();
            RenderSprite();
        }
        if (State.Equals("Jump"))
        {
            Move();
            RenderSprite();
            if (myRigidbody.velocity.y > 300)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 300);
            }
        }
        Velocityy = myRigidbody.velocity.y;
        FindObjectOfType<HealthDisplay>().setHealth(Player_Health, Begin_Health);
    }

	void Move()
	{
		//Ecoute les Inputs
		var mouvement_horizontal = Input.GetAxisRaw("Horizontal");
		//Bouge le player
		//Vector2 Mouvement = new Vector2(mouvement_horizontal, 0);
		//transform.Translate(Mouvement * Move_speed * Time.deltaTime);

        myRigidbody.velocity = new Vector2(mouvement_horizontal * Move_speed, myRigidbody.velocity.y);
    }

    void CheckJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            //Debug.Log("Je Saute");
            //myRigidbody.AddForce(Vector2.up * Force_Jump);
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Force_Jump);
            State = "Jump";
        }
    }

    void RenderSprite()
    {
        if (State.Equals("Normal"))
        {
            //Si On est arreté
            if (myRigidbody.velocity.x == 0)
            {
                myAnimator.SetBool("IsRunning", false);
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                myAnimator.SetBool("IsRunning", true);
            }
        }
        else if(State.Equals("Jump"))
        { 
            myAnimator.SetBool("IsRunning", true);
        }

        //SI on vas vers la gauche ou la droite, il faut inverser le Sprite
       if (myRigidbody.velocity.x > 0)
        {
            myAnimator.SetFloat("MoveX", 1);
        }
        if (myRigidbody.velocity.x < 0)
        {
            myAnimator.SetFloat("MoveX", -1);
        }
    }

    // When Collide
    void OnCollisionEnter2D(Collision2D other)
    {
        bool Collide_droit = false;
        bool Collide_gauche = false;
        bool Collide_haut = false;
        bool Collide_bas = false;
        bool Collide_side = false;
        bool Collide_Diagonal = false;

        if (other.contacts.Length >= 2)
        {
            //A plat
            if (other.contacts[0].point.x == other.contacts[1].point.x)
            {
                // A droite
                Collide_droit = (other.contacts[0].point.x > transform.position.x);
                // A Gauche
                Collide_gauche = (other.contacts[0].point.x < transform.position.x);
            }
            if (other.contacts[0].point.y == other.contacts[1].point.y)
            {
                // Haut
                Collide_haut = (other.contacts[0].point.y > transform.position.y);
                //Bas
                Collide_bas = (other.contacts[0].point.y < transform.position.y);
            }
        }
        Collide_side = Collide_droit || Collide_gauche;
        Collide_Diagonal = (other.contacts.Length == 1);



        if (other.gameObject.tag.Equals("Don't Climb"))
        {
            if (Collide_bas)
            {
                print("Dont't Climb");
                State = "Normal";
            }
        }
        else if (other.gameObject.tag.Equals("Moving Platform"))
        {
            if (Collide_side || Collide_bas)
            {
                transform.parent = other.transform;
                State = "Normal";
            }
        }
        else
        {
            if (Collide_side || Collide_bas || Collide_Diagonal)
            {
                State = "Normal";
            }
        }       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        transform.parent = null;
    }
}
