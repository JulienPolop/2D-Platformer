using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Points_Object : MonoBehaviour {

    public int pointsToAdd;
    public GameObject HitParticle;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
       if(other.gameObject.tag == "Player")
        {
            Instantiate(HitParticle, transform.position, transform.rotation);
            other.gameObject.GetComponent<Player_ScoreManager>().PlayerScore += pointsToAdd;
            Destroy(gameObject);
        }
    }
}
