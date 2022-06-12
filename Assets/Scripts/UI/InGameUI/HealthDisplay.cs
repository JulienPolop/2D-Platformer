using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour{

    public List<Image> heartsTab;

    public  Sprite FullHeartSprite;
    public  Sprite GreyHeartSprite;

    int PlayerHealth;
    int PlayerHealthMax;

    private void Start()
    {
        PlayerHealth = 0;
    }

    private void Update()
    {

    }

    public void setHealth(int ActualHealth, int maxHealth)
    {
        PlayerHealth = ActualHealth;

        for (int i = 0; i < ActualHealth; i++)
        {
            heartsTab[i].color = new Color(1, 1, 1, 1);
            heartsTab[i].sprite = FullHeartSprite;
        }
        for (int i = ActualHealth; i < maxHealth; i++)
        {
            heartsTab[i].color = new Color(1, 1, 1, 1);
            heartsTab[i].sprite = GreyHeartSprite;
        }
        for (int i = maxHealth; i < heartsTab.Count; i++)
        {
            heartsTab[i].color = new Color(1,1,1,0);
        }
    }
}

