using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ScoreManager : MonoBehaviour {

    public int BeginScore = 0;
    public int PlayerScore;

    // Use this for initialization
    void Start()
    {
        PlayerScore = BeginScore;

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerScore < 0)
            PlayerScore = 0;
        PointDisplay.setScore(PlayerScore);
    }
}
