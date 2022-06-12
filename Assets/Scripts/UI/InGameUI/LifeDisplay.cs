using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour {


    public Text text;
    static int playerLife;

    private void Start()
    {
        playerLife = 0;
    }

    private void Update()
    {
        text.text = "x " + playerLife;
    }

    public static void setLife(int ActualHealth)
    {
        playerLife = ActualHealth;
    }
}
