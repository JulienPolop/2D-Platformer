using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointDisplay : MonoBehaviour {

    public static int Score;
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        Score = 0;
    }

    private void Update()
    {
        text.text = "" + Score;
    }

    public static void setScore(int Player_Score)
    {
        Score = Player_Score;
    }
}

