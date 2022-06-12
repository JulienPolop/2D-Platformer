using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Checkpoint : MonoBehaviour {

    private GameObject myGameObject;

    // Use this for initialization
    void Start()
    {
        myGameObject = gameObject;
    }

    // Update is called once per frame
    void Update () {
		
	}

    // When Collide
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            print("NEW CKPT " + myGameObject.name);
            other.GetComponent<Player_DeathManager>().currentCheckpoint = myGameObject;
        }
    }
}
