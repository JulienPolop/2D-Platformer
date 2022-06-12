using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Destroy_Trigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // When Collide
    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.tag.Equals("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}
