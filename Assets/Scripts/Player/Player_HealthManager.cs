using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_HealthManager : MonoBehaviour {
    [Header("Health")]
    public int BeginHealth = 5;
    public int MaxHealth = 5;
    public int _playerHealth;
    public int PlayerHealth
    {
        get { return _playerHealth; }
        set
        {
            if (value < 0)
                _playerHealth = 0;
            else
                _playerHealth = value;
         }
    }

    [Header("Life")]
    public int BeginLife = 1;
    public int PlayerLife;

    [Header("Food")]
    public int MaxFood = 3;
    public int CurrentFood = 3;


    // Use this for initialization
    void Start () {
        PlayerHealth = BeginHealth;
        PlayerLife = BeginLife;
        CurrentFood = MaxFood;
    }
	
	// Update is called once per frame
	void Update () {
        FindObjectOfType<HealthDisplay>().setHealth(PlayerHealth, MaxHealth);
        LifeDisplay.setLife(PlayerLife);
        FoodDisplay.setFood(CurrentFood, MaxFood);
    }

    public void FullHeal()
    {
        PlayerHealth = MaxHealth;
        PlayerLife = BeginLife;
        CurrentFood = MaxFood;

        Instantiate(GetComponent<Player_DeathManager>().respawnParticle, transform.position, transform.rotation);
    }

    public void HealFromFood()
    {
        if (CurrentFood > 0 && PlayerHealth < MaxHealth)
        {
            Instantiate(GetComponent<Player_DeathManager>().respawnParticle, transform.position, transform.rotation);
            CurrentFood--;
            PlayerHealth++;
        }
    }

    public bool GetFood()
    {
        if (CurrentFood >= MaxFood)
            return false;
        else
        {
            CurrentFood++;
            return true;
        }
    }
}
