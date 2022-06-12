using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Moving_Platform : MonoBehaviour, ISpecialGround

{
    public enum MovingPlatformDeplacementMode {
        Loop,
        RoundTrip
    }

    //public GameObject platform;
    public float moveSpeed;
    public Vector2 velocity;
    [Header("Points")]
    public Transform currentPoint;
    public Transform[] points;
    public int pointSelection;

    public MovingPlatformDeplacementMode deplacementMode;
    private bool isReturning;

    // Use this for initialization
    void Start()
    {
        currentPoint = points[pointSelection];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (deplacementMode == MovingPlatformDeplacementMode.Loop)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, Time.deltaTime * moveSpeed);
            Move();

            if (transform.position == currentPoint.position)
            {
                pointSelection++;
                if (pointSelection == points.Length)
                {
                    pointSelection = 0;
                }
                currentPoint = points[pointSelection];
                
            }
        }
        if (deplacementMode == MovingPlatformDeplacementMode.RoundTrip)
        {
            if (!isReturning)
            {
                Move();

                if (transform.position == currentPoint.position)
                {
                    pointSelection++;
                    //print(pointSelection);
                    if (pointSelection == points.Length - 1)
                    {
                        isReturning = true;
                    }
                    currentPoint = points[pointSelection];
                }
            }
            if (isReturning)
            {
                Move();
                if (transform.position == currentPoint.position)
                {
                    pointSelection--;
                    //print(pointSelection);
                    if (pointSelection == 0)
                    {
                        isReturning = false;
                    }
                    currentPoint = points[pointSelection];
                }
            }


        }


    }

    private void Move()
    {
        Vector3 NextPosition = Vector3.MoveTowards(transform.position, currentPoint.transform.position, Time.deltaTime * moveSpeed);
        velocity = new Vector2(NextPosition.x - transform.position.x, NextPosition.y - transform.position.y)/ Time.deltaTime;

        transform.position = NextPosition;
    }




    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length -1; i++)
        {
            if (points[i] != null && points[i+1] != null)
            {
                Gizmos.DrawLine(new Vector2(points[i].position.x, points[i].position.y), new Vector2(points[i + 1].position.x, points[i + 1].position.y));
            }
        }
        if (deplacementMode == MovingPlatformDeplacementMode.Loop && points[0] != null && points[points.Length - 1] != null && points.Length > 2)
            Gizmos.DrawLine(new Vector2(points[0].position.x, points[0].position.y), new Vector2(points[points.Length - 1].position.x, points[points.Length - 1].position.y));
    }

    public void GroundInterraction(Player_Controller Player)
    {
        Player.ResetJump();
        Player.transform.parent = this.transform;
    }
}

