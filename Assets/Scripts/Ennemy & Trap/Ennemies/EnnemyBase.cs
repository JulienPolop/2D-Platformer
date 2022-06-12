using UnityEngine;
using System.Collections;

public  class EnnemyBase : MonoBehaviour
{
    public enum StateEnum
    {
        Normal,
        Knockback,

    }
    public enum HorizontalStateEnum
    {
        Right,
        Left,
        Nothing,
    }
    public enum VerticalStateEnum
    {
        Top,
        Bottom,
        Nothing,
    }


    public StateEnum State = StateEnum.Normal;

    Player_Controller Player { get; set; }

    private Timer TimerKnockback { get; set; }


    // Use this for initialization
    void Start()
    {
        Player = GameManager.Player;

        TimerKnockback = gameObject.AddComponent<Timer>();
        TimerKnockback.SetUpTimer(0.3f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
