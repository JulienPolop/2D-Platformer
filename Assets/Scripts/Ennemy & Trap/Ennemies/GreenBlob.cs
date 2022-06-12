/*/
* Script by Devin Curry
* www.Devination.com
* www.youtube.com/user/curryboy001
* Please like and subscribe if you found my tutorials helpful :D
* Google+ Community: https://plus.google.com/communities/108584850180626452949
* Twitter: https://twitter.com/Devination3D
* Facebook Page: https://www.facebook.com/unity3Dtutorialsbydevin
/*/
using UnityEngine;
using System.Collections;

public class GreenBlob : MonoBehaviour
{
    public LayerMask enemyMask;
    public float speed = 1;
    Rigidbody2D myBody;
    Transform myTrans;
    float myWidth, myHeight;
    public string State = "Normal";

    void Start()
    {
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        SpriteRenderer mySprite = this.GetComponent<SpriteRenderer>();
        myWidth = GetComponent<CircleCollider2D>().bounds.extents.x;
        myHeight = GetComponent<CircleCollider2D>().bounds.extents.y;
    }

    void FixedUpdate()
    {
        //NOTE: This script makes use of the .toVector2() extension method.
        //Be sure you have the following script in your project to avoid errors
        //http://www.devination.com/2015/07/unity-extension-method-tutorial.html

        //Use this position to cast the isGrounded/isBlocked lines from
        //Vector2 lineCastPos = myTrans.position.toVector2() - myTrans.right.toVector2() * myWidth + Vector2.up * myHeight;
        Vector2 lineCastPos = new Vector2 (myTrans.position.x, myTrans.position.y) - new Vector2(myTrans.right.x, myTrans.right.y) * myWidth + Vector2.up * myHeight;

        //Check to see if there's ground in front of us before moving forward
        //NOTE: Unity 4.6 and below use "- Vector2.up" instead of "+ Vector2.down"
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
        //Check to see if there's a wall in front of us before moving forward
        //Debug.DrawLine(lineCastPos, lineCastPos - myTrans.right.toVector2() * .05f);
        Debug.DrawLine(lineCastPos, lineCastPos - new Vector2(myTrans.right.x, myTrans.right.y) * .05f);
        //bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - myTrans.right.toVector2() * .05f, enemyMask);
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - new Vector2(myTrans.right.x, myTrans.right.y) * .05f, enemyMask);

        //If theres no ground, turn around. Or if I hit a wall, turn around
        if (!isGrounded || isBlocked)
        {
            Vector3 currRot = myTrans.eulerAngles;
            currRot.y += 180;
            myTrans.eulerAngles = currRot;
        }

        //Always move forward
        Vector2 myVel = myBody.velocity;
        myVel.x = -myTrans.right.x * speed;
        myBody.velocity = myVel;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Weapon"))
        {
            GameManager.MainCamera.GetComponent<CameraFollow>().Shake(0.1f, 0.25f);
            Die();
        }
    }

    public void Die()
    {
        GetComponent<Animator>().SetBool("IsDying", true);
        speed = 0;
        gameObject.layer = 15;
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}